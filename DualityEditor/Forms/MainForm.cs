using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using Duality;
using Duality.Resources;
using Duality.Editor.Properties;

using WeifenLuo.WinFormsUI.Docking;

using AdamsLair.WinForms.ItemModels;
using AdamsLair.WinForms.ItemViews;

namespace Duality.Editor.Forms
{
	public partial class MainForm : Form, IHelpProvider
	{
		private	bool				nonUserClosing	= false;
		private	ToolStripMenuItem	activeMenu		= null;
		private MenuModel			mainMenuModel	= new MenuModel();
		private	MenuStripMenuView	mainMenuView	= null;

		// Hardcoded main menu items
		private MenuModelItem	menuRunSandboxPlay		= null;
		private MenuModelItem	menuRunSandboxPause		= null;
		private MenuModelItem	menuRunSandboxStop		= null;
		private MenuModelItem	menuRunSandboxStep		= null;
		private MenuModelItem	menuRunSandboxFaster	= null;
		private MenuModelItem	menuRunSandboxSlower	= null;
		private MenuModelItem	menuEditUndo			= null;
		private MenuModelItem	menuEditRedo			= null;
		private MenuModelItem	menuRunApp				= null;
		private MenuModelItem	menuDebugApp			= null;
		private MenuModelItem	menuProfileApp			= null;


		public DockPanel MainDockPanel
		{
			get { return this.dockPanel; }
		}
		public MenuModel MainMenu
		{
			get { return this.mainMenuModel; }
		}



		internal MainForm()
		{
			this.InitializeComponent();
			this.ApplyDockPanelSkin();
			this.mainMenuStrip.Renderer = new Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
			this.mainToolStrip.Renderer = new Controls.ToolStrip.DualitorToolStripProfessionalRenderer();

			this.splitButtonBackupSettings.DropDown.Closing += this.splitButtonBackupSettings_Closing;
			this.menuAutosave.DropDown.Closing += this.menuAutosave_Closing;

			this.InitMenus();
			this.UpdateLaunchAppActions();
		}
		private void ApplyDockPanelSkin()
		{
			Color bgColor = Color.FromArgb(255, 162, 162, 162);
			Color fgColor = Color.FromArgb(255, 196, 196, 196);
			Color inactiveTab = Color.FromArgb(255, 192, 192, 192);
			Color inactiveTab2 = Color.FromArgb(255, 224, 224, 224);
			Color activeTab = Color.FromArgb(255, 224, 224, 224);
			Color activeTab2 = Color.FromArgb(255, 242, 242, 242);

			this.dockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor = bgColor;
			this.dockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.EndColor = bgColor;

			this.dockPanel.Skin.AutoHideStripSkin.DockStripGradient.StartColor = bgColor;
			this.dockPanel.Skin.AutoHideStripSkin.DockStripGradient.EndColor = bgColor;
			this.dockPanel.Skin.AutoHideStripSkin.TabGradient.StartColor = fgColor;
			this.dockPanel.Skin.AutoHideStripSkin.TabGradient.EndColor = fgColor;

			this.dockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.StartColor = activeTab2;
			this.dockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.EndColor = activeTab;
			this.dockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.StartColor = inactiveTab2;
			this.dockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.EndColor = inactiveTab;

			this.dockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.StartColor = fgColor;
			this.dockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.EndColor = fgColor;
			this.dockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;

			this.dockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.StartColor = bgColor;
			this.dockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.EndColor = bgColor;
			this.dockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;

			this.dockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.StartColor = bgColor;
			this.dockPanel.Skin.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.EndColor = bgColor;

			this.dockPanel.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.StartColor = fgColor;
			this.dockPanel.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.EndColor = fgColor;
			this.dockPanel.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.dockPanel.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.TextColor = Color.Black;

			this.dockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.StartColor = activeTab2;
			this.dockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.EndColor = activeTab;
			this.dockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.dockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.TextColor = Color.Black;
		}
		
		public void CloseNonUser()
		{
			// Because FormClosingEventArgs.CloseReason is UserClosing on this.Close()
			this.nonUserClosing = true;
			this.Close();
			this.nonUserClosing = false;
		}
		public void InitMenus()
		{
			this.mainMenuView = new MenuStripMenuView(this.mainMenuStrip.Items);
			this.mainMenuView.ItemInserted += this.mainMenuView_ItemInserted;
			this.mainMenuView.ItemRemoved += this.mainMenuView_ItemRemoved;
			this.mainMenuView.Model = this.mainMenuModel;

			MenuModelItem helpItem;
			this.mainMenuModel.AddItems(new[]
			{
				new MenuModelItem { Name = GeneralRes.MenuName_File, SortValue = MenuModelItem.SortValue_Top, Items = new[]
				{
					new MenuModelItem
					{
						Name			= GeneralRes.MenuItemName_NewProject,
						SortValue		= MenuModelItem.SortValue_Top,
						Icon			= Properties.GeneralResCache.ImageAppCreate,
						Tag				= HelpInfo.FromText(GeneralRes.MenuItemName_NewProject, GeneralRes.MenuItemInfo_NewProject),
						ActionHandler	= this.newProjectItem_Click
					},
					new MenuModelItem
					{
						Name			= GeneralRes.MenuItemName_PublishGame,
						SortValue		= MenuModelItem.SortValue_Top,
						Tag				= HelpInfo.FromText(GeneralRes.MenuItemName_PublishGame, GeneralRes.MenuItemInfo_PublishGame),
						ActionHandler	= this.actionPublishGame_Click
					},
					new MenuModelItem
					{
						Name			= "TopSeparator",
						SortValue		= MenuModelItem.SortValue_Top,
						TypeHint		= MenuItemTypeHint.Separator
					},
					new MenuModelItem
					{
						Name			= this.actionSaveAll.Text,
						Icon			= this.actionSaveAll.Image,
						ShortcutKeys	= Keys.Control | Keys.S,
						Tag				= HelpInfo.FromText(this.actionSaveAll.Text, GeneralRes.MenuItemInfo_SaveAll),
						ActionHandler	= this.actionSaveAll_Click
					},
					new MenuModelItem
					{
						Name			= "CodeSeparator",
						TypeHint		= MenuItemTypeHint.Separator
					},
					new MenuModelItem
					{
						Name			= this.actionOpenCode.Text,
						Icon			= this.actionOpenCode.Image,
						Tag				= HelpInfo.FromText(this.actionOpenCode.Text, GeneralRes.MenuItemInfo_OpenProjectSource),
						ActionHandler	= this.actionOpenCode_Click
					},
					new MenuModelItem
					{
						Name			= "BottomSeparator",
						SortValue		= MenuModelItem.SortValue_Bottom,
						TypeHint		= MenuItemTypeHint.Separator
					},
					new MenuModelItem
					{
						Name			= GeneralRes.MenuItemName_Quit,
						SortValue		= MenuModelItem.SortValue_Bottom,
						ShortcutKeys	= Keys.Alt | Keys.F4,
						ActionHandler	= this.quitItem_Click
					}
				}},
				new MenuModelItem { Name = GeneralRes.MenuName_Edit, SortValue = MenuModelItem.SortValue_Top, Items = new[]
				{
					this.menuEditUndo = new MenuModelItem
					{
						Name			= GeneralRes.MenuItemName_Undo,
						SortValue		= MenuModelItem.SortValue_Top,
						Icon			= GeneralResCache.arrow_undo,
						ShortcutKeys	= Keys.Z | Keys.Control,
						ActionHandler	= this.menuEditUndo_Click
					},
					this.menuEditRedo = new MenuModelItem
					{
						Name			= GeneralRes.MenuItemName_Redo,
						SortValue		= MenuModelItem.SortValue_Top,
						Icon			= GeneralResCache.arrow_redo,
						ShortcutKeys	= Keys.Y | Keys.Control,
						ActionHandler	= this.menuEditRedo_Click
					}
				}},
				new MenuModelItem { Name = GeneralRes.MenuName_Run, SortValue = MenuModelItem.SortValue_OverBottom, Items = new[]
				{
					this.menuRunApp = new MenuModelItem
					{
						Name			= this.actionRunApp.Text,
						SortValue		= MenuModelItem.SortValue_Top,
						Icon			= this.actionRunApp.Image,
						ShortcutKeys	= Keys.Alt | Keys.F5,
						Tag				= HelpInfo.FromText(this.actionRunApp.Text, GeneralRes.MenuItemInfo_RunGame),
						ActionHandler	= this.actionRunApp_Click
					},
					this.menuDebugApp = new MenuModelItem
					{
						Name			= this.actionDebugApp.Text,
						SortValue		= MenuModelItem.SortValue_Top,
						Icon			= this.actionDebugApp.Image,
						ShortcutKeys	= Keys.Alt | Keys.F6,
						Tag				= HelpInfo.FromText(this.actionDebugApp.Text, GeneralRes.MenuItemInfo_DebugGame),
						ActionHandler	= this.actionDebugApp_Click
					},
					this.menuProfileApp = new MenuModelItem
					{
						Name			= GeneralRes.MenuItemName_ProfileGame,
						SortValue		= MenuModelItem.SortValue_Top,
						Icon			= Properties.Resources.application_stopwatch,
						Tag				= HelpInfo.FromText(GeneralRes.MenuItemName_ProfileGame, GeneralRes.MenuItemInfo_ProfileGame),
						ActionHandler	= this.actionProfileApp_Click
					},
					new MenuModelItem
					{
						Name			= GeneralRes.MenuItemName_ConfigureLauncher,
						SortValue		= MenuModelItem.SortValue_Top,
						Tag				= HelpInfo.FromText(GeneralRes.MenuItemName_ConfigureLauncher, GeneralRes.MenuItemInfo_ConfigureLauncher),
						ActionHandler	= this.actionConfigureLauncher_Click
					},
					new MenuModelItem
					{
						Name			= "TopSeparator",
						SortValue		= MenuModelItem.SortValue_Top,
						TypeHint		= MenuItemTypeHint.Separator
					},
					this.menuRunSandboxPlay = new MenuModelItem
					{
						Name			= this.actionRunSandbox.Text,
						Icon			= this.actionRunSandbox.Image,
						ShortcutKeys	= Keys.F5,
						Tag				= HelpInfo.FromText(this.actionRunSandbox.Text, GeneralRes.MenuItemInfo_SandboxPlay),
						ActionHandler	= this.actionRunSandbox_Click
					},
					this.menuRunSandboxStep = new MenuModelItem
					{
						Name			= this.actionStepSandbox.Text,
						Icon			= this.actionStepSandbox.Image,
						ShortcutKeys	= Keys.F6,
						Tag				= HelpInfo.FromText(this.actionStepSandbox.Text, GeneralRes.MenuItemInfo_SandboxStep),
						ActionHandler	= this.actionStepSandbox_Click
					},
					this.menuRunSandboxPause = new MenuModelItem
					{
						Name			= this.actionPauseSandbox.Text,
						Icon			= this.actionPauseSandbox.Image,
						ShortcutKeys	= Keys.F7,
						Tag				= HelpInfo.FromText(this.actionPauseSandbox.Text, GeneralRes.MenuItemInfo_SandboxPause),
						ActionHandler	= this.actionPauseSandbox_Click
					},
					this.menuRunSandboxStop = new MenuModelItem
					{
						Name			= this.actionStopSandbox.Text,
						Icon			= this.actionStopSandbox.Image,
						ShortcutKeys	= Keys.F8,
						Tag				= HelpInfo.FromText(this.actionStopSandbox.Text, GeneralRes.MenuItemInfo_SandboxStop),
						ActionHandler	= this.actionStopSandbox_Click
					},
					new MenuModelItem
					{
						Name			= "BottomSeparator",
						SortValue		= MenuModelItem.SortValue_Bottom,
						TypeHint		= MenuItemTypeHint.Separator
					},
					this.menuRunSandboxSlower = new MenuModelItem
					{
						Name			= GeneralRes.MenuItemName_SandboxSlower,
						ShortcutKeys	= Keys.F9,
						Tag				= HelpInfo.FromText(GeneralRes.MenuItemName_SandboxSlower, GeneralRes.MenuItemInfo_SandboxSlower),
						ActionHandler	= this.menuRunSandboxSlower_Click
					},
					this.menuRunSandboxFaster = new MenuModelItem
					{
						Name			= GeneralRes.MenuItemName_SandboxFaster,
						ShortcutKeys	= Keys.F10,
						Tag				= HelpInfo.FromText(GeneralRes.MenuItemName_SandboxFaster, GeneralRes.MenuItemInfo_SandboxFaster),
						ActionHandler	= this.menuRunSandboxFaster_Click
					}
				}},
				helpItem = new MenuModelItem { Name = GeneralRes.MenuName_Help, SortValue = MenuModelItem.SortValue_Bottom, Items = new[]
				{
					new MenuModelItem
					{
						Name			= GeneralRes.MenuItemName_About,
						SortValue		= MenuModelItem.SortValue_Top,
						ActionHandler	= this.aboutItem_Click
					}
				}}
			});

			// Set some view-specific properties
			ToolStripItem helpViewItem = this.mainMenuView.GetViewItem(helpItem);
			helpViewItem.Alignment = ToolStripItemAlignment.Right;

			// Attach help data to toolstrip actions
			this.actionOpenCode.Tag = HelpInfo.FromText(this.actionOpenCode.Text, GeneralRes.MenuItemInfo_OpenProjectSource);
			this.actionSaveAll.Tag = HelpInfo.FromText(this.actionSaveAll.Text, GeneralRes.MenuItemInfo_SaveAll);
			this.actionRunApp.Tag = HelpInfo.FromText(this.actionRunApp.Text, GeneralRes.MenuItemInfo_RunGame);
			this.actionDebugApp.Tag = HelpInfo.FromText(this.actionDebugApp.Text, GeneralRes.MenuItemInfo_DebugGame);
			this.actionRunSandbox.Tag = HelpInfo.FromText(this.actionRunSandbox.Text, GeneralRes.MenuItemInfo_SandboxPlay);
			this.actionStepSandbox.Tag = HelpInfo.FromText(this.actionStepSandbox.Text, GeneralRes.MenuItemInfo_SandboxStep);
			this.actionPauseSandbox.Tag = HelpInfo.FromText(this.actionPauseSandbox.Text, GeneralRes.MenuItemInfo_SandboxPause);
			this.actionStopSandbox.Tag = HelpInfo.FromText(this.actionStopSandbox.Text, GeneralRes.MenuItemInfo_SandboxStop);
			this.formatUpdateAll.Tag = HelpInfo.FromText(this.formatUpdateAll.Text, GeneralRes.MenuItemInfo_FormatUpdateAll);
		}
		private void UpdateToolbar()
		{
			this.actionRunSandbox.Enabled	= Sandbox.State != SandboxState.Playing;
			this.actionStepSandbox.Enabled	= Sandbox.State != SandboxState.Playing;
			this.actionStopSandbox.Enabled	= Sandbox.State != SandboxState.Inactive;
			this.actionPauseSandbox.Enabled	= Sandbox.State == SandboxState.Playing;

			this.menuRunSandboxPlay.Enabled = this.actionRunSandbox.Enabled;
			this.menuRunSandboxStep.Enabled = this.actionStepSandbox.Enabled;
			this.menuRunSandboxStop.Enabled = this.actionStopSandbox.Enabled;
			this.menuRunSandboxPause.Enabled = this.actionPauseSandbox.Enabled;
			this.menuRunSandboxSlower.Enabled = Sandbox.State != SandboxState.Inactive;
			this.menuRunSandboxFaster.Enabled = Sandbox.State != SandboxState.Inactive;

			if (Duality.Serialization.Serializer.DefaultMethod == Duality.Serialization.SerializeMethod.Xml)
			{
				this.selectFormattingMethod.Image = this.formatXml.Image;
				this.formatXml.Checked = true;
				this.formatBinary.Checked = false;
			}
			else
			{
				this.selectFormattingMethod.Image = this.formatBinary.Image;
				this.formatXml.Checked = false;
				this.formatBinary.Checked = true;
			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			this.WindowState = FormWindowState.Maximized;
			this.UpdateToolbar();

			Sandbox.StateChanged += this.Sandbox_StateChanged;
			UndoRedoManager.StackChanged += this.UndoRedoManager_StackChanged;

			// Initially update Undo / Redo menu
			this.UndoRedoManager_StackChanged(null, EventArgs.Empty);
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			Sandbox.StateChanged -= this.Sandbox_StateChanged;
		}
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			bool userCloseReason;
			switch (e.CloseReason)
			{
				default:
				case CloseReason.ApplicationExitCall:
				case CloseReason.WindowsShutDown:
				case CloseReason.TaskManagerClosing:
					userCloseReason = false;
					break;
				case CloseReason.FormOwnerClosing:
				case CloseReason.MdiFormClosing:
				case CloseReason.UserClosing:
					userCloseReason = true;
					break;
			}
			e.Cancel = !DualityEditorApp.Terminate(userCloseReason && !this.nonUserClosing && !DualityEditorApp.IsReloadingPlugins);
		}
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			Application.Exit();
		}

		private void actionRunApp_Click(object sender, EventArgs e)
		{
			DualityEditorApp.SaveAllProjectData();
			this.VerifyStartScene();

			System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
			startInfo.FileName = Path.GetFullPath(DualityEditorApp.LauncherAppPath);
			startInfo.Arguments = DualityApp.CmdArgEditor;
			startInfo.WorkingDirectory = Environment.CurrentDirectory;
			System.Diagnostics.Process appProc = System.Diagnostics.Process.Start(startInfo);

			AppRunningDialog runningDialog = new AppRunningDialog(appProc);
			runningDialog.ShowDialog(this);
		}
		private void actionDebugApp_Click(object sender, EventArgs e)
		{
			DualityEditorApp.SaveAllProjectData();
			this.VerifyStartScene();

			System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
			startInfo.FileName = Path.GetFullPath(DualityEditorApp.LauncherAppPath);
			startInfo.Arguments = DualityApp.CmdArgEditor + " " + DualityApp.CmdArgDebug;
			startInfo.WorkingDirectory = Environment.CurrentDirectory;
			System.Diagnostics.Process appProc = System.Diagnostics.Process.Start(startInfo);

			AppRunningDialog runningDialog = new AppRunningDialog(appProc);
			runningDialog.ShowDialog(this);
		}
		private void actionProfileApp_Click(object sender, EventArgs e)
		{
			DualityEditorApp.SaveAllProjectData();
			this.VerifyStartScene();

			System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
			startInfo.FileName = Path.GetFullPath(DualityEditorApp.LauncherAppPath);
			startInfo.Arguments = DualityApp.CmdArgEditor + " " + DualityApp.CmdArgProfiling;
			startInfo.WorkingDirectory = Environment.CurrentDirectory;
			System.Diagnostics.Process appProc = System.Diagnostics.Process.Start(startInfo);

			AppRunningDialog runningDialog = new AppRunningDialog(appProc);
			runningDialog.ShowDialog(this);
		}
		private void actionConfigureLauncher_Click(object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.InitialDirectory = Path.GetDirectoryName(DualityEditorApp.LauncherAppPath);
			if (string.IsNullOrWhiteSpace(fileDialog.InitialDirectory))
				fileDialog.InitialDirectory = Environment.CurrentDirectory;
			fileDialog.FileName = Path.GetFileName(DualityEditorApp.LauncherAppPath);
			fileDialog.Filter = "Executable files (*.exe)|*.exe";
			fileDialog.FilterIndex = 1;
			fileDialog.RestoreDirectory = true;
			fileDialog.Multiselect = false;
			fileDialog.Title = GeneralRes.MenuItemName_ConfigureLauncher;
			fileDialog.CheckFileExists = true;
			fileDialog.CheckPathExists = true;
			fileDialog.CustomPlaces.Add(Environment.CurrentDirectory);
			if (fileDialog.ShowDialog(this) == DialogResult.OK)
			{
				DualityEditorApp.LauncherAppPath = PathHelper.MakeFilePathRelative(fileDialog.FileName);
				this.UpdateLaunchAppActions();
			}
		}
		private void actionSaveAll_Click(object sender, EventArgs e)
		{
			DualityEditorApp.SaveAllProjectData();
			System.Media.SystemSounds.Asterisk.Play();
		}
		private void actionOpenCode_Click(object sender, EventArgs e)
		{
			DualityEditorApp.UpdatePluginSourceCode();
			System.Diagnostics.Process.Start(EditorHelper.SourceCodeSolutionFile);
		}
		private void actionPublishGame_Click(object sender, EventArgs e) {
			PublishGameDialog dialog = new PublishGameDialog();
			dialog.ShowDialog();
		}
		private void actionRunSandbox_Click(object sender, EventArgs e)
		{
			Sandbox.Play();
		}
		private void actionStepSandbox_Click(object sender, EventArgs e)
		{
			if (Sandbox.State == SandboxState.Paused)
				Sandbox.Step();
			else if (Sandbox.State == SandboxState.Inactive)
			{
				if (Sandbox.Play())
					Sandbox.Pause();
			}
		}
		private void actionPauseSandbox_Click(object sender, EventArgs e)
		{
			Sandbox.Pause();
		}
		private void actionStopSandbox_Click(object sender, EventArgs e)
		{
			Sandbox.Stop();
		}
		private void menuRunSandboxSlower_Click(object sender, EventArgs e)
		{
			Sandbox.Slower();
		}
		private void menuRunSandboxFaster_Click(object sender, EventArgs e)
		{
			Sandbox.Faster();
		}

		private void menuEditUndo_Click(object sender, EventArgs e)
		{
			UndoRedoManager.Undo();
		}
		private void menuEditRedo_Click(object sender, EventArgs e)
		{
			UndoRedoManager.Redo();
		}
		private void aboutItem_Click(object sender, EventArgs e)
		{
			AboutBox about = new AboutBox();
			about.ShowDialog(this);
		}
		private void newProjectItem_Click(object sender, EventArgs e)
		{
			NewProjectDialog newProject = new NewProjectDialog();
			DialogResult result = newProject.ShowDialog(this);

			// Project successfully created?
			if (result == DialogResult.OK)
			{
				// Open new project
				var startInfo = new System.Diagnostics.ProcessStartInfo(newProject.ResultEditorBinary);
				startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
				startInfo.UseShellExecute = false;
				System.Diagnostics.Process.Start(startInfo);

				// Don't need this DualityEditor anymore - close it!
				this.CloseNonUser();
			}
		}

		private void formatBinary_Click(object sender, EventArgs e)
		{
			if (Duality.Serialization.Serializer.DefaultMethod == Duality.Serialization.SerializeMethod.Binary) return;
			Duality.Serialization.Serializer.DefaultMethod = Duality.Serialization.SerializeMethod.Binary;
			this.UpdateToolbar();

			ProcessingBigTaskDialog taskDialog = new ProcessingBigTaskDialog(this, 
				Properties.GeneralRes.TaskChangeDataFormat_Caption, 
				string.Format(Properties.GeneralRes.TaskChangeDataFormat_Desc, Duality.Serialization.Serializer.DefaultMethod.ToString()), 
				this.async_ChangeDataFormat, null);
			taskDialog.ShowDialog();
		}
		private void formatXml_Click(object sender, EventArgs e)
		{
			if (Duality.Serialization.Serializer.DefaultMethod == Duality.Serialization.SerializeMethod.Xml) return;
			Duality.Serialization.Serializer.DefaultMethod = Duality.Serialization.SerializeMethod.Xml;
			this.UpdateToolbar();

			ProcessingBigTaskDialog taskDialog = new ProcessingBigTaskDialog(this, 
				Properties.GeneralRes.TaskChangeDataFormat_Caption, 
				string.Format(Properties.GeneralRes.TaskChangeDataFormat_Desc, Duality.Serialization.Serializer.DefaultMethod.ToString()), 
				this.async_ChangeDataFormat, null);
			taskDialog.ShowDialog();
		}
		private void formatUpdateAll_Click(object sender, EventArgs e)
		{
			ProcessingBigTaskDialog taskDialog = new ProcessingBigTaskDialog(this, 
				Properties.GeneralRes.TaskFormatUpdateAll_Caption, 
				Properties.GeneralRes.TaskFormatUpdateAll_Desc, 
				this.async_ChangeDataFormat, null);
			taskDialog.ShowDialog();
		}
		private void selectFormattingMethod_Click(object sender, EventArgs e)
		{
			this.selectFormattingMethod.ShowDropDown();
		}
		
		private void Sandbox_StateChanged(object sender, EventArgs e)
		{
			this.UpdateToolbar();
		}
		private void UndoRedoManager_StackChanged(object sender, EventArgs e)
		{
			IUndoRedoActionInfo prevInfo = UndoRedoManager.PrevActionInfo;
			IUndoRedoActionInfo nextInfo = UndoRedoManager.NextActionInfo;

			this.menuEditUndo.Enabled = UndoRedoManager.CanUndo;
			this.menuEditUndo.Name = prevInfo != null ? string.Format(GeneralRes.MenuItemName_Undo, prevInfo.Name) : GeneralRes.MenuItemName_UndoEmpty;
			this.menuEditUndo.Tag = prevInfo != null ? prevInfo.Help : null;

			this.menuEditRedo.Enabled = UndoRedoManager.CanRedo;
			this.menuEditRedo.Name = nextInfo != null ? string.Format(GeneralRes.MenuItemName_Redo, nextInfo.Name) : GeneralRes.MenuItemName_RedoEmpty;
			this.menuEditRedo.Tag = nextInfo != null ? nextInfo.Help : null;
		}

		private System.Collections.IEnumerable async_ChangeDataFormat(ProcessingBigTaskDialog.WorkerInterface state)
		{
			state.StateDesc = "DualityApp Data"; yield return null;
			DualityApp.LoadAppData();
			DualityApp.LoadUserData();
			state.Progress += 0.05f; yield return null;
					
			DualityApp.SaveAppData();
			DualityApp.SaveUserData();
			state.Progress += 0.05f; yield return null;

			// Special case: Current Scene in sandbox mode
			if (Sandbox.IsActive && !string.IsNullOrEmpty(Scene.CurrentPath))
			{
				// Because changes we'll do will be discarded when leaving the sandbox we'll need to
				// do it the hard way - manually load an save the file.
				state.StateDesc = "Current Scene"; yield return null;
				Scene curScene = Resource.Load<Scene>(Scene.CurrentPath, null, false);
				if (curScene != null)
				{
					curScene.Save(null, false);
				}
			}

			var loadedContent = ContentProvider.GetLoadedContent<Resource>();
			List<string> resFiles = Resource.GetResourceFiles();
			foreach (string file in resFiles)
			{
				if (Sandbox.IsActive && file == Scene.CurrentPath) continue; // Skip current Scene in Sandbox
				state.StateDesc = file; yield return null;

				// Wasn't loaded before? Unload it later to keep the memory footprint small.
				bool wasLoaded = loadedContent.Any(r => r.Path == file);

				if (wasLoaded)
				{
					// Retrieve already loaded content
					var cr = ContentProvider.RequestContent(file);
					state.Progress += 0.45f / resFiles.Count; yield return null;

					// Perform rename and flag unsaved / modified
					cr.Res.Save();
					state.Progress += 0.45f / resFiles.Count; yield return null;
				}
				else
				{
					// Load content without initializing it
					Resource res = Resource.Load<Resource>(file, null, false);
					state.Progress += 0.45f / resFiles.Count; yield return null;

					// Perform rename and save it without making it globally available
					res.Save(null, false);
					state.Progress += 0.45f / resFiles.Count; yield return null;
				}
			}
		}

		private void checkBackups_Clicked(object sender, EventArgs e)
		{
			DualityEditorApp.BackupsEnabled = !DualityEditorApp.BackupsEnabled;
			this.UpdateSplitButtonBackupSettings();
		}
		private void optionAutosaveDisabled_Clicked(object sender, EventArgs e)
		{
			DualityEditorApp.Autosaves = DualityEditorApp.Autosaves != AutosaveFrequency.Disabled ? AutosaveFrequency.Disabled : AutosaveFrequency.ThirtyMinutes;
			this.UpdateSplitButtonBackupSettings();
		}
		private void optionAutosaveTenMinutes_Clicked(object sender, EventArgs e)
		{
			DualityEditorApp.Autosaves = DualityEditorApp.Autosaves != AutosaveFrequency.TenMinutes ? AutosaveFrequency.TenMinutes : AutosaveFrequency.Disabled;
			this.UpdateSplitButtonBackupSettings();
		}
		private void optionAutosaveThirtyMinutes_Clicked(object sender, EventArgs e)
		{
			DualityEditorApp.Autosaves = DualityEditorApp.Autosaves != AutosaveFrequency.ThirtyMinutes ? AutosaveFrequency.ThirtyMinutes : AutosaveFrequency.Disabled;
			this.UpdateSplitButtonBackupSettings();
		}
		private void optionAutoSaveOneHour_Clicked(object sender, EventArgs e)
		{
			DualityEditorApp.Autosaves = DualityEditorApp.Autosaves != AutosaveFrequency.OneHour ? AutosaveFrequency.OneHour : AutosaveFrequency.Disabled;
			this.UpdateSplitButtonBackupSettings();
		}
		private void splitButtonBackupSettings_DropDownOpening(object sender, EventArgs e)
		{
			this.UpdateSplitButtonBackupSettings();
		}
		private void splitButtonBackupSettings_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked) e.Cancel = true;
		}
		private void splitButtonBackupSettings_Click(object sender, EventArgs e)
		{
			this.splitButtonBackupSettings.ShowDropDown();
		}
		private void menuAutosave_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked) e.Cancel = true;
		}
		private void quitItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void mainMenuView_ItemRemoved(object sender, MenuStripMenuViewItemEventArgs e)
		{
			if (e.ViewItem is ToolStripMenuItem && e.Modelitem.Parent == null)
			{
				ToolStripMenuItem mainMenuItem = e.ViewItem as ToolStripMenuItem;
				mainMenuItem.DropDownOpened -= this.mainMenuItem_DropDownOpened;
				mainMenuItem.DropDownClosed -= this.mainMenuItem_DropDownClosed;
			}
		}
		private void mainMenuView_ItemInserted(object sender, MenuStripMenuViewItemEventArgs e)
		{
			if (e.ViewItem is ToolStripMenuItem && e.Modelitem.Parent == null)
			{
				ToolStripMenuItem mainMenuItem = e.ViewItem as ToolStripMenuItem;
				mainMenuItem.DropDownOpened += this.mainMenuItem_DropDownOpened;
				mainMenuItem.DropDownClosed += this.mainMenuItem_DropDownClosed;
			}
		}
		private void mainMenuItem_DropDownClosed(object sender, EventArgs e)
		{
			this.activeMenu = null;
		}
		private void mainMenuItem_DropDownOpened(object sender, EventArgs e)
		{
			this.activeMenu = sender as ToolStripMenuItem;
		}

		private void UpdateSplitButtonBackupSettings()
		{
			this.checkBackups.Checked = DualityEditorApp.BackupsEnabled;
			this.optionAutosaveDisabled.Checked = DualityEditorApp.Autosaves == AutosaveFrequency.Disabled;
			this.optionAutosaveTenMinutes.Checked = DualityEditorApp.Autosaves == AutosaveFrequency.TenMinutes;
			this.optionAutosaveThirtyMinutes.Checked = DualityEditorApp.Autosaves == AutosaveFrequency.ThirtyMinutes;
			this.optionAutoSaveOneHour.Checked = DualityEditorApp.Autosaves == AutosaveFrequency.OneHour;
		}
		private void UpdateLaunchAppActions()
		{
			bool launcherAvailable = File.Exists(DualityEditorApp.LauncherAppPath);
			this.actionRunApp.Enabled = launcherAvailable;
			this.actionDebugApp.Enabled = launcherAvailable && EditorHelper.IsJITDebuggerAvailable;
			this.menuRunApp.Enabled = launcherAvailable;
			this.menuDebugApp.Enabled = launcherAvailable && EditorHelper.IsJITDebuggerAvailable;
			this.menuProfileApp.Enabled = launcherAvailable;
		}
		private void VerifyStartScene()
		{
			if (DualityApp.AppData.StartScene != null) return;

			// If there is no StartScene defined, attempt to find one automatically.
			if (!Scene.Current.IsRuntimeResource)
			{
				DualityApp.AppData.StartScene = Scene.Current;
				DualityApp.SaveAppData();
			}
			else
			{
				ContentRef<Scene> existingScene = ContentProvider.GetAvailableContent<Scene>().FirstOrDefault();
				if (existingScene != null)
				{
					DualityApp.AppData.StartScene = existingScene;
					DualityApp.SaveAppData();
				}
				else if (!Scene.Current.IsEmpty)
				{
					DualityEditorApp.SaveCurrentScene(false);
					DualityApp.AppData.StartScene = Scene.Current;
					DualityApp.SaveAppData();
				}
			}
		}

		HelpInfo IHelpProvider.ProvideHoverHelp(Point localPos, ref bool captured)
		{
			HelpInfo result = null;
			Point globalPos = this.PointToScreen(localPos);

			// Hovering a menu
			if (this.activeMenu != null)
			{
				ToolStripItem	item		= this.mainMenuStrip.GetItemAtDeep(globalPos);
				object			itemTag		= item != null ? item.Tag : null;
				
				result = itemTag as HelpInfo;
				captured = true;
			}
			// Hovering toolstrip stuff
			else
			{
				ToolStripItem	item		= this.mainToolStrip.GetItemAtDeep(globalPos);
				object			itemTag		= item != null ? item.Tag : null;
				
				result = itemTag as HelpInfo;
				captured = false;
			}

			return result;
		}
	}
}
