using System;

using Duality.Editor;
using Duality.Drawing;

using OpenTK;

namespace Duality.Components
{
	/// <summary>
	/// A Renderer usually gives its <see cref="GameObject"/> a visual appearance in space.
	/// However, in general it may render anything and isn't bound by any conceptual restrictions.
	/// </summary>
	[Serializable]
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
		/// Determines if the Renderer is visible to the specified <see cref="IDrawDevice"/>.
		/// This is usually the case if they share at least one mutual <see cref="VisibilityGroup">visibility group</see>.
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		public virtual bool IsVisible(IDrawDevice device)
		{
			if ((device.VisibilityMask & VisibilityFlag.ScreenOverlay) != (this.visibilityGroup & VisibilityFlag.ScreenOverlay)) return false;
			if ((this.visibilityGroup & device.VisibilityMask & VisibilityFlag.AllGroups) == VisibilityFlag.None) return false;
			return device.IsCoordInView(this.gameobj.Transform.Pos, this.BoundRadius);
		}

		protected override void OnCopyTo(Component target, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(target, provider);
			Renderer t = target as Renderer;
			t.visibilityGroup	= this.visibilityGroup;
		}
	}
}
