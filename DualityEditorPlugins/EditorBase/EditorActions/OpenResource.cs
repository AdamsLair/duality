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
	public class OpenResource : EditorSingleAction<Resource>
	{
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_OpenResourceExternal; }
		}

		public override void Perform(Resource obj)
		{
			Pixmap			pixmap		= obj as Pixmap;
			AudioData		audioData	= obj as AudioData;
			AbstractShader	shader		= obj as AbstractShader;

			if (pixmap != null)
				FileImportProvider.OpenSourceFile(pixmap, PixmapFileImporter.SourceFileExtPrimary, pixmap.SavePixelData);
			else if (audioData != null)
				FileImportProvider.OpenSourceFile(audioData, AudioDataFileImporter.SourceFileExtPrimary, audioData.SaveOggVorbisData);
			else if (shader != null)
				FileImportProvider.OpenSourceFile(shader, shader is FragmentShader ? ShaderFileImporter.SourceFileExtFragment : ShaderFileImporter.SourceFileExtVertex, shader.SaveSource);
		}
		public override bool CanPerformOn(Resource obj)
		{
			if (!base.CanPerformOn(obj)) return false;
			if (obj is Pixmap) return true;
			if (obj is AudioData) return true;
			if (obj is AbstractShader) return true;
			return false;
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextOpenRes;
		}
	}
}
