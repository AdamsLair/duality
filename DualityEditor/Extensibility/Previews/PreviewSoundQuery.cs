using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

using Duality;
using Duality.Resources;

namespace Duality.Editor
{
	public class PreviewSoundQuery : PreviewQuery<Sound>
	{
		public PreviewSoundQuery(object src) : base(src) {}
	}
}
