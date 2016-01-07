namespace Duality.Editor.Plugins.Base.EditorActions
{
    public abstract class InitializeComponent<T> : EditorSingleAction<T> where T : Component
    {
        public override bool MatchesContext(string context)
        {
            return context == DualityEditorApp.ActionContextInitializeComponent;
        }
    }
}
