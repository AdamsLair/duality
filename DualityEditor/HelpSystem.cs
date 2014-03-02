using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

using Duality;

using OpenTK;

namespace Duality.Editor
{
	public static class HelpSystem
	{
		private	static XmlCodeDoc		docDatabase			= new XmlCodeDoc();
		private	static InputEventMessageFilter	inputFilter	= null;
		private	static Control			hoveredControl		= null;
		private	static IHelpProvider	hoveredHelpProvider	= null;
		private	static bool				hoveredHelpCaptured	= false;
		private	static HelpStack		stack				= new HelpStack();
		private	static bool				needStackUpdate		= false;
		
		public static event EventHandler<HelpStackChangedEventArgs> ActiveHelpChanged
		{
			add { stack.ActiveHelpChanged += value; }
			remove { stack.ActiveHelpChanged -= value; }
		}
		public static IHelpInfoReader ActiveHelp
		{
			get { return stack.ActiveHelp; }
		}
		public static IHelpProvider ActiveHelpProvider
		{
			get { return stack.ActiveHelpProvider; }
		}

		
		internal static void Init()
		{
			LoadXmlCodeDoc();

			// Hook global message filter
			inputFilter = new InputEventMessageFilter();
			inputFilter.MouseMove += inputFilter_MouseMove;
			inputFilter.MouseLeave += inputFilter_MouseLeave;
			inputFilter.KeyDown += inputFilter_KeyDown;
			inputFilter.MouseUp += inputFilter_MouseUp;
			Application.AddMessageFilter(inputFilter);

			DualityEditorApp.EventLoopIdling += DualityEditorApp_EventLoopIdling;
		}
		internal static void Terminate()
		{
			docDatabase.Clear();

			DualityEditorApp.EventLoopIdling -= DualityEditorApp_EventLoopIdling;

			// Remove global message filter
			Application.RemoveMessageFilter(inputFilter);
			inputFilter.MouseMove -= inputFilter_MouseMove;
			inputFilter.MouseLeave -= inputFilter_MouseLeave;
			inputFilter.KeyDown -= inputFilter_KeyDown;
			inputFilter.MouseUp -= inputFilter_MouseUp;
			inputFilter = null;
		}
		
		public static void LoadXmlCodeDoc()
		{
			string mainDocPath = "Duality.xml";
			
			if (!File.Exists(mainDocPath))
			{
				string remappedPath = Path.Combine(PathHelper.ExecutingAssemblyDir, mainDocPath);
				if (File.Exists(remappedPath))
					mainDocPath = remappedPath;
			}

			if (File.Exists(mainDocPath)) LoadXmlCodeDoc(mainDocPath);
			foreach (string xmlDocFile in DualityApp.GetPluginLibPaths("*.core.xml"))
				LoadXmlCodeDoc(xmlDocFile);
		}
		public static void LoadXmlCodeDoc(string file)
		{
			XmlCodeDoc xmlDoc = new XmlCodeDoc(file);
			docDatabase.AppendDoc(xmlDoc);
		}
		public static XmlCodeDoc.Entry GetXmlCodeDoc(MemberInfo info)
		{
			return docDatabase.GetMemberDoc(info);
		}

		private static void UpdateHelpStack()
		{
			needStackUpdate = false;

			foreach (Form f in EditorHelper.GetZSortedAppWindows())
			{
				if (!f.Visible) continue;
				if (!new Rectangle(f.Location, f.Size).Contains(Cursor.Position)) continue;

				Point localPos = f.PointToClient(Cursor.Position);
				hoveredControl = f.GetChildAtPointDeep(localPos, GetChildAtPointSkip.Invisible | GetChildAtPointSkip.Transparent);
				break;
			}

			Control c;
			HelpInfo help;
			
			// Get rid of disposed Controls
			c = hoveredHelpProvider as Control;
			if (c == null || c.IsDisposed)
			{
				hoveredHelpProvider = null;
				hoveredHelpCaptured = false;
			}

			// An IHelpProvider has captured the mouse: Ask what to do with it.
			if (hoveredHelpCaptured)
			{
				help = hoveredHelpProvider.ProvideHoverHelp(c.PointToClient(Cursor.Position), ref hoveredHelpCaptured);

				// Update provider's help info
				stack.UpdateFromProvider(hoveredHelpProvider, help);

				// If still in charge: Return early.
				if (hoveredHelpCaptured) return;
			}

			// No IHelpProvider in charge: Find one that provides help
			help = null;
			IHelpProvider lastHelpProvider = hoveredHelpProvider;
			foreach (IHelpProvider hp in hoveredControl.GetControlAncestors<IHelpProvider>())
			{
				c = hp as Control;
				help = hp.ProvideHoverHelp(c.PointToClient(Cursor.Position), ref hoveredHelpCaptured);
				hoveredHelpProvider = hp;
				if (help != null || hoveredHelpCaptured) break;
			}

			// Update help system based on the result.
			if (lastHelpProvider != hoveredHelpProvider)
				stack.UpdateFromProvider(lastHelpProvider, hoveredHelpProvider, help);
			else if (hoveredHelpProvider != null)
				stack.UpdateFromProvider(hoveredHelpProvider, help);
		}
		private static bool PerformHelpAction()
		{
			bool success = false;

			// Ask Help Provider for help
			if (stack.ActiveHelp != null)
			{
				success = success | stack.ActiveHelp.PerformHelpAction(stack.ActiveHelp);
			}

			// No reaction? Just open the reference then.
			if (!success && File.Exists("DDoc.chm") && !System.Diagnostics.Process.GetProcessesByName("hh").Any())
			{
				System.Diagnostics.Process.Start("HH.exe", Path.GetFullPath("DDoc.chm"));
				success = true;
			}

			return success;
		}
		
		private static void DualityEditorApp_EventLoopIdling(object sender, EventArgs e)
		{
			if (needStackUpdate)
			{
				// Schedule help stack update in main form
				UpdateHelpStack();
				needStackUpdate = false;
			}
		}

		private static void inputFilter_MouseLeave(object sender, EventArgs e)
		{
			needStackUpdate = true;
		}
		private static void inputFilter_MouseMove(object sender, EventArgs e)
		{
			if (Control.MouseButtons != MouseButtons.None) return;
			needStackUpdate = true;
		}
		private static void inputFilter_MouseUp(object sender, EventArgs e)
		{
			if (Control.MouseButtons == MouseButtons.None)
				needStackUpdate = true;
		}
		private static void inputFilter_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F1)
				e.Handled = e.Handled || PerformHelpAction();
		}
	}
}
