using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Diagnostics;

using NuGet;

using Duality.IO;
using Duality.Editor.Properties;
using Duality.Editor.Forms;

namespace Duality.Editor.PackageManagement
{
	/// <summary>
	/// Describes the required operations to apply a previously performed package operation.
	/// 
	/// The schedule is what's usually stored in a file at the <see cref="PackageManagerEnvironment.UpdateFilePath"/>,
	/// which is read and executed by the updater application.
	/// </summary>
	public class PackageUpdateSchedule
	{
		/// <summary>
		/// Element name of copy file update instructions. See <see cref="AppendCopyFile"/>.
		/// </summary>
		public static readonly string CopyItem = "Update";
		/// <summary>
		/// Element name of delete file update instructions. See <see cref="AppendDeleteFile"/>.
		/// </summary>
		public static readonly string DeleteItem = "Remove";
		/// <summary>
		/// Element name of project integration update instructions. See <see cref="AppendIntegrateProject"/>.
		/// </summary>
		public static readonly string IntegrateProjectItem = "IntegrateProject";
		/// <summary>
		/// Element name of project separation update instructions. See <see cref="AppendSeparateProject"/>.
		/// </summary>
		public static readonly string SeparateProjectItem = "SeparateProject";


		private XDocument document;

		/// <summary>
		/// [GET] Enumerates all update instruction items in the schedule.
		/// The type of each instruction can be derived from the element names,
		/// but be aware that both names and attributes are an implementation
		/// detail that can be subject to change in future versions.
		/// </summary>
		public IEnumerable<XElement> Items
		{
			get { return this.document.Root.Elements(); }
		}

		public PackageUpdateSchedule()
		{
			this.document = new XDocument(new XElement("UpdateConfig"));
		}

		/// <summary>
		/// Appends a copy file instruction to the update schedule.
		/// </summary>
		/// <param name="copySource"></param>
		/// <param name="copyTarget"></param>
		public void AppendCopyFile(string copySource, string copyTarget)
		{
			// Remove previous deletion schedules referring to the copy target
			this.RemoveItems(DeleteItem, copyTarget);

			// Append the copy entry
			this.document.Root.Add(new XElement(CopyItem, 
				new XAttribute("source", copySource), 
				new XAttribute("target", copyTarget)));
		}
		/// <summary>
		/// Appends a delete file instruction to the update schedule.
		/// </summary>
		/// <param name="deleteTarget"></param>
		public void AppendDeleteFile(string deleteTarget)
		{
			// Remove previous elements referring to the yet-to-delete file
			this.RemoveItems(CopyItem, deleteTarget);
			this.RemoveItems(IntegrateProjectItem, deleteTarget);

			// Append the delete entry
			this.document.Root.Add(new XElement(DeleteItem, 
				new XAttribute("target", deleteTarget)));
		}
		/// <summary>
		/// Appends a project integration instruction to the update schedule.
		/// This instruction type will make sure that the specified source code project file
		/// is referenced in the Duality project's primary source code solution file.
		/// </summary>
		/// <param name="projectFile"></param>
		/// <param name="solutionFile"></param>
		/// <param name="pluginDirectory"></param>
		public void AppendIntegrateProject(string projectFile, string solutionFile, string pluginDirectory)
		{
			// Remove previous deletion schedules referring to the copy target
			this.RemoveItems(DeleteItem, projectFile);
			this.RemoveItems(DeleteItem, solutionFile);
			this.RemoveItems(SeparateProjectItem, solutionFile);

			// Append the integrate entry
			this.document.Root.Add(new XElement(IntegrateProjectItem, 
				new XAttribute("project", projectFile), 
				new XAttribute("solution", solutionFile), 
				new XAttribute("pluginDirectory", pluginDirectory)));
		}
		/// <summary>
		/// Appends a project separation instruction to the update schedule.
		/// This instruction type will make sure that the specified source code project file
		/// is no longer referenced in the Duality project's primary source code solution file.
		/// </summary>
		/// <param name="projectFile"></param>
		/// <param name="solutionFile"></param>
		public void AppendSeparateProject(string projectFile, string solutionFile)
		{
			this.RemoveItems(IntegrateProjectItem, projectFile);

			// Append the integrate entry
			this.document.Root.Add(new XElement(SeparateProjectItem, 
				new XAttribute("project", projectFile), 
				new XAttribute("solution", solutionFile)));
		}
		private void RemoveItems(string itemType, string referringToFile)
		{
			IEnumerable<XElement> query = string.IsNullOrEmpty(itemType) ? 
				this.document.Root.Elements() : 
				this.document.Root.Elements(itemType);
			List<XElement> queryResults = query.ToList();

			foreach (XElement element in queryResults)
			{
				bool anyReference = false;
				foreach (XAttribute attribute in element.Attributes())
				{
					try
					{
						if (PathOp.ArePathsEqual(attribute.Value, referringToFile))
						{
							anyReference = true;
							break;
						}
					}
					catch (Exception) {}
				}
				if (anyReference)
				{
					element.Remove();
				}
			}
		}
		
		/// <summary>
		/// Applies all changes that affect the Duality updater itself, which for that
		/// reason can't be done by the updater. The applied items will be removed from
		/// the schedule.
		/// </summary>
		/// <param name="updaterFilePath"></param>
		public void ApplyUpdaterChanges(string updaterFilePath)
		{
			List<XElement> updateItems = this.document.Root.Elements().ToList();
			foreach (XElement element in updateItems)
			{
				XAttribute attribTarget = element.Attribute("target");
				XAttribute attribSource = element.Attribute("source");
				string target = (attribTarget != null) ? attribTarget.Value : null;
				string source = (attribSource != null) ? attribSource.Value : null;

				// Apply updates that affect the updater itself
				bool applied = false;
				if (PathOp.ArePathsEqual(target, updaterFilePath))
				{
					if (string.Equals(element.Name.LocalName, DeleteItem, StringComparison.InvariantCultureIgnoreCase))
					{
						File.Delete(target);
						applied = true;
					}
					else if (string.Equals(element.Name.LocalName, CopyItem, StringComparison.InvariantCultureIgnoreCase))
					{
						File.Copy(source, target, true);
						applied = true;
					}
				}

				// Remove applied elements from the schedule
				if (applied)
					element.Remove();
			}
		}

		/// <summary>
		/// Saves the update schedule to the specified file path.
		/// </summary>
		/// <param name="updateFilePath"></param>
		public void Save(string updateFilePath)
		{
			this.document.Save(updateFilePath);
		}

		/// <summary>
		/// Loads an existing update schedule from the specified file path.
		/// </summary>
		/// <param name="updateFilePath"></param>
		/// <returns></returns>
		public static PackageUpdateSchedule Load(string updateFilePath)
		{
			PackageUpdateSchedule schedule = new PackageUpdateSchedule();
			schedule.document = XDocument.Load(updateFilePath);
			return schedule;
		}
	}
}
