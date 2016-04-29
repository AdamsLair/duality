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
		private class DeletedEventArgsExt : FileSystemEventArgs
		{
			private bool isDirectory = false;
			public bool IsDirectory
			{
				get { return this.isDirectory; }
			}
			public bool IsFile
			{
				get { return !this.isDirectory; }
			}
			public DeletedEventArgsExt(WatcherChangeTypes changeTypes, string directory, string name, bool isDir) : base(changeTypes, directory, name)
			{
				this.isDirectory = isDir;
			}
		}

		private	static DateTime						lastEventProc			= DateTime.Now;
		private static FileSystemWatcher			pluginWatcherWorking	= null;
		private static FileSystemWatcher			pluginWatcherExec		= null;
		private static FileSystemWatcher			dataDirWatcherFile		= null;
		private static FileSystemWatcher			dataDirWatcherDirectory	= null;
		private static FileSystemWatcher			sourceDirWatcher		= null;
		private	static HashSet<string>				reimportSchedule		= new HashSet<string>();
		private	static HashSet<string>				editorModifiedFiles		= new HashSet<string>();
		private	static HashSet<string>				editorModifiedFilesLast	= new HashSet<string>();
		private	static List<FileSystemEventArgs>	dataDirEventBuffer		= new List<FileSystemEventArgs>();
		private	static List<FileSystemEventArgs>	sourceDirEventBuffer	= new List<FileSystemEventArgs>();


		public	static	event	EventHandler<ResourceEventArgs>				ResourceCreated		= null;
		public	static	event	EventHandler<ResourceEventArgs>				ResourceDeleted		= null;
		public	static	event	EventHandler<ResourceEventArgs>				ResourceModified	= null;
		public	static	event	EventHandler<ResourceRenamedEventArgs>		ResourceRenamed		= null;
		public	static	event	EventHandler<FileSystemEventArgs>			PluginChanged		= null;
		public	static	event	EventHandler<BeginGlobalRenameEventArgs>	BeginGlobalRename	= null;
		
		
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
			pluginWatcherWorking.Changed += corePluginWatcher_Changed;
			pluginWatcherWorking.Created += corePluginWatcher_Changed;
			pluginWatcherWorking.EnableRaisingEvents = true;

			string execPluginDir = Path.Combine(PathHelper.ExecutingAssemblyDir, DualityApp.PluginDirectory);
			if (Path.GetFullPath(execPluginDir).ToLower() != Path.GetFullPath(DualityApp.PluginDirectory).ToLower() && Directory.Exists(execPluginDir))
			{
				pluginWatcherExec = new FileSystemWatcher();
				pluginWatcherExec.SynchronizingObject = DualityEditorApp.MainForm;
				pluginWatcherExec.EnableRaisingEvents = false;
				pluginWatcherExec.Filter = "*.dll";
				pluginWatcherExec.IncludeSubdirectories = true;
				pluginWatcherExec.NotifyFilter = NotifyFilters.LastWrite;
				pluginWatcherExec.Path = execPluginDir;
				pluginWatcherExec.Changed += corePluginWatcher_Changed;
				pluginWatcherExec.Created += corePluginWatcher_Changed;
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

			// Register events
			DualityEditorApp.MainForm.Activated += mainForm_Activated;
			DualityEditorApp.EditorIdling += DualityEditorApp_EditorIdling;
			Resource.ResourceSaved += Resource_ResourceSaved;
		}
		internal static void Terminate()
		{
			// Unregister events
			DualityEditorApp.MainForm.Activated -= mainForm_Activated;
			DualityEditorApp.EditorIdling -= DualityEditorApp_EditorIdling;
			Resource.ResourceSaved -= Resource_ResourceSaved;

			// Destroy file system watchers
			pluginWatcherWorking.EnableRaisingEvents = false;
			pluginWatcherWorking.Changed -= corePluginWatcher_Changed;
			pluginWatcherWorking.Created -= corePluginWatcher_Changed;
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


		private static bool IsResPathIgnored(string filePath)
		{
			return IsPathIgnored(filePath);
		}
		private static bool IsSourcePathIgnored(string filePath)
		{
			return IsPathIgnored(filePath);
		}
		private static bool IsPathIgnored(string filePath)
		{
			if (!File.Exists(filePath) && !Directory.Exists(filePath)) return false;
			if (!PathHelper.IsPathVisible(filePath)) return true;
			if (filePath.Contains(@"/.svn/") || filePath.Contains(@"\.svn\")) return true;
			return false;
		}
		
		private static FileSystemEventArgs FetchFileSystemEvent(List<FileSystemEventArgs> dirEventList, string basePath)
		{
			if (dirEventList.Count == 0) return null;

			FileSystemEventArgs	current	= dirEventList[0];
			dirEventList.RemoveAt(0);

			// Discard or pack rename-rename
			if (current.ChangeType == WatcherChangeTypes.Renamed)
			{
				RenamedEventArgs rename = current as RenamedEventArgs;

				while (rename != null)
				{
					RenamedEventArgs renameB = dirEventList.OfType<RenamedEventArgs>().FirstOrDefault(e => 
						Path.GetFileName(e.OldFullPath) == Path.GetFileName(rename.FullPath));
					if (renameB != null)
					{
						dirEventList.Remove(renameB);
						rename = new RenamedEventArgs(WatcherChangeTypes.Renamed, basePath, renameB.Name, rename.OldName);
						current = rename;
					}
					else break;
				}

				// Discard useless renames
				if (rename.OldFullPath == rename.FullPath) return null;
			}

			// Pack del-rename to change
			if (current.ChangeType == WatcherChangeTypes.Deleted)
			{
				FileSystemEventArgs del		= current;
				RenamedEventArgs	rename	= null;
				
				rename = dirEventList.OfType<RenamedEventArgs>().FirstOrDefault(e => e.FullPath == del.FullPath);
				dirEventList.Remove(rename);

				if (del != null && rename != null) return new FileSystemEventArgs(WatcherChangeTypes.Changed, basePath, del.Name);
			}

			// Pack del-create to rename
			if (current.ChangeType == WatcherChangeTypes.Deleted)
			{
				FileSystemEventArgs del		= current;
				FileSystemEventArgs create	= null;

				create = dirEventList.FirstOrDefault(e => 
					e.ChangeType == WatcherChangeTypes.Created && 
					Path.GetFileName(e.FullPath) == Path.GetFileName(del.FullPath));
				dirEventList.Remove(create);

				if (del != null && create != null) return new RenamedEventArgs(WatcherChangeTypes.Renamed, basePath, create.Name, del.Name);
			}
			
			return current;
		}
		private static void PushDataDirEvent(FileSystemEventArgs e, bool isDirectory)
		{
			if (IsResPathIgnored(e.FullPath)) return;

			// In case we're dealing with a deletion, we'll need to add some meta information to know whether it was a file or directory.
			if (e.ChangeType == WatcherChangeTypes.Deleted)
			{
				string baseDir = (e.Name.Length > 0) ? e.FullPath.Remove(e.FullPath.Length - e.Name.Length, e.Name.Length) : "";
				e = new DeletedEventArgsExt(e.ChangeType, baseDir, e.Name, isDirectory);
			}

			dataDirEventBuffer.RemoveAll(f => f.FullPath == e.FullPath && f.ChangeType == e.ChangeType);
			dataDirEventBuffer.Add(e);
		}
		private static void ProcessDataDirEvents()
		{
			List<ResourceRenamedEventArgs> renameEventBuffer = null;

			// Process events
			while (dataDirEventBuffer.Count > 0)
			{
				FileSystemEventArgs e = FetchFileSystemEvent(dataDirEventBuffer, DualityApp.DataDirectory);
				if (e == null) continue;

				// Determine whether we're dealing with a directory
				bool isDirectory = Directory.Exists(e.FullPath);
				{
					// If this is a deletion, nothing exists anymore, rely on metadata instead.
					DeletedEventArgsExt de = e as DeletedEventArgsExt;
					if (de != null && de.IsDirectory)
						isDirectory = true;
				}

				if (e.ChangeType == WatcherChangeTypes.Changed)
				{
					// Ignore stuff saved by the editor itself
					if (IsPathEditorModified(e.FullPath))
						continue;

					if (Resource.IsResourceFile(e.FullPath) || isDirectory)
					{
						ContentRef<Resource> resRef = new ContentRef<Resource>(null, e.FullPath);

						// Unregister outdated resources, if modified outside the editor
						if (!isDirectory && ContentProvider.HasContent(e.FullPath))
						{
							bool isCurrentScene = resRef.Is<Scene>() && Scene.Current == resRef.Res;
							if (isCurrentScene || DualityEditorApp.IsResourceUnsaved(e.FullPath))
							{
								DialogResult result = MessageBox.Show(
									String.Format(Properties.GeneralRes.Msg_ConfirmReloadResource_Text, e.FullPath), 
									Properties.GeneralRes.Msg_ConfirmReloadResource_Caption, 
									MessageBoxButtons.YesNo,
									MessageBoxIcon.Exclamation);
								if (result == DialogResult.Yes)
								{
									string curScenePath = Scene.CurrentPath;
									ContentProvider.RemoveContent(e.FullPath);
									if (isCurrentScene) Scene.SwitchTo(ContentProvider.RequestContent<Scene>(curScenePath), true);
								}
							}
							else
								ContentProvider.RemoveContent(e.FullPath);
						}

						if (ResourceModified != null)
							ResourceModified(null, new ResourceEventArgs(e.FullPath, isDirectory));
					}
				}
				else if (e.ChangeType == WatcherChangeTypes.Created)
				{
					if (File.Exists(e.FullPath))
					{
						// Register newly detected Resource file
						if (Resource.IsResourceFile(e.FullPath))
						{
							if (ResourceCreated != null)
								ResourceCreated(null, new ResourceEventArgs(e.FullPath, false));
						}
					}
					else if (Directory.Exists(e.FullPath))
					{
						// Register newly detected Resource directory
						if (ResourceCreated != null)
							ResourceCreated(null, new ResourceEventArgs(e.FullPath, true));
					}
				}
				else if (e.ChangeType == WatcherChangeTypes.Deleted)
				{
					// Is it a Resource file or just something else?
					if (Resource.IsResourceFile(e.FullPath) || isDirectory)
					{
						ResourceEventArgs args = new ResourceEventArgs(e.FullPath, isDirectory);

						// Organize the Source/Media directory accordingly
						DeleteSourceMediaFile(args);

						// Unregister no-more existing resources
						if (isDirectory)	ContentProvider.RemoveContentTree(args.Path);
						else				ContentProvider.RemoveContent(args.Path);

						if (ResourceDeleted != null)
							ResourceDeleted(null, args);
					}
				}
				else if (e.ChangeType == WatcherChangeTypes.Renamed)
				{
					// Is it a Resource file or just something else?
					RenamedEventArgs re = e as RenamedEventArgs;
					ResourceRenamedEventArgs args = new ResourceRenamedEventArgs(re.FullPath, re.OldFullPath, isDirectory);
					if (Resource.IsResourceFile(e.FullPath) || isDirectory)
					{
						// Determine which Source / Media files would belong to this Resource - before moving it
						string[] oldMediaPaths = PreMoveSourceMediaFile(args);;

						// Rename content registerations
						if (isDirectory)	ContentProvider.RenameContentTree(args.OldPath, args.Path);
						else				ContentProvider.RenameContent(args.OldPath, args.Path);

						// Query skipped paths
						bool isEmptyDir = isDirectory && !Directory.EnumerateFileSystemEntries(args.Path).Any();
						bool isSkippedPath = isEmptyDir;
						if (!isSkippedPath && BeginGlobalRename != null)
						{
							BeginGlobalRenameEventArgs beginGlobalRenameArgs = new BeginGlobalRenameEventArgs(args.Path, args.OldPath, isDirectory);
							BeginGlobalRename(null, beginGlobalRenameArgs);
							isSkippedPath = beginGlobalRenameArgs.Cancel;
						}

						if (!isSkippedPath)
						{
							// Buffer rename event to perform the global rename for all at once.
							if (renameEventBuffer == null) renameEventBuffer = new List<ResourceRenamedEventArgs>();
							renameEventBuffer.Add(args);
						}

						if (ResourceRenamed != null)
							ResourceRenamed(null, args);

						if (!isSkippedPath)
						{
							// Organize the Source/Media directory accordingly
							MoveSourceMediaFile(args, oldMediaPaths);
						}
					}
				}
			}

			// If required, perform a global rename operation in all existing content
			if (renameEventBuffer != null)
			{
				// Don't do it now - schedule it for the main form event loop so we don't block here.
				DualityEditorApp.MainForm.BeginInvoke((Action)delegate() {
					ProcessingBigTaskDialog taskDialog = new ProcessingBigTaskDialog( 
						Properties.GeneralRes.TaskRenameContentRefs_Caption, 
						Properties.GeneralRes.TaskRenameContentRefs_Desc, 
						async_RenameContentRefs, renameEventBuffer);
					taskDialog.ShowDialog(DualityEditorApp.MainForm);
				});
			}
		}
		private static void PushSourceDirEvent(FileSystemEventArgs e)
		{
			if (IsSourcePathIgnored(e.FullPath)) return;
			sourceDirEventBuffer.RemoveAll(f => f.FullPath == e.FullPath && f.ChangeType == e.ChangeType);
			sourceDirEventBuffer.Add(e);
		}
		private static void ProcessSourceDirEvents()
		{
			// Process events
			while (sourceDirEventBuffer.Count > 0)
			{
				FileSystemEventArgs e = FetchFileSystemEvent(sourceDirEventBuffer, sourceDirWatcher.Path);
				if (e == null) continue;

				// Mind modified source files for re-import
				if (e.ChangeType == WatcherChangeTypes.Changed)
				{
					// Ignore stuff saved by the editor itself
					if (IsPathEditorModified(e.FullPath))
						continue;

					if (File.Exists(e.FullPath) && PathOp.IsPathLocatedIn(e.FullPath, EditorHelper.SourceMediaDirectory)) 
						reimportSchedule.Add(e.FullPath);
				}
			}
		}

		public static void FlagPathEditorModified(string path)
		{
			if (string.IsNullOrEmpty(path)) return; // Ignore bad paths
			string fullPath = Path.GetFullPath(path);
			editorModifiedFiles.Add(fullPath);
			editorModifiedFilesLast.Remove(fullPath);
		}
		private static bool IsPathEditorModified(string path)
		{
			return editorModifiedFiles.Contains(Path.GetFullPath(path));
		}
		private static void DeleteSourceMediaFile(ResourceEventArgs deleteEvent)
		{
			if (deleteEvent.IsResource)
			{
				IList<string> mediaPaths = AssetManager.GetAssetSourceFiles(deleteEvent.Content);
				for (int i = 0; i < mediaPaths.Count; i++)
				{
					if (File.Exists(mediaPaths[i]))
					{
						RecycleBin.SendSilent(mediaPaths[i]);
						PathHelper.DeleteEmptyDirectory(Path.GetDirectoryName(mediaPaths[i]), true);
					}
				}
			}
			else if (deleteEvent.IsDirectory)
			{
				string mediaPath = Path.Combine(
					EditorHelper.SourceMediaDirectory, 
					PathHelper.MakeFilePathRelative(deleteEvent.Path, DualityApp.DataDirectory));

				if (Directory.Exists(mediaPath))
				{
					RecycleBin.SendSilent(mediaPath);
					PathHelper.DeleteEmptyDirectory(Path.GetDirectoryName(mediaPath), true);
				}
			}
		}
		private static string[] PreMoveSourceMediaFile(ResourceRenamedEventArgs renameEvent)
		{
			if (!renameEvent.IsResource)
				return new string[0];
			else
				return AssetManager.GetAssetSourceFiles(renameEvent.OldContent);
		}
		private static void MoveSourceMediaFile(ResourceRenamedEventArgs renameEvent, string[] oldMediaPaths)
		{
			if (renameEvent.IsResource)
			{
				string[] newMediaPaths = AssetManager.GetAssetSourceFiles(renameEvent.Content);
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
							File.Move(oldPath, newPath);
							PathHelper.DeleteEmptyDirectory(Path.GetDirectoryName(oldPath), true);
						}
					}
				}
			}
			else if (renameEvent.IsDirectory)
			{
				// Determine which source/media directory we're going to move
				string oldMediaPath = Path.Combine(
					EditorHelper.SourceMediaDirectory, 
					PathHelper.MakeFilePathRelative(renameEvent.OldPath, DualityApp.DataDirectory));

				// Determine where that old source/media directory needs to be moved
				string newMediaPath = Path.Combine(
					EditorHelper.SourceMediaDirectory, 
					PathHelper.MakeFilePathRelative(renameEvent.Path, DualityApp.DataDirectory));

				// Move the media directory to mirror the data directories movement
				if (!PathOp.ArePathsEqual(newMediaPath, oldMediaPath))
				{
					if (Directory.Exists(oldMediaPath) && !Directory.Exists(newMediaPath))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(newMediaPath));
						Directory.Move(oldMediaPath, newMediaPath);
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
				ProcessSourceDirEvents();
				ProcessDataDirEvents();

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

				lastEventProc = DateTime.Now;
			}
		}
		private static void Resource_ResourceSaved(object sender, ResourceSaveEventArgs e)
		{
			FlagPathEditorModified(e.SaveAsPath);
		}
		
		private static void mainForm_Activated(object sender, EventArgs e)
		{
			// Perform scheduled source file reimports
			if (reimportSchedule.Count > 0)
			{
				// Hacky: Wait a little for the files to be accessable again (Might be used by another process)
				System.Threading.Thread.Sleep(50);

				string[] existingReImportFiles = reimportSchedule
					.Where(path => File.Exists(path))
					.ToArray();
				reimportSchedule.Clear();
				AssetManager.ReImportAssets(existingReImportFiles);
			}
		}
		private static void fileSystemWatcher_ForwardSource(object sender, FileSystemEventArgs e)
		{
			PushSourceDirEvent(e);
		}
		private static void fileSystemWatcher_ForwardData(object sender, FileSystemEventArgs e)
		{
			PushDataDirEvent(e, sender == dataDirWatcherDirectory);
		}
		private static void corePluginWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			// Ignore other class libraries that clearly aren't plugins
			if (!e.FullPath.EndsWith(".core.dll", StringComparison.InvariantCultureIgnoreCase) &&
				!e.FullPath.EndsWith(".editor.dll", StringComparison.InvariantCultureIgnoreCase))
				return;

			if (PluginChanged != null)
				PluginChanged(sender, e);
		}

		private static System.Collections.IEnumerable async_RenameContentRefs(ProcessingBigTaskDialog.WorkerInterface state)
		{
			var renameData = state.Data as List<ResourceRenamedEventArgs>;
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
			var targetResTypes = renameData.Any(e => e.IsDirectory) ? null : renameData.Select(e => e.ContentType).ToArray();
			var loadedContent = ContentProvider.GetLoadedContent<Resource>();
			var reloadContent = new List<IContentRef>();
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
						Log.Editor.WriteWarning("Could not determine Resource type for File '{0}' using file name only. Skipping it during rename.", file);
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
		private static int async_RenameContentRefs_Perform(object obj, List<ResourceRenamedEventArgs> args)
		{
			int counter = 0;
			ReflectionHelper.VisitObjectsDeep<IContentRef>(obj, r => 
			{
				if (r.IsDefaultContent) return r;
				if (r.IsExplicitNull) return r;
				if (string.IsNullOrEmpty(r.Path)) return r;

				foreach (ResourceRenamedEventArgs e in args)
				{
					if (e.IsResource && r.Path == e.OldPath)
					{
						r.Path = e.Path;
						counter++;
						break;
					}
					else if (e.IsDirectory && PathOp.IsPathLocatedIn(r.Path, e.OldPath))
					{
						r.Path = r.Path.Replace(e.OldPath, e.Path);
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
