using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using AdamsLair.WinForms.PropertyEditing;

using Duality;
using Duality.Editor.AssetManagement;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	/// <summary>
	/// This is a top-level wrapper <see cref="PropertyEditor"/> that is used in <see cref="Resource"/> editing
	/// for displaying both <see cref="Resource"/> and <see cref="AssetInfo"/> editors on the same page.
	/// </summary>
	[PropertyEditorAssignment(typeof(ResourceOverviewPropertyEditor), "MatchToProperty")]
	public class ResourceOverviewPropertyEditor : GroupedPropertyEditor
	{
		private Type						resourceType   = null;
		private PropertyEditor			  resourceEditor = null;
		private ResourceAssetPropertyEditor assetEditor = null;
		
		public override object DisplayedValue
		{
			get { return this.GetValue(); }
		}
		public override bool CanGetFocus
		{
			get { return false; }
		}

		public ResourceOverviewPropertyEditor()
		{
			this.HeaderHeight = 0;
			this.Hints = HintFlags.None;
			this.Indent = 0;
		}

		public override void InitContent()
		{
			base.InitContent();
			this.SetupChildEditors();
		}
		public override void ClearContent()
		{
			base.ClearContent();
			this.DisposeResourceEditor();
			this.DisposeAssetEditor();
		}

		protected override void OnGetValue()
		{
			base.OnGetValue();

			// Determine if we have any Resource in our selection that can be imported / exported.
			// Try to keep this as fast as possible, also avoid file system access if possible.
			Resource[] values = this.GetValue().OfType<Resource>().ToArray();
			bool anyImportExport = false;
			foreach (Resource res in values)
			{
				string[] exportFiles = AssetManager.SimulateExportAssets(res);
				if (exportFiles.Length > 0)
				{
					anyImportExport = true;
					break;
				}

				string[] sourceFiles = AssetManager.GetAssetSourceFiles(res);
				if (sourceFiles.Length > 0 && sourceFiles.Any(f => File.Exists(f)))
				{
					anyImportExport = true;
					break;
				}
			}

			// Add or remove the Asset editor based on whether this Resource can be
			// imported or exported. It'll figure out the details itself.
			bool assetEditorActive = this.ChildEditors.Contains(this.assetEditor);
			if (anyImportExport && !assetEditorActive)
			{
				this.AddPropertyEditor(this.assetEditor);
				this.assetEditor.Expanded = true;
			}
			else if (!anyImportExport && assetEditorActive)
				this.RemovePropertyEditor(this.assetEditor);

			// Make all child editors update their values as well
			foreach (PropertyEditor e in this.ChildEditors)
				e.PerformGetValue();
		}
		protected override void OnSetValue()
		{
			base.OnSetValue();
			if (this.ReadOnly) return;

			// Make all child editors trigger a value-set as well
			foreach (PropertyEditor e in this.ChildEditors)
				e.PerformSetValue();
		}
		protected override void OnEditedTypeChanged()
		{
			base.OnEditedTypeChanged();

			// If our content is initialized, make sure it still matches the edited type.
			if (this.ContentInitialized)
				this.SetupChildEditors();
		}

		private void SetupChildEditors()
		{
			this.BeginUpdate();

			// Remove the old Resource editor if it doesn't match
			if (this.resourceEditor != null && this.resourceType != this.EditedType)
				this.DisposeResourceEditor();

			// Add a new Resource editor if we don't have one yet
			if (this.resourceEditor == null)
			{
				this.resourceType = this.EditedType;
				this.resourceEditor = this.ParentGrid.CreateEditor(this.EditedType, this);
				this.resourceEditor.Getter = this.GetValue;
				this.resourceEditor.Setter = v => {};
				this.ParentGrid.ConfigureEditor(this.resourceEditor);
				this.AddPropertyEditor(this.resourceEditor);

				// If the child editor is a grouped one, expand it immediately after adding
				// and hide the expand / collapse checkbox. We don't want to expose our wrapper
				// structure after all.
				GroupedPropertyEditor group = this.resourceEditor as GroupedPropertyEditor;
				if (group != null)
				{
					group.Expanded = true;
					group.Hints &= ~HintFlags.HasExpandCheck;
				}
			}

			// Set up a shared Asset editor that will be added or removed on demand
			if (this.assetEditor == null)
			{
				this.assetEditor = new ResourceAssetPropertyEditor();
				this.assetEditor.EditedType = this.EditedType; // Needs to match so the forwarded getter's safety check passes
				this.assetEditor.Getter = this.GetValue;
				this.assetEditor.Setter = v => {};
				this.ParentGrid.ConfigureEditor(this.assetEditor);
			}

			this.EndUpdate();
		}
		private void DisposeResourceEditor()
		{
			if (this.resourceEditor == null) return;
			this.RemovePropertyEditor(this.resourceEditor);
			this.resourceEditor.Dispose();
			this.resourceEditor = null;
			this.resourceType = null;
		}
		private void DisposeAssetEditor()
		{
			if (this.assetEditor == null) return;
			this.RemovePropertyEditor(this.assetEditor);
			this.assetEditor.Dispose();
			this.assetEditor = null;
		}

		private static int MatchToProperty(Type propertyType, ProviderContext context)
		{
			if (typeof(Resource).IsAssignableFrom(propertyType) && context.ParentEditor == null)
				return PropertyEditorAssignmentAttribute.PrioritySpecialized + 1;
			else
				return PropertyEditorAssignmentAttribute.PriorityNone;
		}
	}
}