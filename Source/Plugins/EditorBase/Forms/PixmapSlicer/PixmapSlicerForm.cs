﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml.Linq;
using Duality.Drawing;
using Duality.Editor.Controls.ToolStrip;
using Duality.Editor.Plugins.Base.Forms.PixmapSlicer;
using Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Resources;
using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor.Plugins.Base.Forms
{
	public partial class PixmapSlicerForm : DockContent, IHelpProvider
	{
		private Pixmap targetPixmap = null;
		private IPixmapSlicerState state = null;

		/// <summary>
		/// The <see cref="Pixmap"/> currently being sliced
		/// </summary>
		public Pixmap TargetPixmap
		{
			get { return this.targetPixmap; }
			set
			{
				this.targetPixmap = value;
				this.state.TargetPixmap = value;
				this.pixmapView.TargetPixmap = value;
				this.pixmapView.ZoomToFit();
				this.stateControlToolStrip.Enabled = this.state.TargetPixmap != null;
			}
		}

		public PixmapSlicerForm()
		{
			InitializeComponent();

			this.stateControlToolStrip.Renderer = new DualitorToolStripProfessionalRenderer();
			this.stateControlToolStrip.Enabled = false;

			Bitmap bmp = EditorBaseResCache.IconPixmapSlicer;
			this.Icon = Icon.FromHandle(bmp.GetHicon());

			this.SetState(new DefaultPixmapSlicerState());

			// Set styles that reduce flickering and optimize drawing
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
			DualityEditorApp.SelectionChanged += this.DualityEditorApp_SelectionChanged;
			Resource.ResourceDisposing += this.Resource_ResourceDisposing;
		}

		internal void SaveUserData(XElement node)
		{
			node.SetElementValue("DarkBackground", this.buttonBrightness.Checked);
			node.SetElementValue("DisplayIndices", this.pixmapView.NumberingStyle);
		}
		internal void LoadUserData(XElement node)
		{
			bool tryParseBool;
			PixmapNumberingStyle tryParseNumeringStyle;

			if (node.GetElementValue("DarkBackground", out tryParseBool))
				this.buttonBrightness.Checked = tryParseBool;
			if (node.GetElementValue("DisplayIndices", out tryParseNumeringStyle))
				this.pixmapView.NumberingStyle = tryParseNumeringStyle;

			this.UpdateIndicesButton();
		}

		private void SetState(IPixmapSlicerState nextState)
		{
			this.stateControlToolStrip.SuspendLayout();

			// Tear down old state
			if (this.state != null)
			{
				this.state.OnStateLeaving(EventArgs.Empty);

				// Unsubscribe from event handlers
				this.state.StateCancelled -= this.OnStateCancelled;
				this.state.StateChangeRequested -= this.OnStateChangeRequested;

				// Remove old states controls
				foreach (ToolStripItem item in this.state.StateControls)
				{
					this.stateControlToolStrip.Items.Remove(item);
				}
			}

			this.state = nextState;
			this.state.View = this.pixmapView;
			this.state.TargetPixmap = this.targetPixmap;
			this.state.StateCancelled += this.OnStateCancelled;
			this.state.StateChangeRequested += this.OnStateChangeRequested;

			// Add controls for the given state
			if (this.state.StateControls != null && this.state.StateControls.Count > 0)
			{
				foreach (ToolStripItem item in this.state.StateControls)
				{
					this.stateControlToolStrip.Items.Add(item);
				}
			}

			this.stateControlToolStrip.ResumeLayout();

			this.UpdateIndicesButton();
			this.pixmapView.Invalidate();

			if (this.state != null)
			{
				this.state.OnStateEntered(EventArgs.Empty);
			}
		}

		private void UpdateIndicesButton()
		{
			switch (this.pixmapView.NumberingStyle)
			{
				case PixmapNumberingStyle.None:
					this.buttonIndices.Image = EditorBaseResCache.IconHideIndices;
					break;
				case PixmapNumberingStyle.Hovered:
					this.buttonIndices.Image = EditorBaseResCache.IconRevealIndices;
					break;
				case PixmapNumberingStyle.All:
					this.buttonIndices.Image = EditorBaseResCache.IconShowIndices;
					break;
			}
		}

		HelpInfo IHelpProvider.ProvideHoverHelp(Point localPos, ref bool captured)
		{
			if (this.pixmapView.ClientRectangle.Contains(localPos))
				return this.state.ProvideHoverHelp(localPos, ref captured);

			return HelpInfo.FromText(
				EditorBaseRes.Help_PixmapSlicer_Topic,
				EditorBaseRes.Help_PixmapSlicer_Desc);
		}

		private void OnStateCancelled(object sender, EventArgs e)
		{
			this.SetState(new DefaultPixmapSlicerState());
		}
		private void OnStateChangeRequested(object sender, PixmapSlicerStateEventArgs e)
		{
			IPixmapSlicerState newState = (IPixmapSlicerState)Activator.CreateInstance(e.StateType);
			this.SetState(newState);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			DualityEditorApp.ObjectPropertyChanged -= this.DualityEditorApp_ObjectPropertyChanged;
			DualityEditorApp.SelectionChanged -= this.DualityEditorApp_SelectionChanged;
			Resource.ResourceDisposing -= this.Resource_ResourceDisposing;
		}

		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (e.HasObject(this.targetPixmap))
				this.pixmapView.Invalidate();
		}
		private void DualityEditorApp_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.Current.MainResource is Pixmap)
				this.TargetPixmap = e.Current.MainResource as Pixmap;
		}
		private void Resource_ResourceDisposing(object sender, ResourceEventArgs e)
		{
			if (e.ContentType == typeof(Pixmap) && e.Content.Res == this.targetPixmap)
				this.Close();
		}

		private void pixmapView_PaintContentOverlay(object sender, PaintEventArgs e)
		{
			this.state.OnPaint(e);
		}
		private void pixmapView_MouseDown(object sender, MouseEventArgs e)
		{
			this.state.OnMouseDown(e);
		}
		private void pixmapView_MouseMove(object sender, MouseEventArgs e)
		{
			this.state.OnMouseMove(e);
		}
		private void pixmapView_MouseUp(object sender, MouseEventArgs e)
		{
			this.state.OnMouseUp(e);
		}
		private void pixmapView_KeyUp(object sender, KeyEventArgs e)
		{
			this.state.OnKeyUp(e);
		}

		private void buttonBrightness_CheckedChanged(object sender, EventArgs e)
		{
			this.pixmapView.DarkMode = this.buttonBrightness.Checked;
		}
		private void buttonZoomIn_Click(object sender, EventArgs e)
		{
			this.pixmapView.ScaleFactor += this.pixmapView.ScaleFactor * 0.2f;
		}
		private void buttonZoomOut_Click(object sender, EventArgs e)
		{
			this.pixmapView.ScaleFactor -= this.pixmapView.ScaleFactor * 0.2f;
		}
		private void buttonDefaultZoom_Click(object sender, EventArgs e)
		{
			this.pixmapView.ScaleFactor = 1.0f;
		}
		private void buttonIndices_Click(object sender, EventArgs e)
		{
			PixmapNumberingStyle currentStyle = this.pixmapView.NumberingStyle;

			PixmapNumberingStyle newStyle = (PixmapNumberingStyle)(
				((int)currentStyle + 1) % 
				((int)PixmapNumberingStyle.All + 1));

			this.pixmapView.NumberingStyle = newStyle;
			this.UpdateIndicesButton();
		}
	}
}
