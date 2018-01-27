using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Duality.Resources;
using Duality.Backend;

namespace Duality.Drawing
{
	/// <summary>
	/// Describes a rendering batch that can be executed as a whole without state changes.
	/// </summary>
	[DontSerialize]
	public class DrawBatch
	{
		private VertexBuffer             vertexBuffer = null;
		private RawList<VertexDrawRange> vertexRanges = null;
		private VertexMode               vertexMode   = VertexMode.Points;
		private BatchInfo                material     = null;
		
		/// <summary>
		/// [GET] The vertex buffer that stores the geometry to be rendered.
		/// </summary>
		public VertexBuffer VertexBuffer
		{
			get { return this.vertexBuffer; }
		}
		/// <summary>
		/// [GET] A list of continuous vertex ranges / indices to be rendered in this batch.
		/// </summary>
		public RawList<VertexDrawRange> VertexRanges
		{
			get { return this.vertexRanges; }
		}
		/// <summary>
		/// [GET] The <see cref="VertexMode"/> that is used to interpret the geometry 
		/// that is described in the specified vertices.
		/// </summary>
		public VertexMode VertexMode
		{
			get { return this.vertexMode; }
		}
		/// <summary>
		/// [GET] The material that is used when rendering this batch.
		/// </summary>
		public BatchInfo Material
		{
			get { return this.material; }
		}

		public DrawBatch(VertexBuffer buffer, RawList<VertexDrawRange> ranges, VertexMode mode, Material material) : this(buffer, ranges, mode, material.Info) { }
		public DrawBatch(VertexBuffer buffer, RawList<VertexDrawRange> ranges, VertexMode mode, BatchInfo material)
		{
			this.vertexBuffer = buffer;
			this.vertexRanges = ranges;
			this.vertexMode = mode;
			this.material = material;
		}
	}
}
