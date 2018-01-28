using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Resources;

using OpenTK.Graphics.OpenGL;

namespace Duality.Backend.DefaultOpenTK
{
	[DontSerialize]
	public class NativeShaderProgram : INativeShaderProgram
	{
		private	static NativeShaderProgram curBound = null;
		public static void Bind(NativeShaderProgram prog)
		{
			if (curBound == prog) return;

			if (prog == null)
			{
				GL.UseProgram(0);
				curBound = null;
			}
			else
			{
				GL.UseProgram(prog.Handle);
				curBound = prog;
			}
		}
		public static void SetUniform(ShaderFieldInfo field, int location, float[] data)
		{
			if (field.Scope != ShaderFieldScope.Uniform) return;
			if (location == -1) return;
			switch (field.Type)
			{
				case ShaderFieldType.Bool:
				case ShaderFieldType.Int:
					int[] arrI = new int[field.ArrayLength];
					for (int j = 0; j < arrI.Length; j++) arrI[j] = (int)data[j];
					GL.Uniform1(location, arrI.Length, arrI);
					break;
				case ShaderFieldType.Float:
					GL.Uniform1(location, data.Length, data);
					break;
				case ShaderFieldType.Vec2:
					GL.Uniform2(location, data.Length / 2, data);
					break;
				case ShaderFieldType.Vec3:
					GL.Uniform3(location, data.Length / 3, data);
					break;
				case ShaderFieldType.Vec4:
					GL.Uniform4(location, data.Length / 4, data);
					break;
				case ShaderFieldType.Mat2:
					GL.UniformMatrix2(location, data.Length / 4, false, data);
					break;
				case ShaderFieldType.Mat3:
					GL.UniformMatrix3(location, data.Length / 9, false, data);
					break;
				case ShaderFieldType.Mat4:
					GL.UniformMatrix4(location, data.Length / 16, false, data);
					break;
			}
		}

		private int handle;
		private ShaderFieldInfo[] fields;
		private int[] fieldLocations;

		public int Handle
		{
			get { return this.handle; }
		}
		public ShaderFieldInfo[] Fields
		{
			get { return this.fields; }
		}
		public int[] FieldLocations
		{
			get { return this.fieldLocations; }
		}

		void INativeShaderProgram.LoadProgram(IEnumerable<INativeShaderPart> shaderParts)
		{
			DefaultOpenTKBackendPlugin.GuardSingleThreadState();

			// Verify that we have exactly one shader part for each stage.
			// Other scenarios are valid in desktop GL, but not GL ES, so 
			// we'll enforce the stricter rules manually to ease portability.
			int vertexCount = 0;
			int fragmentCount = 0;
			foreach (INativeShaderPart part in shaderParts)
			{
				Resources.ShaderType type = (part as NativeShaderPart).Type;
				if (type == Resources.ShaderType.Fragment)
					fragmentCount++;
				else if (type == Resources.ShaderType.Vertex)
					vertexCount++;
			}
			if (vertexCount == 0) throw new ArgumentException("Cannot load program without vertex shader.");
			if (fragmentCount == 0) throw new ArgumentException("Cannot load program without fragment shader.");
			if (vertexCount > 1) throw new ArgumentException("Cannot attach multiple vertex shaders to the same program.");
			if (fragmentCount > 1) throw new ArgumentException("Cannot attach multiple fragment shaders to the same program.");

			// Create or reset GL program
			if (this.handle == 0) 
				this.handle = GL.CreateProgram();
			else
				this.DetachShaders();

			// Attach all individual shaders to the program
			foreach (INativeShaderPart part in shaderParts)
			{
				GL.AttachShader(this.handle, (part as NativeShaderPart).Handle);
			}

			// Link the shader program
			GL.LinkProgram(this.handle);

			int result;
			GL.GetProgram(this.handle, GetProgramParameterName.LinkStatus, out result);
			if (result == 0)
			{
				string errorLog = GL.GetProgramInfoLog(this.handle);
				this.RollbackAtFault();
				throw new BackendException(string.Format("Linker error:{1}{0}", errorLog, Environment.NewLine));
			}

			// Collect variable infos from sub programs
			Dictionary<string, ShaderFieldInfo> fieldMap = new Dictionary<string, ShaderFieldInfo>();
			foreach (INativeShaderPart item in shaderParts)
			{
				NativeShaderPart shaderPart = item as NativeShaderPart;
				foreach (ShaderFieldInfo field in shaderPart.Fields)
				{
					fieldMap[field.Name] = field;
				}
			}

			// Collect variables that are available through shader reflection, e.g.
			// haven't been optimized of #ifdef'd away.
			List<int> validLocations = new List<int>();
			List<ShaderFieldInfo> validFields = new List<ShaderFieldInfo>();
			foreach (ShaderFieldInfo field in fieldMap.Values)
			{
				int location;
				if (field.Scope == ShaderFieldScope.Uniform)
					location = GL.GetUniformLocation(this.handle, field.Name);
				else
					location = GL.GetAttribLocation(this.handle, field.Name);

				if (location >= 0)
				{
					validLocations.Add(location);
					validFields.Add(field);
				}
			}

			this.fields = validFields.ToArray();
			this.fieldLocations = validLocations.ToArray();
		}
		ShaderFieldInfo[] INativeShaderProgram.GetFields()
		{
			return this.fields;
		}
		void IDisposable.Dispose()
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated)
				return;

			this.DeleteProgram();
		}

		/// <summary>
		/// Given a vertex element declaration, this method selects, which of the shaders
		/// attribute fields best matches it, and returns the <see cref="Fields"/> index.
		/// Returns -1, if no match was found.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public int SelectField(ref VertexElement element)
		{
			// Check for fields matching the elements preferred name
			for (int i = 0; i < this.fields.Length; i++)
			{
				// Skip invalid and non-attribute fields
				if (this.fieldLocations[i] == -1) continue;
				if (this.fields[i].Scope != ShaderFieldScope.Attribute) continue;

				// We do not check for type or data length matches. When matching
				// explicitly, this is the users responsibility.
				if (this.fields[i].Name == element.FieldName)
					return i;
			}
			return -1;
		}

		private void DeleteProgram()
		{
			if (this.handle == 0) return;

			this.DetachShaders();
			GL.DeleteProgram(this.handle);
			this.handle = 0;
		}
		private void DetachShaders()
		{
			// Determine currently attached shaders
			int[] attachedShaders = new int[10];
			int attachCount = 0;
			GL.GetAttachedShaders(this.handle, attachedShaders.Length, out attachCount, attachedShaders);

			// Detach all attached shaders
			for (int i = 0; i < attachCount; i++)
			{
				GL.DetachShader(this.handle, attachedShaders[i]);
			}
		}
		/// <summary>
		/// In case of errors loading the program, this methods rolls back the state of this
		/// shader program, so consistency can be assured.
		/// </summary>
		private void RollbackAtFault()
		{
			this.fields = new ShaderFieldInfo[0];
			this.fieldLocations = new int[0];

			this.DeleteProgram();
		}
	}
}
