using System;

using Duality.Drawing;
using Duality.Resources;

namespace Duality
{
	/// <summary>
	/// Event arguments for drawcall collection events.
	/// </summary>
	public class CollectDrawcallEventArgs : EventArgs
	{
		private string renderStepId;
		private IDrawDevice device;

		/// <summary>
		/// [GET] An identifier that represents the rendering step that triggered this drawcall collection.
		/// </summary>
		public string RenderStepId
		{
			get { return this.renderStepId; }
		}
		/// <summary>
		/// [GET] The drawing device that is used for collecting additional drawcalls.
		/// </summary>
		public IDrawDevice Device
		{
			get { return this.device; }
		}

		public CollectDrawcallEventArgs(string renderStepId, IDrawDevice device)
		{
			this.renderStepId = renderStepId;
			this.device = device;
		}
	}
}
