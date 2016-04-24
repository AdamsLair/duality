using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.PropertyEditing.Editors;

using Duality;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	public class FontContentPropertyEditor : ResourcePropertyEditor
	{
		private bool canRenderFont = false;
		public event EventHandler CanRenderFontChanged = null;
		public bool CanRenderFont
		{
			get { return this.canRenderFont; }
		}

		protected override void OnGetValue()
		{
			base.OnGetValue();
			Font font = this.GetValue().FirstOrDefault() as Font;

			bool lastCanRenderFont = this.canRenderFont;
			this.canRenderFont = (font != null && font.EmbeddedTrueTypeFont != null);
			if (lastCanRenderFont != this.canRenderFont)
			{
				if (CanRenderFontChanged != null)
					CanRenderFontChanged(this, EventArgs.Empty);
			}
		}
	}
}
