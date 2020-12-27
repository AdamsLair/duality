﻿using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Resources;
using Duality.Cloning;

namespace Duality.Drawing
{
	/// <summary>
	/// Provides high level drawing operations on top of an existing <see cref="IDrawDevice"/>. However, this class is not designed
	/// for drawing large batches of primitives / vertices at once. For large amounts of primitives you should consider directly 
	/// using the underlying IDrawDevice instead to achieve best Profile.
	/// </summary>
	[DontSerialize]
	public class Canvas
	{
		private IDrawDevice           device     = null;
		private List<CanvasState>     stateStack = new List<CanvasState>();
		private int                   stateCount = 0;
		private RawList<VertexC1P3T2> buffer     = new RawList<VertexC1P3T2>();


		/// <summary>
		/// [GET] The underlying <see cref="IDrawDevice"/> that is used for drawing.
		/// Use <see cref="Begin"/> and <see cref="End"/> to tie this <see cref="Canvas"/>
		/// to a device for rendering.
		/// </summary>
		public IDrawDevice DrawDevice
		{
			get { return this.device; }
		}
		/// <summary>
		/// [GET] The currently active <see cref="CanvasState"/>.
		/// </summary>
		public CanvasState State
		{
			get { return this.stateStack[this.stateCount - 1]; }
		}
		/// <summary>
		/// [GET] The available width to draw on this Canvas.
		/// </summary>
		public int Width
		{
			get { return MathF.RoundToInt(this.device.TargetSize.X); }
		}
		/// <summary>
		/// [GET] The available height to draw on this Canvas.
		/// </summary>
		public int Height
		{
			get { return MathF.RoundToInt(this.device.TargetSize.Y); }
		}
		

		public Canvas()
		{
			this.stateStack.Add(new CanvasState(this));
			this.stateCount = 1;
		}

		/// <summary>
		/// Prepares the <see cref="Canvas"/> for drawing using the specified <see cref="IDrawDevice"/>.
		/// </summary>
		/// <param name="device"></param>
		/// <seealso cref="End"/>
		public void Begin(IDrawDevice device)
		{
			if (device == null) throw new ArgumentNullException("device");
			if (this.device != null) throw new InvalidOperationException("Can't begin a drawing operation on a Canvas where one is already in progress.");

			this.device = device;
		}
		/// <summary>
		/// Ends the drawing operation that was started using <see cref="Begin"/>.
		/// </summary>
		/// <seealso cref="Begin"/>
		public void End()
		{
			if (this.device == null) throw new InvalidOperationException("Can't end a drawing operation on a Canvas where none is in progress.");

			this.device = null;
			this.stateCount = 1;
			this.buffer.Count = 0;
		}

		/// <summary>
		/// Adds a clone of the <see cref="Canvas.State">current state</see> on top of the internal
		/// <see cref="CanvasState"/> stack.
		/// </summary>
		public void PushState()
		{
			this.stateCount++;

			if (this.stateStack.Count <= this.stateCount)
				this.stateStack.Add(new CanvasState(this));

			CanvasState oldState = this.stateStack[this.stateCount - 2];
			CanvasState newState = this.stateStack[this.stateCount - 1];

			oldState.CopyTo(newState);
		}
		/// <summary>
		/// Removes the topmost <see cref="CanvasState"/> from the internal State stack.
		/// </summary>
		public void PopState()
		{
			if (this.stateCount <= 1) throw new InvalidOperationException("Can't pop the last CanvasState from the stack.");
			this.stateCount--;
		}


		/// <summary>
		/// Draws a predefined set of vertices using the Canvas transformation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="vertices"></param>
		/// <param name="mode"></param>
		public void DrawVertices<T>(T[] vertices, VertexMode mode) where T : struct, IVertexData
		{
			this.DrawVertices<T>(vertices, mode, vertices.Length);
		}
		/// <summary>
		/// Draws part of a predefined set of vertices using the Canvas transformation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="vertices"></param>
		/// <param name="mode"></param>
		/// <param name="vertexCount"></param>
		public void DrawVertices<T>(T[] vertices, VertexMode mode, int vertexCount) where T : struct, IVertexData
		{
			if (vertexCount == 0) return;
			if (vertices.Length == 0) return;
			if (vertexCount < 0) throw new ArgumentException("Vertex count cannot be negative.", "vertexCount");
			if (vertexCount > vertices.Length) throw new ArgumentException("Specified vertex count is higher than the size of the vertex array.", "vertexCount");

			this.State.TransformVertices(vertices, vertices[0].Pos.Xy, vertexCount);
			this.device.AddVertices<T>(this.State.MaterialDirect, mode, vertices, vertexCount);
		}

		/// <summary>
		/// Draws a convex polygon. All vertices share the same Z value.
		/// </summary>
		/// <param name="points"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void DrawPolygon(Vector2[] points, float x, float y, float z = 0.0f)
		{
			Vector3 pos = new Vector3(x, y, z);

			float offset = this.State.DepthOffset;
			ColorRgba shapeColor = this.State.ColorTint;
			Rect texCoordRect = this.State.TextureCoordinateRect;
			VertexC1P3T2[] vertices = this.RentVertices(points.Length);
			for (int i = 0; i < points.Length; i++)
			{
				vertices[i].Pos.X = points[i].X + pos.X + 0.5f;
				vertices[i].Pos.Y = points[i].Y + pos.Y + 0.5f;
				vertices[i].Pos.Z = pos.Z;
				vertices[i].DepthOffset = offset;
				vertices[i].TexCoord.X = texCoordRect.X + texCoordRect.W * (float)i / (float)(points.Length - 1);
				vertices[i].TexCoord.Y = texCoordRect.Y;
				vertices[i].Color = shapeColor;
			}

			this.State.TransformVertices(vertices, pos.Xy);
			this.device.AddVertices(this.State.MaterialDirect, VertexMode.LineLoop, vertices, points.Length);
		}

		/// <summary>
		/// Draws a three-dimensional sphere.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="r"></param>
		public void DrawSphere(float x, float y, float z, float r)
		{
			r = MathF.Abs(r);
			Vector3 pos = new Vector3(x, y, z);
			if (!this.device.IsSphereInView(pos, r)) return;

			float projectedSizeAtPos = this.device.GetScaleAtZ(pos.Z);

			int segmentNum = MathF.Clamp(MathF.RoundToInt(MathF.Pow(r * projectedSizeAtPos, 0.65f) * 2.5f), 4, 128);
			Vector2 shapeHandle = pos.Xy;
			float offset = this.State.DepthOffset;
			ColorRgba shapeColor = this.State.ColorTint;
			Rect texCoordRect = this.State.TextureCoordinateRect;
			VertexC1P3T2[] vertices;
			float angle;

			// XY circle
			vertices = this.RentVertices(segmentNum);
			angle = 0.0f;
			for (int i = 0; i < segmentNum; i++)
			{
				vertices[i].Pos.X = pos.X + (float)Math.Sin(angle) * r;
				vertices[i].Pos.Y = pos.Y - (float)Math.Cos(angle) * r;
				vertices[i].Pos.Z = pos.Z;
				vertices[i].DepthOffset = offset;
				vertices[i].TexCoord.X = texCoordRect.X + texCoordRect.W * (float)i / (float)(segmentNum - 1);
				vertices[i].TexCoord.Y = texCoordRect.Y;
				vertices[i].Color = shapeColor;
				angle += (MathF.TwoPi / segmentNum);
			}
			this.State.TransformVertices(vertices, shapeHandle);
			this.device.AddVertices(this.State.MaterialDirect, VertexMode.LineLoop, vertices, segmentNum);

			// XZ circle
			vertices = this.RentVertices(segmentNum);
			angle = 0.0f;
			for (int i = 0; i < segmentNum; i++)
			{
				vertices[i].Pos.X = pos.X + (float)Math.Sin(angle) * r;
				vertices[i].Pos.Y = pos.Y;
				vertices[i].Pos.Z = pos.Z - (float)Math.Cos(angle) * r;
				vertices[i].DepthOffset = offset;
				vertices[i].TexCoord.X = texCoordRect.X + texCoordRect.W * (float)i / (float)(segmentNum - 1);
				vertices[i].TexCoord.Y = texCoordRect.Y;
				vertices[i].Color = shapeColor;
				angle += (MathF.TwoPi / segmentNum);
			}
			this.State.TransformVertices(vertices, shapeHandle);
			this.device.AddVertices(this.State.MaterialDirect, VertexMode.LineLoop, vertices, segmentNum);

			// YZ circle
			vertices = this.RentVertices(segmentNum);
			angle = 0.0f;
			for (int i = 0; i < segmentNum; i++)
			{
				vertices[i].Pos.X = pos.X;
				vertices[i].Pos.Y = pos.Y + (float)Math.Sin(angle) * r;
				vertices[i].Pos.Z = pos.Z - (float)Math.Cos(angle) * r;
				vertices[i].DepthOffset = offset;
				vertices[i].TexCoord.X = texCoordRect.X + texCoordRect.W * (float)i / (float)(segmentNum - 1);
				vertices[i].TexCoord.Y = texCoordRect.Y;
				vertices[i].Color = shapeColor;
				angle += (MathF.TwoPi / segmentNum);
			}
			this.State.TransformVertices(vertices, shapeHandle);
			this.device.AddVertices(this.State.MaterialDirect, VertexMode.LineLoop, vertices, segmentNum);
		}

		/// <summary>
		/// Draws a three-dimensional line.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="z2"></param>
		public void DrawLine(float x, float y, float z, float x2, float y2, float z2)
		{
			Vector3 pos = new Vector3(x, y, z);
			Vector3 target = new Vector3(x2, y2, z2);

			Vector2 shapeHandle = pos.Xy;
			float offset = this.State.DepthOffset;
			ColorRgba shapeColor = this.State.ColorTint;
			Rect texCoordRect = this.State.TextureCoordinateRect;
			VertexC1P3T2[] vertices = this.RentVertices(2);

			vertices[0].Pos = pos + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[1].Pos = target + new Vector3(0.5f, 0.5f, 0.0f);

			vertices[0].DepthOffset = offset;
			vertices[1].DepthOffset = offset;

			vertices[0].TexCoord = new Vector2(texCoordRect.X, 0.0f);
			vertices[1].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W, 0.0f);

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;

			this.State.TransformVertices(vertices, shapeHandle);
			this.device.AddVertices(this.State.MaterialDirect, VertexMode.Lines, vertices, 2);
		}
		/// <summary>
		/// Draws a flat line.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		public void DrawLine(float x, float y, float x2, float y2)
		{
			this.DrawLine(x, y, 0, x2, y2, 0);
		}
		/// <summary>
		/// Draws a three-dimensional line.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="z2"></param>
		/// <param name="pattern"></param>
		/// <param name="patternLen"></param>
		public void DrawDashLine(float x, float y, float z, float x2, float y2, float z2, DashPattern pattern = DashPattern.Dash, float patternLen = 1.0f)
		{
			uint patternBits = (uint)pattern;
			string dashTextPath = string.Format("__DashLineTexture{0}__", patternBits);
			ContentRef<Texture> dashTexRef = new ContentRef<Texture>(null, dashTextPath);
			if (!dashTexRef.IsAvailable)
			{
				PixelData pxLayerDash = new PixelData(32, 1);
				for (int i = 31; i >= 0; i--) pxLayerDash[i, 0] = ((patternBits & (1U << i)) != 0) ? ColorRgba.White : ColorRgba.TransparentWhite;
				Pixmap pxDash = new Pixmap(pxLayerDash);
				Texture texDash = new Texture(pxDash, TextureSizeMode.Stretch, TextureMagFilter.Nearest, TextureMinFilter.Nearest, TextureWrapMode.Repeat);
				ContentProvider.AddContent(dashTextPath, texDash);
			}

			Vector3 pos = new Vector3(x, y, z);
			Vector3 target = new Vector3(x2, y2, z2);
			float lineLength = (target - pos).Length;

			Vector2 shapeHandle = pos.Xy;
			float offset = this.State.DepthOffset;
			ColorRgba shapeColor = this.State.ColorTint;
			VertexC1P3T2[] vertices = this.RentVertices(2);
			vertices[0].Pos = pos + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[1].Pos = target + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[0].DepthOffset = offset;
			vertices[1].DepthOffset = offset;
			vertices[0].TexCoord = new Vector2(0.0f, 0.0f);
			vertices[1].TexCoord = new Vector2(lineLength * patternLen / 32.0f, 0.0f);
			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;

			BatchInfo customMat = this.device.RentMaterial(this.State.MaterialDirect);
			customMat.MainTexture = dashTexRef;
			this.State.TransformVertices(vertices, shapeHandle);
			this.device.AddVertices(customMat, VertexMode.Lines, vertices, 2);
		}
		/// <summary>
		/// Draws a flat line.
		/// </summary>
		public void DrawDashLine(float x, float y, float x2, float y2, DashPattern pattern = DashPattern.Dash, float patternLen = 1.0f)
		{
			this.DrawDashLine(x, y, 0, x2, y2, 0, pattern, patternLen);
		}
		/// <summary>
		/// Draws a thick, three-dimensional line.
		/// </summary>
		public void DrawThickLine(float x, float y, float z, float x2, float y2, float z2, float width)
		{
			Vector3 pos = new Vector3(x, y, z);
			Vector3 target = new Vector3(x2, y2, z2);

			Vector2 dir = (target.Xy - pos.Xy).Normalized;
			Vector2 left = dir.PerpendicularLeft * width * 0.5f;
			Vector2 right = dir.PerpendicularRight * width * 0.5f;
			Vector2 left2 = dir.PerpendicularLeft * width * 0.5f;
			Vector2 right2 = dir.PerpendicularRight * width * 0.5f;

			Vector2 shapeHandle = pos.Xy;
			float offset = this.State.DepthOffset;
			ColorRgba shapeColor = this.State.ColorTint;
			Rect texCoordRect = this.State.TextureCoordinateRect;
			VertexC1P3T2[] vertices = this.RentVertices(4);

			vertices[0].Pos = pos + new Vector3(left);
			vertices[1].Pos = target + new Vector3(left2);
			vertices[2].Pos = target + new Vector3(right2);
			vertices[3].Pos = pos + new Vector3(right);

			vertices[0].DepthOffset = offset;
			vertices[1].DepthOffset = offset;
			vertices[2].DepthOffset = offset;
			vertices[3].DepthOffset = offset;

			vertices[0].TexCoord = new Vector2(texCoordRect.X, 0.0f);
			vertices[1].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W * 0.3333333f, 0.0f);
			vertices[2].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W * 0.6666666f, 0.0f);
			vertices[3].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W, 0.0f);

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;

			this.State.TransformVertices(vertices, shapeHandle);
			this.device.AddVertices(this.State.MaterialDirect, VertexMode.LineLoop, vertices, 4);
		}
		/// <summary>
		/// Draws a thick, flat line.
		/// </summary>
		public void DrawThickLine(float x, float y, float x2, float y2, float width)
		{
			this.DrawThickLine(x, y, 0, x2, y2, 0, width);
		}

		/// <summary>
		/// Draws the specified texture in its original pixel size at the specified position.
		/// 
		/// This is a shortcut for using <see cref="CanvasState.SetMaterial"/> with a temporary material
		/// using the specified texture and specifying the appropriate size values in <see cref="FillRect"/>.
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void DrawImage(ContentRef<Texture> texture, float x, float y, float z = 0.0f)
		{
			BatchInfo tempMaterial = this.device.RentMaterial(this.State.MaterialDirect);
			tempMaterial.MainTexture = texture;

			Vector3 pos = new Vector3(x, y, z);
			Vector2 size = tempMaterial.MainTexture.Res.ContentSize;

			Vector2 shapeHandle = pos.Xy;
			float offset = this.State.DepthOffset;
			ColorRgba shapeColor = this.State.ColorTint;
			VertexC1P3T2[] vertices = this.RentVertices(4);

			Vector2 uvRatio = Vector2.One;
			if (tempMaterial.MainTexture.IsAvailable)
				uvRatio = tempMaterial.MainTexture.Res.UVRatio;

			vertices[0].Pos = new Vector3(pos.X, pos.Y, pos.Z);
			vertices[1].Pos = new Vector3(pos.X + size.X, pos.Y, pos.Z);
			vertices[2].Pos = new Vector3(pos.X + size.X, pos.Y + size.Y, pos.Z);
			vertices[3].Pos = new Vector3(pos.X, pos.Y + size.Y, pos.Z);

			vertices[0].DepthOffset = offset;
			vertices[1].DepthOffset = offset;
			vertices[2].DepthOffset = offset;
			vertices[3].DepthOffset = offset;

			vertices[0].TexCoord = Vector2.Zero;
			vertices[1].TexCoord = Vector2.UnitX * uvRatio;
			vertices[2].TexCoord = Vector2.One * uvRatio;
			vertices[3].TexCoord = Vector2.UnitY * uvRatio;

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;

			this.State.TransformVertices(vertices, shapeHandle);
			this.device.AddVertices(tempMaterial, VertexMode.Quads, vertices, 4);
		}

		/// <summary>
		/// Draws a rectangle.
		/// </summary>
		public void DrawRect(float x, float y, float z, float w, float h)
		{
			if (w < 0.0f) { x += w; w = -w; }
			if (h < 0.0f) { y += h; h = -h; }

			Vector3 pos = new Vector3(x, y, z);

			Vector2 shapeHandle = pos.Xy;
			float offset = this.State.DepthOffset;
			ColorRgba shapeColor = this.State.ColorTint;
			Rect texCoordRect = this.State.TextureCoordinateRect;
			VertexC1P3T2[] vertices = this.RentVertices(4);

			vertices[0].Pos = new Vector3(pos.X + 0.5f, pos.Y + 0.5f, pos.Z);
			vertices[1].Pos = new Vector3(pos.X + w - 0.5f, pos.Y + 0.5f, pos.Z);
			vertices[2].Pos = new Vector3(pos.X + w - 0.5f, pos.Y + h - 0.5f, pos.Z);
			vertices[3].Pos = new Vector3(pos.X + 0.5f, pos.Y + h - 0.5f, pos.Z);

			vertices[0].DepthOffset = offset;
			vertices[1].DepthOffset = offset;
			vertices[2].DepthOffset = offset;
			vertices[3].DepthOffset = offset;

			vertices[0].TexCoord = new Vector2(texCoordRect.X, 0.0f);
			vertices[1].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W * 0.3333333f, 0.0f);
			vertices[2].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W * 0.6666666f, 0.0f);
			vertices[3].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W, 0.0f);

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;

			this.State.TransformVertices(vertices, shapeHandle);
			this.device.AddVertices(this.State.MaterialDirect, VertexMode.LineLoop, vertices, 4);
		}
		/// <summary>
		/// Draws a rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void DrawRect(float x, float y, float w, float h)
		{
			this.DrawRect(x, y, 0, w, h);
		}
		
		/// <summary>
		/// Draws the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="width">The rendered ovals total width.</param>
		/// <param name="height">The rendered ovals total height.</param>
		/// <param name="minAngle">The oval segments minimum angle.</param>
		/// <param name="maxAngle">The oval segments maximum angle.</param>
		/// <param name="outline">If true, the oval sections complete outline is drawn instead of just the outer perimeter.</param>
		public void DrawOvalSegment(float x, float y, float z, float width, float height, float minAngle, float maxAngle, bool outline = false)
		{
			if (minAngle == maxAngle) return;
			if (width < 0.0f) { x += width; width = -width; }
			if (height < 0.0f) { y += height; height = -height; }
			width *= 0.5f; x += width;
			height *= 0.5f; y += height;

			Vector3 pos = new Vector3(x, y, z);
			if (!this.device.IsSphereInView(pos, MathF.Max(width, height) + this.State.TransformHandle.Length)) return;

			if (maxAngle <= minAngle)
				maxAngle += MathF.Ceiling((minAngle - maxAngle) / MathF.RadAngle360) * MathF.RadAngle360;

			float angleRange = MathF.Min(maxAngle - minAngle, MathF.RadAngle360);
			bool loop = angleRange >= MathF.RadAngle360 - MathF.RadAngle1 * 0.001f;

			if (loop && outline)
				outline = false;
			else if (outline)
				loop = true;

			float projectedSizeAtPos = this.device.GetScaleAtZ(pos.Z);
			int segmentNum = MathF.Clamp(MathF.RoundToInt(MathF.Pow(MathF.Max(width, height) * projectedSizeAtPos, 0.65f) * 3.5f * angleRange / MathF.RadAngle360), 4, 128);
			float angleStep = angleRange / segmentNum;
			Vector2 shapeHandle = pos.Xy - new Vector2(width, height);
			float offset = this.State.DepthOffset;
			ColorRgba shapeColor = this.State.ColorTint;
			Rect texCoordRect = this.State.TextureCoordinateRect;
			int vertexCount = segmentNum + (loop ? 0 : 1) + (outline ? 2 : 0);
			VertexC1P3T2[] vertices = this.RentVertices(vertexCount);
			float angle = minAngle;
			
			if (outline)
			{
				vertices[0].Pos.X = pos.X + 0.5f;
				vertices[0].Pos.Y = pos.Y + 0.5f;
				vertices[0].Pos.Z = pos.Z;
				vertices[0].DepthOffset = offset;
				vertices[0].TexCoord.X = texCoordRect.X;
				vertices[0].TexCoord.Y = texCoordRect.Y;
				vertices[0].Color = shapeColor;
			}

			// XY circle
			for (int i = outline ? 1 : 0; i < vertexCount; i++)
			{
				vertices[i].Pos.X = pos.X + (float)Math.Sin(angle) * (width - 0.5f);
				vertices[i].Pos.Y = pos.Y - (float)Math.Cos(angle) * (height - 0.5f);
				vertices[i].Pos.Z = pos.Z;
				vertices[i].DepthOffset = offset;
				vertices[i].TexCoord.X = texCoordRect.X + texCoordRect.W * (float)i / (float)(vertexCount - 1);
				vertices[i].TexCoord.Y = texCoordRect.Y;
				vertices[i].Color = shapeColor;
				angle += angleStep;
			}
			this.State.TransformVertices(vertices, shapeHandle);
			this.device.AddVertices(this.State.MaterialDirect, loop ? VertexMode.LineLoop : VertexMode.LineStrip, vertices, vertexCount);
		}
		/// <summary>
		/// Draws the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width">The rendered ovals total width.</param>
		/// <param name="height">The rendered ovals total height.</param>
		/// <param name="minAngle">The oval segments minimum angle.</param>
		/// <param name="maxAngle">The oval segments maximum angle.</param>
		/// <param name="outline">If true, the oval sections complete outline is drawn instead of just the outer perimeter.</param>
		public void DrawOvalSegment(float x, float y, float width, float height, float minAngle, float maxAngle, bool outline = false)
		{
			this.DrawOvalSegment(x, y, 0, width, height, minAngle, maxAngle, outline);
		}
		/// <summary>
		/// Draws the section of a circle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="radius">The rendered circles radius.</param>
		/// <param name="minAngle">The circle segments minimum angle.</param>
		/// <param name="maxAngle">The circle segments maximum angle.</param>
		/// <param name="outline">If true, the circle sections complete outline is drawn instead of just the outer perimeter.</param>
		public void DrawCircleSegment(float x, float y, float z, float radius, float minAngle, float maxAngle, bool outline = false)
		{
			this.State.TransformHandle += new Vector2(radius, radius);
			this.DrawOvalSegment(x, y, z, radius * 2, radius * 2, minAngle, maxAngle, outline);
			this.State.TransformHandle -= new Vector2(radius, radius);
		}
		/// <summary>
		/// Draws the section of a circle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="radius">The rendered circles radius.</param>
		/// <param name="minAngle">The circle segments minimum angle.</param>
		/// <param name="maxAngle">The circle segments maximum angle.</param>
		/// <param name="outline">If true, the circle sections complete outline is drawn instead of just the outer perimeter.</param>
		public void DrawCircleSegment(float x, float y, float radius, float minAngle, float maxAngle, bool outline = false)
		{
			this.State.TransformHandle += new Vector2(radius, radius);
			this.DrawOvalSegment(x, y, 0, radius * 2, radius * 2, minAngle, maxAngle, outline);
			this.State.TransformHandle -= new Vector2(radius, radius);
		}

		/// <summary>
		/// Draws the section of an oval.
		/// </summary>
		public void DrawOval(float x, float y, float z, float width, float height)
		{
			this.DrawOvalSegment(x, y, z, width, height, 0.0f, MathF.RadAngle360);
		}
		/// <summary>
		/// Draws the section of an oval.
		/// </summary>
		public void DrawOval(float x, float y, float width, float height)
		{
			this.DrawOvalSegment(x, y, 0, width, height, 0.0f, MathF.RadAngle360);
		}
		/// <summary>
		/// Draws the section of a circle.
		/// </summary>
		public void DrawCircle(float x, float y, float z, float radius)
		{
			this.State.TransformHandle += new Vector2(radius, radius);
			this.DrawOvalSegment(x, y, z, radius * 2, radius * 2, 0.0f, MathF.RadAngle360);
			this.State.TransformHandle -= new Vector2(radius, radius);
		}
		/// <summary>
		/// Draws the section of a circle
		/// </summary>
		public void DrawCircle(float x, float y, float radius)
		{
			this.State.TransformHandle += new Vector2(radius, radius);
			this.DrawOvalSegment(x, y, 0, radius * 2, radius * 2, 0.0f, MathF.RadAngle360);
			this.State.TransformHandle -= new Vector2(radius, radius);
		}
		
		/// <summary>
		/// Fills a polygon. All vertices share the same Z value, and the polygon needs to be convex.
		/// </summary>
		public void FillPolygon(Vector2[] points, float x, float y, float z = 0.0f)
		{
			Vector3 pos = new Vector3(x, y, z);

			float offset = this.State.DepthOffset;
			ColorRgba shapeColor = this.State.ColorTint;
			Rect texCoordRect = this.State.TextureCoordinateRect;

			// Determine bounding box
			Rect pointBoundingRect = points.BoundingBox();

			// Set up vertex array
			VertexC1P3T2[] vertices = this.RentVertices(points.Length);
			for (int i = 0; i < points.Length; i++)
			{
				vertices[i].Pos.X = points[i].X + pos.X;
				vertices[i].Pos.Y = points[i].Y + pos.Y;
				vertices[i].Pos.Z = pos.Z;
				vertices[i].DepthOffset = offset;
				vertices[i].TexCoord.X = texCoordRect.X + ((points[i].X - pointBoundingRect.X) / pointBoundingRect.W) * texCoordRect.W;
				vertices[i].TexCoord.Y = texCoordRect.Y + ((points[i].Y - pointBoundingRect.Y) / pointBoundingRect.H) * texCoordRect.H;
				vertices[i].Color = shapeColor;
			}

			this.State.TransformVertices(vertices, pos.Xy);
			this.device.AddVertices(this.State.MaterialDirect, VertexMode.TriangleFan, vertices, points.Length);
		}
		/// <summary>
		/// Fills a polygons outline. All vertices share the same Z value.
		/// </summary>
		/// <param name="points"></param>
		/// <param name="width"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void FillPolygonOutline(Vector2[] points, float width, float x, float y, float z = 0.0f)
		{
			this.FillThickOutline(points, width, 1.0f, x, y, z, true);
		}
		/// <summary>
		/// Fills a polygons outline. All vertices share the same Z value.
		/// </summary>
		/// <param name="points"></param>
		/// <param name="width"></param>
		/// <param name="inOutFactor">
		/// A factor that determines on which side of the polygon the line will be drawn, ranging from -1 to 1.
		/// Zero represents a line that is centered on the original polygon.
		/// </param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void FillPolygonOutline(Vector2[] points, float width, float inOutFactor, float x, float y, float z = 0.0f)
		{
			this.FillThickOutline(points, width, inOutFactor, x, y, z, true);
		}

		/// <summary>
		/// Fills a three-dimensional line.
		/// </summary>
		public void FillThickLine(float x, float y, float z, float x2, float y2, float z2, float width)
		{
			Vector3 pos = new Vector3(x, y, z);
			Vector3 target = new Vector3(x2, y2, z2);

			Vector2 dir = (target.Xy - pos.Xy).Normalized;
			Vector2 left = dir.PerpendicularLeft * width * 0.5f;
			Vector2 right = dir.PerpendicularRight * width * 0.5f;
			Vector2 left2 = dir.PerpendicularLeft * width * 0.5f;
			Vector2 right2 = dir.PerpendicularRight * width * 0.5f;

			Vector2 shapeHandle = pos.Xy;
			float offset = this.State.DepthOffset;
			ColorRgba shapeColor = this.State.ColorTint;
			Rect texCoordRect = this.State.TextureCoordinateRect;
			VertexC1P3T2[] vertices = this.RentVertices(4);

			vertices[0].Pos = pos + new Vector3(left);
			vertices[1].Pos = target + new Vector3(left2);
			vertices[2].Pos = target + new Vector3(right2);
			vertices[3].Pos = pos + new Vector3(right);

			vertices[0].DepthOffset = offset;
			vertices[1].DepthOffset = offset;
			vertices[2].DepthOffset = offset;
			vertices[3].DepthOffset = offset;

			vertices[0].TexCoord = texCoordRect.TopLeft;
			vertices[1].TexCoord = texCoordRect.TopRight;
			vertices[2].TexCoord = texCoordRect.BottomRight;
			vertices[3].TexCoord = texCoordRect.BottomLeft;

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;

			this.State.TransformVertices(vertices, shapeHandle);
			this.device.AddVertices(this.State.MaterialDirect, VertexMode.Quads, vertices, 4);
		}
		/// <summary>
		/// Fills a thick, flat line.
		/// </summary>
		public void FillThickLine(float x, float y, float x2, float y2, float width)
		{
			this.FillThickLine(x, y, 0, x2, y2, 0, width);
		}
		/// <summary>
		/// Fills a thick line strip. All vertices share the same Z value.
		/// </summary>
		/// <param name="points"></param>
		/// <param name="width"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void FillThickLineStrip(Vector2[] points, float width, float x, float y, float z = 0.0f)
		{
			this.FillThickOutline(points, width, 1.0f, x, y, z, false);
		}
		/// <summary>
		/// Fills a thick line strip. All vertices share the same Z value.
		/// </summary>
		/// <param name="points"></param>
		/// <param name="width"></param>
		/// <param name="inOutFactor">
		/// A factor that determines on which side of the polygon the line will be drawn, ranging from -1 to 1.
		/// Zero represents a line that is centered on the original polygon.
		/// </param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void FillThickLineStrip(Vector2[] points, float width, float inOutFactor, float x, float y, float z = 0.0f)
		{
			this.FillThickOutline(points, width, inOutFactor, x, y, z, false);
		}
		
		/// <summary>
		/// Fills the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="width">The rendered ovals total width.</param>
		/// <param name="height">The rendered ovals total height.</param>
		/// <param name="minAngle">The oval segments minimum angle.</param>
		/// <param name="maxAngle">The oval segments maximum angle.</param>
		/// <param name="donutWidth">If bigger than zero, a donut with the specified width is rendered instead of a completely filled oval.</param>
		public void FillOvalSegment(float x, float y, float z, float width, float height, float minAngle, float maxAngle, float donutWidth = 0.0f)
		{
			if (minAngle == maxAngle) return;
			if (width < 0.0f) { x += width; width = -width; }
			if (height < 0.0f) { y += height; height = -height; }
			if (donutWidth > MathF.Min(width, height)) donutWidth = 0.0f;
			width *= 0.5f; x += width;
			height *= 0.5f; y += height;

			Vector3 pos = new Vector3(x, y, z);
			if (!this.device.IsSphereInView(pos, MathF.Max(width, height) + this.State.TransformHandle.Length)) return;

			if (maxAngle <= minAngle)
				maxAngle += MathF.Ceiling((minAngle - maxAngle) / MathF.RadAngle360) * MathF.RadAngle360;

			float angleRange = MathF.Min(maxAngle - minAngle, MathF.RadAngle360);

			float projectedSizeAtPos = this.device.GetScaleAtZ(pos.Z);
			int segmentNum = MathF.Clamp(MathF.RoundToInt(MathF.Pow(MathF.Max(width, height) * projectedSizeAtPos, 0.65f) * 3.5f * angleRange / MathF.RadAngle360), 4, 128);
			float angleStep = angleRange / segmentNum;
			Vector2 shapeHandle = pos.Xy - new Vector2(width, height);
			float offset = this.State.DepthOffset;
			ColorRgba shapeColor = this.State.ColorTint;
			Rect texCoordRect = this.State.TextureCoordinateRect;
			int vertexCount;
			VertexC1P3T2[] vertices;

			if (donutWidth <= 0.0f)
			{
				vertexCount = segmentNum + 2;
				vertices = this.RentVertices(vertexCount);
				vertices[0].Pos = pos;
				vertices[0].DepthOffset = offset;
				vertices[0].Color = shapeColor;
				vertices[0].TexCoord = texCoordRect.Center;
				float angle = minAngle;
				for (int i = 1; i < vertexCount; i++)
				{
					float sin = (float)Math.Sin(angle);
					float cos = (float)Math.Cos(angle); 
					vertices[i].Pos.X = pos.X + sin * width;
					vertices[i].Pos.Y = pos.Y - cos * height;
					vertices[i].Pos.Z = pos.Z;
					vertices[i].DepthOffset = offset;
					vertices[i].Color = shapeColor;
					vertices[i].TexCoord.X = texCoordRect.X + (0.5f + 0.5f * sin) * texCoordRect.W;
					vertices[i].TexCoord.Y = texCoordRect.Y + (0.5f - 0.5f * cos) * texCoordRect.H;
					angle += angleStep;
				}
				this.State.TransformVertices(vertices, shapeHandle);
				this.device.AddVertices(this.State.MaterialDirect, VertexMode.TriangleFan, vertices, vertexCount);
			}
			else
			{
				vertexCount = (segmentNum + 1) * 2;
				vertices = this.RentVertices(vertexCount);
				float angle = minAngle;
				Vector2 donutWidthTexCoord = 0.5f * donutWidth * Vector2.One / new Vector2(width, height);
				for (int i = 0; i < vertexCount; i += 2)
				{
					float sin = (float)Math.Sin(angle);
					float cos = (float)Math.Cos(angle); 

					vertices[i + 0].Pos.X = pos.X + sin * width;
					vertices[i + 0].Pos.Y = pos.Y - cos * height;
					vertices[i + 0].Pos.Z = pos.Z;
					vertices[i + 0].DepthOffset = offset;
					vertices[i + 0].Color = shapeColor;
					vertices[i + 0].TexCoord.X = texCoordRect.X + (0.5f + 0.5f * sin) * texCoordRect.W;
					vertices[i + 0].TexCoord.Y = texCoordRect.Y + (0.5f - 0.5f * cos) * texCoordRect.H;

					vertices[i + 1].Pos.X = pos.X + sin * (width - donutWidth);
					vertices[i + 1].Pos.Y = pos.Y - cos * (height - donutWidth);
					vertices[i + 1].Pos.Z = pos.Z;
					vertices[i + 1].DepthOffset = offset;
					vertices[i + 1].Color = shapeColor;
					vertices[i + 1].TexCoord.X = texCoordRect.X + (0.5f + (0.5f - donutWidthTexCoord.X) * sin) * texCoordRect.W;
					vertices[i + 1].TexCoord.Y = texCoordRect.Y + (0.5f - (0.5f - donutWidthTexCoord.Y) * cos) * texCoordRect.H;

					angle += angleStep;
				}
				this.State.TransformVertices(vertices, shapeHandle);
				this.device.AddVertices(this.State.MaterialDirect, VertexMode.TriangleStrip, vertices, vertexCount);
			}
		}
		/// <summary>
		/// Fills the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width">The rendered ovals total width.</param>
		/// <param name="height">The rendered ovals total height.</param>
		/// <param name="minAngle">The oval segments minimum angle.</param>
		/// <param name="maxAngle">The oval segments maximum angle.</param>
		public void FillOvalSegment(float x, float y, float width, float height, float minAngle, float maxAngle)
		{
			this.FillOvalSegment(x, y, 0, width, height, minAngle, maxAngle, 0);
		}
		/// <summary>
		/// Fills the section of a circle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="radius">The circles radius.</param>
		/// <param name="minAngle">The circle segments minimum angle.</param>
		/// <param name="maxAngle">The circle segments maximum angle.</param>
		/// <param name="donutWidth">If bigger than zero, a donut with the specified width is rendered instead of a completely filled circle area.</param>
		public void FillCircleSegment(float x, float y, float z, float radius, float minAngle, float maxAngle, float donutWidth = 0.0f)
		{
			this.State.TransformHandle += new Vector2(radius, radius);
			this.FillOvalSegment(x, y, z, radius * 2, radius * 2, minAngle, maxAngle, donutWidth);
			this.State.TransformHandle -= new Vector2(radius, radius);
		}
		/// <summary>
		/// Fills the section of a circle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="r"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void FillCircleSegment(float x, float y, float r, float minAngle, float maxAngle)
		{
			this.State.TransformHandle += new Vector2(r, r);
			this.FillOvalSegment(x, y, 0, r * 2, r * 2, minAngle, maxAngle, 0);
			this.State.TransformHandle -= new Vector2(r, r);
		}

		/// <summary>
		/// Fills an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void FillOval(float x, float y, float z, float width, float height)
		{
			this.FillOvalSegment(x, y, z, width, height, 0.0f, MathF.RadAngle360, 0.0f);
		}
		/// <summary>
		/// Fills an oval
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void FillOval(float x, float y, float width, float height)
		{
			this.FillOvalSegment(x, y, 0, width, height, 0.0f, MathF.RadAngle360, 0.0f);
		}
		/// <summary>
		/// Fills a circle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="radius"></param>
		public void FillCircle(float x, float y, float z, float radius)
		{
			this.State.TransformHandle += new Vector2(radius, radius);
			this.FillOvalSegment(x, y, z, radius * 2, radius * 2, 0.0f, MathF.RadAngle360, 0.0f);
			this.State.TransformHandle -= new Vector2(radius, radius);
		}
		/// <summary>
		/// Fills a circle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="r"></param>
		public void FillCircle(float x, float y, float r)
		{
			this.State.TransformHandle += new Vector2(r, r);
			this.FillOvalSegment(x, y, 0, r * 2, r * 2, 0.0f, MathF.RadAngle360, 0.0f);
			this.State.TransformHandle -= new Vector2(r, r);
		}

		/// <summary>
		/// Fills a rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void FillRect(float x, float y, float z, float width, float height)
		{
			if (width < 0.0f) { x += width; width = -width; }
			if (height < 0.0f) { y += height; height = -height; }

			Vector3 pos = new Vector3(x, y, z);

			Vector2 shapeHandle = pos.Xy;
			float offset = this.State.DepthOffset;
			ColorRgba shapeColor = this.State.ColorTint;
			Rect texCoordRect = this.State.TextureCoordinateRect;
			VertexC1P3T2[] vertices = this.RentVertices(4);

			vertices[0].Pos = new Vector3(pos.X, pos.Y, pos.Z);
			vertices[1].Pos = new Vector3(pos.X + width, pos.Y, pos.Z);
			vertices[2].Pos = new Vector3(pos.X + width, pos.Y + height, pos.Z);
			vertices[3].Pos = new Vector3(pos.X, pos.Y + height, pos.Z);

			vertices[0].DepthOffset = offset;
			vertices[1].DepthOffset = offset;
			vertices[2].DepthOffset = offset;
			vertices[3].DepthOffset = offset;

			vertices[0].TexCoord = texCoordRect.TopLeft;
			vertices[1].TexCoord = texCoordRect.TopRight;
			vertices[2].TexCoord = texCoordRect.BottomRight;
			vertices[3].TexCoord = texCoordRect.BottomLeft;

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;

			this.State.TransformVertices(vertices, shapeHandle);
			this.device.AddVertices(this.State.MaterialDirect, VertexMode.Quads, vertices, 4);
		}
		/// <summary>
		/// Fills a rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void FillRect(float x, float y, float width, float height)
		{
			this.FillRect(x, y, 0, width, height);
		}

		/// <summary>
		/// Draws the specified text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block.</param>
		/// <param name="drawBackground"></param>
		public void DrawText(string text, float x, float y, float z = 0.0f, Alignment blockAlign = Alignment.TopLeft, bool drawBackground = false)
		{
			if(!string.IsNullOrEmpty(text))
				this.DrawText(new string[] { text }, x, y, z, blockAlign, drawBackground);
		}
		/// <summary>
		/// Draws the specified text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="vertices">Optional vertex cache to use for the text. If set, the texts vertices are cached and re-used for better Profile.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block. To make use of individual line alignment, use the <see cref="FormattedText"/> overload.</param>
		/// <param name="drawBackground"></param>
		public void DrawText(string[] text, ref VertexC1P3T2[][] vertices, float x, float y, float z = 0.0f, Alignment blockAlign = Alignment.TopLeft, bool drawBackground = false)
		{
			if (text == null || text.Length == 0) return;
			Font font = this.State.TextFont.Res;

			Vector2 textSize = Vector2.Zero;
			if (blockAlign != Alignment.TopLeft)
			{
				if (textSize == Vector2.Zero) textSize = this.MeasureText(text);
				Vector2 blockAlignVec = Vector2.Zero;
				blockAlign.ApplyTo(
					ref blockAlignVec.X, 
					ref blockAlignVec.Y, 
					textSize.X * this.State.TransformScale.X, 
					textSize.Y * this.State.TransformScale.Y);
				MathF.TransformCoord(ref blockAlignVec.X, ref blockAlignVec.Y, this.State.TransformAngle);
				x += blockAlignVec.X;
				y += blockAlignVec.Y;
			}

			if (drawBackground)
			{
				if (textSize == Vector2.Zero) textSize = this.MeasureText(text);
				Rect padding = new Rect(font.Height * 0.7f, font.Height * 0.7f);
				padding.X = padding.W * this.State.TransformScale.X * 0.5f;
				padding.Y = padding.H * this.State.TransformScale.Y * 0.5f;
				MathF.TransformCoord(ref padding.X, ref padding.Y, this.State.TransformAngle);

				ColorRgba baseColor = this.State.ColorTint;
				const float backAlpha = 0.65f;
				float baseAlpha = (float)baseColor.A / 255.0f;
				float baseLuminance = baseColor.GetLuminance();

				this.PushState();
				this.State.SetMaterial(DrawTechnique.Alpha);
				this.State.ColorTint = (baseLuminance > 0.5f ? ColorRgba.Black : ColorRgba.White).WithAlpha(baseAlpha * backAlpha);
				this.State.DepthOffset += 1.0f;
				this.FillRect(
					x - padding.X, 
					y - padding.Y, 
					z,
					textSize.X + padding.W, 
					textSize.Y + padding.H);
				this.PopState();
			}

			Vector3 pos = new Vector3(x, y, z);
			
			if (font.IsPixelGridAligned)
			{
				pos.X = MathF.Round(pos.X);
				pos.Y = MathF.Round(pos.Y);
				bool worldSpace = (this.device.VisibilityMask & VisibilityFlag.ScreenOverlay) == VisibilityFlag.None;
				if (worldSpace)
				{
					if (MathF.RoundToInt(this.device.TargetSize.X) != (MathF.RoundToInt(this.device.TargetSize.X) / 2) * 2) pos.X += 0.5f;
					if (MathF.RoundToInt(this.device.TargetSize.Y) != (MathF.RoundToInt(this.device.TargetSize.Y) / 2) * 2) pos.Y += 0.5f;
				}
			}
			Vector2 shapeHandle = pos.Xy;
			
			BatchInfo material;
			if (this.State.IsDefaultMaterial)
			{
				material = font.Material.Info;
			}
			else
			{
				material = this.device.RentMaterial(this.State.MaterialDirect);
				material.MainTexture = font.Material.MainTexture;
			}

			// Prepare for attempt to use Canvas buffering
			if (vertices == null || vertices.Length < text.Length)
				vertices = new VertexC1P3T2[text.Length][];

			Vector2 size = Vector2.Zero;
			for (int i = 0; i < text.Length; i++)
			{
				// Attempt to use the internal vertex buffer of this Canvas
				if (vertices[i] == null || vertices[i].Length < text[i].Length * 4)
					vertices[i] = this.RentVertices(text[i].Length * 4);

				int vertexCount = font.EmitTextVertices(text[i], ref vertices[i], pos.X, pos.Y, pos.Z, this.State.ColorTint);

				// Apply depth offset to generated vertices
				float offset = this.State.DepthOffset;
				for (int k = 0; k < vertexCount; k++)
				{
					vertices[i][k].DepthOffset = offset;
				}

				this.State.TransformVertices(vertices[i], shapeHandle);
				this.device.AddVertices(material, VertexMode.Quads, vertices[i], vertexCount);

				pos.Y += font.LineSpacing;
			}
		}
		/// <summary>
		/// Draws the specified text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block. To make use of individual line alignment, use the <see cref="FormattedText"/> overload.</param>
		/// <param name="drawBackground"></param>
		public void DrawText(string[] text, float x, float y, float z = 0.0f, Alignment blockAlign = Alignment.TopLeft, bool drawBackground = false)
		{
			VertexC1P3T2[][] vertices = null;
			this.DrawText(text, ref vertices, x, y, z, blockAlign, drawBackground);
		}
		/// <summary>
		/// Draws the specified formatted text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="vertText">Optional vertex cache to use for the text. If set, the texts vertices are cached and re-used for better Profile.</param>
		/// <param name="vertIcon">Optional vertex cache to use for the icons. If set, the texts vertices are cached and re-used for better Profile.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="iconMat"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block. To make use of individual line alignment, make use of <see cref="FormattedText"/> format tags.</param>
		/// <param name="drawBackground"></param>
		public void DrawText(FormattedText text, ref VertexC1P3T2[][] vertText, ref VertexC1P3T2[] vertIcon, float x, float y, float z = 0.0f, BatchInfo iconMat = null, Alignment blockAlign = Alignment.TopLeft, bool drawBackground = false)
		{
			if (text == null || text.IsEmpty) return;

			if (blockAlign != Alignment.TopLeft)
			{
				Vector2 alignTextSize = text.Size;
				if (text.MaxWidth > 0) alignTextSize.X = text.MaxWidth;

				Vector2 blockAlignVec = Vector2.Zero;
				blockAlign.ApplyTo(
					ref blockAlignVec.X, 
					ref blockAlignVec.Y, 
					alignTextSize.X * this.State.TransformScale.X, 
					alignTextSize.Y * this.State.TransformScale.Y);
				MathF.TransformCoord(ref blockAlignVec.X, ref blockAlignVec.Y, this.State.TransformAngle);
				x += blockAlignVec.X;
				y += blockAlignVec.Y;
			}

			if (drawBackground)
			{
				Font font = this.State.TextFont.Res;
				Rect padding = new Rect(font.Height * 0.7f, font.Height * 0.7f);
				padding.X = padding.W * this.State.TransformScale.X * 0.5f;
				padding.Y = padding.H * this.State.TransformScale.Y * 0.5f;
				MathF.TransformCoord(ref padding.X, ref padding.Y, this.State.TransformAngle);

				ColorRgba baseColor = this.State.ColorTint;
				const float backAlpha = 0.65f;
				float baseAlpha = (float)baseColor.A / 255.0f;
				float baseLuminance = baseColor.GetLuminance();

				this.PushState();
				this.State.SetMaterial(DrawTechnique.Alpha);
				this.State.ColorTint = (baseLuminance > 0.5f ? ColorRgba.Black : ColorRgba.White).WithAlpha(baseAlpha * backAlpha);
				this.FillRect(
					x - padding.X, 
					y - padding.Y, 
					z,
					text.Size.X + padding.W, 
					text.Size.Y + padding.H);
				this.PopState();
			}

			Vector3 pos = new Vector3(x, y, z);
			
			if (text.Fonts != null && text.Fonts.Any(r => r.IsAvailable && r.Res.IsPixelGridAligned))
			{
				pos.X = MathF.Round(pos.X);
				pos.Y = MathF.Round(pos.Y);
				bool worldSpace = ((this.device.VisibilityMask & VisibilityFlag.ScreenOverlay) == VisibilityFlag.None);
				if (worldSpace)
				{
					if (MathF.RoundToInt(this.device.TargetSize.X) != (MathF.RoundToInt(this.device.TargetSize.X) / 2) * 2) pos.X += 0.5f;
					if (MathF.RoundToInt(this.device.TargetSize.Y) != (MathF.RoundToInt(this.device.TargetSize.Y) / 2) * 2) pos.Y += 0.5f;
				}
			}
			Vector2 shapeHandle = pos.Xy;
			int[] vertLen = text.EmitVertices(ref vertText, ref vertIcon, pos.X, pos.Y, pos.Z, this.State.ColorTint);

			// Apply depth offset to generated vertices
			float offset = this.State.DepthOffset;
			for (int i = 0; i < vertText.Length; i++)
			{
				for (int j = 0; j < vertLen[i + 1]; j++)
				{
					vertText[i][j].DepthOffset = offset;
				}
			}
			for (int i = 0; i < vertLen[0]; i++)
			{
				vertIcon[i].DepthOffset = offset;
			}

			if (text.Fonts != null)
			{
				for (int i = 0; i < text.Fonts.Length; i++)
				{
					if (text.Fonts[i] != null && text.Fonts[i].IsAvailable) 
					{
						this.State.TransformVertices(vertText[i], shapeHandle);
						BatchInfo material;
						if (this.State.IsDefaultMaterial)
						{
							material = text.Fonts[i].Res.Material.Info;
						}
						else
						{
							material = this.device.RentMaterial(this.State.MaterialDirect);
							material.MainTexture = text.Fonts[i].Res.Material.MainTexture;
						}
						this.device.AddVertices(material, VertexMode.Quads, vertText[i], vertLen[i + 1]);
					}
				}
			}
			if (text.Icons != null && iconMat != null)
			{
				this.device.AddVertices(iconMat, VertexMode.Quads, vertIcon, vertLen[0]);
			}
		}
		/// <summary>
		/// Draws the specified formatted text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="iconMat"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block. To make use of individual line alignment, make use of <see cref="FormattedText"/> format tags.</param>
		/// <param name="drawBackground"></param>
		public void DrawText(FormattedText text, float x, float y, float z = 0.0f, BatchInfo iconMat = null, Alignment blockAlign = Alignment.TopLeft, bool drawBackground = false)
		{
			VertexC1P3T2[][] vertText = null;
			VertexC1P3T2[] vertIcon = null;
			this.DrawText(text, ref vertText, ref vertIcon, x, y, z, iconMat, blockAlign, drawBackground);
		}

		/// <summary>
		/// Measures the specified text using the currently used <see cref="Duality.Resources.Font"/>.
		/// </summary>
		/// <param name="text"></param>
		public Vector2 MeasureText(string text)
		{
			Font font = this.State.TextFont.Res;
			return font.MeasureText(text);
		}
		/// <summary>
		/// Measures the specified text using the currently used <see cref="Duality.Resources.Font"/>.
		/// </summary>
		/// <param name="text"></param>
		public Vector2 MeasureText(string[] text)
		{
			Font font = this.State.TextFont.Res;
			return font.MeasureText(text);
		}

		/// <summary>
		/// Draws a thick line strip or loop.
		/// </summary>
		/// <param name="points"></param>
		/// <param name="width">The width of the filled line.</param>
		/// <param name="inOutFactor">
		/// A factor that determines on which side of the polygon the line will be drawn, ranging from -1 to 1.
		/// Zero represents a line that is centered on the original polygon.
		/// </param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="closedLoop"></param>
		private void FillThickOutline(Vector2[] points, float width, float inOutFactor, float x, float y, float z, bool closedLoop)
		{
			// When specifying a negative width, flip the in / out factor
			// and work with a positive width from here.
			if (width < 0.0f)
			{
				inOutFactor = -inOutFactor;
				width = MathF.Abs(width);
			}

			width *= 0.5f;
			Vector3 pos = new Vector3(x, y, z);

			float offset = this.State.DepthOffset;
			ColorRgba shapeColor = this.State.ColorTint;
			Rect texCoordRect = this.State.TextureCoordinateRect;
			
			// Determine line width on the inside and outside of the original polygon
			inOutFactor = MathF.Clamp(inOutFactor, -1.0f, 1.0f);
			float innerScale = 0.5f - 0.5f * inOutFactor;
			float outerScale = 0.5f + 0.5f * inOutFactor;

			// Determine bounding box
			Rect pointBoundingRect = points.BoundingBox();
			pointBoundingRect.X -= width * 0.5f;
			pointBoundingRect.Y -= width * 0.5f;
			pointBoundingRect.W += width;
			pointBoundingRect.H += width;

			// Set up vertex array
			int vertexCount = points.Length * 4 + (closedLoop ? 2 : 0);
			VertexC1P3T2[] vertices = this.RentVertices(vertexCount);
			for (int i = 0; i < points.Length; i++)
			{
				int vertexBase = i * 4;

				int currentIndex = i;
				int prevIndex = (i - 1 + points.Length) % points.Length;
				int nextIndex = (i + 1) % points.Length;
				
				Vector2 current = points[currentIndex];
				Vector2 prev = points[prevIndex];
				Vector2 next = points[nextIndex];

				// For duplicate points, duplicate vertices as well
				if (i > 0 && current == prev)
				{
					int prevVertexBase = ((i - 1 + points.Length) % points.Length) * 4;
					vertices[vertexBase + 0] = vertices[prevVertexBase + 0];
					vertices[vertexBase + 1] = vertices[prevVertexBase + 1];
					vertices[vertexBase + 2] = vertices[prevVertexBase + 2];
					vertices[vertexBase + 3] = vertices[prevVertexBase + 3];
					continue;
				}
				
				Vector2 tangent = (current - prev).Normalized;
				Vector2 tangent2 = (next - current).Normalized;
				Vector2 normal = tangent.PerpendicularLeft;
				Vector2 normal2 = tangent2.PerpendicularLeft;
				
				float normalDot = Vector2.Dot(normal, tangent2);

				Vector2 offsetA;
				Vector2 offsetB;
				
				// Special cases for first and last vertex when not rendering a loop
				if (!closedLoop && i == 0)
				{
					offsetA = normal2 * width * 2.0f;
					offsetB = offsetA;
				}
				else if (!closedLoop && i == points.Length - 1)
				{
					offsetA = normal * width * 2.0f;
					offsetB = offsetA;
				}
				// Avoid the "parallel lines" edge case by using just the first 
				// line segment's orientation and ignoring the second.
				else if (MathF.Abs(normalDot) < 0.0001f)
				{
					offsetA = normal * width * 2.0f;
					offsetB = offsetA;
				}
				// Calculate the point where the two joining line segments cross
				// and joing them in a sharp angle.
				else
				{
					Vector2 cross;
					MathF.LinesCross(
						prev.X - normal.X * width, prev.Y - normal.Y * width, 
						current.X - normal.X * width, current.Y - normal.Y * width, 
						current.X - normal2.X * width, current.Y - normal2.Y * width,
						next.X - normal2.X * width, next.Y - normal2.Y * width,
						out cross.X, out cross.Y,
						true);

					float tangentDot = Vector2.Dot(tangent, -tangent2);

					// Calculate the propre sharp / miter joint vertex
					float sharpEdgeLength = MathF.Min((cross - current).Length, width * 10.0f);
					Vector2 sharpEdgeOffset = (tangent - tangent2).Normalized * sharpEdgeLength * MathF.Sign(normalDot);

					// Sharp outward edges: Use bevel joints
					// Right angles, inward and blunt edges: Use miter joints
					float sharpEdgeFactor = MathF.Clamp(tangentDot * 1.5f, 0.0f, 1.0f);
					float outwardEdgeFactor = MathF.Clamp((-normalDot * inOutFactor + 0.125f) / 0.25f, 0.0f, 1.0f);
					float bevelFactor = sharpEdgeFactor * outwardEdgeFactor;

					offsetA = Vector2.Lerp(-sharpEdgeOffset, normal * width, bevelFactor) * 2.0f;
					offsetB = Vector2.Lerp(-sharpEdgeOffset, normal2 * width, bevelFactor) * 2.0f;
				}

				vertices[vertexBase + 0].Pos.X = (current.X - offsetA.X * innerScale) + pos.X;
				vertices[vertexBase + 0].Pos.Y = (current.Y - offsetA.Y * innerScale) + pos.Y;
				vertices[vertexBase + 0].Pos.Z = pos.Z;
				vertices[vertexBase + 0].DepthOffset = offset;
				vertices[vertexBase + 0].TexCoord.X = texCoordRect.X + ((current.X - pointBoundingRect.X) / pointBoundingRect.W) * texCoordRect.W;
				vertices[vertexBase + 0].TexCoord.Y = texCoordRect.Y + ((current.Y - pointBoundingRect.Y) / pointBoundingRect.H) * texCoordRect.H;
				vertices[vertexBase + 0].Color = shapeColor;
				
				vertices[vertexBase + 1].Pos.X = (current.X + offsetA.X * outerScale) + pos.X;
				vertices[vertexBase + 1].Pos.Y = (current.Y + offsetA.Y * outerScale) + pos.Y;
				vertices[vertexBase + 1].Pos.Z = pos.Z;
				vertices[vertexBase + 1].DepthOffset = offset;
				vertices[vertexBase + 1].TexCoord.X = texCoordRect.X + ((current.X + offsetA.X - pointBoundingRect.X) / pointBoundingRect.W) * texCoordRect.W;
				vertices[vertexBase + 1].TexCoord.Y = texCoordRect.Y + ((current.Y + offsetA.Y - pointBoundingRect.Y) / pointBoundingRect.H) * texCoordRect.H;
				vertices[vertexBase + 1].Color = shapeColor;

				vertices[vertexBase + 2].Pos.X = (current.X - offsetB.X * innerScale) + pos.X;
				vertices[vertexBase + 2].Pos.Y = (current.Y - offsetB.Y * innerScale) + pos.Y;
				vertices[vertexBase + 2].Pos.Z = pos.Z;
				vertices[vertexBase + 2].DepthOffset = offset;
				vertices[vertexBase + 2].TexCoord.X = texCoordRect.X + ((current.X - pointBoundingRect.X) / pointBoundingRect.W) * texCoordRect.W;
				vertices[vertexBase + 2].TexCoord.Y = texCoordRect.Y + ((current.Y - pointBoundingRect.Y) / pointBoundingRect.H) * texCoordRect.H;
				vertices[vertexBase + 2].Color = shapeColor;
				
				vertices[vertexBase + 3].Pos.X = (current.X + offsetB.X * outerScale) + pos.X;
				vertices[vertexBase + 3].Pos.Y = (current.Y + offsetB.Y * outerScale) + pos.Y;
				vertices[vertexBase + 3].Pos.Z = pos.Z;
				vertices[vertexBase + 3].DepthOffset = offset;
				vertices[vertexBase + 3].TexCoord.X = texCoordRect.X + ((current.X + offsetB.X - pointBoundingRect.X) / pointBoundingRect.W) * texCoordRect.W;
				vertices[vertexBase + 3].TexCoord.Y = texCoordRect.Y + ((current.Y + offsetB.Y - pointBoundingRect.Y) / pointBoundingRect.H) * texCoordRect.H;
				vertices[vertexBase + 3].Color = shapeColor;
			}

			if (closedLoop)
			{
				vertices[vertexCount - 2] = vertices[0];
				vertices[vertexCount - 1] = vertices[1]; 
			}

			this.State.TransformVertices(vertices, pos.Xy);
			this.device.AddVertices(this.State.MaterialDirect, VertexMode.TriangleStrip, vertices, vertexCount);
		}

		/// <summary>
		/// Rents a chunk from the internal vertex buffer. Can only rent one chunk
		/// at a time, to be re-used after submitting it to the device.
		/// </summary>
		/// <param name="minLength"></param>
		private VertexC1P3T2[] RentVertices(int minLength)
		{
			this.buffer.Count = minLength;
			return this.buffer.Data;
		}
	}
}
