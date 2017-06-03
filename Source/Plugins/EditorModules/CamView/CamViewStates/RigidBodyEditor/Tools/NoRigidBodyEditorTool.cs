using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// A dummy tool that represents not performing any operation on the edited <see cref="Duality.Components.Physics.RigidBody"/>.
	/// </summary>
	public class NoRigidBodyEditorTool : RigidBodyEditorTool
	{
		public override string Name
		{
			get { return null; }
		}
		public override Image Icon
		{
			get { return null; }
		}
		public override Cursor ActionCursor
		{
			get { return CursorHelper.Arrow; }
		}
	}
}
