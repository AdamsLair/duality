using System.Collections.Generic;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Duality;

namespace FarseerPhysics.Factories
{
	public static class LinkFactory
	{
		/// <summary>
		/// Creates a chain.
		/// </summary>
		/// <param name="world">The world.</param>
		/// <param name="start">The start.</param>
		/// <param name="end">The end.</param>
		/// <param name="linkWidth">The width.</param>
		/// <param name="linkHeight">The height.</param>
		/// <param name="numberOfLinks">The number of links.</param>
		/// <param name="linkDensity">The link density.</param>
		/// <returns></returns>
		public static Path CreateChain(World world, Vector2 start, Vector2 end, float linkWidth, float linkHeight, int numberOfLinks, float linkDensity)
		{
			//Chain start / end
			Path path = new Path();
			path.Add(start);
			path.Add(end);

			//A single chainlink
			PolygonShape shape = new PolygonShape(PolygonTools.CreateRectangle(linkWidth, linkHeight), linkDensity);

			//Use PathManager to create all the chainlinks based on the chainlink created before.
			List<Body> chainLinks = PathManager.EvenlyDistributeShapesAlongPath(world, path, shape, BodyType.Dynamic,
																				numberOfLinks);

			//Attach all the chainlinks together with a revolute joint
			PathManager.AttachBodiesWithRevoluteJoint(world, chainLinks, new Vector2(0, -linkHeight),
													  new Vector2(0, linkHeight),
													  false, false);

			return (path);
		}
	}
}