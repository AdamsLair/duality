using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.CamView;
using Duality.Editor.Plugins.Tilemaps.CamViewStates;
using Duality.Editor.Plugins.Tilemaps.Properties;


namespace Duality.Editor.Plugins.Tilemaps.EditorActions
{
	/// <summary>
	/// Opens a Camera View in Tilemap Editor mode and selects the Tilemap for editing.
	/// </summary>
	public class OpenTilemapEditor : EditorSingleAction<object>
	{
		public override string Name
		{
			get { return TilemapsRes.ActionName_OpenTilemapEditor; }
		}
		public override string Description
		{
			get { return TilemapsRes.ActionDesc_OpenTilemapEditor; }
		}
		public override Image Icon
		{
			get { return typeof(Tilemap).GetEditorImage(); }
		}
		public override int Priority
		{
			get { return base.Priority + 1; }
		}

		public override bool CanPerformOn(object obj)
		{
			if (!base.CanPerformOn(obj)) return false;
			if (GetTilemap(obj) == null) return false;
			if (DualityEditorApp.GetPlugin<CamViewPlugin>() == null) return false;
			return true;
		}
		public override void Perform(object obj)
		{
			Tilemap tilemap = GetTilemap(obj);
			if (tilemap == null) return;

			// Find or create a camera view in tilemap editing mode
			CamViewPlugin camViewPlugin = DualityEditorApp.GetPlugin<CamViewPlugin>();
			if (camViewPlugin == null) return;
			CamView.CamView camView = camViewPlugin.CreateOrSwitchCamView(typeof(TilemapEditorCamViewState));
			if (camViewPlugin == null) return;

			// Select the tilemap in question. The tilemap editor will react.
			DualityEditorApp.Select(this, new ObjectSelection(tilemap));

			// See if we can find a renderer that uses the tilemap and focus on it.
			TilemapRenderer renderer = Scene.Current
				.FindComponents<TilemapRenderer>()
				.FirstOrDefault(r => r.ActiveTilemap == tilemap);
			if (renderer != null)
				DualityEditorApp.Highlight(this, new ObjectSelection(renderer), HighlightMode.Spatial);
		}
		public override bool MatchesContext(string context)
		{
			return 
				context == DualityEditorApp.ActionContextOpenRes ||
				context == DualityEditorApp.ActionContextMenu;
		}

		private static Tilemap GetTilemap(object obj)
		{
			Tilemap tilemap = obj as Tilemap;
			if (tilemap == null)
			{
				GameObject gameObj = obj as GameObject;
				if (gameObj != null) tilemap = gameObj.GetComponent<Tilemap>();
			}
			return tilemap;
		}
	}
}
