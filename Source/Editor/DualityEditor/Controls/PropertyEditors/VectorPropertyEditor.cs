﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.PropertyEditing.Templates;

using Duality;
using Duality.Editor;

namespace Duality.Editor.Controls.PropertyEditors
{
	public abstract class VectorPropertyEditor : PropertyEditor
	{
		private const string VectorDataFormat = "Vectors";

		protected	NumericEditorTemplate[]	editor		= null;
		protected	bool[]					multiple	= null;
		protected	int						focusEditor	= -2;
		protected	int						lines;


		protected VectorPropertyEditor(int size, int lines)
		{
			this.editor = new NumericEditorTemplate[size];
			this.multiple = new bool[size];
			this.lines = lines;

			for (int i = 0; i < this.editor.Length; i++)
			{
				this.editor[i] = new NumericEditorTemplate(this);
				this.editor[i].Invalidate += this.child_Invalidate;
			}

			this.Height = 18 * this.lines;
		}


		public void SetFocusEditorIndex(int index, bool select)
		{
			index = MathF.Clamp(index, -1, this.editor.Length - 1);
			if (this.focusEditor == index) return;

			if (this.Focused) this.LeaveFocusIndexState(this.focusEditor, index);

			this.focusEditor = index;

			if (this.Focused) this.EnterFocusIndexState(this.focusEditor, select);
		}
		private void EnterFocusIndexState(int newFocus, bool select)
		{
			if (newFocus == -1)
			{
				foreach (NumericEditorTemplate t in this.editor)
				{
					if (!t.Focused) t.OnGotFocus(EventArgs.Empty);
					if (select) t.Select();
				}
			}
			else
			{
				this.editor[newFocus].OnGotFocus(EventArgs.Empty);
				if (select) this.editor[newFocus].Select();
			}
		}
		private void LeaveFocusIndexState(int lastFocus, int newFocus)
		{
			if (lastFocus == -1)
			{
				for (int i = 0; i < this.editor.Length; i++)
				{
					if (!this.editor[i].Focused) continue;
					if (i == newFocus) continue;
					this.editor[i].OnLostFocus(EventArgs.Empty);
				}
			}
			else
			{
				this.editor[lastFocus].OnLostFocus(EventArgs.Empty);
			}
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			for (int i = 0; i < this.editor.Length; i++)
				this.editor[i].OnPaint(e, this.Enabled, this.multiple[i]);
		}
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.focusEditor = -1;
			this.EnterFocusIndexState(this.focusEditor, true);
		}
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.LeaveFocusIndexState(this.focusEditor, -2);
			this.focusEditor = -2;
			this.OnEditingFinished(FinishReason.LostFocus);
		}
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (this.focusEditor != -1)
				this.editor[this.focusEditor].OnKeyPress(e);
			else
			{
				for (int i = 0; i < this.editor.Length; i++)
				{
					this.editor[i].OnKeyPress(e);
					if (e.Handled)
					{
						this.SetFocusEditorIndex(i, false);
						break;
					}
				}
			}
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Oemcomma || e.KeyCode == Keys.Return || e.KeyCode == Keys.Space)
			{
				this.SetFocusEditorIndex((this.focusEditor + 1) % this.editor.Length, true);
				e.Handled = true;
			}
			else if ((e.KeyCode == Keys.C || e.KeyCode == Keys.X) && e.Control)
			{
				if (this.focusEditor == -1)
				{
					string valString = this.editor.Select(ve => ve.Value).ToString(", ");
					DataObject data = new DataObject();
					data.SetText(valString);
					data.SetWrappedData(new [] { this.DisplayedValue }, VectorDataFormat, DataObjectStorage.Value);

					Clipboard.SetDataObject(data);
					this.SetFocusEditorIndex(-1, true);
					if (e.KeyCode == Keys.X)
					{
						for (int i = 0; i < this.editor.Length; i++)
							this.editor[i].Value = 0;
					}
					e.Handled = true;
				}
				else
				{
					this.editor[this.focusEditor].OnKeyDown(e);
				}
			}
			else if (e.KeyCode == Keys.V && e.Control)
			{
				if (this.focusEditor == -1)
				{
					DataObject data = Clipboard.GetDataObject() as DataObject;
					bool success = false;
					IEnumerable<object> storedVectors;
					if (data.TryGetWrappedData(VectorDataFormat, DataObjectStorage.Value, out storedVectors))
					{
						object vectorObj = storedVectors.First();
						this.SetValue(vectorObj);
						this.PerformGetValue();
						this.OnEditingFinished(FinishReason.LeapValue);
						this.SetFocusEditorIndex(-1, true);
						success = true;
					}
					else if (data.ContainsText())
					{
						string valString = data.GetText();
						string[] token = valString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
						for (int i = 0; i < Math.Min(token.Length, this.editor.Length); i++)
						{
							token[i] = token[i].Trim();
							decimal val;
							if (decimal.TryParse(token[i], out val))
							{
								this.editor[i].Value = val;
								success = true;
							}
						}
						if (success)
						{
							this.PerformSetValue();
							this.PerformGetValue();
							this.OnEditingFinished(FinishReason.LeapValue);
							this.SetFocusEditorIndex(-1, true);
						}
					}

					if (!success) System.Media.SystemSounds.Beep.Play();
					e.Handled = true;
				}
				else
				{
					this.editor[this.focusEditor].OnKeyDown(e);
				}
			}
			else
			{
				if (this.focusEditor != -1)
					this.editor[this.focusEditor].OnKeyDown(e);
				else
				{
					for (int i = 0; i < this.editor.Length; i++)
					{
						this.editor[i].OnKeyDown(e);
						if (e.Handled)
						{
							this.SetFocusEditorIndex(i, false);
							break;
						}
					}
				}
			}
			base.OnKeyDown(e);
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (this.focusEditor != -1)
				this.editor[this.focusEditor].OnKeyUp(e);
			else
			{
				for (int i = 0; i < this.editor.Length; i++)
				{
					this.editor[i].OnKeyUp(e);
					if (e.Handled)
					{
						this.SetFocusEditorIndex(i, false);
						break;
					}
				}
			}
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			foreach (NumericEditorTemplate t in this.editor)
				t.OnMouseMove(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			foreach (NumericEditorTemplate t in this.editor)
				t.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			for (int i = 0; i < this.editor.Length; i++)
			{
				if (this.editor[i].Rect.Contains(e.Location)) this.SetFocusEditorIndex(i, true);
				this.editor[i].OnMouseDown(e);
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			foreach (NumericEditorTemplate t in this.editor)
				t.OnMouseUp(e);
		}
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			for (int i = 0; i < this.editor.Length; i++)
				this.editor[i].OnMouseDoubleClick(e);
		}

		protected override void UpdateGeometry()
		{
			base.UpdateGeometry();

			int horNum = MathF.RoundToInt((float)this.editor.Length / (float)this.lines);
			int verNum = this.lines;

			const int subEditSpace = 1;
			int subEditWidth = (this.ClientRectangle.Width - 2 - (subEditSpace * (horNum - 1))) / horNum;
			int subEditHeight = (this.ClientRectangle.Height - 1 - (subEditSpace * (verNum - 1))) / verNum;
			
			int curX = this.ClientRectangle.X + 1;
			int curY = this.ClientRectangle.Y + 1;
			for (int i = 0; i < this.editor.Length; i++)
			{
				bool lastOneInLine = ((i + 1) % horNum) == 0;
				this.editor[i].Rect = new Rectangle(curX, curY, lastOneInLine ? (this.ClientRectangle.Right - 1 - curX) : subEditWidth, subEditHeight);
				curX += subEditWidth + subEditSpace;
				if (lastOneInLine)
				{
					curX = this.ClientRectangle.X + 1;
					curY += subEditHeight + subEditSpace;
				}
			}
		}
		protected override void OnReadOnlyChanged()
		{
			base.OnReadOnlyChanged();
			foreach (NumericEditorTemplate t in this.editor)
				t.ReadOnly = this.ReadOnly;
		}

		protected override void ConfigureEditor(object configureData)
		{
			base.ConfigureEditor(configureData);
			var hintOverride = configureData as IEnumerable<EditorHintAttribute>;

			foreach (NumericEditorTemplate t in this.editor)
			{
				t.ResetProperties();
				this.ApplyDefaultSubEditorConfig(t);
			}

			if (this.EditedMember != null)
			{
				var range = EditorHintAttribute.Get<EditorHintRangeAttribute>(this.EditedMember, hintOverride);
				var increment = EditorHintAttribute.Get<EditorHintIncrementAttribute>(this.EditedMember, hintOverride);
				var places = EditorHintAttribute.Get<EditorHintDecimalPlacesAttribute>(this.EditedMember, hintOverride);
				
				foreach (NumericEditorTemplate t in this.editor)
				{
					if (places != null) t.DecimalPlaces = places.Places;
					if (increment != null) t.Increment = increment.Increment;
					if (range != null)
					{
						t.ValueBarMaximum = range.ReasonableMaximum;
						t.ValueBarMinimum = range.ReasonableMinimum;
						t.Minimum = range.LimitMinimum;
						t.Maximum = range.LimitMaximum;
					}
				}
			}
		}
		protected virtual void ApplyDefaultSubEditorConfig(NumericEditorTemplate subEditor)
		{
			subEditor.DecimalPlaces = 2;
			subEditor.Increment = 0.1m;
		}

		/// <summary>
		/// Generic handler for "edited" events of the individual vector editors, allowing to apply changes
		/// to only a single sub-editor, without affecting any others. Also handles the usual boilerplate code
		/// for retrieving, mergind and re-assigning values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="mergeOldNew">
		/// A merge function that applies the modified value but leaves the others unchanged. 
		/// The first parameter is the old value, the second parameters is the new one.
		/// </param>
		protected void HandleValueEdited<T>(Func<T,T,T> mergeOldNew)
		{
			if (this.IsUpdating) return;
			if (this.Disposed) return;
			if (!this.ReadOnly)
			{
				object[] values = this.GetValue().ToArray();
				T newVal = (T)this.DisplayedValue;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == null)
						values[i] = this.DisplayedValue;
					else
					{
						T oldVal = (T)values[i];
						T mergedVal = mergeOldNew(oldVal, newVal);
						values[i] = mergedVal;
					}
				}
				this.SetValues(values);
			}
			this.PerformGetValue();
		}

		private void child_Invalidate(object sender, EventArgs e)
		{
			this.Invalidate();
		}
	}
}

