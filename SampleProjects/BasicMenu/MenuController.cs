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
	public abstract class MenuController : Component, ICmpInitializable
	{
        private MenuPage startingMenu;

        [DontSerialize]
        private MenuPage currentMenu;

        public MenuPage StartingMenu
        {
            get { return this.startingMenu; }
            set { this.startingMenu = value; }
        }

        public MenuController()
        { }

        public void SwitchToMenu(MenuPage page)
        {
            if (this.currentMenu != null)
            {
                this.currentMenu.GameObj.Active = false;
            }

            page.GameObj.Active = true;
            this.currentMenu = page;
        }

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
            if(context == InitContext.Activate)
            {
                SwitchToMenu(this.startingMenu);
            }
        }

        void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
        {
            // nothing to do here
        }
    }
}
