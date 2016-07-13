using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Editor;
using Duality.Plugins.Tilemaps;
using Duality.Plugins.Tilemaps.Properties;
using Duality.Plugins.Tilemaps.Sample.RpgLike.Properties;

namespace Duality.Plugins.Tilemaps.Sample.RpgLike
{
	/// <summary>
	/// Moves the camera to follow an object, but keeps it within the constraints
	/// of the current tilemaps.
	/// </summary>
	[RequiredComponent(typeof(Transform))]
	[RequiredComponent(typeof(Camera))]
	[EditorHintCategory(SampleResNames.CategoryRpgLike)]
	public class CameraController : Component, ICmpUpdatable, ICmpInitializable
	{
		private float      smoothness = 1.0f;
		private GameObject targetObj  = null;

		[DontSerialize] private Rect mapRect;

		/// <summary>
		/// [GET / SET] How smooth the camera should follow its target.
		/// </summary>
		public float Smoothness
		{
			get { return this.smoothness; }
			set { this.smoothness = value; }
		}
		/// <summary>
		/// [GET / SET] The target object the camera should follow.
		/// </summary>
		public GameObject TargetObject
		{
			get { return this.targetObj; }
			set { this.targetObj = value; }
		}

		void ICmpUpdatable.OnUpdate()
		{
			// Early-out, if no target is specified
			if (this.targetObj == null) return;
			if (this.targetObj.Transform == null) return;

			Transform transform = this.GameObj.Transform;
			Camera camera = this.GameObj.GetComponent<Camera>();

			// Determine the rect in which the camera can move, given the
			// rect of the map and the one of the cameras visible area
			Rect moveRect;
			Vector3 camAreaTopLeft = camera.GetSpaceCoord(new Vector2(0.0f, 0.0f));
			Vector3 camAreaBottomRight = camera.GetSpaceCoord(DualityApp.TargetResolution);
			Rect camArea = new Rect(
				camAreaTopLeft.X, 
				camAreaTopLeft.Y, 
				camAreaBottomRight.X - camAreaTopLeft.X, 
				camAreaBottomRight.Y - camAreaTopLeft.Y);
			moveRect = new Rect(
				this.mapRect.X + camArea.W / 2,
				this.mapRect.Y + camArea.H / 2,
				MathF.Max(this.mapRect.W - camArea.W, 0.0f),
				MathF.Max(this.mapRect.H - camArea.H, 0.0f));
			if (moveRect.W == 0.0f) moveRect.X = this.mapRect.CenterX;
			if (moveRect.H == 0.0f) moveRect.Y = this.mapRect.CenterY;

			// The position to focus on.
			Vector3 focusPos = this.targetObj.Transform.Pos;
			focusPos.X = MathF.Clamp(focusPos.X, moveRect.X, moveRect.X + moveRect.W);
			focusPos.Y = MathF.Clamp(focusPos.Y, moveRect.Y, moveRect.Y + moveRect.H);
			// The position where the camera itself should move
			Vector3 targetPos = focusPos - new Vector3(0.0f, 0.0f, camera.FocusDist);
			// A relative movement vector that would place the camera directly at its target position.
			Vector3 posDiff = (targetPos - transform.Pos);
			// A relative movement vector that doesn't go all the way, but just a bit towards its target.
			Vector3 targetVelocity = posDiff * 0.1f * MathF.Pow(2.0f, -this.smoothness);

			// Move the camera
			transform.MoveByAbs(targetVelocity * Time.TimeMult);
		}
		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate)
			{
				// Find the constrained rectangle we're allowed to move in,
				// based on the rects of all the active tilemaps
				bool first = true;
				IEnumerable<ICmpTilemapRenderer> allTilemapRenderers = 
					this.GameObj.ParentScene.FindComponents<ICmpTilemapRenderer>();
				foreach (ICmpTilemapRenderer tilemapRenderer in allTilemapRenderers)
				{
					Transform transform = (tilemapRenderer as Component).GameObj.Transform;
					Vector3 pos = transform.Pos;
					Rect localRect = tilemapRenderer.LocalTilemapRect;
					Rect worldRect = localRect.WithOffset(pos.X, pos.Y);
					if (first)
					{
						this.mapRect = worldRect;
						first = false;
					}
					else
					{
						this.mapRect = this.mapRect.Intersection(worldRect);
					}
				}
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) { }
	}
}
