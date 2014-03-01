using System;
using System.Linq;

using Duality;

namespace Duality.Editor.Controls.PropertyEditors
{
	[PropertyEditorAssignment(typeof(Range))]
	public class RangePropertyEditor : VectorPropertyEditor
	{
		public override object DisplayedValue
		{
			get 
			{ 
				return new Range((float)this.editor[0].Value, (float)this.editor[1].Value);
			}
		}


		public RangePropertyEditor() : base(2, 1)
		{
			this.editor[0].Edited += this.editorMin_Edited;
			this.editor[1].Edited += this.editorMax_Edited;
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
				float avgX = valNotNull.Average(o => ((Range)o).MinValue);
				float avgY = valNotNull.Average(o => ((Range)o).MaxValue);

				this.editor[0].Value = MathF.SafeToDecimal(avgX);
				this.editor[1].Value = MathF.SafeToDecimal(avgY);

				this.multiple[0] = (values.Any(o => o == null) || values.Any(o => ((Range)o).MinValue != avgX));
				this.multiple[1] = (values.Any(o => o == null) || values.Any(o => ((Range)o).MaxValue != avgY));
			}
			this.EndUpdate();
		}

		private void editorMin_Edited(object sender, EventArgs e)
		{
			if (this.IsUpdating) return;
			if (this.Disposed) return;
			if (!this.ReadOnly)
			{
				object[] values = this.GetValue().ToArray();
				Range newVal = (Range)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Range oldVal = (Range)values[i];
						values[i] = new Range(newVal.MinValue, oldVal.MaxValue);
					}
				}
				this.SetValues(values);
			}
			this.PerformGetValue();
		}
		private void editorMax_Edited(object sender, EventArgs e)
		{
			if (this.IsUpdating) return;
			if (this.Disposed) return;
			if (!this.ReadOnly)
			{
				object[] values = this.GetValue().ToArray();
				Range newVal = (Range)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Range oldVal = (Range)values[i];
						values[i] = new Range(oldVal.MinValue, newVal.MaxValue);
					}
				}
				this.SetValues(values);
			}
			this.PerformGetValue();
		}
	}
}

