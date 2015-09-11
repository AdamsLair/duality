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
				AssetManager.OpenSourceFile(
					pixmap,
 					(res, dir) => new string[] { Path.Combine(dir, res.Name) + PixmapAssetImporter.SourceFileExtPrimary },
					(res, dir) => SavePixmapData(res as Pixmap, Path.Combine(dir, res.Name) + PixmapAssetImporter.SourceFileExtPrimary));
			}
			else if (audioData != null)
			{
				AssetManager.OpenSourceFile(
					audioData, 
 					(res, dir) => new string[] { Path.Combine(dir, res.Name) + AudioDataAssetImporter.SourceFileExtPrimary },
					(res, dir) => SaveAudioData(res as AudioData, Path.Combine(dir, res.Name) + AudioDataAssetImporter.SourceFileExtPrimary));
			}
			else if (shader != null)
			{
				string fileExt;
				if (shader is FragmentShader)
					fileExt = ShaderAssetImporter.SourceFileExtFragment;
				else
					fileExt = ShaderAssetImporter.SourceFileExtVertex;

				AssetManager.OpenSourceFile(
					shader, 
 					(res, dir) => new string[] { Path.Combine(dir, res.Name) + fileExt },
					(res, dir) => SaveShaderData(res as AbstractShader, Path.Combine(dir, res.Name) + fileExt));
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
