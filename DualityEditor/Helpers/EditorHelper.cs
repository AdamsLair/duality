using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using Microsoft.Win32;
using Microsoft.Build.Execution;

using Ionic.Zip;

using Duality;
using Duality.Serialization;
using Duality.Serialization.MetaFormat;

namespace DualityEditor
{
	public static class EditorHelper
	{
		public const string DualityLauncherExecFile		= @"DualityLauncher.exe";
		public const string BackupDirectory				= @"Backup";
		public const string SourceDirectory				= @"Source";
		public const string SourceMediaDirectory		= SourceDirectory + @"\Media";
		public const string SourceCodeDirectory			= SourceDirectory + @"\Code";
		public const string SourceCodeProjectCorePluginDir		= SourceCodeDirectory + @"\CorePlugin";
		public const string SourceCodeProjectEditorPluginDir	= SourceCodeDirectory + @"\EditorPlugin";
		public const string SourceCodeSolutionFile				= SourceCodeDirectory + @"\ProjectPlugins.sln";
		public const string SourceCodeProjectCorePluginFile		= SourceCodeProjectCorePluginDir + @"\CorePlugin.csproj";
		public const string SourceCodeProjectEditorPluginFile	= SourceCodeProjectEditorPluginDir + @"\EditorPlugin.csproj";
		public const string SourceCodeGameResFile			= SourceCodeProjectCorePluginDir + @"\Properties\GameRes.cs";
		public const string SourceCodeCorePluginFile		= SourceCodeProjectCorePluginDir + @"\CorePlugin.cs";
		public const string SourceCodeComponentExampleFile	= SourceCodeProjectCorePluginDir + @"\YourCustomComponentType.cs";
		public const string SourceCodeEditorPluginFile		= SourceCodeProjectEditorPluginDir + @"\EditorPlugin.cs";

		public static readonly string GlobalUserDirectory = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Duality"));
		public static readonly string GlobalProjectTemplateDirectory = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Duality"), "ProjectTemplates");

		public static string CurrentProjectName
		{
			get
			{
				string dataFullPath = Path.GetFullPath(DualityApp.DataDirectory).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
				string dataDir = Path.GetDirectoryName(dataFullPath);
				return Path.GetFileName(dataDir);
			}
		}


		public static bool IsJITDebuggerAvailable()
		{
			return Registry.LocalMachine
				.OpenSubKey("SOFTWARE")
				.OpenSubKey("Microsoft")
				.OpenSubKey(".NetFramework")
				.GetValueNames().Contains("DbgManagedDebugger");
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
		public static string GenerateGameResSrcFile()
		{
			string gameRes = EditorRes.GeneralRes.GameResTemplate;
			string mainClassName;
			StringBuilder builder = new StringBuilder();
			GenerateGameResSrcFile_ScanDir(builder, DualityApp.DataDirectory, 1, out mainClassName);
			return gameRes.Replace("CONTENT", builder.ToString());
		}
		private static void GenerateGameResSrcFile_ScanFile(StringBuilder builder, string filePath, int indent, out string propName)
		{
			if (!PathHelper.IsPathVisible(filePath)) { propName = null; return; }
			if (!Resource.IsResourceFile(filePath)) { propName = null; return; }

			Type resType = Resource.GetTypeByFileName(filePath);
			string typeStr = resType.GetTypeCSCodeName();
			string indentStr = new string('\t', indent);
			propName = GenerateGameResSrcFile_ClassName(filePath);

			builder.Append(indentStr); 
			builder.Append("private static Duality.ContentRef<");
			builder.Append(typeStr);
			builder.Append("> _");
			builder.Append(propName);
			builder.AppendLine(";");

			builder.Append(indentStr); 
			builder.Append("public static Duality.ContentRef<");
			builder.Append(typeStr);
			builder.Append("> ");
			builder.Append(propName);
			builder.Append(" { get { ");
			builder.Append("if (_");
			builder.Append(propName);
			builder.Append(".IsExplicitNull) _");
			builder.Append(propName);
			builder.Append(" = Duality.ContentProvider.RequestContent<");
			builder.Append(typeStr);
			builder.Append(">(@\"");
			builder.Append(filePath);
			builder.Append("\"); ");
			builder.Append("return _");
			builder.Append(propName);
			builder.AppendLine("; }}");
		}
		private static void GenerateGameResSrcFile_ScanDir(StringBuilder builder, string dirPath, int indent, out string className)
		{
			if (!PathHelper.IsPathVisible(dirPath)) { className = null; return; }
			dirPath = dirPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

			string indentStr = new string('\t', indent);
			className = GenerateGameResSrcFile_ClassName(dirPath);

			// ---------- Begin class ----------
			builder.Append(indentStr); 
			builder.Append("public static class ");
			builder.Append(className);
			builder.AppendLine(" {");

			// ---------- Sub directories ----------
			string[] subDirs = Directory.GetDirectories(dirPath);
			List<string> dirClassNames = new List<string>();
			foreach (string dir in subDirs)
			{
				string dirClassName;
				GenerateGameResSrcFile_ScanDir(builder, dir, indent + 1, out dirClassName);
				if (!string.IsNullOrEmpty(dirClassName))
					dirClassNames.Add(dirClassName);
			}

			// ---------- Files ----------
			string[] files = Directory.GetFiles(dirPath);
			List<string> filePropNames = new List<string>();
			foreach (string file in files)
			{
				string propName;
				GenerateGameResSrcFile_ScanFile(builder, file, indent + 1, out propName);
				if (!string.IsNullOrEmpty(propName))
					filePropNames.Add(propName);
			}

			// ---------- LoadAll() method ----------
			builder.Append(indentStr); 
			builder.Append('\t'); 
			builder.AppendLine("public static void LoadAll() {");
			foreach (string dirClassName in dirClassNames)
			{
				builder.Append(indentStr); 
				builder.Append('\t'); 
				builder.Append('\t'); 
				builder.Append(dirClassName);
				builder.AppendLine(".LoadAll();");
			}
			foreach (string propName in filePropNames)
			{
				builder.Append(indentStr); 
				builder.Append('\t'); 
				builder.Append('\t'); 
				builder.Append(propName);
				builder.AppendLine(".MakeAvailable();");
			}
			builder.Append(indentStr); 
			builder.Append('\t'); 
			builder.AppendLine("}");

			// ---------- End class ----------
			builder.Append(indentStr); 
			builder.AppendLine("}");
		}
		private static string GenerateGameResSrcFile_ClassName(string path)
		{
			// Strip path and resource extension
			if (Resource.IsResourceFile(path))
				path = Path.GetFileNameWithoutExtension(path);
			else
				path = Path.GetFileName(path);

			return GenerateClassNameFromPath(path);
		}

		public static string CreateNewProject(string projName, string projFolder, ProjectTemplateInfo template)
		{
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
					string srcFile = Path.Combine(DualityApp.PluginDirectory, Path.GetFileName(dstFile));
					if (File.Exists(srcFile)) File.Copy(srcFile, dstFile, true);
				}
			}
			else if (template.SpecialTag == ProjectTemplateInfo.SpecialInfo.Current)
			{
				DualityEditorApp.SaveAllProjectData();
				PathHelper.CopyDirectory(Environment.CurrentDirectory, projFolder, true, delegate(string path)
				{
					bool isDir = Directory.Exists(path);
					string fullPath = Path.GetFullPath(path);
					if (isDir)
					{
						return fullPath != Path.GetFullPath(EditorHelper.BackupDirectory);
					}
					else
					{
						return true;
					}
				});
			}
			else
			{
				PathHelper.CopyDirectory(Environment.CurrentDirectory, projFolder, true, delegate(string path)
				{
					bool isDir = Directory.Exists(path);
					string fullPath = Path.GetFullPath(path);
					if (isDir)
					{
						return 
							fullPath != Path.GetFullPath(DualityApp.DataDirectory) &&
							fullPath != Path.GetFullPath(EditorHelper.SourceDirectory) &&
							fullPath != Path.GetFullPath(EditorHelper.BackupDirectory);
					}
					else
					{
						string fileName = Path.GetFileName(fullPath);
						return fileName != "appdata.dat" && fileName != "defaultuserdata.dat" && fileName != "designtimedata.dat";
					}
				});
			}

			// Adjust current directory for further operations
			string oldPath = Environment.CurrentDirectory;
			Environment.CurrentDirectory = projFolder;

			// Initialize content
			if (Directory.Exists(DualityApp.DataDirectory))
			{
				// Read content source code data (needed to rename classes / namespaces)
				string oldRootNamespaceNameCore;
				string newRootNamespaceNameCore;
				DualityEditorApp.ReadPluginSourceCodeContentData(out oldRootNamespaceNameCore, out newRootNamespaceNameCore);

				// Rename classes / namespaces
				List<string> resFiles = Resource.GetResourceFiles(DualityApp.DataDirectory);
				foreach (string resFile in resFiles)
				{
					MetaFormatHelper.FilePerformAction(resFile, d => d.ReplaceTypeStrings(oldRootNamespaceNameCore, newRootNamespaceNameCore));
				}
			}

			// Initialize AppData
			DualityAppData data;
			if (File.Exists(DualityApp.AppDataPath))
			{
				try
				{
					using (FileStream str = File.OpenRead(DualityApp.AppDataPath))
					{
						using (var formatter = Formatter.Create(str))
						{
							data = formatter.ReadObject() as DualityAppData ?? new DualityAppData();
						}
					}
				}
				catch (Exception) { data = new DualityAppData(); }
			}
			else
			{
				data = new DualityAppData();
			}
			data.AppName = projName;
			data.AuthorName = Environment.UserName;
			data.Version = 0;
			using (FileStream str = File.Open(DualityApp.AppDataPath, FileMode.Create))
			{
				using (var formatter = Formatter.Create(str, FormattingMethod.Binary))
				{
					formatter.WriteObject(data);
				}
			}

			// Initialize source code
			DualityEditorApp.InitPluginSourceCode(); // Force re-init to update namespaces, etc.
			DualityEditorApp.UpdatePluginSourceCode();

			// Compile plugins
			var buildProperties = new Dictionary<string, string>();
			buildProperties["Configuration"] = "Release";
			var buildRequest = new BuildRequestData(EditorHelper.SourceCodeSolutionFile, buildProperties, null, new string[] { "Build" }, null);
			var buildParameters = new BuildParameters();
			var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest);

			Environment.CurrentDirectory = oldPath;
			return Path.Combine(projFolder, "DualityEditor.exe");
		}

		public static List<Form> GetZSortedAppWindows()
		{
			List<Form> result = new List<Form>();

			IntPtr hwnd = NativeMethods.GetTopWindow((IntPtr)null);
			while (hwnd != IntPtr.Zero)
			{
				// Get next window under the current handler
				hwnd = NativeMethods.GetNextWindow(hwnd, NativeMethods.GW_HWNDNEXT);

				try
				{
					Form frm = Form.FromHandle(hwnd) as Form;
					if (frm != null && Application.OpenForms.OfType<Form>().Contains(frm))
						result.Add(frm);
				}
				catch
				{
					// Weird behaviour: In some cases, trying to cast to a Form a handle of an object 
					// that isn't a form will just return null. In other cases, will throw an exception.
				}
			}

			return result;
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

			using (ZipFile templateZip = ZipFile.Read(this.file))
			{
				templateZip.ExtractAll(dir, ExtractExistingFileAction.OverwriteSilently);
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

			using (ZipFile templateZip = ZipFile.Read(templateStream))
			{
				ZipEntry entryInfo = templateZip.FirstOrDefault(z => !z.IsDirectory && z.FileName == "TemplateInfo.xml");
				ZipEntry entryIcon = templateZip.FirstOrDefault(z => !z.IsDirectory && z.FileName == "TemplateIcon.png");

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

					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(xmlSource);

					XmlElement elemName = xmlDoc.DocumentElement["name"];
					if (elemName != null) this.name = elemName.InnerText;

					XmlElement elemDesc = xmlDoc.DocumentElement["description"];
					if (elemDesc != null) this.desc = elemDesc.InnerText;
				}
			}

			return;
		}
	}
}
