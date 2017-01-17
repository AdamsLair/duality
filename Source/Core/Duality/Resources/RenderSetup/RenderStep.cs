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
		private string                   id                = null;
		private bool                     defaultClearColor = false;
		private ColorRgba                clearColor        = ColorRgba.TransparentBlack;
		private float                    clearDepth        = 1.0f;
		private ClearFlag                clearFlags        = ClearFlag.All;
		private RenderMatrix             matrixMode        = RenderMatrix.WorldSpace;
		private VisibilityFlag           visibilityMask    = VisibilityFlag.AllGroups;
		private BatchInfo                input             = null;
		private TargetResize             inputResize       = TargetResize.None;
		private ContentRef<RenderTarget> output            = null;

		/// <summary>
		/// [GET / SET] An optional ID value that can be used to refer to a <see cref="RenderStep"/>
		/// in an abstract way, i.e. identifying it by its role for rendering, rather than as a
		/// concrete instance.
		/// </summary>
		public string Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		/// <summary>
		/// The input to use for rendering. This can for example be a <see cref="Duality.Resources.Texture"/> that
		/// has been rendered to before and is now bound to perform a postprocessing step. If this is null, the current
		/// <see cref="Duality.Resources.Scene"/> is used as input - which is usually the case in the first rendering step.
		/// </summary>
		public BatchInfo Input
		{
			get { return this.input; }
			set { this.input = value; }
		}
		/// <summary>
		/// The resize behavior that is used to fit the rectangular <see cref="Input"/> to the output viewport.
		/// </summary>
		public TargetResize InputResize
		{
			get { return this.inputResize; }
			set { this.inputResize = value; }
		}
		/// <summary>
		/// The output to render to in this step. If this is null, the screen is used as rendering target.
		/// </summary>
		public ContentRef<RenderTarget> Output
		{
			get { return this.output; }
			set { this.output = value; }
		}
		/// <summary>
		/// [GET / SET] When true, the <see cref="ClearColor"/> that is specified by this <see cref="RenderStep"/>
		/// will be replaced with the rendering environment's default color, e.g. the <see cref="Duality.Components.Camera"/> background
		/// color.
		/// </summary>
		public bool DefaultClearColor
		{
			get { return this.defaultClearColor; }
			set { this.defaultClearColor = value; }
		}
		/// <summary>
		/// [GET / SET] The clear color to apply when clearing the color buffer.
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
		/// [GET / SET] Specifies which buffers to clean before rendering this step.
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
		/// [GET / SET] A step-local bitmask flagging all visibility groups that are considered visible to the active drawing device.
		/// </summary>
		public VisibilityFlag VisibilityMask
		{
			get { return this.visibilityMask; }
			set { this.visibilityMask = value; }
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
