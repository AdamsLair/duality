using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using OpenTK;

namespace FarseerPhysics.Controllers
{
    public enum GravityType
    {
        Linear,
        DistanceSquared
    }

    public class GravityController : Controller
    {
        public List<Body> Bodies = new List<Body>();
        public List<Vector2> Points = new List<Vector2>();

        public GravityController(float strength)
            : base(ControllerType.GravityController)
        {
            Strength = strength;
            MaxRadius = float.MaxValue;
        }

        public GravityController(float strength, float maxRadius, float minRadius)
            : base(ControllerType.GravityController)
        {
            MinRadius = minRadius;
            MaxRadius = maxRadius;
            Strength = strength;
        }

        public float MinRadius { get; set; }
        public float MaxRadius { get; set; }
        public float Strength { get; set; }
        public GravityType GravityType { get; set; }

        public override void Update(float dt)
        {
            Vector2 f = Vector2.Zero;

            foreach (Body body1 in World.BodyList)
            {
                if (!IsActiveOn(body1))
                    continue;

                foreach (Body body2 in Bodies)
                {
                    if (body1 == body2 || (body1.IsStatic && body2.IsStatic) || !body2.Enabled)
                        continue;

                    Vector2 d = body2.WorldCenter - body1.WorldCenter;
                    float r2 = d.LengthSquared;

                    if (r2 < Settings.Epsilon)
                        continue;

                    float r = d.Length;

                    if (r >= MaxRadius || r <= MinRadius)
                        continue;

                    switch (GravityType)
                    {
                        case GravityType.DistanceSquared:
                            f = Strength / r2 / (float)Math.Sqrt(r2) * body1.Mass * body2.Mass * d;
                            break;
                        case GravityType.Linear:
                            f = Strength / r2 * body1.Mass * body2.Mass * d;
                            break;
                    }

                    body1.ApplyForce(ref f);
                    f = -f;
                    body2.ApplyForce(ref f);
                }

                foreach (Vector2 point in Points)
                {
                    Vector2 d = point - body1.Position;
                    float r2 = d.LengthSquared;

                    if (r2 < Settings.Epsilon)
                        continue;

                    float r = d.Length;

                    if (r >= MaxRadius || r <= MinRadius)
                        continue;

                    switch (GravityType)
                    {
                        case GravityType.DistanceSquared:
                            f = Strength / r2 / (float)Math.Sqrt(r2) * body1.Mass * d;
                            break;
                        case GravityType.Linear:
                            f = Strength / r2 * body1.Mass * d;
                            break;
                    }

                    body1.ApplyForce(ref f);
                }
            }
        }

        public void AddBody(Body body)
        {
            Bodies.Add(body);
        }

        public void AddPoint(Vector2 point)
        {
            Points.Add(point);
        }
    }
}