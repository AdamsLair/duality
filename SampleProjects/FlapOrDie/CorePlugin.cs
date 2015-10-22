using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Resources;

namespace FlapOrDie
{
	/// <summary>
	/// Defines a Duality core plugin.
	/// </summary>
    public class FlapOrDieCorePlugin : CorePlugin
    {
        [DontSerialize]
        private static float halfWidth;

        public static float HalfWidth
        {
            get { return halfWidth; }
        }
		// Override methods here for global logic
        protected override void InitPlugin()
        {
            base.InitPlugin();
            halfWidth = MathF.Max(DualityApp.TargetResolution.X / 2, 600);
        }
    }
}
