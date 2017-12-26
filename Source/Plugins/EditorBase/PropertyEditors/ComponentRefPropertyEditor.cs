using System;
using System.Linq;
using System.Windows.Forms;

using AdamsLair.WinForms.PropertyEditing;
using Duality.Editor.Extensibility.DataConversion;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(ComponentRefPropertyEditor), "MatchToProperty")]
	public class ComponentRefPropertyEditor : ObjectRefPropertyEditor
	{
		protected	Type		editedCmpType		= null;
		protected	Component	component			= null;
		
		public override object DisplayedValue
		{
			get { return this.component; }
		}
		public override string ReferenceName
		{
			get { return this.component != null ? (this.component.GameObj != null ? this.component.GameObj.FullName : this.component.ToString()) : null; }
		}
		public override Type ReferenceType
		{
			get { return typeof(Component); }
		}
		public override bool ReferenceBroken
		{
			get { return this.component != null && (this.component.Disposed || (this.component.GameObj != null && this.component.GameObj.Disposed)); }
		}


		public override void ShowReferencedContent()
		{
			if (this.component == null) return;
			DualityEditorApp.Highlight(this, new ObjectSelection(this.component));
		}
		public override void ResetReference()
		{
			if (this.ReadOnly) return;
			this.component = null;
			this.PerformSetValue();
			this.PerformGetValue();
			this.OnEditingFinished(FinishReason.LeapValue);
		}
		protected override void OnGetValue()
		{
			base.OnGetValue();
			Component[] values = this.GetValue().Cast<Component>().ToArray();

			this.BeginUpdate();
			Component lastCmp = this.component;
			bool lastMultiple = this.multiple;
			if (!values.Any())
			{
				this.component = null;
			}
			else
			{
				Component first = values.NotNull().FirstOrDefault();
				this.component = first;
				this.multiple = (values.Any(o => o == null) || values.Any(o => o != first));
			}
			this.EndUpdate();
			if (lastCmp != this.component || lastMultiple != this.multiple) this.Invalidate();
		}
		
		protected override void GeneratePreview()
		{
			base.GeneratePreview();
			
			this.StopPreviewSound();
			if (this.prevSound != null) this.prevSound.Dispose();
			this.prevSound = null;

			if (this.prevImage != null) this.prevImage.Dispose();
			this.prevImage = null;
			this.Height = 22;

			if (this.component != null)
			{
				this.prevImage = PreviewProvider.GetPreviewImage(this.component, this.ClientRectangle.Width - 4 - 22, 64 - 4, PreviewSizeMode.FixedHeight);
				if (this.prevImage != null)
				{
					this.Height = 64;
					var avgColor = this.prevImage.GetAverageColor();
					this.prevImageLum = avgColor.GetLuminance();
				}

				this.prevSound = PreviewProvider.GetPreviewSound(this.component);
			}
		}
		protected override int GetPreviewHash()
		{
			return this.component != null ? this.component.GetHashCode() : 0;
		}
		
		protected override void SerializeToData(DataObject data)
		{
			data.SetComponentRefs(new[] { this.component });
		}
		protected override void DeserializeFromData(DataObject data)
		{
			ConvertOperation convert = new ConvertOperation(data, ConvertOperation.Operation.Convert);
			if (convert.CanPerform(this.editedCmpType))
			{
				var refQuery = convert.Perform(this.editedCmpType);
				if (refQuery != null)
				{
					Component[] refArray = refQuery.Cast<Component>().ToArray();
					this.component = (refArray != null && refArray.Length > 0) ? refArray[0] : null;
					this.PerformSetValue();
					this.PerformGetValue();
					this.OnEditingFinished(FinishReason.LeapValue);
				}
			}
			else if (convert.CanPerform(typeof(GameObject)))
			{
				GameObject obj = convert.Perform<GameObject>().FirstOrDefault();
				Component cmp = obj != null ? obj.GetComponent(this.editedCmpType) : null;
				if (cmp != null)
				{
					this.component = cmp;
					this.PerformSetValue();
					this.PerformGetValue();
					this.OnEditingFinished(FinishReason.LeapValue);
				}
			}
		}
		protected override bool CanDeserializeFromData(DataObject data)
		{
			ConvertOperation convert = new ConvertOperation(data, ConvertOperation.Operation.Convert);
			if (convert.CanPerform(this.editedCmpType)) return true;

			if (convert.CanPerform(typeof(GameObject)))
			{
				GameObject obj = convert.Perform<GameObject>().FirstOrDefault();
				return obj != null && obj.GetComponent(this.editedCmpType) != null;
			}

			return false;
		}

		protected override void OnEditedTypeChanged()
		{
			base.OnEditedTypeChanged();
			if (typeof(Component).IsAssignableFrom(this.EditedType))
				this.editedCmpType = this.EditedType;
			else
				this.editedCmpType = typeof(Component);
		}

		private static int MatchToProperty(Type propertyType, ProviderContext context)
		{
			bool compRef = !(context.ParentEditor is GameObjectOverviewPropertyEditor);
			if (typeof(Component).IsAssignableFrom(propertyType) && compRef)
				return PropertyEditorAssignmentAttribute.PriorityGeneral;
			else
				return PropertyEditorAssignmentAttribute.PriorityNone;
		}

		protected override void UpdateReference(IObjectRefHolder holder)
		{
			if (holder.ComponentReference == null)
			{
				return;
			}

			DataObject tmpDataObject = new DataObject();
			tmpDataObject.SetComponentRefs(new[] { holder.ComponentReference });
			DeserializeFromData(tmpDataObject);
		}
	}
}
