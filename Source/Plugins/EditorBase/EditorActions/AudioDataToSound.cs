using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;

using Duality.IO;
using Duality.Resources;
using Duality.Properties;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.UndoRedoActions;

namespace Duality.Editor.Plugins.Base.EditorActions
{
	/// <summary>
	/// Creates a new Sound Resource based on the AudioData.
	/// </summary>
	public class AudioDataToSound : EditorAction<AudioData>
	{
		public override string Name
		{
			get { return EditorBaseRes.ActionName_CreateSound; }
		}
		public override Image Icon
		{
			get { return typeof(Sound).GetEditorImage(); }
		}

		public override void Perform(IEnumerable<AudioData> objEnum)
		{
			CreateMultipleFromAudioData(objEnum.Ref());
		}

		/// <summary>
		/// Creates a new Sound Resource based on the specified AudioData, saves it and returns a reference to it.
		/// </summary>
		/// <param name="baseRes"></param>
		/// <param name="name"></param>
		private static ContentRef<Sound> CreateFromAudioData(IEnumerable<ContentRef<AudioData>> baseRes, string name = null)
		{
			if (!baseRes.Any()) return null;

			string basePath = baseRes.FirstOrDefault().FullName;
			if (name != null) basePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(basePath), name);

			string resPath = PathHelper.GetFreePath(basePath, Resource.GetFileExtByType(typeof(Sound)));
			Sound res = new Sound(baseRes);
			res.Save(resPath);
			return res;
		}
		/// <summary>
		/// Creates a set of new Sound Resources based on the specified AudioData, saves it and returns references to it.
		/// The incoming AudioData is automatically grouped to the least number of Sounds, according to naming and path conventions.
		/// </summary>
		/// <param name="baseRes"></param>
		public static List<ContentRef<Sound>> CreateMultipleFromAudioData(IEnumerable<ContentRef<AudioData>> baseRes)
		{
			char[] trimEndChars = new []{'0','1','2','3','4','5','6','7','8','9','_','-','.','#','~'};
			List<ContentRef<AudioData>> sourceData = baseRes.Reverse().ToList();
			List<ContentRef<Sound>> result = new List<ContentRef<Sound>>();

			// Split data into singular data and grouped data
			while (sourceData.Count > 0)
			{
				ContentRef<AudioData> data = sourceData[sourceData.Count - 1];
				string mutualName = data.Name.TrimEnd(trimEndChars);
				string mutualDir = System.IO.Path.GetDirectoryName(data.Path);

				// Group similar AudioData
				List<ContentRef<AudioData>> localGroup = new List<ContentRef<AudioData>>();
				for (int i = sourceData.Count - 1; i >= 0; i--)
				{
					if (System.IO.Path.GetDirectoryName(sourceData[i].Path) != mutualDir) continue;
					if (!sourceData[i].Name.StartsWith(mutualName)) continue;
					localGroup.Add(sourceData[i]);
					sourceData.RemoveAt(i);
				}
				result.Add(CreateFromAudioData(localGroup, localGroup.Count > 1 ? mutualName : null));
			}

			return result;
		}
	}
}
