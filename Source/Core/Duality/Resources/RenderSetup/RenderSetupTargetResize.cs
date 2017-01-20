using System;

using Duality.Editor;
using Duality.Drawing;


namespace Duality.Resources
{
	/// <summary>
	/// Describes how a <see cref="RenderTarget"/> in a rendering setup will be rescaled to fit window- or screen resolution settings.
	/// </summary>
	public struct RenderSetupTargetResize
	{
		/// <summary>
		/// The <see cref="RenderTarget"/> resource to be resized.
		/// </summary>
		public ContentRef<RenderTarget> Target;
		/// <summary>
		/// The <see cref="TargetResize"/> mode that will be applied to the <see cref="RenderTarget"/>.
		/// 
		/// Usually, this should be set to <see cref="TargetResize.None"/>, <see cref="TargetResize.Stretch"/>
		/// or <see cref="TargetResize.Fit"/>.
		/// </summary>
		public TargetResize ResizeMode;
		/// <summary>
		/// An additional scale factor that will be applied to the target size after <see cref="ResizeMode"/>
		/// was taken into account.
		/// </summary>
		public Vector2 Scale;

		public override string ToString()
		{
			if (this.Scale == Vector2.Zero)
				return string.Format("{0}, scale to zero", this.Target.Name);
			else if (this.Scale == Vector2.One)
				return string.Format("{0}: {1}", this.Target.Name, this.ResizeMode);
			else if (this.Scale.X == this.Scale.Y)
				return string.Format("{0}: {1} x{2}", this.Target.Name, this.ResizeMode, this.Scale.X);
			else
				return string.Format("{0}: {1} x{2}", this.Target.Name, this.ResizeMode, this.Scale);
		}
	}
}
