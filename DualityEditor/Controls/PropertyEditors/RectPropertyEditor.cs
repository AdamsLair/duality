using System;
using System.Linq;

using AdamsLair.WinForms.EditorTemplates;

using Duality;

namespace Duality.Editor.Controls.PropertyEditors
{
	[PropertyEditorAssignment(typeof(Rect))]
	public class RectPropertyEditor : VectorPropertyEditor
	{
		public override object DisplayedValue
		{
			get 
			{ 
				return new Rect((float)this.editor[0].Value, (float)this.editor[1].Value, (float)this.editor[2].Value, (float)this.editor[3].Value);
			}
		}


		public RectPropertyEditor() : base(4, 2)
		{
			this.editor[0].Edited += this.editorX_Edited;
			this.editor[1].Edited += this.editorY_Edited;
			this.editor[2].Edited += this.editorW_Edited;
			this.editor[3].Edited += this.editorH_Edited;
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
				this.editor[2].Value = 0;
				this.editor[3].Value = 0;
			}
			else
			{
				var valNotNull = values.NotNull();
				float avgX = valNotNull.Average(o => ((Rect)o).X);
				float avgY = valNotNull.Average(o => ((Rect)o).Y);
				float avgW = valNotNull.Average(o => ((Rect)o).W);
				float avgH = valNotNull.Average(o => ((Rect)o).H);

				this.editor[0].Value = MathF.SafeToDecimal(avgX);
				this.editor[1].Value = MathF.SafeToDecimal(avgY);
				this.editor[2].Value = MathF.SafeToDecimal(avgW);
				this.editor[3].Value = MathF.SafeToDecimal(avgH);

				this.multiple[0] = (values.Any(o => o == null) || values.Any(o => ((Rect)o).X != avgX));
				this.multiple[1] = (values.Any(o => o == null) || values.Any(o => ((Rect)o).Y != avgY));
				this.multiple[2] = (values.Any(o => o == null) || values.Any(o => ((Rect)o).W != avgW));
				this.multiple[3] = (values.Any(o => o == null) || values.Any(o => ((Rect)o).H != avgH));
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
				Rect newVal = (Rect)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Rect oldVal = (Rect)values[i];
						values[i] = new Rect(newVal.X, oldVal.Y, oldVal.W, oldVal.H);
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
				Rect newVal = (Rect)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Rect oldVal = (Rect)values[i];
						values[i] = new Rect(oldVal.X, newVal.Y, oldVal.W, oldVal.H);
					}
				}
				this.SetValues(values);
			}
			this.PerformGetValue();
		}
		private void editorW_Edited(object sender, EventArgs e)
		{
			if (this.IsUpdating) return;
			if (this.Disposed) return;
			if (!this.ReadOnly)
			{
				object[] values = this.GetValue().ToArray();
				Rect newVal = (Rect)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Rect oldVal = (Rect)values[i];
						values[i] = new Rect(oldVal.X, oldVal.Y, newVal.W, oldVal.H);
					}
				}
				this.SetValues(values);
			}
			this.PerformGetValue();
		}
		private void editorH_Edited(object sender, EventArgs e)
		{
			if (this.IsUpdating) return;
			if (this.Disposed) return;
			if (!this.ReadOnly)
			{
				object[] values = this.GetValue().ToArray();
				Rect newVal = (Rect)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Rect oldVal = (Rect)values[i];
						values[i] = new Rect(oldVal.X, oldVal.Y, oldVal.W, newVal.H);
					}
				}
				this.SetValues(values);
			}
			this.PerformGetValue();
		}
	}
}

