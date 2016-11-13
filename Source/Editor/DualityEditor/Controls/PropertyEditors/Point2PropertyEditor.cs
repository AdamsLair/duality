using System;
using System.Linq;

using Duality;
using AdamsLair.WinForms.PropertyEditing.Templates;

namespace Duality.Editor.Controls.PropertyEditors
{
	[PropertyEditorAssignment(typeof(Point2))]
	public class Point2PropertyEditor : VectorPropertyEditor
	{
		public override object DisplayedValue
		{
			get 
			{ 
				return new Point2((int)this.editor[0].Value, (int)this.editor[1].Value);
			}
		}

		public Point2PropertyEditor() : base(2, 1)
		{
			this.editor[0].Edited += this.editorX_Edited;
			this.editor[1].Edited += this.editorY_Edited;
		}

		protected override void OnGetValue()
		{
			base.OnGetValue();
			object[] values = this.GetValue().ToArray();

			this.BeginUpdate();
			if (!values.Any())
			{
				this.editor[0].Value = 0;
				this.editor[1].Value = 0;
			}
			else
			{
				var valNotNull = values.NotNull();
				float avgX = valNotNull.Average(o => (float)((Point2)o).X);
				float avgY = valNotNull.Average(o => (float)((Point2)o).Y);

				this.editor[0].Value = MathF.SafeToDecimal(avgX);
				this.editor[1].Value = MathF.SafeToDecimal(avgY);

				this.multiple[0] = (values.Any(o => o == null) || values.Any(o => ((Point2)o).X != avgX));
				this.multiple[1] = (values.Any(o => o == null) || values.Any(o => ((Point2)o).Y != avgY));
			}
			this.EndUpdate();
		}
		protected override void ApplyDefaultSubEditorConfig(NumericEditorTemplate subEditor)
		{
			base.ApplyDefaultSubEditorConfig(subEditor);
			subEditor.DecimalPlaces = 0;
			subEditor.Increment = 1;
		}

		private void editorX_Edited(object sender, EventArgs e)
		{
			if (this.IsUpdating) return;
			if (this.Disposed) return;
			if (!this.ReadOnly)
			{
				object[] values = this.GetValue().ToArray();
				Point2 newVal = (Point2)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Point2 oldVal = (Point2)values[i];
						values[i] = new Point2(newVal.X, oldVal.Y);
					}
				}
				this.SetValues(values);
			}
			this.PerformGetValue();
		}
		private void editorY_Edited(object sender, EventArgs e)
		{
			if (this.IsUpdating) return;
			if (this.Disposed) return;
			if (!this.ReadOnly)
			{
				object[] values = this.GetValue().ToArray();
				Point2 newVal = (Point2)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Point2 oldVal = (Point2)values[i];
						values[i] = new Point2(oldVal.X, newVal.Y);
					}
				}
				this.SetValues(values);
			}
			this.PerformGetValue();
		}
	}
}

