﻿using System;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;
using AdamsLair.WinForms.PropertyEditing;

using Duality.Resources;
using Duality.Editor.AssetManagement;

namespace Duality.Editor.Plugins.ObjectInspector
{
	public partial class ObjectInspector : DockContent
	{
		private int                      runtimeId           = 0;
		private TimeSpan                 lastRefreshTime     = TimeSpan.MinValue;
		private TimeSpan                 lastRefreshGameTime = TimeSpan.MinValue;
		private ObjectSelection.Category selSchedMouseCat    = ObjectSelection.Category.None;
		private ObjectSelection          selSchedMouse       = null;
		private ObjectSelection          displaySel          = null;
		private ObjectSelection.Category displayCat          = ObjectSelection.Category.None;

		private ExpandState              gridExpandState     = new ExpandState();
		private ObjectInspectorSettings  userSettings        = new ObjectInspectorSettings();
		

		public int Id
		{
			get { return this.runtimeId; }
		}
		public ObjectInspectorSettings UserSettings
		{
			get { return this.userSettings; }
			set { this.userSettings = value; }
		}
		public bool LockedSelection
		{
			get { return this.buttonLock.Checked; }
			set { this.buttonLock.Checked = value; }
		}
		public ObjectSelection DisplayedSelection
		{
			get { return (this.timerSelectSched.Enabled || this.selSchedMouse != null) ? this.selSchedMouse : this.displaySel; }
			set { this.UpdateSelection(value, value.Categories); }
		}
		public ObjectSelection.Category DisplayedCategory
		{
			get { return (this.timerSelectSched.Enabled || this.selSchedMouse != null) ? this.selSchedMouseCat : this.displayCat; }
		}
		public bool EmptySelection
		{
			get { return (this.displaySel == null || this.displaySel.Empty) && (!this.timerSelectSched.Enabled || this.selSchedMouse == null); }
		}


		public ObjectInspector(int runtimeId)
		{
			this.InitializeComponent();
			this.runtimeId = runtimeId;
			this.toolStrip.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
		}
		
		public void ApplyUserSettings()
		{
			this.buttonAutoRefresh.Checked = this.userSettings.AutoRefresh;
			this.buttonLock.Checked = this.userSettings.Locked;
			this.buttonDebug.Checked = this.userSettings.DebugMode;
			this.buttonSortByName.Checked = this.userSettings.SortByName;
			this.Text = this.userSettings.TitleText;

			if (this.userSettings.ExpandState != null)
			{
				this.gridExpandState.LoadFromXml(this.userSettings.ExpandState);
			}
		}
		internal void SaveGridExpandState()
		{
			// gridExpandState is normally only updated when the current selection changes.
			// Make sure we have the latest information when saving UserData.
			this.gridExpandState.UpdateFrom(this.propertyGrid.MainEditor);

			XElement expandStateNode = new XElement("ExpandState");
			this.gridExpandState.SaveToXml(expandStateNode);
			this.userSettings.ExpandState = expandStateNode;
		}
		
		public void CopyTo(ObjectInspector other)
		{
			this.gridExpandState.UpdateFrom(this.propertyGrid.MainEditor);

			other.buttonAutoRefresh.Checked = this.buttonAutoRefresh.Checked;
			other.buttonLock.Checked = this.buttonLock.Checked;
			other.buttonDebug.Checked = this.buttonDebug.Checked;
			other.buttonSortByName.Checked = this.buttonSortByName.Checked;

			this.gridExpandState.CopyTo(other.gridExpandState);
		}

		public bool AcceptsSelection(ObjectSelection sel)
		{
			if (this.LockedSelection) return false;
			return true;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.ApplyUserSettings();
			this.UpdateButtons();

			// Add the global selection event once
			DualityEditorApp.SelectionChanged -= GlobalUpdateSelection;
			DualityEditorApp.SelectionChanged += GlobalUpdateSelection;

			DualityEditorApp.UpdatingEngine += this.DualityEditorApp_AfterUpdateDualityApp;
			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
			DualityEditorApp.Terminating += this.DualityEditorApp_Terminating;
			AssetManager.ImportFinished += this.AssetManager_ImportFinished;

			// Select something initially, if not done yet
			if (this.propertyGrid.Selection.Count() == 0)
				this.UpdateSelection(DualityEditorApp.Selection, DualityEditorApp.SelectionActiveCategory);
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			DualityEditorApp.UpdatingEngine -= this.DualityEditorApp_AfterUpdateDualityApp;
			DualityEditorApp.ObjectPropertyChanged -= this.DualityEditorApp_ObjectPropertyChanged;
			DualityEditorApp.Terminating -= this.DualityEditorApp_Terminating;
			AssetManager.ImportFinished -= this.AssetManager_ImportFinished;
		}
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.propertyGrid.Focus();
		}

		private void UpdateButtons()
		{
			this.buttonClone.Enabled = this.propertyGrid.Selection.Any();
			this.buttonLock.Enabled = true;
		}
		private void UpdateSelection(ObjectSelection sel, ObjectSelection.Category showCat)
		{
			this.selSchedMouse = null;
			this.selSchedMouseCat = ObjectSelection.Category.None;
			this.displaySel = sel;
			this.displayCat = showCat;

			if (showCat == ObjectSelection.Category.None) return;
			this.gridExpandState.UpdateFrom(this.propertyGrid.MainEditor);

			// Selection update may change MainEditor, unsubscribe from events
			if (this.propertyGrid.MainEditor is GroupedPropertyEditor)
			{
				GroupedPropertyEditor groupedMainEditor = this.propertyGrid.MainEditor as GroupedPropertyEditor;
				groupedMainEditor.EditorAdded -= this.MainEditor_EditorAdded;
			}

			if ((showCat & ObjectSelection.Category.GameObjCmp) != ObjectSelection.Category.None)
			{
				this.displaySel = new ObjectSelection(sel.GameObjects.Union(sel.Components.GameObject()));
				this.displayCat = ObjectSelection.Category.GameObjCmp;
				this.propertyGrid.SelectObjects(this.displaySel, false);
			}
			else if ((showCat & ObjectSelection.Category.Resource) != ObjectSelection.Category.None)
			{
				this.displaySel = new ObjectSelection(sel.Resources);
				this.displayCat = ObjectSelection.Category.Resource;
				this.propertyGrid.SelectObjects(this.displaySel, this.displaySel.Resources.Any(r => r.IsDefaultContent));
			}
			else if ((showCat & ObjectSelection.Category.Other) != ObjectSelection.Category.None)
			{
				this.displaySel = new ObjectSelection(sel.OtherObjects);
				this.displayCat = ObjectSelection.Category.Other;
				this.propertyGrid.SelectObjects(this.displaySel, false);
			}

			if (this.propertyGrid.MainEditor is GroupedPropertyEditor)
			{
				GroupedPropertyEditor groupedMainEditor = this.propertyGrid.MainEditor as GroupedPropertyEditor;
				groupedMainEditor.EditorAdded += this.MainEditor_EditorAdded;
			}

			this.gridExpandState.ApplyTo(this.propertyGrid.MainEditor);
			this.buttonClone.Enabled = this.propertyGrid.Selection.Any();
		}
		private void UpdateDisplayedValues(bool forceFullUpdate)
		{
			if (forceFullUpdate)
			{
				// Force a full update by re-selecting internally
				object[] obj = this.propertyGrid.Selection.ToArray();
				this.propertyGrid.SelectObject(null);
				this.propertyGrid.SelectObjects(obj, false, 100);
			}
			else
			{
				// A (selective) regular update - deferred to make sure we don't do it redundantly
				this.propertyGrid.UpdateFromObjects(100);
			}
		}

		private void MainEditor_EditorAdded(object sender, PropertyEditorEventArgs e)
		{
			// Make sure new editors start in the correct expand state
			GroupedPropertyEditor groupedEditor = e.Editor as GroupedPropertyEditor;
			if (groupedEditor != null)
				groupedEditor.Expanded = this.gridExpandState.IsEditorExpanded(groupedEditor);
		}
		private void DualityEditorApp_AfterUpdateDualityApp(object sender, EventArgs e)
		{
			// Perform auto-refresh as the game state changes
			if (this.buttonAutoRefresh.Checked)
			{
				// Continuous updates during play mode
				if (Sandbox.State == SandboxState.Playing)
				{
					// Sandbox was restarted - MainTimer got reset
					if (this.lastRefreshTime > Time.MainTimer)
						this.lastRefreshTime = Time.MainTimer;

					if ((Time.MainTimer - this.lastRefreshTime).TotalMilliseconds > 100.0d)
					{
						this.lastRefreshTime = Time.MainTimer;
						this.lastRefreshGameTime = Time.GameTimer;
						this.propertyGrid.UpdateFromObjects();
					}
				}
				// Distinct updates on simulation time changes / single-step
				else if (Sandbox.State == SandboxState.Paused)
				{
					if (this.lastRefreshGameTime != Time.GameTimer)
					{
						this.lastRefreshTime = Time.MainTimer;
						this.lastRefreshGameTime = Time.GameTimer;
						this.propertyGrid.UpdateFromObjects();
					}
				}
			}
		}
		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (!(e is PrefabAppliedEventArgs) && (sender is PropertyEditor) && (sender as PropertyEditor).ParentGrid == this.propertyGrid) return;
			if (!(e is PrefabAppliedEventArgs) && sender == this.propertyGrid) return;

			// Update values if anything changed that relates to the grids current selection
			if (e.Objects.Components.GameObject().Any(o => this.propertyGrid.Selection.Contains(o)) ||
				e.Objects.Any(o => this.propertyGrid.Selection.Contains(o)) ||
				(e.Objects.Contains(Scene.Current) && this.propertyGrid.Selection.Any(o => o is GameObject || o is Component)))
				this.propertyGrid.UpdateFromObjects(100);
		}
		private void AssetManager_ImportFinished(object sender, AssetImportFinishedEventArgs e)
		{
			if (!e.IsSuccessful) return;
			if (!e.IsReImport) return;

			// Force updating all potentially generated previews when we display a resource that was modified externally
			bool forceFullUpdate = false;
			foreach (AssetImportOutput item in e.Output)
			{
				if (this.propertyGrid.Selection.Contains(item.Resource))
				{
					forceFullUpdate = true;
					break;
				}
			}

			this.UpdateDisplayedValues(forceFullUpdate);
		}
		private void DualityEditorApp_Terminating(object sender, EventArgs e)
		{
			if (this.runtimeId == -1) this.Close();
		}
		private static void GlobalUpdateSelection(object sender, SelectionChangedEventArgs e)
		{
			ObjectInspector target = null;
			
			foreach (ObjectSelection.Category cat in ObjectSelection.EnumerateCategories(e.AffectedCategories))
			{
				var objViews = 
					from v in ObjectInspectorPlugin.Instance.ObjViews
					where v.AcceptsSelection(e.Current)
					select new { 
						View = v, 
						Empty = v.EmptySelection,
						PerfectFit = v.EmptySelection || (cat & v.DisplayedCategory) != ObjectSelection.Category.None,
						TypeShare = ObjectSelection.GetTypeShareLevel(e.Current.Exclusive(cat), v.DisplayedSelection),
						NumSameCatViews = ObjectInspectorPlugin.Instance.ObjViews.Count(o => o.AcceptsSelection(e.Current) && o.DisplayedCategory == v.DisplayedCategory) };
				var sortedQuery = 
					from o in objViews
					orderby o.PerfectFit descending, o.Empty ascending, o.NumSameCatViews descending, o.TypeShare ascending
					select o;
				var targetItem = sortedQuery.FirstOrDefault();
				if (targetItem == null) return;
				target = targetItem.View;

				// If a mouse button is pressed, reschedule the selection for later - there might be a drag in progress
				if (Control.MouseButtons != System.Windows.Forms.MouseButtons.None)
				{
					target.selSchedMouse = e.Current;
					target.selSchedMouseCat = cat;
					target.timerSelectSched.Enabled = true;
				}
				else
				{
					target.UpdateSelection(e.Current, cat);
				}
			}
			
			//  Make sure disposed objects are deselected in non-target views (locked, etc.)
			foreach (ObjectInspector v in ObjectInspectorPlugin.Instance.ObjViews)
			{
				if (v.EmptySelection) continue;
				if (v == target) continue;

				var disposedObj = e.Removed.OfType<IManageableObject>().Where(o => o.Disposed);
				if (disposedObj.Any())
				{
					ObjectSelection disposedSel = new ObjectSelection(disposedObj);
					v.UpdateSelection(v.DisplayedSelection - disposedSel, v.DisplayedCategory);
				}
			}
		}

		private void timerSelectSched_Tick(object sender, EventArgs e)
		{
			if (this.selSchedMouse == null)
			{
				this.timerSelectSched.Enabled = false;
			}
			else if (Control.MouseButtons == System.Windows.Forms.MouseButtons.None)
			{
				// If no more mouse buttons are currently pressed, process previously scheduled selection change...
				// .. but only if the cursor isn't located on this Control. That might mean something has been dragged here.
				Point curRelPos = this.PointToClient(Cursor.Position);
				if (curRelPos.X < 0 || curRelPos.Y < 0 || curRelPos.X > this.Bounds.Width || curRelPos.Y > this.Bounds.Height)
				{
					this.UpdateSelection(this.selSchedMouse, this.selSchedMouseCat);
				}
				this.timerSelectSched.Enabled = false;
			}
		}
		private void buttonClone_Click(object sender, EventArgs e)
		{
			ObjectInspector objView = ObjectInspectorPlugin.Instance.RequestObjView(true);
			this.CopyTo(objView);
			objView.buttonLock.Checked = true;

			DockPanel mainDoc = DualityEditorApp.MainForm.MainDockPanel;
			if (this.DockHandler.DockState.IsAutoHide())
				objView.Show(this.DockPanel, this.DockHandler.DockState);
			else
				objView.Show(this.DockHandler.Pane, DockAlignment.Bottom, 0.5d);
			
			// Need it before showing because of instant-selection
			objView.propertyGrid.SelectObjects(this.propertyGrid.Selection);
			objView.gridExpandState.ApplyTo(objView.propertyGrid.MainEditor);
		}
		private void buttonAutoRefresh_CheckedChanged(object sender, EventArgs e)
		{
			this.UserSettings.AutoRefresh = this.buttonAutoRefresh.Checked;
			if (this.buttonAutoRefresh.Checked)
			{
				this.lastRefreshTime = Time.MainTimer;
				this.lastRefreshGameTime = Time.GameTimer;
				this.propertyGrid.UpdateFromObjects(100);
			}
		}
		private void buttonDebug_CheckedChanged(object sender, EventArgs e)
		{
			this.UserSettings.DebugMode = this.buttonDebug.Checked;
			this.propertyGrid.ShowNonPublic = this.buttonDebug.Checked;
		}
		private void buttonSortByName_CheckedChanged(object sender, EventArgs e)
		{
			this.UserSettings.SortByName = this.buttonSortByName.Checked;
			this.propertyGrid.SortEditorsByName = this.buttonSortByName.Checked;
		}
		private void buttonLock_CheckedChanged(object sender, EventArgs e)
		{
			this.UserSettings.Locked = this.buttonLock.Checked;
		}
	}
}
