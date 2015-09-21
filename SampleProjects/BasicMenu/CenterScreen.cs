using Duality;
using Duality.Components;
using Duality.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMenu
{
    [RequiredComponent(typeof(Transform))]
    public class CenterScreen : Component, ICmpUpdatable, ICmpInitializable
    {
        [DontSerialize]
        private Vector3 centerScreen;

        void ICmpUpdatable.OnUpdate()
        {
            this.Center();
        }

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
            if(context == InitContext.Activate)
            {
                this.Center();
            }
        }

        void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
        {
            //nothing to do here
        }

        private void Center()
        {
            // Simply keep the associated GameObject centered in the middle of the screen
            centerScreen.Xy = DualityApp.TargetResolution / 2;
            this.GameObj.Transform.Pos = centerScreen;
        }
    }
}
