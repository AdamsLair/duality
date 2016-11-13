using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.Drawing;

using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Plugins.CamView.Properties;
using Duality.Editor.Plugins.CamView.UndoRedoActions;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	public enum ObjectEditorAction
	{
		None,
		RectSelect,
		Move,
		Rotate,
		Scale,
	}
}
