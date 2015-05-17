using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Duality;
using Duality.Resources;
using Duality.Drawing;
using Duality.Audio;
using Duality.Editor;
using Font = Duality.Resources.Font;


namespace Duality.Editor.Plugins.Base.PreviewGenerators
{
	public class AudioDataPreviewGenerator : PreviewGenerator<AudioData>
	{
		public override void Perform(AudioData audio, PreviewImageQuery query)
		{
			int desiredWidth = query.DesiredWidth;
			int desiredHeight = query.DesiredHeight;
			int oggHash = audio.OggVorbisData.GetCombinedHashCode();
			int oggLen = audio.OggVorbisData.Length;
			PcmData pcm = OggVorbis.LoadChunkFromMemory(audio.OggVorbisData, 500000);
			short[] sdata = pcm.Data;
			short maxVal = 1;
			for (int i = 0; i < pcm.DataLength; i++)
			{
				maxVal = Math.Max(maxVal, Math.Abs(pcm.Data[i]));
			}

			Bitmap result = new Bitmap(desiredWidth, desiredHeight);
			int channelLength = pcm.DataLength / pcm.ChannelCount;
			int yMid = result.Height / 2;
			int stepWidth = (channelLength / (2 * result.Width)) - 1;
			const int samples = 10;
			using (Graphics g = Graphics.FromImage(result))
			{
				Color baseColor = ExtMethodsColor.ColorFromHSV(
					(float)(oggHash % 90) * (float)(oggLen % 4) / 360.0f, 
					0.5f, 
					1f);
				Pen linePen = new Pen(Color.FromArgb(MathF.RoundToInt(255.0f / MathF.Pow((float)samples, 0.65f)), baseColor));
				g.Clear(Color.Transparent);
				for (int x = 0; x < result.Width; x++)
				{
					float invMaxVal = 2.0f / ((float)maxVal + (float)short.MaxValue);
					float timePercentage = (float)x / (float)result.Width;
					int i = MathF.RoundToInt(timePercentage * channelLength);
					float left;
					float right;
					short channel1;
					short channel2;

					for (int s = 0; s <= samples; s++)
					{
						int offset = stepWidth * s / samples;
						channel1 = sdata[(i + offset) * pcm.ChannelCount + 0];
						channel2 = sdata[(i + offset) * pcm.ChannelCount + 1];
						left = (float)Math.Abs(channel1) * invMaxVal;
						right = (float)Math.Abs(channel2) * invMaxVal;
						g.DrawLine(linePen, x, yMid, x, yMid + MathF.RoundToInt(left * yMid));
						g.DrawLine(linePen, x, yMid, x, yMid - MathF.RoundToInt(right * yMid));
					}
				}
			}

			query.Result = result;
		}
		public override void Perform(AudioData obj, PreviewSoundQuery query)
		{
			base.Perform(obj, query);
			query.Result = new Sound(obj);
		}
	}
}
