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
		private	Point	dragBeginPos	= Point.Empty;
		private	Dictionary<string,PropertyEditor>	shaderVarEditors = new Dictionary<string,PropertyEditor>();

		public BatchInfoPropertyEditor()
		{
			//this.HeaderStyle = GroupHeaderStyle.SmoothSunken;
			//this.HeaderHeight = 35;
		}

		public override void ClearContent()
		{
 			base.ClearContent();
			this.shaderVarEditors.Clear();
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

				// Retrieve data about shader variables
				ShaderFieldInfo[] shaderFields = null;
				if (refTech != null && refTech.Shader.IsAvailable)
				{
					if (!refTech.Shader.Res.Compiled)
						refTech.Shader.Res.Compile();
					shaderFields = refTech.Shader.Res.Fields;
				}
				else
				{
					shaderFields = new ShaderFieldInfo[] { new ShaderFieldInfo(
						ShaderFieldInfo.DefaultNameMainTex,
						ShaderFieldType.Sampler2D,
						ShaderFieldScope.Uniform) };
				}

				// Create editors according to shader variables. Some of them may already have
				// backing values, others may not, in which case they're written on change.
				Dictionary<string,PropertyEditor> oldEditors = new Dictionary<string,PropertyEditor>(this.shaderVarEditors);
				HashSet<string> usedFields = new HashSet<string>();
				foreach (ShaderFieldInfo field in shaderFields)
				{
					if (field.IsPrivate) continue;
					if (field.Scope != ShaderFieldScope.Uniform) continue;
					
					usedFields.Add(field.Name);
					if (field.Type == ShaderFieldType.Sampler2D)
					{
						PropertyEditor oldEditor;
						this.shaderVarEditors.TryGetValue(field.Name, out oldEditor);
						if (oldEditor == null || oldEditor.EditedType != typeof(ContentRef<Texture>))
						{
							PropertyEditor editor = this.ParentGrid.CreateEditor(typeof(ContentRef<Texture>), this);
							editor.Getter = this.CreateTextureValueGetter(field.Name);
							editor.Setter = !this.ReadOnly ? this.CreateTextureValueSetter(field.Name) : null;
							editor.PropertyName = field.Name;
							this.shaderVarEditors[field.Name] = editor;
							this.ParentGrid.ConfigureEditor(editor);
							this.AddPropertyEditor(editor);
						}
					}
					else
					{
						this.CreateOrUpdateUniformEditor(field);
					}
				}

				// Remove old editors that aren't needed anymore
				foreach (var pair in oldEditors)
				{
					PropertyEditor oldEditor = pair.Value;
					PropertyEditor newEditor = this.shaderVarEditors[pair.Key];
					bool editorFieldInUse = usedFields.Contains(pair.Key);

					// Replaced an old editor with a new one for an existing field
					if (oldEditor != newEditor)
					{
						this.RemovePropertyEditor(oldEditor);
					}
					// An existing editor for a field is no longer is use
					else if (!editorFieldInUse)
					{
						this.shaderVarEditors.Remove(pair.Key);
						this.RemovePropertyEditor(oldEditor);
					}
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
		protected void CreateOrUpdateUniformEditor(ShaderFieldInfo varInfo)
		{
			PropertyEditor oldEditor;
			this.shaderVarEditors.TryGetValue(varInfo.Name, out oldEditor);
			List<EditorHintAttribute> configData = new List<EditorHintAttribute>();

			if (varInfo.ArrayLength == 1)
			{
				if (varInfo.Type == ShaderFieldType.Float || varInfo.Type == ShaderFieldType.Int)
				{
					Type editType = typeof(float);
					if (varInfo.Type == ShaderFieldType.Int) editType = typeof(int);

					if (oldEditor == null || oldEditor.EditedType != editType)
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(editType, this);
						if (varInfo.Type == ShaderFieldType.Int)
						{
							e.Getter = this.CreateUniformValueGetter<int>(varInfo.Name);
							e.Setter = !this.ReadOnly ? this.CreateUniformValueSetter<int>(varInfo.Name) : null;
						}
						else
						{
							e.Getter = this.CreateUniformValueGetter<float>(varInfo.Name);
							e.Setter = !this.ReadOnly ? this.CreateUniformValueSetter<float>(varInfo.Name) : null;
							configData.Add(new EditorHintIncrementAttribute(0.1f));
						}
						e.PropertyName = varInfo.Name;
						this.ParentGrid.ConfigureEditor(e, configData);
						this.shaderVarEditors[varInfo.Name] = e;
						this.AddPropertyEditor(e);
					}
				}
				else if (varInfo.Type == ShaderFieldType.Vec2)
				{
					if (oldEditor == null || oldEditor.EditedType != typeof(Vector2))
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(Vector2), this);
						e.Getter = this.CreateUniformValueGetter<Vector2>(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformValueSetter<Vector2>(varInfo.Name) : null;
						e.PropertyName = varInfo.Name;
						configData.Add(new EditorHintIncrementAttribute(0.1f));
						this.ParentGrid.ConfigureEditor(e, configData);
						this.shaderVarEditors[varInfo.Name] = e;
						this.AddPropertyEditor(e);
					}
				}
				else if (varInfo.Type == ShaderFieldType.Vec3)
				{
					if (oldEditor == null || oldEditor.EditedType != typeof(Vector3))
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(Vector3), this);
						e.Getter = this.CreateUniformValueGetter<Vector3>(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformValueSetter<Vector3>(varInfo.Name) : null;
						e.PropertyName = varInfo.Name;
						configData.Add(new EditorHintIncrementAttribute(0.1f));
						this.ParentGrid.ConfigureEditor(e, configData);
						this.shaderVarEditors[varInfo.Name] = e;
						this.AddPropertyEditor(e);
					}
				}
				else if (varInfo.Type == ShaderFieldType.Vec4)
				{
					if (oldEditor == null || oldEditor.EditedType != typeof(Vector4))
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(Vector4), this);
						e.Getter = this.CreateUniformValueGetter<Vector4>(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformValueSetter<Vector4>(varInfo.Name) : null;
						e.PropertyName = varInfo.Name;
						configData.Add(new EditorHintIncrementAttribute(0.1f));
						this.ParentGrid.ConfigureEditor(e, configData);
						this.shaderVarEditors[varInfo.Name] = e;
						this.AddPropertyEditor(e);
					}
				}
				else
				{
					if (oldEditor == null || oldEditor.EditedType != typeof(float[]))
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(float[]), this);
						e.Getter = this.CreateUniformArrayValueGetter<float>(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformArrayValueSetter<float>(varInfo.Name) : null;
						e.PropertyName = varInfo.Name;
						if (e is GroupedPropertyEditor)
						{
							(e as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
						}
						this.ParentGrid.ConfigureEditor(e, configData);
						this.shaderVarEditors[varInfo.Name] = e;
						this.AddPropertyEditor(e);
					}
				}
			}
			else
			{
				Array oldValue = oldEditor != null ? (oldEditor as IListPropertyEditor).DisplayedValue as Array : null;
				Type oldElementType = oldValue != null ? oldValue.GetType().GetElementType() : null;
				int oldLen = oldValue != null ? oldValue.Length : -1;

				if (varInfo.Type == ShaderFieldType.Float || varInfo.Type == ShaderFieldType.Int)
				{
					Type editType = typeof(float);
					if (varInfo.Type == ShaderFieldType.Int) editType = typeof(int);

					if (oldLen != varInfo.ArrayLength || oldElementType != editType)
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(editType.MakeArrayType(), this);
						e.Getter = this.CreateUniformArrayValueGetter<float>(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformArrayValueSetter<float>(varInfo.Name) : null;
						e.PropertyName = varInfo.Name;
						e.ForceWriteBack = true;
						if (e is GroupedPropertyEditor)
						{
							if (varInfo.Type == ShaderFieldType.Float) (e as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
						}
						this.ParentGrid.ConfigureEditor(e, configData);
						this.shaderVarEditors[varInfo.Name] = e;
						this.AddPropertyEditor(e);
					}
				}
				else if (varInfo.Type == ShaderFieldType.Vec2)
				{
					if (oldLen != varInfo.ArrayLength || oldElementType != typeof(Vector2))
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(Vector2[]), this);
						e.Getter = this.CreateUniformArrayValueGetter<Vector2>(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformArrayValueSetter<Vector2>(varInfo.Name) : null;
						e.PropertyName = varInfo.Name;
						e.ForceWriteBack = true;
						if (e is GroupedPropertyEditor)
						{
							(e as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
						}
						this.ParentGrid.ConfigureEditor(e, configData);
						this.shaderVarEditors[varInfo.Name] = e;
						this.AddPropertyEditor(e);
					}
				}
				else if (varInfo.Type == ShaderFieldType.Vec3)
				{
					if (oldLen != varInfo.ArrayLength || oldElementType != typeof(Vector3))
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(Vector3[]), this);
						e.Getter = this.CreateUniformArrayValueGetter<Vector3>(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformArrayValueSetter<Vector3>(varInfo.Name) : null;
						e.PropertyName = varInfo.Name;
						e.ForceWriteBack = true;
						if (e is GroupedPropertyEditor)
						{
							(e as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
						}
						this.ParentGrid.ConfigureEditor(e, configData);
						this.shaderVarEditors[varInfo.Name] = e;
						this.AddPropertyEditor(e);
					}
				}
				else if (varInfo.Type == ShaderFieldType.Vec4)
				{
					if (oldLen != varInfo.ArrayLength || oldElementType != typeof(Vector4))
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(Vector4[]), this);
						e.Getter = this.CreateUniformArrayValueGetter<Vector4>(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformArrayValueSetter<Vector4>(varInfo.Name) : null;
						e.PropertyName = varInfo.Name;
						e.ForceWriteBack = true;
						if (e is GroupedPropertyEditor)
						{
							(e as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
						}
						this.ParentGrid.ConfigureEditor(e, configData);
						this.shaderVarEditors[varInfo.Name] = e;
						this.AddPropertyEditor(e);
					}
				}
				else
				{
					if (oldEditor == null || oldEditor.EditedType != typeof(float[]))
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(float[]), this);
						e.Getter = this.CreateUniformArrayValueGetter<float>(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformArrayValueSetter<float>(varInfo.Name) : null;
						e.PropertyName = varInfo.Name;
						if (e is GroupedPropertyEditor)
						{
							(e as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
						}
						this.ParentGrid.ConfigureEditor(e, configData);
						this.shaderVarEditors[varInfo.Name] = e;
						this.AddPropertyEditor(e);
					}
				}
			}
		}
		
		protected Func<IEnumerable<object>> CreateTextureValueGetter(string name)
		{
			return () => this.GetValue().Cast<BatchInfo>().Select(o => o != null ? (object)o.Parameters.Get<ContentRef<Texture>>(name) : null);
		}
		protected Func<IEnumerable<object>> CreateUniformArrayValueGetter<T>(string name) where T : struct
		{
			return () => this.GetValue().Cast<BatchInfo>().Select(o => o != null ? (object)o.Parameters.GetArray<T>(name) : null);
		}
		protected Func<IEnumerable<object>> CreateUniformValueGetter<T>(string name) where T : struct
		{
			return () => this.GetValue().Cast<BatchInfo>().Select(o => o != null ? (object)o.Parameters.Get<T>(name) : null);
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
					if (info != null) info.Parameters.Set(name, curValue);
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(ReflectionInfo.Property_BatchInfo_Parameters, batchInfoArray);
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
					if (info != null) info.Parameters.SetArray(name, curValue);
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(ReflectionInfo.Property_BatchInfo_Parameters, batchInfoArray);
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
					if (info != null) info.Parameters.Set(name, curValue);
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(ReflectionInfo.Property_BatchInfo_Parameters, batchInfoArray);
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
