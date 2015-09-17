using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMenu
{
    public class MenuBackToMain : MenuComponent
    {
        public override void DoAction()
        {
            this.GameObj.ParentScene.FindComponent<MenuController>().SwitchToMenu("#MenuMain");
        }
    }
}
