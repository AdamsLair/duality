using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Duality.Editor.Plugins.ResourceHacker
{
	public partial class RenameTypeDialog : Form
	{
		private	string[]	availTypes;
		private	string		searchFor;
		private	string		replaceWith;

		public string SearchFor
		{
			get { return this.searchFor; }
		}
		public string ReplaceWith
		{
			get { return this.replaceWith; }
		}

		protected RenameTypeDialog()
		{
			this.InitializeComponent();
		}
		public RenameTypeDialog(IEnumerable<string> availTypes) : this()
		{
			this.availTypes = availTypes.OrderBy(s => s).ToArray();
			this.UpdateSearchBox();
		}

		protected void UpdateSearchBox()
		{
			this.comboBoxSearchFor.Items.Clear();
			this.comboBoxSearchFor.Items.AddRange(availTypes);
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			this.searchFor = this.comboBoxSearchFor.Text.Trim();
			this.replaceWith = this.textBoxReplaceWith.Text.Trim();
			this.DialogResult = DialogResult.OK;
		}
		private void buttonAbort_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}
		private void textBoxReplaceWith_TextChanged(object sender, EventArgs e)
		{
			bool valid = !String.IsNullOrWhiteSpace(this.textBoxReplaceWith.Text);
			bool bothValid = valid && !String.IsNullOrWhiteSpace(this.comboBoxSearchFor.Text);
			this.buttonOk.Enabled = bothValid;

			string typeString = this.textBoxReplaceWith.Text.Trim();
			bool resolved = valid && Duality.ReflectionHelper.ResolveType(typeString, false) != null;
			this.textBoxReplaceWith.BackColor = resolved ? Color.FromArgb(225, 255, 225) : Color.FromArgb(255, 225, 225);
		}
		private void comboBoxSearchFor_TextChanged(object sender, EventArgs e)
		{
			bool valid = !String.IsNullOrWhiteSpace(this.comboBoxSearchFor.Text);
			bool bothValid = valid && !String.IsNullOrWhiteSpace(this.textBoxReplaceWith.Text);
			this.buttonOk.Enabled = bothValid;

			string typeString = this.comboBoxSearchFor.Text.Trim();
			bool resolved = valid && Duality.ReflectionHelper.ResolveType(typeString, false) != null;
			this.comboBoxSearchFor.BackColor = resolved ? Color.FromArgb(225, 255, 225) : Color.FromArgb(255, 225, 225);
		}
	}
}
