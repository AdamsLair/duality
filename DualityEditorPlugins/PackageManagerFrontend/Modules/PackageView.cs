using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor.Plugins.PackageManagerFrontend
{
	public partial class PackageView : DockContent
	{
		public PackageView()
		{
			this.InitializeComponent();
		}

		internal void SaveUserData(XElement node) {}
		internal void LoadUserData(XElement node) {}
	}
}
