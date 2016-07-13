using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components;
using Duality.Editor;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// A helper class that provides algorithms for determining the visible tile area for a given <see cref="IDrawDevice"/>.
	/// </summary>
	public static class TilemapCulling
	{
		/// <summary>
		/// Input parameters for a <see cref="Tilemap"/> culling operation, detailing world transform and
		/// tile extent.
		/// </summary>
		public struct TileInput
		{
			/// <summary>
			/// World space position of the <see cref="Tilemap"/>.
			/// </summary>
			public Vector3 TilemapPos;
			/// <summary>
			/// World space scale of the <see cref="Tilemap"/>.
			/// </summary>
			public float TilemapScale;
			/// <summary>
			/// World space rotation of the <see cref="Tilemap"/>.
			/// </summary>
			public float TilemapAngle;
			/// <summary>
			/// The size of a single tile in the <see cref="Tilemap"/>.
			/// </summary>
			public Vector2 TileSize;
			/// <summary>
			/// The total number of tiles in the <see cref="Tilemap"/>.
			/// </summary>
			public Point2 TileCount;
		}
		/// <summary>
		/// The end result of a <see cref="Tilemap"/> culling operation, specifying the rectangular area
		/// of the <see cref="Tilemap"/> that is to be rendered, as well as view space transform data
		/// which can be used for doing so.
		/// </summary>
		public struct TileOutput
		{
			/// <summary>
			/// The top left origin of the visible <see cref="Tilemap"/> rect, in tile coordinates.
			/// </summary>
			public Point2 VisibleTileStart;
			/// <summary>
			/// The number of visible tiles to render, starting from <see cref="VisibleTileStart"/>.
			/// </summary>
			public Point2 VisibleTileCount;
			/// <summary>
			/// The view space rendering origin of the visible tile rect.
			/// </summary>
			public Vector3 RenderOriginView;
			/// <summary>
			/// The view space x axis of the rendered <see cref="Tilemap"/>, taking rotation and scale into account.
			/// </summary>
			public Vector2 XAxisView;
			/// <summary>
			/// The view space y axis of the rendered <see cref="Tilemap"/>, taking rotation and scale into account.
			/// </summary>
			public Vector2 YAxisView;
			/// <summary>
			/// The world space rendering origin of the visible tile rect.
			/// </summary>
			public Vector3 RenderOriginWorld;
			/// <summary>
			/// The world space x axis of the rendered <see cref="Tilemap"/>, taking rotation and scale into account.
			/// </summary>
			public Vector2 XAxisWorld;
			/// <summary>
			/// The world space y axis of the rendered <see cref="Tilemap"/>, taking rotation and scale into account.
			/// </summary>
			public Vector2 YAxisWorld;
		}

		/// <summary>
		/// An empty culling result that indicates no rendering is necessary at all.
		/// </summary>
		public static readonly TileOutput EmptyOutput = new TileOutput();

		/// <summary>
		/// Determines the rectangular tile area that is visible to the specified <see cref="IDrawDevice"/>.
		/// </summary>
		/// <param name="device"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		public static TileOutput GetVisibleTileRect(IDrawDevice device, TileInput input)
		{
			TileOutput output;

			// Determine the view space transform of the tilemap
			Vector3 objPos = input.TilemapPos;
			float objScale = input.TilemapScale;
			float cameraScaleAtObj = 1.0f;
			device.PreprocessCoords(ref objPos, ref cameraScaleAtObj);
			objScale *= cameraScaleAtObj;

			// Early-out, if so small that it might break the math behind rendering a single tile.
			if (objScale <= 0.000000001f) return EmptyOutput;

			// Determine transformed X and Y axis in view and world space
			output.XAxisView = Vector2.UnitX;
			output.YAxisView = Vector2.UnitY;
			MathF.TransformCoord(ref output.XAxisView.X, ref output.XAxisView.Y, input.TilemapAngle, objScale);
			MathF.TransformCoord(ref output.YAxisView.X, ref output.YAxisView.Y, input.TilemapAngle, objScale);
			output.XAxisWorld = Vector2.UnitX;
			output.YAxisWorld = Vector2.UnitY;
			MathF.TransformCoord(ref output.XAxisWorld.X, ref output.XAxisWorld.Y, input.TilemapAngle, input.TilemapScale);
			MathF.TransformCoord(ref output.YAxisWorld.X, ref output.YAxisWorld.Y, input.TilemapAngle, input.TilemapScale);

			// Determine which tile is in the center of view space.
			Point2 viewCenterTile = Point2.Zero;
			{
				// Project the view center coordinate (view space zero) into the local space of the rendered tilemap
				Vector2 viewRenderStartPos = objPos.Xy;
				Vector2 localViewCenter = Vector2.Zero - viewRenderStartPos;
				localViewCenter = new Vector2(
					Vector2.Dot(localViewCenter, output.XAxisView.Normalized),
					Vector2.Dot(localViewCenter, output.YAxisView.Normalized)) / objScale;
				viewCenterTile = new Point2(
					(int)MathF.Floor(localViewCenter.X / input.TileSize.X),
					(int)MathF.Floor(localViewCenter.Y / input.TileSize.Y));
			}

			// Determine the edge length of a square that is big enough to enclose the world space rect of the Camera view
			float visualAngle = input.TilemapAngle - device.RefAngle;
			Vector2 visualBounds = new Vector2(
				device.TargetSize.Y * MathF.Abs(MathF.Sin(visualAngle)) + device.TargetSize.X * MathF.Abs(MathF.Cos(visualAngle)),
				device.TargetSize.X * MathF.Abs(MathF.Sin(visualAngle)) + device.TargetSize.Y * MathF.Abs(MathF.Cos(visualAngle)));
			Vector2 localVisualBounds = visualBounds / cameraScaleAtObj;
			Point2 targetVisibleTileCount = new Point2(
				3 + (int)MathF.Ceiling(localVisualBounds.X / (MathF.Min(input.TileSize.X, input.TileSize.Y) * input.TilemapScale)), 
				3 + (int)MathF.Ceiling(localVisualBounds.Y / (MathF.Min(input.TileSize.X, input.TileSize.Y) * input.TilemapScale)));

			// Determine the tile indices (xy) that are visible within that rect
			output.VisibleTileStart = new Point2(
				MathF.Max(viewCenterTile.X - targetVisibleTileCount.X / 2, 0),
				MathF.Max(viewCenterTile.Y - targetVisibleTileCount.Y / 2, 0));
			Point2 tileGridEndPos = new Point2(
				MathF.Min(viewCenterTile.X + targetVisibleTileCount.X / 2, input.TileCount.X),
				MathF.Min(viewCenterTile.Y + targetVisibleTileCount.Y / 2, input.TileCount.Y));
			output.VisibleTileCount = new Point2(
				MathF.Clamp(tileGridEndPos.X - output.VisibleTileStart.X, 0, input.TileCount.X),
				MathF.Clamp(tileGridEndPos.Y - output.VisibleTileStart.Y, 0, input.TileCount.Y));

			// Determine start position for rendering
			output.RenderOriginView = objPos + new Vector3(
				output.VisibleTileStart.X * output.XAxisView * input.TileSize.X + 
				output.VisibleTileStart.Y * output.YAxisView * input.TileSize.Y);
			output.RenderOriginWorld = input.TilemapPos + new Vector3(
				output.VisibleTileStart.X * output.XAxisWorld * input.TileSize.X + 
				output.VisibleTileStart.Y * output.YAxisWorld * input.TileSize.Y);

			return output;
		}
	}
}
