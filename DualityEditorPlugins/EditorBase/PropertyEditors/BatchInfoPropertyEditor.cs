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

		public override void  ClearContent()
		{
 			base.ClearContent();
			this.shaderVarEditors.Clear();
		}

		protected override bool IsAutoCreateMember(MemberInfo info)
		{
			if (ReflectionHelper.MemberInfoEquals(info, ReflectionInfo.Property_BatchInfo_MainColor)) return true;
			if (ReflectionHelper.MemberInfoEquals(info, ReflectionInfo.Property_BatchInfo_Technique)) return true;
			return false;
		}
		protected override void OnUpdateFromObjects(object[] values)
		{
			base.OnUpdateFromObjects(values);

			if (values.Any(o => o != null))
			{
				bool invokeSetter = false;
				IEnumerable<BatchInfo> batchInfos = null;
				DrawTechnique refTech = null;
				batchInfos = values.Cast<BatchInfo>();
				refTech = batchInfos.NotNull().First().Technique.Res;

				// Retrieve data about shader variables
				ShaderVarInfo[] varInfoArray = null;
				if (refTech != null && refTech.Shader.IsAvailable)
				{
					varInfoArray = refTech.Shader.Res.VarInfo;
				}
				else
				{
					varInfoArray = new ShaderVarInfo[] { new ShaderVarInfo() };
					varInfoArray[0].ArrayLength = 1;
					varInfoArray[0].Handle = -1;
					varInfoArray[0].Name = ShaderVarInfo.VarName_MainTex;
					varInfoArray[0].Scope = ShaderVarScope.Uniform;
					varInfoArray[0].Type = ShaderVarType.Sampler2D;
				}

				// Get rid of unused variables (This changes actual Resource data!)
				if (!this.ReadOnly)
				{
					foreach (BatchInfo info in batchInfos.NotNull())
					{
						List<string> texRemoveSched = null;
						List<string> uniRemoveSched = null;
						if (info.Textures != null)
						{
							foreach (var pair in info.Textures)
							{
								if (!varInfoArray.Any(v => v.Scope == ShaderVarScope.Uniform && v.Type == ShaderVarType.Sampler2D && v.Name == pair.Key))
								{
									if (texRemoveSched == null) texRemoveSched = new List<string>();
									texRemoveSched.Add(pair.Key);
								}
							}
						}
						if (info.Uniforms != null)
						{
							foreach (var pair in info.Uniforms)
							{
								if (!varInfoArray.Any(v => v.Scope == ShaderVarScope.Uniform && v.Type != ShaderVarType.Sampler2D && v.Name == pair.Key))
								{
									if (uniRemoveSched == null) uniRemoveSched = new List<string>();
									uniRemoveSched.Add(pair.Key);
								}
							}
						}
						if (texRemoveSched != null)
						{
							foreach (string name in texRemoveSched) info.SetTexture(name, null);
							invokeSetter = true;
						}
						if (uniRemoveSched != null)
						{
							foreach (string name in uniRemoveSched) info.SetUniform(name, null);
							invokeSetter = true;
						}
					}
				}

				// Create BatchInfo variables according to Shader uniforms, if not existing yet
				if (!this.ReadOnly)
				{
					foreach (ShaderVarInfo varInfo in varInfoArray)
					{
						if (varInfo.Scope != ShaderVarScope.Uniform) continue;

						// Set Texture variables
						if (varInfo.Type == ShaderVarType.Sampler2D)
						{
							foreach (BatchInfo info in batchInfos.NotNull())
							{
								if (info.GetTexture(varInfo.Name).IsExplicitNull)
								{
									info.SetTexture(varInfo.Name, Texture.White);
									invokeSetter = true;
								}
							}
						}
						// Set other uniform variables
						else
						{
							float[] uniformVal = new float[varInfo.ArrayLength * varInfo.Type.GetElementCount()];
							if (uniformVal != null)
							{
								foreach (BatchInfo info in batchInfos.NotNull())
								{
									float[] oldVal = info.GetUniform(varInfo.Name);
									if (oldVal == null) 
									{
										info.SetUniform(varInfo.Name, uniformVal);
										invokeSetter = true;
									}
									else if (oldVal.Length != uniformVal.Length)
									{
										for (int i = 0; i < Math.Min(oldVal.Length, uniformVal.Length); i++) uniformVal[i] = oldVal[i];
										info.SetUniform(varInfo.Name, uniformVal);
										invokeSetter = true;
									}
								}
							}
						}
					}
				}

				// Create editors according to existing variables
				var texDict = batchInfos.NotNull().First().Textures;
				var uniformDict = batchInfos.NotNull().First().Uniforms;
				Dictionary<string,PropertyEditor> oldEditors = new Dictionary<string,PropertyEditor>(this.shaderVarEditors);
				if (texDict != null)
				{
					foreach (var tex in texDict)
					{
						ShaderVarInfo varInfo = varInfoArray.FirstOrDefault(v => v.Scope == ShaderVarScope.Uniform && v.Name == tex.Key);
						if (varInfo.IsPrivate) continue;

						string texName = varInfo.Name;
						if (oldEditors.ContainsKey(texName))
							oldEditors.Remove(texName);
						else
						{
							PropertyEditor e = this.ParentGrid.CreateEditor(typeof(ContentRef<Texture>), this);
							e.Getter = this.CreateTextureValueGetter(texName);
							e.Setter = !this.ReadOnly ? this.CreateTextureValueSetter(texName) : null;
							e.PropertyName = texName;
							this.shaderVarEditors[texName] = e;
							this.ParentGrid.ConfigureEditor(e);
							this.AddPropertyEditor(e);
						}
					}
				}
				if (uniformDict != null)
				{
					foreach (var uniform in uniformDict)
					{
						ShaderVarInfo varInfo = varInfoArray.FirstOrDefault(v => v.Scope == ShaderVarScope.Uniform && v.Name == uniform.Key);
						if (varInfo.IsPrivate) continue;

						PropertyEditor e = this.CreateUniformEditor(varInfo);
						if (e != null)
						{
							if (oldEditors.ContainsValue(e))
								oldEditors.Remove(varInfo.Name);
							else
							{
								e.PropertyName = uniform.Key;
								this.shaderVarEditors[uniform.Key] = e;
								this.AddPropertyEditor(e);
							}
						}
					}
				}

				// Remove old editors that aren't needed anymore
				foreach (var pair in oldEditors)
				{
					if (this.shaderVarEditors[pair.Key] == pair.Value) this.shaderVarEditors.Remove(pair.Key);
					this.RemovePropertyEditor(pair.Value);
				}

				// If we actually changed (updated) data here, invoke the setter
				if (invokeSetter)
				{
					this.SetValues(batchInfos);
					if (!this.IsUpdating)
						this.PerformGetValue();
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
		protected PropertyEditor CreateUniformEditor(ShaderVarInfo varInfo)
		{
			PropertyEditor oldEditor;
			this.shaderVarEditors.TryGetValue(varInfo.Name, out oldEditor);
			List<EditorHintAttribute> configData = new List<EditorHintAttribute>();

			if (varInfo.ArrayLength == 1)
			{
				if (varInfo.Type == ShaderVarType.Float || varInfo.Type == ShaderVarType.Int)
				{
					Type editType = typeof(float);
					if (varInfo.Type == ShaderVarType.Int) editType = typeof(int);

					if (oldEditor != null && oldEditor.EditedType == editType)
						return oldEditor;
					else
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(editType, this);
						if (varInfo.Type == ShaderVarType.Int)
						{
							e.Getter = this.CreateUniformIntValueGetter(varInfo.Name);
							e.Setter = !this.ReadOnly ? this.CreateUniformIntValueSetter(varInfo.Name) : null;
						}
						else
						{
							e.Getter = this.CreateUniformFloatValueGetter(varInfo.Name);
							e.Setter = !this.ReadOnly ? this.CreateUniformFloatValueSetter(varInfo.Name) : null;
							configData.Add(new EditorHintIncrementAttribute(0.1f));
						}
						this.ParentGrid.ConfigureEditor(e, configData);
						return e;
					}
				}
				else if (varInfo.Type == ShaderVarType.Vec2)
				{
					if (oldEditor != null && oldEditor.EditedType == typeof(Vector2))
						return oldEditor;
					else
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(Vector2), this);
						e.Getter = this.CreateUniformVec2ValueGetter(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformVec2ValueSetter(varInfo.Name) : null;
						configData.Add(new EditorHintIncrementAttribute(0.1f));
						this.ParentGrid.ConfigureEditor(e, configData);
						return e;
					}
				}
				else if (varInfo.Type == ShaderVarType.Vec3)
				{
					if (oldEditor != null && oldEditor.EditedType == typeof(Vector3))
						return oldEditor;
					else
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(Vector3), this);
						e.Getter = this.CreateUniformVec3ValueGetter(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformVec3ValueSetter(varInfo.Name) : null;
						configData.Add(new EditorHintIncrementAttribute(0.1f));
						this.ParentGrid.ConfigureEditor(e, configData);
						return e;
					}
				}
				else
				{
					if (oldEditor != null && oldEditor.EditedType == typeof(float[]))
						return oldEditor;
					else
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(float[]), this);
						e.Getter = this.CreateUniformValueGetter(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformValueSetter(varInfo.Name) : null;
						if (e is GroupedPropertyEditor)
						{
							(e as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
						}
						this.ParentGrid.ConfigureEditor(e, configData);
						return e;
					}
				}
			}
			else
			{
				Array oldValue = oldEditor != null ? (oldEditor as IListPropertyEditor).DisplayedValue as Array : null;
				Type oldElementType = oldValue != null ? oldValue.GetType().GetElementType() : null;
				int oldLen = oldValue != null ? oldValue.Length : -1;

				if (varInfo.Type == ShaderVarType.Float || varInfo.Type == ShaderVarType.Int)
				{
					Type editType = typeof(float);
					if (varInfo.Type == ShaderVarType.Int) editType = typeof(int);

					if (oldLen == varInfo.ArrayLength && oldElementType == editType)
						return oldEditor;
					else
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(editType.MakeArrayType(), this);
						e.Getter = this.CreateUniformValueGetter(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformValueSetter(varInfo.Name) : null;
						e.ForceWriteBack = true;
						if (e is GroupedPropertyEditor)
						{
							if (varInfo.Type == ShaderVarType.Float) (e as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
						}
						this.ParentGrid.ConfigureEditor(e, configData);
						return e;
					}
				}
				else if (varInfo.Type == ShaderVarType.Vec2)
				{
					if (oldLen == varInfo.ArrayLength && oldElementType == typeof(Vector2))
						return oldEditor;
					else
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(Vector2[]), this);
						e.Getter = this.CreateUniformVec2ArrayValueGetter(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformVec2ArrayValueSetter(varInfo.Name) : null;
						e.ForceWriteBack = true;
						if (e is GroupedPropertyEditor)
						{
							(e as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
						}
						this.ParentGrid.ConfigureEditor(e, configData);
						return e;
					}
				}
				else if (varInfo.Type == ShaderVarType.Vec3)
				{
					if (oldLen == varInfo.ArrayLength && oldElementType == typeof(Vector3))
						return oldEditor;
					else
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(Vector3[]), this);
						e.Getter = this.CreateUniformVec3ArrayValueGetter(varInfo.Name);
						e.Setter = !this.ReadOnly ? this.CreateUniformVec3ArrayValueSetter(varInfo.Name) : null;
						e.ForceWriteBack = true;
						if (e is GroupedPropertyEditor)
						{
							(e as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
						}
						this.ParentGrid.ConfigureEditor(e, configData);
						return e;
					}
				}
				else
				{
					if (oldLen == varInfo.ArrayLength)
						return oldEditor;
					else
					{
						PropertyEditor e = this.ParentGrid.CreateEditor(typeof(float[][]), this);
						e.Getter = this.CreateUniformArrayValueGetter(varInfo.Name, varInfo.ArrayLength);
						e.Setter = !this.ReadOnly ? this.CreateUniformArrayValueSetter(varInfo.Name) : null;
						e.ForceWriteBack = true;
						if (e is GroupedPropertyEditor)
						{
							(e as GroupedPropertyEditor).EditorAdded += this.UniformList_EditorAdded;
						}
						this.ParentGrid.ConfigureEditor(e, configData);
						return e;
					}
				}
			}
		}
		
		protected Func<IEnumerable<object>> CreateTextureValueGetter(string name)
		{
			return () => this.GetValue().Cast<BatchInfo>().Select(o => o != null ? (object)o.GetTexture(name) : null);
		}
		protected Func<IEnumerable<object>> CreateUniformValueGetter(string name)
		{
			return () => this.GetValue().Cast<BatchInfo>().Select(o => o != null ? (object)o.GetUniform(name) : null);
		}
		protected Func<IEnumerable<object>> CreateUniformFloatValueGetter(string name)
		{
			return () => this.GetValue().Cast<BatchInfo>().Select(o => o != null ? (object)o.GetUniform(name)[0] : null);
		}
		protected Func<IEnumerable<object>> CreateUniformIntValueGetter(string name)
		{
			return () => this.GetValue().Cast<BatchInfo>().Select(o => o != null ? (object)(int)o.GetUniform(name)[0] : null);
		}
		protected Func<IEnumerable<object>> CreateUniformVec2ValueGetter(string name)
		{
			return () => this.GetValue().Cast<BatchInfo>().Select(o => o != null ? (object)
				new Vector2(o.GetUniform(name)[0], o.GetUniform(name)[1])
				: null);
		}
		protected Func<IEnumerable<object>> CreateUniformVec3ValueGetter(string name)
		{
			return () => this.GetValue().Cast<BatchInfo>().Select(o => o != null ? (object)
				new Vector3(o.GetUniform(name)[0], o.GetUniform(name)[1], o.GetUniform(name)[2])
				: null);
		}

		protected Func<IEnumerable<object>> CreateUniformArrayValueGetter(string name, int arraySize)
		{
			return delegate()
			{
				var batchInfos = this.GetValue().Cast<BatchInfo>();
				List<object> result = new List<object>();
				foreach (var info in batchInfos)
				{
					float[] arrayBase = info.GetUniform(name);
					int elementCount = arrayBase.Length / arraySize;
					float[][] arrays = new float[arraySize][];
					for (int i = 0; i < arraySize; i++)
					{
						arrays[i] = new float[elementCount];
						for (int j = 0; j < elementCount; j++)
						{
							arrays[i][j] = arrayBase[i * elementCount + j];
						}
					}
					result.Add(arrays);
				}
				return result;
			};
		}
		protected Func<IEnumerable<object>> CreateUniformVec2ArrayValueGetter(string name)
		{
			return delegate()
			{
				var batchInfos = this.GetValue().Cast<BatchInfo>();
				List<object> result = new List<object>();
				foreach (var info in batchInfos)
				{
					float[] arrayBase = info.GetUniform(name);
					int arraySize = arrayBase.Length / 2;
					Vector2[] arrays = new Vector2[arraySize];
					for (int i = 0; i < arraySize; i++)
					{
						arrays[i] = new Vector2(
							arrayBase[i * 2 + 0],
							arrayBase[i * 2 + 1]);
					}
					result.Add(arrays);
				}
				return result;
			};
		}
		protected Func<IEnumerable<object>> CreateUniformVec3ArrayValueGetter(string name)
		{
			return delegate()
			{
				var batchInfos = this.GetValue().Cast<BatchInfo>();
				List<object> result = new List<object>();
				foreach (var info in batchInfos)
				{
					float[] arrayBase = info.GetUniform(name);
					int arraySize = arrayBase.Length / 3;
					Vector3[] arrays = new Vector3[arraySize];
					for (int i = 0; i < arraySize; i++)
					{
						arrays[i] = new Vector3(
							arrayBase[i * 3 + 0],
							arrayBase[i * 3 + 1],
							arrayBase[i * 3 + 2]);
					}
					result.Add(arrays);
				}
				return result;
			};
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
				this.OnPropertySet(ReflectionInfo.Property_BatchInfo_Textures, batchInfoArray);
			};
		}
		protected Action<IEnumerable<object>> CreateUniformValueSetter(string name)
		{
			return delegate(IEnumerable<object> values)
			{
				IEnumerator<float[]> valuesEnum = values.Cast<float[]>().GetEnumerator();
				BatchInfo[] batchInfoArray = this.GetValue().Cast<BatchInfo>().ToArray();

				float[] curValue = null;
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				foreach (BatchInfo info in batchInfoArray)
				{
					if (info != null) info.SetUniform(name, curValue);
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(ReflectionInfo.Property_BatchInfo_Uniforms, batchInfoArray);
			};
		}
		protected Action<IEnumerable<object>> CreateUniformFloatValueSetter(string name)
		{
			return delegate(IEnumerable<object> values)
			{
				IEnumerator<float> valuesEnum = values.Cast<float>().GetEnumerator();
				BatchInfo[] batchInfoArray = this.GetValue().Cast<BatchInfo>().ToArray();

				float curValue = 0.0f;
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				foreach (BatchInfo info in batchInfoArray)
				{
					if (info != null) info.SetUniform(name, 0, curValue);
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(ReflectionInfo.Property_BatchInfo_Uniforms, batchInfoArray);
			};
		}
		protected Action<IEnumerable<object>> CreateUniformIntValueSetter(string name)
		{
			return delegate(IEnumerable<object> values)
			{
				IEnumerator<int> valuesEnum = values.Cast<int>().GetEnumerator();
				BatchInfo[] batchInfoArray = this.GetValue().Cast<BatchInfo>().ToArray();

				int curValue = 0;
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				foreach (BatchInfo info in batchInfoArray)
				{
					if (info != null) info.SetUniform(name, 0, curValue);
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(ReflectionInfo.Property_BatchInfo_Uniforms, batchInfoArray);
			};
		}
		protected Action<IEnumerable<object>> CreateUniformVec2ValueSetter(string name)
		{
			return delegate(IEnumerable<object> values)
			{
				IEnumerator<Vector2> valuesEnum = values.Cast<Vector2>().GetEnumerator();
				BatchInfo[] batchInfoArray = this.GetValue().Cast<BatchInfo>().ToArray();

				Vector2 curValue = Vector2.Zero;
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				foreach (BatchInfo info in batchInfoArray)
				{
					if (info != null)
					{
						info.SetUniform(name, 0, curValue.X);
						info.SetUniform(name, 1, curValue.Y);
					}
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(ReflectionInfo.Property_BatchInfo_Uniforms, batchInfoArray);
			};
		}
		protected Action<IEnumerable<object>> CreateUniformVec3ValueSetter(string name)
		{
			return delegate(IEnumerable<object> values)
			{
				IEnumerator<Vector3> valuesEnum = values.Cast<Vector3>().GetEnumerator();
				BatchInfo[] batchInfoArray = this.GetValue().Cast<BatchInfo>().ToArray();

				Vector3 curValue = Vector3.Zero;
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				foreach (BatchInfo info in batchInfoArray)
				{
					if (info != null)
					{
						info.SetUniform(name, 0, curValue.X);
						info.SetUniform(name, 1, curValue.Y);
						info.SetUniform(name, 2, curValue.Z);
					}
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(ReflectionInfo.Property_BatchInfo_Uniforms, batchInfoArray);
			};
		}

		protected Action<IEnumerable<object>> CreateUniformArrayValueSetter(string name)
		{
			return delegate(IEnumerable<object> values)
			{
				IEnumerator<float[][]> valuesEnum = values.Cast<float[][]>().GetEnumerator();
				BatchInfo[] batchInfoArray = this.GetValue().Cast<BatchInfo>().ToArray();

				float[][] curValue = null;
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				foreach (BatchInfo info in batchInfoArray)
				{
					if (info != null)
					{
						IEnumerable<float> curValueConcat = new float[0];
						foreach (float[] arrayChunk in curValue) curValueConcat = curValueConcat.Concat(arrayChunk);
						info.SetUniform(name, curValueConcat.ToArray());
					}
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(ReflectionInfo.Property_BatchInfo_Uniforms, batchInfoArray);
			};
		}
		protected Action<IEnumerable<object>> CreateUniformVec2ArrayValueSetter(string name)
		{
			return delegate(IEnumerable<object> values)
			{
				IEnumerator<Vector2[]> valuesEnum = values.Cast<Vector2[]>().GetEnumerator();
				BatchInfo[] batchInfoArray = this.GetValue().Cast<BatchInfo>().ToArray();

				Vector2[] curValue = null;
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				foreach (BatchInfo info in batchInfoArray)
				{
					if (info != null)
					{
						float[] curValueConcat = new float[curValue.Length * 2];
						for (int i = 0; i < curValue.Length; i++)
						{
							curValueConcat[i * 2 + 0] = curValue[i].X;
							curValueConcat[i * 2 + 1] = curValue[i].Y;
						}
						info.SetUniform(name, curValueConcat);
					}
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(ReflectionInfo.Property_BatchInfo_Uniforms, batchInfoArray);
			};
		}
		protected Action<IEnumerable<object>> CreateUniformVec3ArrayValueSetter(string name)
		{
			return delegate(IEnumerable<object> values)
			{
				IEnumerator<Vector3[]> valuesEnum = values.Cast<Vector3[]>().GetEnumerator();
				BatchInfo[] batchInfoArray = this.GetValue().Cast<BatchInfo>().ToArray();

				Vector3[] curValue = null;
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				foreach (BatchInfo info in batchInfoArray)
				{
					if (info != null)
					{
						float[] curValueConcat = new float[curValue.Length * 3];
						for (int i = 0; i < curValue.Length; i++)
						{
							curValueConcat[i * 3 + 0] = curValue[i].X;
							curValueConcat[i * 3 + 1] = curValue[i].Y;
							curValueConcat[i * 3 + 2] = curValue[i].Z;
						}
						info.SetUniform(name, curValueConcat);
					}
					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}
				this.OnPropertySet(ReflectionInfo.Property_BatchInfo_Uniforms, batchInfoArray);
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
					IEnumerable<BatchInfo> batchInfoValues = values.Cast<BatchInfo>().NotNull();
					if (batchInfoValues.Any())
					{
						foreach (BatchInfo info in batchInfoValues) newBatchInfoArray[0].CopyTo(info);
						// BatchInfos aren't usually referenced, they're nested. Make sure the change notification is passed on.
						this.SetValues(batchInfoValues);
					}
					else
					{
						this.SetValue(newBatchInfoArray[0]);
					}
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
