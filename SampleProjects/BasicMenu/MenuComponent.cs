using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Components.Renderers;

namespace BasicMenu
{
	[RequiredComponent(typeof(SpriteRenderer))]
	public abstract class MenuComponent : Component
	{
        private ColorRgba hoverTint;

        [DontSerialize]
        private ColorRgba originalTint;
        [DontSerialize]
        private SpriteRenderer spriteRenderer;

        public ColorRgba HoverTint
        {
            get { return this.hoverTint; }
            set { this.hoverTint = value; }
        }

        public void MouseEnter()
        {
            if (this.spriteRenderer != null)
            {
                if (this.originalTint == null)
                {
                    this.originalTint = this.spriteRenderer.ColorTint;
                }

                this.spriteRenderer.ColorTint = this.hoverTint;
            }
        }

        public void MouseLeave()
        {
            if (this.originalTint != null && this.spriteRenderer != null)
            {
                this.spriteRenderer.ColorTint = this.originalTint;
            }
        }

        public abstract void DoAction();

        public Rect GetAreaOnScreen()
        {
            if(this.spriteRenderer == null)
            {
                this.spriteRenderer = this.GameObj.GetComponent<SpriteRenderer>();
            }

            // return the area only if it's currently drawn on the overlay - to simplify
            return (this.spriteRenderer.VisibilityGroup & VisibilityFlag.ScreenOverlay) != VisibilityFlag.None ? 
                this.spriteRenderer.Rect.WithOffset(this.GameObj.Transform.Pos.Xy) : 
                Rect.Empty;
        }
	}
}
