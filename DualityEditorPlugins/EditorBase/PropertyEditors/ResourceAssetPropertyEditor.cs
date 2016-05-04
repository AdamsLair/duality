using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Drawing;

using AdamsLair.WinForms.PropertyEditing;

using Duality;
using Duality.Drawing;
using Duality.Editor;
using Duality.Editor.AssetManagement;
using Duality.Editor.UndoRedoActions;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	/// <summary>
	/// Allows to edit the import / export parameters of a <see cref="Resource"/> and provides
	/// buttons for commonly used Asset operations, such as showing source files, or performing
	/// an export or re-import.
	/// </summary>
	public class ResourceAssetPropertyEditor : GroupedPropertyEditor
	{
		private ResourceImportExportPropertyEditor importExportEditor = null;
		private bool registeredGlobalEvents = false;


		public override object DisplayedValue
		{
			get { return this.GetValue(); }
		}


		public ResourceAssetPropertyEditor()
		{
			// Retrieve the icon for Asset importers
			Image importExportImage = typeof(IAssetImporter).GetEditorImage();

			this.PropertyName = "Import / Export Settings";
			this.HeaderHeight = 24;
			this.HeaderStyle = GroupHeaderStyle.Emboss;
			this.HeaderValueText = null;
			this.HeaderColor = ExtMethodsColor.ColorFromHSV(0.75f, 0.2f, 0.8f);
			this.HeaderIcon = importExportImage;
			this.Hints = HintFlags.None;
		}

		public override void InitContent()
		{
			base.InitContent();
			if (this.importExportEditor == null)
			{
				this.importExportEditor = new ResourceImportExportPropertyEditor();
				this.importExportEditor.EditedType = this.EditedType; // Needs to match so the forwarded getter's safety check passes
				this.importExportEditor.Getter = this.GetValue;
				this.importExportEditor.Setter = this.SetValues;
				this.AddPropertyEditor(this.importExportEditor);
			}
			this.RegisterGlobalEvents();
		}
		public override void ClearContent()
		{
			base.ClearContent();
			if (this.importExportEditor != null)
			{
				this.RemovePropertyEditor(this.importExportEditor);
				this.importExportEditor.Dispose();
				this.importExportEditor = null;
			}
			this.UnregisterGlobalEvents();
		}

		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			this.UnregisterGlobalEvents();
		}
		protected override void OnGetValue()
		{
			base.OnGetValue();
			foreach (PropertyEditor e in this.ChildEditors)
				e.PerformGetValue();
		}
		protected override void OnSetValue()
		{
			base.OnSetValue();
			foreach (PropertyEditor e in this.ChildEditors)
				e.PerformSetValue();
		}

		private void RegisterGlobalEvents()
		{
			if (this.registeredGlobalEvents) return;
			this.registeredGlobalEvents = true;
			AssetManager.ExportFinished += this.AssetManager_ExportFinished;
		}
		private void UnregisterGlobalEvents()
		{
			if (!this.registeredGlobalEvents) return;
			this.registeredGlobalEvents = false;
			AssetManager.ExportFinished -= this.AssetManager_ExportFinished;
		}

		private void AssetManager_ExportFinished(object sender, AssetExportFinishedEventArgs e)
		{
			// Update after export in order to apply changes to the selected AssetInfo.
			//
			// Note that we don't need to do this explicitly for the import, as an
			// import fill fire an ObjectPropertyChanged event, which in turn leads to
			// the Object Inspector updating itself. The only reason we need this here
			// is because an export will likely not change anything about the Resource
			// and thus won't trigger editor updates. However, we do need to update here,
			// because we depend on the fact whether or not source files are available.
			//
			this.PerformGetValue();
		}
	}
}
