using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Linq;
using System.Text.RegularExpressions;

using Duality;
using Duality.IO;
using Duality.Resources;
using Duality.Serialization;
using Duality.Editor.Forms;
using Duality.Editor.AssetManagement;

using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor
{
	public static class FileEventManager
	{
		private static DateTime          lastEventProc           = DateTime.Now;
		private static FileSystemWatcher pluginWatcherWorking    = null;
		private static FileSystemWatcher pluginWatcherExec       = null;
		private static FileSystemWatcher dataDirWatcherFile      = null;
		private static FileSystemWatcher dataDirWatcherDirectory = null;
		private static FileSystemWatcher sourceDirWatcher        = null;
		private static FileSystemWatcher assetsDirWatcher        = null;
		private static HashSet<string>   reimportSchedule        = new HashSet<string>();
		private static HashSet<string>   editorModifiedFiles     = new HashSet<string>();
		private static HashSet<string>   editorModifiedFilesLast = new HashSet<string>();
		private static FileEventQueue    dataDirEventQueue       = new FileEventQueue();
		private static FileEventQueue    sourceDirEventQueue     = new FileEventQueue();
		private static FileEventQueue    pluginDirEventQueue     = new FileEventQueue();


		public static event EventHandler<ResourceFilesChangedEventArgs> ResourcesChanged  = null;
		public static event EventHandler<FileSystemChangedEventArgs>    SourcesChanged    = null;
		public static event EventHandler<FileSystemChangedEventArgs>    PluginsChanged    = null;
		public static event EventHandler<BeginGlobalRenameEventArgs>    BeginGlobalRename = null;
		
		
		/// <summary>
		/// Whether any asset re-imports are pending and will be processed on the 
		/// next <see cref="ProcessPendingReImports"/> call.
		/// </summary>
		public static bool HasPendingReImports
		{
			get { return reimportSchedule.Count > 0; }
		}


		internal static void Init()
		{
			// Set up different file system watchers
			pluginWatcherWorking = new FileSystemWatcher();
			pluginWatcherWorking.SynchronizingObject = DualityEditorApp.MainForm;
			pluginWatcherWorking.EnableRaisingEvents = false;
			pluginWatcherWorking.Filter = "*.dll";
			pluginWatcherWorking.IncludeSubdirectories = true;
			pluginWatcherWorking.NotifyFilter = NotifyFilters.LastWrite;
			pluginWatcherWorking.Path = DualityApp.PluginDirectory;
			pluginWatcherWorking.Changed += fileSystemWatcher_ForwardPlugin;
			pluginWatcherWorking.Created += fileSystemWatcher_ForwardPlugin;
			pluginWatcherWorking.EnableRaisingEvents = true;

			string execPluginDir = Path.Combine(PathHelper.ExecutingAssemblyDir, DualityApp.PluginDirectory);
			bool hasSeparateExecPluginDir = !PathOp.ArePathsEqual(execPluginDir, DualityApp.PluginDirectory);
			if (hasSeparateExecPluginDir && Directory.Exists(execPluginDir))
			{
				pluginWatcherExec = new FileSystemWatcher();
				pluginWatcherExec.SynchronizingObject = DualityEditorApp.MainForm;
				pluginWatcherExec.EnableRaisingEvents = false;
				pluginWatcherExec.Filter = "*.dll";
				pluginWatcherExec.IncludeSubdirectories = true;
				pluginWatcherExec.NotifyFilter = NotifyFilters.LastWrite;
				pluginWatcherExec.Path = execPluginDir;
				pluginWatcherExec.Changed += fileSystemWatcher_ForwardPlugin;
				pluginWatcherExec.Created += fileSystemWatcher_ForwardPlugin;
				pluginWatcherExec.EnableRaisingEvents = true;
			}
			
			dataDirWatcherFile = new FileSystemWatcher();
			dataDirWatcherFile.SynchronizingObject = DualityEditorApp.MainForm;
			dataDirWatcherFile.EnableRaisingEvents = false;
			dataDirWatcherFile.IncludeSubdirectories = true;
			dataDirWatcherFile.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
			dataDirWatcherFile.Path = DualityApp.DataDirectory;
			dataDirWatcherFile.Created += fileSystemWatcher_ForwardData;
			dataDirWatcherFile.Changed += fileSystemWatcher_ForwardData;
			dataDirWatcherFile.Deleted += fileSystemWatcher_ForwardData;
			dataDirWatcherFile.Renamed += fileSystemWatcher_ForwardData;
			dataDirWatcherFile.EnableRaisingEvents = true;

			dataDirWatcherDirectory = new FileSystemWatcher();
			dataDirWatcherDirectory.SynchronizingObject = DualityEditorApp.MainForm;
			dataDirWatcherDirectory.EnableRaisingEvents = false;
			dataDirWatcherDirectory.IncludeSubdirectories = true;
			dataDirWatcherDirectory.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.LastWrite;
			dataDirWatcherDirectory.Path = DualityApp.DataDirectory;
			dataDirWatcherDirectory.Created += fileSystemWatcher_ForwardData;
			dataDirWatcherDirectory.Changed += fileSystemWatcher_ForwardData;
			dataDirWatcherDirectory.Deleted += fileSystemWatcher_ForwardData;
			dataDirWatcherDirectory.Renamed += fileSystemWatcher_ForwardData;
			dataDirWatcherDirectory.EnableRaisingEvents = true;
			
			sourceDirWatcher = new FileSystemWatcher();
			sourceDirWatcher.SynchronizingObject = DualityEditorApp.MainForm;
			sourceDirWatcher.EnableRaisingEvents = false;
			sourceDirWatcher.IncludeSubdirectories = true;
			sourceDirWatcher.Path = EditorHelper.SourceDirectory;
			sourceDirWatcher.Created += fileSystemWatcher_ForwardSource;
			sourceDirWatcher.Changed += fileSystemWatcher_ForwardSource;
			sourceDirWatcher.Deleted += fileSystemWatcher_ForwardSource;
			sourceDirWatcher.Renamed += fileSystemWatcher_ForwardSource;
			sourceDirWatcher.EnableRaisingEvents = true;

			assetsDirWatcher = new FileSystemWatcher();
			assetsDirWatcher.SynchronizingObject = DualityEditorApp.MainForm;
			assetsDirWatcher.EnableRaisingEvents = false;
			assetsDirWatcher.IncludeSubdirectories = true;
			assetsDirWatcher.Path = EditorHelper.ImportDirectory;
			assetsDirWatcher.Created += fileSystemWatcher_ForwardSource;
			assetsDirWatcher.Changed += fileSystemWatcher_ForwardSource;
			assetsDirWatcher.Deleted += fileSystemWatcher_ForwardSource;
			assetsDirWatcher.Renamed += fileSystemWatcher_ForwardSource;
			assetsDirWatcher.EnableRaisingEvents = true;

			// Register events
			DualityEditorApp.EditorIdling += DualityEditorApp_EditorIdling;
			Resource.ResourceSaved += Resource_ResourceSaved;
		}
		internal static void Terminate()
		{
			// Unregister events
			DualityEditorApp.EditorIdling -= DualityEditorApp_EditorIdling;
			Resource.ResourceSaved -= Resource_ResourceSaved;

			// Destroy file system watchers
			pluginWatcherWorking.EnableRaisingEvents = false;
			pluginWatcherWorking.Changed -= fileSystemWatcher_ForwardPlugin;
			pluginWatcherWorking.Created -= fileSystemWatcher_ForwardPlugin;
			pluginWatcherWorking.SynchronizingObject = null;
			pluginWatcherWorking.Dispose();
			pluginWatcherWorking = null;

			dataDirWatcherFile.EnableRaisingEvents = false;
			dataDirWatcherFile.Created -= fileSystemWatcher_ForwardData;
			dataDirWatcherFile.Changed -= fileSystemWatcher_ForwardData;
			dataDirWatcherFile.Deleted -= fileSystemWatcher_ForwardData;
			dataDirWatcherFile.Renamed -= fileSystemWatcher_ForwardData;
			dataDirWatcherFile.SynchronizingObject = null;
			dataDirWatcherFile.Dispose();
			dataDirWatcherFile = null;

			dataDirWatcherDirectory.EnableRaisingEvents = false;
			dataDirWatcherDirectory.Created -= fileSystemWatcher_ForwardData;
			dataDirWatcherDirectory.Changed -= fileSystemWatcher_ForwardData;
			dataDirWatcherDirectory.Deleted -= fileSystemWatcher_ForwardData;
			dataDirWatcherDirectory.Renamed -= fileSystemWatcher_ForwardData;
			dataDirWatcherDirectory.SynchronizingObject = null;
			dataDirWatcherDirectory.Dispose();
			dataDirWatcherDirectory = null;

			sourceDirWatcher.EnableRaisingEvents = false;
			sourceDirWatcher.Created -= fileSystemWatcher_ForwardSource;
			sourceDirWatcher.Changed -= fileSystemWatcher_ForwardSource;
			sourceDirWatcher.Deleted -= fileSystemWatcher_ForwardSource;
			sourceDirWatcher.Renamed -= fileSystemWatcher_ForwardSource;
			sourceDirWatcher.SynchronizingObject = null;
			sourceDirWatcher.Dispose();
			sourceDirWatcher = null;
		}

		/// <summary>
		/// Immediately processes any pending file system events and triggers their public events.
		/// This is usually done automatically by the editor and doesn't need to be called explicitly.
		/// </summary>
		public static void ProcessEvents()
		{
			ProcessSourceDirEvents(sourceDirEventQueue);
			ProcessDataDirEvents(dataDirEventQueue);
			ProcessPluginDirEvents(pluginDirEventQueue);

			// Manage the list of editor-modified files to be ignored in a 
			// two-pass process, so event order doesn't matter.
			{
				// Remove the ones that were known last time
				foreach (string file in editorModifiedFilesLast)
					editorModifiedFiles.Remove(file);

				// Mind the ones that are known right now
				editorModifiedFilesLast.Clear();
				foreach (string file in editorModifiedFiles)
					editorModifiedFilesLast.Add(file);
			}
		}
		/// <summary>
		/// Processes any pending asset re-import operations that were scheduled due to source file changes.
		/// Will do nothing when <see cref="HasPendingReImports"/> is false.
		/// </summary>
		public static void ProcessPendingReImports()
		{
			if (reimportSchedule.Count == 0) return;

			// Gather valid scheduled source files
			string[] existingReImportFiles = reimportSchedule
				.Where(path => File.Exists(path))
				.ToArray();
			reimportSchedule.Clear();

			// Perform scheduled source file reimports
			AssetManager.ReImportAssets(existingReImportFiles);
		}

		/// <summary>
		/// Flags the specified path as recently modified by an editor operation and 
		/// thus to be ignored when detecing file system changes.
		/// </summary>
		/// <param name="path"></param>
		public static void FlagPathEditorModified(string path)
		{
			if (string.IsNullOrEmpty(path)) return; // Ignore bad paths
			string fullPath = Path.GetFullPath(path);
			editorModifiedFiles.Add(fullPath);
			editorModifiedFilesLast.Remove(fullPath);
		}
		/// <summary>
		/// Returns whether the specified path is currently flagged as modified by the editor and 
		/// thus to be ignored when detecing file system changes.
		/// </summary>
		/// <param name="path"></param>
		public static bool IsPathEditorModified(string path)
		{
			return editorModifiedFiles.Contains(Path.GetFullPath(path));
		}

		private static bool EditorDataEventFilter(FileEvent fileEvent)
		{
			// Filter out changes made by the editor itself
			if (fileEvent.Type == FileEventType.Changed && IsPathEditorModified(fileEvent.Path))
				return true;

			// Skip everything that isn't either a Resource or a directory
			if (!Resource.IsResourceFile(fileEvent.Path) && !fileEvent.IsDirectory)
				return true;

			return false;
		}
		private static bool EditorSourceEventFilter(FileEvent fileEvent)
		{
			// Filter out changes made by the editor itself
			if (fileEvent.Type == FileEventType.Changed && IsPathEditorModified(fileEvent.Path))
				return true;

			return false;
		}
		private static bool EditorPluginEventFilter(FileEvent fileEvent)
		{
			// Filter out class libraries that clearly aren't plugins
			if (!fileEvent.Path.EndsWith(".core.dll", StringComparison.InvariantCultureIgnoreCase) &&
				!fileEvent.Path.EndsWith(".editor.dll", StringComparison.InvariantCultureIgnoreCase))
				return true;

			return false;
		}

		private static FileEvent TranslateFileEvent(FileSystemEventArgs watcherEvent, bool isDirectory)
		{
			FileEvent fileEvent;
			fileEvent.Path = watcherEvent.FullPath;
			fileEvent.IsDirectory = isDirectory;
			fileEvent.Type = FileEventType.Changed;

			switch (watcherEvent.ChangeType)
			{
				case WatcherChangeTypes.Created: fileEvent.Type = FileEventType.Created; break;
				case WatcherChangeTypes.Deleted: fileEvent.Type = FileEventType.Deleted; break;
				case WatcherChangeTypes.Changed: fileEvent.Type = FileEventType.Changed; break;
				case WatcherChangeTypes.Renamed: fileEvent.Type = FileEventType.Renamed; break;
			}

			if (watcherEvent is RenamedEventArgs)
				fileEvent.OldPath = ((RenamedEventArgs)watcherEvent).OldFullPath;
			else
				fileEvent.OldPath = fileEvent.Path;

			return fileEvent;
		}
		private static void PushFileEvent(FileEventQueue queue, FileSystemEventArgs watcherEvent, bool isDirectory)
		{
			// Do not track hidden paths
			if (!PathHelper.IsPathVisible(watcherEvent.FullPath)) return;

			// Translate the file system watcher event into our own data structure
			FileEvent fileEvent = TranslateFileEvent(
				watcherEvent,
				isDirectory);

			queue.Add(fileEvent);
		}

		private static void ProcessDataDirEvents(FileEventQueue eventQueue)
		{
			// Filter out events we don't want to process in the editor
			eventQueue.ApplyFilter(EditorDataEventFilter);
			if (eventQueue.IsEmpty) return;

			// System internal event processing / do all the low-level stuff
			HandleDataDirEvents(eventQueue);

			// Fire an editor-wide event to allow plugins and editor modules to react
			if (ResourcesChanged != null)
				ResourcesChanged(null, new ResourceFilesChangedEventArgs(eventQueue));

			// Handled all events, start over with an empty buffer
			eventQueue.Clear();
		}
		private static void HandleDataDirEvents(FileEventQueue eventQueue)
		{
			// Gather a list of all externally modified resources that are currently loaded.
			// We'll use this to later tell the editor about the changes we've applied.
			List<Resource> modifiedLoadedResources = null;
			foreach (FileEvent fileEvent in eventQueue.Items)
			{
				if (fileEvent.IsDirectory) continue;

				// We're only interested in the path where we would find the Resource right now,
				// because we need the instance for the change notification
				string contentPath = (fileEvent.Type == FileEventType.Renamed) ? fileEvent.OldPath : fileEvent.Path;
				ContentRef<Resource> content = new ContentRef<Resource>(contentPath);

				// Some editor modules rely on change notifications to re-establish links to previously
				// removed Resources. In order to do that, we'll speculatively load newly arrived Resources
				// so we can put out a change notification for them.
				// Note: If ObjectSelection supported ContentRefs, we could improve and do that without the
				// speculative load, triggering a load only when someone was actually interested.
				if (content.IsLoaded || fileEvent.Type == FileEventType.Created)
				{
					if (modifiedLoadedResources == null)
						modifiedLoadedResources = new List<Resource>();
					modifiedLoadedResources.Add(content.Res);
				}
			}

			// Handle each event according to its type
			List<FileEvent> renameEventBuffer = null;
			HashSet<string> sourceMediaDeleteSchedule = null;
			foreach (FileEvent fileEvent in eventQueue.Items)
			{
				if (fileEvent.Type == FileEventType.Changed)
				{
					HandleDataDirChangeEvent(fileEvent);
				}
				else if (fileEvent.Type == FileEventType.Deleted)
				{
					HandleDataDirDeleteEvent(fileEvent, ref sourceMediaDeleteSchedule);
				}
				else if (fileEvent.Type == FileEventType.Renamed)
				{
					HandleDataDirRenameEvent(fileEvent, ref renameEventBuffer);
				}
			}

			// If we scheduled source / media files for deletion, do it now at once
			if (sourceMediaDeleteSchedule != null)
			{
				// Gather a list of directories from which we're removing
				HashSet<string> affectedDirectories = new HashSet<string>();
				foreach (string file in sourceMediaDeleteSchedule)
				{
					affectedDirectories.Add(Path.GetDirectoryName(file));
				}

				// Send all the files to the recycle bin
				RecycleBin.SendSilent(sourceMediaDeleteSchedule);

				// Remove directories that are now empty
				foreach (string dir in affectedDirectories)
				{
					PathHelper.DeleteEmptyDirectory(dir, true);
				}
			}

			// If required, perform a global rename operation in all existing content
			if (renameEventBuffer != null)
			{
				// Don't do it now - schedule it for the main form event loop so we don't block here.
				DualityEditorApp.MainForm.BeginInvoke((Action)delegate () {
					ProcessingBigTaskDialog taskDialog = new ProcessingBigTaskDialog(
						Properties.GeneralRes.TaskRenameContentRefs_Caption,
						Properties.GeneralRes.TaskRenameContentRefs_Desc,
						async_RenameContentRefs, renameEventBuffer);
					taskDialog.ShowDialog(DualityEditorApp.MainForm);
				});
			}

			// Notify the editor about externally modified resources
			if (modifiedLoadedResources != null)
			{
				DualityEditorApp.NotifyObjPropChanged(
					null,
					new ObjectSelection(modifiedLoadedResources),
					false);
			}
		}
		private static void HandleDataDirChangeEvent(FileEvent fileEvent)
		{
			// Unregister outdated resources when modified outside the editor
			if (!fileEvent.IsDirectory && ContentProvider.HasContent(fileEvent.Path))
			{
				ContentRef<Resource> resRef = new ContentRef<Resource>(null, fileEvent.Path);
				bool isCurrentScene = resRef.Is<Scene>() && Scene.Current == resRef.Res;
				if (isCurrentScene || DualityEditorApp.IsResourceUnsaved(fileEvent.Path))
				{
					DialogResult result = MessageBox.Show(
						string.Format(Properties.GeneralRes.Msg_ConfirmReloadResource_Text, fileEvent.Path),
						Properties.GeneralRes.Msg_ConfirmReloadResource_Caption,
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Exclamation);
					if (result == DialogResult.Yes)
					{
						string curScenePath = Scene.CurrentPath;
						ContentProvider.RemoveContent(fileEvent.Path);
						if (isCurrentScene) Scene.SwitchTo(ContentProvider.RequestContent<Scene>(curScenePath), true);
					}
				}
				else
				{
					ContentProvider.RemoveContent(fileEvent.Path);
				}
			}
		}
		private static void HandleDataDirDeleteEvent(FileEvent fileEvent, ref HashSet<string> sourceMediaDeleteSchedule)
		{
			// Schedule Source/Media file deletion to keep it organized / synced with Resource Data
			if (sourceMediaDeleteSchedule == null)
				sourceMediaDeleteSchedule = new HashSet<string>();
			GetDeleteSourceMediaFilePaths(fileEvent, sourceMediaDeleteSchedule);

			// Unregister no-longer existing resources
			if (fileEvent.IsDirectory)
				ContentProvider.RemoveContentTree(fileEvent.Path);
			else
				ContentProvider.RemoveContent(fileEvent.Path);
		}
		private static void HandleDataDirRenameEvent(FileEvent fileEvent, ref List<FileEvent> renameEventBuffer)
		{
			// Determine which Source / Media files would belong to this Resource - before moving it
			string[] oldMediaPaths = PreMoveSourceMediaFile(fileEvent);

			// Rename registered content
			if (fileEvent.IsDirectory)
				ContentProvider.RenameContentTree(fileEvent.OldPath, fileEvent.Path);
			else
				ContentProvider.RenameContent(fileEvent.OldPath, fileEvent.Path);

			// Query skipped paths
			bool isEmptyDir = fileEvent.IsDirectory && !Directory.EnumerateFileSystemEntries(fileEvent.Path).Any();
			bool isSkippedPath = isEmptyDir;
			if (!isSkippedPath && BeginGlobalRename != null)
			{
				BeginGlobalRenameEventArgs beginGlobalRenameArgs = new BeginGlobalRenameEventArgs(
					fileEvent.Path,
					fileEvent.OldPath,
					fileEvent.IsDirectory);
				BeginGlobalRename(null, beginGlobalRenameArgs);
				isSkippedPath = beginGlobalRenameArgs.IsCancelled;
			}

			if (!isSkippedPath)
			{
				// Buffer rename event to perform the global rename for all at once.
				if (renameEventBuffer == null)
					renameEventBuffer = new List<FileEvent>();
				renameEventBuffer.Add(fileEvent);
			}

			if (!isSkippedPath)
			{
				// Organize the Source/Media directory accordingly
				MoveSourceMediaFile(fileEvent, oldMediaPaths);
			}
		}

		private static void ProcessSourceDirEvents(FileEventQueue eventQueue)
		{
			// Filter out events we don't want to process in the editor
			eventQueue.ApplyFilter(EditorSourceEventFilter);
			if (eventQueue.IsEmpty) return;

			// Process events
			foreach (FileEvent fileEvent in eventQueue.Items)
			{
				// Mind modified source files for re-import
				if (fileEvent.Type == FileEventType.Changed)
				{
					if (File.Exists(fileEvent.Path) && PathOp.IsPathLocatedIn(fileEvent.Path, EditorHelper.ImportDirectory)) 
						reimportSchedule.Add(fileEvent.Path);
				}
			}

			// Fire an editor-wide event to allow reacting to source changes and re-importing resources
			if (SourcesChanged != null)
				SourcesChanged(null, new FileSystemChangedEventArgs(eventQueue));

			// Handled all events, start over with an empty buffer
			eventQueue.Clear();
		}
		private static void ProcessPluginDirEvents(FileEventQueue eventQueue)
		{
			// Filter out events we don't want to process in the editor
			eventQueue.ApplyFilter(EditorPluginEventFilter);
			if (eventQueue.IsEmpty) return;

			// Fire an editor-wide event to allow scheduling a plugin reload
			if (PluginsChanged != null)
				PluginsChanged(null, new FileSystemChangedEventArgs(eventQueue));

			// Handled all events, start over with an empty buffer
			eventQueue.Clear();
		}

		private static void GetDeleteSourceMediaFilePaths(FileEvent deleteEvent, ICollection<string> deletePathSchedule)
		{
			if (!deleteEvent.IsDirectory)
			{
				IList<string> mediaPaths = AssetManager.GetAssetSourceFiles(new ContentRef<Resource>(deleteEvent.Path));
				for (int i = 0; i < mediaPaths.Count; i++)
				{
					if (File.Exists(mediaPaths[i]))
					{
						deletePathSchedule.Add(mediaPaths[i]);
					}
				}
			}
			else
			{
				string mediaPath = Path.Combine(
					EditorHelper.ImportDirectory, 
					PathHelper.MakeFilePathRelative(deleteEvent.Path, DualityApp.DataDirectory));
				if (Directory.Exists(mediaPath))
				{
					deletePathSchedule.Add(mediaPath);
				}
			}
		}
		private static string[] PreMoveSourceMediaFile(FileEvent renameEvent)
		{
			if (renameEvent.IsDirectory)
				return new string[0];
			else
				return AssetManager.GetAssetSourceFiles(new ContentRef<Resource>(renameEvent.OldPath));
		}
		private static void MoveSourceMediaFile(FileEvent renameEvent, string[] oldMediaPaths)
		{
			if (!renameEvent.IsDirectory)
			{
				string[] newMediaPaths = AssetManager.GetAssetSourceFiles(new ContentRef<Resource>(renameEvent.Path));
				for (int i = 0; i < oldMediaPaths.Length; i++)
				{
					string oldPath = oldMediaPaths[i];
					string newPath = newMediaPaths.Length > i ? newMediaPaths[i] : oldPath;

					// Move the media file to mirror the data files movement
					if (!PathOp.ArePathsEqual(oldPath, newPath))
					{
						if (File.Exists(oldPath) && !File.Exists(newPath))
						{
							Directory.CreateDirectory(Path.GetDirectoryName(newPath));
							try
							{
								File.Move(oldPath, newPath);
							}
							catch (IOException exception)
							{
								Logs.Editor.WriteWarning(
									"Unable to move source media file '{0}' to '{1}' ({2}). Copying the file instead.",
									oldPath,
									newPath,
									exception.Message);
								File.Copy(oldPath, newPath);
							}
							PathHelper.DeleteEmptyDirectory(Path.GetDirectoryName(oldPath), true);
						}
					}
				}
			}
			else
			{
				// Determine which source/media directory we're going to move
				string oldMediaPath = Path.Combine(
					EditorHelper.ImportDirectory, 
					PathHelper.MakeFilePathRelative(renameEvent.OldPath, DualityApp.DataDirectory));

				// Determine where that old source/media directory needs to be moved
				string newMediaPath = Path.Combine(
					EditorHelper.ImportDirectory, 
					PathHelper.MakeFilePathRelative(renameEvent.Path, DualityApp.DataDirectory));

				// Move the media directory to mirror the data directories movement
				if (!PathOp.ArePathsEqual(newMediaPath, oldMediaPath))
				{
					if (Directory.Exists(oldMediaPath) && !Directory.Exists(newMediaPath))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(newMediaPath));
						try
						{
							Directory.Move(oldMediaPath, newMediaPath);
						}
						catch (IOException exception)
						{
							Logs.Editor.WriteWarning(
								"Unable to move source media directory '{0}' to '{1}' ({2}). Copying the directory instead.",
								oldMediaPath,
								newMediaPath,
								exception.Message);
							PathHelper.CopyDirectory(oldMediaPath, newMediaPath);
						}
						PathHelper.DeleteEmptyDirectory(Path.GetDirectoryName(oldMediaPath), true);
					}
				}
			}
		}

		private static void DualityEditorApp_EditorIdling(object sender, EventArgs e)
		{
			// Process file / source events regularily, if no modal dialog is open.
			if ((DateTime.Now - lastEventProc).TotalMilliseconds > 100.0d)
			{
				ProcessEvents();
				lastEventProc = DateTime.Now;
			}
		}
		private static void Resource_ResourceSaved(object sender, ResourceSaveEventArgs e)
		{
			FlagPathEditorModified(e.SaveAsPath);
		}
		
		private static void fileSystemWatcher_ForwardData(object sender, FileSystemEventArgs e)
		{
			PushFileEvent(dataDirEventQueue, e, sender == dataDirWatcherDirectory);
		}
		private static void fileSystemWatcher_ForwardSource(object sender, FileSystemEventArgs e)
		{
			PushFileEvent(sourceDirEventQueue, e, Directory.Exists(e.FullPath));
		}
		private static void fileSystemWatcher_ForwardPlugin(object sender, FileSystemEventArgs e)
		{
			PushFileEvent(pluginDirEventQueue, e, Directory.Exists(e.FullPath));
		}

		private static System.Collections.IEnumerable async_RenameContentRefs(ProcessingBigTaskDialog.WorkerInterface state)
		{
			List<FileEvent> renameData = (List<FileEvent>)state.Data;
			int totalCounter = 0;
			int fileCounter = 0;
			
			// Rename in static application data
			state.StateDesc = "DualityApp Data"; yield return null;
			DualityApp.LoadAppData();
			DualityApp.LoadUserData();
			state.Progress += 0.04f; yield return null;

			totalCounter += async_RenameContentRefs_Perform(DualityApp.AppData, renameData);
			totalCounter += async_RenameContentRefs_Perform(DualityApp.UserData, renameData);
			state.Progress += 0.02f; yield return null;

			DualityApp.SaveAppData();
			DualityApp.SaveUserData();
			state.Progress += 0.04f; yield return null;

			// Special case: Current Scene in sandbox mode
			if (Sandbox.IsActive)
			{
				// Because changes we'll do will be discarded when leaving the sandbox we'll need to
				// do it the hard way - manually load an save the file.
				state.StateDesc = "Current Scene"; yield return null;
				Scene curScene = Resource.Load<Scene>(Scene.CurrentPath, null, false);
				fileCounter = async_RenameContentRefs_Perform(curScene, renameData);
				totalCounter += fileCounter;
				if (fileCounter > 0) curScene.Save(Scene.CurrentPath, false);
			}
			// Special case: Current Scene NOT in sandbox mode, but still unsaved
			else if (Scene.Current.IsRuntimeResource)
			{
				state.StateDesc = "Current Scene"; yield return null;
				fileCounter = async_RenameContentRefs_Perform(Scene.Current, renameData);
				if (fileCounter > 0)
					DualityEditorApp.NotifyObjPropChanged(null, new ObjectSelection(Scene.Current.AllObjects));
				totalCounter += fileCounter;
			}

			// Rename in actual content
			Type[] targetResTypes = renameData.Any(e => e.IsDirectory) ? null : renameData.Select(e => new ContentRef<Resource>(e.Path).ResType).ToArray();
			List<ContentRef<Resource>> loadedContent = ContentProvider.GetLoadedContent<Resource>();
			List<IContentRef> reloadContent = new List<IContentRef>();
			string[] resFiles = Resource.GetResourceFiles().ToArray();
			List<Resource> modifiedRes = new List<Resource>();
			foreach (string file in resFiles)
			{
				// Early-out, if this kind of Resource isn't able to reference the renamed Resource
				if (targetResTypes != null)
				{
					Type resType = Resource.GetTypeByFileName(file);

					if (resType == null)
					{
						Logs.Editor.WriteWarning("Could not determine Resource type for File '{0}' using file name only. Skipping it during rename.", file);
						continue;
					}

					bool canReferenceRes = false;
					foreach (Type targetType in targetResTypes)
					{
						if (ReflectionHelper.CanReferenceResource(resType, targetType))
						{
							canReferenceRes = true;
							break;
						}
					}
					if (!canReferenceRes)
					{
						state.Progress += 0.9f / resFiles.Length;
						continue;
					}
				}

				// Set displayed name
				state.StateDesc = file; yield return null;

				// Wasn't loaded before? Unload it later to keep the memory footprint small.
				bool wasLoaded = loadedContent.Any(r => r.Path == file);
				// Keep in mind that this operation is performed while Duality content was
				// in an inconsistent state. Loading Resources now may lead to wrong data.
				// Because the ContentRefs might be wrong right now.

				if (wasLoaded)
				{
					// Retrieve already loaded content
					IContentRef cr = ContentProvider.RequestContent(file);
					state.Progress += 0.45f / resFiles.Length; yield return null;

					// Perform rename and flag unsaved / modified
					fileCounter = async_RenameContentRefs_Perform(cr.Res, renameData);
					if (fileCounter > 0) modifiedRes.Add(cr.Res);
				}
				else
				{
					// Load content without initializing it
					Resource res = Resource.Load<Resource>(file, null, false);
					state.Progress += 0.45f / resFiles.Length; yield return null;

					// Perform rename and save it without making it globally available
					fileCounter = async_RenameContentRefs_Perform(res, renameData);
					if (fileCounter > 0) res.Save(null, false);
				}

				totalCounter += fileCounter;
				state.Progress += 0.45f / resFiles.Length; yield return null;
			}

			// Notify the editor about modified Resources
			if (modifiedRes.Count > 0)
			{
				DualityEditorApp.NotifyObjPropChanged(null, new ObjectSelection(modifiedRes));
			}
		}
		private static int async_RenameContentRefs_Perform(object obj, List<FileEvent> args)
		{
			int counter = 0;
			ReflectionHelper.VisitObjectsDeep<IContentRef>(obj, r => 
			{
				if (r.IsDefaultContent) return r;
				if (r.IsExplicitNull) return r;
				if (string.IsNullOrEmpty(r.Path)) return r;

				foreach (FileEvent e in args)
				{
					if (!e.IsDirectory && r.Path == e.OldPath)
					{
						r.Path = e.Path;
						counter++;
						break;
					}
					else if (e.IsDirectory && PathOp.IsPathLocatedIn(r.Path, e.OldPath))
					{
						r.Path = e.Path + r.Path.Remove(0, e.OldPath.Length);
						counter++;
						break;
					}
				}
				return r; 
			});
			return counter;
		}
	}
}
