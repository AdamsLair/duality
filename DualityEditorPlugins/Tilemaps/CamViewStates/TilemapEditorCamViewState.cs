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
using Duality.Editor.Plugins.Tilemaps.UndoRedoActions;
using Duality.Editor.Plugins.CamView.CamViewStates;
using Duality.Editor.Plugins.CamView.CamViewLayers;


namespace Duality.Editor.Plugins.Tilemaps.CamViewStates
{
	public class TilemapEditorCamViewState : CamViewState
	{
		private enum TilemapTool
		{
			None,

			Select,
			Brush,
			Rect,
			Oval,
			Fill
		}
		private enum ContinuousAction
		{
			None,

			DrawTile,
			FillTileRect,
			FillTileOval
		}

		private static readonly Point2 InvalidTile = new Point2(-1, -1);

		private TilemapTool      selectedTool     = TilemapTool.Brush;
		private Tilemap          selectedTilemap  = null;
		private TilemapRenderer  hoveredRenderer  = null;
		private Point2           hoveredTile      = InvalidTile;
		private TilemapTool      activeTool       = TilemapTool.None;
		private Tilemap          activeTilemap    = null;
		private Point2           activeAreaOrigin = InvalidTile;
		private Grid<bool>       activeArea       = new Grid<bool>();
		private ContinuousAction action           = ContinuousAction.None;

		private ToolStrip        toolstrip        = null;
		private ToolStripButton  toolButtonSelect = null;
		private ToolStripButton  toolButtonBrush  = null;
		private ToolStripButton  toolButtonRect   = null;
		private ToolStripButton  toolButtonOval   = null;
		private ToolStripButton  toolButtonFill   = null;


		public override string StateName
		{
			get { return "Tilemap Editor"; }
		}


		public TilemapEditorCamViewState()
		{
			this.SetDefaultObjectVisibility(
				typeof(Tilemap),
				typeof(TilemapRenderer));
			this.SetDefaultActiveLayers(
				typeof(GridCamViewLayer));
		}
		
		private void SetActiveTool(TilemapTool tool)
		{
			this.selectedTool = tool;
			this.UpdateToolbar();
			this.OnMouseMove();
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
		
		private void UpdateToolbar()
		{
			this.toolButtonSelect.Checked = false;
			this.toolButtonBrush.Checked  = false;
			this.toolButtonRect.Checked   = false;
			this.toolButtonOval.Checked   = false;
			this.toolButtonFill.Checked   = false;

			switch (this.selectedTool)
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
			switch (this.activeTool)
			{
				case TilemapTool.None:   this.Cursor = TilemapsResCache.CursorTileSelect;       break;
				case TilemapTool.Select: this.Cursor = TilemapsResCache.CursorTileSelectActive; break;
				case TilemapTool.Brush:  this.Cursor = TilemapsResCache.CursorTileBrush;        break;
				case TilemapTool.Rect:   this.Cursor = TilemapsResCache.CursorTileRect;         break;
				case TilemapTool.Oval:   this.Cursor = TilemapsResCache.CursorTileOval;         break;
				case TilemapTool.Fill:   this.Cursor = TilemapsResCache.CursorTileFill;         break;
			}
		}
		private void UpdateHoverState(Point cursorPos)
		{
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
				if (this.selectedTool != TilemapTool.Select && a.ActiveTilemap == this.selectedTilemap && a.ActiveTilemap != b.ActiveTilemap)
					return -1;
				// Otherwise, do regular Z sorting
				else
					return (a.GameObj.Transform.Pos.Z > b.GameObj.Transform.Pos.Z) ? 1 : -1;
			});

			// While doing an action, it's either the selected tilemap or none. No switch inbetween.
			if (this.action != ContinuousAction.None && visibleRenderers.Length > 0)
			{
				visibleRenderers = new TilemapRenderer[] { visibleRenderers[0] };
			}

			// Iterate over visible tilemap renderers to find out what the cursor is hovering
			for (int i = 0; i < visibleRenderers.Length; i++)
			{
				TilemapRenderer renderer = visibleRenderers[i];
				Transform transform = renderer.GameObj.Transform;

				// Determine where the cursor is hovering in various coordinate systems
				Vector3 worldCursorPos = this.CameraComponent.GetSpaceCoord(new Vector3(cursorPos.X, cursorPos.Y, transform.Pos.Z));
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
		private void UpdateActiveState()
		{
			TilemapTool lastActiveTool = this.activeTool;
			Tilemap lastActiveTilemap = this.activeTilemap;

			// Determine what action the cursor would do in the current state
			if (this.hoveredRenderer == null)
				this.activeTool = TilemapTool.None;
			else if (this.selectedTilemap != null && this.hoveredRenderer != null && this.hoveredRenderer.ActiveTilemap != this.selectedTilemap)
				this.activeTool = TilemapTool.Select;
			else
				this.activeTool = this.selectedTool;

			if (this.activeTool != TilemapTool.None)
				this.activeTilemap = this.hoveredRenderer.ActiveTilemap ?? this.selectedTilemap;

			// Determine the area that is affected by the current action
			switch (this.activeTool)
			{
				case TilemapTool.None:
				{
					this.activeAreaOrigin = this.hoveredTile;
					this.activeArea.Resize(0, 0);
					break;
				}
				case TilemapTool.Select:
				case TilemapTool.Brush:
				{
					this.activeAreaOrigin = this.hoveredTile;
					this.activeArea.Resize(1, 1);
					this.activeArea[0, 0] = true;
					break;
				}
				case TilemapTool.Rect:
				{
					this.activeAreaOrigin = this.hoveredTile;
					this.activeArea.Resize(1, 1);
					this.activeArea[0, 0] = true;
					break;
				}
				case TilemapTool.Oval:
				{
					this.activeAreaOrigin = this.hoveredTile;
					this.activeArea.Resize(1, 1);
					this.activeArea[0, 0] = true;
					break;
				}
				case TilemapTool.Fill:
				{
					this.activeAreaOrigin = this.hoveredTile;
					this.activeArea.Resize(1, 1);
					this.activeArea[0, 0] = true;
					break;
				}
			}

			// If our highlighted action changed, redraw view and update the cursor
			if (lastActiveTool != this.activeTool || lastActiveTilemap != this.activeTilemap)
			{
				this.UpdateCursor();
				this.Invalidate();
			}
		}

		private void EditTilemapDrawTile(Tilemap tilemap, Point2 pos, Tile tile)
		{
			UndoRedoManager.Do(new EditTilemapAction(
				tilemap, 
				EditTilemapActionType.DrawTile, 
				pos, 
				new Grid<Tile>(1, 1, new Tile[] { tile }),
				new Grid<bool>(1, 1, new bool[] { true })));
		}

		private void BeginContinuousAction(ContinuousAction action)
		{
			if (this.action == action) return;
			this.action = action;

			if (this.action == ContinuousAction.DrawTile)
			{
				this.EditTilemapDrawTile(this.activeTilemap, this.activeAreaOrigin, new Tile { Index = 1 });
			}
		}
		private void UpdateContinuousAction()
		{
			if (this.action == ContinuousAction.DrawTile)
			{
				this.EditTilemapDrawTile(this.activeTilemap, this.activeAreaOrigin, new Tile { Index = 1 });
			}
		}
		private void EndContinuousAction()
		{
			if (this.action == ContinuousAction.None) return;
			this.action = ContinuousAction.None;
			UndoRedoManager.Finish();
		}

		private void DrawTileHighlights(Canvas canvas, TilemapRenderer renderer, ColorRgba color, Point2 origin, Grid<bool> highlight)
		{
			if (highlight.Capacity == 0) return;

			BatchInfo defaultMaterial = canvas.State.Material;
			BatchInfo highlightMaterial = this.FgColor.GetLuminance() > 0.5f ? 
				new BatchInfo(DrawTechnique.Light, ColorRgba.White.WithAlpha(0.5f)) :
				new BatchInfo(DrawTechnique.Alpha, ColorRgba.White);

			Transform transform = renderer.GameObj.Transform;
			Tilemap tilemap = renderer.ActiveTilemap;
			Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
			Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;
			Rect localRect = renderer.LocalTilemapRect;

			// Determine the object's local coordinate system (rotated, scaled) in world space
			Vector2 worldAxisX = Vector2.UnitX;
			Vector2 worldAxisY = Vector2.UnitY;
			MathF.TransformCoord(ref worldAxisX.X, ref worldAxisX.Y, transform.Angle, transform.Scale);
			MathF.TransformCoord(ref worldAxisY.X, ref worldAxisY.Y, transform.Angle, transform.Scale);

			Vector2 localOriginPos = tileSize * origin;
			Vector2 worldOriginPos = localOriginPos.X * worldAxisX + localOriginPos.Y * worldAxisY;

			canvas.PushState();
			{
				// Configure the canvas so our shapes are properly rotated and scaled
				canvas.State.TransformHandle = -localRect.TopLeft;
				canvas.State.TransformAngle = transform.Angle;
				canvas.State.TransformScale = new Vector2(transform.Scale);
				canvas.State.ZOffset = -0.01f;
				
				// Fill all highlighted tiles that are currently visible
				{
					canvas.State.ColorTint = this.FgColor.WithAlpha(0.25f);
					canvas.State.SetMaterial(highlightMaterial);
				
					// Determine tile visibility
					Vector2 worldTilemapOriginPos = localRect.TopLeft;
					MathF.TransformCoord(ref worldTilemapOriginPos.X, ref worldTilemapOriginPos.Y, transform.Angle, transform.Scale);
					TilemapCulling.TileInput cullingIn = new TilemapCulling.TileInput
					{
						// Remember: All these transform values are in world space
						TilemapPos = transform.Pos + new Vector3(worldTilemapOriginPos) + new Vector3(worldOriginPos),
						TilemapScale = transform.Scale,
						TilemapAngle = transform.Angle,
						TileCount = new Point2(highlight.Width, highlight.Height),
						TileSize = tileSize
					};
					TilemapCulling.TileOutput cullingOut = TilemapCulling.GetVisibleTileRect(canvas.DrawDevice, cullingIn);
					int renderedTileCount = cullingOut.VisibleTileCount.X * cullingOut.VisibleTileCount.Y;

					// Draw all visible highlighted tiles
					Point2 tileGridPos = cullingOut.VisibleTileStart;
					Vector2 renderStartPos = worldOriginPos + tileGridPos.X * tileSize.X * worldAxisX + tileGridPos.Y * tileSize.Y * worldAxisY;;
					Vector2 renderPos = renderStartPos;
					Vector2 tileXStep = worldAxisX * tileSize.X;
					Vector2 tileYStep = worldAxisY * tileSize.Y;
					for (int tileIndex = 0; tileIndex < renderedTileCount; tileIndex++)
					{
						if (highlight[tileGridPos.X, tileGridPos.Y])
						{
							canvas.FillRect(
								transform.Pos.X + renderPos.X, 
								transform.Pos.Y + renderPos.Y, 
								transform.Pos.Z,
								tileSize.X, 
								tileSize.Y);
						}

						tileGridPos.X++;
						renderPos += tileXStep;
						if ((tileGridPos.X - cullingOut.VisibleTileStart.X) >= cullingOut.VisibleTileCount.X)
						{
							tileGridPos.X = cullingOut.VisibleTileStart.X;
							tileGridPos.Y++;
							renderPos = renderStartPos;
							renderPos += tileYStep * (tileGridPos.Y - cullingOut.VisibleTileStart.Y);
						}
					}
				}

				// Determine the outlines of individual highlighted tile patches
				List<Vector2[]> outlines = new List<Vector2[]>();
				{
					// Generate a data structure containing all visible edges
					TileEdgeMap edgeMap = new TileEdgeMap(highlight.Width + 1, highlight.Height + 1);
					for (int y = 0; y < edgeMap.Height; y++)
					{
						for (int x = 0; x < edgeMap.Width; x++)
						{
							// Determine highlight state of the four tiles around this node
							bool topLeft     = x > 0               && y > 0                && highlight[x - 1, y - 1];
							bool topRight    = x < highlight.Width && y > 0                && highlight[x    , y - 1];
							bool bottomLeft  = x > 0               && y < highlight.Height && highlight[x - 1, y    ];
							bool bottomRight = x < highlight.Width && y < highlight.Height && highlight[x    , y    ];

							// Determine which edges are visible
							if (topLeft     != topRight   ) edgeMap.AddEdge(new Point2(x, y), new Point2(x    , y - 1));
							if (topRight    != bottomRight) edgeMap.AddEdge(new Point2(x, y), new Point2(x + 1, y    ));
							if (bottomRight != bottomLeft ) edgeMap.AddEdge(new Point2(x, y), new Point2(x    , y + 1));
							if (bottomLeft  != topLeft    ) edgeMap.AddEdge(new Point2(x, y), new Point2(x - 1, y    ));
						}
					}

					// Traverse edges to form outlines until no more edges are left
					RawList<Vector2> outlineBuilder = new RawList<Vector2>();
					while (true)
					{
						// Find the beginning of an outline
						Point2 current = edgeMap.FindNonEmpty();
						if (current.X == -1 || current.Y == -1) break;

						// Traverse it until no more edges are left
						while (true)
						{
							Point2 next = edgeMap.GetClockwiseNextFrom(current);
							if (next.X == -1 || next.Y == -1) break;

							outlineBuilder.Add(next * tileSize);
							edgeMap.RemoveEdge(current, next);
							current = next;
						}

						// If we have enough vertices, keep the outline for drawing
						Vector2[] outline = new Vector2[outlineBuilder.Count];
						outlineBuilder.CopyTo(outline, 0);
						outlines.Add(outline);

						// Reset the outline builder to an empty state
						outlineBuilder.Clear();
					}
				}

				// Draw outlines around all highlighted tile patches
				canvas.State.ColorTint = this.FgColor;
				canvas.State.SetMaterial(defaultMaterial);
				foreach (Vector2[] outline in outlines)
				{
					canvas.DrawPolygon(
						outline,
						transform.Pos.X + worldOriginPos.X, 
						transform.Pos.Y + worldOriginPos.Y, 
						transform.Pos.Z);
				}

			}
			canvas.PopState();
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
			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;

			// Initial update
			this.SetActiveTool(this.selectedTool);
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
			DualityEditorApp.ObjectPropertyChanged -= this.DualityEditorApp_ObjectPropertyChanged;

			// Reset state
			this.Cursor = CursorHelper.Arrow;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			Point2 lastHoverTile = this.hoveredTile;

			// Determine what the cursor is hovering over and what actions it could perform
			this.UpdateHoverState(e.Location);
			this.UpdateActiveState();

			// If we're performing a continuous action, update it when our hover tile changes
			if (this.hoveredTile != lastHoverTile)
			{
				this.UpdateContinuousAction();
			}
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.hoveredTile = InvalidTile;
			this.hoveredRenderer = null;
			this.activeTool = TilemapTool.None;
			this.activeTilemap = null;
			this.activeAreaOrigin = InvalidTile;
			this.activeArea.Resize(0, 0);
			this.UpdateCursor();
			this.Invalidate();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left)
			{
				// Begin a continuous action, if one is associated with the currently active tool
				ContinuousAction newAction = GetContinuousActionOfTool(this.activeTool);
				if (newAction != ContinuousAction.None)
				{
					if (this.selectedTilemap != this.activeTilemap)
						DualityEditorApp.Select(this, new ObjectSelection(this.activeTilemap.GameObj));
					this.BeginContinuousAction(newAction);
				}
				// Otherwise, do a selection or deselection
				else if (this.activeTool == TilemapTool.Select)
					DualityEditorApp.Select(this, new ObjectSelection(this.hoveredRenderer.ActiveTilemap.GameObj));
				else if (this.activeTool == TilemapTool.None)
					DualityEditorApp.Deselect(this, ObjectSelection.Category.GameObjCmp);
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.EndContinuousAction();
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
			this.OnMouseMove();
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
				Rect localRect = renderer.LocalTilemapRect;
				bool greyOut = this.selectedTilemap != null && this.selectedTilemap != tilemap;

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

				// Highlight the currently active tiles
				if (this.activeTilemap == renderer.ActiveTilemap && this.activeAreaOrigin != InvalidTile)
				{
					this.DrawTileHighlights(canvas, renderer, ColorRgba.Red, this.activeAreaOrigin, this.activeArea);
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
				if (this.Mouseover)
					this.OnMouseMove();
				this.Invalidate();
			}
		}
		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (e.HasProperty(TilemapsReflectionInfo.Property_Tilemap_Tiles))
			{
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

		private static ContinuousAction GetContinuousActionOfTool(TilemapTool tool)
		{
			switch (tool)
			{
				default:                return ContinuousAction.None;
				case TilemapTool.Brush: return ContinuousAction.DrawTile;
				case TilemapTool.Rect:  return ContinuousAction.FillTileRect;
				case TilemapTool.Oval:  return ContinuousAction.FillTileOval;
			}
		}
	}
}
