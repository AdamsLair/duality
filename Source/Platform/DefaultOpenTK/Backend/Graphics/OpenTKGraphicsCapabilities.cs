using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Duality.Drawing;
using Duality.Resources;
using System.Text;

namespace Duality.Backend.DefaultOpenTK
{
	/// <summary>
	/// Provides information about the graphics capabilities of an OpenTK <see cref="GraphicsBackend"/>.
	/// </summary>
	public class OpenTKGraphicsCapabilities : GraphicsBackendCapabilities
	{
		protected static readonly string UndefinedCapsString = "Unknown";

		protected Version glVersion;
		protected Version glslVersion;
		protected string glVersionString;
		protected string glslVersionString;
		protected string glVendor;
		protected string glRenderer;


		/// <summary>
		/// The OpenGL version that is used.
		/// </summary>
		public Version GLVersion
		{
			get { return this.glVersion; }
		}
		/// <summary>
		/// The GLSL version that is used.
		/// </summary>
		public Version GLSLVersion
		{
			get { return this.glslVersion; }
		}
		/// <summary>
		/// The name of the vendor that provided the OpenGL / driver implementation.
		/// </summary>
		public string GLVendor
		{
			get { return this.glVendor; }
		}
		/// <summary>
		/// The name of the rendering hardware that is used by the currently active OpenGL context.
		/// </summary>
		public string GLRenderer
		{
			get { return this.glRenderer; }
		}


		/// <summary>
		/// Resets all capability values back to their default / undefined state.
		/// </summary>
		protected override void Reset()
		{
			base.Reset();
			this.glVersion = new Version(0, 0);
			this.glslVersion = new Version(0, 0);
			this.glVersionString = UndefinedCapsString;
			this.glslVersionString = UndefinedCapsString;
			this.glVendor = UndefinedCapsString;
			this.glRenderer = UndefinedCapsString;
		}
		/// <summary>
		/// Retrieves all capability values from the currently active graphics context.
		/// </summary>
		public void RetrieveFromAPI()
		{
			// Reset all values to default / undefined so we'll end up with only the values we successfully retrieved
			this.Reset();

			// Accessing OpenGL functionality requires context. Don't get confused by AccessViolationExceptions, fail better instead.
			GraphicsContext.Assert();

			// Retrieve raw values from OpenGL
			try
			{
				GraphicsBackend.CheckOpenGLErrors();
				this.glVersionString = GL.GetString(StringName.Version);
				this.glVendor = GL.GetString(StringName.Vendor);
				this.glRenderer = GL.GetString(StringName.Renderer);
				this.glslVersionString = GL.GetString(StringName.ShadingLanguageVersion);
				GraphicsBackend.CheckOpenGLErrors();
			}
			catch (Exception e)
			{
				Logs.Core.WriteWarning(
					"Can't determine OpenGL base specs: {0}", 
					LogFormat.Exception(e));
			}

			// Parse raw values such as version strings for higher level info
			this.TryParseVersionString(this.glVersionString, out this.glVersion);
			this.TryParseVersionString(this.glslVersionString, out this.glslVersion);

			// Translate some of the GL-specific attributes to the generalized base class ones
			this.apiVersion = this.glslVersion;
		}
		/// <summary>
		/// Appends all relevant capability values to the specified log message.
		/// </summary>
		/// <param name="messageBuilder"></param>
		protected override void AppendToLogMessage(StringBuilder messageBuilder)
		{
			base.AppendToLogMessage(messageBuilder);

			messageBuilder.AppendFormat(
				"OpenGL Version: '{0}', i.e. {1}", 
				this.glVersionString,
				this.glVersion);
			messageBuilder.AppendLine();

			messageBuilder.AppendFormat(
				"Vendor: {0}",
				this.glVendor);
			messageBuilder.AppendLine();

			messageBuilder.AppendFormat(
				"Renderer: {0}",
				this.glRenderer);
			messageBuilder.AppendLine();

			messageBuilder.AppendFormat(
				"GLSL Version: '{0}', i.e. {1}",
				this.glslVersionString,
				this.glslVersion);
			messageBuilder.AppendLine();
		}

		/// <summary>
		/// Attempts to parse the specified version string into an actual version number.
		/// </summary>
		/// <param name="versionString"></param>
		/// <param name="version"></param>
		/// <returns></returns>
		protected bool TryParseVersionString(string versionString, out Version version)
		{
			version = new Version();
			if (string.IsNullOrEmpty(versionString)) return false;

			string[] token = versionString.Split(' ');
			for (int i = 0; i < token.Length; i++)
			{
				if (Version.TryParse(token[i], out version))
					return true;
			}

			return false;
		}
	}
}
