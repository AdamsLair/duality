using Duality;
using Duality.Components.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlapOrDie.Components
{
    public class Ticker : TextRenderer, ICmpUpdatable
    {
        private float speed;

        public float Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }

        private List<string> strings;

        public List<string> Strings
        {
            get { return this.strings; }
            set { this.strings = value; }
        }

        private Vector2 delta;

        private int index;

        void ICmpUpdatable.OnUpdate()
        {
            delta.X = this.speed * Time.MillisecondsPerFrame * Time.TimeMult / 1000;

            this.GameObj.Transform.MoveByLocal(-delta);
            if(this.GameObj.Transform.Pos.X + this.Text.TextMetrics.Size.X < -FlapOrDieCorePlugin.HalfWidth)
            {
                ShowNextString();
            }
        }

        private void ShowNextString()
        {
            this.Text.SourceText = this.strings[index];
            this.GameObj.Transform.LocalPos = new Vector3(FlapOrDieCorePlugin.HalfWidth + this.Text.TextMetrics.Size.X, 0, 0);

            index = (index + 1) % this.strings.Count;
        }
    }
}
