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
using Duality.Editor.Plugins.Tilemaps.Properties;


namespace Duality.Editor.Plugins.Tilemaps.EditorActions
{
	/// <summary>
	/// Opens a setup dialog that allows to configure the selected tilemaps.
	/// </summary>
	public class OpenTilemapResizeDialog : EditorAction<Tilemap>
	{
		public override string Name
		{
			get { return TilemapsRes.ActionName_ResizeTilemap; }
		}
		public override string Description
		{
			get { return TilemapsRes.ActionDesc_ResizeTilemap; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconResize; }
		}

		public override void Perform(IEnumerable<Tilemap> objEnum)
		{
			// Since resizing is usually done for all tilemaps in a layered setup,
			// but users will often have only one of them selected due to previous
			// editing operations, check for layered setups and deal with them.
			List<Tilemap> tilemaps = objEnum.ToList();
			if (tilemaps.Count == 1)
			{
				Tilemap baseTilemap = tilemaps[0];
				GameObject baseObject = baseTilemap.GameObj;
				Scene baseScene = (baseObject != null) ? baseObject.ParentScene : null;
				
				// Prerequisite: We are operating on a tilemap with a proper parent and
				// parent scene available. Otherwise, there is nothing we could do.
				if (baseScene != null)
				{
					// Since a tilemap is just a data container without any relation to a
					// scene or layered setups, we will check tilemap renderers instead.
					// Only they can provide the required information for determining layered setups.
					ICmpTilemapRenderer[] allRenderers = baseScene.FindComponents<ICmpTilemapRenderer>().ToArray();
					ICmpTilemapRenderer baseRenderer = allRenderers.FirstOrDefault(r => r.ActiveTilemap == baseTilemap);
					Transform baseTransform = (baseRenderer != null) ? (baseRenderer as Component).GameObj.Transform : null;
					if (baseTransform != null)
					{
						foreach (ICmpTilemapRenderer renderer in allRenderers)
						{
							if (renderer.ActiveTilemap == null) continue;
							if (tilemaps.Contains(renderer.ActiveTilemap)) continue;
							
							Transform transform = (renderer as Component).GameObj.Transform;
							if (baseTransform.Pos.Xy != transform.Pos.Xy) continue;
							if (baseTransform.Angle != transform.Angle) continue;
							if (baseTransform.Scale != transform.Scale) continue;

							if (baseTilemap.Size != renderer.ActiveTilemap.Size) continue;
							if (baseRenderer.LocalTilemapRect != renderer.LocalTilemapRect) continue;

							tilemaps.Add(renderer.ActiveTilemap);
						}
					}
				}
			}

			// Open the resize dialog. It will handle all the rest.
			TilemapResizeDialog resizeDialog = new TilemapResizeDialog();
			resizeDialog.Tilemaps = tilemaps;
			resizeDialog.ShowDialog(DualityEditorApp.MainForm);
		}
		public override bool MatchesContext(string context)
		{
			return context == TilemapsEditorPlugin.ActionTilemapEditor;
		}
		public override bool CanPerformOn(IEnumerable<Tilemap> objEnum)
		{
			return base.CanPerformOn(objEnum) && objEnum.Any();
		}
	}
}
