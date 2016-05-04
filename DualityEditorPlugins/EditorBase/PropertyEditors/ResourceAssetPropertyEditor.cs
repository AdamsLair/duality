using System;
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
		private Dictionary<string,PropertyEditor> parameterEditors = new Dictionary<string,PropertyEditor>();
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
			this.UpdateParameterEditors();
			foreach (PropertyEditor e in this.ChildEditors)
				e.PerformGetValue();
		}
		protected override void OnSetValue()
		{
			base.OnSetValue();
			foreach (PropertyEditor e in this.ChildEditors)
				e.PerformSetValue();
		}
		protected override void OnValueChanged(object sender, PropertyEditorValueEventArgs args)
		{
			base.OnValueChanged(sender, args);

			// Did we edit an editor that's not a direct child of this one?
			bool indirectDescendant = !this.HasPropertyEditor(args.Editor);

			// If an editor has changed that was NOT a direct descendant, fire the appropriate notifier events.
			// Otherwise, there will be changed events for the child objects, but no-one knows they belong to
			// the Resource that is selected here.
			// Direct descendants are not a problem, because we'll run into our own setter method that takes care
			// of this.
			if (indirectDescendant)
			{
				DualityEditorApp.NotifyObjPropChanged(this.ParentGrid, 
					new ObjectSelection(this.GetValue()), 
					ReflectionInfo.Property_Resource_AssetInfo);
			}
		}

		/// <summary>
		/// Updates child editors to match the available custom AssetInfo data 
		/// segments in the selected Resources.
		/// </summary>
		private void UpdateParameterEditors()
		{
			this.BeginUpdate();

			// Determine which editors we need
			Resource[] resources = this.GetValue().OfType<Resource>().ToArray();
			Dictionary<string,Type> requiredEditors = new Dictionary<string,Type>();
			this.GetRequiredEditors(resources, requiredEditors);

			// Match our editors with the ones that are required
			this.DisposeObsoleteEditors(requiredEditors);
			this.CreateMissingEditors(requiredEditors);

			this.EndUpdate();
		}
		/// <summary>
		/// Determines a key-Type mapping of the required editors for the given
		/// Resource selection.
		/// </summary>
		/// <param name="resources"></param>
		/// <param name="requiredEditors"></param>
		private void GetRequiredEditors(IEnumerable<Resource> resources, IDictionary<string,Type> requiredEditors)
		{
			// Aggregate data keys pairs that are defined in the selected Resources
			foreach (Resource res in resources)
			{
				if (res.AssetInfo == null) continue;
				if (res.AssetInfo.CustomData == null) continue;

				foreach (var pair in res.AssetInfo.CustomData)
				{
					if (string.IsNullOrEmpty(pair.Key)) continue;
					if (pair.Key[0] == '_') continue; // Skip internal data

					// Determine the type of this value
					Type type = (pair.Value != null) ? pair.Value.GetType() : typeof(object);

					// If we already have an entry with a type, merge them
					Type preferredType;
					if (requiredEditors.TryGetValue(pair.Key, out preferredType))
					{
						while (!preferredType.IsAssignableFrom(type))
						{
							preferredType = preferredType.BaseType;
						}
					}
					// Otherwise, just add a new entry
					else
					{
						requiredEditors.Add(pair.Key, type);
					}
				}
			}
		}
		/// <summary>
		/// Given the specified key-Type mapping of required editors, this method
		/// will dispose all currently active editors that are no longer needed.
		/// </summary>
		/// <param name="requiredEditors"></param>
		private void DisposeObsoleteEditors(IReadOnlyDictionary<string,Type> requiredEditors)
		{
			// Gather all property editors for keys we no longer need or that don't match the required type.
			List<string> obsoleteParameterEditors = new List<string>();
			foreach (var pair in this.parameterEditors)
			{
				Type requiredType;
				if (requiredEditors.TryGetValue(pair.Key, out requiredType))
				{
					if (pair.Value.EditedType == requiredType)
						continue;
				}

				obsoleteParameterEditors.Add(pair.Key);
			}

			// Dispose all previously gathered editors. This is a two-step process,
			// because we can't modify a collection while iterating it.
			foreach (string key in obsoleteParameterEditors)
			{
				PropertyEditor editor = this.parameterEditors[key];
				this.parameterEditors.Remove(key);
				this.RemovePropertyEditor(editor);
				editor.Dispose();
			}
		}
		/// <summary>
		/// Given the specified key-Type mapping of required editors, this method
		/// will create new editors for all keys that do not have one.
		/// This method assumes that <see cref="DisposeObsoleteEditors"/> was called
		/// before and thus doesn't check whether any of the existing editors are still
		/// valid.
		/// </summary>
		/// <param name="requiredEditors"></param>
		private void CreateMissingEditors(IReadOnlyDictionary<string,Type> requiredEditors)
		{
			// Gather all keys we do not yet have a matching editor for
			List<string> missingParameterEditors = new List<string>();
			foreach (var pair in requiredEditors)
			{
				// We assume that this step is done after disposing of obsolete
				// editors, so we don't need to check if the type matches. Non-matching
				// editors will already have been removed at this point.
				if (this.parameterEditors.ContainsKey(pair.Key))
					continue;

				missingParameterEditors.Add(pair.Key);
			}

			// Create editors for all that's missing
			foreach (string key in missingParameterEditors)
			{
				Type type = requiredEditors[key];
				PropertyEditor editor = this.ParentGrid.CreateEditor(type, this);
				editor.Getter = this.CreatePropertyValueGetter(key);
				editor.Setter = this.CreatePropertyValueSetter(key);
				editor.PropertyName = key;
				this.ParentGrid.ConfigureEditor(editor);
				this.AddPropertyEditor(editor);
				this.parameterEditors.Add(key, editor);
			}
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

		/// <summary>
		/// Returns a new lambda method that acts getter for custom AssetInfo data values 
		/// from the currently selected objects and with the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private Func<IEnumerable<object>> CreatePropertyValueGetter(string key)
		{
			return () =>
			{
				return this.GetValue().Select(obj =>
				{
					Resource res = obj as Resource;
					if (res == null) return null;
					if (res.AssetInfo == null) return null;
					if (res.AssetInfo.CustomData == null) return null;

					Dictionary<string,object> data = res.AssetInfo.CustomData;
					object value;
					if (!data.TryGetValue(key, out value))
						return null;
					else
						return value;
				});
			};
		}
		/// <summary>
		/// Returns a new lambda method that acts setter for custom AssetInfo data values 
		/// from the currently selected objects and with the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private Action<IEnumerable<object>> CreatePropertyValueSetter(string key)
		{
			return delegate(IEnumerable<object> values)
			{
				UndoRedoManager.Do(new EditResourceAssetDataAction(
					this.ParentGrid, 
					key, 
					this.GetValue().Select(o => o as Resource), 
					values));
			};
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
