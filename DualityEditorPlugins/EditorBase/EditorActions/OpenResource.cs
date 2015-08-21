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
			{
				FileImportProvider.OpenSourceFile(
					pixmap, 
					PixmapFileImporter.SourceFileExtPrimary, 
					path => SavePixmapData(pixmap, path));
			}
			else if (audioData != null)
			{
				FileImportProvider.OpenSourceFile(
					audioData, 
					AudioDataFileImporter.SourceFileExtPrimary, 
					path => SaveAudioData(audioData, path));
			}
			else if (shader != null)
			{
				string fileExt;
				if (shader is FragmentShader)
					fileExt = ShaderFileImporter.SourceFileExtFragment;
				else
					fileExt = ShaderFileImporter.SourceFileExtVertex;

				FileImportProvider.OpenSourceFile(
					shader, 
					fileExt, 
					path => SaveShaderData(shader, path));
			}
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

		private static void SavePixmapData(Pixmap pixmap, string targetPath)
		{
			using (Bitmap bmp = pixmap.MainLayer.ToBitmap())
			{
				bmp.Save(targetPath);
			}
		}
		private static void SaveShaderData(AbstractShader shader, string targetPath)
		{
			File.WriteAllText(targetPath, shader.Source);
		}
		private static void SaveAudioData(AudioData audio, string targetPath)
		{
			File.WriteAllBytes(targetPath, audio.OggVorbisData);
		}
	}
}
