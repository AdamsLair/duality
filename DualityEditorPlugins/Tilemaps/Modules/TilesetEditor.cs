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

		
		private TilesetEditorMode activeMode       = null;
		private EditorModeInfo[]  availableModes   = null;


		/// <summary>
		/// [GET] The currently selected <see cref="Tileset"/> in this editor. This property
		/// is dependent on and automatically set by editor-wide selection events.
		/// </summary>
		public ContentRef<Tileset> SelectedTileset
		{
			get { return this.tilesetView.TargetTileset; }
			private set
			{
				if (this.tilesetView.TargetTileset != value)
				{
					this.tilesetView.TargetTileset = value;
					this.OnTilesetSelectionChanged();
				}
			}
		}


		public TilesetEditor()
		{
			this.InitializeComponent();
			this.toolStripModeSelect.Renderer = new DualitorToolStripProfessionalRenderer();
			this.toolStripEdit.Renderer = new DualitorToolStripProfessionalRenderer();
		}
		
		internal void SaveUserData(XElement node)
		{
			node.SetElementValue("DarkBackground", this.buttonBrightness.Checked);
		}
		internal void LoadUserData(XElement node)
		{
			bool tryParseBool;

			if (node.GetElementValue("DarkBackground", out tryParseBool)) this.buttonBrightness.Checked = tryParseBool;

			this.ApplyBrightness();
		}

		private void SetActiveEditorMode(TilesetEditorMode mode)
		{
			// Reset the layer view selection
			this.layerView.SelectedNode = null;

			if (this.activeMode != null)
				this.activeMode.RaiseOnLeave();

			this.activeMode = mode;

			if (this.activeMode != null)
				this.activeMode.RaiseOnEnter();

			this.layerView.Model = (this.activeMode != null) ? this.activeMode.LayerModel : null;
			for (int i = 0; i < this.availableModes.Length; i++)
			{
				EditorModeInfo info = this.availableModes[i];
				info.ToolButton.Checked = (info.Mode == mode);
			}
		}
		private void ApplySelectedTileset()
		{
			Tileset tileset = DualityEditorApp.Selection.Resources.OfType<Tileset>().FirstOrDefault();
			this.SelectedTileset = tileset;
		}
		private void ApplyBrightness()
		{
			bool darkMode = this.buttonBrightness.Checked;
			this.tilesetView.BackColor = darkMode ? Color.FromArgb(64, 64, 64) : Color.FromArgb(192, 192, 192);
			this.tilesetView.ForeColor = darkMode ? Color.FromArgb(255, 255, 255) : Color.FromArgb(0, 0, 0);
		}
		
		private void StartRecordTilesetChanges()
		{
			Log.Editor.Write(
				"Start recording Tileset changes for Tileset '{0}'", 
				this.SelectedTileset);
			// ToDo
		}
		private void ApplyTilesetChanges()
		{
			Log.Editor.Write(
				"Apply Tileset changes for Tileset '{0}'", 
				this.SelectedTileset);

			// ToDo

			this.buttonApply.Enabled = false;
			this.buttonRevert.Enabled = false;
		}
		private void ResetTilesetChanges()
		{
			Log.Editor.Write(
				"Revert Tileset changes for Tileset '{0}'", 
				this.SelectedTileset);

			// ToDo

			this.buttonApply.Enabled = false;
			this.buttonRevert.Enabled = false;
		}
		private void AskApplyOrResetTilesetChanges()
		{
			Log.Editor.Write(
				"Ask Apply or Revert Tileset changes for Tileset '{0}'", 
				this.SelectedTileset);

			// ToDo: Show a user confirmation dialog when necessary

			this.ResetTilesetChanges();
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
			Resource.ResourceDisposing             += this.Resource_ResourceDisposing;

			// Apply editor-global tileset selection
			this.ApplySelectedTileset();
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			DualityEditorApp.ObjectPropertyChanged -= this.DualityEditorApp_ObjectPropertyChanged;
			DualityEditorApp.SelectionChanged      -= this.DualityEditorApp_SelectionChanged;
			Resource.ResourceDisposing             -= this.Resource_ResourceDisposing;
		}
		private void OnTilesetSelectionChanged()
		{
			// When switching to a different tileset, either apply or revert what we did to the current one
			this.AskApplyOrResetTilesetChanges();

			// We now know that we're operating on an unchanged Tileset. Start recording for the next apply / revert
			this.StartRecordTilesetChanges();

			// Update the label that tells us which tileset is selected
			this.labelSelectedTileset.Text = (this.SelectedTileset != null) ? 
				string.Format(TilemapsRes.TilesetEditor_SelectedTileset, this.SelectedTileset.Name) : 
				TilemapsRes.TilesetEditor_NoTilesetSelected;

			// Inform the currently active editing mode of this change
			if (this.activeMode != null)
				this.activeMode.RaiseOnTilesetSelectionChanged();
		}
		
		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (this.SelectedTileset == null) return;
			if (!e.HasObject(this.SelectedTileset.Res) &&
				!e.HasAnyObject(this.SelectedTileset.Res.RenderConfig)) return;

			if (this.activeMode != null)
				this.activeMode.RaiseOnTilesetModified(e);

			this.buttonApply.Enabled = true;
			this.buttonRevert.Enabled = true;
		}
		private void DualityEditorApp_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.SameObjects) return;
			this.ApplySelectedTileset();
		}
		private void Resource_ResourceDisposing(object sender, ResourceEventArgs e)
		{
			if (!e.IsResource) return;

			// Deselect the current tileset, if it's being disposed
			if (this.SelectedTileset == e.Content.As<Tileset>())
			{
				this.SelectedTileset = null;
			}
		}
		
		private void buttonBrightness_CheckedChanged(object sender, EventArgs e)
		{
			this.ApplyBrightness();
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

		}
		private void buttonRemoveLayer_Click(object sender, EventArgs e)
		{

		}
		private void layerView_SelectionChanged(object sender, EventArgs e)
		{
			TreeNodeAdv viewNode = this.layerView.SelectedNode;
			LayerSelectionChangedEventArgs args = new LayerSelectionChangedEventArgs(viewNode);

			if (this.activeMode != null)
				this.activeMode.RaiseOnLayerSelectionChanged(args);
		}

		HelpInfo IHelpProvider.ProvideHoverHelp(Point localPos, ref bool captured)
		{
			Point globalPos = this.PointToScreen(localPos);
			object hoveredObj = null;

			// Retrieve the currently hovered / active item from all child toolstrips
			ToolStripItem hoveredItem = this.GetHoveredToolStripItem(globalPos, out captured);
			hoveredObj = (hoveredItem != null) ? hoveredItem.Tag : null;

			return hoveredObj as HelpInfo;
		}
	}
}
