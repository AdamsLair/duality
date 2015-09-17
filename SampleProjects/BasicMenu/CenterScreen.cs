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
    public class CenterScreen : Component, ICmpUpdatable
    {
        [DontSerialize]
        private Vector3 centerScreen;

        void ICmpUpdatable.OnUpdate()
        {
            centerScreen.Xy = DualityApp.TargetResolution / 2;
            this.GameObj.Transform.Pos = centerScreen;
        }
    }
}
