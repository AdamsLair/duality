using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

using AdamsLair.WinForms.PropertyEditing;

using Duality;
using Duality.Editor;
using Duality.Components;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(TransformPropertyEditor), "MatchToProperty")]
	public class TransformPropertyEditor : ComponentPropertyEditor, IHelpProvider
	{
		private bool           showLocal          = false;
		private PropertyEditor editorPos          = null;
		private PropertyEditor editorVel          = null;
		private PropertyEditor editorScale        = null;
		private PropertyEditor editorAngle        = null;
		private PropertyEditor editorAngleVel     = null;
		private PropertyEditor editorShowRelative = null;
		
		public override MemberInfo MapEditorToMember(PropertyEditor editor)
		{
			if (editor == this.editorPos)
				return ReflectionInfo.Property_Transform_LocalPos;
			else if (editor == this.editorScale)
				return ReflectionInfo.Property_Transform_LocalScale;
			else if (editor == this.editorAngle)
				return ReflectionInfo.Property_Transform_LocalAngle;
			else
				return base.MapEditorToMember(editor);
		}

		protected override bool IsAutoCreateMember(MemberInfo info)
		{
			return false;
		}
		protected override void BeforeAutoCreateEditors()
		{
			base.BeforeAutoCreateEditors();
			
			this.editorPos = this.ParentGrid.CreateEditor((typeof(Vector3)), this);
			if (this.editorPos != null)
			{
				this.editorPos.BeginUpdate();
				this.editorPos.Getter = this.PosGetter;
				this.editorPos.Setter = this.PosSetter;
				this.editorPos.PropertyName = "Pos";
				this.ParentGrid.ConfigureEditor(this.editorPos, new EditorHintAttribute[] 
				{ new EditorHintDecimalPlacesAttribute(0), new EditorHintIncrementAttribute(1) });
				this.AddPropertyEditor(this.editorPos);
				this.editorPos.EndUpdate();
			}
			this.editorVel = this.ParentGrid.CreateEditor(typeof(Vector3), this);
			if (this.editorVel != null)
			{
				this.editorVel.BeginUpdate();
				this.editorVel.Getter = this.VelGetter;
				this.editorVel.PropertyName = "Vel";
				this.ParentGrid.ConfigureEditor(this.editorVel);
				this.AddPropertyEditor(this.editorVel);
				this.editorVel.EndUpdate();
			}
			this.editorScale = this.ParentGrid.CreateEditor(typeof(float), this);
			if (this.editorScale != null)
			{
				this.editorScale.BeginUpdate();
				this.editorScale.Getter = this.ScaleGetter;
				this.editorScale.Setter = this.ScaleSetter;
				this.editorScale.PropertyName = "Scale";
				this.ParentGrid.ConfigureEditor(this.editorScale);
				this.AddPropertyEditor(this.editorScale);
				this.editorScale.EndUpdate();
			}
			this.editorAngle = this.ParentGrid.CreateEditor(typeof(float), this);
			if (this.editorAngle != null)
			{
				this.editorAngle.BeginUpdate();
				this.editorAngle.Getter = this.AngleGetter;
				this.editorAngle.Setter = this.AngleSetter;
				this.editorAngle.PropertyName = "Angle";
				this.ParentGrid.ConfigureEditor(this.editorAngle, new EditorHintAttribute[] { 
					new EditorHintDecimalPlacesAttribute(1), 
					new EditorHintIncrementAttribute(1),
					new EditorHintRangeAttribute(float.MinValue, float.MaxValue, 0.0f, 359.999f) });
				this.AddPropertyEditor(this.editorAngle);
				this.editorAngle.EndUpdate();
			}
			this.editorAngleVel = this.ParentGrid.CreateEditor(typeof(float), this);
			if (this.editorAngleVel != null)
			{
				this.editorAngleVel.BeginUpdate();
				this.editorAngleVel.Getter = this.AngleVelGetter;
				this.editorAngleVel.PropertyName = "AngleVel";
				this.ParentGrid.ConfigureEditor(this.editorAngleVel, new[] { new EditorHintIncrementAttribute(0.1f) });
				this.AddPropertyEditor(this.editorAngleVel);
				this.editorAngleVel.EndUpdate();
			}

			this.AddEditorForMember(ReflectionInfo.Property_Transform_IgnoreParent);

			this.editorShowRelative = this.ParentGrid.CreateEditor(typeof(bool), this);
			if (this.editorShowRelative != null)
			{
				this.editorShowRelative.BeginUpdate();
				this.editorShowRelative.Getter = this.ShowRelativeGetter;
				this.editorShowRelative.Setter = this.ShowRelativeSetter;
				this.editorShowRelative.PropertyName = "[ Local Space ]";
				this.ParentGrid.ConfigureEditor(this.editorShowRelative);
				this.AddPropertyEditor(this.editorShowRelative);
				this.editorShowRelative.EndUpdate();
			}
		}

		protected IEnumerable<object> ShowRelativeGetter()
		{
			return new object[] { this.showLocal };
		}
		protected void ShowRelativeSetter(IEnumerable<object> values)
		{
			this.showLocal = values.Cast<bool>().FirstOrDefault();
			this.PerformGetValue();
		}
		protected IEnumerable<object> PosGetter()
		{
			if (this.showLocal)
				return this.GetValue().OfType<Transform>().Select(o => (object)o.LocalPos);
			else
				return this.GetValue().OfType<Transform>().Select(o => (object)o.Pos);
		}
		protected void PosSetter(IEnumerable<object> values)
		{
			if (this.showLocal)
			{
				this.MemberPropertySetter(ReflectionInfo.Property_Transform_LocalPos, this.GetValue(), values);
			}
			else
			{
				List<Vector3> valuesList = values.Cast<Vector3>().ToList();
				List<object> valuesListLocal = new List<object>(valuesList.Count);
				List<Transform> targetList = this.GetValue().OfType<Transform>().ToList();
				List<Transform> targetListLocal = new List<Transform>(targetList.Count);
				List<int> removeIndices = new List<int>(targetList.Count);
				while (targetList.Count > 0)
				{
					targetListLocal.Clear();
					valuesListLocal.Clear();
					removeIndices.Clear();
					for (int i = targetList.Count - 1; i >= 0; i--)
					{
						Transform t = targetList[i];
						Transform parent = 
							t != null && 
							t.GameObj != null && 
							t.GameObj.Parent != null ? t.GameObj.Parent.Transform : null;

						if (parent == null || !targetList.Contains(parent))
						{
							Vector3 curValue = valuesList[Math.Min(i, valuesList.Count - 1)];
							
							targetListLocal.Add(t);
							valuesListLocal.Add(parent != null ? parent.GetLocalPoint(curValue) : curValue);
							removeIndices.Add(i);
						}
					}
					for (int i = 0; i < removeIndices.Count; i++)
					{
						int removeIndex = removeIndices[i];
						targetList.RemoveAt(removeIndex);
						if (removeIndex < valuesList.Count)
							valuesList.RemoveAt(removeIndex);
					}
					this.MemberPropertySetter(ReflectionInfo.Property_Transform_LocalPos, targetListLocal, valuesListLocal);
				}
			}

			this.OnPropertySet(ReflectionInfo.Property_Transform_LocalPos, values);
			this.PerformGetValue();
		}
		protected IEnumerable<object> VelGetter()
		{
			if (this.showLocal)
			{
				return this.GetValue().OfType<Transform>().Select(o =>
				{
					Vector3 localVel;
					if (o.IgnoreParent || o.GameObj.Parent == null || o.GameObj.Parent.Transform == null)
						localVel = o.Vel;
					else
						localVel = o.GetLocalVector(o.Vel - o.GameObj.Parent.Transform.Vel);
					return (object)localVel;
				});
			}
			else
			{
				return this.GetValue().OfType<Transform>().Select(o => (object)o.Vel);
			}
		}
		protected IEnumerable<object> ScaleGetter()
		{
			if (this.showLocal)
				return this.GetValue().OfType<Transform>().Select(o => (object)o.LocalScale);
			else
				return this.GetValue().OfType<Transform>().Select(o => (object)o.Scale);
		}
		protected void ScaleSetter(IEnumerable<object> values)
		{
			if (this.showLocal)
			{
				this.MemberPropertySetter(ReflectionInfo.Property_Transform_LocalScale, this.GetValue(), values);
			}
			else
			{
				List<float> valuesList = values.Cast<float>().ToList();
				List<object> valuesListLocal = new List<object>(valuesList.Count);
				List<Transform> targetList = this.GetValue().OfType<Transform>().ToList();
				List<Transform> targetListLocal = new List<Transform>(targetList.Count);
				List<int> removeIndices = new List<int>(targetList.Count);
				while (targetList.Count > 0)
				{
					targetListLocal.Clear();
					valuesListLocal.Clear();
					removeIndices.Clear();
					for (int i = targetList.Count - 1; i >= 0; i--)
					{
						Transform t = targetList[i];
						Transform parent = 
							t != null && 
							t.GameObj != null && 
							t.GameObj.Parent != null ? t.GameObj.Parent.Transform : null;

						if (parent == null || !targetList.Contains(parent))
						{
							float curValue = valuesList[Math.Min(i, valuesList.Count - 1)];
							
							targetListLocal.Add(t);
							valuesListLocal.Add(parent != null ? curValue / parent.Scale : curValue);
							removeIndices.Add(i);
						}
					}
					for (int i = 0; i < removeIndices.Count; i++)
					{
						int removeIndex = removeIndices[i];
						targetList.RemoveAt(removeIndex);
						if (removeIndex < valuesList.Count)
							valuesList.RemoveAt(removeIndex);
					}
					this.MemberPropertySetter(ReflectionInfo.Property_Transform_LocalScale, targetListLocal, valuesListLocal);
				}
			}

			this.OnPropertySet(ReflectionInfo.Property_Transform_LocalScale, values);
			this.PerformGetValue();
		}
		protected IEnumerable<object> AngleGetter()
		{
			if (this.showLocal)
				return this.GetValue().OfType<Transform>().Select(o => (object)MathF.RadToDeg(o.LocalAngle));
			else
				return this.GetValue().OfType<Transform>().Select(o => (object)MathF.RadToDeg(o.Angle));
		}
		protected void AngleSetter(IEnumerable<object> values)
		{
			values = values.Select(v => (object)MathF.DegToRad((float)v));
			if (this.showLocal)
			{
				this.MemberPropertySetter(ReflectionInfo.Property_Transform_LocalAngle, this.GetValue(), values);
			}
			else
			{
				List<float> valuesList = values.Cast<float>().ToList();
				List<object> valuesListLocal = new List<object>(valuesList.Count);
				List<Transform> targetList = this.GetValue().OfType<Transform>().ToList();
				List<Transform> targetListLocal = new List<Transform>(targetList.Count);
				List<int> removeIndices = new List<int>(targetList.Count);
				while (targetList.Count > 0)
				{
					targetListLocal.Clear();
					valuesListLocal.Clear();
					removeIndices.Clear();
					for (int i = targetList.Count - 1; i >= 0; i--)
					{
						Transform t = targetList[i];
						Transform parent = 
							t != null && 
							t.GameObj != null && 
							t.GameObj.Parent != null ? t.GameObj.Parent.Transform : null;

						if (parent == null || !targetList.Contains(parent))
						{
							float curValue = valuesList[Math.Min(i, valuesList.Count - 1)];
							
							targetListLocal.Add(t);
							valuesListLocal.Add(parent != null ? curValue - parent.Angle : curValue);
							removeIndices.Add(i);
						}
					}
					for (int i = 0; i < removeIndices.Count; i++)
					{
						int removeIndex = removeIndices[i];
						targetList.RemoveAt(removeIndex);
						if (removeIndex < valuesList.Count)
							valuesList.RemoveAt(removeIndex);
					}
					this.MemberPropertySetter(ReflectionInfo.Property_Transform_LocalAngle, targetListLocal, valuesListLocal);
				}
			}

			this.OnPropertySet(ReflectionInfo.Property_Transform_LocalAngle, values);
			this.PerformGetValue();
		}
		protected IEnumerable<object> AngleVelGetter()
		{
			if (this.showLocal)
			{
				return this.GetValue().OfType<Transform>().Select(o =>
				{
					float localAngleVel;
					if (o.IgnoreParent || o.GameObj.Parent == null || o.GameObj.Parent.Transform == null)
						localAngleVel = o.AngleVel;
					else
						localAngleVel = o.AngleVel - o.GameObj.Parent.Transform.AngleVel;
					return (object)MathF.RadToDeg(localAngleVel);
				});
			}
			else
			{
				return this.GetValue().OfType<Transform>().Select(o => (object)MathF.RadToDeg(o.AngleVel));
			}
		}

		HelpInfo IHelpProvider.ProvideHoverHelp(System.Drawing.Point localPos, ref bool captured)
		{
			PropertyEditor pickedEditor = this.PickEditorAt(this.Location.X + localPos.X, this.Location.Y + localPos.Y, true);
			if (this.showLocal)
			{
				if (pickedEditor == this.editorPos)
					return HelpInfo.FromMember(ReflectionInfo.Property_Transform_LocalPos);
				else if (pickedEditor == this.editorScale)
					return HelpInfo.FromMember(ReflectionInfo.Property_Transform_LocalScale);
				else if (pickedEditor == this.editorAngle)
					return HelpInfo.FromMember(ReflectionInfo.Property_Transform_LocalAngle);
			}
			else
			{
				if (pickedEditor == this.editorPos)
					return HelpInfo.FromMember(ReflectionInfo.Property_Transform_Pos);
				else if (pickedEditor == this.editorVel)
					return HelpInfo.FromMember(ReflectionInfo.Property_Transform_Vel);
				else if (pickedEditor == this.editorScale)
					return HelpInfo.FromMember(ReflectionInfo.Property_Transform_Scale);
				else if (pickedEditor == this.editorAngle)
					return HelpInfo.FromMember(ReflectionInfo.Property_Transform_Angle);
				else if (pickedEditor == this.editorAngleVel)
					return HelpInfo.FromMember(ReflectionInfo.Property_Transform_AngleVel);
			}
			
			if (pickedEditor == this.editorShowRelative)
				return HelpInfo.FromText("Show Local Values", "If true, all values will be displayed and edited in local space of the parent object. This is an editor property that does not affect object behaviour in any way.");
			else if (pickedEditor.EditedMember != null)
				return HelpInfo.FromMember(pickedEditor.EditedMember);
			else if (pickedEditor.EditedType != null)
				return HelpInfo.FromMember(pickedEditor.EditedType);

			return null;
		}

		private static int MatchToProperty(Type propertyType, ProviderContext context)
		{
			bool compRef = !(context.ParentEditor is GameObjectOverviewPropertyEditor);
			if (typeof(Transform).IsAssignableFrom(propertyType) && !compRef)
				return PropertyEditorAssignmentAttribute.PrioritySpecialized;
			else
				return PropertyEditorAssignmentAttribute.PriorityNone;
		}
	}
}
