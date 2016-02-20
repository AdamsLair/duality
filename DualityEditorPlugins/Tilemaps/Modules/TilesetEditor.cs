using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;

using Duality.Editor.Controls.ToolStrip;

using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor.Plugins.Tilemaps
{
	public partial class TilesetEditor : DockContent
	{
		public TilesetEditor()
		{
			this.InitializeComponent();
			this.toolStripModeSelect.Renderer = new DualitorToolStripProfessionalRenderer();
			this.toolStripEdit.Renderer = new DualitorToolStripProfessionalRenderer();
		}

		internal void SaveUserData(XElement node) { }
		internal void LoadUserData(XElement node) { }
	}
}
