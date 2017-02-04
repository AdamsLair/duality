using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Resources;
using Duality.IO;
using System.IO;

namespace FlapOrDie
{
	/// <summary>
	/// Defines a Duality core plugin.
	/// </summary>
	public class FlapOrDieCorePlugin : CorePlugin
	{
		private static readonly string HIGHSCORE_FILE = @".\high.score";

		[DontSerialize]
		private static ushort highScore;

		[DontSerialize]
		private static ushort currentHighScore;

		[DontSerialize]
		private static float halfWidth;

		public static float HalfWidth
		{
			get { return halfWidth; }
		}

		public static ushort HighScore
		{
			get { return highScore; }
		}

		public static ushort CurrentHighScore
		{
			get { return currentHighScore; }
			set { currentHighScore = Math.Max(currentHighScore, value); }
		}

		// Override methods here for global logic
		protected override void InitPlugin()
		{
			base.InitPlugin();
			halfWidth = MathF.Max(DualityApp.TargetViewSize.X / 2, 600);

			highScore = 0;
			currentHighScore = 0;
			//loading highscore
			if (FileOp.Exists(HIGHSCORE_FILE))
			{
				try
				{
					using (Stream s = FileOp.Open(HIGHSCORE_FILE, FileAccessMode.Read))
					using (StreamReader sr = new StreamReader(s))
					{
						highScore = Convert.ToUInt16(sr.ReadLine());
					}
				}
				catch { }
			}
		}

		protected override void OnDisposePlugin()
		{
			base.OnDisposePlugin();
			if (currentHighScore > highScore)
			{
				try
				{
					using (Stream s = FileOp.Create(HIGHSCORE_FILE))
					using (StreamWriter sw = new StreamWriter(s))
					{
						sw.WriteLine(String.Format("{0}", currentHighScore));
					}
				}
				catch { }
			}
		}
	}
}