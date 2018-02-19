using System;

using Duality.Editor;
using Duality.Properties;
using Duality.Cloning;

namespace Duality.Components
{
	/// <summary>
	/// Represents the location, rotation and scale of a <see cref="GameObject"/>, relative to its <see cref="GameObject.Parent"/>.
	/// </summary>
	[ManuallyCloned]
	[EditorHintCategory(CoreResNames.CategoryNone)]
	[EditorHintImage(CoreResNames.ImageTransform)]
	public sealed class Transform : Component, ICmpInitializable
	{
		private const float MinScale = 0.0000001f;

		private Vector3   pos             = Vector3.Zero;
		private float     angle           = 0.0f;
		private float     scale           = 1.0f;
		private bool      ignoreParent    = false;

		// Cached values, recalc on change
		private Vector3   posAbs          = Vector3.Zero;
		private float     angleAbs        = 0.0f;
		private float     scaleAbs        = 1.0f;

		[DontSerialize] private Vector2 rotationDirAbs = new Vector2(0.0f, -1.0f);


		/// <summary>
		/// [GET / SET] The objects position in local space of its parent object.
		/// </summary>
		public Vector3 LocalPos
		{
			get { return this.pos; }
			set
			{ 
				// Update position
				this.pos = value;
				this.UpdateAbs();
				this.ResetVelocity();
			}
		}
		/// <summary>
		/// [GET / SET] The objects angle / rotation in local space of its parent object, in radians.
		/// </summary>
		public float LocalAngle
		{
			get { return this.angle; }
			set 
			{ 
				// Update angle
				this.angle = MathF.NormalizeAngle(value);
				this.UpdateAbs();
				this.ResetAngleVelocity();
			}
		}
		/// <summary>
		/// [GET / SET] The objects scale in local space of its parent object.
		/// </summary>
		public float LocalScale
		{
			get { return this.scale; }
			set
			{
				this.scale = MathF.Max(value, MinScale);
				this.UpdateAbs();
			}
		}
		/// <summary>
		/// [GET / SET] Specifies whether the <see cref="Transform"/> component should behave as if 
		/// it was part of a root object. When true, it behaves the same as if it didn't have a 
		/// parent <see cref="Transform"/>.
		/// </summary>
		public bool IgnoreParent
		{
			get { return this.ignoreParent; }
			set
			{
				if (this.ignoreParent != value)
				{
					this.ignoreParent = value;
					this.UpdateRel();
				}
			}
		}

		/// <summary>
		/// [GET] The objects directional forward vector in local space of its parent object.
		/// </summary>
		public Vector3 LocalForward
		{
			get 
			{ 
				return new Vector3(
					MathF.Sin(this.LocalAngle),
					-MathF.Cos(this.LocalAngle),
					0.0f);
			}
		}
		/// <summary>
		/// [GET] The objects directional right vector in local space of its parent object.
		/// </summary>
		public Vector3 LocalRight
		{
			get 
			{
				return new Vector3(
					-MathF.Cos(this.LocalAngle),
					-MathF.Sin(this.LocalAngle),
					0.0f);
			}
		}
		
		/// <summary>
		/// [GET / SET] The objects position in world space.
		/// </summary>
		public Vector3 Pos
		{
			get { return this.posAbs; }
			set 
			{ 
				// Update position
				this.posAbs = value;

				Transform parent = this.ParentTransform;
				if (parent != null)
				{
					this.pos = this.posAbs;
					Vector3.Subtract(ref this.pos, ref parent.posAbs, out this.pos);
					Vector3.Divide(ref this.pos, parent.scaleAbs, out this.pos);
					MathF.TransformCoord(ref this.pos.X, ref this.pos.Y, -parent.angleAbs);
				}
				else
				{
					this.pos = this.posAbs;
				}

				this.UpdateAbsChild();
				this.ResetVelocity();
			}
		}
		/// <summary>
		/// [GET / SET] The objects angle / rotation in world space, in radians.
		/// </summary>
		public float Angle
		{
			get { return this.angleAbs; }
			set 
			{ 
				// Update angle
				this.angleAbs = MathF.NormalizeAngle(value);

				Transform parent = this.ParentTransform;
				if (parent != null)
					this.angle = MathF.NormalizeAngle(this.angleAbs - parent.angleAbs);
				else
					this.angle = this.angleAbs;

				this.UpdateRotationDirAbs();
				this.UpdateAbsChild();
				this.ResetAngleVelocity();
			}
		}
		/// <summary>
		/// [GET / SET] The objects scale in world space.
		/// </summary>
		public float Scale
		{
			get { return this.scaleAbs; }
			set 
			{ 
				this.scaleAbs = MathF.Max(value, MinScale);

				Transform parent = this.ParentTransform;
				if (parent != null)
					this.scale = this.scaleAbs / parent.scaleAbs;
				else
					this.scale = value;

				this.UpdateAbsChild();
			}
		}

		/// <summary>
		/// [GET] The objects directional forward (zero degree angle) vector in world space.
		/// </summary>
		public Vector3 Forward
		{
			get 
			{ 
				return new Vector3(
					this.rotationDirAbs.X,
					this.rotationDirAbs.Y,
					0.0f);
			}
		}
		/// <summary>
		/// [GET] The objects directional right (90 degree angle) vector in world space.
		/// </summary>
		public Vector3 Right
		{
			get 
			{
				return new Vector3(
					-this.rotationDirAbs.Y,
					this.rotationDirAbs.X,
					0.0f);
			}
		}

		private Transform ParentTransform
		{
			get
			{
				if (this.ignoreParent) return null;
				if (this.gameobj == null) return null;

				GameObject parent = this.gameobj.Parent;
				if (parent == null) return null;

				return parent.Transform;
			}
		}


		/// <summary>
		/// Transforms a position from local space of this object to world space.
		/// </summary>
		/// <param name="local"></param>
		/// <returns></returns>
		public Vector3 GetWorldPoint(Vector3 local)
		{
			return new Vector3(
				local.X * this.scaleAbs * -this.rotationDirAbs.Y + local.Y * this.scaleAbs * -this.rotationDirAbs.X + this.posAbs.X,
				local.X * this.scaleAbs * this.rotationDirAbs.X + local.Y * this.scaleAbs * -this.rotationDirAbs.Y + this.posAbs.Y,
				local.Z * this.scaleAbs + this.posAbs.Z);
		}
		/// <summary>
		/// Transforms a position from local space of this object to world space.
		/// </summary>
		/// <param name="local"></param>
		/// <returns></returns>
		public Vector2 GetWorldPoint(Vector2 local)
		{
			return new Vector2(
				local.X * this.scaleAbs * -this.rotationDirAbs.Y + local.Y * this.scaleAbs * -this.rotationDirAbs.X + this.posAbs.X,
				local.X * this.scaleAbs * this.rotationDirAbs.X + local.Y * this.scaleAbs * -this.rotationDirAbs.Y + this.posAbs.Y);
		}
		/// <summary>
		/// Transforms a position from world space to local space of this object.
		/// </summary>
		/// <param name="world"></param>
		/// <returns></returns>
		public Vector3 GetLocalPoint(Vector3 world)
		{
			float inverseScale = 1f / this.scaleAbs;
			return new Vector3(
				((world.X - this.posAbs.X) * -this.rotationDirAbs.Y + (world.Y - this.posAbs.Y) * this.rotationDirAbs.X) * inverseScale,
				((world.X - this.posAbs.X) * -this.rotationDirAbs.X + (world.Y - this.posAbs.Y) * -this.rotationDirAbs.Y) * inverseScale,
				(world.Z - this.posAbs.Z) * inverseScale);
		}
		/// <summary>
		/// Transforms a position from world space to local space of this object.
		/// </summary>
		/// <param name="world"></param>
		/// <returns></returns>
		public Vector2 GetLocalPoint(Vector2 world)
		{
			float inverseScale = 1f / this.scaleAbs;
			return new Vector2(
				((world.X - this.posAbs.X) * -this.rotationDirAbs.Y + (world.Y - this.posAbs.Y) * this.rotationDirAbs.X) * inverseScale,
				((world.X - this.posAbs.X) * -this.rotationDirAbs.X + (world.Y - this.posAbs.Y) * -this.rotationDirAbs.Y) * inverseScale);
		}

		/// <summary>
		/// Transforms a vector from local space of this object to world space.
		/// Does only take scale and rotation into account, but not position.
		/// </summary>
		/// <param name="local"></param>
		/// <returns></returns>
		public Vector3 GetWorldVector(Vector3 local)
		{
			return new Vector3(
				local.X * this.scaleAbs * -this.rotationDirAbs.Y + local.Y * this.scaleAbs * -this.rotationDirAbs.X,
				local.X * this.scaleAbs * this.rotationDirAbs.X + local.Y * this.scaleAbs * -this.rotationDirAbs.Y,
				local.Z * this.scaleAbs);
		}
		/// <summary>
		/// Transforms a vector from local space of this object to world space.
		/// Does only take scale and rotation into account, but not position.
		/// </summary>
		/// <param name="local"></param>
		/// <returns></returns>
		public Vector2 GetWorldVector(Vector2 local)
		{
			return new Vector2(
				local.X * this.scaleAbs * -this.rotationDirAbs.Y + local.Y * this.scaleAbs * -this.rotationDirAbs.X,
				local.X * this.scaleAbs * this.rotationDirAbs.X + local.Y * this.scaleAbs * -this.rotationDirAbs.Y);
		}
		/// <summary>
		/// Transforms a vector from world space to local space of this object.
		/// Does only take scale and rotation into account, but not position.
		/// </summary>
		/// <param name="world"></param>
		/// <returns></returns>
		public Vector3 GetLocalVector(Vector3 world)
		{
			float inverseScale = 1f / this.scaleAbs;
			return new Vector3(
				(world.X * -this.rotationDirAbs.Y + world.Y * this.rotationDirAbs.X) * inverseScale,
				(world.X * -this.rotationDirAbs.X + world.Y * -this.rotationDirAbs.Y) * inverseScale,
				world.Z * inverseScale);
		}
		/// <summary>
		/// Transforms a vector from world space to local space of this object.
		/// Does only take scale and rotation into account, but not position.
		/// </summary>
		/// <param name="world"></param>
		/// <returns></returns>
		public Vector2 GetLocalVector(Vector2 world)
		{
			float inverseScale = 1f / this.scaleAbs;
			return new Vector2(
				(world.X * -this.rotationDirAbs.Y + world.Y * this.rotationDirAbs.X) * inverseScale,
				(world.X * -this.rotationDirAbs.X + world.Y * -this.rotationDirAbs.Y) * inverseScale);
		}

		/// <summary>
		/// Moves the object by the given local offset. This will be treates as movement, rather than teleportation.
		/// </summary>
		/// <param name="value"></param>
		public void MoveByLocal(Vector3 value)
		{
			this.pos += value; 
			this.UpdateAbs();
		}
		/// <summary>
		/// Moves the object by the given local offset. This will be treates as movement, rather than teleportation.
		/// </summary>
		/// <param name="value"></param>
		public void MoveByLocal(Vector2 value)
		{
			this.MoveByLocal(new Vector3(value));
		}
		/// <summary>
		/// Moves the object by given world offset. This will be treates as movement, rather than teleportation.
		/// </summary>
		/// <param name="value"></param>
		public void MoveBy(Vector3 value)
		{
			this.posAbs += value;

			Transform parent = this.ParentTransform;
			if (parent != null)
			{
				this.pos = this.posAbs;
				Vector3.Subtract(ref this.pos, ref parent.posAbs, out this.pos);
				Vector3.Divide(ref this.pos, parent.scaleAbs, out this.pos);
				MathF.TransformCoord(ref this.pos.X, ref this.pos.Y, -parent.angleAbs);
			}
			else
			{
				this.pos = this.posAbs;
			}

			this.UpdateAbsChild();
		}
		/// <summary>
		/// Moves the object by given world offset. This will be treates as movement, rather than teleportation.
		/// </summary>
		/// <param name="value"></param>
		public void MoveBy(Vector2 value)
		{
			this.MoveBy(new Vector3(value));
		}
		/// <summary>
		/// Moves the object to the given position in local space of its parent object. This will be treates as movement, rather than teleportation.
		/// </summary>
		/// <param name="value"></param>
		public void MoveToLocal(Vector3 value)
		{
			this.MoveByLocal(value - this.pos);
		}
		/// <summary>
		/// Moves the object to the given position in local space of its parent object, leaving the Z coordinate unchanged.
		/// This will be treates as movement, rather than teleportation.
		/// </summary>
		/// <param name="value"></param>
		public void MoveToLocal(Vector2 value)
		{
			this.MoveToLocal(new Vector3(value, this.pos.Z));
		}
		/// <summary>
		/// Moves the object to the given world position. This will be treates as movement, rather than teleportation.
		/// </summary>
		/// <param name="value"></param>
		public void MoveTo(Vector3 value)
		{
			this.MoveBy(value - this.posAbs);
		}
		/// <summary>
		/// Moves the object to the given world position, leaving the Z coordinate unchanged.
		/// This will be treates as movement, rather than teleportation.
		/// </summary>
		/// <param name="value"></param>
		public void MoveTo(Vector2 value)
		{
			this.MoveTo(new Vector3(value, this.posAbs.Z));
		}
		/// <summary>
		/// Turns the object by the given radian angle. This will be treates as movement, rather than teleportation.
		/// </summary>
		/// <param name="value"></param>
		public void TurnBy(float value)
		{
			this.angle = MathF.NormalizeAngle(this.angle + value);
			this.UpdateAbs();
		}
		/// <summary>
		/// Turns the object to the given radian angle in local space of its parent object. This will be treates as movement, rather than teleportation.
		/// </summary>
		/// <param name="value"></param>
		public void TurnToLocal(float value)
		{
			this.TurnBy(MathF.TurnDir(this.angle, value) * MathF.CircularDist(value, this.angle));
		}
		/// <summary>
		/// Turns the object to the given world space radian angle. This will be treates as movement, rather than teleportation.
		/// </summary>
		/// <param name="value"></param>
		public void TurnTo(float value)
		{
			this.TurnBy(MathF.TurnDir(this.angleAbs, value) * MathF.CircularDist(value, this.angleAbs));
		}

		/// <summary>
		/// Updates the Transforms world space data all at once. This change is
		/// not regarded as a continuous movement, but as a hard teleport.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="angle"></param>
		/// <param name="scale"></param>
		/// <param name="angleVel"></param>
		public void SetTransform(Vector3 pos, float angle, float scale)
		{
			this.posAbs = pos;
			this.angleAbs = angle;
			this.scaleAbs = scale;

			this.UpdateRel();
			this.UpdateAbsChild();

			this.ResetVelocity();
			this.ResetAngleVelocity();
		}
		/// <summary>
		/// Updates the Transforms world space data all at once. This change is
		/// not regarded as a continuous movement, but as a hard teleport.
		/// </summary>
		/// <param name="other"></param>
		public void SetTransform(Transform other)
		{
			if (other == this) return;
			this.SetTransform(other.Pos, other.Angle, other.Scale);
		}
		/// <summary>
		/// Updates the Transforms local space data all at once. This change is
		/// not regarded as a continuous movement, but as a hard teleport.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="angle"></param>
		/// <param name="scale"></param>
		/// <param name="angleVel"></param>
		public void SetLocalTransform(Vector3 pos, float angle, float scale)
		{
			this.pos = pos;
			this.angle = angle;
			this.scale = scale;

			this.UpdateAbs();

			this.ResetVelocity();
			this.ResetAngleVelocity();
		}
		/// <summary>
		/// Updates the Transforms local space data all at once. This change is
		/// not regarded as a continuous movement, but as a hard teleport.
		/// </summary>
		/// <param name="other"></param>
		public void SetLocalTransform(Transform other)
		{
			if (other == this) return;
			this.SetLocalTransform(other.LocalPos, other.LocalAngle, other.LocalScale);
		}
		
		void ICmpInitializable.OnInit(InitContext context)
		{
			if (context == InitContext.AddToGameObject ||
				context == InitContext.Loaded)
			{
				if (this.gameobj != null)
				{
					this.gameobj.EventParentChanged += this.gameobj_EventParentChanged;
					if (this.gameobj.Parent != null)
					{
						Transform parentTransform = this.gameobj.Parent.Transform;
						if (parentTransform == null)
							this.gameobj.Parent.EventComponentAdded += this.Parent_EventComponentAdded;
						else
							this.gameobj.Parent.EventComponentRemoving += this.Parent_EventComponentRemoving;
					}
				}
				this.UpdateRel();
			}

			// Recalculate values we didn't serialize
			if (context == InitContext.Loaded)
			{
				this.UpdateRotationDirAbs();
			}
		}
		void ICmpInitializable.OnShutdown(ShutdownContext context)
		{
			if (context == ShutdownContext.RemovingFromGameObject)
			{
				if (this.gameobj != null)
				{
					this.gameobj.EventParentChanged -= this.gameobj_EventParentChanged;
					if (this.gameobj.Parent != null)
					{
						this.gameobj.Parent.EventComponentAdded -= this.Parent_EventComponentAdded;
						this.gameobj.Parent.EventComponentRemoving -= this.Parent_EventComponentRemoving;
					}
				}
				this.UpdateRel();
			}
		}
		private void gameobj_EventParentChanged(object sender, GameObjectParentChangedEventArgs e)
		{
			this.UpdateRel();
		}
		private void Parent_EventComponentAdded(object sender, ComponentEventArgs e)
		{
			Transform cmpTransform = e.Component as Transform;
			if (cmpTransform != null)
			{
				cmpTransform.GameObj.EventComponentAdded -= this.Parent_EventComponentAdded;
				cmpTransform.GameObj.EventComponentRemoving += this.Parent_EventComponentRemoving;
				this.UpdateRel();
			}
		}
		private void Parent_EventComponentRemoving(object sender, ComponentEventArgs e)
		{
			Transform cmpTransform = e.Component as Transform;
			if (cmpTransform != null)
			{
				cmpTransform.GameObj.EventComponentAdded += this.Parent_EventComponentAdded;
				cmpTransform.GameObj.EventComponentRemoving -= this.Parent_EventComponentRemoving;
				this.UpdateRel();
			}
		}
		
		private void UpdateRotationDirAbs()
		{
			this.rotationDirAbs = new Vector2(
				MathF.Sin(this.angleAbs), 
				-MathF.Cos(this.angleAbs));
		}
		private void ResetVelocity()
		{
			if (this.gameobj == null) return;
			VelocityTracker tracker = this.gameobj.GetComponent<VelocityTracker>();
			if (tracker != null)
				tracker.ResetVelocity(this.posAbs);
		}
		private void ResetAngleVelocity()
		{
			if (this.gameobj == null) return;
			VelocityTracker tracker = this.gameobj.GetComponent<VelocityTracker>();
			if (tracker != null)
				tracker.ResetAngleVelocity(this.angleAbs);
		}

		private void UpdateAbs()
		{
			this.CheckValidTransform();

			Transform parent = this.ParentTransform;
			if (parent == null)
			{
				this.angleAbs = this.angle;
				this.posAbs = this.pos;
				this.scaleAbs = this.scale;
			}
			else
			{
				this.angleAbs = MathF.NormalizeAngle(this.angle + parent.angleAbs);
				this.scaleAbs = this.scale * parent.scaleAbs;
				this.posAbs = parent.GetWorldPoint(this.pos);
			}

			// Update cached values
			this.UpdateRotationDirAbs();

			// Update absolute children coordinates
			this.UpdateAbsChild();

			this.CheckValidTransform();
		}
		private void UpdateAbsChild()
		{
			this.CheckValidTransform();

			if (this.gameobj != null)
			{
				foreach (GameObject obj in this.gameobj.Children)
				{
					Transform transform = obj.Transform;
					if (transform == null) continue;
					if (transform.ignoreParent) continue;

					transform.UpdateAbs();
				}
			}

			this.CheckValidTransform();
		}
		private void UpdateRel()
		{
			this.CheckValidTransform();

			Transform parent = this.ParentTransform;
			if (parent == null)
			{
				this.angle = this.angleAbs;
				this.pos = this.posAbs;
				this.scale = this.scaleAbs;
			}
			else
			{
				this.angle = MathF.NormalizeAngle(this.angleAbs - parent.angleAbs);
				this.scale = this.scaleAbs / parent.scaleAbs;
				
				Vector2 parentAngleAbsDotX;
				Vector2 parentAngleAbsDotY;
				MathF.GetTransformDotVec(-parent.angleAbs, out parentAngleAbsDotX, out parentAngleAbsDotY);

				Vector3.Subtract(ref this.posAbs, ref parent.posAbs, out this.pos);
				MathF.TransformDotVec(ref this.pos, ref parentAngleAbsDotX, ref parentAngleAbsDotY);
				Vector3.Divide(ref this.pos, parent.scaleAbs, out this.pos);
			}

			this.CheckValidTransform();
		}
		
		protected override void OnCopyDataTo(object targetObj, ICloneOperation operation)
		{
			base.OnCopyDataTo(targetObj, operation);
			Transform target = targetObj as Transform;

			target.ignoreParent   = this.ignoreParent;

			target.pos            = this.pos;
			target.angle          = this.angle;
			target.scale          = this.scale;

			target.posAbs         = this.posAbs;
			target.angleAbs       = this.angleAbs;
			target.scaleAbs       = this.scaleAbs;
			target.rotationDirAbs = this.rotationDirAbs;
			
			// Update absolute transformation data, because the target is relative to a different parent.
			target.UpdateAbs();
		}

		[System.Diagnostics.Conditional("DEBUG")]
		internal void CheckValidTransform()
		{
			MathF.CheckValidValue(this.pos);
			MathF.CheckValidValue(this.scale);
			MathF.CheckValidValue(this.angle);

			MathF.CheckValidValue(this.posAbs);
			MathF.CheckValidValue(this.scaleAbs);
			MathF.CheckValidValue(this.angleAbs);
		}
	}
}
