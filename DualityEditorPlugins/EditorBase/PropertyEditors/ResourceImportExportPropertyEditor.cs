using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.Drawing;
using ButtonState = AdamsLair.WinForms.Drawing.ButtonState;

using Duality;
using Duality.Drawing;
using Duality.Editor;
using Duality.Editor.AssetManagement;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.Base.Properties;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	/// <summary>
	/// A <see cref="PropertyEditor"/> that provides action buttons for commonly
	/// used asset operations, such as showing source files, or performing an export
	/// or re-import operation.
	/// </summary>
	public class ResourceImportExportPropertyEditor : PropertyEditor, IHelpProvider
	{
		private bool      sourceFilesAvailable = false;
		private bool      exporterAvailable    = false;
		private Rectangle rectButtonShow       = Rectangle.Empty;
		private Rectangle rectButtonExport     = Rectangle.Empty;
		private Rectangle rectButtonReImport   = Rectangle.Empty;
		private int       pressedButton        = -1;
		private int       hoveredButton        = -1;


		public override object DisplayedValue
		{
			get { return this.GetValue(); }
		}
		/// <summary>
		/// [GET] Whether the current Resource value allows to show source files.
		/// </summary>
		private bool CanShowSourceFiles
		{
			get { return this.Enabled && this.sourceFilesAvailable; }
		}
		/// <summary>
		/// [GET] Whether the current Resource value allows to perform an export operation.
		/// </summary>
		private bool CanExportResource
		{
			get { return this.Enabled && this.exporterAvailable; }
		}
		/// <summary>
		/// [GET] Whether the current Resource value allows to perform a re-import operation.
		/// </summary>
		private bool CanReImportResource
		{
			get { return this.Enabled && !this.ReadOnly && this.sourceFilesAvailable; }
		}


		public ResourceImportExportPropertyEditor()
		{
			this.PropertyName = "Actions";
			this.Height += 4;
		}

		protected override void UpdateGeometry()
		{
			base.UpdateGeometry();

			int totalWidthWithoutSpacing = this.ClientRectangle.Width - 4;
			int showButtonWidth = totalWidthWithoutSpacing / 4;
			int exportButtonWidth = (totalWidthWithoutSpacing - showButtonWidth) / 2;
			int importButtonWidth = totalWidthWithoutSpacing - showButtonWidth - exportButtonWidth;

			this.rectButtonShow = new Rectangle(
				this.ClientRectangle.X, this.ClientRectangle.Y + 2,
				showButtonWidth, this.ClientRectangle.Height - 4);
			this.rectButtonExport = new Rectangle(
				this.ClientRectangle.X + showButtonWidth + 2, this.ClientRectangle.Y + 2,
				exportButtonWidth, this.ClientRectangle.Height - 4);
			this.rectButtonReImport = new Rectangle(
				this.ClientRectangle.X + showButtonWidth + exportButtonWidth + 4, this.ClientRectangle.Y + 2,
				importButtonWidth, this.ClientRectangle.Height - 4);
		}

		protected override void OnGetValue()
		{
			base.OnGetValue();
			Resource[] resources = this.GetValue().OfType<Resource>().ToArray();
			CheckAvailableActions(resources);
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			int lastHoveredButton = this.hoveredButton;

			if (this.rectButtonShow.Contains(e.Location) && this.CanShowSourceFiles)
				this.hoveredButton = 0;
			else if (this.rectButtonExport.Contains(e.Location) && this.CanExportResource)
				this.hoveredButton = 1;
			else if (this.rectButtonReImport.Contains(e.Location) && this.CanReImportResource)
				this.hoveredButton = 2;
			else
				this.hoveredButton = -1;

			if (this.hoveredButton != lastHoveredButton)
				this.Invalidate();
		}
		protected override void OnMouseLeave(System.EventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.hoveredButton != -1)
			{
				this.hoveredButton = -1;
				this.Invalidate();
			}
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.hoveredButton != -1 && this.pressedButton == -1)
			{
				this.pressedButton = this.hoveredButton;
				this.Invalidate();
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (this.pressedButton != -1)
			{
				if (this.hoveredButton == this.pressedButton)
				{
					switch (this.pressedButton)
					{
						case 0: this.ShowSourceFiles();  break;
						case 1: this.ExportResource();   break;
						case 2: this.ReImportResource(); break;
					}
				}
				this.pressedButton = -1;
				this.Invalidate();
			}
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			bool canShow = this.CanShowSourceFiles;
			bool canExport = this.CanExportResource;
			bool canImport = this.CanReImportResource;

			this.ControlRenderer.DrawButton(
				e.Graphics, this.rectButtonShow, 
				!canShow ?                 ButtonState.Disabled :
				(this.pressedButton == 0 ? ButtonState.Pressed  :
				(this.hoveredButton == 0 ? ButtonState.Hot      :
				                           ButtonState.Normal   )), 
				"Show");
			this.ControlRenderer.DrawButton(
				e.Graphics, this.rectButtonExport, 
				!canExport ?               ButtonState.Disabled : 
				(this.pressedButton == 1 ? ButtonState.Pressed  : 
				(this.hoveredButton == 1 ? ButtonState.Hot      :
				                           ButtonState.Normal   )), 
				"Export");
			this.ControlRenderer.DrawButton(
				e.Graphics, this.rectButtonReImport, 
				!canImport ?               ButtonState.Disabled : 
				(this.pressedButton == 2 ? ButtonState.Pressed  :
				(this.hoveredButton == 2 ? ButtonState.Hot      :
				                           ButtonState.Normal   )), 
				"Import");
		}

		/// <summary>
		/// Determines which actions are available, given the specified enumerable of
		/// <see cref="Resource"/> instances.
		/// </summary>
		/// <param name="resources"></param>
		private void CheckAvailableActions(IEnumerable<Resource> resources)
		{
			// Remember the old button states
			bool lastCanShow = this.CanShowSourceFiles;
			bool lastCanExport = this.CanExportResource;
			bool lastCanImport = this.CanReImportResource;

			// Determine what we can actually do with our current selection
			this.sourceFilesAvailable = false;
			this.exporterAvailable = false;
			foreach (Resource res in resources)
			{
				if (!this.sourceFilesAvailable)
				{
					string[] sourceFiles = AssetManager.GetAssetSourceFiles(res);
					if (sourceFiles.All(path => File.Exists(path)))
						this.sourceFilesAvailable = true;
				}
				if (!this.exporterAvailable)
				{
					string[] sourceFiles = AssetManager.SimulateExportAssets(res);
					if (sourceFiles.Length > 0)
						this.exporterAvailable = true;
				}
			}

			// If any button state changed, request a redraw
			if (lastCanShow != this.CanShowSourceFiles ||
				lastCanExport != this.CanExportResource ||
				lastCanImport != this.CanReImportResource)
				this.Invalidate();
		}

		/// <summary>
		/// Shows the primary source files of each of the current <see cref="Resource"/> selection
		/// in the operating system's default file manager.
		/// </summary>
		private void ShowSourceFiles()
		{
			Resource[] resources = this.GetValue().OfType<Resource>().ToArray();

			// Check again which actions are available, in case this has changed in the mean time.
			CheckAvailableActions(resources);
			if (!this.CanShowSourceFiles) return;

			// Show the primary source file of each of the selected Resources.
			foreach (Resource res in resources)
			{
				string[] sourceFiles = AssetManager.GetAssetSourceFiles(res);
				if (sourceFiles.Length == 0) continue;

				string primarySource = sourceFiles.FirstOrDefault(f => File.Exists(f));
				if (primarySource == null) continue;
				
				EditorHelper.ShowInExplorer(primarySource);
			}
		}
		/// <summary>
		/// Performs an export operation for the current <see cref="Resource"/> selection.
		/// </summary>
		private void ExportResource()
		{
			Resource[] resources = this.GetValue().OfType<Resource>().ToArray();

			// Check again which actions are available, in case this has changed in the mean time.
			CheckAvailableActions(resources);
			if (!this.CanExportResource) return;

			// Perform the export operation
			bool anySuccess = false;
			foreach (Resource res in resources)
			{
				string[] result = AssetManager.ExportAssets(res);
				if (result != null && result.Length > 0) anySuccess = true;
			}

			// If we were successful, play a feedback sound
			if (anySuccess)
			{
				System.Media.SystemSounds.Asterisk.Play();
			}
		}
		/// <summary>
		/// Performs a re-import operation for the current <see cref="Resource"/> selection.
		/// </summary>
		private void ReImportResource()
		{
			Resource[] resources = this.GetValue().OfType<Resource>().ToArray();

			// Check again which actions are available, in case this has changed in the mean time.
			CheckAvailableActions(resources);
			if (!this.CanReImportResource) return;

			// Perform the re-import operation
			bool anySuccess = false;
			foreach (Resource res in resources)
			{
				string[] sourceFiles = AssetManager.GetAssetSourceFiles(res);
				if (sourceFiles.Length == 0) continue;

				AssetImportOutput[] result = AssetManager.ReImportAssets(sourceFiles);
				if (result != null && result.Length > 0) anySuccess = true;
			}

			// If we were successful, play a feedback sound
			if (anySuccess)
			{
				System.Media.SystemSounds.Asterisk.Play();
			}
		}

		/// <summary>
		/// Provides custom <see cref="HelpInfo"/> tooltips for the action buttons.
		/// </summary>
		/// <param name="localPos"></param>
		/// <param name="captured"></param>
		/// <returns></returns>
		HelpInfo IHelpProvider.ProvideHoverHelp(Point localPos, ref bool captured)
		{
			HelpInfo result = null;
			localPos.X += this.Location.X;
			localPos.Y += this.Location.Y;

			if (this.rectButtonShow.Contains(localPos))
				result = HelpInfo.FromText(EditorBaseRes.ActionName_ShowResourceSources, EditorBaseRes.ActionDesc_ShowResourceSources);
			else if (this.rectButtonExport.Contains(localPos))
				result = HelpInfo.FromText(EditorBaseRes.ActionName_ExportResource, EditorBaseRes.ActionDesc_ExportResource);
			else if (this.rectButtonReImport.Contains(localPos))
				result = HelpInfo.FromText(EditorBaseRes.ActionName_ReImportResource, EditorBaseRes.ActionDesc_ReImportResource);

			return result;
		}
	}
}
