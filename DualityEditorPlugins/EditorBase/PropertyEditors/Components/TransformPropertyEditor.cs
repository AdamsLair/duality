using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

using AdamsLair.PropertyGrid;
using OpenTK;

using Duality;
using Duality.EditorHints;
using Duality.Components;
using DualityEditor;

namespace EditorBase.PropertyEditors
{
	public class TransformPropertyEditor : ComponentPropertyEditor, IHelpProvider
	{
		private bool			showRelative	= false;
		private PropertyEditor	editorPos		= null;
		private PropertyEditor	editorVel		= null;
		private PropertyEditor	editorScale		= null;
		private PropertyEditor	editorAngle		= null;
		private PropertyEditor	editorAngleVel	= null;
		private PropertyEditor	editorShowRelative	= null;

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
				this.ParentGrid.ConfigureEditor(this.editorAngle, new EditorHintAttribute[] 
				{ new EditorHintDecimalPlacesAttribute(1), new EditorHintIncrementAttribute(1) });
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

			this.AddEditorForProperty(ReflectionInfo.Property_Transform_DeriveAngle);
			this.AddEditorForProperty(ReflectionInfo.Property_Transform_IgnoreParent);

			this.editorShowRelative = this.ParentGrid.CreateEditor(typeof(bool), this);
			if (editorShowRelative != null)
			{
				this.editorShowRelative.BeginUpdate();
				this.editorShowRelative.Getter = this.ShowRelativeGetter;
				this.editorShowRelative.Setter = this.ShowRelativeSetter;
				this.editorShowRelative.PropertyName = "[ Relative values ]";
				this.ParentGrid.ConfigureEditor(this.editorShowRelative);
				this.AddPropertyEditor(this.editorShowRelative);
				this.editorShowRelative.EndUpdate();
			}
		}

		protected override bool IsChildValueModified(PropertyEditor childEditor)
		{
			MemberInfo info = childEditor.EditedMember;

			if (childEditor == this.editorPos)
				info = ReflectionInfo.Property_Transform_RelativePos;
			else if (childEditor == this.editorVel)
				info = ReflectionInfo.Property_Transform_RelativeVel;
			else if (childEditor == this.editorScale)
				info = ReflectionInfo.Property_Transform_RelativeScale;
			else if (childEditor == this.editorAngle)
				info = ReflectionInfo.Property_Transform_RelativeAngle;
			else if (childEditor == this.editorAngleVel)
				info = ReflectionInfo.Property_Transform_RelativeAngleVel;

			if (info != null)
			{
				Component[] values = this.GetValue().Cast<Component>().NotNull().ToArray();
				return values.Any(delegate (Component c)
				{
					Duality.Resources.PrefabLink l = c.GameObj.AffectedByPrefabLink;
					return l != null && l.HasChange(c, info as PropertyInfo);
				});
			}
			else
				return base.IsChildValueModified(childEditor);
		}

		protected IEnumerable<object> ShowRelativeGetter()
		{
			return new object[] { this.showRelative };
		}
		protected void ShowRelativeSetter(IEnumerable<object> values)
		{
			this.showRelative = values.Cast<bool>().FirstOrDefault();
			this.PerformGetValue();
		}
		protected IEnumerable<object> PosGetter()
		{
			if (this.showRelative)
				return this.GetValue().OfType<Transform>().Select(o => (object)o.RelativePos);
			else
				return this.GetValue().OfType<Transform>().Select(o => (object)o.Pos);
		}
		protected void PosSetter(IEnumerable<object> values)
		{
			if (this.showRelative)
			{
				this.MemberPropertySetter(ReflectionInfo.Property_Transform_RelativePos, this.GetValue(), values);
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
					this.MemberPropertySetter(ReflectionInfo.Property_Transform_RelativePos, targetListLocal, valuesListLocal);
				}
			}

			this.OnPropertySet(ReflectionInfo.Property_Transform_RelativePos, values);
			this.PerformGetValue();
		}
		protected IEnumerable<object> VelGetter()
		{
			if (this.showRelative)
				return this.GetValue().OfType<Transform>().Select(o => (object)o.RelativeVel);
			else
				return this.GetValue().OfType<Transform>().Select(o => (object)o.Vel);
		}
		protected IEnumerable<object> ScaleGetter()
		{
			if (this.showRelative)
				return this.GetValue().OfType<Transform>().Select(o => (object)o.RelativeScale);
			else
				return this.GetValue().OfType<Transform>().Select(o => (object)o.Scale);
		}
		protected void ScaleSetter(IEnumerable<object> values)
		{
			if (this.showRelative)
			{
				this.MemberPropertySetter(ReflectionInfo.Property_Transform_RelativeScale, this.GetValue(), values);
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
					this.MemberPropertySetter(ReflectionInfo.Property_Transform_RelativeScale, targetListLocal, valuesListLocal);
				}
			}

			this.OnPropertySet(ReflectionInfo.Property_Transform_RelativeScale, values);
			this.PerformGetValue();
		}
		protected IEnumerable<object> AngleGetter()
		{
			if (this.showRelative)
				return this.GetValue().OfType<Transform>().Select(o => (object)MathF.RadToDeg(o.RelativeAngle));
			else
				return this.GetValue().OfType<Transform>().Select(o => (object)MathF.RadToDeg(o.Angle));
		}
		protected void AngleSetter(IEnumerable<object> values)
		{
			values = values.Select(v => (object)MathF.DegToRad((float)v));
			if (this.showRelative)
			{
				this.MemberPropertySetter(ReflectionInfo.Property_Transform_RelativeAngle, this.GetValue(), values);
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
					this.MemberPropertySetter(ReflectionInfo.Property_Transform_RelativeAngle, targetListLocal, valuesListLocal);
				}
			}

			this.OnPropertySet(ReflectionInfo.Property_Transform_RelativeAngle, values);
			this.PerformGetValue();
		}
		protected IEnumerable<object> AngleVelGetter()
		{
			if (this.showRelative)
				return this.GetValue().OfType<Transform>().Select(o => (object)MathF.RadToDeg(o.RelativeAngleVel));
			else
				return this.GetValue().OfType<Transform>().Select(o => (object)MathF.RadToDeg(o.AngleVel));
		}

		HelpInfo IHelpProvider.ProvideHoverHelp(System.Drawing.Point localPos, ref bool captured)
		{
			PropertyEditor pickedEditor = this.PickEditorAt(localPos.X, localPos.Y, true);
			if (this.showRelative)
			{
				if (pickedEditor == this.editorPos)
					return HelpInfo.FromMember(ReflectionInfo.Property_Transform_RelativePos);
				else if (pickedEditor == this.editorVel)
					return HelpInfo.FromMember(ReflectionInfo.Property_Transform_RelativeVel);
				else if (pickedEditor == this.editorScale)
					return HelpInfo.FromMember(ReflectionInfo.Property_Transform_RelativeScale);
				else if (pickedEditor == this.editorAngle)
					return HelpInfo.FromMember(ReflectionInfo.Property_Transform_RelativeAngle);
				else if (pickedEditor == this.editorAngleVel)
					return HelpInfo.FromMember(ReflectionInfo.Property_Transform_RelativeAngleVel);
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
				return HelpInfo.FromText("Show relative values?", "If true, the relative Transform values are displayed for editing. This is an editor property that does not affect object behaviour in any way.");
			else if (pickedEditor.EditedMember != null)
				return HelpInfo.FromMember(pickedEditor.EditedMember);
			else if (pickedEditor.EditedType != null)
				return HelpInfo.FromMember(pickedEditor.EditedType);

			return null;
		}
	}
}
