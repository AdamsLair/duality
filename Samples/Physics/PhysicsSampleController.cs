using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Editor;
using Duality.Drawing;
using Duality.Input;
using Duality.Components;
using Duality.Components.Physics;

namespace Duality.Samples.Physics
{
	[EditorHintCategory("PhysicsSample")]
    public class PhysicsSampleController : Component, ICmpUpdatable, ICmpRenderer
	{
		private float dragForceFactor = 3.0f;
		private float dragDampingFactor = 1.0f;
		private ColorRgba defaultColor = ColorRgba.White;
		private ColorRgba interactionColor = ColorRgba.Black;

		[DontSerialize] private RigidBody dragObj;
		[DontSerialize] private Vector2 dragAnchor;

		
		public float DragForceFactor
		{
			get { return this.dragForceFactor; }
			set { this.dragForceFactor = value; }
		}
		public float DragDampingFactor
		{
			get { return this.dragDampingFactor; }
			set { this.dragDampingFactor = value; }
		}
		public ColorRgba DefaultColor
		{
			get { return this.defaultColor; }
			set { this.defaultColor = value; }
		}
		public ColorRgba InteractionColor
		{
			get { return this.interactionColor; }
			set { this.interactionColor = value; }
		}
		float ICmpRenderer.BoundRadius
		{
			get { return float.MaxValue; }
		}


		bool ICmpRenderer.IsVisible(IDrawDevice device)
		{
			return 
				(device.VisibilityMask & VisibilityFlag.ScreenOverlay) != VisibilityFlag.None &&
				(device.VisibilityMask & VisibilityFlag.AllGroups) != VisibilityFlag.None;
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			Canvas canvas = new Canvas(device);
			Vector2 mousePos = DualityApp.Mouse.Pos;

			// Draw drag anchor markers when dragging an object
			if (this.dragObj != null)
			{
				Transform dragTransform = this.dragObj.GameObj.Transform;
				Vector2 worldAnchor = dragTransform.GetWorldPoint(this.dragAnchor);
				Vector2 screenAnchor = device.GetScreenCoord(worldAnchor).Xy;
				canvas.State.ColorTint = this.interactionColor;
				if ((screenAnchor - mousePos).Length < 10.0f)
					canvas.DrawLine(mousePos.X, mousePos.Y, screenAnchor.X, screenAnchor.Y);
				else
					canvas.DrawDashLine(mousePos.X, mousePos.Y, screenAnchor.X, screenAnchor.Y);
				canvas.FillCircle(screenAnchor.X, screenAnchor.Y, 3.0f);
				canvas.State.ColorTint = this.defaultColor;
			}

			// When the mouse is hovering over the game area and the system cursor 
			// is disabled, draw a custom cursor as a replacement
			if (!DualityApp.UserData.SystemCursorVisible && DualityApp.Mouse.IsAvailable)
			{
				canvas.State.ColorTint = (this.dragObj != null) ? this.interactionColor : this.defaultColor;
				canvas.FillThickLine(
					mousePos.X - 5, 
					mousePos.Y, 
					mousePos.X + 5, 
					mousePos.Y,
					2.0f);
				canvas.FillThickLine(
					mousePos.X, 
					mousePos.Y - 5, 
					mousePos.X, 
					mousePos.Y + 5,
					2.0f);
				canvas.State.ColorTint = this.defaultColor;
			}
		}

		void ICmpUpdatable.OnUpdate()
		{
			// Determine the world position of the cursor
			Camera mainCamera = this.GameObj.ParentScene.FindComponent<Camera>();
			Vector2 screenMousePos = DualityApp.Mouse.Pos;
			Vector3 worldMousePos = mainCamera.GetSpaceCoord(screenMousePos);

			// Pressing the left mouse button: Picking up an object
			if (DualityApp.Mouse.ButtonHit(MouseButton.Left))
			{
				// Determine which shape is located at the cursor position
				ShapeInfo shapeAtCursor = RigidBody.PickShapeGlobal(worldMousePos.Xy);
				if (shapeAtCursor != null)
				{
					Transform dragTransform = shapeAtCursor.Parent.GameObj.Transform;
					this.dragObj = shapeAtCursor.Parent;
					this.dragAnchor = dragTransform.GetLocalPoint(worldMousePos.Xy);
				}
			}

			// Releasing the left mouse button: Releasing an object
			if (DualityApp.Mouse.ButtonReleased(MouseButton.Left))
			{
				if (this.dragObj != null)
				{
					this.dragObj = null;
					this.dragAnchor = Vector2.Zero;
				}
			}

			// Continuously apply a force to the dragged object
			if (this.dragObj != null)
			{
				Transform dragTransform = this.dragObj.GameObj.Transform;
				Vector2 worldAnchor = dragTransform.GetWorldPoint(this.dragAnchor);
				Vector2 worldDiff = worldMousePos.Xy - worldAnchor;
				Vector2 dragForce = worldDiff.Normalized * MathF.Min(MathF.Sqrt(worldDiff.Length), 20.0f) * this.dragForceFactor;
				Vector2 dampingForce = -this.dragObj.LinearVelocity * this.dragDampingFactor;
				float dampingAngularForce = -this.dragObj.AngularVelocity * 2.0f * this.dragDampingFactor;

				Log.Game.Write("Drag Force: {0}", dragForce.Length);
				Log.Game.Write("Damping Force: {0}", dampingForce.Length);

				this.dragObj.ApplyWorldForce(dragForce + dampingForce, worldAnchor);
				this.dragObj.ApplyLocalForce(dampingAngularForce);
			}
		}
	}
}
