using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;

using AdamsLair.PropertyGrid.Renderer;

namespace AdamsLair.PropertyGrid
{
	public class PropertyEditorEventArgs : EventArgs
	{
		PropertyEditor editor;

		public PropertyEditor Editor
		{
			get { return this.editor; }
		}

		public PropertyEditorEventArgs(PropertyEditor e)
		{
			this.editor = e;
		}
	}

	public class PropertyEditorValueEventArgs : EventArgs
	{
		private	PropertyEditor	editor	= null;
		private	object			value	= null;

		public PropertyEditor Editor
		{
			get { return this.editor; }
		}
		public object Value
		{
			get { return this.value; }
		}

		public PropertyEditorValueEventArgs(PropertyEditor editor, object value)
		{
			this.editor = editor;
			this.value = value;
		}
	}

	public enum FinishReason
	{
		Unknown,

		LostFocus,
		LeapValue,
		UserAccept
	}

	public class PropertyEditingFinishedEventArgs : PropertyEditorValueEventArgs
	{
		private	FinishReason	reason	= FinishReason.Unknown;

		public FinishReason Reason
		{
			get { return this.reason; }
		}

		public PropertyEditingFinishedEventArgs(PropertyEditor editor, object value, FinishReason reason) : base(editor, value)
		{
			this.reason = reason;
		}
	}
	
	public abstract class PropertyEditor : IDisposable
	{
		[Flags]
		public enum HintFlags
		{
			None			= 0x00,

			HasPropertyName	= 0x01,
			HasButton		= 0x02,
			HasExpandCheck	= 0x04,
			HasActiveCheck	= 0x08,

			ButtonEnabled	= 0x10,
			ExpandEnabled	= 0x20,
			ActiveEnabled	= 0x40,

			All = HasPropertyName | HasButton | HasExpandCheck | HasActiveCheck | ButtonEnabled | ExpandEnabled | ActiveEnabled,
			Default = HasPropertyName
		}

		private	PropertyGrid	parentGrid		= null;
		private	PropertyEditor	parentEditor	= null;
		private	Type			editedType		= null;
		private	MemberInfo		editedMember	= null;
		private	string			propertyName	= AdamsLair.PropertyGrid.EmbeddedResources.Resources.PropertyName_Default;
		private	string			propertyDesc	= null;
		private	object			configureData	= null;
		private	bool			forceWriteBack	= false;
		private	bool			valueModified	= false;
		private	bool			mutableValue	= false;
		private	bool			memberNonPublic	= false;
		private	bool			updatingFromObj	= false;
		private	bool			disposed		= false;
		private	HintFlags		hints			= HintFlags.Default;
		private	Size			size			= new Size(0, 20);
		private	Rectangle		clientRect		= Rectangle.Empty;
		private	Rectangle		nameLabelRect	= Rectangle.Empty;
		private	Rectangle		buttonRect		= Rectangle.Empty;
		private	bool			buttonHovered	= false;
		private	bool			buttonPressed	= false;
		private	IconImage		buttonIcon		= null;
		private	Func<IEnumerable<object>>	getter	= null;
		private	Action<IEnumerable<object>>	setter	= null;
		private	Func<object,object>	converterGet	= null;
		private	Func<object,object>	converterSet	= null;


		public event EventHandler	SizeChanged		= null;
		public event EventHandler	ButtonPressed	= null;
		public event EventHandler<PropertyEditingFinishedEventArgs>	EditingFinished = null;
		public event EventHandler<PropertyEditorValueEventArgs>		ValueChanged	= null;


		public PropertyGrid ParentGrid
		{
			get { return this.parentGrid; }
			internal set
			{
				bool lastReadOnly = this.ReadOnly;
				this.parentGrid = value;
				if (this.parentGrid == null) this.parentEditor = null;
				if (this.ReadOnly != lastReadOnly) this.OnReadOnlyChanged();
			}
		}
		public PropertyEditor ParentEditor
		{
			get { return this.parentEditor; }
			internal set
			{
				bool lastReadOnly = this.ReadOnly;
				this.parentEditor = value;
				if (this.parentEditor != null) this.parentGrid = this.parentEditor.ParentGrid;
				if (this.ReadOnly != lastReadOnly) this.OnReadOnlyChanged();
				this.OnParentEditorChanged();
			}
		}
		public PropertyEditor NextEditor
		{
			get
			{
				if (this.parentEditor == null) return null;
				bool foundMe = false;
				foreach (PropertyEditor child in this.parentEditor.Children)
				{
					if (foundMe) return child;
					if (child == this) foundMe = true;
				}
				return null;
			}
		}
		public PropertyEditor PrevEditor
		{
			get
			{
				if (this.parentEditor == null) return null;
				PropertyEditor last = null;
				foreach (PropertyEditor child in this.parentEditor.Children)
				{
					if (child == this) return last;
					last = child;
				}
				return null;
			}
		}
		public virtual IEnumerable<PropertyEditor> Children
		{
			get { return new PropertyEditor[0]; }
		}
		public IEnumerable<PropertyEditor> ChildrenDeep
		{
			get
			{
				foreach (PropertyEditor child in this.Children)
				{
					yield return child;
					foreach (PropertyEditor subChild in child.ChildrenDeep)
					{
						yield return subChild;
					}
				}
			}
		}
		
		public bool Disposed
		{
			get { return this.disposed; }
		}
		public Type EditedType
		{
			get { return this.editedType; }
			set 
			{
				if (this.editedType != value)
				{
					this.editedType = value;
					this.OnEditedTypeChanged();
				}
			}
		}
		public MemberInfo EditedMember
		{
			get { return this.editedMember; }
			set 
			{
				if (this.editedMember != value)
				{
					this.editedMember = value;
					this.OnEditedMemberChanged();
				}
			}
		}
		public string PropertyName
		{
			get { return this.propertyName; }
			set 
			{
				if (this.propertyName != value)
				{
					this.propertyName = value;
					this.Invalidate();
				}
			}
		}
		public string PropertyDesc
		{
			get { return this.propertyDesc; }
			set { this.propertyDesc = value; }
		}
		public object ConfigureData
		{
			get { return this.configureData; }
		}
		public bool ForceWriteBack
		{
			get { return this.forceWriteBack; }
			set { this.forceWriteBack = value; }
		}
		public bool ValueModified
		{
			get { return this.valueModified; }
			set { this.valueModified = value; }
		}
		public bool ValueMutable
		{
			get { return this.mutableValue; }
			set { this.mutableValue = value; }
		}
		public bool NonPublic
		{
			get { return this.memberNonPublic; }
			set { this.memberNonPublic = value; }
		}
		public Func<IEnumerable<object>> Getter
		{
			set { this.getter = value; }
		}
		public Action<IEnumerable<object>> Setter
		{
			set
			{ 
				bool lastReadOnly = this.ReadOnly;
				this.setter = value;
				if (this.ReadOnly != lastReadOnly) this.OnReadOnlyChanged();
			}
		}
		public Func<object,object> ConverterGet
		{
			get { return this.converterGet; }
			set
			{
				if (this.converterGet != value)
				{
					this.converterGet = value;
					if (this.getter != null) this.PerformGetValue();
				}
			}
		}
		public Func<object,object> ConverterSet
		{
			get { return this.converterSet; }
			set { this.converterSet = value; }
		}
		public abstract object DisplayedValue { get; }
		public bool ReadOnly
		{
			get { return this.setter == null || (!this.mutableValue && this.parentEditor != null && this.parentEditor.ReadOnly); }
		}
		public bool Enabled
		{
			get { return this.parentGrid != null && this.parentGrid.Enabled; }
		}
		public bool Focused
		{
			get
			{
				if (this.parentGrid == null) return false;
				return this.parentGrid.Focused && this.parentGrid.FocusEditor == this;
			}
		}
		public virtual bool FocusOnClick
		{
			get { return this.CanGetFocus; }
		}
		public virtual bool CanGetFocus
		{
			get { return this.Height > 0; }
		}
		public HintFlags Hints
		{
			get { return this.hints; }
			set
			{
				if (this.hints != value)
				{
					this.hints = value;
					if (this.parentGrid != null) this.UpdateGeometry();
				}
			}
		}
		public Image ButtonIcon
		{
			get { return this.buttonIcon.SourceImage; }
			set
			{
				if (value == null) value = AdamsLair.PropertyGrid.EmbeddedResources.Resources.ImageDelete;
				if (this.buttonIcon == null || this.buttonIcon.SourceImage != value)
				{
					this.buttonIcon = new IconImage(value);
					this.Invalidate();
				}
			}
		}
		protected bool IsUpdatingFromObject
		{
			get { return this.updatingFromObj; }
		}

		public Size Size
		{
			get { return this.size; }
			set
			{
				if (this.size != value)
				{
					this.size = value;
					this.OnSizeChanged();
				}
			}
		}
		public int Width
		{
			get { return this.size.Width; }
			set { this.Size = new Size(value, this.size.Height); }
		}
		public int Height
		{
			get { return this.size.Height; }
			set { this.Size = new Size(this.size.Width, value); }
		}
		
		public Rectangle ClientRectangle
		{
			get { return this.clientRect; }
			protected set { this.clientRect = value; }
		}
		protected Rectangle NameLabelRectangle
		{
			get { return this.nameLabelRect; }
			set { this.nameLabelRect = value; }
		}
		protected Rectangle ButtonRectangle
		{
			get { return this.buttonRect; }
			set { this.buttonRect = value; }
		}
		protected int NameLabelWidth
		{
			get { return this.size.Width * 2 / 5; }
		}
		internal protected ControlRenderer ControlRenderer
		{
			get { return this.parentGrid != null ? this.parentGrid.ControlRenderer : null; }
		}
		

		public PropertyEditor()
		{
			this.ButtonIcon = null;
		}
		~PropertyEditor()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool manually)
		{
			if (this.disposed) return;

			this.OnDisposing(manually);
			this.disposed = true;
		}
		protected virtual void OnDisposing(bool manually) {}

		public virtual void PerformGetValue() {}
		public virtual void PerformSetValue()
		{
			if (this.ReadOnly) return;
			this.SetValue(this.DisplayedValue);
		}

		protected IEnumerable<object> GetValue()
		{
			if (this.getter == null) return null;
			IEnumerable<object> result = this.getter();
			if (this.converterGet != null && result != null)
				return result.Select(this.converterGet);
			else
				return result;
		}
		protected void SetValues(IEnumerable<object> objEnum)
		{
			if (this.setter == null) return;
			this.parentGrid.PrepareSetValue();

			if (this.converterSet != null && objEnum != null)
				this.setter(objEnum.Select(this.converterSet));
			else
				this.setter(objEnum);
			this.OnValueChanged();

			this.parentGrid.PostSetValue();
		}
		protected void SetValue(object obj)
		{
			this.SetValues(new object[] { obj });
		}

		public void Invalidate()
		{
			if (this.parentGrid == null) return;
			Rectangle invalidateRect = new Rectangle(this.parentGrid.GetEditorLocation(this, true), this.size);
			this.parentGrid.Invalidate(invalidateRect);
		}
		public void Invalidate(Rectangle rect)
		{
			if (this.parentGrid == null) return;
			Point editorLoc = this.parentGrid.GetEditorLocation(this, true);
			Rectangle invalidateRect = new Rectangle(
				editorLoc.X + rect.X,
				editorLoc.Y + rect.Y,
				rect.Width,
				rect.Height);
			this.parentGrid.Invalidate(invalidateRect);
		}
		public void Focus()
		{
			if (this.parentGrid != null) this.parentGrid.Focus(this);
		}
		public bool IsChildOf(PropertyEditor parent)
		{
			if (this.parentEditor == parent) return true;
			if (this.parentEditor == null) return false;
			return this.parentEditor.IsChildOf(parent);
		}
		public virtual PropertyEditor PickEditorAt(int x, int y)
		{
			return this;
		}
		public virtual Point GetChildLocation(PropertyEditor child)
		{
			return Point.Empty;
		}

		protected virtual void UpdateGeometry()
		{
			if ((this.hints & HintFlags.HasPropertyName) != HintFlags.None)
				this.nameLabelRect = new Rectangle(0, 0, this.NameLabelWidth, this.size.Height);
			else
				this.nameLabelRect = Rectangle.Empty;

			if ((this.hints & HintFlags.HasButton) != HintFlags.None)
			{
				Size buttonSize = this.buttonIcon != null ? this.buttonIcon.Size : new Size(10, 10);
				this.buttonRect.Height = this.Size.Height;
				this.buttonRect.Width = Math.Min(this.size.Height, buttonSize.Height + 2);
				this.buttonRect.X = this.Size.Width - buttonRect.Width - 1;
				this.buttonRect.Y = 0;
			}
			else
				this.buttonRect = Rectangle.Empty;

			this.clientRect = new Rectangle(0, 0, this.size.Width, this.size.Height);
			this.clientRect.X += this.nameLabelRect.Width;
			this.clientRect.Width -= this.nameLabelRect.Width;
			this.clientRect.Width -= this.buttonRect.Width;
		}
		public virtual void BeginUpdate()
		{
			if (this.updatingFromObj) throw new InvalidOperationException("The PropertyEditor already is updating");
			this.updatingFromObj = true;
		}
		public virtual void EndUpdate()
		{
			if (!this.updatingFromObj) throw new InvalidOperationException("The PropertyEditor was not updating");
			this.updatingFromObj = false;
		}
		
		protected void PaintBackground(Graphics g)
		{
			bool focusBg = this.Focused || (this is IPopupControlHost && (this as IPopupControlHost).IsDropDownOpened);
			g.FillRectangle(new SolidBrush(focusBg ? ControlRenderer.ColorBackground.ScaleBrightness(0.85f) : ControlRenderer.ColorBackground), new Rectangle(Point.Empty, this.size));
		}
		protected void PaintButton(Graphics g)
		{
			if ((this.hints & HintFlags.HasButton) == HintFlags.None || this.buttonIcon == null) return;

			Size buttonSize = new Size(
				Math.Min(this.buttonIcon.Width, this.buttonRect.Width),
				Math.Min(this.buttonIcon.Height, this.buttonRect.Height));
			Point buttonCenter = new Point(this.buttonRect.X + this.buttonRect.Width / 2, this.buttonRect.Y + this.buttonRect.Height / 2);

			Image buttonImage;
			if ((this.Hints & HintFlags.ButtonEnabled) == HintFlags.None || this.ReadOnly || !this.Enabled)
				buttonImage = this.buttonIcon.Disabled;
			else if (this.buttonPressed)
				buttonImage = this.buttonIcon.Active;
			else if (this.buttonHovered || this.Focused)
				buttonImage = this.buttonIcon.Normal;
			else
				buttonImage = this.buttonIcon.Passive;
				
			if (this.buttonHovered)
			{
				Rectangle buttonBgRect = this.buttonRect;
				buttonBgRect.Height = Math.Min(buttonBgRect.Height, buttonBgRect.Width) - 1;
				buttonBgRect.Width = buttonBgRect.Height;
				buttonBgRect.X = this.buttonRect.X + this.buttonRect.Width / 2 - buttonBgRect.Width / 2 - 1;
				buttonBgRect.Y = this.buttonRect.Y + this.buttonRect.Height / 2 - buttonBgRect.Height / 2 - 1;
				g.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), buttonBgRect);
				g.DrawRectangle(new Pen(Color.FromArgb(255, Color.White)), buttonBgRect);
			}
				
			g.DrawImage(buttonImage, buttonCenter.X - buttonSize.Width / 2, buttonCenter.Y - buttonSize.Height / 2, buttonSize.Width, buttonSize.Height);
		}
		protected void PaintNameLabel(Graphics g)
		{
			if ((this.hints & HintFlags.HasPropertyName) == HintFlags.None) return;
			ControlRenderer.DrawStringLine(g, 
				this.propertyName, 
				this.ValueModified ? ControlRenderer.DefaultFontBold : ControlRenderer.DefaultFont, 
				this.nameLabelRect, 
				this.Enabled && !this.NonPublic ? ControlRenderer.ColorText : ControlRenderer.ColorGrayText);
		}
		internal protected virtual void OnPaint(PaintEventArgs e)
		{
			this.PaintBackground(e.Graphics);
			this.PaintNameLabel(e.Graphics);
			this.PaintButton(e.Graphics);
		}
		
		internal protected virtual void OnMouseEnter(EventArgs e) {}
		internal protected virtual void OnMouseLeave(EventArgs e)
		{
			if (this.buttonHovered) this.Invalidate();
			this.buttonHovered = false;
			this.buttonPressed = false;
		}
		internal protected virtual void OnMouseMove(MouseEventArgs e)
		{
			bool lastHovered = this.buttonHovered;
			this.buttonHovered = (this.Hints & HintFlags.ButtonEnabled) != HintFlags.None && !this.ReadOnly && this.ButtonRectangle.Contains(e.Location);
			if (lastHovered != this.buttonHovered) this.Invalidate();
		}
		internal protected virtual void OnMouseDown(MouseEventArgs e)
		{
			if (this.FocusOnClick && new Rectangle(0, 0, this.size.Width, this.size.Height).Contains(e.Location))
				this.Focus();
			if (this.buttonHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.buttonPressed = true;
				this.Invalidate();
			}
		}
		internal protected virtual void OnMouseUp(MouseEventArgs e)
		{
			//if (this.FocusOnClick && new Rectangle(0, 0, this.size.Width, this.size.Height).Contains(e.Location))
			//    this.Focus();
			if (this.buttonPressed && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.buttonPressed = false;
				this.Invalidate();
				if (this.buttonHovered) this.OnButtonPressed();
			}
		}
		internal protected virtual void OnMouseClick(MouseEventArgs e) {}
		internal protected virtual void OnMouseDoubleClick(MouseEventArgs e) {}

		internal protected virtual void OnKeyDown(KeyEventArgs e) {}
		internal protected virtual void OnKeyUp(KeyEventArgs e) {}
		internal protected virtual void OnKeyPress(KeyPressEventArgs e) {}

		internal protected virtual void OnDragEnter(DragEventArgs e) {}
		internal protected virtual void OnDragLeave(EventArgs e) {}
		internal protected virtual void OnDragOver(DragEventArgs e) {}
		internal protected virtual void OnDragDrop(DragEventArgs e) {}

		internal protected virtual void OnGotFocus(EventArgs e)
		{
			this.Invalidate();
		}
		internal protected virtual void OnLostFocus(EventArgs e)
		{
			this.Invalidate();
		}

		internal protected virtual void OnReadOnlyChanged()
		{
			this.Invalidate();
		}
		protected virtual void OnEditedTypeChanged()
		{
			this.Invalidate();
		}
		protected virtual void OnEditedMemberChanged()
		{
			this.Invalidate();
			if (this.editedMember != null)
			{
				bool flaggedForceWriteBack = false;
				this.forceWriteBack = flaggedForceWriteBack;
			}
			else
				this.forceWriteBack = false;
		}
		protected virtual void OnSizeChanged()
		{
			if (this.parentGrid != null) this.UpdateGeometry();
			if (this.SizeChanged != null)
				this.SizeChanged(this, EventArgs.Empty);
		}
		protected virtual void OnButtonPressed()
		{
			if (this.ButtonPressed != null)
				this.ButtonPressed(this, EventArgs.Empty);
		}
		protected virtual void OnParentEditorChanged() {}
		protected virtual void OnValueChanged(object sender, PropertyEditorValueEventArgs args)
		{
			if (this.ValueChanged != null)
				this.ValueChanged(sender, args);
		}
		protected virtual void OnEditingFinished(object sender, PropertyEditingFinishedEventArgs args)
		{
			if (this.EditingFinished != null)
				this.EditingFinished(sender, args);
		}

		internal protected virtual void ConfigureEditor(object configureData)
		{
			this.configureData = configureData;
		}

		protected void OnValueChanged()
		{
			this.OnValueChanged(this, new PropertyEditorValueEventArgs(this, this.DisplayedValue));
		}
		protected void OnEditingFinished(FinishReason reason)
		{
			this.OnEditingFinished(this, new PropertyEditingFinishedEventArgs(this, this.DisplayedValue, reason));
		}
	}
}
