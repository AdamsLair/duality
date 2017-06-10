using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Components.Physics;


namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// Provides an interface for <see cref="RigidBodyEditorTool"/> instances to access 
	/// <see="RigidBodyEditorCamViewState"/> internals and perform editing operations.
	/// </summary>
	public interface IRigidBodyEditorToolEnvironment
	{
		/// <summary>
		/// [GET / SET] The currently selected <see cref="RigidBodyEditorTool"/>.
		/// </summary>
		RigidBodyEditorTool SelectedTool { get; set; }
		/// <summary>
		/// [GET] The <see cref="RigidBody"/> that is currently being edited 
		/// by the <see cref="RigidBodyEditorCamViewState"/>.
		/// </summary>
		RigidBody ActiveBody { get; }
		/// <summary>
		/// [GET] The cursor-hovered local position relative to the current <see cref="ActiveBody"/>.
		/// </summary>
		Vector2 ActiveBodyPos { get; }
		/// <summary>
		/// [GET] The cursor-hovered world position on the Z-plane of the current <see cref="ActiveBody"/>.
		/// </summary>
		Vector3 ActiveWorldPos { get; }
		/// <summary>
		/// [GET] The world position to which cursor movement will be locked when using the
		/// axis lock editor feature.
		/// </summary>
		Vector3 LockedWorldPos { get; set; }
		/// <summary>
		/// [GET] Returns whether the key that was used to start this action is still pressed.
		/// </summary>
		bool IsActionKeyPressed { get; }

		/// <summary>
		/// Stops the currently active tool action. Call this when you want a custom tool action
		/// implementation to end without the user explicitly ending it.
		/// </summary>
		void EndToolAction();
		/// <summary>
		/// Selects the specified enumeration of <see cref="RigidBody"/> shapes in the editor.
		/// To clear the current shape selection, specify an empty or null shape enumerable
		/// with a <see cref="SelectMode"/> of <see cref="SelectMode.Set"/>.
		/// </summary>
		/// <param name="shapes"></param>
		/// <param name="mode"></param>
		void SelectShapes(IEnumerable<ShapeInfo> shapes, SelectMode mode = SelectMode.Set);

		/// <summary>
		/// Determines whether a world space with the given radius is visible in the
		/// <see cref="RigidBodyEditorCamViewState"/> view.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		bool IsCoordInView(Vector3 worldPos, float radius = 1.0f);
		/// <summary>
		/// Determins the view scale at a given world space Z position.
		/// </summary>
		/// <param name="z"></param>
		/// <returns></returns>
		float GetScaleAtZ(float z);
		/// <summary>
		/// Determins the world space position of a given point in screen space.
		/// The Z coordinate of that point will be evaluated as the assumed Z position.
		/// </summary>
		/// <param name="screenCoord"></param>
		/// <returns></returns>
		Vector3 GetSpaceCoord(Vector3 screenCoord);
		/// <summary>
		/// Determines the screen space position of a given point in world space.
		/// </summary>
		/// <param name="spaceCoord"></param>
		/// <returns></returns>
		Vector3 GetScreenCoord(Vector3 spaceCoord);
	}
}
