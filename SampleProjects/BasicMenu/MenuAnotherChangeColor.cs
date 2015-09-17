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
        [DontSerialize]
        private Random rnd;

        public MenuChangeColor()
        {
            rnd = new Random();
        }

        public override void DoAction()
        {
            this.GameObj.GetComponent<SpriteRenderer>().ColorTint = rnd.NextColorRgba();
        }
    }
}
