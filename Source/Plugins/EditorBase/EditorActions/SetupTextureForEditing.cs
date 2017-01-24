using Duality.Resources;

namespace Duality.Editor.Plugins.Base.EditorActions
{
	public class SetupTextureForEditing : EditorSingleAction<Texture>
	{
		public override bool CanPerformOn(Texture texture)
		{
			return base.CanPerformOn(texture) && texture.PixelWidth == 0 && texture.PixelHeight == 0;
		}

		public override void Perform(Texture texture)
		{
			texture.Size = new Vector2(128, 128);
		}
		
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextSetupObjectForEditing;
		}
	}
}
