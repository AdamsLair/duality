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

using WeifenLuo.WinFormsUI.Docking;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace Duality.Editor.Plugins.Tilemaps
{
	public partial class TilesetEditor : DockContent
	{
		private class SummaryNodeControl : NodeControl
		{
			public override void Draw(TreeNodeAdv node, DrawContext context)
			{
				Graphics g = context.Graphics;
				Rectangle targetRect = new Rectangle(
					context.Bounds.X + this.LeftMargin,
					context.Bounds.Y,
					context.Bounds.Width - this.LeftMargin,
					context.Bounds.Height);

				// Retrieve item information
				ISummaryNode item = node.Tag as ISummaryNode;
				if (item == null) return;

				string headline = item.Title;
				string summary = item.Description;

				// Calculate drawing layout and data
				StringFormat headlineFormat = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
				StringFormat summaryFormat = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.LineLimit };
				Rectangle headlineRect;
				Rectangle summaryRect;
				{
					SizeF headlineSize;
					SizeF summarySize;
					// Base info
					{
						headlineSize = g.MeasureString(headline, context.Font, targetRect.Width, headlineFormat);
						headlineRect = new Rectangle(targetRect.X, targetRect.Y, targetRect.Width, (int)headlineSize.Height + 2);
						summaryRect = new Rectangle(targetRect.X, targetRect.Y + headlineRect.Height, targetRect.Width, targetRect.Height - headlineRect.Height);
						summarySize = g.MeasureString(summary, context.Font, summaryRect.Size, summaryFormat);
					}
					// Alignment info
					{
						Size totelContentSize = new Size(Math.Max(headlineRect.Width, summaryRect.Width), headlineRect.Height + (int)summarySize.Height);
						Point alignAdjust = new Point(0, Math.Max((targetRect.Height - totelContentSize.Height) / 2, 0));
						headlineRect.X += alignAdjust.X;
						headlineRect.Y += alignAdjust.Y;
						summaryRect.X += alignAdjust.X;
						summaryRect.Y += alignAdjust.Y;
					}
				}

				Color textColor = this.Parent.ForeColor;

				g.DrawString(headline, context.Font, new SolidBrush(Color.FromArgb(context.Enabled ? 255 : 128, textColor)), headlineRect, headlineFormat);
				g.DrawString(summary, context.Font, new SolidBrush(Color.FromArgb(context.Enabled ? 128 : 64, textColor)), summaryRect, summaryFormat);
			}
			public override Size MeasureSize(TreeNodeAdv node, DrawContext context)
			{
				return new Size(100, 48);
			}
		}
		private interface ISummaryNode
		{
			string Title { get; }
			string Description { get; }
		}
		private class VisualLayerNode : Node, ISummaryNode
		{
			private TilesetRenderInput layer = null;

			public TilesetRenderInput VisualLayer
			{
				get { return this.layer; }
			}
			public string Title
			{
				get { return this.layer.Name; }
			}
			public string Description
			{
				get { return this.layer.Id; }
			}

			public VisualLayerNode(TilesetRenderInput layer) : base()
			{
				this.layer = layer;
				this.Image = Properties.TilemapsResCache.IconTilesetSingleVisualLayer;
			}
		}

		
		private	TreeModel visualLayerModel = null;


		private ContentRef<Tileset> SelectedTileset
		{
			get { return this.tilesetView.TargetTileset; }
			set
			{
				if (this.tilesetView.TargetTileset != value)
				{
					this.tilesetView.TargetTileset = value;
					this.UpdateVisualLayerModel();
				}
			}
		}


		public TilesetEditor()
		{
			this.InitializeComponent();
			this.toolStripModeSelect.Renderer = new DualitorToolStripProfessionalRenderer();
			this.toolStripEdit.Renderer = new DualitorToolStripProfessionalRenderer();

			this.visualLayerModel = new TreeModel();
			this.UpdateVisualLayerModel();
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

		private void UpdateVisualLayerModel()
		{
			Tileset tileset = this.SelectedTileset.Res;

			// If the tileset is unavailable, or none is selected, there are no nodes
			if (tileset == null)
			{
				this.visualLayerModel.Nodes.Clear();
				return;
			}

			// Remove nodes that no longer have an equivalent in the Tileset
			foreach (VisualLayerNode node in this.visualLayerModel.Nodes.ToArray())
			{
				if (!tileset.RenderConfig.Contains(node.VisualLayer))
				{ 
					this.visualLayerModel.Nodes.Remove(node);
				}
			}

			// Add nodes that don't have a corresponding tree model node yet
			foreach (TilesetRenderInput layer in tileset.RenderConfig)
			{
				if (!this.visualLayerModel.Nodes.Any(node => (node as VisualLayerNode).VisualLayer == layer))
				{
					this.visualLayerModel.Nodes.Add(new VisualLayerNode(layer));
				}
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

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			this.layerView.Model = this.visualLayerModel;

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
		
		private void buttonBrightness_CheckedChanged(object sender, EventArgs e)
		{
			this.ApplyBrightness();
		}
		
		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (this.SelectedTileset == null) return;
			if (!e.HasObject(this.SelectedTileset.Res)) return;

			this.UpdateVisualLayerModel();
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

		private void buttonApply_Click(object sender, EventArgs e)
		{

		}
		private void buttonRevert_Click(object sender, EventArgs e)
		{

		}
		private void checkModeVisualLayers_Click(object sender, EventArgs e)
		{

		}
		private void checkModeCollisionInfo_Click(object sender, EventArgs e)
		{

		}
		private void checkModeDepthInfo_Click(object sender, EventArgs e)
		{

		}
		private void buttonAddLayer_Click(object sender, EventArgs e)
		{

		}
		private void buttonRemoveLayer_Click(object sender, EventArgs e)
		{

		}
	}
}
