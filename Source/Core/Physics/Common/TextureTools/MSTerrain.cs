using System.Collections.Generic;
using Duality;
using Duality.Drawing;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;
using FarseerPhysics.Factories;

namespace FarseerPhysics.Common
{
	public enum Decomposer
	{
		Bayazit,
		CDT,
		Earclip,
		Flipcode,
		Seidel,
	}

	/// <summary>
	/// Return true if the specified color is inside the terrain.
	/// </summary>
	public delegate bool TerrainTester(ColorRgba color);

	/// <summary>
	/// Simple class to maintain a terrain.
	/// </summary>
	public class MSTerrain
	{
		/// <summary>
		/// World to manage terrain in.
		/// </summary>
		public World World;

		/// <summary>
		/// Center of terrain in world units.
		/// </summary>
		public Vector2 Center;

		/// <summary>
		/// Width of terrain in world units.
		/// </summary>
		public float Width;

		/// <summary>
		/// Height of terrain in world units.
		/// </summary>
		public float Height;

		/// <summary>
		/// Points per each world unit used to define the terrain in the point cloud.
		/// </summary>
		public int PointsPerUnit;

		/// <summary>
		/// Points per cell.
		/// </summary>
		public int CellSize;

		/// <summary>
		/// Points per sub cell.
		/// </summary>
		public int SubCellSize;

		/// <summary>
		/// Number of iterations to perform in the Marching Squares algorithm.
		/// Note: More then 3 has almost no effect on quality.
		/// </summary>
		public int Iterations = 2;

		/// <summary>
		/// Decomposer to use when regenerating terrain. Can be changed on the fly without consequence.
		/// Note: Some decomposerers are unstable.
		/// </summary>
		public Decomposer Decomposer;

		/// <summary>
		/// Point cloud defining the terrain.
		/// </summary>
		private sbyte[,] _terrainMap;

		/// <summary>
		/// Generated bodies.
		/// </summary>
		private List<Body>[,] _bodyMap;

		private float _localWidth;
		private float _localHeight;
		private int _xnum;
		private int _ynum;
		private AABB _dirtyArea;
		private Vector2 _topLeft;

		public MSTerrain(World world, AABB area)
		{
			this.World = world;
			this.Width = area.Extents.X * 2;
			this.Height = area.Extents.Y * 2;
			this.Center = area.Center;
		}

		/// <summary>
		/// Initialize the terrain for use.
		/// </summary>
		public void Initialize()
		{
			// find top left of terrain in world space
			this._topLeft = new Vector2(this.Center.X - (this.Width * 0.5f), this.Center.Y - (-this.Height * 0.5f));

			// convert the terrains size to a point cloud size
			this._localWidth = this.Width * this.PointsPerUnit;
			this._localHeight = this.Height * this.PointsPerUnit;

			this._terrainMap = new sbyte[(int)this._localWidth + 1, (int)this._localHeight + 1];

			for (int x = 0; x < this._localWidth; x++)
			{
				for (int y = 0; y < this._localHeight; y++)
				{
					this._terrainMap[x, y] = 1;
				}
			}

			this._xnum = (int)(this._localWidth / this.CellSize);
			this._ynum = (int)(this._localHeight / this.CellSize);
			this._bodyMap = new List<Body>[this._xnum, this._ynum];

			// make sure to mark the dirty area to an infinitely small box
			this._dirtyArea = new AABB(new Vector2(float.MaxValue, float.MaxValue), new Vector2(float.MinValue, float.MinValue));
		}

		/// <summary>
		/// Apply a texture to the terrain using the specified TerrainTester.
		/// </summary>
		/// <param name="texture">Texture to apply.</param>
		/// <param name="position">Top left position of the texture relative to the terrain.</param>
		/// <param name="tester">Delegate method used to determine what colors should be included in the terrain.</param>
		public void ApplyTexture(int texture, Vector2 position, TerrainTester tester)
		{
			throw new System.NotImplementedException();
			//Color[] colorData = new Color[texture.Width * texture.Height];

			//texture.GetData(colorData);

			//for (int y = (int)position.Y; y < texture.Height + (int)position.Y; y++)
			//{
			//    for (int x = (int)position.X; x < texture.Width + (int)position.X; x++)
			//    {
			//        if (x >= 0 && x < _localWidth && y >= 0 && y < _localHeight)
			//        {
			//            bool inside = tester(colorData[((y - (int)position.Y) * texture.Width) + (x - (int)position.X)]);

			//            if (!inside)
			//                _terrainMap[x, y] = 1;
			//            else
			//                _terrainMap[x, y] = -1;
			//        }
			//    }
			//}

			//// generate terrain
			//for (int gy = 0; gy < _ynum; gy++)
			//{
			//    for (int gx = 0; gx < _xnum; gx++)
			//    {
			//        //remove old terrain object at grid cell
			//        if (_bodyMap[gx, gy] != null)
			//        {
			//            for (int i = 0; i < _bodyMap[gx, gy].Count; i++)
			//            {
			//                World.RemoveBody(_bodyMap[gx, gy][i]);
			//            }
			//        }

			//        _bodyMap[gx, gy] = null;

			//        //generate new one
			//        GenerateTerrain(gx, gy);
			//    }
			//}
		}

		/// <summary>
		/// Apply a texture to the terrain using the specified TerrainTester.
		/// </summary>
		/// <param name="position">Top left position of the texture relative to the terrain.</param>
		/// <param name="data"></param>
		public void ApplyData(sbyte[,] data, Vector2 position)
		{
			for (int y = (int)position.Y; y < data.GetUpperBound(1) + (int)position.Y; y++)
			{
				for (int x = (int)position.X; x < data.GetUpperBound(0) + (int)position.X; x++)
				{
					if (x >= 0 && x < this._localWidth && y >= 0 && y < this._localHeight)
					{
						this._terrainMap[x, y] = data[x, y];
					}
				}
			}

			// generate terrain
			for (int gy = 0; gy < this._ynum; gy++)
			{
				for (int gx = 0; gx < this._xnum; gx++)
				{
					//remove old terrain object at grid cell
					if (this._bodyMap[gx, gy] != null)
					{
						for (int i = 0; i < this._bodyMap[gx, gy].Count; i++)
						{
							this.World.RemoveBody(this._bodyMap[gx, gy][i]);
						}
					}

					this._bodyMap[gx, gy] = null;

					//generate new one
					GenerateTerrain(gx, gy);
				}
			}
		}

		/// <summary>
		/// Convert a texture to an sbtye array compatible with ApplyData().
		/// </summary>
		/// <param name="texture">Texture to convert.</param>
		/// <param name="tester"></param>
		public static sbyte[,] ConvertTextureToData(int texture, TerrainTester tester)
		{
			throw new System.NotImplementedException();
			//sbyte[,] data = new sbyte[texture.Width, texture.Height];
			//Color[] colorData = new Color[texture.Width * texture.Height];

			//texture.GetData(colorData);

			//for (int y = 0; y < texture.Height; y++)
			//{
			//    for (int x = 0; x < texture.Width; x++)
			//    {
			//        bool inside = tester(colorData[(y * texture.Width) + x]);

			//        if (!inside)
			//            data[x, y] = 1;
			//        else
			//            data[x, y] = -1;
			//    }
			//}

			//return data;
		}

		/// <summary>
		/// Modify a single point in the terrain.
		/// </summary>
		/// <param name="location">World location to modify. Automatically clipped.</param>
		/// <param name="value">-1 = inside terrain, 1 = outside terrain</param>
		public void ModifyTerrain(Vector2 location, sbyte value)
		{
			// find local position
			// make position local to map space
			Vector2 p = location - this._topLeft;

			// find map position for each axis
			p.X = p.X * this._localWidth / this.Width;
			p.Y = p.Y * -this._localHeight / this.Height;

			if (p.X >= 0 && p.X < this._localWidth && p.Y >= 0 && p.Y < this._localHeight)
			{
				this._terrainMap[(int)p.X, (int)p.Y] = value;

				// expand dirty area
				if (p.X < this._dirtyArea.LowerBound.X) this._dirtyArea.LowerBound.X = p.X;
				if (p.X > this._dirtyArea.UpperBound.X) this._dirtyArea.UpperBound.X = p.X;

				if (p.Y < this._dirtyArea.LowerBound.Y) this._dirtyArea.LowerBound.Y = p.Y;
				if (p.Y > this._dirtyArea.UpperBound.Y) this._dirtyArea.UpperBound.Y = p.Y;
			}
		}

		/// <summary>
		/// Regenerate the terrain.
		/// </summary>
		public void RegenerateTerrain()
		{
			//iterate effected cells
			int gx0 = (int)(this._dirtyArea.LowerBound.X / this.CellSize);
			int gx1 = (int)(this._dirtyArea.UpperBound.X / this.CellSize) + 1;
			if (gx0 < 0) gx0 = 0;
			if (gx1 > this._xnum) gx1 = this._xnum;
			int gy0 = (int)(this._dirtyArea.LowerBound.Y / this.CellSize);
			int gy1 = (int)(this._dirtyArea.UpperBound.Y / this.CellSize) + 1;
			if (gy0 < 0) gy0 = 0;
			if (gy1 > this._ynum) gy1 = this._ynum;

			for (int gx = gx0; gx < gx1; gx++)
			{
				for (int gy = gy0; gy < gy1; gy++)
				{
					//remove old terrain object at grid cell
					if (this._bodyMap[gx, gy] != null)
					{
						for (int i = 0; i < this._bodyMap[gx, gy].Count; i++)
						{
							this.World.RemoveBody(this._bodyMap[gx, gy][i]);
						}
					}

					this._bodyMap[gx, gy] = null;

					//generate new one
					GenerateTerrain(gx, gy);
				}
			}

			this._dirtyArea = new AABB(new Vector2(float.MaxValue, float.MaxValue), new Vector2(float.MinValue, float.MinValue));
		}

		private void GenerateTerrain(int gx, int gy)
		{
			float ax = gx * this.CellSize;
			float ay = gy * this.CellSize;

			List<Vertices> polys = MarchingSquares.DetectSquares(new AABB(new Vector2(ax, ay), new Vector2(ax + this.CellSize, ay + this.CellSize)), this.SubCellSize, this.SubCellSize, this._terrainMap, this.Iterations, true);
			if (polys.Count == 0) return;

			this._bodyMap[gx, gy] = new List<Body>();

			// create the scale vector
			Vector2 scale = new Vector2(1f / this.PointsPerUnit, 1f / -this.PointsPerUnit);

			// create physics object for this grid cell
			foreach (Vertices item in polys)
			{
				// does this need to be negative?
				item.Scale(ref scale);
				item.Translate(ref this._topLeft);
				item.ForceCounterClockWise();
				Vertices p = FarseerPhysics.Common.PolygonManipulation.SimplifyTools.CollinearSimplify(item);
				List<Vertices> decompPolys = new List<Vertices>();

				switch (this.Decomposer)
				{
					case Decomposer.Bayazit:
						decompPolys = Decomposition.BayazitDecomposer.ConvexPartition(p);
						break;
					case Decomposer.CDT:
						decompPolys = Decomposition.CDTDecomposer.ConvexPartition(p);
						break;
					case Decomposer.Earclip:
						decompPolys = Decomposition.EarclipDecomposer.ConvexPartition(p);
						break;
					case Decomposer.Flipcode:
						decompPolys = Decomposition.FlipcodeDecomposer.ConvexPartition(p);
						break;
					case Decomposer.Seidel:
						decompPolys = Decomposition.SeidelDecomposer.ConvexPartition(p, 0.001f);
						break;
					default:
						break;
				}

				foreach (Vertices poly in decompPolys)
				{
					if (poly.Count > 2)
						this._bodyMap[gx, gy].Add(BodyFactory.CreatePolygon(this.World, poly, 1));
				}
			}
		}
	}
}
