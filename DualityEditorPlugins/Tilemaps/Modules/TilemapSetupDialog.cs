using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;

namespace Duality.Editor.Plugins.Tilemaps
{
	public partial class TilemapSetupDialog : Form
	{
		private Tilemap[] tilemaps = null;

		/// <summary>
		/// [GET / SET] The tilemaps that are to be adjusted or have been created by the dialog.
		/// </summary>
		public IEnumerable<Tilemap> Tilemaps
		{
			get { return this.tilemaps; }
			set
			{
				this.tilemaps = (value != null) ? value.NotNull().ToArray() : null;
				this.UpdateContent();
			}
		}


		public TilemapSetupDialog()
		{
			this.InitializeComponent();
			this.UpdateContent();
		}


		private void UpdateContent()
		{
			this.labelHeader.Text = TilemapsRes.TilemapSetupHeader_ResizeMap;
			if (this.tilemaps != null)
			{
				this.Text = (this.tilemaps.Length == 1) ?
					string.Format(TilemapsRes.TilemapSetupHeader_ResizeSingleTilemap, this.tilemaps[0].GameObj != null ? this.tilemaps[0].GameObj.Name : typeof(Tilemap).Name) :
					string.Format(TilemapsRes.TilemapSetupHeader_ResizeMultipleTilemaps, this.tilemaps.Length);
			}
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
