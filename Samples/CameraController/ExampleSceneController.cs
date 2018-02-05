using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Input;
using Duality.Components;
using Duality.Drawing;

namespace CameraController
{
	/// <summary>
	/// This class is in control of the example scene. It draws some debug information
	/// and makes sure that users can select a camera controller to use, etc.
	/// </summary>
	public class ExampleSceneController : Component, ICmpUpdatable, ICmpRenderer
	{
		private GameObject mainCameraObj = null;
		private GameObject targetObj = null;

		[DontSerialize] private List<ICameraController> cameraControllers = null;
		[DontSerialize] private int activeCamController = -1;
		[DontSerialize] private FormattedText infoText = null;
		[DontSerialize] private FormattedText stateText = null;
		[DontSerialize] private bool movementHistoryActive = false;
		[DontSerialize] private float movementHistoryTimer = 0.0f;

		/// <summary>
		/// [GET / SET] The main camera, on which sample camera controllers will be installed.
		/// </summary>
		public GameObject MainCameraObject
		{
			get { return this.mainCameraObj; }
			set { this.mainCameraObj = value; }
		}
		/// <summary>
		/// [GET / SET] The target object, which sample camera controllers will be configured to follow.
		/// </summary>
		public GameObject TargetObject
		{
			get { return this.targetObj; }
			set { this.targetObj = value; }
		}
		/// <summary>
		/// [GET / SET] The index of the currently active camera controller.
		/// </summary>
		public int ActiveCameraController
		{
			get { return this.activeCamController; }
			set
			{
				// Normalize the index
				if (value < -1) value = this.cameraControllers.Count - 1;
				if (value >= this.cameraControllers.Count) value = -1;

				// Apply the index
				this.activeCamController = value;

				// Remove the old camera controller
				ICameraController oldController = this.mainCameraObj.GetComponent<ICameraController>();
				if (oldController != null)
				{
					this.mainCameraObj.RemoveComponent(oldController as Component);
				}

				// Add the new camera controller, if one is selected
				ICameraController newController = null;
				if (this.activeCamController != -1)
				{
					newController = this.cameraControllers[this.activeCamController];
					newController.TargetObject = this.targetObj;
					this.mainCameraObj.AddComponent(newController as Component);
				}
			}
		}
		
		void ICmpUpdatable.OnUpdate()
		{
			// Prepare a list of camera controllers, if we don't already have one
			if (this.cameraControllers == null)
			{
				this.cameraControllers = new List<ICameraController>();

				// Use Reflection to get a list of all ICameraController classes
				TypeInfo[] availableCameraControllerTypes = DualityApp.GetAvailDualityTypes(typeof(ICameraController)).ToArray();
				foreach (TypeInfo camControllerType in availableCameraControllerTypes)
				{
					// Create an instance of each class
					ICameraController camController = camControllerType.CreateInstanceOf() as ICameraController;
					if (camController != null)
					{
						this.cameraControllers.Add(camController);
					}
				}
			}

			// Allow the user to select which camera controller to use
			if (DualityApp.Keyboard.KeyHit(Key.Number1))
				this.ActiveCameraController--;
			if (DualityApp.Keyboard.KeyHit(Key.Number2))
				this.ActiveCameraController++;
			if (DualityApp.Keyboard.KeyHit(Key.M))
				this.movementHistoryActive = !this.movementHistoryActive;

			// Is there a Gamepad we can use?
			GamepadInput gamepad = DualityApp.Gamepads.FirstOrDefault(g => g.IsAvailable);
			if (gamepad != null)
			{
				if (gamepad.ButtonHit(GamepadButton.A))
					this.ActiveCameraController--;
				if (gamepad.ButtonHit(GamepadButton.B))
					this.ActiveCameraController++;
				if (gamepad.ButtonHit(GamepadButton.X))
					this.movementHistoryActive = !this.movementHistoryActive;
			}

			// Every 100 ms, draw one visual log entry to document movement
			if (this.movementHistoryActive)
			{
				this.movementHistoryTimer += Time.MillisecondsPerFrame * Time.TimeMult;
				if (this.movementHistoryTimer > 100.0f)
				{
					this.movementHistoryTimer -= 100.0f;
					Vector2 targetPos = this.targetObj.Transform.Pos.Xy;
					Vector2 cameraPos = this.mainCameraObj.Transform.Pos.Xy;
					VisualLogs.Default.DrawPoint(
						targetPos.X,
						targetPos.Y,
						0.0f)
						.WithColor(new ColorRgba(255, 128, 0))
						.KeepAlive(3000.0f);
					VisualLogs.Default.DrawPoint(
						cameraPos.X,
						cameraPos.Y,
						0.0f)
						.WithColor(new ColorRgba(0, 255, 0))
						.KeepAlive(3000.0f);
				}
			}
		}
		void ICmpRenderer.GetCullingInfo(out CullingInfo info)
		{
			info.Position = Vector3.Zero;
			info.Radius = float.MaxValue;
			info.Visibility = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			Canvas canvas = new Canvas();
			canvas.Begin(device);
			
			Vector2 screenSize = device.TargetSize;
			ICameraController activeController = this.mainCameraObj.GetComponent<ICameraController>();
			VelocityTracker camTracker = this.mainCameraObj.GetComponent<VelocityTracker>();
			Transform camTransform = this.mainCameraObj.Transform;
			Transform targetTransform = this.targetObj.Transform;

			float camDist = (camTransform.Pos.Xy - targetTransform.Pos.Xy).Length;
			string activeControllerName = activeController != null ? activeController.GetType().Name : "None";
			activeControllerName = activeControllerName.Replace("CameraController", "");

			// Draw the screen center, so we know what exactly our camera controller is pointing at
			canvas.State.ColorTint = ColorRgba.Green.WithAlpha(0.5f);
			canvas.FillCircle(screenSize.X * 0.5f, screenSize.Y * 0.5f, 8.0f);

			// Draw the camera distance around the screen center
			canvas.State.ColorTint = ColorRgba.Green.WithAlpha(0.25f);
			canvas.DrawCircle(screenSize.X * 0.5f, screenSize.Y * 0.5f, camDist);

			// Draw the camera velocity (movement per second) around the screen center
			canvas.State.ColorTint = ColorRgba.Green.WithAlpha(0.5f);
			canvas.DrawLine(
				screenSize.X * 0.5f, 
				screenSize.Y * 0.5f, 
				screenSize.X * 0.5f + camTracker.Vel.X / Time.SecondsPerFrame,
				screenSize.Y * 0.5f + camTracker.Vel.Y / Time.SecondsPerFrame);

			// Draw some info text
			if (this.infoText == null)
			{
				this.infoText = new FormattedText();
				this.infoText.MaxWidth = 350;
			}
			this.infoText.SourceText = string.Format(
				"Camera Controller Sample/n/n" +
				"Use /c44AAFFFFarrow keys/cFFFFFFFF // /c44AAFFFFleft thumbstick/cFFFFFFFF to move./n" + 
				"Use /c44AAFFFFnumber keys 1, 2/cFFFFFFFF // /c44AAFFFFbuttons A, B/cFFFFFFFF to select a Camera Controller./n" + 
				"Use the /c44AAFFFFM key/cFFFFFFFF // /c44AAFFFFbutton X/cFFFFFFFF to toggle movement history./n/n" + 
				"Active Camera Controller:/n/cFF8800FF{0}/cFFFFFFFF",
				activeControllerName);

			canvas.State.ColorTint = ColorRgba.White;
			canvas.DrawText(this.infoText, 10, 10, 0.0f, null, Alignment.TopLeft, true);

			// Draw state information on the current camera controller
			if (this.stateText == null) this.stateText = new FormattedText();
			this.stateText.SourceText = string.Format(
				"Camera Distance: {0:F}/n" +
				"Camera Velocity: {1:F}, {2:F}",
				camDist,
				camTracker.Vel.X,
				camTracker.Vel.Y);

			canvas.State.ColorTint = ColorRgba.White;
			canvas.DrawText(this.stateText, 10, screenSize.Y - 10, 0.0f, null, Alignment.BottomLeft, true);

			canvas.End();
		}
	}
}
