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
	public class PackageUpdateSchedule
	{
		private XDocument document;

		public PackageUpdateSchedule()
		{
			this.document = new XDocument(new XElement("UpdateConfig"));
		}

		public void AppendCopyFile(string copySource, string copyTarget)
		{
			// Remove previous deletion schedules referring to the copy target
			this.RemoveUpdateItems("Remove", copyTarget);

			// Append the copy entry
			this.document.Root.Add(new XElement("Update", 
				new XAttribute("source", copySource), 
				new XAttribute("target", copyTarget)));
		}
		public void AppendDeleteFile(string deleteTarget)
		{
			// Remove previous elements referring to the yet-to-delete file
			this.RemoveUpdateItems("Update", deleteTarget);
			this.RemoveUpdateItems("IntegrateProject", deleteTarget);

			// Append the delete entry
			this.document.Root.Add(new XElement("Remove", 
				new XAttribute("target", deleteTarget)));
		}
		public void AppendIntegrateProject(string projectFile, string solutionFile, string pluginDirectory)
		{
			// Remove previous deletion schedules referring to the copy target
			this.RemoveUpdateItems("Remove", projectFile);
			this.RemoveUpdateItems("Remove", solutionFile);
			this.RemoveUpdateItems("SeparateProject", solutionFile);

			// Append the integrate entry
			this.document.Root.Add(new XElement("IntegrateProject", 
				new XAttribute("project", projectFile), 
				new XAttribute("solution", solutionFile), 
				new XAttribute("pluginDirectory", pluginDirectory)));
		}
		public void AppendSeparateProject(string projectFile, string solutionFile)
		{
			this.RemoveUpdateItems("IntegrateProject", projectFile);

			// Append the integrate entry
			this.document.Root.Add(new XElement("SeparateProject", 
				new XAttribute("project", projectFile), 
				new XAttribute("solution", solutionFile)));
		}
		public void RemoveUpdateItems(string elementName, string referringTo)
		{
			IEnumerable<XElement> query = string.IsNullOrEmpty(elementName) ? 
				this.document.Root.Elements() : 
				this.document.Root.Elements(elementName);
			List<XElement> queryResults = query.ToList();

			foreach (XElement element in queryResults)
			{
				bool anyReference = false;
				foreach (XAttribute attribute in element.Attributes())
				{
					try
					{
						if (PathOp.ArePathsEqual(attribute.Value, referringTo))
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
				if (string.Equals(Path.GetFileName(target), updaterFilePath, StringComparison.InvariantCultureIgnoreCase))
				{
					if (string.Equals(element.Name.LocalName, "Remove", StringComparison.InvariantCultureIgnoreCase))
					{
						File.Delete(target);
						applied = true;
					}
					else if (string.Equals(element.Name.LocalName, "Update", StringComparison.InvariantCultureIgnoreCase))
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

		public void Save(string updateFilePath)
		{
			this.document.Save(updateFilePath);
		}

		public static PackageUpdateSchedule Load(string updateFilePath)
		{
			PackageUpdateSchedule schedule = new PackageUpdateSchedule();
			schedule.document = XDocument.Load(updateFilePath);
			return schedule;
		}
	}
}
