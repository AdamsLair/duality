using Duality;
using Duality.Components.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMenu
{
	public class MenuChangeColor : MenuComponent
	{
		public override void DoAction()
		{
			base.DoAction();
			this.FadeTo(MathF.Rnd.NextColorRgba(), true);
		}
	}
}
