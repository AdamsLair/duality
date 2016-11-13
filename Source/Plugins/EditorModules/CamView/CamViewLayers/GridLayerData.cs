using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.Drawing;
using Duality.Resources;

using Duality.Editor.Plugins.CamView.CamViewStates;

namespace Duality.Editor.Plugins.CamView.CamViewLayers
{
	/// <summary>
	/// Defines how the <see cref="GridCamViewLayer"/> is displayed to the user.
	/// </summary>
	public struct GridLayerData
	{
		/// <summary>
		/// The base size of the grid in horizontal and vertical direction.
		/// </summary>
		public Vector2 GridBaseSize;
		/// <summary>
		/// The displayed cursor position on the grid.
		/// </summary>
		public Vector3 DisplayedGridPos;
	}
}
