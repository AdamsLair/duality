using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Duality;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps.CamViewStates
{
	/// <summary>
	/// Base class for user-specified <see cref="TilemapTool"/> settings.
	/// </summary>
	public class TilemapToolSettings
	{
		private AutoTilePaintMode autoTileMode = AutoTilePaintMode.Full;

		public AutoTilePaintMode UseAutoTiling
		{
			get { return this.autoTileMode; }
			set { this.autoTileMode = value; }
		}
	}
}
