using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;

using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.PropertyEditing.Editors;

using Duality;
using Duality.Drawing;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(BatchInfo), PropertyEditorAssignmentAttribute.PrioritySpecialized)]
	public class BatchInfoPropertyEditor : MemberwisePropertyEditor
	{
		private static readonly ShaderFieldInfo[] DefaultShaderFields = new ShaderFieldInfo[]
		{
			new ShaderFieldInfo(
				BuiltinShaderFields.MainTex,
				ShaderFieldType.Sampler2D,
				ShaderFieldScope.Uniform)
		};

		private struct FieldEditorItem
		{
			public PropertyEditor Editor;
			public ShaderFieldInfo Field;
		}

		private Point dragBeginPos = Point.Empty;
		private Dictionary<string,FieldEditorItem> fieldEditors = new Dictionary<string,FieldEditorItem>();

		public BatchInfoPropertyEditor()
		{
			//this.HeaderStyle = GroupHeaderStyle.SmoothSunken;
			//this.HeaderHeight = 35;
		}

		public override void ClearContent()
		{
 			base.ClearContent();
			this.fieldEditors.Clear();
		}

		protected override bool IsAutoCreateMember(MemberInfo info)
		{
			if (info.IsEquivalent(ReflectionInfo.Property_BatchInfo_MainColor)) return true;
			if (info.IsEquivalent(ReflectionInfo.Property_BatchInfo_Technique)) return true;
			return false;
		}
		protected override void OnUpdateFromObjects(object[] values)
		{
			base.OnUpdateFromObjects(values);

			if (values.Any(o => o != null))
			{
				IEnumerable<BatchInfo> batchInfos = values.Cast<BatchInfo>();
				DrawTechnique refTech = batchInfos.NotNull().First().Technique.Res;

				// Retrieve a list of shader variables to edit
				ShaderFieldInfo[] shaderFields = null;
				if (refTech != null && refTech.Shader.IsAvailable)
					shaderFields = refTech.Shader.Res.Fields;
				else
					shaderFields = DefaultShaderFields;

				// Remove editors that are no longer needed or no longer match their shader field
				List<string> removeEditors = null;
				foreach (var pair in this.fieldEditors)
				{
					int matchingIndex = Array.FindIndex(shaderFields, f => f.Name == pair.Key);
					bool isMatchingEditor = 
						matchingIndex != -1 &&
						shaderFields[matchingIndex].Scope == pair.Value.Field.Scope &&
						shaderFields[matchingIndex].Type == pair.Value.Field.Type &&
						shaderFields[matchingIndex].ArrayLength == pair.Value.Field.ArrayLength;
					if (!isMatchingEditor)
					{
						if (removeEditors == null)
							removeEditors = new List<string>();
						removeEditors.Add(pair.Key);
					}
				}
				if (removeEditors != null)
				{
					foreach (string fieldName in removeEditors)
					{
						this.RemovePropertyEditor(this.fieldEditors[fieldName].Editor);
						this.fieldEditors.Remove(fieldName);
					}
				}

				// Create editors for fields that do not yet have a matching editor, or which
				// were removed because they did no longer match.
				int autoCreateEditorCount = 2;
				int displayedFieldIndex = 0;
				for (int i = 0; i < shaderFields.Length; i++)
				{
					ShaderFieldInfo field = shaderFields[i];

					// Skip fields that shouldn't be displayed
					if (field.IsPrivate) continue;
					if (field.Scope != ShaderFieldScope.Uniform) continue;
					if (field.Name == BuiltinShaderFields.MainColor) continue;

					displayedFieldIndex++;

					// Skip fields that already have a matching editor
					if (this.fieldEditors.ContainsKey(field.Name)) continue;

					// Create a new editor for this field
					PropertyEditor editor;
					if (field.Type == ShaderFieldType.Sampler2D)
					{
						editor = this.ParentGrid.CreateEditor(typeof(ContentRef<Texture>), this);
						editor.Getter = this.CreateTextureValueGetter(field.Name);
						editor.Setter = !this.ReadOnly ? this.CreateTextureValueSetter(field.Name) : null;
						editor.PropertyName = field.Name;
						this.ParentGrid.ConfigureEditor(editor);
					}
					else
					{
						editor = this.CreateUniformEditor(field);
					}

					// Add and register this editor
					this.fieldEditors[field.Name] = new FieldEditorItem
					{
						Editor = editor,
						Field = field
					};
					if (autoCreateEditorCount + displayedFieldIndex <= this.ChildEditors.Count)
						this.AddPropertyEditor(editor, autoCreateEditorCount + displayedFieldIndex);
					else
						this.AddPropertyEditor(editor);
				}
			}
		}
		protected override void OnPropertySet(PropertyInfo property, IEnumerable<object> targets)
		{
			base.OnPropertySet(property, targets);
			// BatchInfos aren't usually referenced, they're nested. Make sure the change notification is passed on.
			this.SetValues(targets);
			// Run a GetValue-pass to make sure automatic changes are applied if necessary.
			this.PerformGetValue();
		}
		protected PropertyEditor CreateUniformEditor(ShaderFieldInfo field)
		{
			List<EditorHintAttribute> configData = new List<EditorHintAttribute>();
			PropertyEditor editor = null;

			if (field.ArrayLength == 1)
			{
				if (field.Type == ShaderFieldType.Float || field.Type == ShaderFieldType.Int)
				{
					Type editType = typeof(float);
					if (field.Type == ShaderFieldType.Int) editType = typeof(int);

					editor = this.ParentGrid.CreateEditor(editType, this);
					if (field.Type == ShaderFieldType.Int)
					{
						editor.Getter = this.CreateUniformValueGetter<int>(field.Name);
						editor.Setter = !this.ReadOnly ? this.CreateUniformValueSetter<int>(field.Name) : null;
					}
					else
					{
						editor.Getter = this.CreateUniformValueGetter<float>(field.Name);
						editor.Setter = !this.ReadOnly ? this.CreateUniformValueSetter<float>(field.Name) : null;
						configData.Add(new EditorHintIncrementAttribute(0.1f));
					}
				}
				else if (field.Type == ShaderFieldType.Vec2)
				{
					editor = this.ParentGrid.CreateEditor(typeof(Vector2), this);
					editor.Getter = this.CreateUniformValueGetter<Vector2>(field.Name);
					editor.Setter = !this.ReadOnly ? this.CreateUniformValueSetter<Vector2>(field.Name) : null;
					configData.Add(new EditorHintIncrementAttribute(0.1f));
				}
				else if (field.Type == ShaderFieldType.Vec3)
				{
					editor = this.ParentGrid.CreateEditor(typeof(Vector3), this);
					editor.Getter = this.CreateUniformValueGetter<Vector3>(field.Name);
					editor.Setter = !this.ReadOnly ? this.CreateUniformValueSetter<Vector3>(field.Name) : null;
					configData.Add(new EditorHintIncrementAttribute(0.1f));
				}
				else if (field.Type == ShaderFieldType.Vec4)
				{
					editor = this.ParentGrid.CreateEditor(typeof(Vector4), this);
					editor.Getter = this.CreateUniformValueGetter<Vector4>(field.Name);
					editor.Setter = !this.ReadOnly ? this.CreateUniformValueSetter<Vector4>(field.Name) : null;
					configData.Add(new EditorHintIncrementAttribute(0.1f));
				}
				else
				{
					editor = this.ParentGrid.CreateEditor(typeof(float[]), this);
					editor.Getter = this.CreateUniformArrayValueGetter<float>(field.Name);
					editor.Setter = !this.ReadOnly ? this.CreateUniformArrayValueSetter<float>(field.Name) : null;
					if (editor is GroupedPropertyEditor)
					{
						(editor as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
					}
				}
			}
			else
			{
				if (field.Type == ShaderFieldType.Float || field.Type == ShaderFieldType.Int)
				{
					Type editType = typeof(float);
					if (field.Type == ShaderFieldType.Int) editType = typeof(int);

					editor = this.ParentGrid.CreateEditor(editType.MakeArrayType(), this);
					editor.Getter = this.CreateUniformArrayValueGetter<float>(field.Name);
					editor.Setter = !this.ReadOnly ? this.CreateUniformArrayValueSetter<float>(field.Name) : null;
					editor.ForceWriteBack = true;
					if (editor is GroupedPropertyEditor)
					{
						if (field.Type == ShaderFieldType.Float) (editor as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
					}
				}
				else if (field.Type == ShaderFieldType.Vec2)
				{
					editor = this.ParentGrid.CreateEditor(typeof(Vector2[]), this);
					editor.Getter = this.CreateUniformArrayValueGetter<Vector2>(field.Name);
					editor.Setter = !this.ReadOnly ? this.CreateUniformArrayValueSetter<Vector2>(field.Name) : null;
					editor.ForceWriteBack = true;
					if (editor is GroupedPropertyEditor)
					{
						(editor as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
					}
				}
				else if (field.Type == ShaderFieldType.Vec3)
				{
					editor = this.ParentGrid.CreateEditor(typeof(Vector3[]), this);
					editor.Getter = this.CreateUniformArrayValueGetter<Vector3>(field.Name);
					editor.Setter = !this.ReadOnly ? this.CreateUniformArrayValueSetter<Vector3>(field.Name) : null;
					editor.ForceWriteBack = true;
					if (editor is GroupedPropertyEditor)
					{
						(editor as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
					}
				}
				else if (field.Type == ShaderFieldType.Vec4)
				{
					editor = this.ParentGrid.CreateEditor(typeof(Vector4[]), this);
					editor.Getter = this.CreateUniformArrayValueGetter<Vector4>(field.Name);
					editor.Setter = !this.ReadOnly ? this.CreateUniformArrayValueSetter<Vector4>(field.Name) : null;
					editor.ForceWriteBack = true;
					if (editor is GroupedPropertyEditor)
					{
						(editor as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
					}
				}
				else
				{
					editor = this.ParentGrid.CreateEditor(typeof(float[]), this);
					editor.Getter = this.CreateUniformArrayValueGetter<float>(field.Name);
					editor.Setter = !this.ReadOnly ? this.CreateUniformArrayValueSetter<float>(field.Name) : null;
					if (editor is GroupedPropertyEditor)
					{
						(editor as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
					}
				}
			}

			editor.PropertyName = field.Name;
			this.ParentGrid.ConfigureEditor(editor, configData);
			return editor;
		}
		
		protected Func<IEnumerable<object>> CreateTextureValueGetter(string name)
		{
			return () => this.GetValue().Cast<BatchInfo>().Select(o => o != null ? (object)o.GetTexture(name) : null);
		}
		protected Func<IEnumerable<object>> CreateUniformArrayValueGetter<T>(string name) where T : struct
		{
			return () => this.GetValue().Cast<BatchInfo>().Select(o => o != null ? (object)o.GetArray<T>(name) : null);
		}
		protected Func<IEnumerable<object>> CreateUniformValueGetter<T>(string name) where T : struct
		{
			return () => this.GetValue().Cast<BatchInfo>().Select(o => o != null ? (object)o.GetValue<T>(name) : null);
		}

		protected Action<IEnumerable<object>> CreateTextureValueSetter(string name)
		{
			return delegate(IEnumerable<object> values)
			{
				IEnumerator<ContentRef<Texture>> valuesEnum = values.Cast<ContentRef<Texture>>().GetEnumerator();
				BatchInfo[] batchInfoArray = this.GetValue().Cast<BatchInfo>().ToArray();

				ContentRef<Texture> curValue = null;
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				foreach (BatchInfo info in batchInfoArray)
				{
					if (info != null) info.SetTexture(name, curValue);
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(null, batchInfoArray);
			};
		}
		protected Action<IEnumerable<object>> CreateUniformArrayValueSetter<T>(string name) where T : struct
		{
			return delegate(IEnumerable<object> values)
			{
				IEnumerator<T[]> valuesEnum = values.Cast<T[]>().GetEnumerator();
				BatchInfo[] batchInfoArray = this.GetValue().Cast<BatchInfo>().ToArray();

				T[] curValue = null;
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				foreach (BatchInfo info in batchInfoArray)
				{
					if (info != null) info.SetArray(name, curValue);
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(null, batchInfoArray);
			};
		}
		protected Action<IEnumerable<object>> CreateUniformValueSetter<T>(string name) where T : struct
		{
			return delegate(IEnumerable<object> values)
			{
				IEnumerator<T> valuesEnum = values.Cast<T>().GetEnumerator();
				BatchInfo[] batchInfoArray = this.GetValue().Cast<BatchInfo>().ToArray();

				T curValue = default(T);
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				foreach (BatchInfo info in batchInfoArray)
				{
					if (info != null) info.SetValue(name, curValue);
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(null, batchInfoArray);
			};
		}

		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			DataObject dragDropData = e.Data as DataObject;
			if (this.HoverEditor == null && dragDropData != null && !this.ReadOnly)
			{
				if (new ConvertOperation(dragDropData, ConvertOperation.Operation.All).CanPerform<BatchInfo>())
				{
					// Accept drop
					e.Effect = e.AllowedEffect;
				}
			}
		}
		protected override void OnDragDrop(DragEventArgs e)
		{
			DataObject dragDropData = e.Data as DataObject;
			if (this.HoverEditor == null && dragDropData != null && !this.ReadOnly)
			{
				var batchInfoQuery = new ConvertOperation(dragDropData, ConvertOperation.Operation.All).Perform<BatchInfo>();
				if (batchInfoQuery != null)
				{
					BatchInfo[] newBatchInfoArray = batchInfoQuery.ToArray();
					// Accept drop
					e.Effect = e.AllowedEffect;

					IEnumerable<object> values = this.GetValue();
					BatchInfo[] newValues = new BatchInfo[Math.Max(1, values.Count())];
					for (int i = 0; i < newValues.Length; i++)
					{
						newValues[i] = new BatchInfo(newBatchInfoArray[0]);
					}
					this.SetValues(newValues);
					this.PerformGetValue();
					return;
				}
			}

			// Move on to children if failed otherwise
			base.OnDragDrop(e);
		}

		private void UniformList_EditorAdded(object sender, PropertyEditorEventArgs e)
		{
			this.ParentGrid.ConfigureEditor(e.Editor, new[] { new EditorHintIncrementAttribute(0.1f) });
			e.Editor.ForceWriteBack = true;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.dragBeginPos = Point.Empty;
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.ClientRectangle.Contains(e.Location) && e.Y - this.ClientRectangle.Y < this.HeaderHeight)
			{
				this.dragBeginPos = e.Location;
			}
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.dragBeginPos != Point.Empty)
			{
				if (Math.Abs(this.dragBeginPos.X - e.X) > 5 || Math.Abs(this.dragBeginPos.Y - e.Y) > 5)
				{
					DataObject dragDropData = new DataObject();
					dragDropData.SetBatchInfos(this.GetValue().OfType<BatchInfo>().ToArray());
					this.ParentGrid.DoDragDrop(dragDropData, DragDropEffects.All);
				}
			}
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.dragBeginPos = Point.Empty;
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.C && e.Control)
			{
				DataObject data = new DataObject();
				data.SetBatchInfos(new[] { this.DisplayedValue as BatchInfo });
				Clipboard.SetDataObject(data);
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.V && e.Control)
			{
				DataObject data = Clipboard.GetDataObject() as DataObject;
				ConvertOperation convert = new ConvertOperation(data, ConvertOperation.Operation.All);
				IEnumerable<BatchInfo> refQuery = null;
				if (convert.CanPerform<BatchInfo>() && (refQuery = convert.Perform<BatchInfo>()) != null)
				{
					this.SetValue(refQuery.FirstOrDefault());
					this.PerformGetValue();
					this.OnEditingFinished(FinishReason.LeapValue);
				}
				else
					System.Media.SystemSounds.Beep.Play();

				e.Handled = true;
			}
			base.OnKeyDown(e);
		}
	}
}
