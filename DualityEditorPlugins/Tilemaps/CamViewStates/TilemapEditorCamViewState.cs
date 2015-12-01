using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.Drawing;
using Duality.Components;
using Duality.Resources;
using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;
using Duality.Editor.Plugins.CamView.CamViewStates;


namespace Duality.Editor.Plugins.Tilemaps.CamViewStates
{
	public class TilemapEditorCamViewState : CamViewState
	{
		private enum TilemapTool
		{
			Select,
			Brush,
			Rect,
			Oval,
			Fill
		}

		private static readonly Point2 InvalidTile = new Point2(-1, -1);

		private TilemapTool     activeTool       = TilemapTool.Brush;
		private Tilemap         selectedTilemap  = null;
		private TilemapRenderer hoveredRenderer  = null;
		private Point2          hoveredTile      = InvalidTile;

		private ToolStrip       toolstrip        = null;
		private ToolStripButton toolButtonSelect = null;
		private ToolStripButton toolButtonBrush  = null;
		private ToolStripButton toolButtonRect   = null;
		private ToolStripButton toolButtonOval   = null;
		private ToolStripButton toolButtonFill   = null;


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
		
		private void UpdateToolbar()
		{
			this.toolButtonSelect.Checked = false;
			this.toolButtonBrush.Checked  = false;
			this.toolButtonRect.Checked   = false;
			this.toolButtonOval.Checked   = false;
			this.toolButtonFill.Checked   = false;

			switch (this.activeTool)
			{
				case TilemapTool.Select: this.toolButtonSelect.Checked = true; break;
				case TilemapTool.Brush:  this.toolButtonBrush.Checked  = true; break;
				case TilemapTool.Rect:   this.toolButtonRect.Checked   = true; break;
				case TilemapTool.Oval:   this.toolButtonOval.Checked   = true; break;
				case TilemapTool.Fill:   this.toolButtonFill.Checked   = true; break;
			}
		}
		private void UpdateCursor()
		{
			if (this.hoveredRenderer == null)
			{
				this.Cursor = TilemapsResCache.CursorTileSelect;
			}
			else if (this.hoveredRenderer != null && this.hoveredRenderer.ActiveTilemap != this.selectedTilemap)
			{
				this.Cursor = TilemapsResCache.CursorTileSelectActive;
			}
			else
			{
				switch (this.activeTool)
				{
					case TilemapTool.Select: this.Cursor = TilemapsResCache.CursorTileSelect; break;
					case TilemapTool.Brush:  this.Cursor = TilemapsResCache.CursorTileBrush;  break;
					case TilemapTool.Rect:   this.Cursor = TilemapsResCache.CursorTileRect;   break;
					case TilemapTool.Oval:   this.Cursor = TilemapsResCache.CursorTileOval;   break;
					case TilemapTool.Fill:   this.Cursor = TilemapsResCache.CursorTileFill;   break;
				}
			}
		}
		private void SetActiveTool(TilemapTool tool)
		{
			this.activeTool = tool;
			this.UpdateCursor();
			this.UpdateToolbar();
			this.Invalidate();
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

			// Init the custom tile editing toolbar
			{
				this.View.SuspendLayout();
				this.toolstrip = new ToolStrip();
				this.toolstrip.SuspendLayout();

				this.toolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
				this.toolstrip.Name = "toolstrip";
				this.toolstrip.Text = "Tilemap Editor Tools";

				this.toolButtonSelect = new ToolStripButton(TilemapsRes.ItemName_TileSelect, TilemapsResCache.IconTileSelect, this.toolButtonSelect_Click);
				this.toolButtonSelect.DisplayStyle = ToolStripItemDisplayStyle.Image;
				this.toolButtonSelect.AutoToolTip = true;
				this.toolstrip.Items.Add(this.toolButtonSelect);

				this.toolButtonBrush = new ToolStripButton(TilemapsRes.ItemName_TileBrush, TilemapsResCache.IconTileBrush, this.toolButtonBrush_Click);
				this.toolButtonBrush.DisplayStyle = ToolStripItemDisplayStyle.Image;
				this.toolButtonBrush.AutoToolTip = true;
				this.toolstrip.Items.Add(this.toolButtonBrush);

				this.toolButtonRect = new ToolStripButton(TilemapsRes.ItemName_TileRect, TilemapsResCache.IconTileRect, this.toolButtonRect_Click);
				this.toolButtonRect.DisplayStyle = ToolStripItemDisplayStyle.Image;
				this.toolButtonRect.AutoToolTip = true;
				this.toolstrip.Items.Add(this.toolButtonRect);

				this.toolButtonOval = new ToolStripButton(TilemapsRes.ItemName_TileOval, TilemapsResCache.IconTileOval, this.toolButtonOval_Click);
				this.toolButtonOval.DisplayStyle = ToolStripItemDisplayStyle.Image;
				this.toolButtonOval.AutoToolTip = true;
				this.toolstrip.Items.Add(this.toolButtonOval);

				this.toolButtonFill = new ToolStripButton(TilemapsRes.ItemName_TileFill, TilemapsResCache.IconTileFill, this.toolButtonFill_Click);
				this.toolButtonFill.DisplayStyle = ToolStripItemDisplayStyle.Image;
				this.toolButtonFill.AutoToolTip = true;
				this.toolstrip.Items.Add(this.toolButtonFill);

				this.toolstrip.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
				this.toolstrip.BackColor = Color.FromArgb(212, 212, 212);

				this.View.Controls.Add(this.toolstrip);
				this.View.Controls.SetChildIndex(this.toolstrip, this.View.Controls.IndexOf(this.View.ToolbarCamera));
				this.toolstrip.ResumeLayout(true);
				this.View.ResumeLayout(true);
			}

			// Register events
			DualityEditorApp.SelectionChanged += this.DualityEditorApp_SelectionChanged;

			// Initial update
			this.SetActiveTool(this.activeTool);
			this.UpdateToolbar();
		}
		protected override void OnLeaveState()
		{
			base.OnLeaveState();

			// Cleanup the custom tile editing toolbar
			{
				this.toolstrip.Dispose();
				this.toolButtonSelect.Dispose();
				this.toolButtonBrush.Dispose();
				this.toolButtonRect.Dispose();
				this.toolButtonOval.Dispose();
				this.toolButtonFill.Dispose();
				this.toolstrip = null;
				this.toolButtonSelect = null;
				this.toolButtonBrush = null;
				this.toolButtonRect = null;
				this.toolButtonOval = null;
				this.toolButtonFill = null;
			}

			// Unregister events
			DualityEditorApp.SelectionChanged -= this.DualityEditorApp_SelectionChanged;

			// Reset state
			this.Cursor = CursorHelper.Arrow;
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
				if (this.activeTool != TilemapTool.Select && a.ActiveTilemap == this.selectedTilemap && a.ActiveTilemap != b.ActiveTilemap)
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
				this.UpdateCursor();
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
				if (this.hoveredRenderer != null && this.hoveredRenderer.ActiveTilemap != null)
					DualityEditorApp.Select(this, new ObjectSelection(this.hoveredRenderer.ActiveTilemap.GameObj));
				else
					DualityEditorApp.Deselect(this, ObjectSelection.Category.GameObjCmp);
			}
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (Control.ModifierKeys == Keys.None)
			{
				if (e.KeyCode == Keys.Q && this.toolButtonSelect.Enabled)
					this.toolButtonSelect_Click(this, EventArgs.Empty);
				else if (e.KeyCode == Keys.W && this.toolButtonBrush.Enabled)
					this.toolButtonBrush_Click(this, EventArgs.Empty);
				else if (e.KeyCode == Keys.E && this.toolButtonRect.Enabled)
					this.toolButtonRect_Click(this, EventArgs.Empty);
				else if (e.KeyCode == Keys.R && this.toolButtonOval.Enabled)
					this.toolButtonOval_Click(this, EventArgs.Empty);
				else if (e.KeyCode == Keys.T && this.toolButtonFill.Enabled)
					this.toolButtonFill_Click(this, EventArgs.Empty);
			}
		}
		protected override void OnCamActionRequiresCursorChanged(EventArgs e)
		{
			base.OnCamActionRequiresCursorChanged(e);
			this.UpdateCursor();
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

			// Tilemap selection changed
			Tilemap newSelection = this.QuerySelectedTilemap();
			if (this.selectedTilemap != newSelection)
			{
				this.selectedTilemap = newSelection;
				this.UpdateCursor();
				this.Invalidate();
			}
		}

		private void toolButtonSelect_Click(object sender, EventArgs e)
		{
			this.SetActiveTool(TilemapTool.Select);
		}
		private void toolButtonBrush_Click(object sender, EventArgs e)
		{
			this.SetActiveTool(TilemapTool.Brush);
		}
		private void toolButtonRect_Click(object sender, EventArgs e)
		{
			this.SetActiveTool(TilemapTool.Rect);
		}
		private void toolButtonOval_Click(object sender, EventArgs e)
		{
			this.SetActiveTool(TilemapTool.Oval);
		}
		private void toolButtonFill_Click(object sender, EventArgs e)
		{
			this.SetActiveTool(TilemapTool.Fill);
		}
	}
}
