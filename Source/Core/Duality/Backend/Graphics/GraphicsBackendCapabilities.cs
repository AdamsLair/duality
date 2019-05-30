using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality.Resources;

namespace Duality.Backend
{
	/// <summary>
	/// Provides information about the graphics / rendering capabilities of an <see cref="IGraphicsBackend"/>.
	/// </summary>
	public class GraphicsBackendCapabilities
	{
		protected Version apiVersion;
		protected int maxTextureSize;
		protected int maxTextureBindings;
		protected int maxRenderTargetSize;

		/// <summary>
		/// The API version of the backends underlying graphics API, such as the available 
		/// OpenGL version, DirectX version or similar.
		/// </summary>
		public Version ApiVersion
		{
			get { return this.apiVersion; }
		}
		/// <summary>
		/// The maximum edge length of a <see cref="Texture"/>.
		/// </summary>
		public int MaxTextureSize
		{
			get { return this.maxTextureSize; }
		}
		/// <summary>
		/// The maximum number of textures that can be bound simultaneously for shader access.
		/// </summary>
		public int MaxTextureBindings
		{
			get { return this.maxTextureBindings; }
		}
		/// <summary>
		/// The maximum edge length of a <see cref="RenderTarget"/>.
		/// </summary>
		public int MaxRenderTargetSize
		{
			get { return this.maxRenderTargetSize; }
		}


		public GraphicsBackendCapabilities()
		{
			this.Reset();
		}
		/// <summary>
		/// Resets all capability values back to their default / undefined state.
		/// </summary>
		protected virtual void Reset()
		{
			this.apiVersion = new Version(0, 0);
			this.maxTextureSize = 64;
			this.maxTextureBindings = 8;
			this.maxRenderTargetSize = 1;
		}
		/// <summary>
		/// Appends all relevant capability values to the specified log message.
		/// </summary>
		/// <param name="messageBuilder"></param>
		protected virtual void AppendToLogMessage(StringBuilder messageBuilder)
		{
			messageBuilder.AppendFormat(
				"API Version: {0}",
				this.apiVersion);
			messageBuilder.AppendLine();

			messageBuilder.AppendFormat(
				"Max Texture Size: {0}",
				this.maxTextureSize);
			messageBuilder.AppendLine();

			messageBuilder.AppendFormat(
				"Max Texture Bindings: {0}",
				this.maxTextureBindings);
			messageBuilder.AppendLine();

			messageBuilder.AppendFormat(
				"Max RenderTarget Size: {0}",
				this.maxRenderTargetSize);
			messageBuilder.AppendLine();
		}

		/// <summary>
		/// Writes all capability data to the specified <see cref="Log"/> for
		/// diagnostic purposes.
		/// </summary>
		/// <param name="log"></param>
		public void WriteToLog(Log log)
		{
			StringBuilder messageBuilder = new StringBuilder();
			messageBuilder.AppendLine("Graphics backend capabilities:");

			this.AppendToLogMessage(messageBuilder);

			// Add indentation after first line
			messageBuilder.Replace(Environment.NewLine, Environment.NewLine + "  ");
			log.Write(messageBuilder.ToString().Trim());
		}
	}
}
