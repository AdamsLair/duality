using System;

using Duality.Editor;
using Duality.Drawing;
using Duality.Cloning;

namespace Duality.Components
{
	/// <summary>
	/// A Renderer usually gives its <see cref="GameObject"/> a visual appearance in space.
	/// However, in general it may render anything and isn't bound by any conceptual restrictions.
	/// </summary>
	[ManuallyCloned]
	[RequiredComponent(typeof(Transform))]
	public abstract class Renderer : Component, ICmpRenderer
	{
		private	VisibilityFlag	visibilityGroup	= VisibilityFlag.Group0;

		/// <summary>
		/// [GET / SET] A bitmask that informs about the set of visibility groups to which this Renderer
		/// belongs. Usually, a Renderer is considered visible to a <see cref="Duality.Components.Camera"/> if they
		/// share at least one mutual visibility group.
		/// </summary>
		public VisibilityFlag VisibilityGroup
		{
			get { return this.visibilityGroup; }
			set { this.visibilityGroup = value; }
		}
		/// <summary>
		/// [GET] The Renderers bounding radius, originating from the <see cref="GameObject">GameObjects</see> position.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public abstract float BoundRadius { get; }

		/// <summary>
		/// Performs the Renderers drawing operation.
		/// </summary>
		/// <param name="device"></param>
		public abstract void Draw(IDrawDevice device);
		/// <summary>
		/// Retrieves information that can be used to decide whether this <see cref="Renderer"/> could 
		/// be visible to any given observer or not.
		/// </summary>
		/// <param name="info"></param>
		public virtual void GetCullingInfo(out CullingInfo info)
		{
			info.Position = this.gameobj.Transform.Pos;
			info.Radius = this.BoundRadius;
			info.Visibility = this.visibilityGroup;
		}

		protected override void OnCopyDataTo(object targetObj, ICloneOperation operation)
		{
			base.OnCopyDataTo(targetObj, operation);
			Renderer target = targetObj as Renderer;

			target.visibilityGroup = this.visibilityGroup;
		}
	}
}
