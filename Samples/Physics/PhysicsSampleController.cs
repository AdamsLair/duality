using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Editor;
using Duality.Drawing;
using Duality.Input;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Physics;

namespace Duality.Samples.Physics
{
	[EditorHintCategory("PhysicsSample")]
    public class PhysicsSampleController : Component, ICmpInitializable, ICmpUpdatable, ICmpRenderer
	{
		private float dragForceFactor = 3.0f;
		private float dragDampingFactor = 1.0f;
		private ColorRgba defaultColor = ColorRgba.White;
		private ColorRgba interactionColor = ColorRgba.Black;

		[DontSerialize] private List<ContentRef<Scene>> sampleScenes;
		[DontSerialize] private bool dragObjWasContinuous;
		[DontSerialize] private RigidBody dragObj;
		[DontSerialize] private Vector2 dragAnchor;
		[DontSerialize] private Vector2 cameraDragScreenAnchor;
		[DontSerialize] private Vector3 cameraDragWorldAnchor;

		
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


		private void SwitchToSample(int index)
		{
			// Force reload of current sample later on by disposing it.
			this.GameObj.ParentScene.DisposeLater();
			// Switch to new sample
			Scene.SwitchTo(this.sampleScenes[index]);
		}
		private void AdvanceSampleBy(int indexOffset)
		{
			// Determine the current samples' index and advance it
			int currentIndex = this.sampleScenes.IndexOf(this.GameObj.ParentScene);
			int newIndex = (currentIndex + indexOffset + this.sampleScenes.Count) % this.sampleScenes.Count;
			this.SwitchToSample(newIndex);
		}


		void ICmpRenderer.GetCullingInfo(out CullingInfo info)
		{
			info.Position = Vector3.Zero;
			info.Radius = float.MaxValue;
			info.Visibility = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			Vector2 mousePos = DualityApp.Mouse.Pos;

			Canvas canvas = new Canvas();
			canvas.Begin(device);

			// Make sure we'll draw below the sample info text
			canvas.State.DepthOffset = 1.0f;

			// Draw drag anchor markers when dragging an object
			if (this.dragObj != null)
			{
				Camera mainCam = this.GameObj.ParentScene.FindComponent<Camera>();
				Transform dragTransform = this.dragObj.GameObj.Transform;
				Vector2 worldAnchor = dragTransform.GetWorldPoint(this.dragAnchor);
				Vector2 screenAnchor = mainCam.GetScreenPos(worldAnchor);
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

			canvas.End();
		}

		void ICmpUpdatable.OnUpdate()
		{
			// Determine the world position of the cursor
			Camera mainCamera = this.GameObj.ParentScene.FindComponent<Camera>();
			Vector2 screenMousePos = DualityApp.Mouse.Pos;
			Vector3 worldMousePos = mainCamera.GetWorldPos(screenMousePos);

			// Pressing the right mouse button: Starting a camera drag
			if (DualityApp.Mouse.ButtonHit(MouseButton.Right))
			{
				this.cameraDragScreenAnchor = screenMousePos;
				this.cameraDragWorldAnchor = mainCamera.GameObj.Transform.Pos;
			}

			// Holding the right mouse button: Adjusting the camera position
			if (DualityApp.Mouse[MouseButton.Right])
			{
				Vector3 cameraMovement = new Vector3(this.cameraDragScreenAnchor - screenMousePos);
				mainCamera.GameObj.Transform.MoveTo(this.cameraDragWorldAnchor + cameraMovement);
			}

			// Pressing the left mouse button: Picking up an object
			if (DualityApp.Mouse.ButtonHit(MouseButton.Left))
			{
				// Determine which shape is located at the cursor position
				List<ShapeInfo> shapesAtCursor = new List<ShapeInfo>();
				RigidBody.PickShapesGlobal(worldMousePos.Xy, shapesAtCursor);
				foreach (ShapeInfo shape in shapesAtCursor)
				{
					if (shape.IsSensor) continue;

					Transform dragTransform = shape.Parent.GameObj.Transform;
					this.dragObj = shape.Parent;
					this.dragAnchor = dragTransform.GetLocalPoint(worldMousePos.Xy);

					// Temporarily switch to continuous collision for dragged objects, as they
					// might go very fast while in user control. Continuous collision prevents
					// tunnelling in these cases.
					this.dragObjWasContinuous = this.dragObj.ContinousCollision;
					this.dragObj.ContinousCollision = true;

					break;
				}
			}

			// Releasing the left mouse button: Releasing an object
			if (DualityApp.Mouse.ButtonReleased(MouseButton.Left))
			{
				if (this.dragObj != null)
				{
					this.dragObj.ContinousCollision = this.dragObjWasContinuous;
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

				this.dragObj.ApplyWorldForce(dragForce + dampingForce, worldAnchor);
				this.dragObj.ApplyLocalForce(dampingAngularForce);
			}

			// Pressing an arrow key: Switch sample scenes
			if (DualityApp.Keyboard.KeyHit(Key.Left) || DualityApp.Keyboard.KeyHit(Key.Up))
			{
				this.AdvanceSampleBy(-1);
			}
			else if (DualityApp.Keyboard.KeyHit(Key.Right) || DualityApp.Keyboard.KeyHit(Key.Down))
			{
				this.AdvanceSampleBy(1);
			}

			// Pressing space bar: Toggle simulation pause mode
			if (DualityApp.Keyboard.KeyHit(Key.Space))
			{
				bool isPaused = Time.TimeMult == 0.0f;
				if (isPaused)
					Time.Resume();
				else
					Time.Freeze();
			}
		}

		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate && DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
			{
				// Retrieve a list of all available scenes to cycle through.
				this.sampleScenes = ContentProvider.GetAvailableContent<Scene>();
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) { }
	}
}
