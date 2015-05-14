using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Duality;
using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;
using Font = Duality.Resources.Font;


namespace Duality.Editor.Plugins.Base.PreviewGenerators
{
	public class SoundPreviewGenerator : PreviewGenerator<Sound>
	{
		public override void Perform(Sound obj, PreviewSoundQuery query)
		{
			base.Perform(obj, query);
			query.Result = obj.Clone() as Sound;
		}
	}
}
