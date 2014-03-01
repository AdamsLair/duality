using System;
using System.Linq;

using Vector3 = OpenTK.Vector3;

using Duality;

namespace Duality.Editor.Controls.PropertyEditors
{
	[PropertyEditorAssignment(typeof(Vector3))]
	public class Vector3PropertyEditor : VectorPropertyEditor
	{
		public override object DisplayedValue
		{
			get 
			{ 
				return new Vector3((float)this.editor[0].Value, (float)this.editor[1].Value, (float)this.editor[2].Value);
			}
		}


		public Vector3PropertyEditor() : base(3, 1)
		{
			this.editor[0].Edited += this.editorX_Edited;
			this.editor[1].Edited += this.editorY_Edited;
			this.editor[2].Edited += this.editorZ_Edited;
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
			}
			else
			{
				var valNotNull = values.NotNull();
				float avgX = valNotNull.Average(o => ((Vector3)o).X);
				float avgY = valNotNull.Average(o => ((Vector3)o).Y);
				float avgZ = valNotNull.Average(o => ((Vector3)o).Z);

				this.editor[0].Value = MathF.SafeToDecimal(avgX);
				this.editor[1].Value = MathF.SafeToDecimal(avgY);
				this.editor[2].Value = MathF.SafeToDecimal(avgZ);

				this.multiple[0] = (values.Any(o => o == null) || values.Any(o => ((Vector3)o).X != avgX));
				this.multiple[1] = (values.Any(o => o == null) || values.Any(o => ((Vector3)o).Y != avgY));
				this.multiple[2] = (values.Any(o => o == null) || values.Any(o => ((Vector3)o).Z != avgZ));
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
				Vector3 newVal = (Vector3)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Vector3 oldVal = (Vector3)values[i];
						values[i] = new Vector3(newVal.X, oldVal.Y, oldVal.Z);
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
				Vector3 newVal = (Vector3)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Vector3 oldVal = (Vector3)values[i];
						values[i] = new Vector3(oldVal.X, newVal.Y, oldVal.Z);
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
				Vector3 newVal = (Vector3)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						Vector3 oldVal = (Vector3)values[i];
						values[i] = new Vector3(oldVal.X, oldVal.Y, newVal.Z);
					}
				}
				this.SetValues(values);
			}
			this.PerformGetValue();
		}
	}
}

