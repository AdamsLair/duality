using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;


namespace Duality.Editor.Plugins.Base.DataConverters
{
	public class SoundFromAudioData : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(Sound); }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
			{
				return convert.CanPerform<AudioData>();
			}
			
			if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.Convert))
			{
				List<AudioData> availData = convert.Perform<AudioData>(ConvertOperation.Operation.Convert).ToList();
				return availData.Any(t => this.FindMatch(t) != null);
			}

			return false;
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;
			List<AudioData> availData = convert.Perform<AudioData>().ToList();
			List<AudioData> createDataSource = null;

			// Match objects
			foreach (AudioData baseRes in availData)
			{
				if (convert.IsObjectHandled(baseRes)) continue;

				// Find target Resource matching the source
				Sound targetRes = this.FindMatch(baseRes);
				if (targetRes != null)
				{
					convert.AddResult(targetRes);
				}
				else if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
				{
					if (createDataSource == null) createDataSource = new List<AudioData>();
					createDataSource.Add(baseRes);
				}
				else
				{
					// Can't handle this AudioData
					continue;
				}

				finishConvertOp = true;
				convert.MarkObjectHandled(baseRes);
			}

			// Create objects
			if (createDataSource != null)
			{
				List<ContentRef<Sound>> createdSounds = EditorActions.AudioDataToSound.CreateMultipleFromAudioData(createDataSource.Ref());
				foreach (ContentRef<Sound> sound in createdSounds)
					convert.AddResult(sound.Res);
			}

			return finishConvertOp;
		}
		private Sound FindMatch(AudioData baseRes)
		{
			if (baseRes == null)
			{
				return null;
			}
			else if (baseRes.IsDefaultContent)
			{
				var defaultContent = ContentProvider.GetDefaultContent<Resource>();
				return defaultContent.Res().OfType<Sound>().FirstOrDefault(r => r.Data != null && r.Data.Count == 1 && r.MainData == baseRes);
			}
			else
			{
				// First try a direct approach
				string fileExt = Resource.GetFileExtByType<Sound>();
				string targetPath = baseRes.FullName + fileExt;
				Sound match = ContentProvider.RequestContent<Sound>(targetPath).Res;
				if (match != null) return match;

				// If that fails, search for other matches
				string targetName = baseRes.Name + fileExt;
				string[] resFilePaths = Resource.GetResourceFiles().ToArray();
				var resNameMatch = resFilePaths.Where(p => Path.GetFileName(p) == targetName);
				var resQuery = resNameMatch.Concat(resFilePaths).Distinct();
				List<Sound> matchCandidates = new List<Sound>();
				foreach (string resFile in resQuery)
				{
					if (!resFile.EndsWith(fileExt)) continue;
					match = ContentProvider.RequestContent<Sound>(resFile).Res;
					if (match != null && match.Data != null && match.Data.Contains(baseRes)) matchCandidates.Add(match);
				}
				// Found some matches? Return the narrowest one
				if (matchCandidates.Count > 0)
				{
					return matchCandidates.OrderBy(m => m.Data.Count).First();
				}

				// Give up.
				return null;
			}
		}
	}
}
