using System;

using Duality.Properties;
using Duality.Editor;

namespace Duality.Components
{
	/// <summary>
	/// Makes this <see cref="GameObject"/> the 3d sound listener.
	/// </summary>
	[RequiredComponent(typeof(Transform))]
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategorySound)]
	[EditorHintImage(typeof(CoreRes), CoreResNames.ImageSoundListener)]
	public sealed class SoundListener : Component, ICmpInitializable
	{
		public void MakeCurrent()
		{
			if (!this.Active) return;
			DualityApp.Sound.Listener = this.GameObj;
		}

		void ICmpInitializable.OnInit(InitContext context)
		{
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Editor && context == InitContext.Activate)
				this.MakeCurrent();
		}
		void ICmpInitializable.OnShutdown(ShutdownContext context)
		{
		}
	}
}
