using System;
using System.Linq;

using Duality.Editor;
using Duality.Drawing;
using Duality.Properties;
using Duality.Backend;


namespace Duality.Resources
{
	/// <summary>
	/// Describes a single rendering step in a <see cref="RenderSetup"/>.
	/// </summary>
	public class RenderStep
	{
		private ColorRgba                clearColor     = ColorRgba.TransparentBlack;
		private float                    clearDepth     = 1.0f;
		private ClearFlag                clearFlags     = ClearFlag.All;
		private RenderMatrix             matrixMode     = RenderMatrix.PerspectiveWorld;
		private VisibilityFlag           visibilityMask = VisibilityFlag.AllGroups;
		private BatchInfo                input          = null;
		private ContentRef<RenderTarget> output         = null;

		/// <summary>
		/// The input to use for rendering. This can for example be a <see cref="Duality.Resources.Texture"/> that
		/// has been rendered to before and is now bound to perform a postprocessing step. If this is null, the current
		/// <see cref="Duality.Resources.Scene"/> is used as input - which is usually the case in the first rendering pass.
		/// </summary>
		public BatchInfo Input
		{
			get { return this.input; }
			set { this.input = value; }
		}
		/// <summary>
		/// The output to render to in this pass. If this is null, the screen is used as rendering target.
		/// </summary>
		public ContentRef<RenderTarget> Output
		{
			get { return this.output; }
			set { this.output = value; }
		}
		/// <summary>
		/// [GET / SET] The clear color to apply when clearing the color buffer
		/// </summary>
		public ColorRgba ClearColor
		{
			get { return this.clearColor; }
			set { this.clearColor = value; }
		}
		/// <summary>
		/// [GET / SET] The clear depth to apply when clearing the depth buffer
		/// </summary>
		public float ClearDepth
		{
			get { return this.clearDepth; }
			set { this.clearDepth = value; }
		}
		/// <summary>
		/// [GET / SET] Specifies which buffers to clean before rendering this pass
		/// </summary>
		public ClearFlag ClearFlags
		{
			get { return this.clearFlags; }
			set { this.clearFlags = value; }
		}
		/// <summary>
		/// [GET / SET] How to set up the coordinate space before rendering
		/// </summary>
		public RenderMatrix MatrixMode
		{
			get { return this.matrixMode; }
			set { this.matrixMode = value; }
		}
		/// <summary>
		/// [GET / SET] A Pass-local bitmask flagging all visibility groups that are considered visible to this drawing device.
		/// </summary>
		public VisibilityFlag VisibilityMask
		{
			get { return this.visibilityMask; }
			set { this.visibilityMask = value; }
		}


		public void MakeAvailable()
		{
			this.output.MakeAvailable();
		}

		public override string ToString()
		{
			ContentRef<Texture> inputTex = input == null ? null : input.MainTexture;
			return string.Format("{0} => {1}{2}",
				inputTex.IsExplicitNull ? (input == null ? "World" : "Undefined") : inputTex.Name,
				output.IsExplicitNull ? "Screen" : output.Name,
				(this.visibilityMask & VisibilityFlag.ScreenOverlay) != VisibilityFlag.None ? " (Overlay)" : "");
		}
	}
}
