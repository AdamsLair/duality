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
	/// <summary>
	/// Provides tools for editing <see cref="Tilemap">tilemaps</see>, which are embedded
	/// directly into the current <see cref="Scene"/> using an <see cref="ICmpTilemapRenderer"/>.
	/// </summary>
	public class TilemapEditorCamViewState : CamViewState, ITilemapToolEnvironment
	{
		private struct TilemapActionEntry
		{
			public IEditorAction Action;
			public ToolStripButton ToolButton;
		}

		private static readonly float           FillAnimDuration = 250.0f;
		private static readonly Point2          InvalidTile      = new Point2(-1, -1);
		private static Texture                  strippledLineTex = null;


		private TilemapTool toolNone   = new NoTilemapTool();
		private TilemapTool toolSelect = new SelectTilemapTool();

		private List<TilemapActionEntry> actions     = new List<TilemapActionEntry>();
		private List<TilemapTool>   tools              = new List<TilemapTool>();
		private TilemapTool         overrideTool       = null;
		private TilemapTool         selectedTool       = null;
		private Tilemap             selectedTilemap    = null;
		private ICmpTilemapRenderer hoveredRenderer    = null;
		private Point2              hoveredTile        = InvalidTile;
		private TilemapTool         activeTool         = null;
		private Tilemap             activeTilemap      = null;
		private ICmpTilemapRenderer activeRenderer     = null;
		private Point2              activeAreaOrigin   = InvalidTile;
		private Grid<bool>          activeArea         = new Grid<bool>();
		private List<Vector2[]>     activeAreaOutlines = new List<Vector2[]>();
		private bool                activePreviewValid = false;
		private DateTime            activePreviewTime  = DateTime.Now;
		private TilemapTool         actionTool         = null;
		private Point2              actionBeginTile    = InvalidTile;
		private ToolStrip           toolstrip          = null;


		public override string StateName
		{
			get { return TilemapsRes.CamViewState_TilemapEditor_Name; }
		}
		public ITileDrawSource TileDrawSource
		{
			get { return TilemapsEditorPlugin.Instance.TileDrawingSource; }
			set { TilemapsEditorPlugin.Instance.TileDrawingSource = value; }
		}
		public TilemapTool SelectedTool
		{
			get { return this.selectedTool; }
			set
			{
				if (this.selectedTool == value) return;

				this.selectedTool = value;
				this.UpdateTilemapToolButtons();

				// Invalidate cursor state 
				this.OnMouseLeave(EventArgs.Empty);
				this.OnMouseMove();
			}
		}
		private TilemapTool OverrideTool
		{
			get { return this.overrideTool; }
			set
			{
				if (this.overrideTool == value) return;

				this.overrideTool = value;
				this.UpdateTilemapToolButtons();

				// Invalidate cursor state 
				this.OnMouseLeave(EventArgs.Empty);
				this.OnMouseMove();
			}
		}
		
		Point2 ITilemapToolEnvironment.HoveredTile
		{
			get { return this.hoveredTile; }
		}
		Tilemap ITilemapToolEnvironment.ActiveTilemap
		{
			get { return this.activeTilemap; }
		}
		Point2 ITilemapToolEnvironment.ActiveOrigin
		{
			get { return this.activeAreaOrigin; }
			set { this.activeAreaOrigin = value; }
		}
		Grid<bool> ITilemapToolEnvironment.ActiveArea
		{
			get { return this.activeArea; }
		}
		IList<Vector2[]> ITilemapToolEnvironment.ActiveAreaOutlines
		{
			get { return this.activeAreaOutlines; }
		}
		Point2 ITilemapToolEnvironment.ActionBeginTile
		{
			get { return this.actionBeginTile; }
		}


		public TilemapEditorCamViewState()
		{
			this.SetDefaultObjectVisibility(
				typeof(Tilemap),
				typeof(ICmpTilemapRenderer));
			this.SetDefaultActiveLayers(
				typeof(GridCamViewLayer));
		}
		
		/// <inheritdoc />
		public override void GetDisplayedGridData(Point cursorPos, ref GridLayerData data)
		{
			base.GetDisplayedGridData(cursorPos, ref data);

			Tilemap tilemap = this.activeTilemap ?? this.selectedTilemap;
			Tileset tileset = (tilemap != null) ? tilemap.Tileset.Res : null;
			Vector2 tileSize = (tileset != null) ? tileset.TileSize : Tileset.DefaultTileSize;
			data.GridBaseSize = tileSize;

			if (this.hoveredRenderer != null)
				data.DisplayedGridPos = new Vector3(this.hoveredTile);
		}

		private IEnumerable<ICmpTilemapRenderer> QueryVisibleTilemapRenderers()
		{
			return Scene.Current.FindComponents<ICmpTilemapRenderer>().Where(r => 
			{
				Component cmp = r as Component;
				return
					cmp.Active && 
					!DesignTimeObjectData.Get(cmp.GameObj).IsHidden && 
					this.IsCoordInView(cmp.GameObj.Transform.Pos, r.BoundRadius);
			});
		}
		private IEnumerable<Tilemap> QueryTilemapsInScene()
		{
			return Scene.Current.FindComponents<ICmpTilemapRenderer>()
				.Select(r => r.ActiveTilemap)
				.Where(t => t != null && t.Active && !DesignTimeObjectData.Get(t.GameObj).IsHidden)
				.Distinct();
		}
		private IEnumerable<Tilemap> QueryActionTilemaps()
		{
			if (this.selectedTilemap != null)
				return TilemapsEditorSelectionParser.QuerySelectedTilemaps();
			else
				return this.QueryTilemapsInScene();
		}
		private Point2 GetTileAtLocalPos(ICmpTilemapRenderer renderer, Point localPos, TilePickMode pickMode)
		{
			Component component = renderer as Component;
			Transform transform = component.GameObj.Transform;

			// Determine where the cursor is hovering in various coordinate systems
			Vector3 worldCursorPos = this.CameraComponent.GetSpaceCoord(new Vector3(localPos.X, localPos.Y, transform.Pos.Z));
			Vector2 localCursorPos = transform.GetLocalPoint(worldCursorPos.Xy);

			// Determine tile coordinates of the cursor
			return renderer.GetTileAtLocalPos(localCursorPos, pickMode);
		}
		
		private bool IsTileTransparent(Tileset tileset, int tileIndex)
		{
			if (tileset == null) return true;
			if (tileIndex < 0) return true;
			if (tileIndex >= tileset.TileCount) return true;

			return tileset.TileData[tileIndex].IsVisuallyEmpty;
		}

		private void UpdateTilemapToolButtons()
		{
			foreach (TilemapTool tool in this.tools)
			{
				if (tool.ToolButton == null) continue;
				tool.ToolButton.Checked = (this.selectedTool == tool);
			}
		}
		private void UpdateActionToolButtons()
		{
			Tilemap[] actionTilemaps = this.QueryActionTilemaps().ToArray();
			foreach (TilemapActionEntry entry in this.actions)
			{
				if (entry.ToolButton == null) continue;
				entry.ToolButton.Visible = entry.Action.CanPerformOn(actionTilemaps);
			}
		}
		private void UpdateCursor()
		{
			this.Cursor = this.activeTool.ActionCursor;
		}
		private void UpdateHoverState(Point cursorPos)
		{
			Point2 lastHoveredTile = this.hoveredTile;
			ICmpTilemapRenderer lastHoveredRenderer = this.hoveredRenderer;

			// Reset hover data
			this.hoveredTile = InvalidTile;
			this.hoveredRenderer = null;

			// Early-out, if the cursor isn't even inside the CamView area - unless the user is performing a cursor action
			if (!this.View.ClientRectangle.Contains(cursorPos) && this.actionTool == this.toolNone)
			{
				if (lastHoveredTile != this.hoveredTile || lastHoveredRenderer != this.hoveredRenderer)
					this.Invalidate();
				return;
			}

			// Early-out, if a camera action claims the cursor
			if (this.CamActionRequiresCursor)
			{
				if (lastHoveredTile != this.hoveredTile || lastHoveredRenderer != this.hoveredRenderer)
					this.Invalidate();
				return;
			}
			
			// While doing an action, it's either the selected tilemap or none. No picking, just determine the hovered tile
			bool performingAction = this.actionTool != this.toolNone;
			if (performingAction)
			{
				this.hoveredTile = this.GetTileAtLocalPos(this.activeRenderer, cursorPos, TilePickMode.Free);
				this.hoveredRenderer = this.activeRenderer;
			}
			// Otherwise, perform a tile-based picking operation
			else
			{
				List<ICmpTilemapRenderer> visibleRenderers;
				bool pureZSortPicking = !(this.overrideTool ?? this.selectedTool).PickPreferSelectedLayer || this.selectedTilemap == null;

				// Determine which renderers we're able to see right now and sort them by their Z values
				visibleRenderers = this.QueryVisibleTilemapRenderers().ToList();
				visibleRenderers.StableSort((a, b) =>
				{
					// When prefered by the editing tool, the currently edited tilemap always prevails in picking checks
					if (!pureZSortPicking && a.ActiveTilemap != b.ActiveTilemap)
					{
						if (a.ActiveTilemap == this.selectedTilemap)
							return -1;
						else if (b.ActiveTilemap == this.selectedTilemap)
							return 1;
					}

					// Otherwise, do regular Z sorting
					return 
						((a as Component).GameObj.Transform.Pos.Z + a.BaseDepthOffset > 
						(b as Component).GameObj.Transform.Pos.Z + b.BaseDepthOffset) 
						? 1 : -1;
				});

				// Eliminate all tilemap renderers without a tile hit, so we remain with only the renderers under the cursor.
				for (int i = visibleRenderers.Count - 1; i >= 0; i--)
				{
					ICmpTilemapRenderer renderer = visibleRenderers[i];
					Point2 tileCursorPos = this.GetTileAtLocalPos(renderer, cursorPos, TilePickMode.Reject);
					if (tileCursorPos == InvalidTile)
					{
						visibleRenderers.RemoveAt(i);
						continue;
					}
				}

				// Iterate over the remaining tilemap renderers to find out which one prevails
				for (int i = 0; i < visibleRenderers.Count; i++)
				{
					ICmpTilemapRenderer renderer = visibleRenderers[i];
					Component component = renderer as Component;
					Point2 tileCursorPos = this.GetTileAtLocalPos(renderer, cursorPos, TilePickMode.Reject);

					// If the hovered tile is transparent, don't treat it as a hit unless it's the bottom renderer
					bool isBottomRenderer = (i == visibleRenderers.Count - 1);
					if (pureZSortPicking && !isBottomRenderer)
					{
						Tilemap tilemap = renderer.ActiveTilemap;
						int tileIndex = (tilemap != null) ? tilemap.Tiles[tileCursorPos.X, tileCursorPos.Y].Index : -1;
						if (tileIndex == -1 || this.IsTileTransparent(tilemap.Tileset.Res, tileIndex))
						{
							tileCursorPos = InvalidTile;
						}
					}

					// If we're hovering a tile of the current renderer, we're done
					if (tileCursorPos != InvalidTile)
					{
						// If it's locked and not selected, treat it as blocking but don't allow editing
						bool isLocked = DesignTimeObjectData.Get(component.GameObj).IsLocked;
						if (isLocked)
						{
							bool isSelected = false;
							IEnumerable<Tilemap> selectedTilemaps = TilemapsEditorSelectionParser.QuerySelectedTilemaps();
							foreach (Tilemap tilemap in selectedTilemaps)
							{
								if (renderer.ActiveTilemap == tilemap)
								{
									isSelected = true;
									break;
								}
							}
							if (!isSelected) break;
						}

						// Set the hovered renderer and stop picking
						this.hoveredTile = tileCursorPos;
						this.hoveredRenderer = renderer;
						break;
					}
				}
			}

			// If we're not doing an action, let our action begin tile just follow around
			if (this.actionTool == this.toolNone)
				this.actionBeginTile = this.hoveredTile;

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
			ICmpTilemapRenderer lastActiveRenderer = this.activeRenderer;

			// If an action is currently being performed, that action will always be the active tool
			if (this.actionTool != this.toolNone)
			{
				this.activeTool = this.actionTool;
			}
			// Otherwise, determine what action the cursor would do right now
			else
			{
				// Determine the active tool dynamically based on user input state
				if (this.hoveredRenderer == null)
					this.activeTool = this.toolNone;
				else if (this.selectedTilemap == null)
					this.activeTool = this.toolSelect;
				else if (this.hoveredRenderer.ActiveTilemap != this.selectedTilemap)
					this.activeTool = this.toolSelect;
				else
					this.activeTool = this.overrideTool ?? this.selectedTool;

				// Keep in mind on what renderer and tilemap belong to the currently active tool
				this.activeTilemap = (this.hoveredRenderer != null) ? this.hoveredRenderer.ActiveTilemap : null;
				this.activeRenderer = this.hoveredRenderer;
			}

			// Determine the area that is affected by the current action
			this.activeTool.UpdatePreview();

			// If our highlighted action changed, redraw view and update the cursor
			if (lastActiveTool != this.activeTool || lastActiveTilemap != this.activeTilemap || lastActiveRenderer != this.activeRenderer)
			{
				this.UpdateCursor();
				this.Invalidate();
			}
		}
		private void UpdateOverrideToolFromModifierKeys()
		{
			foreach (TilemapTool tool in this.tools)
			{
				bool modifierPressed = false;
				if (tool.OverrideKey == Keys.Menu)
					modifierPressed = (Control.ModifierKeys & Keys.Alt) != Keys.None;
				else if (tool.OverrideKey == Keys.ShiftKey)
					modifierPressed = (Control.ModifierKeys & Keys.Shift) != Keys.None;
				else if (tool.OverrideKey == Keys.ControlKey)
					modifierPressed = (Control.ModifierKeys & Keys.Control) != Keys.None;
				else
					continue;

				if (this.overrideTool == null)
				{
					if (modifierPressed)
					{
						this.OverrideTool = tool;
						break;
					}
				}
				else if (this.overrideTool == tool)
				{
					if (!modifierPressed)
					{
						this.OverrideTool = null;
						break;
					}
				}
			}
		}

		private void BeginToolAction(TilemapTool action)
		{
			if (this.actionTool == action) return;

			this.actionTool = action;
			this.actionBeginTile = this.activeAreaOrigin;

			// Start a tile update operation, so as long as the same tool
			// action is active, the edited tilemap won't fire any change events,
			// bug aggregate them into a single one at the end.
			this.activeTilemap.BeginUpdateTiles();

			this.TileDrawSource.BeginAction();
			this.actionTool.BeginAction();
		}
		private void UpdateToolAction()
		{
			this.actionTool.UpdateAction();
		}
		private void EndToolAction()
		{
			if (this.actionTool == this.toolNone) return;

			this.actionTool.EndAction();
			this.TileDrawSource.EndAction();

			if (this.activeTilemap != null)
			{
				// Finish the tile update operation, which will fire all the
				// change events that were aggregated for this tool action.
				this.activeTilemap.EndUpdateTiles(0, 0, 0, 0);

				// Since the change events will likely trigger some scene changes,
				// such as renderers updating some cached values or colliders updating
				// the generated shapes, we should re-draw the scene. The easiest way
				// to do that across all CamViews is to trigger a change event for the
				// affected tilemap.
				DualityEditorApp.NotifyObjPropChanged(
					this,
					new ObjectSelection(this.activeTilemap),
					TilemapsReflectionInfo.Property_Tilemap_Tiles);
			}

			this.actionTool = this.toolNone;
			this.actionBeginTile = InvalidTile;
			UndoRedoManager.Finish();
		}
		
		protected override string UpdateStatusText()
		{
			// Display which Tilemap we're currently using
			Tilemap displayedTilemap = this.activeTilemap ?? this.selectedTilemap;
			GameObject tilemapObj = (displayedTilemap != null) ? displayedTilemap.GameObj : null;
			string tilemapName = (tilemapObj != null) ? tilemapObj.FullName : "None";
			string escapedTilemapName = tilemapName.Replace("/", "//");
			return escapedTilemapName;
		}

		protected override void OnEnterState()
		{
			base.OnEnterState();

			// Init the available editor actions, if not done before
			if (this.actions.Count == 0)
			{
				IEditorAction[] editorActions = DualityEditorApp.GetEditorActions(
					typeof(Tilemap), null, TilemapsEditorPlugin.ActionTilemapEditor)
					.ToArray();
				foreach (IEditorAction action in editorActions)
				{
					this.actions.Add(new TilemapActionEntry
					{
						Action = action,
					});
				}
			}

			// Init the available toolset, if not done before
			if (this.tools.Count == 0)
			{
				TilemapTool[] availableTools = DualityEditorApp.GetAvailDualityEditorTypes(typeof(TilemapTool))
					.Where(t => !t.IsAbstract)
					.Select(t => t.CreateInstanceOf() as TilemapTool)
					.NotNull()
					.OrderBy(t => t.SortOrder)
					.ToArray();

				this.toolNone = availableTools.OfType<NoTilemapTool>().FirstOrDefault();
				this.toolSelect = availableTools.OfType<SelectTilemapTool>().FirstOrDefault();

				this.tools.AddRange(availableTools);
				foreach (TilemapTool tool in this.tools)
				{
					tool.Environment = this;
				}
				this.tools.Remove(this.toolNone);

				this.selectedTool = this.tools.FirstOrDefault(t => t != this.toolSelect) ?? this.toolSelect;
				this.activeTool   = this.toolNone;
				this.actionTool   = this.toolNone;
			}

			// Init the custom tile editing toolbar
			{
				this.View.SuspendLayout();
				this.toolstrip = new ToolStrip();
				this.toolstrip.SuspendLayout();

				this.toolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
				this.toolstrip.Name = "toolstrip";
				this.toolstrip.Text = "Tilemap Editor Tools";
				this.toolstrip.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
				this.toolstrip.BackColor = Color.FromArgb(212, 212, 212);

				foreach (TilemapTool tool in this.tools)
				{
					tool.InitToolButton();

					if (tool.ToolButton == null)
						continue;

					tool.ToolButton.Tag = tool.HelpInfo;
					tool.ToolButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
					tool.ToolButton.AutoToolTip = true;
					this.toolstrip.Items.Add(tool.ToolButton);
				}
				for (int i = 0; i < this.actions.Count; i++)
				{
					TilemapActionEntry entry = this.actions[i];

					ToolStripButton button = new ToolStripButton(entry.Action.Name, entry.Action.Icon);
					button.DisplayStyle = (button.Image != null) ? ToolStripItemDisplayStyle.Image : ToolStripItemDisplayStyle.Text;
					button.AutoToolTip = true;
					button.Alignment = ToolStripItemAlignment.Right;
					button.Click += this.actionToolButton_Click;

					string desc = entry.Action.Description;
					if (!string.IsNullOrEmpty(desc))
					{
						button.Tag = HelpInfo.FromText(entry.Action.Name, desc);
					}

					entry.ToolButton = button;
					this.toolstrip.Items.Add(button);
					this.actions[i] = entry;
				}

				this.View.Controls.Add(this.toolstrip);
				this.View.Controls.SetChildIndex(this.toolstrip, this.View.Controls.IndexOf(this.View.ToolbarCamera));
				this.toolstrip.ResumeLayout(true);
				this.View.ResumeLayout(true);
			}

			// Register events
			DualityEditorApp.SelectionChanged += this.DualityEditorApp_SelectionChanged;
			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
			DualityEditorApp.UpdatingEngine += this.DualityEditorApp_UpdatingEngine;
			Scene.Entered += this.Scene_Entered;

			// Initial update
			this.UpdateTilemapToolButtons();
			this.UpdateActionToolButtons();
			this.selectedTilemap = TilemapsEditorSelectionParser.QuerySelectedTilemap();
		}
		protected override void OnLeaveState()
		{
			base.OnLeaveState();

			// Cleanup the custom tile editing toolbar
			{
				foreach (TilemapTool tool in this.tools)
				{
					tool.DisposeToolButton();
				}
				for (int i = 0; i < this.actions.Count; i++)
				{
					TilemapActionEntry entry = this.actions[i];
					if (entry.ToolButton != null)
					{
						entry.ToolButton.Click -= this.actionToolButton_Click;
						entry.ToolButton.Dispose();
						entry.ToolButton = null;
					}
					this.actions[i] = entry;
				}
				this.toolstrip.Dispose();
				this.toolstrip = null;
			}

			// Unregister events
			DualityEditorApp.SelectionChanged -= this.DualityEditorApp_SelectionChanged;
			DualityEditorApp.ObjectPropertyChanged -= this.DualityEditorApp_ObjectPropertyChanged;
			DualityEditorApp.UpdatingEngine -= this.DualityEditorApp_UpdatingEngine;
			Scene.Entered -= this.Scene_Entered;

			// Reset state
			this.Cursor = CursorHelper.Arrow;
		}
		protected override void OnShown()
		{
			base.OnShown();

			// Make sure the tile palette is up and running
			TilemapsEditorPlugin.Instance.PushTilePalette();
		}
		protected override void OnHidden()
		{
			base.OnHidden();

			// Release the tile palette we requested before
			TilemapsEditorPlugin.Instance.PopTilePalette();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			Point2 lastHoverTile = this.hoveredTile;
			
			// If we don't have focus, at least check for modifier-based tool overrides
			if (!this.Focused)
				this.UpdateOverrideToolFromModifierKeys();

			// Determine what the cursor is hovering over and what actions it could perform
			this.UpdateHoverState(e.Location);
			this.UpdateActiveState();

			// If we're performing a continuous action, update it when our hover tile changes
			if (this.hoveredTile != lastHoverTile)
			{
				this.UpdateToolAction();
			}
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.hoveredTile = InvalidTile;
			this.hoveredRenderer = null;
			this.activeTool = this.toolNone;
			this.activeTilemap = null;
			this.activeAreaOrigin = InvalidTile;
			this.activeArea.ResizeClear(0, 0);
			this.UpdateCursor();
			this.Invalidate();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left)
			{
				// If we don't have focus, at least check for modifier-based tool overrides
				if (!this.Focused)
					this.UpdateOverrideToolFromModifierKeys();

				// Because selection events may change the currently active tool,
				// start by agreeing on what tool we're dealing with in this mouse event.
				TilemapTool proposedAction = this.activeTool;

				// If there is no tool active, don't do selection changes or begin an action
				if (proposedAction == this.toolNone) return;

				// If the action preview isn't valid, do the full calculation now
				if (!this.activePreviewValid)
				{
					proposedAction.UpdateActiveArea();
				}

				// If the active tilemap isn't currently selected, perform a selection first
				if (this.selectedTilemap != this.activeTilemap)
				{
					if (this.activeTilemap != null)
						DualityEditorApp.Select(this, new ObjectSelection(this.activeTilemap.GameObj));
					else
						DualityEditorApp.Deselect(this, ObjectSelection.Category.GameObjCmp);
				}

				// Begin a new action with the proposed action tool
				this.BeginToolAction(proposedAction);
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (e.Button == MouseButtons.Left)
			{
				this.EndToolAction();
			}
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			
			// Hotkeys for switching the currently selected tilemap
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
			{
				Tilemap[] visibleTilemaps = 
					this.QueryVisibleTilemapRenderers()
					.OrderBy(r => (r as Component).GameObj.Transform.Pos.Z + r.BaseDepthOffset)
					.Select(r => r.ActiveTilemap)
					.NotNull()
					.Distinct()
					.ToArray();
				int selectedIndex = Array.IndexOf(visibleTilemaps, this.selectedTilemap);

				if (visibleTilemaps.Length > 0)
				{
					if (e.KeyCode == Keys.Down)
						selectedIndex = (selectedIndex == -1) ? (visibleTilemaps.Length - 1) : Math.Min(selectedIndex + 1, visibleTilemaps.Length - 1);
					else if (e.KeyCode == Keys.Up)
						selectedIndex = (selectedIndex == -1) ? 0 : Math.Max(selectedIndex - 1, 0);

					Tilemap newSelection = visibleTilemaps[selectedIndex];
					DualityEditorApp.Select(this, new ObjectSelection(newSelection.GameObj));
				}

				e.Handled = true;
				return;
			}
			else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
			{
				DualityEditorApp.Deselect(this, ObjectSelection.Category.GameObjCmp);
				e.Handled = true;
				return;
			}

			// Check for tool-related keys
			foreach (TilemapTool tool in this.tools)
			{
				if (tool.OverrideKey == e.KeyCode)
				{
					this.OverrideTool = tool;
					e.Handled = true;
					break;
				}
				else if (Control.ModifierKeys == Keys.None && tool.ShortcutKey == e.KeyCode)
				{
					this.SelectedTool = tool;
					e.Handled = true;
					break;
				}
			}
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			
			if (this.overrideTool != null && this.overrideTool.OverrideKey == e.KeyCode)
			{
				this.OverrideTool = null;
				e.Handled = true;
			}
		}
		protected override void OnLostFocus()
		{
			base.OnLostFocus();
			this.OverrideTool = null;
		}
		protected override void OnCamActionRequiresCursorChanged(EventArgs e)
		{
			base.OnCamActionRequiresCursorChanged(e);
			this.OnMouseMove();
		}
		
		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);

			DataObject data = e.Data as DataObject;
			if (new ConvertOperation(data, ConvertOperation.Operation.All).CanPerform<Tileset>())
				e.Effect = e.AllowedEffect;
			else
				e.Effect = DragDropEffects.None;
		}
		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);
			
			DataObject data = e.Data as DataObject;
			var dragObjQuery = new ConvertOperation(data, ConvertOperation.Operation.All).Perform<Tileset>();
			if (dragObjQuery != null)
			{
				Tileset dragObj = dragObjQuery.FirstOrDefault();

				// Open up the Tilemap setup dialog
				TilemapSetupDialog setupDialog = new TilemapSetupDialog();
				setupDialog.Tileset = dragObj;
				setupDialog.ShowCentered(DualityEditorApp.MainForm);

				e.Effect = e.AllowedEffect;
			}
		}

		protected override void OnRenderState()
		{
			// Determine whether one specific Tilemap is highlighted
			Tilemap highlightTilemap = null;
			if (this.activeTool != this.toolNone && this.activeTool != this.toolSelect && this.activeTilemap != null)
				highlightTilemap = this.activeTilemap;
			else
				highlightTilemap = this.selectedTilemap;

			// "Grey out" all non-highlighted Tilemap Renderers
			Dictionary<ICmpTilemapRenderer,ColorRgba> oldColors = null;
			if (highlightTilemap != null)
			{
				IEnumerable<ICmpTilemapRenderer> tilemapRenderers = Scene.Current.FindComponents<ICmpTilemapRenderer>();

				// Determine the base depth of the currently highlighted Tilemap renderer.
				float highlightBaseDepth = float.MinValue;
				ICmpTilemapRenderer highlightRenderer = 
					this.activeRenderer ?? 
					tilemapRenderers.FirstOrDefault(r => r.ActiveTilemap == highlightTilemap);
				if (highlightRenderer != null)
				{
					Transform highlightTransform = (highlightRenderer as Component).GameObj.Transform;
					highlightBaseDepth = highlightTransform.Pos.Z + highlightRenderer.BaseDepthOffset;
				}

				// Temporarily reconfigure all tilemap renderers in the scene.
				foreach (ICmpTilemapRenderer renderer in tilemapRenderers)
				{
					if (renderer.ActiveTilemap == highlightTilemap) continue;

					// Backup the old color tint
					if (oldColors == null)
						oldColors = new Dictionary<ICmpTilemapRenderer,ColorRgba>();
					oldColors[renderer] = renderer.ColorTint;

					// Determine the base depth of the tilemap renderer for comparison
					Transform transform = (renderer as Component).GameObj.Transform;
					float baseDepth = transform.Pos.Z + renderer.BaseDepthOffset;
					bool isBelowHightlighted = baseDepth > highlightBaseDepth;

					// If the renderer is below the active one, darken it.
					if (isBelowHightlighted)
						renderer.ColorTint = ColorRgba.Lerp(renderer.ColorTint, this.BgColor, 0.5f).WithAlpha(renderer.ColorTint.A);
					// If the renderer is above the active one, make it transparent.
					else
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
		protected override void OnCollectStateWorldOverlayDrawcalls(Canvas canvas)
		{
			base.OnCollectStateWorldOverlayDrawcalls(canvas);

			ICmpTilemapRenderer[] visibleRenderers = this.QueryVisibleTilemapRenderers().ToArray();
			for (int i = 0; i < visibleRenderers.Length; i++)
			{
				ICmpTilemapRenderer renderer = visibleRenderers[i];
				Component component = renderer as Component;
				Transform transform = component.GameObj.Transform;

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
				canvas.State.ColorTint = ColorRgba.White.WithAlpha(greyOut ? 0.33f : 1.0f);
				canvas.DrawRect(
					transform.Pos.X, 
					transform.Pos.Y, 
					transform.Pos.Z,
					localRect.W, 
					localRect.H);
				
				// Highlight source tiles when available
				if (this.TileDrawSource.SourceTilemap == renderer.ActiveTilemap)
				{
					float intensity = (this.selectedTilemap == this.TileDrawSource.SourceTilemap) ? 1.0f : 0.5f;
					DrawTileHighlights(
						canvas, 
						renderer, 
						this.TileDrawSource.SourceOrigin,
						this.TileDrawSource.SourceShape, 
						ColorRgba.White.WithAlpha(intensity),
						ColorRgba.White.WithAlpha(intensity), 
						TileHighlightMode.Selection);
				}

				// Highlight the currently active tiles
				if (this.activeTilemap == renderer.ActiveTilemap)
				{
					// Fade-in the affected area for the fill tool to prevent visual noise when hovering around
					float outlineIntensity = 1.0f;
					float fillIntensity = 1.0f;
					if (this.activeTool.FadeInPreviews && this.activePreviewValid)
					{
						float timeSinceFillSelect = (float)(DateTime.Now - this.activePreviewTime).TotalMilliseconds;
						fillIntensity = MathF.Clamp(timeSinceFillSelect / FillAnimDuration, 0.0f, 1.0f);
						outlineIntensity = 0.25f + 0.75f * MathF.Clamp(timeSinceFillSelect / FillAnimDuration, 0.0f, 1.0f);
					}

					// Draw the current tile hightlights
					DrawTileHighlights(
						canvas, 
						renderer, 
						this.activeAreaOrigin, 
						this.activeArea, 
						ColorRgba.White.WithAlpha(fillIntensity),
						ColorRgba.White.WithAlpha(outlineIntensity),
						this.activePreviewValid ? TileHighlightMode.Normal : TileHighlightMode.Uncertain,
						this.activeAreaOutlines);
				}
			}
		}

		private void DualityEditorApp_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.SameObjects) return;
			if (!e.AffectedCategories.HasFlag(ObjectSelection.Category.GameObjCmp))
				return;

			// Tilemap selection changed
			Tilemap newSelection = TilemapsEditorSelectionParser.QuerySelectedTilemap();
			if (this.selectedTilemap != newSelection)
			{
				this.selectedTilemap = newSelection;
				if (this.Mouseover)
					this.OnMouseMove();
				this.Invalidate();
				this.UpdateActionToolButtons();
			}
		}
		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (e.HasProperty(TilemapsReflectionInfo.Property_Tilemap_Tiles))
			{
				this.Invalidate();
			}
			else if (e.HasObject(Scene.Current))
			{
				this.UpdateActionToolButtons();
			}
		}
		private void DualityEditorApp_UpdatingEngine(object sender, EventArgs e)
		{
			// When using the fill tool fade-in, we'll need continuous updates until the animation is done
			if (this.activeTool.FadeInPreviews && this.activePreviewValid)
			{
				float timeSinceFillSelect = (float)(DateTime.Now - this.activePreviewTime).TotalMilliseconds;
				if (timeSinceFillSelect <= FillAnimDuration)
					this.Invalidate();
			}
		}
		private void Scene_Entered(object sender, EventArgs e)
		{
			this.UpdateActionToolButtons();
		}
		private void actionToolButton_Click(object sender, EventArgs e)
		{
			TilemapActionEntry clickedEntry = this.actions.FirstOrDefault(entry => entry.ToolButton == sender);
			if (clickedEntry.Action != null)
			{
				Tilemap[] actionTilemaps = this.QueryActionTilemaps().ToArray();
				clickedEntry.Action.Perform(actionTilemaps);
			}
		}

		public override HelpInfo ProvideHoverHelp(Point localPos, ref bool captured)
		{
			if (this.activeTool.HelpInfo != null)
				return this.activeTool.HelpInfo;
			else if (this.Focused)
				return HelpInfo.FromText(TilemapsRes.CamView_Help_TilemapEditorActions, 
					TilemapsRes.CamView_Help_TilemapEditor_SelectTilemaps);
			else
				return base.ProvideHoverHelp(localPos, ref captured);
		}

		void ITilemapToolEnvironment.SubmitActiveAreaChanges(bool isFullPreview)
		{
			this.activePreviewTime = DateTime.Now;
			this.activePreviewValid = isFullPreview;
		}
		void ITilemapToolEnvironment.PerformEditTiles(EditTilemapActionType actionType, Tilemap tilemap, Point2 pos, Grid<bool> brush, ITileDrawSource source, Point2 sourceOffset)
		{
			Grid<Tile> drawPatch = new Grid<Tile>(brush.Width, brush.Height);
			source.FillTarget(drawPatch, sourceOffset);

			UndoRedoManager.Do(new EditTilemapAction(
				tilemap, 
				actionType, 
				pos, 
				drawPatch,
				brush));
		}

		[Flags]
		private enum TileHighlightMode
		{
			Normal    = 0x0,
			Selection = 0x1,
			Uncertain = 0x2
		}
		private static void DrawTileHighlights(Canvas canvas, ICmpTilemapRenderer renderer, Point2 origin, IReadOnlyGrid<bool> highlight, ColorRgba fillTint, ColorRgba outlineTint, TileHighlightMode mode, List<Vector2[]> outlineCache = null)
		{
			if (highlight.Width == 0 || highlight.Height == 0) return;

			// Generate strippled line texture if not available yet
			if (strippledLineTex == null)
			{
				PixelData pixels = new PixelData(8, 1);
				for (int i = 0; i < pixels.Width / 2; i++)
					pixels[i, 0] = ColorRgba.White;
				for (int i = pixels.Width / 2; i < pixels.Width; i++)
					pixels[i, 0] = ColorRgba.TransparentWhite;

				using (Pixmap pixmap = new Pixmap(pixels))
				{
					strippledLineTex = new Texture(pixmap, 
						TextureSizeMode.Default, 
						TextureMagFilter.Nearest, 
						TextureMinFilter.Nearest, 
						TextureWrapMode.Repeat, 
						TextureWrapMode.Repeat, 
						TexturePixelFormat.Rgba);
				}
			}

			BatchInfo defaultMaterial = new BatchInfo(DrawTechnique.Alpha, canvas.State.Material.MainColor);
			BatchInfo strippleMaterial = new BatchInfo(DrawTechnique.Alpha, canvas.State.Material.MainColor, strippledLineTex);
			bool uncertain = (mode & TileHighlightMode.Uncertain) != 0;
			bool selection = (mode & TileHighlightMode.Selection) != 0;
			
			Component component = renderer as Component;
			Transform transform = component.GameObj.Transform;
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

				// Fill all highlighted tiles that are currently visible
				{
					canvas.State.SetMaterial(defaultMaterial);
					canvas.State.ColorTint = fillTint * ColorRgba.White.WithAlpha(selection ? 0.2f : 0.375f);
				
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
					{
						Point2 tileGridPos = cullingOut.VisibleTileStart;
						Vector2 renderStartPos = worldOriginPos + tileGridPos.X * tileSize.X * worldAxisX + tileGridPos.Y * tileSize.Y * worldAxisY;;
						Vector2 renderPos = renderStartPos;
						Vector2 tileXStep = worldAxisX * tileSize.X;
						Vector2 tileYStep = worldAxisY * tileSize.Y;
						int lineMergeCount = 0;
						int totalRects = 0;
						for (int tileIndex = 0; tileIndex < renderedTileCount; tileIndex++)
						{
							bool current = highlight[tileGridPos.X, tileGridPos.Y];
							if (current)
							{
								// Try to merge consecutive rects in the same line to reduce drawcalls / CPU load
								bool hasNext = (tileGridPos.X + 1 < highlight.Width) && ((tileGridPos.X + 1 - cullingOut.VisibleTileStart.X) < cullingOut.VisibleTileCount.X);
								bool next = hasNext ? highlight[tileGridPos.X + 1, tileGridPos.Y] : false;
								if (next)
								{
									lineMergeCount++;
								}
								else
								{
									totalRects++;
									canvas.FillRect(
										transform.Pos.X + renderPos.X - lineMergeCount * tileXStep.X, 
										transform.Pos.Y + renderPos.Y - lineMergeCount * tileXStep.Y, 
										transform.Pos.Z,
										tileSize.X * (1 + lineMergeCount), 
										tileSize.Y);
									lineMergeCount = 0;
								}
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
				}

				// Draw highlight area outlines, unless flagged as uncertain
				if (!uncertain)
				{
					// Determine the outlines of individual highlighted tile patches
					if (outlineCache == null) outlineCache = new List<Vector2[]>();
					if (outlineCache.Count == 0)
					{
						GetTileAreaOutlines(highlight, tileSize, ref outlineCache);
					}

					// Draw outlines around all highlighted tile patches
					canvas.State.SetMaterial(selection ? strippleMaterial : defaultMaterial);
					canvas.State.ColorTint = outlineTint;
					foreach (Vector2[] outline in outlineCache)
					{
						// For strippled-line display, determine total length of outline
						if (selection)
						{
							float totalLength = 0.0f;
							for (int i = 1; i < outline.Length; i++)
							{
								totalLength += (outline[i - 1] - outline[i]).Length;
							}
							canvas.State.TextureCoordinateRect = new Rect(totalLength / strippledLineTex.PixelWidth, 1.0f);
						}

						// Draw the outline
						canvas.DrawPolygon(
							outline,
							transform.Pos.X + worldOriginPos.X, 
							transform.Pos.Y + worldOriginPos.Y, 
							transform.Pos.Z);
					}
				}

				// If this is an uncertain highlight, i.e. not actually reflecting the represented action,
				// draw a gizmo to indicate this for the user.
				if (uncertain)
				{
					Vector2 highlightSize = new Vector2(highlight.Width * tileSize.X, highlight.Height * tileSize.Y);
					Vector2 highlightCenter = highlightSize * 0.5f;

					Vector3 circlePos = transform.Pos + new Vector3(worldOriginPos + worldAxisX * highlightCenter + worldAxisY * highlightCenter);
					float circleRadius = MathF.Min(tileSize.X, tileSize.Y) * 0.2f;

					canvas.State.SetMaterial(defaultMaterial);
					canvas.State.ColorTint = outlineTint;
					canvas.FillCircle(
						circlePos.X,
						circlePos.Y,
						circlePos.Z,
						circleRadius);
				}
			}
			canvas.PopState();
		}
		private static void GetTileAreaOutlines(IReadOnlyGrid<bool> tileArea, Vector2 tileSize, ref List<Vector2[]> outlines)
		{
			// Initialize the container we'll put our outlines into
			if (outlines == null)
				outlines = new List<Vector2[]>();
			else
				outlines.Clear();

			// Generate a data structure containing all visible edges
			TileEdgeMap edgeMap = new TileEdgeMap(tileArea.Width + 1, tileArea.Height + 1);
			for (int y = 0; y < edgeMap.Height; y++)
			{
				for (int x = 0; x < edgeMap.Width; x++)
				{
					// Determine highlight state of the four tiles around this node
					bool topLeft     = x > 0              && y > 0               && tileArea[x - 1, y - 1];
					bool topRight    = x < tileArea.Width && y > 0               && tileArea[x    , y - 1];
					bool bottomLeft  = x > 0              && y < tileArea.Height && tileArea[x - 1, y    ];
					bool bottomRight = x < tileArea.Width && y < tileArea.Height && tileArea[x    , y    ];

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

				// Close the loop by adding the first element again
				if (outlineBuilder.Count > 0)
					outlineBuilder.Add(outlineBuilder[0]);

				// If we have enough vertices, keep the outline for drawing
				Vector2[] outline = new Vector2[outlineBuilder.Count];
				outlineBuilder.CopyTo(outline, 0);
				outlines.Add(outline);

				// Reset the outline builder to an empty state
				outlineBuilder.Clear();
			}
		}
	}
}
