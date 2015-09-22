using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMenu
{
    public class MenuSwitchToPage : MenuComponent
    {
        private MenuPage target;

        public MenuPage Target
        {
            get { return this.target; }
            set { this.target = value; }
        }

        public override void DoAction()
        {
            MenuController mc = this.GameObj.ParentScene.FindComponent<MenuController>();
            if (mc != null)
            {
                mc.SwitchToMenu(this.target);
            }
        }
    }
}
