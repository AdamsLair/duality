namespace FarseerPhysics.Common.Decomposition.Seidel
{
	internal class YNode : Node
	{
		private Edge _edge;

		public YNode(Edge edge, Node lChild, Node rChild)
			: base(lChild, rChild)
		{
			this._edge = edge;
		}

		public override Sink Locate(Edge edge)
		{
			if (this._edge.IsAbove(edge.P))
				return this.RightChild.Locate(edge); // Move down the graph

			if (this._edge.IsBelow(edge.P))
				return this.LeftChild.Locate(edge); // Move up the graph

			// s and segment share the same endpoint, p
			if (edge.Slope < this._edge.Slope)
				return this.RightChild.Locate(edge); // Move down the graph

			// Move up the graph
			return this.LeftChild.Locate(edge);
		}
	}
}