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
        public MenuController()
        { }

        public void SwitchToMenu(string menuName)
        {
            foreach(GameObject menuItem in this.GameObj.ParentScene.ActiveRootObjects.Where(obj => obj.Name.StartsWith("#Menu")))
            {
                menuItem.Active = false;
            }

            this.GameObj.ParentScene.FindGameObject(menuName, false).Active = true;
        }
    }
}
