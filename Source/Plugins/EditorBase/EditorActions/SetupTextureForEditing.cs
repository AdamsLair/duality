using Duality.Resources;

namespace Duality.Editor.Plugins.Base.EditorActions
{
	public class SetupTextureForEditing : EditorSingleAction<Texture>
	{
		public override bool CanPerformOn(Texture texture)
		{
			return base.CanPerformOn(texture) && texture.Size == Point2.Zero;
		}

		public override void Perform(Texture texture)
		{
			texture.Size = new Point2(128, 128);
			texture.ReloadData();
		}
		
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextSetupObjectForEditing;
		}
	}
}
