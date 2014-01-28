using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using AdamsLair.PropertyGrid.Renderer;
using AdamsLair.PropertyGrid.EditorTemplates;

namespace AdamsLair.PropertyGrid.PropertyEditors
{
	public class BitmaskPropertyEditor : PropertyEditor, IPopupControlHost
	{
		private	BitmaskEditorTemplate	bitmaskSelector	= null;
		private ulong	val				= 0L;
		private	bool	valMultiple		= false;

		public override object DisplayedValue
		{
			get { return Convert.ChangeType(this.val, this.EditedType); }
		}
		public ulong BitmaskValue
		{
			get { return this.val; }
		}
		public IEnumerable<BitmaskItem> Items
		{
			get { return this.bitmaskSelector.DropDownItems; }
			set
			{
				this.bitmaskSelector.DropDownItems = value;
				this.bitmaskSelector.BitmaskValue = this.val;
			}
		}
		public bool IsDropDownOpened
		{
			get { return this.bitmaskSelector.IsDropDownOpened; }
		}
		public BitmaskItem DropDownHoveredItem
		{
			get { return this.bitmaskSelector.DropDownHoveredItem; }
		}
		

		public BitmaskPropertyEditor()
		{
			this.bitmaskSelector = new BitmaskEditorTemplate(this);
			this.bitmaskSelector.Invalidate += this.stringSelector_Invalidate;
			this.bitmaskSelector.Edited += this.stringSelector_Edited;

			//this.Height = 18;
		}
		protected override void OnParentEditorChanged()
		{
			base.OnParentEditorChanged();
			this.Height = 5 + (int)Math.Round((float)this.ControlRenderer.DefaultFont.Height);
		}
		
		public void ShowDropDown()
		{
			this.bitmaskSelector.ShowDropDown();
		}
		public void HideDropDown()
		{
			this.bitmaskSelector.HideDropDown();
		}
		public override void PerformGetValue()
		{
			base.PerformGetValue();
			this.BeginUpdate();
			object[] values = this.GetValue().ToArray();

			// Apply values to editors
			if (!values.Any())
				this.val = 0L;
			else
			{
				this.val = Convert.ToUInt64(values.Where(o => o != null).First());
				this.valMultiple = values.Any(o => o == null) || !values.All(o => this.val == Convert.ToUInt64(o));
			}

			this.bitmaskSelector.BitmaskValue = this.val;
			this.EndUpdate();
		}

		protected internal override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			this.bitmaskSelector.OnPaint(e, this.Enabled, this.valMultiple);
		}
		protected internal override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.bitmaskSelector.OnGotFocus(e);
		}
		protected internal override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.bitmaskSelector.OnLostFocus(e);
		}
		protected internal override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this.bitmaskSelector.OnMouseMove(e);
		}
		protected internal override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.bitmaskSelector.OnMouseLeave(e);
		}
		protected internal override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.bitmaskSelector.OnMouseDown(e);
		}
		protected internal override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.bitmaskSelector.OnMouseUp(e);
		}
		protected internal override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			this.bitmaskSelector.OnKeyDown(e);
		}
		protected internal override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			this.bitmaskSelector.OnKeyUp(e);
		}

		protected override void UpdateGeometry()
		{
			base.UpdateGeometry();
			this.bitmaskSelector.Rect = new Rectangle(
				this.ClientRectangle.X + 1,
				this.ClientRectangle.Y + 1,
				this.ClientRectangle.Width - 2,
				this.ClientRectangle.Height - 1);
		}
		protected internal override void OnReadOnlyChanged()
		{
			base.OnReadOnlyChanged();
			this.bitmaskSelector.ReadOnly = this.ReadOnly;
		}

		private void stringSelector_Invalidate(object sender, EventArgs e)
		{
			this.Invalidate();
		}
		private void stringSelector_Edited(object sender, EventArgs e)
		{
			if (this.IsUpdatingFromObject) return;

			this.val = this.bitmaskSelector.BitmaskValue;
			this.Invalidate();
			this.PerformSetValue();
			this.PerformGetValue();
			this.OnEditingFinished(FinishReason.LeapValue);
		}
	}
}
