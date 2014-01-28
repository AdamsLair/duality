using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using AdamsLair.PropertyGrid.Renderer;
using AdamsLair.PropertyGrid.EditorTemplates;

namespace AdamsLair.PropertyGrid.PropertyEditors
{
	public class ObjectSelectorPropertyEditor : PropertyEditor, IPopupControlHost
	{
		private	ComboBoxEditorTemplate	objSelector	= null;
		private object	val			= null;
		private	bool	valMultiple	= false;

		public override object DisplayedValue
		{
			get { return this.val; }
		}
		public IEnumerable<ObjectItem> Items
		{
			get { return this.objSelector.DropDownItems.Cast<ObjectItem>(); }
			set 
			{
				this.objSelector.DropDownItems = value;
				this.UpdateEditorValue();
			}
		}
		public bool IsDropDownOpened
		{
			get { return this.objSelector.IsDropDownOpened; }
		}
		public ObjectItem DropDownHoveredObject
		{
			get { return this.objSelector.DropDownHoveredObject as ObjectItem; }
		}
		

		public ObjectSelectorPropertyEditor()
		{
			this.objSelector = new ComboBoxEditorTemplate(this);
			this.objSelector.Invalidate += this.objSelector_Invalidate;
			this.objSelector.Edited += this.objSelector_Edited;

			//this.Height = 18;
		}
		protected override void OnParentEditorChanged()
		{
			base.OnParentEditorChanged();
			this.Height = 5 + (int)Math.Round((float)this.ControlRenderer.DefaultFont.Height);
		}
		
		public void ShowDropDown()
		{
			this.objSelector.ShowDropDown();
		}
		public void HideDropDown()
		{
			this.objSelector.HideDropDown();
		}
		public override void PerformGetValue()
		{
			base.PerformGetValue();
			this.BeginUpdate();
			object[] values = this.GetValue().ToArray();

			// Apply values to editors
			if (!values.Any())
				this.val = null;
			else
			{
				this.val = values.Where(o => o != null).FirstOrDefault();
				this.valMultiple = values.Any(o => o == null) || !values.All(o => object.Equals(o, this.val));
			}

			this.UpdateEditorValue();
			this.EndUpdate();
		}

		private void UpdateEditorValue()
		{
			this.objSelector.SelectedObject = this.objSelector.DropDownItems.Cast<ObjectItem>().FirstOrDefault(i => object.Equals(i.Value, this.val));
		}

		protected internal override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			this.objSelector.OnPaint(e, this.Enabled, this.valMultiple);
		}
		protected internal override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.objSelector.OnGotFocus(e);
		}
		protected internal override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.objSelector.OnLostFocus(e);
		}
		protected internal override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this.objSelector.OnMouseMove(e);
		}
		protected internal override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.objSelector.OnMouseLeave(e);
		}
		protected internal override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.objSelector.OnMouseDown(e);
		}
		protected internal override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.objSelector.OnMouseUp(e);
		}
		protected internal override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			this.objSelector.OnKeyDown(e);
		}
		protected internal override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			this.objSelector.OnKeyUp(e);
		}

		protected override void UpdateGeometry()
		{
			base.UpdateGeometry();
			this.objSelector.Rect = new Rectangle(
				this.ClientRectangle.X + 1,
				this.ClientRectangle.Y + 1,
				this.ClientRectangle.Width - 2,
				this.ClientRectangle.Height - 1);
		}
		protected internal override void OnReadOnlyChanged()
		{
			base.OnReadOnlyChanged();
			this.objSelector.ReadOnly = this.ReadOnly;
		}

		private void objSelector_Invalidate(object sender, EventArgs e)
		{
			this.Invalidate();
		}
		private void objSelector_Edited(object sender, EventArgs e)
		{
			if (this.IsUpdatingFromObject) return;

			this.val = (this.objSelector.SelectedObject as ObjectItem).Value;
			this.Invalidate();
			this.PerformSetValue();
			this.PerformGetValue();
			this.OnEditingFinished(FinishReason.LeapValue);
		}
	}
}
