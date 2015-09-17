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
	public abstract class MenuComponent : Component, ICmpUpdatable
	{
        private ColorRgba hoverTint;

        [DontSerialize]
        private ColorRgba originalTint;
        [DontSerialize]
        private SpriteRenderer spriteRenderer;
        [DontSerialize]
        private ColorRgba targetTint;
        [DontSerialize]
        private ColorRgba tintDelta;

        public ColorRgba HoverTint
        {
            get { return this.hoverTint; }
            set { this.hoverTint = value; }
        }

        public MenuComponent()
        {
            hoverTint = ColorRgba.Red;
        }

        private void FadeTo(ColorRgba targetColor)
        {

        }

        public void MouseEnter()
        {
            if (this.spriteRenderer != null)
            {
                if (this.originalTint == default(ColorRgba))
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

            Rect result = Rect.Empty;

            // return the area only if it's currently drawn on the overlay - to simplify
            if ((this.spriteRenderer.VisibilityGroup & VisibilityFlag.ScreenOverlay) != VisibilityFlag.None)
            {
                result = this.spriteRenderer.Rect.WithOffset(this.GameObj.Transform.Pos.Xy);
            }

            return result;
        }

        void ICmpUpdatable.OnUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
