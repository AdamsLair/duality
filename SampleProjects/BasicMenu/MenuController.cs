using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Components.Renderers;

namespace BasicMenu
{
	[RequiredComponent(typeof(Camera))]
	public abstract class MenuController : Component
	{
		private MenuPage startingMenu;

		[DontSerialize]
		protected MenuPage currentMenu;

		/// <summary>
		/// [GET / SET] The starting MenuPage that should be displayed when the Scene opens.
		/// </summary>
		public MenuPage StartingMenu
		{
			get { return this.startingMenu; }
			set { this.startingMenu = value; }
		}

		public void SwitchToMenu(MenuPage page)
		{
			if (this.currentMenu != null)
			{
				this.currentMenu.GameObj.Active = false;
			}

			page.GameObj.Active = true;
			this.currentMenu = page;
		}
	}
}
