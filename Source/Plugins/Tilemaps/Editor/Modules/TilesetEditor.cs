using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;

using Duality.Plugins.Tilemaps;
using Duality.Editor.Controls.ToolStrip;
using Duality.Editor.Plugins.Tilemaps.TilesetEditorModes;
using Duality.Editor.Plugins.Tilemaps.UndoRedoActions;
using Duality.Editor.Plugins.Tilemaps.Properties;

using WeifenLuo.WinFormsUI.Docking;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace Duality.Editor.Plugins.Tilemaps
{
	/// <summary>
	/// Allows editing the properties of a <see cref="Tileset"/> that can't be accessed easily in
	/// a regular <see cref="AdamsLair.WinForms.PropertyEditing.PropertyGrid"/>.
	/// </summary>
	public partial class TilesetEditor : DockContent, IHelpProvider
	{
		private struct EditorModeInfo
		{
			public TilesetEditorMode Mode;
			public ToolStripButton ToolButton;
		}

		private const float         ZoomInStep     = 1.25f;
		private const float         ZoomOutStep    = 1.0f / ZoomInStep;

		private TilesetEditorMode   activeMode     = null;
		private EditorModeInfo[]    availableModes = null;
		private ContentRef<Tileset> backupTarget   = null;
		private Tileset             tilesetBackup  = null;
		private bool                applyRequired  = false;
		private TilesetView.TileIndexDrawMode tileIndexDrawMode = TilesetView.TileIndexDrawMode.Hovering;


		/// <summary>
		/// [GET] The currently selected <see cref="Tileset"/> in this editor. This property
		/// is dependent on and automatically set by editor-wide selection events.
		/// </summary>
		public ContentRef<Tileset> SelectedTileset
		{
			get { return this.tilesetView.TargetTileset; }
		}
		internal TilesetView TilesetView
		{
			get { return this.tilesetView; }
		}
		private int SelectedLayerIndex
		{
			get { return this.layerView.SelectedNode == null ? -1 : this.layerView.SelectedNode.Index; }
		}
		private bool ApplyRequired
		{
			get { return this.applyRequired; }
			set
			{
				if (this.applyRequired != value)
				{
					this.applyRequired = value;
					this.buttonApply.Enabled = this.applyRequired;
					this.buttonRevert.Enabled = this.applyRequired;
					this.labelRequireApply.Visible = this.applyRequired;
				}
			}
		}


		public TilesetEditor()
		{
			this.InitializeComponent();
			this.treeColumnMain.DrawColHeaderBg += this.treeColumnMain_DrawColHeaderBg;
			this.toolStripModeSelect.Renderer = new DualitorToolStripProfessionalRenderer();
			this.toolStripEdit.Renderer = new DualitorToolStripProfessionalRenderer();

			// Initial resize event to apply a proper size to the layer view main column
			this.layerView_Resize(this, EventArgs.Empty);
			UpdateZoomButtons();
		}
		
		internal void SaveUserData(XElement node)
		{
			node.SetElementValue("DarkBackground", this.buttonBrightness.Checked);
			node.SetElementValue("DisplayTileIndices", this.tileIndexDrawMode);
		}
		internal void LoadUserData(XElement node)
		{
			bool tryParseBool;
			TilesetView.TileIndexDrawMode tryParseTileIndices;

			if (node.GetElementValue("DarkBackground", out tryParseBool)) this.buttonBrightness.Checked = tryParseBool;
			if (node.GetElementValue("DisplayTileIndices", out tryParseTileIndices)) this.tileIndexDrawMode = tryParseTileIndices;

			this.ApplyBrightness();
			this.ApplyTileIndexDrawMode();
		}

		private void SetActiveEditorMode(TilesetEditorMode mode)
		{
			UndoRedoManager.Finish();

			// Reset the layer view selection
			this.layerView.SelectedNode = null;

			if (this.activeMode != null)
				this.activeMode.RaiseOnLeave();

			this.activeMode = mode;

			// Assign the new data model to the layer view
			this.layerView.Model = (this.activeMode != null) ? this.activeMode.LayerModel : null;

			if (this.activeMode != null)
				this.activeMode.RaiseOnEnter();

			// Update tool buttons so the appropriate one is checked
			for (int i = 0; i < this.availableModes.Length; i++)
			{
				EditorModeInfo info = this.availableModes[i];
				info.ToolButton.Checked = (info.Mode == mode);
			}

			// Update Add / Remove layer buttons
			bool canAddRemove = (this.activeMode != null) ? 
				this.activeMode.AllowLayerEditing.HasFlag(LayerEditingCaps.AddRemove) : 
				false;
			this.buttonAddLayer.Visible = canAddRemove;
			this.buttonRemoveLayer.Visible = canAddRemove;
			this.buttonRemoveLayer.Enabled = (this.layerView.SelectedNode != null);

			// Different editor modes might allow or disallow drawing tile indices
			this.ApplyTileIndexDrawMode();

			// Invalidate TilesetView because editor modes are likely to
			// draw custom overlays using its event handlers. Directly show
			// the effect of having a different editing mode selected.
			this.tilesetView.Invalidate();
		}
		internal void SetSelectedLayer(object layerViewTag)
		{
			this.layerView.SelectedNode = this.layerView
				.Root
				.Children
				.FirstOrDefault(v => v.Tag == layerViewTag);
		}
		private void SetSelectedLayer(int layerIndex)
		{
			layerIndex = MathF.Clamp(layerIndex, 0, this.layerView.Root.Children.Count - 1);
			if (layerIndex != -1)
				this.layerView.SelectedNode = this.layerView.Root.Children[layerIndex];
			else
				this.layerView.SelectedNode = null;
		}
		private void ApplyGlobalTilesetSelection(SelectionChangeReason changeReason)
		{
			Tileset tileset = TilemapsEditorSelectionParser.QuerySelectedTileset().Res;
			if (this.tilesetView.TargetTileset != tileset)
			{
				TilesetSelectionChangedEventArgs args = new TilesetSelectionChangedEventArgs(
					this.tilesetView.TargetTileset,
					tileset,
					changeReason);
				this.OnTilesetSelectionChanging(args);
				this.tilesetView.TargetTileset = tileset;
				this.tilesetView.ResetZoom();
				this.UpdateZoomButtons();
				this.OnTilesetSelectionChanged(args);
			}
		}
		private void ApplyBrightness()
		{
			bool darkMode = this.buttonBrightness.Checked;
			this.tilesetView.BackColor = darkMode ? Color.FromArgb(64, 64, 64) : Color.FromArgb(192, 192, 192);
			this.tilesetView.ForeColor = darkMode ? Color.FromArgb(255, 255, 255) : Color.FromArgb(0, 0, 0);
		}
		private void ApplyTileIndexDrawMode()
		{
			bool allowIndices = (this.activeMode != null) ? this.activeMode.AllowTileIndexDisplay : true;

			this.tilesetView.DrawTileIndices = allowIndices ? this.tileIndexDrawMode : TilesetView.TileIndexDrawMode.Never;
			this.buttonDrawTileIndices.Enabled = allowIndices;
			switch (this.tileIndexDrawMode)
			{
				case TilesetView.TileIndexDrawMode.Never:
					this.buttonDrawTileIndices.Image = TilemapsResCache.IconHideIndices;
					break;
				case TilesetView.TileIndexDrawMode.Hovering:
					this.buttonDrawTileIndices.Image = TilemapsResCache.IconRevealIndices;
					break;
				case TilesetView.TileIndexDrawMode.Always:
					this.buttonDrawTileIndices.Image = TilemapsResCache.IconShowIndices;
					break;
			}
		}
		private void UpdateZoomButtons()
		{
			this.buttonZoomIn.Enabled = (this.tilesetView.ZoomLevelTag != TilesetView.ZoomLevel.Max);
			this.buttonZoomOut.Enabled = (this.tilesetView.ZoomLevelTag != TilesetView.ZoomLevel.Min);
			this.buttonZoomDefault.Enabled = (this.tilesetView.ZoomLevelTag != TilesetView.ZoomLevel.Default);
		}
		
		private void StartRecordTilesetChanges()
		{
			// Copy the selected tileset's original settings to our local backup
			Tileset tileset = this.SelectedTileset.Res;
			if (tileset != null)
			{
				if (this.tilesetBackup == null)
					this.tilesetBackup = new Tileset();
				tileset.CopyTo(this.tilesetBackup);
				this.backupTarget = this.SelectedTileset;
			}
			else
			{
				this.backupTarget = null;
			}
		}
		private void ApplyTilesetChanges()
		{
			int selectedLayerIndex = this.SelectedLayerIndex;

			UndoRedoManager.Do(new ApplyTilesetChangesAction(
				this.SelectedTileset.Res, 
				this.tilesetBackup));

			this.CleanupAfterApplyRevert();
			this.SetSelectedLayer(selectedLayerIndex);
		}
		private void ResetTilesetChanges()
		{
			int selectedLayerIndex = this.SelectedLayerIndex;

			UndoRedoManager.Do(new RevertTilesetChangesAction(
				this.SelectedTileset.Res, 
				this.tilesetBackup));

			this.CleanupAfterApplyRevert();
			this.SetSelectedLayer(selectedLayerIndex);
		}
		private void CleanupAfterApplyRevert()
		{
			// We've handed our backup over to the Undo operation.
			// Don't keep it around here, so we don't accidentally
			// introduce changes to it.
			this.tilesetBackup = null;

			this.ApplyRequired = false;
			this.StartRecordTilesetChanges();

			this.tilesetView.Invalidate();
			if (this.activeMode != null)
				this.activeMode.RaiseOnApplyRevert();
		}
		private bool AskApplyOrResetTilesetChanges(bool allowCancel)
		{
			if (!this.applyRequired) return true;
			if (this.backupTarget == null) return true;

			DialogResult result = MessageBox.Show(
				this,
				string.Format(TilemapsRes.Msg_ApplyOrResetTilesetChanges_Text, this.backupTarget.Name),
				TilemapsRes.Msg_ApplyOrResetTilesetChanges_Caption,
				allowCancel ? MessageBoxButtons.YesNoCancel : MessageBoxButtons.YesNo);

			if (result == DialogResult.Yes)
				this.ApplyTilesetChanges();
			else if (result == DialogResult.No)
				this.ResetTilesetChanges();
			else if (result == DialogResult.Cancel)
				return false;

			return true;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			// Initialize the available editor modes and generate their toolbuttons
			TilesetEditorMode[] modeInstances = DualityEditorApp.GetAvailDualityEditorTypes(typeof(TilesetEditorMode))
				.Where(t => !t.IsAbstract)
				.Select(t => t.CreateInstanceOf() as TilesetEditorMode)
				.NotNull()
				.OrderBy(t => t.SortOrder)
				.ToArray();
			this.availableModes = new EditorModeInfo[modeInstances.Length];
			for (int i = 0; i < this.availableModes.Length; i++)
			{
				TilesetEditorMode mode = modeInstances[i];
				mode.Init(this);

				ToolStripButton modeItem = new ToolStripButton(mode.Name, mode.Icon);
				modeItem.AutoToolTip = false;
				modeItem.ToolTipText = null;
				modeItem.Tag = HelpInfo.FromText(mode.Name, mode.Description);
				modeItem.Click += this.modeToolButton_Click;

				this.toolStripModeSelect.Items.Add(modeItem);
				this.availableModes[i] = new EditorModeInfo
				{
					Mode = mode,
					ToolButton = modeItem
				};
			}

			// If we have at least one mode available, activate it
			if (this.availableModes.Length > 0)
			{
				this.SetActiveEditorMode(this.availableModes[0].Mode);
			}

			// Subscribe for global editor events
			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
			DualityEditorApp.SelectionChanged      += this.DualityEditorApp_SelectionChanged;

			// Apply editor-global tileset selection
			this.ApplyGlobalTilesetSelection(SelectionChangeReason.Unknown);

			// Start recording tileset changes for Apply / Revert support
			this.StartRecordTilesetChanges();
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			// Leave the currently active editor mode so it can shut down properly
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated)
			{
				this.SetActiveEditorMode(null);
			}

			DualityEditorApp.ObjectPropertyChanged -= this.DualityEditorApp_ObjectPropertyChanged;
			DualityEditorApp.SelectionChanged      -= this.DualityEditorApp_SelectionChanged;
		}
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			// Only ask for confirmation if the user is closing the Tileset editor
			// itself - not when the Duality editor as a whole is closed or killed.
			if (e.CloseReason == CloseReason.UserClosing)
			{
				bool cancel = !this.AskApplyOrResetTilesetChanges(true);
				if (cancel) e.Cancel = true;
			}
		}
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			// When getting focus, fake a selection event so we'll get back inspector 
			// focus, should that be desired by the active editing mode.
			this.layerView_SelectionChanged(this, EventArgs.Empty);
		}
		private void OnTilesetSelectionChanging(TilesetSelectionChangedEventArgs args)
		{
			// When switching to a different tileset, either apply or revert what we did to the current one
			if (args.ChangeReason != SelectionChangeReason.ObjectDisposing)
				this.AskApplyOrResetTilesetChanges(false);
		}
		private void OnTilesetSelectionChanged(TilesetSelectionChangedEventArgs args)
		{
			Tileset nextTileset = args.Next.Res;

			this.StartRecordTilesetChanges();
			
			// Update the label that tells us which tileset is selected
			this.labelSelectedTileset.Text = (nextTileset != null) ? 
				string.Format(TilemapsRes.TilesetEditor_SelectedTileset, nextTileset.Name) : 
				TilemapsRes.TilesetEditor_NoTilesetSelected;

			// Update the displayed visual layer index
			if (nextTileset == null)
				this.tilesetView.DisplayedConfigIndex = 0;
			else if (nextTileset.RenderConfig.Count <= this.tilesetView.DisplayedConfigIndex)
				this.tilesetView.DisplayedConfigIndex = 0;

			// Inform the currently active editing mode of this change
			if (this.activeMode != null)
				this.activeMode.RaiseOnTilesetSelectionChanged(args);

			// Update the enabled state of Add and Remove buttons
			this.buttonAddLayer.Enabled = (nextTileset != null);
			this.buttonRemoveLayer.Enabled = (nextTileset != null && this.layerView.SelectedNode != null);
		}
		
		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			// If we changed something about our selected tileset, update the UI to reflect that
			Tileset tileset = this.SelectedTileset.Res;
			if (tileset != null)
			{
				bool affectsTileset = e.HasObject(tileset);
				bool affectsConfigLayer = 
					e.HasAnyObject(tileset.RenderConfig) || 
					e.HasAnyObject(tileset.AutoTileConfig) || 
					e.HasProperty(TilemapsReflectionInfo.Property_Tileset_RenderConfig) || 
					e.HasProperty(TilemapsReflectionInfo.Property_Tileset_AutoTileConfig);
				if (affectsTileset || affectsConfigLayer)
				{
					if (this.activeMode != null)
						this.activeMode.RaiseOnTilesetModified(e);

					this.ApplyRequired = tileset.HasChangedSinceCompile;
				}
			}

			// If the user changed the assigned Tileset of a Tilemap present in the current Scene,
			// we'll need to update the implicit Tileset selection for the palette.
			if (e.Objects.ComponentCount > 0 &&
				e.HasProperty(TilemapsReflectionInfo.Property_Tilemap_Tileset) &&
				e.Objects.Components.OfType<Tilemap>().Any())
			{
				this.ApplyGlobalTilesetSelection(SelectionChangeReason.Unknown);
			}
		}
		private void DualityEditorApp_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.SameObjects) return;
			this.ApplyGlobalTilesetSelection(e.ChangeReason);
		}
		
		private void treeColumnMain_DrawColHeaderBg(object sender, DrawColHeaderBgEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(this.layerView.BackColor), e.Bounds);
			e.Handled = true;
		}
		private void buttonBrightness_CheckedChanged(object sender, EventArgs e)
		{
			this.ApplyBrightness();
		}
		private void buttonDrawTileIndices_Click(object sender, EventArgs e)
		{
			this.tileIndexDrawMode = (TilesetView.TileIndexDrawMode)(((int)this.tileIndexDrawMode + 1) % 3);
			this.ApplyTileIndexDrawMode();
		}
		private void modeToolButton_Click(object sender, EventArgs e)
		{
			EditorModeInfo info = this.availableModes.First(i => i.ToolButton == sender);
			this.SetActiveEditorMode(info.Mode);
		}
		private void buttonApply_Click(object sender, EventArgs e)
		{
			this.ApplyTilesetChanges();
		}
		private void buttonRevert_Click(object sender, EventArgs e)
		{
			this.ResetTilesetChanges();
		}
		private void buttonAddLayer_Click(object sender, EventArgs e)
		{
			if (this.activeMode != null)
				this.activeMode.AddLayer();
		}
		private void buttonRemoveLayer_Click(object sender, EventArgs e)
		{
			int selectedIndex = this.layerView.SelectedNode != null ? this.layerView.SelectedNode.Index : -1;

			if (this.activeMode != null)
				this.activeMode.RemoveLayer();

			if (selectedIndex != -1 && this.layerView.Root.Children.Count > 0)
			{
				selectedIndex = MathF.Clamp(selectedIndex - 1, 0, this.layerView.Root.Children.Count - 1);
				this.layerView.SelectedNode = this.layerView.Root.Children[selectedIndex];
			}
		}
		private void buttonZoomIn_Click(object sender, EventArgs e)
		{
			this.tilesetView.TileSizeFactor *= ZoomInStep;
			this.UpdateZoomButtons();
		}
		private void buttonZoomOut_Click(object sender, EventArgs e)
		{
			this.tilesetView.TileSizeFactor *= ZoomOutStep;
			this.UpdateZoomButtons();
		}
		private void buttonZoomDefault_Click(object sender, EventArgs e)
		{
			this.tilesetView.ResetZoom();
			this.UpdateZoomButtons();
		}
		private void layerView_SelectionChanged(object sender, EventArgs e)
		{
			TreeNodeAdv viewNode = this.layerView.SelectedNode;
			LayerSelectionChangedEventArgs args = new LayerSelectionChangedEventArgs(viewNode);

			this.buttonRemoveLayer.Enabled = (viewNode != null);

			if (this.activeMode != null)
				this.activeMode.RaiseOnLayerSelectionChanged(args);
		}
		private void layerView_Resize(object sender, EventArgs e)
		{
			this.treeColumnMain.Width = this.layerView.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 5;
		}
		private void layerView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete && e.Modifiers == Keys.None)
			{
				bool canAddRemove = this.activeMode.AllowLayerEditing.HasFlag(LayerEditingCaps.AddRemove);
				if (this.activeMode != null && canAddRemove)
					this.activeMode.RemoveLayer();
			}
		}
		private void layerView_Enter(object sender, EventArgs e)
		{
			// When getting focus, fake a selection event so we'll get back inspector 
			// focus, should that be desired by the active editing mode.
			this.layerView_SelectionChanged(this, EventArgs.Empty);
		}

		HelpInfo IHelpProvider.ProvideHoverHelp(Point localPos, ref bool captured)
		{
			Point globalPos = this.PointToScreen(localPos);
			object hoveredObj = null;

			// Retrieve the currently hovered / active item from all child toolstrips
			ToolStripItem hoveredItem = this.GetHoveredToolStripItem(globalPos, out captured);
			hoveredObj = (hoveredItem != null) ? hoveredItem.Tag : null;
			if (hoveredObj is HelpInfo)
				return hoveredObj as HelpInfo;

			// If the currently active editing mode can provide help, ask for it
			if (this.activeMode is IHelpProvider)
			{
				HelpInfo modeHelp = (this.activeMode as IHelpProvider).ProvideHoverHelp(localPos, ref captured);
				if (modeHelp != null)
					return modeHelp;
			}

			return null;
		}
	}
}
