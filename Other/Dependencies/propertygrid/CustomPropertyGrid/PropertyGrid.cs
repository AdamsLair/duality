using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Reflection;

using AdamsLair.PropertyGrid.PropertyEditors;
using AdamsLair.PropertyGrid.Renderer;

namespace AdamsLair.PropertyGrid
{
	public class ProviderContext
	{
		private PropertyGrid parentGrid;
		private	PropertyEditor parentEditor;

		public PropertyGrid ParentGrid
		{
			get { return this.parentGrid; }
		}
		public PropertyEditor ParentEditor
		{
			get { return this.parentEditor; }
		}

		public ProviderContext(PropertyGrid grid, PropertyEditor editor = null)
		{
			this.parentEditor = editor;
			this.parentGrid = editor != null ? editor.ParentGrid : grid;
		}
	}

	public interface IPropertyEditorProvider
	{
		int IsResponsibleFor(Type baseType, ProviderContext context);
		PropertyEditor CreateEditor(Type baseType, ProviderContext context);
	}

	public class ExpandState
	{
		HashSet<string>	expandedNodes	= new HashSet<string>();

		public ExpandState() {}
		public ExpandState(ExpandState cc)
		{
			this.expandedNodes = new HashSet<string>(cc.expandedNodes);
		}
		public void CopyTo(ExpandState other)
		{
			other.expandedNodes.Clear();
			foreach (var n in this.expandedNodes) other.expandedNodes.Add(n);
		}

		public bool IsEditorExpanded(GroupedPropertyEditor editor)
		{
			if (editor == null) return false;
			string id = GetEditorFullId(editor);
			return expandedNodes.Contains(id);
		}
		public void SetEditorExpanded(GroupedPropertyEditor editor, bool expanded)
		{
			if (editor == null) return;
			string id = GetEditorFullId(editor);
			if (expanded) this.expandedNodes.Add(id);
			else this.expandedNodes.Remove(id);
		}
		public void Clear()
		{
			this.expandedNodes.Clear();
		}
		public void UpdateFrom(PropertyEditor mainEditor)
		{
			if (mainEditor == null) return;
			foreach (GroupedPropertyEditor child in mainEditor.ChildrenDeep.OfType<GroupedPropertyEditor>())
				this.SetEditorExpanded(child, child.Expanded);
		}
		public void ApplyTo(PropertyEditor mainEditor, bool dontCollapse = true)
		{
			if (mainEditor == null) return;
			foreach (GroupedPropertyEditor child in mainEditor.ChildrenDeep.OfType<GroupedPropertyEditor>())
			{
				if (child.Expanded && dontCollapse) continue;
				child.Expanded = this.IsEditorExpanded(child);
			}
		}

		private static string GetEditorId(PropertyEditor editor)
		{
			if (editor == null) return "";
			return editor.PropertyName + editor.EditedType;
		}
		private static string GetEditorFullId(PropertyEditor editor)
		{
			if (editor == null) return "";
			string fullId = "";
			while (editor != null)
			{
				fullId = GetEditorId(editor) + "/" + fullId;
				editor = editor.ParentEditor;
			}
			return fullId;
		}
	}

	public partial class PropertyGrid : UserControl
	{
		public const int EditorPriority_None		= 0;
		public const int EditorPriority_General		= 20;
		public const int EditorPriority_Specialized	= 50;
		public const int EditorPriority_Override	= 100;

		private class MainEditorProvider : IPropertyEditorProvider
		{
			private	List<IPropertyEditorProvider>	subProviders	= new List<IPropertyEditorProvider>();
			public List<IPropertyEditorProvider> SubProviders
			{
				get { return this.subProviders; }
				set { this.subProviders = value; }
			}
			
			public int IsResponsibleFor(Type baseType, ProviderContext context)
			{
				return EditorPriority_General;
			}
			public PropertyEditor CreateEditor(Type baseType, ProviderContext context)
			{
				PropertyEditor e = null;

				// Basic numeric data types
				if (baseType == typeof(sbyte) || baseType == typeof(byte) ||
					baseType == typeof(short) || baseType == typeof(ushort) ||
					baseType == typeof(int) || baseType == typeof(uint) ||
					baseType == typeof(long) || baseType == typeof(ulong) ||
					baseType == typeof(float) || baseType == typeof(double) || baseType == typeof(decimal))
					e = new NumericPropertyEditor();
				// Basic data type: Boolean
				else if (baseType == typeof(bool))
				    e = new BoolPropertyEditor();
				// Basic data type: Flagged Enum
				else if (baseType.IsEnum && baseType.GetCustomAttributes(typeof(FlagsAttribute), true).Any())
					e = new FlaggedEnumPropertyEditor();
				// Basic data type: Other Enums
				else if (baseType.IsEnum)
					e = new EnumPropertyEditor();
				// Basic data type: String
				else if (baseType == typeof(string))
					e = new StringPropertyEditor();
				// IList
				else if (typeof(System.Collections.IList).IsAssignableFrom(baseType))
					e = new IListPropertyEditor();
				// IList
				else if (typeof(System.Collections.IDictionary).IsAssignableFrom(baseType))
					e = new IDictionaryPropertyEditor();
				// Unknown data type
				else
				{
					// Ask around if any sub-editor can handle it and choose the most specialized
					var availSubProviders = 
						from p in this.subProviders
						where p.IsResponsibleFor(baseType, context) != EditorPriority_None
						orderby p.IsResponsibleFor(baseType, context) descending
						select p;
					IPropertyEditorProvider subProvider = availSubProviders.FirstOrDefault();
					if (subProvider != null)
					{
						e = subProvider.CreateEditor(baseType, context);
						e.EditedType = baseType;
						return e;
					}

					// If not, default to reflection-driven MemberwisePropertyEditor
					e = new MemberwisePropertyEditor();
				}

				e.EditedType = baseType;
				return e;
			}
		}

		private	MainEditorProvider	editorProvider		= new MainEditorProvider();
		private	ControlRenderer		renderer			= null;
		private	PropertyEditor		mainEditor			= null;
		private	PropertyEditor		focusEditor			= null;
		private	List<object>		selectedObjects		= new List<object>();
		private	bool				showNonPublic		= false;
		private	bool				readOnly			= false;
		private	Timer				updateTimer			= null;
		private	int					updateTimerChangeMs	= 0;
		private	bool				updateScheduled		= false;
		private	bool				deferredSizeUpdate	= false;
		private	bool				mouseInside			= false;
		private	MouseButtons		mouseDownTemp		= MouseButtons.None;
		
		public event EventHandler<PropertyEditorValueEventArgs>	EditingFinished = null;
		public event EventHandler<PropertyEditorValueEventArgs>	ValueChanged	= null;

		public IEnumerable<object> Selection
		{
			get { return this.selectedObjects; }
		}
		public bool ReadOnly
		{
			get { return this.readOnly; }
			set
			{
				if (this.readOnly != value)
				{
					this.readOnly = value;
					if (this.mainEditor != null) this.UpdatePropertyEditor();
				}
			}
		}
		public bool ShowNonPublic
		{
			get { return this.showNonPublic; }
			set
			{
				if (this.showNonPublic != value)
				{
					this.showNonPublic = value;
					if (this.mainEditor != null)
					{
						ExpandState state = new ExpandState();
						state.UpdateFrom(this.mainEditor);
						this.DisposePropertyEditor();
						this.UpdateFromObjects();
						state.ApplyTo(this.mainEditor);
					}
				}
			}
		}
		public PropertyEditor MainEditor
		{
			get { return this.mainEditor; }
		}
		public PropertyEditor FocusEditor
		{
			get { return this.focusEditor; }
		}
		public ControlRenderer ControlRenderer
		{
			get { return this.renderer; }
		}
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				// This somehow fixes the "ghost scrollbar" bug. Note that it also isn't animated anymore and doesn't react properly to hover events.
				cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
				return cp;
			}
		}

		public PropertyGrid()
		{
			this.renderer = new ControlRenderer();

			this.updateTimer = new Timer();
			this.updateTimer.Interval = 100;
			this.updateTimer.Tick += this.updateTimer_Tick;
			this.updateTimer.Enabled = true;

			this.AllowDrop = true;
			this.AutoScroll = true;
			//this.DoubleBuffered = true;

			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}
		
		public void SelectObject(object obj, bool readOnly = false, int scheduleMs = 0)
		{
			if (obj == null)
				this.SelectObjects(new object[0], readOnly, scheduleMs);
			else
				this.SelectObjects(new[] {obj}, readOnly, scheduleMs);
		}
		public void SelectObjects(IEnumerable<object> objEnum, bool readOnly = false, int scheduleMs = 0)
		{
			this.selectedObjects.Clear();
			this.selectedObjects.AddRange(objEnum);
			this.readOnly = readOnly;

			this.UpdateFromObjects(scheduleMs);
		}
		public void UpdateFromObjects(int scheduleMs = 0)
		{
			if (scheduleMs > 0)
			{
				this.updateTimerChangeMs = scheduleMs;
				this.updateScheduled = true;
				return;
			}
			else
				this.updateScheduled = false;

			if (this.selectedObjects.Count > 0)
			{
				Type commonType = this.selectedObjects.First().GetType();
				foreach (object o in this.selectedObjects.Skip(1))
				{
					Type curType = o.GetType();
					while (commonType != curType && !commonType.IsAssignableFrom(curType))
						commonType = commonType.BaseType;
				}

				if (this.mainEditor == null || this.mainEditor.EditedType != commonType)
					this.InitPropertyEditor(commonType);
				else
					this.UpdatePropertyEditor();

				this.mainEditor.PerformGetValue();
			}
			else if (this.mainEditor != null)
				this.DisposePropertyEditor();
		}

		protected void InitPropertyEditor(Type type)
		{
			if (this.mainEditor != null) this.DisposePropertyEditor();

			this.focusEditor = null;
			this.mainEditor = this.editorProvider.CreateEditor(type, new ProviderContext(this));
			this.mainEditor.SizeChanged += this.mainEditor_SizeChanged;
			this.mainEditor.ValueChanged += this.mainEditor_ValueChanged;
			this.mainEditor.EditingFinished += this.mainEditor_EditingFinished;
			this.UpdatePropertyEditor();
			this.ConfigureEditor(this.mainEditor);

			if (this.mainEditor is GroupedPropertyEditor)
			{
				GroupedPropertyEditor mainGroupEditor = this.mainEditor as GroupedPropertyEditor;
				mainGroupEditor.Expanded = true;
			}

			this.Invalidate();
		}
		protected void UpdatePropertyEditor()
		{
			if (this.mainEditor == null) return;

			this.mainEditor.ParentGrid = this;
			this.mainEditor.ParentEditor = null;
			this.mainEditor.Hints &= ~(PropertyEditor.HintFlags.HasButton | PropertyEditor.HintFlags.ButtonEnabled);
			this.mainEditor.Getter = this.ValueGetter;
			this.mainEditor.Setter = this.readOnly ? null : (Action<IEnumerable<object>>)this.ValueSetter;
			this.mainEditor.Width = this.ClientSize.Width;
			if (this.mainEditor is GroupedPropertyEditor)
			{
				GroupedPropertyEditor mainGroupEditor = this.mainEditor as GroupedPropertyEditor;
				mainGroupEditor.HeaderStyle = GroupHeaderStyle.Emboss;
				mainGroupEditor.Hints &= ~PropertyEditor.HintFlags.HasExpandCheck;
			}
		}
		protected void DisposePropertyEditor()
		{
			if (this.mainEditor == null) return;

			this.mainEditor.SizeChanged -= this.mainEditor_SizeChanged;
			this.mainEditor.Dispose();
			this.mainEditor = null;
			this.focusEditor = null;

			this.Invalidate();
		}
		private void mainEditor_SizeChanged(object sender, EventArgs e)
		{
			if (this.AutoScrollMinSize.Height != this.mainEditor.Height)
			{
				if (Control.MouseButtons != System.Windows.Forms.MouseButtons.None)
					this.deferredSizeUpdate = true;
				else
					this.AutoScrollMinSize = new Size(0, this.mainEditor.Height);
			}
			this.Invalidate();
		}
		private void mainEditor_ValueChanged(object sender, PropertyEditorValueEventArgs e)
		{
			this.OnValueChanged(e);
		}
		private void mainEditor_EditingFinished(object sender, PropertyEditorValueEventArgs e)
		{
			this.OnEditingFinished(e);
		}

		public void RegisterEditorProvider(IPropertyEditorProvider provider)
		{
			if (this.editorProvider.SubProviders.Contains(provider)) return;
			this.editorProvider.SubProviders.Add(provider);
		}
		public void RegisterEditorProvider(IEnumerable<IPropertyEditorProvider> provider)
		{
			foreach (IPropertyEditorProvider prov in provider)
				this.RegisterEditorProvider(prov);
		}
		public PropertyEditor CreateEditor(Type editedType, PropertyEditor parentEditor)
		{
			if (parentEditor == null) throw new ArgumentNullException("parentEditor");
			if (parentEditor.ParentGrid != this) throw new ArgumentException("The specified editor is not a child of this PropertyGrid", "parentEditor");
			PropertyEditor e = this.editorProvider.CreateEditor(editedType, new ProviderContext(this, parentEditor));
			e.EditedType = editedType;
			e.ParentGrid = this;
			return e;
		}
		public virtual void ConfigureEditor(PropertyEditor editor, object configureData = null)
		{
			editor.ConfigureEditor(configureData);
		}
		public virtual object CreateObjectInstance(Type objectType)
		{
			return ReflectionHelper.CreateInstanceOf(objectType);
		}
		internal protected virtual void PrepareSetValue() {}
		internal protected virtual void PostSetValue() {}
		protected virtual void OnValueChanged(PropertyEditorValueEventArgs e)
		{
			if (this.ValueChanged != null)
				this.ValueChanged(this, e);
		}
		protected virtual void OnEditingFinished(PropertyEditorValueEventArgs e)
		{
			if (this.EditingFinished != null)
				this.EditingFinished(this, e);
		}

		public void Focus(PropertyEditor editor)
		{
			if (this.focusEditor == editor) return;
			editor = this.GetFocusReciever(editor);

			if (this.focusEditor != null && this.Focused) this.focusEditor.OnLostFocus(EventArgs.Empty);

			this.focusEditor = editor;
			this.ScrollToEditor(this.focusEditor);

			if (this.focusEditor != null)
			{
				if (!this.Focused)
					this.Focus();
				else
					this.focusEditor.OnGotFocus(EventArgs.Empty);
			}

			//this.Invalidate();
		}
		public PropertyEditor GetFocusReciever(PropertyEditor primary, bool secondaryNext = true)
		{
			if (primary == null) return null;
			while (!primary.CanGetFocus)
			{
				if (secondaryNext)
				{
					if (primary.ChildrenDeep.Any(e => e.CanGetFocus))
						primary = primary.ChildrenDeep.FirstOrDefault(e => e.CanGetFocus);
					else if (primary.NextEditor != null)
						primary = primary.NextEditor;
					else if (primary.ParentEditor != null)
						primary = this.GetFocusReciever(primary.ParentEditor.NextEditor, secondaryNext);
					else
						return null;
				}
				else
				{
					if (primary.PrevEditor != null)
						primary = primary.PrevEditor;
					else
						primary = this.GetFocusReciever(primary.ParentEditor, secondaryNext);
				}
				if (primary == null) break;
			}
			return primary;
		}
		public PropertyEditor PickEditorAt(int x, int y, bool scrolled = false)
		{
			if (this.mainEditor == null) return null;
			if (scrolled)
			{
				x -= this.AutoScrollPosition.X;
				y -= this.AutoScrollPosition.Y;
			}
			return this.mainEditor.PickEditorAt(x - this.ClientRectangle.X, y - this.ClientRectangle.Y);
		}
		public Point GetEditorLocation(PropertyEditor editor, bool scrolled = false)
		{
			if (this.mainEditor == null) return Point.Empty;
			Point result = this.mainEditor.GetChildLocation(editor);
			result.X += this.ClientRectangle.X;
			result.Y += this.ClientRectangle.Y;
			if (scrolled)
			{
				result.X += this.AutoScrollPosition.X;
				result.Y += this.AutoScrollPosition.Y;
			}
			return result;
		}
		public void ScrollToEditor(PropertyEditor editor)
		{
			Point editorLoc = this.GetEditorLocation(editor);
			Rectangle editorRect = new Rectangle(editorLoc, editor.Size);
			Point scrollPos = this.AutoScrollPosition;
			
			if (editorRect.Bottom > this.ClientRectangle.Y - scrollPos.Y + this.ClientRectangle.Height)
				scrollPos.Y = -editorRect.Bottom + this.ClientRectangle.Y + this.ClientRectangle.Height;
			if (editorRect.Y < this.ClientRectangle.Y - scrollPos.Y)
				scrollPos.Y = this.ClientRectangle.Y - editorRect.Y;

			this.AutoScrollPosition = new Point(-scrollPos.X, -scrollPos.Y);
		}

		protected IEnumerable<object> ValueGetter()
		{
			return this.selectedObjects;
		}
		protected void ValueSetter(object val)
		{
			// Don't set anything. The PropertyGrid doesn't actually contain any value data.
			// What should be changed here anyway? The selection?
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);

			GraphicsState originalState = e.Graphics.Save();
			if (this.mainEditor != null)
			{
				Rectangle editorRect = new Rectangle(this.ClientRectangle.Location, this.mainEditor.Size);
				editorRect.Intersect(this.ClientRectangle);
				RectangleF clipRect = editorRect;
				clipRect.Intersect(e.Graphics.ClipBounds);
				e.Graphics.SetClip(clipRect);
				e.Graphics.TranslateTransform(this.ClientRectangle.X, this.ClientRectangle.Y + this.AutoScrollPosition.Y);
				this.mainEditor.OnPaint(e);
			}
			e.Graphics.Restore(originalState);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			if (this.mouseInside) return; // Ignore if called multiple times - Windows.Forms bug due to dropdown stuff?

			base.OnMouseEnter(e);
			this.mouseInside = true;

			if (this.updateScheduled) this.UpdateFromObjects();

			if (this.mainEditor != null)
			{
				this.mainEditor.OnMouseEnter(EventArgs.Empty);
			}
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			if (!this.mouseInside) return; // Ignore if called before MouseEnter - Windows.Forms bug due to dropdown stuff?

			base.OnMouseLeave(e);
			this.mouseInside = false;

			if (this.mainEditor != null)
			{
				this.mainEditor.OnMouseLeave(EventArgs.Empty);
			}
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (!this.mouseInside) this.OnMouseEnter(EventArgs.Empty);
			//Console.WriteLine("OnMouseMove");

			base.OnMouseMove(e);

			if (this.mainEditor != null)
			{
				this.mainEditor.OnMouseMove(new MouseEventArgs(
					e.Button, 
					e.Clicks, 
					e.X - this.ClientRectangle.X, 
					e.Y - this.ClientRectangle.Y - this.AutoScrollPosition.Y, 
					e.Delta));
			}
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Emulate stuff if called in wrong order - Windows.Forms bug due to dropdown stuff?
			if (!this.mouseInside) this.OnMouseMove(e);
			if ((this.mouseDownTemp & e.Button) == e.Button) this.OnMouseUp(new MouseEventArgs((this.mouseDownTemp & e.Button), e.Clicks, e.X, e.Y, e.Delta));

			base.OnMouseDown(e);
			this.mouseDownTemp |= e.Button;

			if (this.mainEditor != null)
			{
				this.mainEditor.OnMouseDown(new MouseEventArgs(
					e.Button, 
					e.Clicks, 
					e.X - this.ClientRectangle.X, 
					e.Y - this.ClientRectangle.Y - this.AutoScrollPosition.Y, 
					e.Delta));
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			// Emulate stuff if called in wrong order - Windows.Forms bug due to dropdown stuff?
			if (!this.mouseInside) this.OnMouseMove(e);
			if ((this.mouseDownTemp & e.Button) == MouseButtons.None) this.OnMouseDown(new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));

			base.OnMouseUp(e);
			this.mouseDownTemp &= ~e.Button;

			if (this.deferredSizeUpdate && Control.MouseButtons == System.Windows.Forms.MouseButtons.None)
			{
				this.mainEditor_SizeChanged(this, EventArgs.Empty);
				this.deferredSizeUpdate = false;
			}

			if (this.mainEditor != null)
			{
				this.mainEditor.OnMouseUp(new MouseEventArgs(
					e.Button, 
					e.Clicks, 
					e.X - this.ClientRectangle.X, 
					e.Y - this.ClientRectangle.Y - this.AutoScrollPosition.Y, 
					e.Delta));
			}
		}
		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (this.mainEditor != null)
			{
				this.mainEditor.OnMouseClick(new MouseEventArgs(
					e.Button, 
					e.Clicks, 
					e.X - this.ClientRectangle.X, 
					e.Y - this.ClientRectangle.Y - this.AutoScrollPosition.Y, 
					e.Delta));
			}
		}
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);

			if (this.mainEditor != null)
			{
				this.mainEditor.OnMouseDoubleClick(new MouseEventArgs(
					e.Button, 
					e.Clicks, 
					e.X - this.ClientRectangle.X, 
					e.Y - this.ClientRectangle.Y - this.AutoScrollPosition.Y, 
					e.Delta));
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData.HasFlag(Keys.Up) || keyData.HasFlag(Keys.Down) || keyData.HasFlag(Keys.Left) || keyData.HasFlag(Keys.Right) || keyData.HasFlag(Keys.Tab))
			{
				KeyEventArgs args = new KeyEventArgs(keyData);
				args.Handled = false;
				this.OnKeyDown(args);
				if (args.Handled) return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (this.focusEditor != null)
			{
				PropertyEditor current = this.focusEditor;
				while (current != null)
				{
					current.OnKeyDown(e);
					if (e.Handled) break;
					current = current.ParentEditor;
				}
			}

			if (!e.Handled)
			{
				if (this.focusEditor != null)
				{
					if (e.KeyCode == Keys.Down)
					{
						PropertyEditor current = this.focusEditor;
						PropertyEditor next = current.NextEditor;
						if (next == null)
							next = current.Children.FirstOrDefault();
						if (next == null && current.ParentEditor != null)
							next = current.ParentEditor.NextEditor;

						next = this.GetFocusReciever(next, true);
						if (next != null) next.Focus();
						e.Handled = true;
					}
					else if (e.KeyCode == Keys.Up)
					{
						PropertyEditor current = this.focusEditor;
						PropertyEditor prev = current.PrevEditor;
						if (prev == null)
							prev = current.ParentEditor;

						prev = this.GetFocusReciever(prev, false);
						if (prev != null) prev.Focus();
						e.Handled = true;
					}
					else if (e.KeyCode == Keys.Left)
					{
						PropertyEditor current = this.focusEditor;
						while (current != null)
						{
							current = current.ParentEditor;
							while (current != null && !current.CanGetFocus) current = current.ParentEditor;
							if (current != null)
							{
								current.Focus();
								break;
							}
						}
						e.Handled = true;
					}
					else if (e.KeyCode == Keys.Right)
					{
						if (this.focusEditor != null)
						{
							if (this.focusEditor is GroupedPropertyEditor)
							{
								(this.focusEditor as GroupedPropertyEditor).Expanded = true;
							}
							PropertyEditor current = null;
							current = this.focusEditor.Children.FirstOrDefault(editor => editor.CanGetFocus);
							if (current == null) current = this.focusEditor.ChildrenDeep.FirstOrDefault(editor => editor.CanGetFocus);
							if (current != null) current.Focus();
						}
						e.Handled = true;
					}
					else if (e.KeyCode == Keys.PageUp || e.KeyCode == Keys.Home)
					{
						if (this.focusEditor.ParentEditor != null)
						{
							PropertyEditor current = this.focusEditor.ParentEditor;
							current = current.Children.FirstOrDefault(editor => editor.CanGetFocus);
							if (current != null) current.Focus();
						}
						e.Handled = true;
					}
					else if (e.KeyCode == Keys.PageDown || e.KeyCode == Keys.End)
					{
						if (this.focusEditor.ParentEditor != null)
						{
							PropertyEditor current = this.focusEditor.ParentEditor;
							current = current.Children.LastOrDefault(editor => editor.CanGetFocus);
							if (current != null) current.Focus();
						}
						e.Handled = true;
					}
				}
			}
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (this.focusEditor != null)
			{
				PropertyEditor current = this.focusEditor;
				while (current != null)
				{
					current.OnKeyUp(e);
					if (e.Handled) break;
					current = current.ParentEditor;
				}
			}
		}
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (this.focusEditor != null)
			{
				PropertyEditor current = this.focusEditor;
				while (current != null)
				{
					current.OnKeyPress(e);
					if (e.Handled) break;
					current = current.ParentEditor;
				}
			}
		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);

			if (this.mainEditor != null)
			{
				Point localPoint = this.PointToClient(new Point(e.X, e.Y));
				DragEventArgs subEvent = new DragEventArgs(
					e.Data, 
					e.KeyState, 
					localPoint.X - this.ClientRectangle.X,
					localPoint.Y - this.ClientRectangle.Y - this.AutoScrollPosition.Y,
					e.AllowedEffect,
					e.Effect);
				this.mainEditor.OnDragEnter(subEvent);
				e.Effect = subEvent.Effect;
			}
		}
		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);

			if (this.mainEditor != null)
			{
				this.mainEditor.OnDragLeave(EventArgs.Empty);
			}
		}
		protected override void OnDragOver(DragEventArgs e)
		{
			//Console.WriteLine("OnDragOver");
			base.OnDragOver(e);

			if (this.mainEditor != null)
			{
				Point localPoint = this.PointToClient(new Point(e.X, e.Y));
				DragEventArgs subEvent = new DragEventArgs(
					e.Data, 
					e.KeyState, 
					localPoint.X - this.ClientRectangle.X,
					localPoint.Y - this.ClientRectangle.Y - this.AutoScrollPosition.Y,
					e.AllowedEffect,
					e.Effect);
				this.mainEditor.OnDragOver(subEvent);
				e.Effect = subEvent.Effect;
			}
		}
		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);

			if (this.mainEditor != null)
			{
				Point localPoint = this.PointToClient(new Point(e.X, e.Y));
				DragEventArgs subEvent = new DragEventArgs(
					e.Data, 
					e.KeyState, 
					localPoint.X - this.ClientRectangle.X,
					localPoint.Y - this.ClientRectangle.Y - this.AutoScrollPosition.Y,
					e.AllowedEffect,
					e.Effect);
				this.mainEditor.OnDragDrop(subEvent);
				e.Effect = subEvent.Effect;
			}
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if (this.focusEditor != null)
				this.focusEditor.OnGotFocus(EventArgs.Empty);
			else
				this.Focus(this.mainEditor);
			//this.Invalidate();
		}
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if (this.focusEditor != null) this.focusEditor.OnLostFocus(e);

			// Emulate leaving mouse if losing focus to something that might be a dropdown popup
			if (!Application.OpenForms.OfType<Form>().Any(c => c.Focused || c.ContainsFocus))
				this.OnMouseLeave(EventArgs.Empty);
			//this.Invalidate();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.UpdatePropertyEditor();
		}
		protected override void OnScroll(ScrollEventArgs se)
		{
			base.OnScroll(se);
			this.Invalidate();
			if (this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
			{
				MouseEventArgs childArgs = new MouseEventArgs(Control.MouseButtons, 0, Control.MousePosition.X, Control.MousePosition.Y, 0);
				this.OnMouseMove(childArgs);
			}
		}
		protected override Point ScrollToControl(Control activeControl)
		{
			// Prevent AutoScroll on focus or content resize - will always scroll to top.
			// Solution: Just don't scroll. Won't be needed here anyway.
			return this.AutoScrollPosition;
			//return base.ScrollToControl(activeControl);
		}
		private void updateTimer_Tick(object sender, EventArgs e)
		{
			if (this.updateScheduled)
			{
				this.updateTimerChangeMs -= this.updateTimer.Interval;
				if (this.updateTimerChangeMs <= 0) this.UpdateFromObjects();
			}
		}
	}
}
