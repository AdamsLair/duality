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
					fields[i].Handle = GL.GetUniformLocation(this.handle, fields[i].Name);
				else
					fields[i].Handle = GL.GetAttribLocation(this.handle, fields[i].Name);
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
