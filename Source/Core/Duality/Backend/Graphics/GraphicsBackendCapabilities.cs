using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality.Drawing;

namespace Duality.Backend
{
	/// <summary>
	/// Provides information about the graphics / rendering capabilities of an <see cref="IGraphicsBackend"/>.
	/// </summary>
	public class GraphicsBackendCapabilities
	{
		protected Version apiVersion = new Version(0, 0);

		/// <summary>
		/// The API version of the backends underlying graphics API, such as the available 
		/// OpenGL version, DirectX version or similar.
		/// </summary>
		public Version ApiVersion
		{
			get { return this.apiVersion; }
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
