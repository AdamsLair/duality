using System;

using Duality.Editor;
using Duality.Properties;
using Duality.Cloning;

namespace Duality.Components
{
	/// <summary>
	/// Keeps track of this objects linear and angular velocity by accumulating all
	/// movement (but not teleportation) of its <see cref="Transform"/> component.
	/// </summary>
	[ManuallyCloned]
	[EditorHintCategory(CoreResNames.CategoryNone)]
	[EditorHintImage(CoreResNames.ImageVelocityTracker)]
	[RequiredComponent(typeof(Transform))]
	public sealed class VelocityTracker : Component, ICmpUpdatable, ICmpInitializable
	{
		[DontSerialize] private Vector3 velocity      = Vector3.Zero;
		[DontSerialize] private float   angleVelocity = 0.0f;
		[DontSerialize] private Vector3 posDiff       = Vector3.Zero;
		[DontSerialize] private float   angleDiff     = 0.0f;
		[DontSerialize] private Vector3 lastPosition  = Vector3.Zero;
		[DontSerialize] private float   lastAngle     = 0.0f;


		/// <summary>
		/// [GET] The objects measured velocity in world space. The value is internally smoothed
		/// over several frames to filter out fluctuations due to framerate variations.
		/// </summary>
		public Vector3 Vel
		{
			get { return this.velocity; }
		}
		/// <summary>
		/// [GET] The objects measured angle / rotation velocity in world space, in radians.
		/// The value is internally smoothed over several frames to filter out fluctuations due
		/// to framerate variations.
		/// </summary>
		public float AngleVel
		{
			get { return this.angleVelocity; }
		}
		/// <summary>
		/// [GET] The objects measured continuous position change in world space between the last two frames.
		/// Note that this value can fluctuate depending on framerate variations during simulation.
		/// </summary>
		public Vector3 LastMovement
		{
			get { return this.posDiff; }
		}
		/// <summary>
		/// [GET] The objects measuredcontinuous angle / rotation change in world space between the last two frames.
		/// Note that this value can fluctuate depending on framerate variations during simulation.
		/// </summary>
		public float LastAngleMovement
		{
			get { return this.angleDiff; }
		}


		/// <summary>
		/// Resets the objects velocity value for next frame to zero, assuming the
		/// specified world space position as a basis for further movement.
		/// </summary>
		/// <param name="worldPos"></param>
		public void ResetVelocity(Vector3 worldPos)
		{
			this.lastPosition = worldPos;
		}
		/// <summary>
		/// Resets the objects angle velocity value for next frame to zero, assuming the
		/// specified world space angle as a basis for further rotation.
		/// </summary>
		/// <param name="worldAngle"></param>
		public void ResetAngleVelocity(float worldAngle)
		{
			this.lastAngle = worldAngle;
		}
		
		void ICmpUpdatable.OnUpdate()
		{
			// Calculate velocity values from last frames movement
			if (MathF.Abs(Time.TimeMult) > float.Epsilon)
			{
				Transform transform = this.GameObj.Transform;
				Vector3 pos = transform.Pos;
				float angle = transform.Angle;

				this.posDiff = pos - this.lastPosition;
				this.angleDiff = MathF.TurnDir(this.lastAngle, angle) * MathF.CircularDist(this.lastAngle, angle);

				Vector3 lastVelocity = this.posDiff / Time.TimeMult;
				float lastAngleVelocity = this.angleDiff / Time.TimeMult;

				this.velocity += (lastVelocity - this.velocity) * 0.25f * Time.TimeMult;
				this.angleVelocity += (lastAngleVelocity - this.angleVelocity) * 0.25f * Time.TimeMult;
				this.lastPosition = pos;
				this.lastAngle = angle;
			}
		}
		void ICmpInitializable.OnInit(InitContext context)
		{
			if (context == InitContext.Loaded)
			{
				Transform transform = this.GameObj.Transform;
				this.lastPosition = transform.Pos;
				this.lastAngle = transform.Angle;
			}
		}
		void ICmpInitializable.OnShutdown(ShutdownContext context) { }
		
		protected override void OnCopyDataTo(object targetObj, ICloneOperation operation)
		{
			base.OnCopyDataTo(targetObj, operation);
			VelocityTracker target = targetObj as VelocityTracker;
			target.lastPosition   = this.lastPosition;
			target.lastAngle      = this.lastAngle;
			target.posDiff        = this.posDiff;
			target.angleDiff      = this.angleDiff;
			target.velocity       = this.velocity;
			target.angleVelocity  = this.angleVelocity;
		}
	}
}
