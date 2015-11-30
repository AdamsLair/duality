using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Duality;
using Duality.Drawing;
using Duality.Components;
using Duality.Resources;
using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.CamView.CamViewStates;


namespace Duality.Editor.Plugins.Tilemaps.CamViewStates
{
	public class TilemapEditorCamViewState : CamViewState
	{
		private static readonly Point2 InvalidTile = new Point2(-1, -1);

		private Tilemap         selectedTilemap = null;
		private TilemapRenderer hoveredRenderer = null;
		private Point2          hoveredTile     = InvalidTile;


		public override string StateName
		{
			get { return "Tilemap Editor"; }
		}


		public TilemapEditorCamViewState()
		{
			this.SetDefaultObjectVisibility(
				typeof(Tilemap),
				typeof(TilemapRenderer));
		}
		
		private Tilemap QuerySelectedTilemap()
		{
			// Detect whether the user has either selected a Tilemap directly, 
			// or a related Component that points to an external one
			return
				DualityEditorApp.Selection.Components.OfType<Tilemap>().FirstOrDefault() ?? 
				DualityEditorApp.Selection.GameObjects.GetComponents<Tilemap>().FirstOrDefault() ??
				DualityEditorApp.Selection.Components.OfType<TilemapRenderer>().Select(r => r.ExternalTilemap).FirstOrDefault() ?? 
				DualityEditorApp.Selection.GameObjects.GetComponents<TilemapRenderer>().Select(r => r.ExternalTilemap).FirstOrDefault();
		}
		private IEnumerable<TilemapRenderer> QueryVisibleTilemapRenderers()
		{
			var all = Scene.Current.FindComponents<TilemapRenderer>();
			return all.Where(r => 
				r.Active && 
				!DesignTimeObjectData.Get(r.GameObj).IsHidden && 
				this.IsCoordInView(r.GameObj.Transform.Pos, r.BoundRadius));
		}
		private bool GetTilemapDisplayedGreyedOut(Tilemap tilemap)
		{
			Tilemap hoveredTilemap = this.hoveredRenderer != null ? this.hoveredRenderer.ActiveTilemap : null;
			if (this.selectedTilemap != null)
				return tilemap != this.selectedTilemap && tilemap != hoveredTilemap;
			else
				return false;
		}

		protected override void OnEnterState()
		{
			base.OnEnterState();

			DualityEditorApp.SelectionChanged += this.DualityEditorApp_SelectionChanged;
		}
		protected override void OnLeaveState()
		{
			base.OnLeaveState();

			DualityEditorApp.SelectionChanged -= this.DualityEditorApp_SelectionChanged;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			Point2 lastHoveredTile = this.hoveredTile;
			TilemapRenderer lastHoveredRenderer = this.hoveredRenderer;

			// Reset hover data
			this.hoveredTile = InvalidTile;
			this.hoveredRenderer = null;

			// Determine which renderers we're able to see right now and sort them by their Z values
			TilemapRenderer[] visibleRenderers = this.QueryVisibleTilemapRenderers().ToArray();
			visibleRenderers.StableSort((a, b) =>
			{
				// The currently edited tilemap always prevails
				if (a.ActiveTilemap == this.selectedTilemap && a.ActiveTilemap != b.ActiveTilemap)
					return -1;
				// Otherwise, do regular Z sorting
				else
					return (a.GameObj.Transform.Pos.Z > b.GameObj.Transform.Pos.Z) ? 1 : -1;
			});

			// Iterate over visible tilemap renderers to find out what the cursor is hovering
			for (int i = 0; i < visibleRenderers.Length; i++)
			{
				TilemapRenderer renderer = visibleRenderers[i];
				Transform transform = renderer.GameObj.Transform;

				// Determine where the cursor is hovering in various coordinate systems
				Vector3 worldCursorPos = this.CameraComponent.GetSpaceCoord(new Vector3(e.X, e.Y, transform.Pos.Z));
				Vector2 localCursorPos = transform.GetLocalPoint(worldCursorPos.Xy);
				Point2 tileCursorPos = renderer.GetTileAtLocalPos(localCursorPos);

				// If we're hovering a tile of the current renderer, we're done
				if (tileCursorPos.X != -1 && tileCursorPos.Y != -1)
				{
					if (!DesignTimeObjectData.Get(renderer.GameObj).IsLocked)
					{
						this.hoveredTile = tileCursorPos;
						this.hoveredRenderer = renderer;
					}
					break;
				}
			}

			// If something changed, redraw the view
			if (lastHoveredTile != this.hoveredTile || lastHoveredRenderer != this.hoveredRenderer)
			{
				this.Invalidate();
			}
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.hoveredTile = InvalidTile;
			this.hoveredRenderer = null;
			this.Invalidate();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left)
			{
				if (this.hoveredRenderer != null)
					DualityEditorApp.Select(this, new ObjectSelection(this.hoveredRenderer.ActiveTilemap));
				else
					DualityEditorApp.Deselect(this, ObjectSelection.Category.GameObjCmp);
			}
		}

		protected override void OnRenderState()
		{
			// "Grey out" all non-selected Tilemap Renderers
			Dictionary<TilemapRenderer,ColorRgba> oldColors = null;
			if (this.selectedTilemap != null)
			{
				foreach (TilemapRenderer renderer in Scene.Current.FindComponents<TilemapRenderer>())
				{
					if (renderer.ActiveTilemap == this.selectedTilemap)
						continue;

					if (oldColors == null)
						oldColors = new Dictionary<TilemapRenderer,ColorRgba>();

					oldColors[renderer] = renderer.ColorTint;
					renderer.ColorTint = renderer.ColorTint.WithAlpha(0.33f);
				}
			}

			// Do all the regular state rendering
			base.OnRenderState();

			// Reset each renderer's color tint value
			if (oldColors != null)
			{
				foreach (var pair in oldColors)
				{
					pair.Key.ColorTint = pair.Value;
				}
			}
		}
		protected override void OnCollectStateDrawcalls(Canvas canvas)
		{
			base.OnCollectStateDrawcalls(canvas);

			TilemapRenderer[] visibleRenderers = this.QueryVisibleTilemapRenderers().ToArray();
			for (int i = 0; i < visibleRenderers.Length; i++)
			{
				TilemapRenderer renderer = visibleRenderers[i];
				Transform transform = renderer.GameObj.Transform;

				Tilemap tilemap = renderer.ActiveTilemap;
				Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
				Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;
				Rect localRect = renderer.LocalTilemapRect;
				bool greyOut = this.selectedTilemap != null && this.selectedTilemap != tilemap;

				// Determine the object's local coordinate system (rotated, scaled) in world space
				Vector2 worldAxisX = Vector2.UnitX;
				Vector2 worldAxisY = Vector2.UnitY;
				MathF.TransformCoord(ref worldAxisX.X, ref worldAxisX.Y, transform.Angle, transform.Scale);
				MathF.TransformCoord(ref worldAxisY.X, ref worldAxisY.Y, transform.Angle, transform.Scale);

				// Configure the canvas so our shapes are properly rotated and scaled
				canvas.State.TransformHandle = -localRect.TopLeft;
				canvas.State.TransformAngle = transform.Angle;
				canvas.State.TransformScale = new Vector2(transform.Scale);
				canvas.State.ZOffset = -0.01f;

				// Draw the surrounding rect of the tilemap
				canvas.State.ColorTint = this.FgColor.WithAlpha(greyOut ? 0.33f : 1.0f);
				canvas.DrawRect(
					transform.Pos.X, 
					transform.Pos.Y, 
					transform.Pos.Z,
					localRect.W, 
					localRect.H);

				// Highlight the currently hovered tile
				if (this.hoveredRenderer == renderer && this.hoveredTile != InvalidTile)
				{
					Vector2 localTilePos = tileSize * this.hoveredTile;
					Vector2 worldTilePos = localTilePos.X * worldAxisX + localTilePos.Y * worldAxisY;
					canvas.State.ColorTint = this.FgColor;
					canvas.DrawRect(
						transform.Pos.X + worldTilePos.X, 
						transform.Pos.Y + worldTilePos.Y, 
						transform.Pos.Z,
						tileSize.X, 
						tileSize.Y);
				}
			}
		}

		private void DualityEditorApp_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.SameObjects) return;
			if (!e.AffectedCategories.HasFlag(ObjectSelection.Category.GameObjCmp))
				return;

			// Collider selection changed
			if ((e.AffectedCategories & ObjectSelection.Category.GameObjCmp) != ObjectSelection.Category.None)
			{
				Tilemap newSelection = this.QuerySelectedTilemap();
				this.selectedTilemap = newSelection;
			}

			this.Invalidate();
		}
	}
}
