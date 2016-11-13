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
				return availData.Any(t => 
					this.FindMatchingResources<AudioData,Sound>(t, IsMatch)
					.Any());
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
				Sound targetRes = 
					this.FindMatchingResources<AudioData,Sound>(baseRes, IsMatch)
					.OrderBy(sound => sound.Data.Count)
					.FirstOrDefault();
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

		private static bool IsMatch(AudioData source, Sound target)
		{
			return target.Data != null && target.Data.Contains(source);
		}
	}
}
