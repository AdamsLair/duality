using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

using AdamsLair.PropertyGrid;
using AdamsLair.PropertyGrid.Renderer;
using ButtonState = AdamsLair.PropertyGrid.Renderer.ButtonState;
using BorderStyle = AdamsLair.PropertyGrid.Renderer.BorderStyle;

using Duality;
using Duality.Editor;
using Duality.Editor.CorePluginInterface;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
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
		public override bool ReferenceBroken
		{
			get { return this.component != null && (this.component.Disposed || (this.component.GameObj != null && this.component.GameObj.Disposed)); }
		}


		public override void ShowReferencedContent()
		{
			if (this.component == null) return;
			SceneView view = EditorBasePlugin.Instance.RequestSceneView();
			view.FlashNode(view.FindNode(this.component));
			System.Media.SystemSounds.Beep.Play();
		}
		public override void ResetReference()
		{
			if (this.ReadOnly) return;
			this.component = null;
			this.PerformSetValue();
			this.PerformGetValue();
			this.OnEditingFinished(FinishReason.LeapValue);
		}
		public override void PerformGetValue()
		{
			base.PerformGetValue();
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

				this.GeneratePreview();
			}
			this.EndUpdate();
			if (lastCmp != this.component || lastMultiple != this.multiple) this.Invalidate();
		}

		protected void GeneratePreview()
		{
			int prevHash = this.GetPreviewHash();
			if (this.prevImageHash == prevHash) return;
			this.prevImageHash = prevHash;
			
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
	}
}

