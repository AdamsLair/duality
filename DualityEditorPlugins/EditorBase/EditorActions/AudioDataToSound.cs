using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;

using Duality.Resources;
using Duality.Properties;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.UndoRedoActions;

namespace Duality.Editor.Plugins.Base.EditorActions
{
	public class AudioDataToSound : EditorAction<AudioData>
	{
		public override string Name
		{
			get { return EditorBaseRes.ActionName_CreateSound; }
		}
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_CreateSound; }
		}
		public override Image Icon
		{
			get { return typeof(Sound).GetEditorImage(); }
		}

		public override void Perform(IEnumerable<AudioData> objEnum)
		{
			Sound.CreateMultipleFromAudioData(objEnum.Ref());
		}
	}
}
