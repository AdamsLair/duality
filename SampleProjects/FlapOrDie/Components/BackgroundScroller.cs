using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlapOrDie.Components
{
    public class BackgroundScroller : Component
    {
        private float speed;

        public float Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }

		private SpriteRenderer back;
		private SpriteRenderer middle;
		private SpriteRenderer front;

		public SpriteRenderer Back
		{
			get { return this.back; }
			set { this.back = value; }
		}

		public SpriteRenderer Middle
		{
			get { return this.middle; }
			set { this.middle = value; }
		}

		public SpriteRenderer Front
		{
			get { return this.front; }
			set { this.front = value; }
		}

        public void Update(float speed)
        {
			UpdatePosition(Front.GameObj, speed / 2);
			UpdatePosition(Middle.GameObj, speed / 4);
			UpdatePosition(Back.GameObj, speed / 8);
        }

		private void UpdatePosition(GameObject obj, float speed)
		{
			Vector3 pos = obj.Transform.Pos;
			float textureWidth = obj.GetComponent<SpriteRenderer>().SharedMaterial.Res.MainTexture.Res.Size.X;

			pos.X -= speed;
			if (pos.X < -textureWidth) pos.X += textureWidth;

			obj.Transform.Pos = pos;
		}
    }
}
