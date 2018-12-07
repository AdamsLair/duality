namespace FarseerPhysics.Common.Decomposition.Seidel
{
	internal class XNode : Node
	{
		private Point _point;

		public XNode(Point point, Node lChild, Node rChild)
			: base(lChild, rChild)
		{
			this._point = point;
		}

		public override Sink Locate(Edge edge)
		{
			if (edge.P.X >= this._point.X)
				return this.RightChild.Locate(edge); // Move to the right in the graph

			return this.LeftChild.Locate(edge); // Move to the left in the graph
		}
	}
}