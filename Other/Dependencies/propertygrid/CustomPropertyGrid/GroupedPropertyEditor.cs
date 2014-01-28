using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

using AdamsLair.PropertyGrid.Renderer;

namespace AdamsLair.PropertyGrid
{
	public abstract class GroupedPropertyEditor : PropertyEditor
	{
		public const int	DefaultIndent		= 15;
		public const int	DefaultHeaderHeight	= 18;

		private	int						headerHeight	= DefaultHeaderHeight;
		private	int						indent			= DefaultIndent;
		private	bool					expanded		= false;
		private	bool					active			= false;
		private	bool					contentInit		= false;
		private	List<PropertyEditor>	propertyEditors	= new List<PropertyEditor>();
		private	PropertyEditor			hoverEditor		= null;
		private	bool					hoverEditorLock	= false;
		private	string					headerValueText	= null;
		private	IconImage				headerIcon		= null;
		private	Color?					headerColor		= null;
		private	GroupHeaderStyle		headerStyle		= GroupHeaderStyle.Flat;
		private	Rectangle				headerRect		= Rectangle.Empty;
		private	Rectangle				expandCheckRect		= Rectangle.Empty;
		private	bool					expandCheckHovered	= false;
		private	bool					expandCheckPressed	= false;
		private	Rectangle				activeCheckRect		= Rectangle.Empty;
		private	bool					activeCheckHovered	= false;
		private	bool					activeCheckPressed	= false;
		private	Size					sizeBeforeUpdate	= Size.Empty;

		public event EventHandler<PropertyEditorEventArgs>	EditorAdded;
		public event EventHandler<PropertyEditorEventArgs>	EditorRemoving;
		public event EventHandler							ActiveChanged;
		

		public bool Expanded
		{
			get { return this.expanded; }
			set 
			{ 
				if (this.expanded != value)
				{
					this.expanded = value;
					this.Invalidate();
					if (this.expanded && !this.contentInit)
						this.InitContent();
					else
						this.UpdateHeight();
				}
			}
		}
		public bool Active
		{
			get { return this.active; }
			set 
			{ 
				if (this.active != value)
				{
					this.active = value;
					this.Invalidate();
					this.OnActiveChanged();
				}
			}
		}
		public int Indent
		{
			get { return this.indent; }
			set 
			{
				if (this.indent != value)
				{
					this.indent = value;
					this.UpdateChildWidth();
					this.Invalidate();
				}
			}
		}
		public int HeaderHeight
		{
			get { return this.headerHeight; }
			set
			{
				if (this.headerHeight != value)
				{
					this.headerHeight = value;
					this.UpdateHeight();
				}
			}
		}
		public Image HeaderIcon
		{
			get { return this.headerIcon.SourceImage; }
			set
			{
				if (this.headerIcon == null || this.headerIcon.SourceImage != value)
				{
					this.headerIcon = value != null ? new IconImage(value) : null;
					this.Invalidate(this.headerRect);
				}
			}
		}
		public Color HeaderColor
		{
			get { return this.headerColor.Value; }
			set
			{
				this.headerColor = value;
				this.Invalidate(this.headerRect);
			}
		}
		public GroupHeaderStyle HeaderStyle
		{
			get { return this.headerStyle; }
			set
			{
				if (this.headerStyle != value)
				{
					this.headerStyle = value;
					this.Invalidate(this.headerRect);
				}
			}
		}
		public string HeaderValueText
		{
			get { return this.headerValueText; }
			set
			{
				if (this.headerValueText != value)
				{
					this.headerValueText = value;
					this.Invalidate(this.headerRect);
				}
			}
		}
		public bool ContentInitialized
		{
			get { return this.contentInit; }
		}
		public bool CanExpand
		{
			get { return !this.contentInit || this.propertyEditors.Count > 0; }
		}
		public override IEnumerable<PropertyEditor> Children
		{
			get { return this.expanded ? this.propertyEditors : base.Children; }
		}
		public override bool FocusOnClick
		{
			get { return false; }
		}
		protected bool UseIndentChildExpand
		{
			get { return this.indent > ControlRenderer.ExpandNodeSize.Width + 1; }
		}
		protected bool ParentUseIndentChildExpand
		{
			get { return (this.ParentEditor as GroupedPropertyEditor) != null && (this.ParentEditor as GroupedPropertyEditor).UseIndentChildExpand; }
		}
		protected PropertyEditor HoverEditor
		{
			get { return this.hoverEditor; }
		}


		public GroupedPropertyEditor()
		{
			this.Hints &= (~HintFlags.HasPropertyName);
			this.Hints |= HintFlags.HasExpandCheck | HintFlags.ExpandEnabled;

			this.ClearContent();
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			foreach (PropertyEditor child in this.propertyEditors)
				child.Dispose();
		}

		public virtual void InitContent()
		{
			this.contentInit = true;
		}
		public virtual void ClearContent()
		{
			this.contentInit = false;
			this.ClearPropertyEditors();
		}
		public override void BeginUpdate()
		{
			base.BeginUpdate();
			this.sizeBeforeUpdate = this.Size;
		}
		public override void EndUpdate()
		{
			base.EndUpdate();
			if (this.Size != this.sizeBeforeUpdate)
				this.OnSizeChanged();
		}

		public override void PerformSetValue() {}

		public PropertyEditor PickEditorAt(int x, int y, bool ownChildrenOnly)
		{
			// Pick child editor, if applying
			int curY = this.headerHeight;
			Rectangle indentClientRect = this.ClientRectangle;
			indentClientRect.X += this.indent;
			indentClientRect.Width -= this.indent;
			if (this.expanded && indentClientRect.Contains(new Point(x, y)))
			{
				foreach (PropertyEditor e in this.propertyEditors)
				{
					if (y >= curY && y < curY + e.Height) return ownChildrenOnly ? e : e.PickEditorAt(x - indentClientRect.X, y - curY - indentClientRect.Y);
					curY += e.Height;
				}
			}

			return base.PickEditorAt(x, y);
		}
		public override PropertyEditor PickEditorAt(int x, int y)
		{
			return this.PickEditorAt(x, y, false);
		}
		public override Point GetChildLocation(PropertyEditor child)
		{
			// Pick child editor, if applying
			int curY = this.headerHeight;
			foreach (PropertyEditor e in this.propertyEditors)
			{
				if (child == e || child.IsChildOf(e))
				{
					Point result = e.GetChildLocation(child);
					result.X += this.ClientRectangle.X + this.indent;
					result.Y += this.ClientRectangle.Y + curY;
					return result;
				}
				curY += e.Height;
			}

			return base.GetChildLocation(child);
		}

		protected bool HasPropertyEditor(PropertyEditor editor)
		{
			return this.propertyEditors.Contains(editor);
		}
		protected void AddPropertyEditor(PropertyEditor editor, int atIndex = -1)
		{
			if (this.propertyEditors.Contains(editor)) this.propertyEditors.Remove(editor);

			editor.ParentEditor = this;
			this.UpdateChildWidth(editor);

			if (atIndex == -1)
				this.propertyEditors.Add(editor);
			else
				this.propertyEditors.Insert(atIndex, editor);
			
			GroupedPropertyEditor groupedEditor = editor as GroupedPropertyEditor;
			if (groupedEditor != null && groupedEditor.Expanded && !groupedEditor.ContentInitialized)
				groupedEditor.InitContent();

			this.OnEditorAdded(editor);
			this.UpdateHeight();

			editor.ValueChanged += this.OnValueChanged;
			editor.EditingFinished += this.OnEditingFinished;
			editor.SizeChanged += this.child_SizeChanged;
		}
		protected void RemovePropertyEditor(PropertyEditor editor)
		{
			editor.ParentEditor = null;
			editor.ValueChanged -= this.OnValueChanged;
			editor.EditingFinished -= this.OnEditingFinished;
			editor.SizeChanged -= this.child_SizeChanged;

			this.OnEditorRemoving(editor);
			this.propertyEditors.Remove(editor);
			this.UpdateHeight();
		}
		protected void ClearPropertyEditors()
		{
			foreach (PropertyEditor e in this.propertyEditors)
			{
				e.ParentEditor = null;
				e.ValueChanged -= this.OnValueChanged;
				e.EditingFinished -= this.OnEditingFinished;
				e.SizeChanged -= this.child_SizeChanged;
				this.OnEditorRemoving(e);
			}
			this.propertyEditors.Clear();
			this.UpdateHeight();
		}
		protected void UpdateHeight()
		{
			int h = this.headerHeight;
			if (this.expanded)
			{
				foreach (PropertyEditor e in this.propertyEditors)
					h += e.Height;
			}
			this.Height = h;
		}
		protected void UpdateChildWidth(PropertyEditor child = null)
		{
			if (child == null)
			{
				foreach (PropertyEditor e in this.propertyEditors)
					e.Width = this.ClientRectangle.Width - this.indent;
			}
			else
			{
				child.Width = this.ClientRectangle.Width - this.indent;
			}
		}
		protected override void UpdateGeometry()
		{
			base.UpdateGeometry();

			Rectangle clientRect = this.ClientRectangle;
			Rectangle buttonRect = this.ButtonRectangle;

			clientRect.Width += buttonRect.Width;
			buttonRect.Height = Math.Min(buttonRect.Height, this.headerHeight);
			buttonRect.Width = Math.Min(buttonRect.Width, this.headerHeight);
			buttonRect.X = this.Size.Width - buttonRect.Width - 1;
			buttonRect.Y = this.headerHeight / 2 - buttonRect.Height / 2;

			this.ClientRectangle = clientRect;
			this.ButtonRectangle = buttonRect;

			this.headerRect = new Rectangle(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.headerHeight);
			bool parentExpand = (this.ParentEditor as GroupedPropertyEditor) != null && (this.ParentEditor as GroupedPropertyEditor).UseIndentChildExpand;
			if (!parentExpand && (this.Hints & HintFlags.HasExpandCheck) != HintFlags.None)
			{
				this.expandCheckRect = new Rectangle(
					this.headerRect.X + 2,
					this.headerRect.Y + this.headerRect.Height / 2 - ControlRenderer.CheckBoxSize.Height / 2 - 1,
					ControlRenderer.CheckBoxSize.Width,
					ControlRenderer.CheckBoxSize.Height);
			}
			else
			{
				this.expandCheckRect = new Rectangle(this.headerRect.X, this.headerRect.Y, 0, 0);
			}

			if ((this.Hints & HintFlags.HasActiveCheck) != HintFlags.None)
			{
				this.activeCheckRect = new Rectangle(
					this.expandCheckRect.Right + 2,
					this.headerRect.Y + this.headerRect.Height / 2 - ControlRenderer.CheckBoxSize.Height / 2 - 1,
					ControlRenderer.CheckBoxSize.Width,
					ControlRenderer.CheckBoxSize.Height);
			}
			else
			{
				this.activeCheckRect = new Rectangle(this.expandCheckRect.Right, this.expandCheckRect.Y, 0, 0);
			}
		}
		
		protected virtual void OnEditorAdded(PropertyEditor e)
		{
			if (this.EditorAdded != null)
				this.EditorAdded(this, new PropertyEditorEventArgs(e));
		}
		protected virtual void OnEditorRemoving(PropertyEditor e)
		{
			if (this.EditorRemoving != null)
				this.EditorRemoving(this, new PropertyEditorEventArgs(e));
		}

		internal protected override void OnReadOnlyChanged()
		{
			base.OnReadOnlyChanged();

			foreach (PropertyEditor e in this.propertyEditors)
				e.OnReadOnlyChanged();
		}

		protected void PaintHeader(Graphics g)
		{
			if (this.headerHeight == 0) return;
			Rectangle buttonRect = this.ButtonRectangle;

			CheckBoxState activeState = CheckBoxState.UncheckedDisabled;
			CheckBoxState expandState = CheckBoxState.UncheckedDisabled;
			bool parentExpand = this.ParentUseIndentChildExpand;
			if (!parentExpand && (this.Hints & HintFlags.HasExpandCheck) != HintFlags.None)
			{
				if (this.Enabled && this.CanExpand && (this.Hints & HintFlags.ExpandEnabled) != HintFlags.None)
				{
					if (!this.Expanded)
					{
						if (this.expandCheckPressed)		expandState = CheckBoxState.PlusPressed;
						else if (this.expandCheckHovered)	expandState = CheckBoxState.PlusHot;
						else								expandState = CheckBoxState.PlusNormal;
					}
					else
					{
						if (this.expandCheckPressed)		expandState = CheckBoxState.MinusPressed;
						else if (this.expandCheckHovered)	expandState = CheckBoxState.MinusHot;
						else								expandState = CheckBoxState.MinusNormal;
					}
				}
				else
				{
					if (this.Expanded)	expandState = CheckBoxState.PlusDisabled;
					else				expandState = CheckBoxState.MinusDisabled;
				}
			}
			if ((this.Hints & HintFlags.HasActiveCheck) != HintFlags.None)
			{
				if (this.Enabled && !this.ReadOnly && (this.Hints & HintFlags.ActiveEnabled) != HintFlags.None)
				{
					if (this.Active)
					{
						if (this.activeCheckPressed)		activeState = CheckBoxState.CheckedPressed;
						else if (this.activeCheckHovered)	activeState = CheckBoxState.CheckedHot;
						else								activeState = CheckBoxState.CheckedNormal;
					}
					else
					{
						if (this.activeCheckPressed)		activeState = CheckBoxState.UncheckedPressed;
						else if (this.activeCheckHovered)	activeState = CheckBoxState.UncheckedHot;
						else								activeState = CheckBoxState.UncheckedNormal;
					}
				}
				else
				{
					if (this.Active)	activeState = CheckBoxState.CheckedDisabled;
					else				activeState = CheckBoxState.UncheckedDisabled;
				}
			}

			Rectangle iconRect;
			if (this.headerIcon != null)
			{
				iconRect = new Rectangle(
					this.activeCheckRect.Right + 2,
					this.headerRect.Y + this.headerRect.Height / 2 - this.headerIcon.Height / 2, 
					this.headerIcon.Width,
					this.headerIcon.Height);
			}
			else
			{
				iconRect = new Rectangle(this.activeCheckRect.Right, this.headerRect.Y, 0, 0);
			}
			Rectangle textRect = new Rectangle(
				iconRect.Right, 
				this.headerRect.Y, 
				this.headerRect.Width - buttonRect.Width - iconRect.Width - this.expandCheckRect.Width - this.activeCheckRect.Width - 2, 
				this.headerRect.Height);
			Rectangle nameTextRect;
			Rectangle valueTextRect;
			if (!string.IsNullOrEmpty(this.PropertyName) && !string.IsNullOrEmpty(this.headerValueText))
			{
				int nameWidth;
				nameWidth = this.NameLabelWidth - textRect.X;
				nameTextRect = new Rectangle(textRect.X, textRect.Y, nameWidth, textRect.Height);
				valueTextRect = new Rectangle(textRect.X + nameWidth, textRect.Y, textRect.Width - nameWidth, textRect.Height);
			}
			else if (!string.IsNullOrEmpty(this.headerValueText))
			{
				nameTextRect = new Rectangle(textRect.X, textRect.Y, 0, 0);
				valueTextRect = textRect;
			}
			else
			{
				nameTextRect = textRect;
				valueTextRect = new Rectangle(textRect.X, textRect.Y, 0, 0);
			}


			bool focusBg = this.Focused || (this is IPopupControlHost && (this as IPopupControlHost).IsDropDownOpened);
			bool focusBgColor = this.headerStyle == GroupHeaderStyle.Flat || this.headerStyle == GroupHeaderStyle.Simple;
			ControlRenderer.DrawGroupHeaderBackground(g, this.headerRect, focusBg && focusBgColor ? this.headerColor.Value.ScaleBrightness(0.85f) : this.headerColor.Value, this.headerStyle);
			if (focusBg && !focusBgColor)
			{
				ControlRenderer.DrawBorder(g, this.headerRect, Renderer.BorderStyle.Simple, BorderState.Normal);
			}
			
			if (!parentExpand && (this.Hints & HintFlags.HasExpandCheck) != HintFlags.None)
				ControlRenderer.DrawCheckBox(g, this.expandCheckRect.Location, expandState);
			if ((this.Hints & HintFlags.HasActiveCheck) != HintFlags.None)
				ControlRenderer.DrawCheckBox(g, this.activeCheckRect.Location, activeState);

			if (this.headerIcon != null)
				g.DrawImage(this.Enabled ? this.headerIcon.Normal : this.headerIcon.Disabled, iconRect);

			ControlRenderer.DrawStringLine(g, 
				this.PropertyName, 
				this.ValueModified ? ControlRenderer.DefaultFontBold : ControlRenderer.DefaultFont, 
				nameTextRect, 
				this.Enabled && !this.NonPublic ? ControlRenderer.ColorText : ControlRenderer.ColorGrayText);
			ControlRenderer.DrawStringLine(g, 
				this.headerValueText, 
				this.ValueModified ? ControlRenderer.DefaultFontBold : ControlRenderer.DefaultFont, 
				valueTextRect, 
				this.Enabled ? ControlRenderer.ColorText : ControlRenderer.ColorGrayText);
		}
		protected void PaintIndentExpandButton(Graphics g, GroupedPropertyEditor childGroup, int curY)
		{
			if (childGroup.headerHeight == 0) return;
			if ((childGroup.Hints & HintFlags.HasExpandCheck) == HintFlags.None) return;

			Rectangle indentExpandRect = new Rectangle(0, curY, this.indent, childGroup.headerHeight);
			Rectangle expandButtonRect = new Rectangle(
				indentExpandRect.X + indentExpandRect.Width / 2 - ControlRenderer.ExpandNodeSize.Width / 2,
				indentExpandRect.Y + indentExpandRect.Height / 2 - ControlRenderer.ExpandNodeSize.Height / 2 - 1,
				ControlRenderer.ExpandNodeSize.Width,
				ControlRenderer.ExpandNodeSize.Height);
			ExpandNodeState expandState = ExpandNodeState.OpenedDisabled;
			if (childGroup.Enabled && childGroup.CanExpand && (childGroup.Hints & HintFlags.ExpandEnabled) != HintFlags.None)
			{
				if (!childGroup.Expanded)
				{
					if (childGroup.expandCheckPressed)		expandState = ExpandNodeState.ClosedPressed;
					else if (childGroup.expandCheckHovered)	expandState = ExpandNodeState.ClosedHot;
					else if (childGroup.Focused)			expandState = ExpandNodeState.ClosedHot;
					else									expandState = ExpandNodeState.ClosedNormal;
				}
				else
				{
					if (childGroup.expandCheckPressed)		expandState = ExpandNodeState.OpenedPressed;
					else if (childGroup.expandCheckHovered)	expandState = ExpandNodeState.OpenedHot;
					else if (childGroup.Focused)			expandState = ExpandNodeState.OpenedHot;
					else									expandState = ExpandNodeState.OpenedNormal;
				}
			}
			else
			{
				if (childGroup.Expanded)	expandState = ExpandNodeState.OpenedDisabled;
				else						expandState = ExpandNodeState.ClosedDisabled;
			}

			ControlRenderer.DrawExpandNode(g, expandButtonRect.Location, expandState);
		}
		protected internal override void OnPaint(PaintEventArgs e)
		{
			int curY = 0;
			// Paint background and name label
			this.PaintBackground(e.Graphics);
			this.PaintNameLabel(e.Graphics);

			// Paint header
			this.PaintHeader(e.Graphics);
			curY += this.headerHeight;

			// Paint right button
			this.PaintButton(e.Graphics);
			
			// Paint children
			if (this.expanded)
			{
				Rectangle clipRectBase = new Rectangle(
					(int)e.Graphics.ClipBounds.X,
					(int)e.Graphics.ClipBounds.Y,
					(int)e.Graphics.ClipBounds.Width,
					(int)e.Graphics.ClipBounds.Height);
				foreach (PropertyEditor child in this.propertyEditors)
				{
					if (clipRectBase.IntersectsWith(new Rectangle(
						this.ClientRectangle.X, 
						this.ClientRectangle.Y + curY,
						child.Width, 
						child.Height)))
					{
						// Paint child editor
						GraphicsState oldState = e.Graphics.Save();
						Rectangle editorRect = new Rectangle(this.ClientRectangle.X + this.indent, this.ClientRectangle.Y + curY, child.Width, child.Height);
						editorRect.Intersect(this.ClientRectangle);
						Rectangle clipRect = editorRect;
						clipRect.Intersect(clipRectBase);
						e.Graphics.SetClip(clipRect);
						e.Graphics.TranslateTransform(this.ClientRectangle.X + this.indent, this.ClientRectangle.Y + curY);

						child.OnPaint(e);
						e.Graphics.Restore(oldState);

						// Paint child groups expand button
						if (child is GroupedPropertyEditor && this.UseIndentChildExpand)
							this.PaintIndentExpandButton(e.Graphics, child as GroupedPropertyEditor, curY);
					}

					curY += child.Height;
				}
			}
		}

		protected void IndentChildExpandOnMouseMove(MouseEventArgs e, GroupedPropertyEditor childGroup, int curY)
		{
			if (childGroup == null) return;
			Rectangle expandRect = new Rectangle(0, curY, this.indent, childGroup.headerHeight);
			bool lastExpandHovered = childGroup.expandCheckHovered;

			childGroup.expandCheckHovered = 
				childGroup.CanExpand && 
				(childGroup.Hints & HintFlags.ExpandEnabled) != HintFlags.None && 
				expandRect.Contains(e.Location);

			if (lastExpandHovered != childGroup.expandCheckHovered) this.Invalidate(expandRect);
		}
		protected void IndentChildExpandOnMouseLeave(EventArgs e, GroupedPropertyEditor childGroup, int curY)
		{
			if (childGroup == null) return;
			Rectangle expandRect = new Rectangle(0, curY, this.indent, childGroup.headerHeight);

			if (childGroup.expandCheckHovered) this.Invalidate(expandRect);
			childGroup.expandCheckHovered = false;
			childGroup.expandCheckPressed = false;
		}
		protected bool IndentChildExpandOnMouseDown(MouseEventArgs e, GroupedPropertyEditor childGroup, int curY)
		{
			if (childGroup == null) return false;

			if (childGroup.expandCheckHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				Rectangle expandRect = new Rectangle(0, curY, this.indent, childGroup.headerHeight);
				childGroup.expandCheckPressed = true;
				this.Invalidate(expandRect);
				childGroup.OnExpandCheckPressed();
				return true;
			}

			return false;
		}
		protected void IndentChildExpandOnMouseUp(MouseEventArgs e, GroupedPropertyEditor childGroup, int curY)
		{
			if (childGroup == null) return;
			
			if (childGroup.expandCheckPressed && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				Rectangle expandRect = new Rectangle(0, curY, this.indent, childGroup.headerHeight);
				childGroup.expandCheckPressed = false;
				this.Invalidate(expandRect);
			}
		}

		protected void UpdateHoverEditor(MouseEventArgs e)
		{
			PropertyEditor lastHoverEditor = this.hoverEditor;
			this.hoverEditor = this.PickEditorAt(e.X, e.Y, true);
			if (this.hoverEditor == this) this.hoverEditor = null;

			if (lastHoverEditor != this.hoverEditor && lastHoverEditor != null)
				lastHoverEditor.OnMouseLeave(EventArgs.Empty);
			if (lastHoverEditor != this.hoverEditor && this.hoverEditor != null)
			{
				// Indent expand button
				if (this.UseIndentChildExpand)
				{
					int curY = this.headerHeight;
					foreach (PropertyEditor child in this.propertyEditors)
					{
						this.IndentChildExpandOnMouseLeave(EventArgs.Empty, child as GroupedPropertyEditor, curY);
						curY += child.Height;
					}
				}

				this.hoverEditor.OnMouseEnter(EventArgs.Empty);
			}
		}
		protected internal override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			PropertyEditor lastHoverEditor = this.hoverEditor;
			
			if (!this.hoverEditorLock) this.UpdateHoverEditor(e);

			if (this.hoverEditor != null)
			{
				Point editorLoc = this.GetChildLocation(this.hoverEditor);
				this.hoverEditor.OnMouseMove(new MouseEventArgs(
					e.Button, 
					e.Clicks, 
					e.X - editorLoc.X, 
					e.Y - editorLoc.Y, 
					e.Delta));
			}
			else
			{
				bool lastExpandHovered = this.expandCheckHovered;
				bool lastActiveHovered = this.activeCheckHovered;
				this.expandCheckHovered = this.CanExpand && (this.Hints & HintFlags.ExpandEnabled) != HintFlags.None && this.expandCheckRect.Contains(e.Location);
				this.activeCheckHovered = !this.ReadOnly && (this.Hints & HintFlags.ActiveEnabled) != HintFlags.None && this.activeCheckRect.Contains(e.Location);
				if (lastExpandHovered != this.expandCheckHovered) this.Invalidate();
				if (lastActiveHovered != this.activeCheckHovered) this.Invalidate();

				// Indent expand button
				if (this.UseIndentChildExpand)
				{
					int curY = this.headerHeight;
					foreach (PropertyEditor child in this.propertyEditors)
					{
						this.IndentChildExpandOnMouseMove(e, child as GroupedPropertyEditor, curY);
						curY += child.Height;
					}
				}
			}
		}
		protected internal override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.hoverEditor != null)
			{
				PropertyEditor lastHoverEditor = this.hoverEditor;
				this.hoverEditor = null;

				if (lastHoverEditor != this.hoverEditor && lastHoverEditor != null)
					lastHoverEditor.OnMouseLeave(EventArgs.Empty);
			}

			if (this.expandCheckHovered) this.Invalidate();
			if (this.activeCheckHovered) this.Invalidate();
			this.expandCheckHovered = false;
			this.expandCheckPressed = false;
			this.activeCheckHovered = false;
			this.activeCheckPressed = false;
			
			// Indent expand button
			if (this.UseIndentChildExpand)
			{
				int curY = this.headerHeight;
				foreach (PropertyEditor child in this.propertyEditors)
				{
					this.IndentChildExpandOnMouseLeave(e, child as GroupedPropertyEditor, curY);
					curY += child.Height;
				}
			}
		}
		protected internal override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.hoverEditorLock = Control.MouseButtons != MouseButtons.None;
			if (this.hoverEditor != null)
			{
				Point editorLoc = this.GetChildLocation(this.hoverEditor);
				this.hoverEditor.OnMouseDown(new MouseEventArgs(
					e.Button, 
					e.Clicks, 
					e.X - editorLoc.X, 
					e.Y - editorLoc.Y, 
					e.Delta));
			}
			else
			{
				// Indent expand button
				bool handled = false;
				if (this.UseIndentChildExpand)
				{
					int curY = this.headerHeight;
					foreach (PropertyEditor child in this.propertyEditors)
					{
						handled = handled || this.IndentChildExpandOnMouseDown(e, child as GroupedPropertyEditor, curY);
						curY += child.Height;
					}
				}

				if (!handled)
				{
					if (new Rectangle(0, 0, this.Width, this.Height).Contains(e.Location))
						this.Focus();

					if (this.expandCheckHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
					{
						this.expandCheckPressed = true;
						this.Invalidate();
						this.OnExpandCheckPressed();
					}
					else if (this.activeCheckHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
					{
						this.activeCheckPressed = true;
						this.Invalidate();
						this.OnActiveCheckPressed();
					}
				}
			}
		}
		protected internal override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.hoverEditorLock = Control.MouseButtons != MouseButtons.None;
			if (this.hoverEditor != null)
			{
				Point editorLoc = this.GetChildLocation(this.hoverEditor);
				this.hoverEditor.OnMouseUp(new MouseEventArgs(
					e.Button, 
					e.Clicks, 
					e.X - editorLoc.X, 
					e.Y - editorLoc.Y, 
					e.Delta));
			}
			else
			{
				if (this.expandCheckPressed && (e.Button & MouseButtons.Left) != MouseButtons.None)
				{
					this.expandCheckPressed = false;
					this.Invalidate();
				}
				else if (this.activeCheckPressed && (e.Button & MouseButtons.Left) != MouseButtons.None)
				{
					this.activeCheckPressed = false;
					this.Invalidate();
				}

				// Indent expand button
				if (this.UseIndentChildExpand)
				{
					int curY = this.headerHeight;
					foreach (PropertyEditor child in this.propertyEditors)
					{
						this.IndentChildExpandOnMouseUp(e, child as GroupedPropertyEditor, curY);
						curY += child.Height;
					}
				}
			}
		}
		protected internal override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
			if (this.hoverEditor != null)
			{
				Point editorLoc = this.GetChildLocation(this.hoverEditor);
				this.hoverEditor.OnMouseClick(new MouseEventArgs(
					e.Button, 
					e.Clicks, 
					e.X - editorLoc.X, 
					e.Y - editorLoc.Y, 
					e.Delta));
			}
		}
		protected internal override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			if (this.hoverEditor != null)
			{
				Point editorLoc = this.GetChildLocation(this.hoverEditor);
				this.hoverEditor.OnMouseDoubleClick(new MouseEventArgs(
					e.Button, 
					e.Clicks, 
					e.X - editorLoc.X, 
					e.Y - editorLoc.Y, 
					e.Delta));
			}
			else if ( // Double-Click expand, if we're certain it's not handled elsewhere
				this.CanExpand && 
				(this.Hints & HintFlags.ExpandEnabled) != HintFlags.None && 
				(this.Hints & HintFlags.HasExpandCheck) != HintFlags.None && 
				!this.expandCheckHovered && 
				!this.activeCheckHovered && 
				this.headerRect.Contains(e.Location) && 
				!this.ButtonRectangle.Contains(e.Location))
			{
				this.Invalidate();
				this.OnExpandCheckPressed();
			}
		}

		protected internal override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (this.Focused && e.KeyCode == Keys.Return)
			{
				if (this.CanExpand && 
					(this.Hints & HintFlags.ExpandEnabled) != HintFlags.None &&
					(this.Hints & HintFlags.HasExpandCheck) != HintFlags.None)
				{
					this.OnExpandCheckPressed();

					// Indent expand button
					if (this.ParentEditor != null && 
						this.ParentEditor is GroupedPropertyEditor && 
						(this.ParentEditor as GroupedPropertyEditor).UseIndentChildExpand)
						this.ParentEditor.Invalidate();
				}
				e.Handled = true;
			}
		}

		protected internal override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
			if (this.hoverEditorLock)
			{
				this.hoverEditorLock = false;
				this.hoverEditor = null;
			}
			e.Effect = e.AllowedEffect; // Accept anything to pass it on to children
		}
		protected internal override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);
			if (this.hoverEditor != null)
			{
				this.hoverEditor.OnDragLeave(e);
				this.hoverEditor = null;
			}
		}
		protected internal override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			PropertyEditor lastHoverEditor = this.hoverEditor;
			
			this.hoverEditor = this.PickEditorAt(e.X, e.Y, true);
			if (this.hoverEditor == this) this.hoverEditor = null;

			if (lastHoverEditor != this.hoverEditor && lastHoverEditor != null)
				lastHoverEditor.OnDragLeave(EventArgs.Empty);
			if (lastHoverEditor != this.hoverEditor && this.hoverEditor != null)
			{
				e.Effect = DragDropEffects.None;
				this.hoverEditor.OnDragEnter(e);
			}

			if (this.hoverEditor != null)
			{
				Point editorLoc = this.GetChildLocation(this.hoverEditor);
				DragEventArgs childEvent = new DragEventArgs(
					e.Data, 
					e.KeyState, 
					e.X - editorLoc.X, 
					e.Y - editorLoc.Y, 
					e.AllowedEffect,
					e.Effect);
				this.hoverEditor.OnDragOver(childEvent);
				e.Effect = childEvent.Effect;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}
		protected internal override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);
			if (this.hoverEditor != null)
			{
				Point editorLoc = this.GetChildLocation(this.hoverEditor);
				DragEventArgs childEvent = new DragEventArgs(
					e.Data, 
					e.KeyState, 
					e.X - editorLoc.X, 
					e.Y - editorLoc.Y, 
					e.AllowedEffect,
					e.Effect);
				this.hoverEditor.OnDragDrop(childEvent);
				e.Effect = childEvent.Effect;
			}
		}

		protected internal override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if (this.ParentUseIndentChildExpand) this.ParentEditor.Invalidate();
		}
		protected internal override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if (this.ParentUseIndentChildExpand) this.ParentEditor.Invalidate();
		}

		protected override void OnParentEditorChanged()
		{
			base.OnParentEditorChanged();
			if (!this.headerColor.HasValue) this.headerColor = ControlRenderer.ColorBackground;
			if (this.HeaderHeight == DefaultHeaderHeight)
				this.HeaderHeight = 5 + (int)Math.Round((float)this.ControlRenderer.DefaultFont.Height);
		}
		protected override void OnSizeChanged()
		{
			if (this.IsUpdatingFromObject) return;
			base.OnSizeChanged();
			this.UpdateChildWidth();
		}
		protected void OnExpandCheckPressed()
		{
			this.Expanded = !this.Expanded;
		}
		protected void OnActiveCheckPressed()
		{
			if (this.ReadOnly) return;
			this.Active = !this.Active;
		}
		protected virtual void OnActiveChanged()
		{
			if (this.ActiveChanged != null)
				this.ActiveChanged(this, EventArgs.Empty);
		}

		private void child_SizeChanged(object sender, EventArgs e)
		{
			this.UpdateHeight();
			this.Invalidate();
		}
	}
}
