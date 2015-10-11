using Duality;
using Duality.Components.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMenu
{
	public class MenuChangeVolume : MenuComponent
	{
		private short changeAmount;

		public short ChangeAmount
		{
			get { return this.changeAmount; }
			set { this.changeAmount = value; }
		}

		public override void DoAction()
		{
			base.DoAction();

			float volume = DualityApp.UserData.SfxMasterVol;
			volume += (this.changeAmount / 10f);

			// make sure that the volume is between 0 and 1
			volume = MathF.Min(MathF.Max(volume, 0), 1);

			DualityApp.UserData.SfxMasterVol = volume;
		}
	}
}
