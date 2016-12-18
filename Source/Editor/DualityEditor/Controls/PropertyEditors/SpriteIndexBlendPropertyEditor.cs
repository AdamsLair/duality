using System;
using System.Linq;

using AdamsLair.WinForms.PropertyEditing.Templates;

using Duality;
using Duality.Drawing;

namespace Duality.Editor.Controls.PropertyEditors
{
	[PropertyEditorAssignment(typeof(SpriteIndexBlend))]
	public class SpriteIndexBlendPropertyEditor : VectorPropertyEditor
	{
		public override object DisplayedValue
		{
			get 
			{ 
				return new SpriteIndexBlend((int)this.editor[0].Value, (int)this.editor[2].Value, (float)this.editor[1].Value);
			}
		}


		public SpriteIndexBlendPropertyEditor() : base(3, 1)
		{
			this.editor[0].Edited += this.editorCurrent_Edited;
			this.editor[1].Edited += this.editorBlend_Edited;
			this.editor[2].Edited += this.editorNext_Edited;
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
				int avgCurrent = (int)Math.Round(valNotNull.Average(o => ((SpriteIndexBlend)o).Current));
				int avgNext = (int)Math.Round(valNotNull.Average(o => ((SpriteIndexBlend)o).Next));
				float avgBlend = valNotNull.Average(o => ((SpriteIndexBlend)o).Blend);

				this.editor[0].Value = MathF.SafeToDecimal(avgCurrent);
				this.editor[1].Value = MathF.SafeToDecimal(avgBlend);
				this.editor[2].Value = MathF.SafeToDecimal(avgNext);

				this.multiple[0] = (values.Any(o => o == null) || values.Any(o => ((SpriteIndexBlend)o).Current != avgCurrent));
				this.multiple[1] = (values.Any(o => o == null) || values.Any(o => ((SpriteIndexBlend)o).Blend != avgBlend));
				this.multiple[2] = (values.Any(o => o == null) || values.Any(o => ((SpriteIndexBlend)o).Next != avgNext));
			}
			this.EndUpdate();
		}
		protected override void ApplyDefaultSubEditorConfig(NumericEditorTemplate subEditor)
		{
			base.ApplyDefaultSubEditorConfig(subEditor);
			if (subEditor == this.editor[1])
			{
				subEditor.DecimalPlaces = 2;
				subEditor.Increment = 0.1m;
			}
			else
			{
				subEditor.DecimalPlaces = 0;
				subEditor.Increment = 1;
				subEditor.Minimum = -1;
			}
		}

		private void editorCurrent_Edited(object sender, EventArgs e)
		{
			this.HandleValueEdited<SpriteIndexBlend>((oldVal, newVal) => new SpriteIndexBlend(newVal.Current, oldVal.Next, oldVal.Blend));
		}
		private void editorBlend_Edited(object sender, EventArgs e)
		{
			this.HandleValueEdited<SpriteIndexBlend>((oldVal, newVal) => new SpriteIndexBlend(oldVal.Current, oldVal.Next, newVal.Blend));
		}
		private void editorNext_Edited(object sender, EventArgs e)
		{
			this.HandleValueEdited<SpriteIndexBlend>((oldVal, newVal) => new SpriteIndexBlend(oldVal.Current, newVal.Next, oldVal.Blend));
		}
	}
}

