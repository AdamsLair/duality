using Duality;
using Duality.Cloning;
using Duality.Drawing;
using OpenTK;
using System;

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
	public class Agent : Component
	{
		private IVelocitySampler sampler;
		private IAgentCharacteristics characteristics;
		private ISteeringTarget target;

		private float radius;
		private float toiHorizon;
		[NonSerialized]
		private Vector2 bestVel;

		#region visual logging stuff
#if DEBUG
		public enum VisualLoggingMode
		{
			NONE,
			ONLY_VELOCITY,
			ALL_VELOCITIES,
			TOI_PENALITY,
			COST
		}

		private VisualLoggingMode debugVisualizationMode;

		public VisualLoggingMode DebugVisualizationMode
		{
			get { return debugVisualizationMode; }
			set { debugVisualizationMode = value; }
		}

		[NonSerialized]
		private VisualLog log = VisualLog.Get("agent");
		[NonSerialized]
		private const float debugVelocityRadius = 100f;
#endif
		#endregion

		public Agent()
		{
			sampler = new AdaptiveVelocitySampler();
			characteristics = new DefaultCharacteristics();
			target = new PointTarget();
			toiHorizon = 240f;

#if DEBUG
			log.BaseColor = ColorRgba.White;
#endif
		}

		public void OnShutdown(Component.ShutdownContext context)
		{
		}

		public IVelocitySampler Sampler
		{
			get { return sampler; }
			set { sampler = value; }
		}

		public IAgentCharacteristics Characteristics
		{
			get { return characteristics; }
			set { characteristics = value; }
		}

		public ISteeringTarget Target
		{
			get { return target; }
			set { target = value; }
		}

		/// <summary>
		/// [GET/SET] The radius of the agent (an agent is always representet as circle)
		/// </summary>
		public float Radius
		{
			get { return radius; }
			set { radius = value; }
		}

		/// <summary>
		/// [Get/SET] The maximum time of impact wich the agent will react on. 
		/// If you set this too high your agent will oscillate alot in crowded situations and if you set
		/// it too low your agent will avoid very late which looks artificial.
		/// </summary>
		public float ToiHorizon
		{
			get { return toiHorizon; }
			set { toiHorizon = value; }
		}

		/// <summary>
		/// [GET] The calculated velocity which the agent calculated as optimum. 
		/// </summary>
		public Vector2 BestVel
		{
			get { return bestVel; }
		}

		// TODO: move me to a more general place
		private float RayRayIntersect(Vector2 start1, Vector2 dir1, Vector2 start2, Vector2 dir2)
		{
			var relStart = start2 - start1;
			var num = dir2.Y * relStart.X - dir2.X * relStart.Y;
			var den = dir1.X * dir2.Y - dir2.X * dir1.Y;

			if (den <= float.Epsilon)
				return float.NaN;

			return num / den;
		}

		// TODO: move me to a more general place
		private bool ToiCircleCircle(Vector2 relPos, float obstacleRadius, Vector2 relVel, out float minT, out float maxT)
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

		public void OnUpdate()
		{
			var scene = GameObj.ParentScene;
			if (scene == null)
				return;

			var manager = scene.FindComponent<AgentManager>();
			if (manager == null)
				return; // TODO: auto create agent manager


			var otherAgents = manager.FindNeighborAgents(this);
			sampler.Reset();

			var keepSampling = true;
			var bestVelocity = Vector2.Zero;
			var bestScore = float.PositiveInfinity;

			while (keepSampling)
			{
				Vector2 sample = sampler.GetCurrentSample(this) * characteristics.MaxSpeed;

				// penalities
				float toiPenality = 0f;

				// check against every obstacle
				foreach (var otherAgent in otherAgents)
				{
					float curToiPenality = 0f;

					// calculate helper variables for RVO
					var relPos = otherAgent.GameObj.Transform.Pos.Xy - GameObj.Transform.Pos.Xy;
					// -> calculate side (only sign is of interest) for HRVO
					var averageSpeed = 0.5f * (GameObj.Transform.Vel.Xy + otherAgent.GameObj.Transform.Vel.Xy);
					var side = Vector2.Dot(relPos.PerpendicularRight, sample - averageSpeed);

					var selfFactor = 0f;
					var otherFactor = 1f;
					if (side >= 0f)
					{
						// this is different from original RVO - we use the ratio of the observed velocities to determine
						// how much responsibility one agent has
						var selfSpeed = GameObj.Transform.Vel.Xy.Length;
						var otherSpeed = otherAgent.GameObj.Transform.Vel.Xy.Length;

						selfFactor = 0.5f;
						var selfPlusOtherSpeed = selfSpeed + otherSpeed;
						if (selfPlusOtherSpeed > float.Epsilon)
							selfFactor = otherSpeed / selfPlusOtherSpeed;
						otherFactor = 1f - selfFactor;
					}

					// check time of impact
					float curMinToi, curMaxToi;
					var expectedRelVel = sample - (selfFactor * GameObj.Transform.Vel.Xy + otherFactor * otherAgent.GameObj.Transform.Vel.Xy);
					if (ToiCircleCircle(relPos, Radius + otherAgent.Radius, expectedRelVel, out curMinToi, out curMaxToi) && 0f < curMaxToi)
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
							curToiPenality = MathF.Max(0f, toiHorizon - curMinToi) / toiHorizon;
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
				if (DebugVisualizationMode != VisualLoggingMode.NONE 
					&& DebugVisualizationMode != VisualLoggingMode.ONLY_VELOCITY
					&& DebugVisualizationMode != VisualLoggingMode.ALL_VELOCITIES)
				{
					var debugPos = sample / characteristics.MaxSpeed * debugVelocityRadius;
					var debugColorFactor = 0f;
					switch(DebugVisualizationMode) 
					{
						case VisualLoggingMode.COST:
							debugColorFactor = score;
							break;
						case VisualLoggingMode.TOI_PENALITY:
							debugColorFactor = toiPenality;
							break;
					}
					var debugColor = ColorRgba.Lerp(ColorRgba.White, ColorRgba.Black, MathF.Pow(debugColorFactor, 1f/4f));
					log.DrawCircle(debugPos.X, debugPos.Y, 4f).AnchorAt(this.GameObj).WithColor(debugColor);
				}
#endif
				#endregion
			}

			//
			this.bestVel = bestVelocity;

			#region visual logging of the velocities
#if DEBUG
			if(DebugVisualizationMode == VisualLoggingMode.ALL_VELOCITIES)
			{
				var selfDebugVelocity = GameObj.Transform.Vel.Xy / Characteristics.MaxSpeed * debugVelocityRadius;
				log.DrawVector(0f, 0f, selfDebugVelocity.X, selfDebugVelocity.Y).AnchorAt(GameObj).WithColor(ColorRgba.DarkGrey);

				foreach (var otherAgent in otherAgents)
				{
					var debugVelocity = otherAgent.GameObj.Transform.Vel.Xy / otherAgent.Characteristics.MaxSpeed * debugVelocityRadius;
					log.DrawVector(0f, 0f, debugVelocity.X, debugVelocity.Y).AnchorAt(otherAgent.GameObj).WithColor(ColorRgba.DarkGrey);
				}
			}
			if (DebugVisualizationMode != VisualLoggingMode.NONE)
			{
				var curVelocity = GameObj.Transform.Vel.Xy / Characteristics.MaxSpeed * debugVelocityRadius;
				log.DrawVector(0f, 0f, curVelocity.X, curVelocity.Y).AnchorAt(GameObj).WithColor(ColorRgba.DarkGrey);

				var debugVelocity = this.bestVel / characteristics.MaxSpeed * debugVelocityRadius;
				log.DrawVector(0f, 0f, debugVelocity.X, debugVelocity.Y).AnchorAt(this.GameObj);
			}
#endif
			#endregion
		}

		protected override void OnCopyTo(Component target, CloneProvider provider)
		{
			base.OnCopyTo(target, provider);
			var other = (Agent)target;
			other.Radius = this.radius;
			other.toiHorizon = this.toiHorizon;
			other.Sampler = provider.RequestObjectClone(this.sampler);
			other.Characteristics = provider.RequestObjectClone(this.characteristics);
			other.target = provider.RequestObjectClone(this.target);
		}
	}
}
