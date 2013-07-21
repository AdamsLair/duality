using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using AdamsLair.PropertyGrid.Renderer;

namespace AdamsLair.PropertyGrid.EditorTemplates
{
	public abstract class EditorTemplate
	{
		protected	PropertyEditor	parent	= null;
		protected	Rectangle	rect		= Rectangle.Empty;
		protected	bool		readOnly	= true;
		protected	bool		hovered		= false;
		protected	bool		focused		= false;

		public event EventHandler Invalidate = null;
		public event EventHandler<PropertyEditorValueEventArgs> Edited = null;
		public event EventHandler<PropertyEditingFinishedEventArgs> EditingFinished = null;

		public virtual Rectangle Rect
		{
			get { return this.rect; }
			set { this.rect = value; }
		}
		public virtual bool ReadOnly
		{
			get { return this.readOnly; }
			set { this.readOnly = value; }
		}
		public PropertyEditor Parent
		{
			get { return this.parent; }
		}
		public bool Focused
		{
			get { return this.focused; }
		}
		public bool Hovered
		{
			get { return this.hovered; }
		}
		protected ControlRenderer ControlRenderer
		{
			get { return this.parent.ControlRenderer; }
		}
		
		public EditorTemplate(PropertyEditor parent)
		{
			this.parent = parent;
		}

		public virtual void OnGotFocus(EventArgs e)
		{
			this.focused = true;
		}
		public virtual void OnLostFocus(EventArgs e)
		{
			this.focused = false;
		}
		public virtual void OnMouseMove(MouseEventArgs e)
		{
			bool lastHovered = this.hovered;
			this.hovered = new Rectangle(this.rect.X + 2, this.rect.Y, this.rect.Width - 4, this.rect.Height).Contains(e.Location);
			if (lastHovered != this.hovered) this.EmitInvalidate();
		}
		public virtual void OnMouseLeave(EventArgs e)
		{
			if (this.hovered) this.EmitInvalidate();
			this.hovered = false;
		}

		protected void EmitInvalidate()
		{
			if (this.Invalidate != null)
				this.Invalidate(this, EventArgs.Empty);
		}
		protected void EmitEdited(object value)
		{
			if (this.Edited != null)
				this.Edited(this, new PropertyEditorValueEventArgs(this.parent, value));
		}
		protected void EmitEditingFinished(object value, FinishReason reason)
		{
			if (this.EditingFinished != null)
				this.EditingFinished(this, new PropertyEditingFinishedEventArgs(this.parent, value, reason));
		}
	}
}
