using Duality;
using Duality.Components.Renderers;
using Duality.Drawing;

namespace BasicMenu
{
    [RequiredComponent(typeof(SpriteRenderer))]
    public abstract class MenuComponent : Component, ICmpUpdatable
    {
        private static readonly float MAX_FADE_TIME = 2;

        private ColorRgba hoverTint;

        [DontSerialize]
        private ColorRgba originalTint;

        [DontSerialize]
        private SpriteRenderer spriteRenderer;

        [DontSerialize]
        private ColorRgba startingTint;

        [DontSerialize]
        private ColorRgba targetTint;

        [DontSerialize]
        private Vector4 tintDelta;

        [DontSerialize]
        private float timeToFade;

        [DontSerialize]
        private float fadingTime;

        public MenuComponent()
        {
            hoverTint = ColorRgba.Red;
            fadingTime = timeToFade = MAX_FADE_TIME;
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

            if (fadingTime < timeToFade)
            {
                if(fadingTime + lastDelta >= timeToFade)
                {
                    this.spriteRenderer.ColorTint = this.targetTint;
                    fadingTime = timeToFade;
                }
                else
                {
                    ColorRgba currentTint = this.spriteRenderer.ColorTint;

                    Vector4 newTint = ColorToVector(currentTint);
                    newTint += (this.tintDelta * lastDelta);

                    VisualLog.Default.DrawText(0, 0, this.spriteRenderer.ColorTint.ToString());
                    this.spriteRenderer.ColorTint = VectorToColor(newTint);
                    fadingTime += lastDelta;
                }
            }

            this.timeToFade = MathF.Min(this.timeToFade + lastDelta, MAX_FADE_TIME);
        }

        public void MouseEnter()
        {
            if (this.spriteRenderer != null)
            {
                if (this.originalTint == default(ColorRgba))
                {
                    this.originalTint = this.spriteRenderer.ColorTint;
                }

                //this.spriteRenderer.ColorTint = this.hoverTint;
                FadeTo(this.hoverTint);
            }
        }

        public void MouseLeave()
        {
            if (this.originalTint != null && this.spriteRenderer != null)
            {
                //this.spriteRenderer.ColorTint = this.originalTint;
                FadeTo(this.originalTint);
            }
        }

        private void FadeTo(ColorRgba targetTint)
        {
            this.startingTint = this.spriteRenderer.ColorTint;
            this.targetTint = targetTint;

            Vector4 delta = ColorToVector(targetTint) - ColorToVector(startingTint);
            this.tintDelta = delta / this.fadingTime;

            this.timeToFade = fadingTime;
            this.fadingTime = 0;
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