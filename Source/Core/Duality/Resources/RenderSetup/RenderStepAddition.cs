using System;
using System.Linq;

using Duality.Editor;
using Duality.Drawing;
using Duality.Properties;
using Duality.Backend;


namespace Duality.Resources
{
	/// <summary>
	/// Anchors an additional rendering step inside a predefined sequence of rendering steps.
	/// </summary>
	public struct RenderStepAddition
	{
		/// <summary>
		/// Id of the existing rendering step to which the new step will be anchored.
		/// </summary>
		public string AnchorStepId;
		/// <summary>
		/// Position of the new rendering step relative to the one it is anchored to.
		/// </summary>
		public RenderStepPosition AnchorPosition;
		/// <summary>
		/// The new rendering step that should be inserted into the rendering step sequence.
		/// </summary>
		public RenderStep AddedRenderStep;

		public override string ToString()
		{
			if (this.AnchorPosition == RenderStepPosition.First ||
				this.AnchorPosition == RenderStepPosition.Last)
				return string.Format("{0}: {1}", this.AnchorPosition, this.AddedRenderStep);
			else
				return string.Format("{0} {1}: {2}", this.AnchorPosition, this.AnchorStepId, this.AddedRenderStep);
		}
	}
}
