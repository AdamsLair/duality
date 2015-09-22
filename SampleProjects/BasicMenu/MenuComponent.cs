using Duality;
using Duality.Components.Renderers;
using Duality.Drawing;

namespace BasicMenu
{
    [RequiredComponent(typeof(SpriteRenderer))]
    public abstract class MenuComponent : Component, ICmpUpdatable
    {
        private static readonly float MAX_FADE_TIME = .5f;

        private ColorRgba hoverTint;

        [DontSerialize]
        private ColorRgba originalTint;

        [DontSerialize]
        private SpriteRenderer spriteRenderer;

        /**
         * used for fade effect
         **/
        [DontSerialize]
        private ColorRgba targetTint;
        [DontSerialize]
        private Vector4 startingTint;
        [DontSerialize]
        private Vector4 tintDelta;
        [DontSerialize]
        private float timeToFade;
        [DontSerialize]
        private float fadingTime;
        [DontSerialize]
        private bool isFadingOut;
        /**
         * used for fade effect
         **/

        public MenuComponent()
        {
            this.hoverTint = ColorRgba.Red;
            this.fadingTime = MAX_FADE_TIME;
        }

        /// <summary>
        /// [GET/SET] The color tint that will be used when the mouse is hovering
        /// on the GameObject
        /// </summary>
        public ColorRgba HoverTint
        {
            get { return this.hoverTint; }
            set { this.hoverTint = value; }
        }

        public abstract void DoAction();

        /// <summary>
        /// This returns the area on screen that is currently occupied by the SpriteRenderer.
        /// For simplicity, this works only if it is set with the ScreenOverlay flag.
        /// 
        /// A full 2.5D implementation (no ScreenOverlay) would require more complex calculations.
        /// </summary>
        /// <returns></returns>
        public Rect GetAreaOnScreen()
        {
            if (this.spriteRenderer == null)
            {
                this.spriteRenderer = this.GameObj.GetComponent<SpriteRenderer>();
            }

            Rect result = Rect.Empty;

            // return the area only if it's currently drawn on the overlay
            if ((this.spriteRenderer.VisibilityGroup & VisibilityFlag.ScreenOverlay) != VisibilityFlag.None)
            {
                result = this.spriteRenderer.Rect.WithOffset(this.GameObj.Transform.Pos.Xy);
            }

            return result;
        }

        void ICmpUpdatable.OnUpdate()
        {
            // get the milliseconds elapsed since the last frame
            float lastDelta = Time.TimeMult * Time.MsPFMult / 1000;

            if (this.fadingTime < this.timeToFade)
            {
                // I'm still fading...
                if (this.fadingTime + lastDelta >= this.timeToFade)
                {
                    // ... but after this frame, I will stop. I can simply set the color as the target.
                    this.spriteRenderer.ColorTint = this.targetTint;

                    if(this.isFadingOut)
                    {
                        // since it was a fade out, I set fadingTime as the maximum allowed,
                        // so that the following fade in can take all the time it needs.
                        this.fadingTime = MAX_FADE_TIME;
                    }
                }
                else
                {
                    // ... and after this frame I will still be fading. Get the correct color.
                    Vector4 newTint = this.startingTint + (this.tintDelta * (this.fadingTime + lastDelta));
                    this.spriteRenderer.ColorTint = this.VectorToColor(newTint);
                }
            }

            this.fadingTime = MathF.Min(this.fadingTime + lastDelta, MAX_FADE_TIME);
        }

        public void MouseEnter()
        {
            if (this.spriteRenderer != null)
            {
                if (this.originalTint == default(ColorRgba))
                {
                    this.originalTint = this.spriteRenderer.ColorTint;
                }

                this.FadeTo(this.hoverTint, false);
            }
        }

        public void MouseLeave()
        {
            if (this.originalTint != default(ColorRgba) && this.spriteRenderer != null)
            {
                this.FadeTo(this.originalTint, true);
            }
        }

        protected void FadeTo(ColorRgba targetColor, bool fadeOut)
        {
            this.targetTint = targetColor;
            this.isFadingOut = fadeOut;

            this.startingTint = this.ColorToVector(this.spriteRenderer.ColorTint);

            Vector4 delta = this.ColorToVector(this.targetTint) - this.startingTint;

            // Here I use the time taken for the last fade operation as the time available for the new one.
            // This is because if I am fading in and move out the mouse before the fading is complete,
            // I want to take the same amount of time to revert back and not take MAX_FADE_TIME regardless
            // of the previous situation. If I'm fading out, I can revert it to MAX_FADE_TIME (see OnUpdate)
            this.timeToFade = this.fadingTime;
            this.fadingTime = 0;

            this.tintDelta = delta / this.timeToFade;
        }

        // Utility method to convert a ColorRgba to a Vector4 with values [0-1]
        private Vector4 ColorToVector(ColorRgba color)
        {
            return new Vector4(color.R, color.G, color.B, color.A) / 255f;
        }

        // Utility method to convert a Vector4 with values [0-1] to a ColorRgba
        private ColorRgba VectorToColor(Vector4 vector)
        {
            return new ColorRgba(vector.X, vector.Y, vector.Z, vector.W);
        }
    }
}