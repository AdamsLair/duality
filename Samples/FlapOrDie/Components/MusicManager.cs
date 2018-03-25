using Duality;
using Duality.Audio;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlapOrDie.Components
{
    public class MusicManager : Component, ICmpInitializable
    {
		private ContentRef<Sound> bgm;
		public ContentRef<Sound> Music
		{
			get { return this.bgm; }
			set { this.bgm = value; }
		}

		void ICmpInitializable.OnActivate()
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
			{
				SoundInstance music = DualityApp.Sound.PlaySound(this.bgm);
				music.Looped = true;
			}
		}

		void ICmpInitializable.OnDeactivate()
		{
			DualityApp.Sound.StopAll();
		}
	}
}
