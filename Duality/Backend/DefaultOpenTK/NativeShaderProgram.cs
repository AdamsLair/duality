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
		public static void Bind(ContentRef<Duality.Resources.ShaderProgram> target)
		{
			Duality.Resources.ShaderProgram targetRes = target.Res;
			if (targetRes != null && !targetRes.Compiled)
				targetRes.Compile();

			Bind((targetRes != null ? targetRes.Native : null) as NativeShaderProgram);
		}
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
		public static void SetUniform(ref ShaderFieldInfo field, float[] data)
		{
			if (field.Scope != ShaderFieldScope.Uniform) return;
			if (field.Handle == -1) return;
			switch (field.Type)
			{
				case ShaderFieldType.Int:
					int[] arrI = new int[field.ArrayLength];
					for (int j = 0; j < arrI.Length; j++) arrI[j] = (int)data[j];
					GL.Uniform1(field.Handle, arrI.Length, arrI);
					break;
				case ShaderFieldType.Float:
					GL.Uniform1(field.Handle, data.Length, data);
					break;
				case ShaderFieldType.Vec2:
					GL.Uniform2(field.Handle, data.Length / 2, data);
					break;
				case ShaderFieldType.Vec3:
					GL.Uniform3(field.Handle, data.Length / 3, data);
					break;
				case ShaderFieldType.Vec4:
					GL.Uniform4(field.Handle, data.Length / 4, data);
					break;
				case ShaderFieldType.Mat2:
					GL.UniformMatrix2(field.Handle, data.Length / 4, false, data);
					break;
				case ShaderFieldType.Mat3:
					GL.UniformMatrix3(field.Handle, data.Length / 9, false, data);
					break;
				case ShaderFieldType.Mat4:
					GL.UniformMatrix4(field.Handle, data.Length / 16, false, data);
					break;
			}
		}

		private int handle;
		public int Handle
		{
			get { return this.handle; }
		}

		void INativeShaderProgram.LoadProgram(INativeShaderPart vertex, INativeShaderPart fragment)
		{
			DualityApp.GuardSingleThreadState();

			if (this.handle == 0) 
				this.handle = GL.CreateProgram();
			else
				this.DetachShaders();
			
			// Attach both shaders
			if (vertex != null)
				GL.AttachShader(this.handle, (vertex as NativeShaderPart).Handle);
			if (fragment != null)
				GL.AttachShader(this.handle, (fragment as NativeShaderPart).Handle);

			// Link the shader program
			GL.LinkProgram(this.handle);

			int result;
			GL.GetProgram(this.handle, GetProgramParameterName.LinkStatus, out result);
			if (result == 0)
			{
				string infoLog = GL.GetProgramInfoLog(this.handle);
				Log.Core.WriteError("Error linking shader program. InfoLog:{1}{0}", infoLog, Environment.NewLine);
				return;
			}
		}
		void INativeShaderProgram.GetFieldLocations(ShaderFieldInfo[] fields)
		{
			for (int i = 0; i < fields.Length; i++)
			{
				if (fields[i].Scope == ShaderFieldScope.Uniform)
					fields[i] = fields[i].WithHandle(GL.GetUniformLocation(this.handle, fields[i].Name));
				else
					fields[i] = fields[i].WithHandle(GL.GetAttribLocation(this.handle, fields[i].Name));
			}
		}
		void IDisposable.Dispose()
		{
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated &&
				this.handle != 0)
			{
				this.DetachShaders();
				GL.DeleteProgram(this.handle);
				this.handle = 0;
			}
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
	}
}
