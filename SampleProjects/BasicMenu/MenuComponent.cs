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

        public ColorRgba HoverTint
        {
            get { return this.hoverTint; }
            set { this.hoverTint = value; }
        }
        public abstract void DoAction();

        public Rect GetAreaOnScreen()
        {
            if (this.spriteRenderer == null)
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
            float lastDelta = Time.TimeMult * Time.MsPFMult / 1000;

            if (this.fadingTime < this.timeToFade)
            {
                if (this.fadingTime + lastDelta >= this.timeToFade)
                {
                    this.spriteRenderer.ColorTint = this.targetTint;

                    if(this.isFadingOut)
                    {
                        this.fadingTime = MAX_FADE_TIME;
                    }
                }
                else
                {
                    Vector4 newTint = this.startingTint + (this.tintDelta * this.fadingTime);
                    this.spriteRenderer.ColorTint = VectorToColor(newTint);
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

                FadeTo(this.hoverTint, false);
            }
        }

        public void MouseLeave()
        {
            if (this.originalTint != default(ColorRgba) && this.spriteRenderer != null)
            {
                FadeTo(this.originalTint, true);
            }
        }

        protected void FadeTo(ColorRgba targetColor, bool fadeOut)
        {
            this.targetTint = targetColor;
            this.isFadingOut = fadeOut;

            this.startingTint = ColorToVector(this.spriteRenderer.ColorTint);

            Vector4 delta = ColorToVector(this.targetTint) - this.startingTint;

            this.timeToFade = this.fadingTime;
            this.fadingTime = 0;

            this.tintDelta = delta / this.timeToFade;
        }

        private Vector4 ColorToVector(ColorRgba color)
        {
            return new Vector4(color.R, color.G, color.B, color.A) / 255f;
        }

        private ColorRgba VectorToColor(Vector4 vector)
        {
            return new ColorRgba(vector.X, vector.Y, vector.Z, vector.W);
        }
    }
}