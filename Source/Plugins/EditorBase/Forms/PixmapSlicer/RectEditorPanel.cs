using System;
using System.Drawing;
using System.Windows.Forms;
using AdamsLair.WinForms.PropertyEditing;
using Duality.Resources;
using Duality.Editor.Controls;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer
{
	/// <summary>
	/// Provides a panel for editing a single <see cref="RectAtlas"/> index.
	/// </summary>
	public partial class RectEditorPanel : UserControl
	{
		private readonly DualitorPropertyGrid propertyGrid;
		private PropertyEditor itemEditor;

		private Pixmap pixmap;
		private int index;

		public RectEditorPanel()
		{
			this.InitializeComponent();

			this.propertyGrid = new DualitorPropertyGrid
			{
				Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
				Location = new Point(this.Padding.Left, this.Padding.Top),
				Width = this.Width - this.Padding.Left - this.Padding.Right,
				Height = this.Height - this.Padding.Top - this.Padding.Bottom
			};

			this.BackColor = this.propertyGrid.Renderer.ColorBackground;

			this.Controls.Add(this.propertyGrid);
		}

		/// <summary>
		/// Sets the <see cref="Pixmap"/> and index of the pixmap being edited.
		/// </summary>
		/// <param name="pixmap">The <see cref="Pixmap"/> being edited.</param>
		/// <param name="index">The index within the pixmap that is being edited.</param>
		public void SetItem(Pixmap pixmap, int index)
		{
			this.pixmap = pixmap;
			this.index = index;

			var wrapper = new RectAtlasItemWrapper(this.ItemGetter, this.ItemSetter);
			this.propertyGrid.SelectObject(wrapper);

			this.itemEditor = this.propertyGrid.MainEditor;
			this.itemEditor.ForceWriteBack = true;
			// Don't want a header of any kind.
			((GroupedPropertyEditor)this.itemEditor).HeaderHeight = 0;
			this.Height = this.itemEditor.Height + this.Padding.Top + this.Padding.Bottom;

			this.itemEditor.PerformGetValue();
		}

		protected override void OnInvalidated(InvalidateEventArgs e)
		{
			base.OnInvalidated(e);

			if (this.itemEditor != null)
				this.itemEditor.PerformGetValue();
		}

		/// <summary>
		/// Returns the item being edited or a default item if the pixmap being
		/// edited is null or the index being edited is out of range.
		/// </summary>
		private RectAtlas.RectAtlasItem ItemGetter()
		{
			if (this.pixmap == null || this.index < 0 || this.index >= this.pixmap.Atlas.Count)
				return default(RectAtlas.RectAtlasItem);

			return new RectAtlas.RectAtlasItem
			{
				Rect = this.pixmap.Atlas[this.index],
				Pivot = this.pixmap.Atlas.GetPivot(this.index),
				Tag = this.pixmap.Atlas.GetTag(this.index)
			};
		}

		/// <summary>
		/// Sets the item at the edited index within the pixmap to the given item.
		/// </summary>
		private void ItemSetter(RectAtlas.RectAtlasItem item)
		{
			if (this.pixmap == null || this.index < 0 || this.index >= this.pixmap.Atlas.Count)
				return;

			this.pixmap.Atlas[this.index] = item.Rect;
			this.pixmap.Atlas.SetPivot(this.index, item.Pivot);
			this.pixmap.Atlas.SetTag(this.index, item.Tag);

			// Make sure other parts of the editor can respond to this change
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmap));
		}

		/// <summary>
		/// Wraps a <see cref="RectAtlas.RectAtlasItem"/> in a reference type.
		/// </summary>
		private class RectAtlasItemWrapper
		{
			private readonly Func<RectAtlas.RectAtlasItem> getter;
			private readonly Action<RectAtlas.RectAtlasItem> setter;

			public Rect Rect
			{
				get { return this.getter().Rect; }
				set
				{
					RectAtlas.RectAtlasItem item = this.getter();
					item.Rect = value;
					this.setter(item);
				}
			}

			public Vector2 Pivot
			{
				get { return this.getter().Pivot; }
				set
				{
					RectAtlas.RectAtlasItem item = this.getter();
					item.Pivot = value;
					this.setter(item);
				}
			}

			public string Tag
			{
				get { return this.getter().Tag; }
				set
				{
					RectAtlas.RectAtlasItem item = this.getter();
					item.Tag = value;
					this.setter(item);
				}
			}

			public RectAtlasItemWrapper(Func<RectAtlas.RectAtlasItem> getter, Action<RectAtlas.RectAtlasItem> setter)
			{
				this.getter = getter;
				this.setter = setter;
			}
		}
	}
}
