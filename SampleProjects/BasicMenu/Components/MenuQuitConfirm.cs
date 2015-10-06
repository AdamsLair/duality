using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMenu
{
	public class MenuQuitConfirm : MenuComponent
	{
		public override void DoAction()
		{
			base.DoAction();
			DualityApp.Terminate();
		}
	}
}
