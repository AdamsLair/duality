using System;
using System.Linq;
using System.Collections.Generic;

using Duality.Editor;
using Duality.Properties;
using Duality.Backend;


namespace Duality.Resources
{
	[ExplicitResourceReference(typeof(RenderTarget), typeof(Texture), typeof(Material))]
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageRenderSetup)]
	public class RenderSetup : Resource
	{
		internal static void InitDefaultContent() { }

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
