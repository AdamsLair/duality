using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Reflection;
using Microsoft.Win32;

using Duality;
using Duality.IO;
using Duality.Serialization;

namespace Duality.Editor
{
	public static class EditorHelper
	{
		public const string DualityLauncherExecFile				= @"DualityLauncher.exe";
		public const string BackupDirectory						= @"Backup";
		public const string SourceDirectory						= @"Source";

	    public static readonly string SourceMediaDirectory      = Path.Combine(SourceDirectory, @"Media");
	    public static readonly string SourceCodeDirectory = Path.Combine(SourceDirectory, @"Code");
	    public static readonly string SourceCodeProjectCorePluginDir = Path.Combine(SourceCodeDirectory, @"CorePlugin");
	    public static readonly string SourceCodeProjectEditorPluginDir = Path.Combine(SourceCodeDirectory, @"EditorPlugin");
	    public static readonly string DefaultSourceCodeSolutionFile = Path.Combine(SourceCodeDirectory, @"ProjectPlugins.sln");
	    public static readonly string SourceCodeProjectCorePluginFile = Path.Combine(SourceCodeProjectCorePluginDir, @"CorePlugin.csproj");
	    public static readonly string SourceCodeProjectEditorPluginFile = Path.Combine(SourceCodeProjectEditorPluginDir, @"EditorPlugin.csproj");
	    public static readonly string SourceCodeErrorHandlerFile = Path.Combine(SourceCodeProjectCorePluginDir, @"Properties\ErrorHandlers.cs");
	    public static readonly string SourceCodeCorePluginFile = Path.Combine(SourceCodeProjectCorePluginDir, @"CorePlugin.cs");
	    public static readonly string SourceCodeComponentExampleFile = Path.Combine(SourceCodeProjectCorePluginDir, @"YourCustomComponentType.cs");
	    public static readonly string SourceCodeEditorPluginFile = Path.Combine(SourceCodeProjectEditorPluginDir, @"EditorPlugin.cs");

	    public static readonly string GlobalUserDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Duality");
		public static readonly string GlobalProjectTemplateDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Duality", "ProjectTemplates");

		private static bool isJitDebuggerAvailable;
		private static VisualStudioEdition vsEdition;

		/// <summary>
		/// The path to the *.sln solution file. Will return <see cref="DefaultSourceCodeSolutionFile"/> if the solution file has not yet been created.
		/// </summary>
		public static string SourceCodeSolutionFilePath
		{
			get
			{
				string sourceCodeSolutionFilePath = null;
				if (Directory.Exists(SourceCodeDirectory))
				{
					sourceCodeSolutionFilePath = Directory.EnumerateFiles(
						SourceCodeDirectory, 
						"*.sln", 
						SearchOption.AllDirectories)
						.FirstOrDefault();
				}
				return sourceCodeSolutionFilePath ?? DefaultSourceCodeSolutionFile;
			}
		}

		public static string CurrentProjectName
		{
			get
			{
				string dataFullPath = Path.GetFullPath(DualityApp.DataDirectory).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
				string dataDir = Path.GetDirectoryName(dataFullPath);
				return Path.GetFileName(dataDir);
			}
		}
		public static bool IsJITDebuggerAvailable
		{
			get { return isJitDebuggerAvailable; }
		}
		public static VisualStudioEdition VisualStudioEdition
		{
			get { return vsEdition; }
		}

		static EditorHelper()
		{
			isJitDebuggerAvailable = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NetFramework", "DbgManagedDebugger", null) != null;
			
			RegistryKey localMachine = null;
			if (Environment.Is64BitOperatingSystem)	localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
			else									localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);

			RegistryKey visualStudio = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio");
			string[] visualStudioSubKeys = visualStudio != null ? visualStudio.GetSubKeyNames() : null;

			vsEdition = VisualStudioEdition.Express;
		}

		public static string GenerateClassNameFromPath(string path)
		{
			// Replace chars that aren't allowed as class name
			char[] pathChars = path.ToCharArray();
			for (int i = 0; i < pathChars.Length; i++)
			{
				if (!char.IsLetterOrDigit(pathChars[i]))
					pathChars[i] = '_';
			}
			// Do not allow beginning digit
			if (char.IsDigit(pathChars[0]))
				path = "_" + new string(pathChars);
			else
				path = new string(pathChars);

			// Avoid certain ambiguity
			if (path == "System")		path = "System_";
			else if (path == "Duality")	path = "Duality_";
			else if (path == "OpenTK")	path = "OpenTK_";

			return path;
		}
		public static string GenerateErrorHandlersSrcFile(string oldRootNamespace, string rootNamespace)
		{
			string source = Properties.GeneralRes.ErrorHandlersTemplate;
			source = source.Replace("OLDROOTNAMESPACE", oldRootNamespace);
			source = source.Replace("ROOTNAMESPACE", rootNamespace);
			return source;
		}

		public static string CreateNewProject(string projName, string projFolder, ProjectTemplateInfo template)
		{
			// Determine the current executing directory, in addition to Environment.CurrentDirectory
			Assembly execAssembly = Assembly.GetEntryAssembly() ?? typeof(DualityEditorApp).Assembly;
			string execDirectory = Path.GetFullPath(Path.GetDirectoryName(execAssembly.Location));

			// Create project folder
			projFolder = Path.Combine(projFolder, projName);
			if (!Directory.Exists(projFolder)) Directory.CreateDirectory(projFolder);

			// Extract template
			if (template.SpecialTag == ProjectTemplateInfo.SpecialInfo.None)
			{
				template.ExtractTo(projFolder);

				// Update main directory
				foreach (string srcFile in Directory.GetFiles(Environment.CurrentDirectory, "*", SearchOption.TopDirectoryOnly))
				{
					if (Path.GetFileName(srcFile) == "appdata.dat") continue;
					if (Path.GetFileName(srcFile) == "defaultuserdata.dat") continue;
					string dstFile = Path.Combine(projFolder, Path.GetFileName(srcFile));
					File.Copy(srcFile, dstFile, true);
				}

				// Update plugin directory
				foreach (string dstFile in Directory.GetFiles(Path.Combine(projFolder, DualityApp.PluginDirectory), "*", SearchOption.AllDirectories))
				{
					string srcFileWorking = Path.Combine(DualityApp.PluginDirectory, Path.GetFileName(dstFile));
					string srcFileExec = Path.Combine(PathHelper.ExecutingAssemblyDir, DualityApp.PluginDirectory, Path.GetFileName(dstFile));
					if (File.Exists(srcFileWorking))
					{
						File.Copy(srcFileWorking, dstFile, true);
					}
					else if (File.Exists(srcFileExec))
					{
						File.Copy(srcFileExec, dstFile, true);
					}
				}
			}
			else if (template.SpecialTag == ProjectTemplateInfo.SpecialInfo.Current)
			{
				DualityEditorApp.SaveAllProjectData();

				Predicate<string> copyPredicate = delegate(string path) 
				{
					bool isDir = Directory.Exists(path);
					string fullPath = Path.GetFullPath(path);
					if (isDir)
					{
						return 
							!PathOp.ArePathsEqual(fullPath, EditorHelper.BackupDirectory) &&
							!PathOp.ArePathsEqual(fullPath, Path.Combine(execDirectory, EditorHelper.BackupDirectory));
					}
					else
					{
						return true;
					}
				};

				if (!PathOp.ArePathsEqual(execDirectory, Environment.CurrentDirectory))
					PathHelper.CopyDirectory(execDirectory, projFolder, true, copyPredicate);
				PathHelper.CopyDirectory(Environment.CurrentDirectory, projFolder, true, copyPredicate);
			}
			else
			{
				Predicate<string> copyPredicate = delegate(string path) 
				{
					bool isDir = Directory.Exists(path);
					string fullPath = Path.GetFullPath(path);
					if (isDir)
					{
						return 
							!PathOp.ArePathsEqual(fullPath, DualityApp.DataDirectory) &&
							!PathOp.ArePathsEqual(fullPath, EditorHelper.SourceDirectory) &&
							!PathOp.ArePathsEqual(fullPath, EditorHelper.BackupDirectory) &&
							!PathOp.ArePathsEqual(fullPath, Path.Combine(execDirectory, DualityApp.DataDirectory)) &&
							!PathOp.ArePathsEqual(fullPath, Path.Combine(execDirectory, EditorHelper.SourceDirectory)) &&
							!PathOp.ArePathsEqual(fullPath, Path.Combine(execDirectory, EditorHelper.BackupDirectory));
					}
					else
					{
						string fileName = Path.GetFileName(fullPath);
						return fileName != "appdata.dat" && fileName != "defaultuserdata.dat" && fileName != "designtimedata.dat";
					}
				};

				if (!PathOp.ArePathsEqual(execDirectory, Environment.CurrentDirectory))
					PathHelper.CopyDirectory(execDirectory, projFolder, true, copyPredicate);
				PathHelper.CopyDirectory(Environment.CurrentDirectory, projFolder, true, copyPredicate);
			}

			// Adjust current directory and perform init operations in the new project folder
			string oldPath = Environment.CurrentDirectory;
			Environment.CurrentDirectory = projFolder;
			try
			{
				// Initialize AppData
				DualityAppData data;
				data = Serializer.TryReadObject<DualityAppData>(DualityApp.AppDataPath) ?? new DualityAppData();
				data.AppName = projName;
				data.Version = 0;
				Serializer.WriteObject(data, DualityApp.AppDataPath, typeof(XmlSerializer));
			
				// Read content source code data (needed to rename classes / namespaces)
				string oldRootNamespaceNameCore;
				string newRootNamespaceNameCore;
				DualityEditorApp.ReadPluginSourceCodeContentData(out oldRootNamespaceNameCore, out newRootNamespaceNameCore);

				// Initialize source code
				DualityEditorApp.InitPluginSourceCode(); // Force re-init to update namespaces, etc.
				DualityEditorApp.UpdatePluginSourceCode();

				// Add SerializeErrorHandler class to handle renamed Types
				if (Directory.Exists(DualityApp.DataDirectory))
				{
					// Add error handler source file to project
					XDocument coreProject = XDocument.Load(SourceCodeProjectCorePluginFile);
					string relErrorHandlerPath = PathHelper.MakeFilePathRelative(
						SourceCodeErrorHandlerFile, 
						Path.GetDirectoryName(SourceCodeProjectCorePluginFile));
					if (!coreProject.Descendants("Compile", true).Any(c => string.Equals(c.GetAttributeValue("Include"), relErrorHandlerPath)))
					{
						XElement compileElement = coreProject.Descendants("Compile", true).FirstOrDefault();
						XElement newCompileElement = new XElement(
							XName.Get("Compile", compileElement.Name.NamespaceName), 
							new XAttribute("Include", relErrorHandlerPath));
						compileElement.AddAfterSelf(newCompileElement);
					}
					coreProject.Save(SourceCodeProjectCorePluginFile);

					// Generate and save error handler source code
					File.WriteAllText(
						EditorHelper.SourceCodeErrorHandlerFile, 
						EditorHelper.GenerateErrorHandlersSrcFile(oldRootNamespaceNameCore, newRootNamespaceNameCore));
				}

				// Compile plugins
				BuildHelper.BuildSolutionFile(EditorHelper.SourceCodeSolutionFilePath, "Release");
			}
			finally
			{
				Environment.CurrentDirectory = oldPath;
			}
			return Path.Combine(projFolder, "DualityEditor.exe");
		}

		public static void ShowInExplorer(string filePath)
		{
			string fullPath = Path.GetFullPath(filePath);
			string argument = @"/select, " + fullPath;
			System.Diagnostics.Process.Start("explorer.exe", argument);
		}
		public static List<Form> GetZSortedAppWindows()
		{
			List<Form> result = new List<Form>();

			// Generate a list of handles of this application's top-level forms
			List<IntPtr> openFormHandles = new List<IntPtr>();
			foreach (Form form in Application.OpenForms)
			{
				if (!form.TopLevel)
				{
					bool belongsToAnother = Application.OpenForms.OfType<Form>().Contains(form.TopLevelControl);
					if (belongsToAnother)
						continue;
				}
				openFormHandles.Add(form.Handle);
			}

			// Use WinAPI to iterate over windows in correct z-order until we found all our forms
			IntPtr hwnd = NativeMethods.GetTopWindow((IntPtr)null);
			while (hwnd != IntPtr.Zero)
			{
				// Get next window under the current handle
				hwnd = NativeMethods.GetNextWindow(hwnd, NativeMethods.GW_HWNDNEXT);

				try
				{
					if (openFormHandles.Contains(hwnd))
					{
						Form frm = Form.FromHandle(hwnd) as Form;
						result.Add(frm);

						// Found all of our windows? No need to continue.
						if (result.Count == openFormHandles.Count)
							break;
					}
				}
				catch
				{
					// Weird behaviour: In some cases, trying to cast to a Form, a handle of an object 
					// that isn't a Form will just return null. In other cases, will throw an exception.
				}
			}
			
			return result;
		}
		public static Control GetFocusedControl()
		{
			IntPtr nativeFocusControl = NativeMethods.GetFocus();

			// Early-out if nothing is focused
			if (nativeFocusControl == IntPtr.Zero)
				return null;

			// Retrieve the managed Control reference and return it
			Control focusControl = Control.FromHandle(nativeFocusControl);
			return focusControl;
		}

		private class ImageOverlaySet
		{
			private Image baseImage;
			private Dictionary<Image,Image> overlayDict;

			public Image Base
			{
				get { return this.baseImage; }
			}

			public ImageOverlaySet(Image baseImage)
			{
				this.baseImage = baseImage;
				this.overlayDict = new Dictionary<Image,Image>();;
			}
			public Image GetOverlay(Image overlayImage)
			{
				Image baseWithOverlay;
				if (!this.overlayDict.TryGetValue(overlayImage, out baseWithOverlay))
				{
					baseWithOverlay = baseImage.Clone() as Image;
					using (Graphics g = Graphics.FromImage(baseWithOverlay))
					{
						g.DrawImageUnscaled(overlayImage, 0, 0);
					}
					this.overlayDict[overlayImage] = baseWithOverlay;
				}
				return baseWithOverlay;
			}
		}
		private static Dictionary<Image,ImageOverlaySet> overlayCache = new Dictionary<Image,ImageOverlaySet>();
		public static Image GetImageWithOverlay(Image baseImage, Image overlayImage)
		{
			ImageOverlaySet overlaySet;
			if (!overlayCache.TryGetValue(baseImage, out overlaySet))
			{
				overlaySet = new ImageOverlaySet(baseImage);
				overlayCache[baseImage] = overlaySet;
			}
			return overlaySet.GetOverlay(overlayImage);
		}
	}

	public enum VisualStudioEdition
	{
		Unknown,
		Express,
		Standard
	}

	public class ProjectTemplateInfo
	{
		public enum SpecialInfo
		{
			None,
			Empty,
			Current
		}

		private string	file;
		private	Bitmap	icon;
		private	string	name;
		private	string	desc;
		private	SpecialInfo	specialTag;

		public string FilePath
		{
			get { return this.file; }
			set { this.file = value; }
		}
		public Bitmap Icon
		{
			get { return this.icon; }
			set { this.icon = value; }
		}
		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}
		public string Description
		{
			get { return this.desc; }
			set { this.desc = value; }
		}
		public SpecialInfo SpecialTag
		{
			get { return this.specialTag; }
			set { this.specialTag = value; }
		}

		public ProjectTemplateInfo() {}
		public ProjectTemplateInfo(string templatePath)
		{
			if (string.IsNullOrEmpty(templatePath)) throw new ArgumentNullException("templatePath");
			if (Path.GetExtension(templatePath) != ".zip") throw new ArgumentException("The specified template path is expected to be a .zip file.", "templatePath");
			if (!File.Exists(templatePath)) throw new FileNotFoundException("Template file does not exist", templatePath);

			using (FileStream str = File.OpenRead(templatePath)) { this.InitFrom(str); }
			this.file = templatePath;
		}
		public ProjectTemplateInfo(Stream templateStream)
		{
			this.InitFrom(templateStream);
		}

		public void ExtractTo(string dir)
		{
			if (string.IsNullOrWhiteSpace(this.file) || !File.Exists(this.file)) 
				throw new InvalidOperationException("Can't extract Project Template, because the template file is missing");

			using (FileStream stream = File.OpenRead(this.file))
			using (ZipArchive templateZip = new ZipArchive(stream))
			{
				templateZip.ExtractAll(dir, true);
			}
			if (File.Exists(Path.Combine(dir, "TemplateIcon.png"))) File.Delete(Path.Combine(dir, "TemplateIcon.png"));
			if (File.Exists(Path.Combine(dir, "TemplateInfo.xml"))) File.Delete(Path.Combine(dir, "TemplateInfo.xml"));
		}
		public void InitFrom(Stream templateStream)
		{
			if (templateStream == null) throw new ArgumentNullException("templateStream");

			this.file = null;
			this.name = "Unknown";
			this.specialTag = SpecialInfo.None;

			using (ZipArchive templateZip = new ZipArchive(templateStream))
			{
				ZipArchiveEntry entryInfo = templateZip.Entries.FirstOrDefault(z => z.Name == "TemplateInfo.xml");
				ZipArchiveEntry entryIcon = templateZip.Entries.FirstOrDefault(z => z.Name == "TemplateIcon.png");

				if (entryIcon != null)
				{
					using (MemoryStream str = new MemoryStream())
					{
						entryIcon.Extract(str);
						str.Seek(0, SeekOrigin.Begin);
						this.icon = new Bitmap(str);
					}
				}

				if (entryInfo != null)
				{
					string xmlSource = null;
					using (MemoryStream str = new MemoryStream())
					{
						entryInfo.Extract(str);
						str.Seek(0, SeekOrigin.Begin);
							
						using (StreamReader reader = new StreamReader(str))
						{
							xmlSource = reader.ReadToEnd();
						}
					}

					XDocument xmlDoc = XDocument.Parse(xmlSource);

					XElement elemName = xmlDoc.Element("name");
					if (elemName != null) this.name = elemName.Value;

					XElement elemDesc = xmlDoc.Element("description");
					if (elemDesc != null) this.desc = elemDesc.Value;
				}
			}

			return;
		}
	}
}
