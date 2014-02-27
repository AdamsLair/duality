using System;
using System.Linq;

using Vector2 = OpenTK.Vector2;

using Duality;

namespace Duality.Editor.Controls.PropertyEditors
{
	[PropertyEditorAssignment(typeof(Vector2))]
	public class Vector2PropertyEditor : VectorPropertyEditor
	{
		public override object DisplayedValue
		{
			get 
			{ 
				return new Vector2((float)this.editor[0].Value, (float)this.editor[1].Value);
			}
		}


		public Vector2PropertyEditor() : base(2, 1)
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
				float avgX = valNotNull.Average(o => ((Vector2)o).X);
				float avgY = valNotNull.Average(o => ((Vector2)o).Y);

				this.editor[0].Value = MathF.SafeToDecimal(avgX);
				this.editor[1].Value = MathF.SafeToDecimal(avgY);

				this.multiple[0] = (values.Any(o => o == null) || values.Any(o => ((Vector2)o).X != avgX));
				this.multiple[1] = (values.Any(o => o == null) || values.Any(o => ((Vector2)o).Y != avgY));
			}
			this.EndUpdate();
		}

		private void editorX_Edited(object sender, EventArgs e)
		{
			if (this.IsUpdating) return;
			if (this.Disposed) return;
			if (!this.ReadOnly)
			{
				object[] values = this.GetValue().ToArray();
				Vector2 newVal = (Vector2)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Vector2 oldVal = (Vector2)values[i];
						values[i] = new Vector2(newVal.X, oldVal.Y);
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
				Vector2 newVal = (Vector2)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Vector2 oldVal = (Vector2)values[i];
						values[i] = new Vector2(oldVal.X, newVal.Y);
					}
				}
				this.SetValues(values);
			}
			this.PerformGetValue();
		}
	}
}

