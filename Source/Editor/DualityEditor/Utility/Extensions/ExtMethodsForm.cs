using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Windows7.DesktopIntegration;
using Windows7.DesktopIntegration.WindowsForms;

namespace Duality.Editor
{
	/// <summary>
	/// Provides extension methods for <see cref="System.Windows.Forms.Form"/>.
	/// </summary>
	public static class ExtMethodsForm
	{   
		/// <summary>
		/// Shows the <see cref="Form"/> centered on its parent, even though it
		/// isn't modal. (The default <see cref="Form.Show(IWin32Window)"/> method 
		/// ignores <see cref="FormStartPosition.CenterParent"/> settings.)
		/// </summary>
		/// <param name="form"></param>
		/// <param name="parent"></param>
		public static void ShowCentered(this Form form, Form parent)
		{
			form.StartPosition = FormStartPosition.Manual;
			form.Location = new Point(
				parent.Location.X + (parent.Width - form.Width) / 2, 
				parent.Location.Y + (parent.Height - form.Height) / 2);
			form.Show(parent);
		}
        /// <summary>
        /// Draws the specified overlay icon over this form's taskbar button.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="icon">The overlay icon.</param>
        /// <param name="description">The overlay icon's description.</param>
        public static void SetTaskbarOverlayIcon(this Form form, Icon icon, string description)
        {
			try
			{
				WindowsFormsExtensions.SetTaskbarOverlayIcon(form, icon, description);
			}
			catch (Exception) {} // Library might fail in Windows Vista and before
        }
        /// <summary>
        /// Sets the progress bar in the containing form's taskbar button
        /// to this progress bar's progress.
        /// </summary>
        /// <param name="progressBar">The progress bar.</param>
        public static void SetTaskbarProgress(this ProgressBar progressBar)
        {
			try
			{
				WindowsFormsExtensions.SetTaskbarProgress(progressBar);
			}
			catch (Exception) {} // Library might fail in Windows Vista and before
        }
        /// <summary>
        /// Sets the progress bar in the containing form's taskbar button
        /// to this toolstrip progress bar's progress.
        /// </summary>
        /// <param name="progressBar">The progress bar.</param>
        public static void SetTaskbarProgress(this ToolStripProgressBar progressBar)
        {
			try
			{
				WindowsFormsExtensions.SetTaskbarProgress(progressBar);
			}
			catch (Exception) {} // Library might fail in Windows Vista and before
        }
        /// <summary>
        /// Sets the progress bar of this form's taskbar button to the
        /// specified percentage.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="percent">The progress percentage.</param>
        public static void SetTaskbarProgress(this Form form, float percent)
        {
			try
			{
				WindowsFormsExtensions.SetTaskbarProgress(form, percent);
			}
			catch (Exception) {} // Library might fail in Windows Vista and before
        }
        /// <summary>
        /// Sets the progress bar of this form's taskbar button to the
        /// specified state.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="state">The taskbar progress state.</param>
        public static void SetTaskbarProgressState(this Form form, ThumbnailProgressState state)
        {
			try
			{
				WindowsFormsExtensions.SetTaskbarProgressState(form, (Windows7Taskbar.ThumbnailProgressState)state);
			}
			catch (Exception) {} // Library might fail in Windows Vista and before
        }
	}

	/// <summary>
    /// Represents the thumbnail progress bar state.
    /// </summary>
    public enum ThumbnailProgressState
    {
        /// <summary>
        /// No progress is displayed.
        /// </summary>
        NoProgress = Windows7Taskbar.ThumbnailProgressState.NoProgress,
        /// <summary>
        /// The progress is indeterminate (marquee).
        /// </summary>
        Indeterminate = Windows7Taskbar.ThumbnailProgressState.Indeterminate,
        /// <summary>
        /// Normal progress is displayed.
        /// </summary>
        Normal = Windows7Taskbar.ThumbnailProgressState.Normal,
        /// <summary>
        /// An error occurred (red).
        /// </summary>
        Error = Windows7Taskbar.ThumbnailProgressState.Error,
        /// <summary>
        /// The operation is paused (yellow).
        /// </summary>
        Paused = Windows7Taskbar.ThumbnailProgressState.Paused
    }
}
