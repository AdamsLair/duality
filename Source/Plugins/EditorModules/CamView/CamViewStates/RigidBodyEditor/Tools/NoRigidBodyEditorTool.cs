using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Editor.Plugins.CamView.Properties;
using Duality.Editor.Properties;


namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// A tool that allows you to select and edit <see cref="Duality.Components.Physics.ShapeInfo"/> objects using
	/// the regular move / rotate / scale actions that you already know from the Scene Editor.
	/// </summary>
	public class NoRigidBodyEditorTool : RigidBodyEditorTool
	{
		public override string Name
		{
			get { return CamViewRes.RigidBodyCamViewState_ItemName_EditShapes; }
		}
		public override Image Icon
		{
			get { return CamViewResCache.IconShapeSelect; }
		}
		public override Cursor ActionCursor
		{
			get { return CursorHelper.Arrow; }
		}
		public override int SortOrder
		{
			get { return -1000; }
		}
	}
}
