using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Duality.Resources;
using Duality.Backend;

namespace Duality.Drawing
{
	[DontSerialize]
	public class VertexDrawBatch
	{
		private VertexDeclaration        vertexType   = null;
		private RawList<VertexDrawRange> vertexRanges = null;
		private VertexMode               vertexMode   = VertexMode.Points;
		private BatchInfo                material     = null;
		
		public VertexDeclaration VertexType
		{
			get { return this.vertexType; }
		}
		public RawList<VertexDrawRange> VertexRanges
		{
			get { return this.vertexRanges; }
		}
		public VertexMode VertexMode
		{
			get { return this.vertexMode; }
		}
		public BatchInfo Material
		{
			get { return this.material; }
		}

		public VertexDrawBatch(VertexDeclaration type, RawList<VertexDrawRange> ranges, VertexMode mode, BatchInfo material)
		{
			this.vertexType = type;
			this.vertexRanges = ranges;
			this.vertexMode = mode;
			this.material = material;
		}
	}
}
