using System;

using OpenTK;

using Duality.EditorHints;

namespace Duality.Components
{
	/// <summary>
	/// Represents a <see cref="GameObject">GameObjects</see> physical location in the world, relative to its <see cref="GameObject.Parent"/>.
	/// </summary>
	[Serializable]
	public sealed class Transform : Component, ICmpUpdatable, ICmpEditorUpdatable, ICmpGameObjectListener, ICmpInitializable
	{
		/// <summary>
		/// Flags that are used to specify, whether certain Properties have been changed.
		/// </summary>
		[Flags]
		public enum DirtyFlags
		{
			None		= 0x00,

			Pos			= 0x01,
			Angle		= 0x04,
			Scale		= 0x10,

			All			= Pos | Angle | Scale
		}

		private const float MinScale = 0.0000001f;

		private	Vector3	pos			= Vector3.Zero;
		private	float	angle		= 0.0f;
		private	float	scale		= 1.0f;
		private	bool	deriveAngle		= true;
		private	bool	ignoreParent	= false;

		// Cached values, recalc on change
		private	Transform	parentTransform	= null;
		private	Vector3		posAbs			= Vector3.Zero;
		private	float		angleAbs		= 0.0f;
		private	float		scaleAbs		= 1.0f;
		// Auto-calculated values
		private	Vector3		vel				= Vector3.Zero;
		private	Vector3		velAbs			= Vector3.Zero;
		private	float		angleVel		= 0.0f;
		private	float		angleVelAbs		= 0.0f;
		// Temporary per-frame values
		[NonSerialized] private	DirtyFlags	changes			= DirtyFlags.None;
		[NonSerialized] private	Vector3		tempVel			= Vector3.Zero;
		[NonSerialized] private	Vector3		tempVelAbs		= Vector3.Zero;
		[NonSerialized] private	float		tempAngleVel	= 0.0f;
		[NonSerialized] private	float		tempAngleVelAbs	= 0.0f;

		[NonSerialized]
		private EventHandler<TransformChangedEventArgs> eventTransformChanged = null;
		public event EventHandler<TransformChangedEventArgs> EventTransformChanged
		{
			add { this.eventTransformChanged += value; }
			remove { this.eventTransformChanged -= value; }
		}


		/// <summary>
		/// [GET / SET] The objects position relative to its parent object.
		/// </summary>
		public Vector3 RelativePos
		{
			get { return this.pos; }
			set
			{ 
				// Update position
				this.pos = value; 
				this.changes |= DirtyFlags.Pos; 
				this.UpdateAbs();
			}
		}
		/// <summary>
		/// [GET / SET] The objects velocity relative to its parent object.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public Vector3 RelativeVel
		{
			get { return this.vel; }
		}
		/// <summary>
		/// [GET / SET] The objects angle / rotation relative to its parent object, in radians.
		/// </summary>
		public float RelativeAngle
		{
			get { return this.angle; }
			set 
			{ 
				// Update angle
				this.angle = MathF.NormalizeAngle(value);
				this.changes |= DirtyFlags.Angle; 
				this.UpdateAbs();
			}
		}
		/// <summary>
		/// [GET / SET] The objects angle / rotation velocity relative to its parent object, in radians.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float RelativeAngleVel
		{
			get { return this.angleVel; }
		}
		/// <summary>
		/// [GET / SET] The objects scale relative to its parent object.
		/// </summary>
		public float RelativeScale
		{
			get { return this.scale; }
			set
			{
				this.scale = MathF.Max(value, MinScale);
				this.changes |= DirtyFlags.Scale; 
				this.UpdateAbs();
			}
		}
		/// <summary>
		/// [GET / SET] Specifies whether the Transform component should ignore its parent transform.
		/// </summary>
		public bool IgnoreParent
		{
			get { return this.ignoreParent; }
			set
			{
				if (this.ignoreParent != value)
				{
					this.ignoreParent = value;
					this.UpdateAbs();
				}
			}
		}
		/// <summary>
		/// [GET / SET] If false, this objects rotation values aren't relative to its parent.
		/// However, its position, velocity, etc. still depend on parent rotation.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public bool DeriveAngle
		{
			get { return this.deriveAngle; }
			set
			{
				this.deriveAngle = value;
				this.changes |= DirtyFlags.Angle;
				this.UpdateRel();
			}
		}

		/// <summary>
		/// [GET] The objects forward vector, relative to its parent object.
		/// </summary>
		public Vector3 RelativeForward
		{
			get 
			{ 
				return new Vector3(
					MathF.Sin(this.RelativeAngle),
					-MathF.Cos(this.RelativeAngle),
					0.0f);
			}
		}
		/// <summary>
		/// [GET] The objects right (directional) vector, relative to its parent object.
		/// </summary>
		public Vector3 RelativeRight
		{
			get 
			{
				return new Vector3(
					-MathF.Cos(this.RelativeAngle),
					-MathF.Sin(this.RelativeAngle),
					0.0f);
			}
		}
		
		/// <summary>
		/// [GET / SET] The objects position.
		/// </summary>
		public Vector3 Pos
		{
			get { return this.posAbs; }
			set 
			{ 
				// Update position
				this.posAbs = value;

				if (this.parentTransform != null)
				{
					this.pos = this.posAbs;
					Vector3.Subtract(ref this.pos, ref this.parentTransform.posAbs, out this.pos);
					Vector3.Divide(ref this.pos, this.parentTransform.scaleAbs, out this.pos);
					MathF.TransformCoord(ref this.pos.X, ref this.pos.Y, -this.parentTransform.angleAbs);
				}
				else
				{
					this.pos = this.posAbs;
				}

				this.changes |= DirtyFlags.Pos;
				this.UpdateAbsChild();
			}
		}
		/// <summary>
		/// [GET] The objects velocity.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public Vector3 Vel
		{
			get { return this.velAbs; }
		}
		/// <summary>
		/// [GET / SET] The objects angle / rotation, in radians.
		/// </summary>
		public float Angle
		{
			get { return this.angleAbs; }
			set 
			{ 
				// Update angle
				this.angleAbs = MathF.NormalizeAngle(value);

				if (this.parentTransform != null && this.deriveAngle)
					this.angle = MathF.NormalizeAngle(this.angleAbs - this.parentTransform.angleAbs);
				else
					this.angle = this.angleAbs;

				this.changes |= DirtyFlags.Angle;
				this.UpdateAbsChild();
			}
		}
		/// <summary>
		/// [GET] The objects angle / rotation velocity, in radians.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float AngleVel
		{
			get { return this.angleVelAbs; }
		}
		/// <summary>
		/// [GET / SET] The objects scale.
		/// </summary>
		public float Scale
		{
			get { return this.scaleAbs; }
			set 
			{ 
				this.scaleAbs = MathF.Max(value, MinScale);

				if (this.parentTransform != null)
				{
					this.scale = this.scaleAbs / this.parentTransform.scaleAbs;
				}
				else
				{
					this.scale = value;
				}

				this.changes |= DirtyFlags.Scale;
				this.UpdateAbsChild();
			}
		}
		
		/// <summary>
		/// [GET] The objects forward vector.
		/// </summary>
		public Vector3 Forward
		{
			get 
			{ 
				return new Vector3(
					MathF.Sin(this.Angle),
					-MathF.Cos(this.Angle),
					0.0f);
			}
		}
		/// <summary>
		/// [GET] The objects right (directional) vector.
		/// </summary>
		public Vector3 Right
		{
			get 
			{
				return new Vector3(
					-MathF.Cos(this.Angle),
					-MathF.Sin(this.Angle),
					0.0f);
			}
		}


		/// <summary>
		/// Calculates a world coordinate from a Transform-local coordinate.
		/// </summary>
		/// <param name="local"></param>
		/// <returns></returns>
		public Vector3 GetWorldPoint(Vector3 local)
		{
			Vector3 world;

			Vector3.Multiply(ref local, this.scaleAbs, out world);
			MathF.TransformCoord(ref world.X, ref world.Y, this.angleAbs);
			Vector3.Add(ref world, ref this.posAbs, out world);

			return world;
		}
		/// <summary>
		/// Calculates a world coordinate from a Transform-local coordinate.
		/// </summary>
		/// <param name="local"></param>
		/// <returns></returns>
		public Vector2 GetWorldPoint(Vector2 local)
		{
			return this.GetWorldPoint(new Vector3(local)).Xy;
		}
		/// <summary>
		/// Calculates a Transform-local coordinate from a world coordinate.
		/// </summary>
		/// <param name="world"></param>
		/// <returns></returns>
		public Vector3 GetLocalPoint(Vector3 world)
		{
			Vector3 local;
			
			Vector3.Subtract(ref world, ref this.posAbs, out local);
			MathF.TransformCoord(ref local.X, ref local.Y, -this.angleAbs);
			Vector3.Divide(ref local, this.scaleAbs, out local);

			return local;
		}
		/// <summary>
		/// Calculates a Transform-local coordinate from a world coordinate.
		/// </summary>
		/// <param name="world"></param>
		/// <returns></returns>
		public Vector2 GetLocalPoint(Vector2 world)
		{
			return this.GetLocalPoint(new Vector3(world)).Xy;
		}
		/// <summary>
		/// Calculates a world vector from a Transform-local vector.
		/// Does only take scale and rotation into account, but not position.
		/// </summary>
		/// <param name="local"></param>
		/// <returns></returns>
		public Vector3 GetWorldVector(Vector3 local)
		{
			Vector3 world;

			Vector3.Multiply(ref local, this.scaleAbs, out world);
			MathF.TransformCoord(ref world.X, ref world.Y, this.angleAbs);

			return world;
		}
		/// <summary>
		/// Calculates a world vector from a Transform-local vector.
		/// Does only take scale and rotation into account, but not position.
		/// </summary>
		/// <param name="local"></param>
		/// <returns></returns>
		public Vector2 GetWorldVector(Vector2 local)
		{
			return this.GetWorldVector(new Vector3(local)).Xy;
		}
		/// <summary>
		/// Calculates a Transform-local vector from a world vector.
		/// Does only take scale and rotation into account, but not position.
		/// </summary>
		/// <param name="world"></param>
		/// <returns></returns>
		public Vector3 GetLocalVector(Vector3 world)
		{
			Vector3 local = world;
			
			MathF.TransformCoord(ref local.X, ref local.Y, -this.angleAbs);
			Vector3.Divide(ref local, this.scaleAbs, out local);

			return local;
		}
		/// <summary>
		/// Calculates a Transform-local vector from a world vector.
		/// Does only take scale and rotation into account, but not position.
		/// </summary>
		/// <param name="world"></param>
		/// <returns></returns>
		public Vector2 GetLocalVector(Vector2 local)
		{
			return this.GetLocalVector(new Vector3(local)).Xy;
		}
		/// <summary>
		/// Calculates the Transforms world velocity at a given world coordinate;
		/// </summary>
		/// <param name="world"></param>
		/// <returns></returns>
		public Vector3 GetWorldVelocityAt(Vector3 world)
		{
			return GetWorldVector(GetLocalVelocityAt(GetLocalPoint(world)));
		}
		/// <summary>
		/// Calculates the Transforms world velocity at a given world coordinate;
		/// </summary>
		/// <param name="world"></param>
		/// <returns></returns>
		public Vector2 GetWorldVelocityAt(Vector2 world)
		{
			return this.GetWorldVelocityAt(new Vector3(world)).Xy;
		}
		/// <summary>
		/// Calculates the Transforms local velocity at a given local coordinate;
		/// </summary>
		/// <param name="local"></param>
		/// <returns></returns>
		public Vector3 GetLocalVelocityAt(Vector3 local)
		{
			Vector3 vel = this.velAbs;
			Vector2 angleVel = local.Xy.PerpendicularRight * this.angleVelAbs;
			return vel + new Vector3(angleVel);
		}
		/// <summary>
		/// Calculates the Transforms local velocity at a given local coordinate;
		/// </summary>
		/// <param name="local"></param>
		/// <returns></returns>
		public Vector2 GetLocalVelocityAt(Vector2 world)
		{
			return this.GetLocalVelocityAt(new Vector3(world)).Xy;
		}

		/// <summary>
		/// Moves the object by the given vector. This will affect the Transforms <see cref="Vel">velocity</see> value.
		/// </summary>
		/// <param name="value"></param>
		public void MoveBy(Vector3 value)
		{
			// Accumulate velocity
			if (MathF.Abs(Time.TimeMult) > float.Epsilon)
			{
				this.tempVel += value / Time.TimeMult;
			}

			this.pos += value; 
			this.changes |= DirtyFlags.Pos; 
			this.UpdateAbs(true);
		}
		/// <summary>
		/// Moves the object by the given vector. This will affect the Transforms <see cref="Vel">velocity</see> value.
		/// </summary>
		/// <param name="value"></param>
		public void MoveBy(Vector2 value)
		{
			this.MoveBy(new Vector3(value));
		}
		/// <summary>
		/// Moves the object by given absolute vector. This will affect the Transforms <see cref="Vel">velocity</see> value.
		/// </summary>
		/// <param name="value"></param>
		public void MoveByAbs(Vector3 value)
		{
			// Accumulate velocity
			if (MathF.Abs(Time.TimeMult) > float.Epsilon)
			{
				this.tempVelAbs += value / Time.TimeMult;
			}

			this.posAbs += value;

			if (this.parentTransform != null)
			{
				this.pos = this.posAbs;
				Vector3.Subtract(ref this.pos, ref this.parentTransform.posAbs, out this.pos);
				Vector3.Divide(ref this.pos, this.parentTransform.scaleAbs, out this.pos);
				MathF.TransformCoord(ref this.pos.X, ref this.pos.Y, -this.parentTransform.angleAbs);

				this.tempVel = this.tempVelAbs;
				Vector3.Subtract(ref this.tempVel, ref this.parentTransform.tempVelAbs, out this.tempVel);
				Vector3.Divide(ref this.tempVel, this.parentTransform.scaleAbs, out this.tempVel);
				MathF.TransformCoord(ref this.tempVel.X, ref this.tempVel.Y, -this.parentTransform.angleAbs);
			}
			else
			{
				this.pos = this.posAbs;
				this.tempVel = this.tempVelAbs;
			}

			this.changes |= DirtyFlags.Pos;
			this.UpdateAbsChild(true);
		}
		/// <summary>
		/// Moves the object by given absolute vector. This will affect the Transforms <see cref="Vel">velocity</see> value.
		/// </summary>
		/// <param name="value"></param>
		public void MoveByAbs(Vector2 value)
		{
			this.MoveByAbs(new Vector3(value));
		}
		/// <summary>
		/// Moves the object to the given relative position. This will affect the Transforms <see cref="Vel">velocity</see> value.
		/// </summary>
		/// <param name="value"></param>
		public void MoveTo(Vector3 value)
		{
			this.MoveBy(value - this.pos);
		}
		/// <summary>
		/// Moves the object to the given relative position. This will affect the Transforms <see cref="Vel">velocity</see> value.
		/// </summary>
		/// <param name="value"></param>
		public void MoveTo(Vector2 value)
		{
			this.MoveTo(new Vector3(value));
		}
		/// <summary>
		/// Moves the object to the given absolute position. This will affect the Transforms <see cref="Vel">velocity</see> value.
		/// </summary>
		/// <param name="value"></param>
		public void MoveToAbs(Vector3 value)
		{
			this.MoveByAbs(value - this.posAbs);
		}
		/// <summary>
		/// Moves the object to the given absolute position. This will affect the Transforms <see cref="Vel">velocity</see> value.
		/// </summary>
		/// <param name="value"></param>
		public void MoveToAbs(Vector2 value)
		{
			this.MoveToAbs(new Vector3(value));
		}
		/// <summary>
		/// Turns the object by the given radian angle. This will affect the Transforms <see cref="AngleVel">angular velocity</see> value.
		/// </summary>
		/// <param name="value"></param>
		public void TurnBy(float value)
		{
			// Accumulate velocity
			if (MathF.Abs(Time.TimeMult) > float.Epsilon)
			{
				this.tempAngleVel += value / Time.TimeMult;
				this.tempAngleVelAbs += value / Time.TimeMult;
			}

			this.angle = MathF.NormalizeAngle(this.angle + value);
			this.changes |= DirtyFlags.Angle; 
			this.UpdateAbs(true);
		}
		/// <summary>
		/// Turns the object to the given relative radian angle. This will affect the Transforms <see cref="AngleVel">angular velocity</see> value.
		/// </summary>
		/// <param name="value"></param>
		public void TurnTo(float value)
		{
			this.TurnBy(MathF.TurnDir(this.angle, value) * MathF.CircularDist(value, this.angle));
		}
		/// <summary>
		/// Turns the object to the given absolute radian angle. This will affect the Transforms <see cref="AngleVel">angular velocity</see> value.
		/// </summary>
		/// <param name="value"></param>
		public void TurnToAbs(float value)
		{
			this.TurnBy(MathF.TurnDir(this.angleAbs, value) * MathF.CircularDist(value, this.angleAbs));
		}

		/// <summary>
		/// Updates the Transforms data all at once.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="scale"></param>
		/// <param name="angle"></param>
		/// <param name="angleVel"></param>
		public void SetTransform(Vector3 pos, float scale, float angle)
		{
			this.posAbs = pos;
			this.angleAbs = angle;
			this.scaleAbs = scale;

			this.changes |= DirtyFlags.All;
			this.UpdateRel();
			this.UpdateAbsChild();
		}
		/// <summary>
		/// Updates the Transforms data all at once.
		/// </summary>
		/// <param name="other"></param>
		public void SetTransform(Transform other)
		{
			if (other == this) return;
			this.SetTransform(other.Pos, other.Scale, other.Angle);
		}
		/// <summary>
		/// Updates the Transforms data all at once.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="scale"></param>
		/// <param name="angle"></param>
		/// <param name="angleVel"></param>
		public void SetRelativeTransform(Vector3 pos, float scale, float angle)
		{
			this.pos = pos;
			this.angle = angle;
			this.scale = scale;

			this.changes |= DirtyFlags.All;
			this.UpdateAbs();
		}
		/// <summary>
		/// Updates the Transforms data all at once.
		/// </summary>
		/// <param name="other"></param>
		public void SetRelativeTransform(Transform other)
		{
			if (other == this) return;
			this.SetRelativeTransform(other.RelativePos, other.RelativeScale, other.RelativeAngle);
		}
		
		/// <summary>
		/// Checks whether transform values have been changed, clears the changelist and fires the appropriate events
		/// </summary>
		/// <param name="sender"></param>
		public void CommitChanges(Component sender = null)
		{
			if (this.changes == DirtyFlags.None) return;
			if (sender == null) sender = this;
			if (this.eventTransformChanged != null)
				this.eventTransformChanged(sender, new TransformChangedEventArgs(this, this.changes));
			this.changes = DirtyFlags.None;
		}

		void ICmpUpdatable.OnUpdate()
		{
			this.CheckValidTransform();

			// Calculate velocity values from last frames movement
			if (MathF.Abs(Time.TimeMult) > float.Epsilon)
			{
				this.vel = this.tempVel;
				this.velAbs = this.tempVelAbs;
				this.angleVel = this.tempAngleVel;
				this.angleVelAbs = this.tempAngleVelAbs;
				this.tempVel = Vector3.Zero;
				this.tempVelAbs = Vector3.Zero;
				this.tempAngleVel = 0.0f;
				this.tempAngleVelAbs = 0.0f;
				this.CheckValidTransform();
			}

			// Clear change flags
			this.CommitChanges();

			this.CheckValidTransform();
		}
		void ICmpEditorUpdatable.OnUpdate()
		{
			this.CheckValidTransform();

			if (this.ignoreParent)
				this.UpdateRel();
			else
				this.UpdateAbs();

			// Clear change flags
			this.CommitChanges();

			this.CheckValidTransform();
		}
		void ICmpGameObjectListener.OnGameObjectParentChanged(GameObject oldParent, GameObject newParent)
		{
			if (oldParent != null)
			{
				if (this.parentTransform == null)
					oldParent.EventComponentAdded -= this.Parent_EventComponentAdded;
				else
					oldParent.EventComponentRemoving -= this.Parent_EventComponentRemoving;
			}

			if (newParent != null)
			{
				this.parentTransform = newParent.Transform;
				if (this.parentTransform == null)
					newParent.EventComponentAdded += this.Parent_EventComponentAdded;
				else
					newParent.EventComponentRemoving += this.Parent_EventComponentRemoving;
			}
			else
				this.parentTransform = null;

			this.UpdateRel();
		}
		void ICmpInitializable.OnInit(InitContext context)
		{
			if (context == InitContext.AddToGameObject ||
				context == InitContext.Loaded)
			{
				if (this.gameobj.Parent != null)
				{
					this.parentTransform = this.gameobj.Parent.Transform;
					if (this.parentTransform == null)
						this.gameobj.Parent.EventComponentAdded += this.Parent_EventComponentAdded;
					else
						this.gameobj.Parent.EventComponentRemoving += this.Parent_EventComponentRemoving;
				}
				else
					this.parentTransform = null;
				this.UpdateRel();
			}
		}
		void ICmpInitializable.OnShutdown(ShutdownContext context)
		{
			if (context == ShutdownContext.RemovingFromGameObject)
			{
				if (this.gameobj.Parent != null)
				{
					if (this.parentTransform == null)
						this.gameobj.Parent.EventComponentAdded -= this.Parent_EventComponentAdded;
					else
						this.gameobj.Parent.EventComponentRemoving -= this.Parent_EventComponentRemoving;
				}

				this.parentTransform = null;
				this.UpdateRel();
			}
		}
		private void Parent_EventComponentAdded(object sender, ComponentEventArgs e)
		{
			Transform cmpTransform = e.Component as Transform;
			if (cmpTransform != null)
			{
				cmpTransform.GameObj.EventComponentAdded -= this.Parent_EventComponentAdded;
				cmpTransform.GameObj.EventComponentRemoving += this.Parent_EventComponentRemoving;
				this.parentTransform = cmpTransform;
				this.UpdateRel();
			}
		}
		private void Parent_EventComponentRemoving(object sender, ComponentEventArgs e)
		{
			if (this.parentTransform != null)
			{
				Transform cmpTransform = e.Component as Transform;
				if (cmpTransform == this.parentTransform)
				{
					cmpTransform.GameObj.EventComponentAdded += this.Parent_EventComponentAdded;
					cmpTransform.GameObj.EventComponentRemoving -= this.Parent_EventComponentRemoving;
					this.parentTransform = null;
					this.UpdateRel();
				}
			}
		}

		private void UpdateAbs(bool updateTempVel = false)
		{
			this.CheckValidTransform();

			if (this.parentTransform == null)
			{
				this.angleAbs = this.angle;
				this.posAbs = this.pos;
				this.scaleAbs = this.scale;
				if (updateTempVel)
				{
					this.tempVelAbs = this.tempVel;
					this.tempAngleVelAbs = this.tempAngleVel;
				}
			}
			else
			{
				if (this.deriveAngle)
				{
					this.angleAbs = MathF.NormalizeAngle(this.angle + this.parentTransform.angleAbs);
					if (updateTempVel) this.tempAngleVelAbs = this.tempAngleVel + this.parentTransform.tempAngleVelAbs;
				}
				else
				{
					this.angleAbs = this.angle;
					if (updateTempVel) this.tempAngleVelAbs = this.tempAngleVel;
				}

				this.scaleAbs = this.scale * this.parentTransform.scaleAbs;

				Vector2 parentAngleAbsDotX;
				Vector2 parentAngleAbsDotY;
				MathF.GetTransformDotVec(this.parentTransform.angleAbs, out parentAngleAbsDotX, out parentAngleAbsDotY);

				Vector3.Multiply(ref this.pos, this.parentTransform.scaleAbs, out this.posAbs);
				MathF.TransformDotVec(ref this.posAbs, ref parentAngleAbsDotX, ref parentAngleAbsDotY);
				Vector3.Add(ref this.posAbs, ref this.parentTransform.posAbs, out this.posAbs);

				if (updateTempVel)
				{
					Vector2 parentTurnVelAdjust = this.pos.Xy.PerpendicularRight;
					Vector2.Multiply(ref parentTurnVelAdjust, this.parentTransform.tempAngleVelAbs, out parentTurnVelAdjust);
					MathF.TransformDotVec(ref parentTurnVelAdjust, ref parentAngleAbsDotX, ref parentAngleAbsDotY);

					Vector3.Multiply(ref this.tempVel, this.parentTransform.scaleAbs, out this.tempVelAbs);
					MathF.TransformDotVec(ref this.tempVelAbs, ref parentAngleAbsDotX, ref parentAngleAbsDotY);
					Vector3.Add(ref this.tempVelAbs, ref this.parentTransform.tempVelAbs, out this.tempVelAbs);

					this.tempVelAbs.X += parentTurnVelAdjust.X;
					this.tempVelAbs.Y += parentTurnVelAdjust.Y;
				}
			}

			// Update absolute children coordinates
			this.UpdateAbsChild(updateTempVel);

			this.CheckValidTransform();
		}
		private void UpdateAbsChild(bool updateTempVel = false)
		{
			this.CheckValidTransform();

			if (this.gameobj != null)
			{
				foreach (GameObject obj in this.gameobj.Children)
				{
					Transform t = obj.Transform;
					if (t == null) continue;
					if (!t.ignoreParent)
					{
						t.UpdateAbs(updateTempVel);

						t.changes |= this.changes;
						if ((this.changes & DirtyFlags.Scale) != DirtyFlags.None || (this.changes & DirtyFlags.Angle) != DirtyFlags.None)
							t.changes |= DirtyFlags.Pos;
					}
					else
					{
						t.UpdateRel(updateTempVel);
					}
				}
			}

			this.CheckValidTransform();
		}
		private void UpdateRel(bool updateTempVel = false)
		{
			this.CheckValidTransform();

			if (this.parentTransform == null)
			{
				this.angle = this.angleAbs;
				this.pos = this.posAbs;
				this.scale = this.scaleAbs;
				if (updateTempVel)
				{
					this.tempVel = this.tempVelAbs;
					this.tempAngleVel = this.tempAngleVelAbs;
				}
			}
			else
			{
				if (this.deriveAngle)
				{
					this.angle = MathF.NormalizeAngle(this.angleAbs - this.parentTransform.angleAbs);
					if (updateTempVel) this.tempAngleVel = this.tempAngleVelAbs - this.parentTransform.tempAngleVelAbs;
				}
				else
				{
					this.angle = this.angleAbs;
					if (updateTempVel) this.tempAngleVel = this.tempAngleVelAbs;
				}

				this.scale = this.scaleAbs / this.parentTransform.scaleAbs;
				
				Vector2 parentAngleAbsDotX;
				Vector2 parentAngleAbsDotY;
				MathF.GetTransformDotVec(-this.parentTransform.angleAbs, out parentAngleAbsDotX, out parentAngleAbsDotY);

				Vector3.Subtract(ref this.posAbs, ref this.parentTransform.posAbs, out this.pos);
				MathF.TransformDotVec(ref this.pos, ref parentAngleAbsDotX, ref parentAngleAbsDotY);
				Vector3.Divide(ref this.pos, this.parentTransform.scaleAbs, out this.pos);

				if (updateTempVel)
				{
					Vector2 parentTurnVelAdjust = this.pos.Xy.PerpendicularRight;
					Vector2.Multiply(ref parentTurnVelAdjust, this.parentTransform.tempAngleVelAbs, out parentTurnVelAdjust);
					MathF.TransformDotVec(ref parentTurnVelAdjust, ref parentAngleAbsDotX, ref parentAngleAbsDotY);
					
					Vector3.Subtract(ref this.tempVelAbs, ref this.parentTransform.tempVelAbs, out this.tempVel);
					MathF.TransformDotVec(ref this.tempVel, ref parentAngleAbsDotX, ref parentAngleAbsDotY);
					Vector3.Divide(ref this.tempVel, this.parentTransform.scaleAbs, out this.tempVel);

					this.tempVel.X -= parentTurnVelAdjust.X;
					this.tempVel.Y -= parentTurnVelAdjust.Y;
				}
			}

			this.CheckValidTransform();
		}

		protected override void OnCopyTo(Component target, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(target, provider);
			Transform t = target as Transform;

			t.deriveAngle		= this.deriveAngle;
			t.ignoreParent		= this.ignoreParent;

			t.pos				= this.pos;
			t.angle				= this.angle;
			t.scale				= this.scale;

			t.posAbs			= this.posAbs;
			t.angleAbs			= this.angleAbs;
			t.scaleAbs			= this.scaleAbs;

			t.tempVel			= this.tempVel;
			t.tempVelAbs		= this.tempVelAbs;
			t.tempAngleVel		= this.tempAngleVel;
			t.tempAngleVelAbs	= this.tempAngleVelAbs;

			t.velAbs			= this.velAbs;
			t.vel				= this.vel;
			t.angleVel			= this.angleVel;
			t.angleVelAbs		= this.angleVelAbs;

			// Update absolute transformation. 
			// Need to update parent transform, because this may happen 
			// during Prefab.Apply, which occurs before OnLoaded. 
			t.parentTransform	= (t.gameobj != null && t.gameobj.Parent != null) ? t.gameobj.Parent.Transform : null;
			t.UpdateAbs();
		}

		[System.Diagnostics.Conditional("DEBUG")]
		internal void CheckValidTransform()
		{
			MathF.CheckValidValue(this.pos);
			MathF.CheckValidValue(this.vel);
			MathF.CheckValidValue(this.scale);
			MathF.CheckValidValue(this.angle);
			MathF.CheckValidValue(this.angleVel);
			MathF.CheckValidValue(this.tempVel);
			MathF.CheckValidValue(this.tempAngleVel);

			MathF.CheckValidValue(this.posAbs);
			MathF.CheckValidValue(this.velAbs);
			MathF.CheckValidValue(this.scaleAbs);
			MathF.CheckValidValue(this.angleAbs);
			MathF.CheckValidValue(this.angleVelAbs);
			MathF.CheckValidValue(this.tempVelAbs);
			MathF.CheckValidValue(this.tempAngleVelAbs);
		}
	}
}
