using System;
using System.Linq;

using Vector4 = OpenTK.Vector4;

using Duality;

namespace Duality.Editor.Controls.PropertyEditors
{
	[PropertyEditorAssignment(typeof(Vector4))]
	public class Vector4PropertyEditor : VectorPropertyEditor
	{
		public override object DisplayedValue
		{
			get 
			{ 
				return new Vector4((float)this.editor[0].Value, (float)this.editor[1].Value, (float)this.editor[2].Value, (float)this.editor[3].Value);
			}
		}


		public Vector4PropertyEditor() : base(4, 2)
		{
			this.editor[0].Edited += this.editorX_Edited;
			this.editor[1].Edited += this.editorY_Edited;
			this.editor[2].Edited += this.editorZ_Edited;
			this.editor[3].Edited += this.editorW_Edited;
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
				float avgX = valNotNull.Average(o => ((Vector4)o).X);
				float avgY = valNotNull.Average(o => ((Vector4)o).Y);
				float avgZ = valNotNull.Average(o => ((Vector4)o).Z);
				float avgW = valNotNull.Average(o => ((Vector4)o).W);

				this.editor[0].Value = MathF.SafeToDecimal(avgX);
				this.editor[1].Value = MathF.SafeToDecimal(avgY);
				this.editor[2].Value = MathF.SafeToDecimal(avgZ);
				this.editor[3].Value = MathF.SafeToDecimal(avgW);

				this.multiple[0] = (values.Any(o => o == null) || values.Any(o => ((Vector4)o).X != avgX));
				this.multiple[1] = (values.Any(o => o == null) || values.Any(o => ((Vector4)o).Y != avgY));
				this.multiple[2] = (values.Any(o => o == null) || values.Any(o => ((Vector4)o).Z != avgZ));
				this.multiple[3] = (values.Any(o => o == null) || values.Any(o => ((Vector4)o).W != avgW));
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
				Vector4 newVal = (Vector4)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Vector4 oldVal = (Vector4)values[i];
						values[i] = new Vector4(newVal.X, oldVal.Y, oldVal.Z, oldVal.W);
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
				Vector4 newVal = (Vector4)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Vector4 oldVal = (Vector4)values[i];
						values[i] = new Vector4(oldVal.X, newVal.Y, oldVal.Z, oldVal.W);
					}
				}
				this.SetValues(values);
			}
			this.PerformGetValue();
		}
		private void editorZ_Edited(object sender, EventArgs e)
		{
			if (this.IsUpdating) return;
			if (this.Disposed) return;
			if (!this.ReadOnly)
			{
				object[] values = this.GetValue().ToArray();
				Vector4 newVal = (Vector4)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Vector4 oldVal = (Vector4)values[i];
						values[i] = new Vector4(oldVal.X, oldVal.Y, newVal.Z, oldVal.W);
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
				Vector4 newVal = (Vector4)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Vector4 oldVal = (Vector4)values[i];
						values[i] = new Vector4(oldVal.X, oldVal.Y, oldVal.Z, newVal.W);
					}
				}
				this.SetValues(values);
			}
			this.PerformGetValue();
		}
	}
}

