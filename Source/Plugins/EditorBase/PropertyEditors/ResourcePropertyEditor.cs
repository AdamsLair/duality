using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using AdamsLair.WinForms.PropertyEditing;

using Duality;
using Duality.Drawing;
using Duality.Editor;
using Duality.Editor.UndoRedoActions;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(Resource))]
	public class ResourcePropertyEditor : MemberwisePropertyEditor
	{
		private	bool	isInvokingDirectChild	= false;
		private	bool	preventFocus			= false;

		public override bool CanGetFocus
		{
			get { return base.CanGetFocus && !this.preventFocus; }
		}
		public bool PreventFocus
		{
			get { return this.preventFocus; }
			set { this.preventFocus = value; }
		}

		public ResourcePropertyEditor()
		{
			this.PropertyName = "Resource";
			this.HeaderHeight = 24;
			this.HeaderStyle = GroupHeaderStyle.Emboss;
		}

		protected override void OnUpdateFromObjects(object[] values)
		{
			base.OnUpdateFromObjects(values);

			Resource[] resValues = values.OfType<Resource>().ToArray();
			if (resValues.Length == 1) this.HeaderValueText = resValues[0].FullName;
		}
		protected override void OnEditedTypeChanged()
		{
			base.OnEditedTypeChanged();

			System.Drawing.Bitmap iconBitmap = this.EditedType.GetEditorImage() as System.Drawing.Bitmap;
			ColorHsva avgClr = iconBitmap != null ? 
				iconBitmap.GetAverageColor().ToHsva() : 
				Duality.Drawing.ColorHsva.TransparentWhite;
			if (avgClr.S <= 0.05f)
			{
				avgClr = new ColorHsva(
					0.001f * (float)(this.EditedType.Name.GetHashCode() % 1000), 
					1.0f, 
					0.5f);
			}

			this.PropertyName = this.EditedType.GetTypeCSCodeName(true);
			this.HeaderIcon = iconBitmap;
			this.HeaderColor = ExtMethodsColor.ColorFromHSV(avgClr.H, 0.2f, 0.8f);

			this.Hints &= ~HintFlags.HasButton;
			this.Hints &= ~HintFlags.ButtonEnabled;
		}
		protected override void OnValueChanged(object sender, PropertyEditorValueEventArgs args)
		{
			base.OnValueChanged(sender, args);
			if (this.isInvokingDirectChild) return;

			// Find the direct descendant editor on the path to the changed one
			PropertyEditor directChild = args.Editor;
			while (directChild != null && !this.HasPropertyEditor(directChild))
				directChild = directChild.ParentEditor;
			if (directChild == args.Editor) return;

			// If an editor has changed that was NOT a direct descendant, fire the appropriate notifier events.
			// Otherwise, there will be changed events for the child objects, but no-one knows they belong to
			// the Resource that is selected here.
			// Direct descendants are not a problem, because we'll run into the default member setter method 
			// that already takes care of this.
			if (directChild != null && directChild != args.Editor && directChild.EditedMember != null)
			{
				// We don't actually fire notifier events, but perform an UndoRedoAction, which will
				// do this among other things. Not sure if this is still required, but let's leave it like 
				// this for now.
				this.isInvokingDirectChild = true;
				if (directChild.EditedMember is PropertyInfo)
					UndoRedoManager.Do(new EditPropertyAction(this.ParentGrid, directChild.EditedMember as PropertyInfo, this.GetValue(), null));
				else if (directChild.EditedMember is FieldInfo)
					UndoRedoManager.Do(new EditFieldAction(this.ParentGrid, directChild.EditedMember as FieldInfo, this.GetValue(), null));
				this.isInvokingDirectChild = false;
			}
		}
	}
}
