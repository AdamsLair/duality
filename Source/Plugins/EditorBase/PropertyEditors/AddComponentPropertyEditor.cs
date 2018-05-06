using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AdamsLair.WinForms.PropertyEditing;
using Duality.Editor.Forms;
using Duality.Editor.UndoRedoActions;
using ButtonState = AdamsLair.WinForms.Drawing.ButtonState;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	/// <summary>
	/// A <see cref="PropertyEditor"/> that provides a button
	/// for adding a component to a <see cref="GameObject"/>
	/// </summary>
	public class AddComponentPropertyEditor : PropertyEditor
	{
		private Rectangle	rectButton		= Rectangle.Empty;
		private bool		buttonHovered	= false;
		private bool		buttonPressed	= false;

		public override object DisplayedValue
		{
			get { return this.GetValue(); }
		}

		public AddComponentPropertyEditor()
		{
			this.Hints = HintFlags.None;
			this.Height += 8;
		}

		protected override void UpdateGeometry()
		{
			base.UpdateGeometry();

			this.rectButton = new Rectangle(
				this.ClientRectangle.X + 5, this.ClientRectangle.Y + 5,
				this.ClientRectangle.Width - 10, this.ClientRectangle.Height - 10);
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			ButtonState buttonState = this.buttonPressed
				? ButtonState.Pressed
				: this.buttonHovered
					? ButtonState.Hot
					: ButtonState.Normal;

			if (this.ReadOnly)
				buttonState = ButtonState.Disabled;

			this.ControlRenderer.DrawButton(
				e.Graphics, this.rectButton,
				buttonState, "Add Component");
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (this.ReadOnly)
				return;

			bool buttonContainsMouse = this.rectButton.Contains(e.Location);
			if (buttonContainsMouse != this.buttonHovered)
			{
				this.buttonHovered = buttonContainsMouse;
				this.Invalidate();
			}
		}
		protected override void OnMouseLeave(System.EventArgs e)
		{
			base.OnMouseLeave(e);

			if (this.ReadOnly)
				return;

			this.buttonHovered = false;
			this.Invalidate();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (this.ReadOnly)
				return;

			if (this.buttonHovered)
			{
				this.buttonPressed = true;
				this.Invalidate();
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (this.ReadOnly)
				return;

			if (this.buttonPressed)
			{
				this.OnAddComponentClicked();
				this.buttonPressed = false;
				this.Invalidate();
			}
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (this.ReadOnly)
				return;

			if (e.KeyCode == Keys.Enter)
			{
				this.OnAddComponentClicked();
				this.Invalidate();
			}
		}
		private void OnAddComponentClicked()
		{
			SelectionDialog compTypeSelector = new SelectionDialog
			{
				FilteredType = typeof(Component),
				SelectType = true
			};
			DialogResult result = compTypeSelector.ShowDialog();

			if (result == DialogResult.OK)
			{
				UndoRedoManager.BeginMacro("Add Component");
				foreach (GameObject obj in this.GetValue().Cast<GameObject>())
				{
					UndoRedoManager.Do(new CreateComponentAction(obj, compTypeSelector.TypeReference));
				}
				UndoRedoManager.EndMacro("Add Component");

				this.PerformGetValue();
				this.ParentGrid.Invalidate();
			}
		}
	}
}
