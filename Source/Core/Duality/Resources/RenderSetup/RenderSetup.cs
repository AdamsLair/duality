using System;
using System.Linq;
using System.Collections.Generic;

using Duality.Editor;
using Duality.Properties;
using Duality.Backend;
using Duality.Drawing;


namespace Duality.Resources
{
	[ExplicitResourceReference(typeof(RenderTarget), typeof(Texture), typeof(Material))]
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageRenderSetup)]
	public class RenderSetup : Resource
	{
		/// <summary>
		/// The default rendering setup with one world-space step and one screen-space overlay step.
		/// </summary>
		public static ContentRef<RenderSetup> Default { get; private set; }

		internal static void InitDefaultContent()
		{
			RenderSetup defaultSetup = new RenderSetup();
			defaultSetup.Steps.Add(new RenderStep
			{
				Id = "World"
			});
			defaultSetup.Steps.Add(new RenderStep
			{
				Id = "ScreenOverlay",
				MatrixMode = RenderMatrix.ScreenSpace,
				ClearFlags = ClearFlag.None,
				VisibilityMask = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay
			});

			InitDefaultContent<RenderSetup>(new Dictionary<string,RenderSetup>
			{
				{ "Default", defaultSetup },
			});
		}

		private List<RenderStep> steps = new List<RenderStep>();

		/// <summary>
		/// [GET / SET] A set of rendering steps that describes the rendering process. Is never null nor empty.
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		public List<RenderStep> Steps
		{
			get { return this.steps; }
			set 
			{ 
				if (value != null)
					this.steps = value.Select(v => v ?? new RenderStep()).ToList();
				else
					this.steps = new List<RenderStep>();
			}
		}
	}
}
