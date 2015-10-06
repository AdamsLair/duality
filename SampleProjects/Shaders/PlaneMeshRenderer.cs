using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaders
{
	[RequiredComponent(typeof(Transform))]
	public class PlaneMeshRenderer : Component, ICmpRenderer
	{
		private Rect rect;
		private ushort subdivisions;
		private VisibilityFlag visibilityGroup;
		private ContentRef<Material> material;
		private int offset;

		[DontSerialize]
		private VertexC1P3T2[] _calcVertices;
		[DontSerialize]
		private VertexC1P3T2[] _drawVertices;

		/// <summary>
		/// [GET / SET] The rectangular area the sprite occupies. Relative to the <see cref="GameObject"/>.
		/// </summary>
		[EditorHintDecimalPlaces(1)]
		public Rect Rect
		{
			get { return this.rect; }
			set { this.rect = value; }
		}
		/// <summary>
		/// [GET / SET] The <see cref="Duality.Resources.Material"/> that is used for rendering the sprite.
		/// </summary>
		public ContentRef<Material> Material
		{
			get { return this.material; }
			set { this.material = value; }
		}
		/// <summary>
		/// [GET / SET] A virtual Z offset that affects the order in which objects are drawn. If you want to assure an object is drawn after another one,
		/// just assign a higher Offset value to the background object.
		/// </summary>
		public int Offset
		{
			get { return this.offset; }
			set { this.offset = value; }
		}
		/// <summary>
		/// [GET] The internal Z-Offset added to the renderers vertices based on its <see cref="Offset"/> value.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float VertexZOffset
		{
			get { return this.offset * 0.01f; }
		}

		/// <summary>
		/// [GET / SET] the number of mesh subdivisions (min. 1)
		/// </summary>
		public ushort Subdivisions
		{
			get { return this.subdivisions; }
			set { this.subdivisions = (ushort)Math.Max(value, 1.0); }
		}

		/// <summary>
        /// [GET / SET] the VisibilityGroup
        /// </summary>
        public VisibilityFlag VisibilityGroup 
		{
			get { return this.visibilityGroup; }
			set { this.visibilityGroup = value; }
		}

		public PlaneMeshRenderer()
		{
			this.material = Duality.Resources.Material.Checkerboard;
			this.rect = Rect.Align(Alignment.Center, 0, 0, 50, 50);
			this.subdivisions = 1;
			this.visibilityGroup = VisibilityFlag.Group0;
		}

		float ICmpRenderer.BoundRadius
		{
			get { return this.rect.BoundingRadius; }
		}

		bool ICmpRenderer.IsVisible(Duality.Drawing.IDrawDevice device)
		{
			bool result = true;

			// Differing ScreenOverlay flag? Don't render!
			if ((device.VisibilityMask & VisibilityFlag.ScreenOverlay) != (VisibilityGroup & VisibilityFlag.ScreenOverlay))
			{
				result = false;
			}
			// No match in any VisibilityGroup? Don't render!
			if ((VisibilityGroup & device.VisibilityMask & VisibilityFlag.AllGroups) == VisibilityFlag.None)
			{
				result = false;
			}

			return result;
		}

		void ICmpRenderer.Draw(Duality.Drawing.IDrawDevice device)
		{
			PrepareVertices(device);
		}

		private void PrepareVertices(Duality.Drawing.IDrawDevice device)
		{
			int calcVerticesCount = (int)MathF.Pow(this.subdivisions + 1, 2);
			int drawVerticesCount = (int)MathF.Pow(this.subdivisions, 2) * 4;

			if (_calcVertices== null || _calcVertices.Length != calcVerticesCount)
			{
				_calcVertices = new VertexC1P3T2[calcVerticesCount];
			}

			if (_drawVertices == null || _drawVertices.Length != drawVerticesCount)
			{
				_drawVertices = new VertexC1P3T2[drawVerticesCount];
			}

			Vector3 posTemp = this.GameObj.Transform.Pos;
			float scaleTemp = 1.0f;
			device.PreprocessCoords(ref posTemp, ref scaleTemp);

			Vector2 xDot, yDot;
			MathF.GetTransformDotVec(this.GameObj.Transform.Angle, scaleTemp, out xDot, out yDot);

			Rect rectTemp = this.rect.Transformed(this.GameObj.Transform.Scale, this.GameObj.Transform.Scale);
			Vector2 topLeft = rectTemp.TopLeft;
			Vector2 bottomRight = rectTemp.BottomRight;

			MathF.TransformDotVec(ref topLeft, ref xDot, ref yDot);
			MathF.TransformDotVec(ref bottomRight, ref xDot, ref yDot);

			Vector2 area = bottomRight - topLeft;

			Vector3 deltaPos = new Vector3(area / this.subdivisions, 0);
			Vector2 deltaTex = Vector2.One / this.subdivisions;

			// calculate the subdivisions' vertices
			for (int y = 0; y <= this.subdivisions; y++)
			{
				int row = y * (this.subdivisions + 1);

				for(int x = 0; x <= this.subdivisions; x++)
				{
					Vector3 relativeDelta = new Vector3(x, y, 0);

					_calcVertices[row + x].Color = ColorRgba.White;
					_calcVertices[row + x].TexCoord = Vector2.Zero + (deltaTex * relativeDelta.Xy);
					_calcVertices[row + x].Pos = (deltaPos * relativeDelta);
					_calcVertices[row + x].Pos.Xy += topLeft;
					_calcVertices[row + x].Pos.Z += this.VertexZOffset;
					_calcVertices[row + x].Pos += posTemp;
				}
			}

			// map the subdivisions' vertices to quad vertices
			// i.e. quad [0,0] = vertices [0, 4, 5, 1]
			// the formula for the vertices are, given S = subdivisions + 1
			// v0 = S * y + x
			// v1 = S * (y + 1) + x
			// v2 = S * (y + 1) + (x + 1)
			// v3 = S * y + (x + 1)

			int S = this.subdivisions + 1;
			for (int y = 0; y < this.subdivisions; y++)
			{
				for (int x = 0; x < this.subdivisions; x++)
				{
					int k = (x * 4) + (y * this.subdivisions * 4);

					_drawVertices[k] = _calcVertices[S * y + x];
					_drawVertices[k + 1] = _calcVertices[S * (y + 1) + x];
					_drawVertices[k + 2] = _calcVertices[S * (y + 1) + (x + 1)];
					_drawVertices[k + 3] = _calcVertices[S * y + (x + 1)];
				}
			}

			device.AddVertices(this.material, VertexMode.Quads, _drawVertices);
		}
	}
}
