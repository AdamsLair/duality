using System;
using System.Linq;

using Duality;
using Duality.Cloning;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Properties;
using Duality.Plugins.Steering.Properties;

using OpenTK;

namespace Duality.Plugins.Steering
{
	/// <summary>
	/// An agent is the basic component you want to attach to computer-controlled characters. 
	/// It contains functionallity to avoid collisions with other agents/obstacles and tries
	/// to get to some defined target-location. The avoidance is only local therefore it's 
	/// possible that the agent get stuck in local minima. For more complex navigation you
	/// need a high-level pathfinding layer on top of the local avoidance.
	/// </summary>
	[Serializable]
	[RequiredComponent(typeof(Transform))]
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryAI)]
	[EditorHintImage(typeof(SteeringRes), SteeringResNames.ImageAgent)]
	public class Agent : Component
	{
		private IVelocitySampler		sampler			= null;
		private IAgentCharacteristics	characteristics	= null;
		private ISteeringTarget			target			= null;
		private float					radius			= 64.0f;
		private float					toiHorizon		= 240.0f;
		[NonSerialized] private Vector2 suggestedVel	= Vector2.Zero;


		public IVelocitySampler Sampler
		{
			get { return this.sampler; }
			set { this.sampler = value; }
		}
		public IAgentCharacteristics Characteristics
		{
			get { return this.characteristics; }
			set { this.characteristics = value; }
		}
		public ISteeringTarget Target
		{
			get { return this.target; }
			set { this.target = value; }
		}
		/// <summary>
		/// [GET / SET] The Agents target velocity, i.e. the one which it tries to acquire.
		/// This is a convenience property that automatically sets <see cref="TargetSpeed"/> and 
		/// <see cref="Target"/> to the appropriate values.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public Vector2 TargetVel
		{
			get
			{
				if (this.target is DirectionTarget)
				{
					return (this.target as DirectionTarget).Direction * this.TargetSpeed;
				}
				else
				{
					return Vector2.Zero;
				}
			}
			set
			{
				if (!(this.target is DirectionTarget)) this.target = new DirectionTarget();
				float valueLen = value.Length;
				(this.target as DirectionTarget).Direction = value / (valueLen > 0.0f ? valueLen : 1.0f);
				this.TargetSpeed = value.Length;
			}
		}
		/// <summary>
		/// [GET / SET] The Agents target velocity, i.e. the one which it tries to acquire.
		/// This is a convenience property that automatically sets <see cref="TargetSpeed"/> and 
		/// <see cref="Target"/> to the appropriate values.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public Vector2 TargetPos
		{
			get
			{
				if (this.target is PointTarget)
				{
					return (this.target as PointTarget).Location;
				}
				else
				{
					return Vector2.Zero;
				}
			}
			set
			{
				if (!(this.target is PointTarget)) this.target = new PointTarget();
				(this.target as PointTarget).Location = value;
			}
		}
		/// <summary>
		/// [GET / SET] The target speed this Agent attempts to acquire unless distracted by other Agents.
		/// </summary>
		[EditorHintRange(0.0f, 10000.0f)]
		public float TargetSpeed
		{
			get { return this.characteristics != null ? this.characteristics.PreferredSpeed : 0.0f; }
			set
			{
				this.AcquireConfigObjects();
				this.characteristics.PreferredSpeed = value;
			}
		}
		/// <summary>
		/// [GET / SET] The radius of the agent (an agent is always representet as circle)
		/// </summary>
		[EditorHintRange(0.0f, 10000.0f)]
		public float Radius
		{
			get { return this.radius; }
			set { this.radius = value; }
		}
		/// <summary>
		/// [GET / SET] The maximum time of impact wich the agent will react on. 
		/// If you set this too high your agent will oscillate alot in crowded situations and if you set
		/// it too low your agent will avoid very late which looks artificial.
		/// </summary>
		public float ToiHorizon
		{
			get { return this.toiHorizon; }
			set { this.toiHorizon = value; }
		}
		/// <summary>
		/// [GET] The calculated velocity which the agent calculated as optimum. 
		/// </summary>
		public Vector2 SuggestedVel
		{
			get { return this.suggestedVel; }
		}


		public Agent()
		{
			this.AcquireConfigObjects();
		}

		internal void Update()
		{
			this.AcquireConfigObjects();

			Transform transform = this.GameObj.Transform;
			float scaledRad = this.radius * transform.Scale;

			Agent[] otherAgents = AgentManager.Instance.FindNeighborAgents(this).ToArray();
			this.sampler.Reset();

			bool keepSampling = true;
			Vector2 bestVelocity = Vector2.Zero;
			float bestScore = float.PositiveInfinity;

			while (keepSampling)
			{
				Vector2 sample = this.sampler.GetCurrentSample(this) * this.characteristics.MaxSpeed;

				// penalities
				float toiPenality = 0f;

				// check against every obstacle
				foreach (Agent otherAgent in otherAgents)
				{
					Transform otherTransform = otherAgent.GameObj.Transform;
					float curToiPenality = 0f;

					// calculate helper variables for RVO
					Vector2 relPos = otherTransform.Pos.Xy - transform.Pos.Xy;
					// -> calculate side (only sign is of interest) for HRVO
					Vector2 averageVel = 0.5f * (transform.Vel.Xy + otherTransform.Vel.Xy);
					float side = Vector2.Dot(relPos.PerpendicularRight, sample - averageVel);

					float selfFactor = 0f;
					float otherFactor = 1f;
					if (side >= 0f)
					{
						// this is different from original RVO - we use the ratio of the observed velocities to determine
						// how much responsibility one agent has
						float selfSpeed = transform.Vel.Xy.Length;
						float otherSpeed = otherTransform.Vel.Xy.Length;

						selfFactor = 0.5f;
						var selfPlusOtherSpeed = selfSpeed + otherSpeed;
						if (selfPlusOtherSpeed > float.Epsilon)
							selfFactor = otherSpeed / selfPlusOtherSpeed;
						otherFactor = 1f - selfFactor;
					}

					// check time of impact
					float curMinToi;
					float curMaxToi;
					Vector2 expectedRelVel = sample - (selfFactor * transform.Vel.Xy + otherFactor * otherTransform.Vel.Xy);
					float otherScaledRad = otherAgent.radius * otherTransform.Scale;
					if (ToiCircleCircle(relPos, scaledRad + otherScaledRad, expectedRelVel, out curMinToi, out curMaxToi) && 0f < curMaxToi)
					{
						if (curMinToi <= 0f)
						{
							// we collided - check which way we get here out
							if (MathF.Abs(curMinToi) < MathF.Abs(curMaxToi))
							{
								// => if minT (which is behind us) is lower then it's a bad idea to keep going because the
								// other way would bring us out earlier
								curToiPenality = float.PositiveInfinity;
							}
							else
							{
								curToiPenality = curMaxToi;
							}
						}
						else
						{
							// this is a new minimum toi 
							// => calculate penality
							curToiPenality = MathF.Max(0f, this.toiHorizon - curMinToi) / this.toiHorizon;
						}
					}
					toiPenality = MathF.Max(toiPenality, curToiPenality);
				}

				// ask the characteristics implementation how good this sample is
				float score = characteristics.CalculateVelocityCost(this, sample, toiPenality);

				// update sampler and check if we should stop
				keepSampling = sampler.SetCurrentCost(score);

				// check if this velocity is better then everything else we've seen so far
				if (score < bestScore)
				{
					bestScore = score;
					bestVelocity = sample;
				}

				#region visual logging of all sampled velocities
#if DEBUG
				if (DebugVisualizationMode != VisualLoggingMode.None 
					&& DebugVisualizationMode != VisualLoggingMode.VelocityOnly
					&& DebugVisualizationMode != VisualLoggingMode.AllVelocities)
				{
					Vector2 debugPos = sample / this.characteristics.MaxSpeed * DebugVelocityRadius;
					float debugColorFactor = 0.0f;
					switch (DebugVisualizationMode) 
					{
						case VisualLoggingMode.Cost:
							debugColorFactor = score;
							break;
						case VisualLoggingMode.ToiPenalty:
							debugColorFactor = toiPenality;
							break;
					}
					ColorRgba debugColor = ColorRgba.Lerp(ColorRgba.White, ColorRgba.Black, MathF.Pow(debugColorFactor, 1f/4f));
					VisualDebugLog.DrawCircle(debugPos.X, debugPos.Y, 4f).AnchorAt(this.GameObj).WithColor(debugColor);
				}
#endif
				#endregion
			}

			this.suggestedVel = bestVelocity;

			#region visual logging of the velocities
#if DEBUG
			if (DebugVisualizationMode == VisualLoggingMode.AllVelocities)
			{
				Vector2 selfDebugVelocity = transform.Vel.Xy / Characteristics.MaxSpeed * DebugVelocityRadius;
				VisualDebugLog.DrawVector(0f, 0f, selfDebugVelocity.X, selfDebugVelocity.Y).AnchorAt(GameObj).WithColor(ColorRgba.DarkGrey);

				foreach (var otherAgent in otherAgents)
				{
					Transform otherTransform = otherAgent.GameObj.Transform;
					Vector2 debugVelocity = otherTransform.Vel.Xy / otherAgent.Characteristics.MaxSpeed * DebugVelocityRadius;
					VisualDebugLog.DrawVector(0f, 0f, debugVelocity.X, debugVelocity.Y).AnchorAt(otherAgent.GameObj).WithColor(ColorRgba.DarkGrey);
				}
			}
			if (DebugVisualizationMode != VisualLoggingMode.None)
			{
				Vector2 curVelocity = transform.Vel.Xy / Characteristics.MaxSpeed * DebugVelocityRadius;
				VisualDebugLog.DrawVector(0f, 0f, curVelocity.X, curVelocity.Y).AnchorAt(GameObj).WithColor(ColorRgba.DarkGrey);

				var debugVelocity = this.suggestedVel / characteristics.MaxSpeed * DebugVelocityRadius;
				VisualDebugLog.DrawVector(0f, 0f, debugVelocity.X, debugVelocity.Y).AnchorAt(this.GameObj);
			}
#endif
			#endregion
		}
		private void AcquireConfigObjects()
		{
			if (this.sampler == null)			this.sampler = new AdaptiveVelocitySampler();
			if (this.target == null)			this.target = new DirectionTarget();
			if (this.characteristics == null)	this.characteristics = new DefaultAgentCharacteristics();
		}
		protected override void OnCopyTo(Component target, CloneProvider provider)
		{
			base.OnCopyTo(target, provider);
			Agent other = (Agent)target;
			other.Radius			= this.radius;
			other.toiHorizon		= this.toiHorizon;
			other.Sampler			= provider.RequestObjectClone(this.sampler);
			other.Characteristics	= provider.RequestObjectClone(this.characteristics);
			other.target			= provider.RequestObjectClone(this.target);
			other.suggestedVel		= this.suggestedVel;
		}
		
		// TODO: move me to a more general place
		private static float RayRayIntersect(Vector2 start1, Vector2 dir1, Vector2 start2, Vector2 dir2)
		{
			var relStart = start2 - start1;
			var num = dir2.Y * relStart.X - dir2.X * relStart.Y;
			var den = dir1.X * dir2.Y - dir2.X * dir1.Y;

			if (den <= float.Epsilon)
				return float.NaN;

			return num / den;
		}
		// TODO: move me to a more general place
		private static bool ToiCircleCircle(Vector2 relPos, float obstacleRadius, Vector2 relVel, out float minT, out float maxT)
		{
			// special handling for zero vector
			if (relVel == Vector2.Zero)
			{
				// check if we collide
				if (relVel.Length <= obstacleRadius)
				{
					minT = 0f;
					maxT = float.PositiveInfinity;
					return true;
				}
				else
				{
					minT = maxT = 0f;
					return false;
				}
			}

			/*
			* Matlab: 
			* syms vx vy t cx cy x y r;
			* x = vx * t;
			* y = vy * t;
			* res = solve((x - cx)^2 + (y - cy)^2 - r^2 == 0, t);
			* 
			* (cx*vx + cy*vy + (- cx^2*vy^2 + 2*cx*cy*vx*vy - cy^2*vx^2 + r^2*vx^2 + r^2*vy^2)^(1/2))/(vx^2 + vy^2)
			* (cx*vx + cy*vy - (- cx^2*vy^2 + 2*cx*cy*vx*vy - cy^2*vx^2 + r^2*vx^2 + r^2*vy^2)^(1/2))/(vx^2 + vy^2)
			*/
			var cx = relPos.X;
			var cy = relPos.Y;
			var vx = relVel.X;
			var vy = relVel.Y;
			var r = obstacleRadius;

			var sqrtTerm = -(cx * cx) * (vy * vy) + 2 * cx * cy * vx * vy - (cy * cy) * (vx * vx) + (r * r) * (vx * vx) + (r * r) * (vy * vy);
			if (sqrtTerm < 0f)
			{
				minT = maxT = float.NaN;
				return false;
			}

			var denom = ((vx * vx) + (vy * vy));
			if (Math.Abs(denom) < float.Epsilon) {
				if(denom < 0f)
					minT = maxT = float.NegativeInfinity;
				else
					minT = maxT = float.PositiveInfinity;
				return true;
			}
			maxT = (cx * vx + cy * vy + MathF.Sqrt(sqrtTerm)) / denom;
			minT = (cx * vx + cy * vy - MathF.Sqrt(sqrtTerm)) / denom;
			return true;
		}

		#region visual logging stuff
		public enum VisualLoggingMode
		{
			None,
			VelocityOnly,
			AllVelocities,
			ToiPenalty,
			Cost
		}

#if DEBUG

		private VisualLoggingMode debugVisualizationMode;
		public VisualLoggingMode DebugVisualizationMode
		{
			get { return debugVisualizationMode; }
			set { debugVisualizationMode = value; }
		}

		static Agent()
		{
			VisualDebugLog.BaseColor = ColorRgba.White;
		}

		private static readonly VisualLog VisualDebugLog = VisualLog.Get("agent");
		private const float DebugVelocityRadius = 100f;
#endif
		#endregion
	}
}
