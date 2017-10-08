using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;
using Duality.Cloning;
using Duality.Components.Physics;
using Duality.Properties;

namespace Duality.Components.Renderers
{
	/// <summary>
	/// A <see cref="Duality.Component"/> that renders a RigidBodies shape and outline.
	/// </summary>
	[RequiredComponent(typeof(RigidBody))]
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageRigidBodyRenderer)]
	public class RigidBodyRenderer : Renderer
	{
		private ContentRef<Material> areaMaterial          = Material.Checkerboard;
		private ContentRef<Material> outlineMaterial       = Material.SolidWhite;
		private BatchInfo            customAreaMaterial    = null;
		private BatchInfo            customOutlineMaterial = null;
		private ColorRgba            colorTint             = ColorRgba.White;
		private float                outlineWidth          = 3.0f;
		private float                offset                = 0.0f;
		private bool                 fillHollowShapes      = false;
		private bool                 wrapTexture           = true;

		[DontSerialize] private Canvas canvas = new Canvas();


		public override float BoundRadius
		{
			get { return this.GameObj.GetComponent<RigidBody>().BoundRadius; }
		}
		/// <summary>
		/// [GET / SET] The <see cref="Duality.Resources.Material"/> that is used for rendering the RigidBodies shape areaa.
		/// </summary>
		public ContentRef<Material> AreaMaterial
		{
			get { return this.areaMaterial; }
			set { this.areaMaterial = value; }
		}
		/// <summary>
		/// [GET / SET] The <see cref="Duality.Resources.Material"/> that is used for rendering the RigidBodies shape outlines.
		/// </summary>
		public ContentRef<Material> OutlineMaterial
		{
			get { return this.outlineMaterial; }
			set { this.outlineMaterial = value; }
		}
		/// <summary>
		/// [GET / SET] A custom, local <see cref="Duality.Drawing.BatchInfo"/> overriding the <see cref="AreaMaterial"/>.
		/// </summary>
		public BatchInfo CustomAreaMaterial
		{
			get { return this.customAreaMaterial; }
			set { this.customAreaMaterial = value; }
		}
		/// <summary>
		/// [GET / SET] A custom, local <see cref="Duality.Drawing.BatchInfo"/> overriding the <see cref="OutlineMaterial"/>.
		/// </summary>
		public BatchInfo CustomOutlineMaterial
		{
			get { return this.customOutlineMaterial; }
			set { this.customOutlineMaterial = value; }
		}
		/// <summary>
		/// [GET / SET] A color by which the rendered debug output is tinted.
		/// </summary>
		public ColorRgba ColorTint
		{
			get { return this.colorTint; }
			set { this.colorTint = value; }
		}
		/// <summary>
		/// [GET / SET] A depth / Z offset that affects the order in which objects are drawn. If you want to assure an object is drawn after another one,
		/// just assign a higher Offset value to the background object.
		/// </summary>
		public float DepthOffset
		{
			get { return this.offset; }
			set { this.offset = value; }
		}
		/// <summary>
		/// [GET / SET] Specifies the width of the RigidBody outline when rendering. 
		/// No outline will be rendered, if this value is smaller than or equal zero.
		/// </summary>
		[EditorHintRange(0.0f, 100.0f)]
		public float OutlineWidth
		{
			get { return this.outlineWidth; }
			set { this.outlineWidth = value; }
		}
		/// <summary>
		/// [GET / SET] Whether or not hollow shapes like the <see cref="LoopShapeInfo"/> will be filled as if they were in fact solid.
		/// </summary>
		public bool FillHollowShapes
		{
			get { return this.fillHollowShapes; }
			set { this.fillHollowShapes = value; }
		}
		/// <summary>
		/// [GET / SET] Specifies, whether or not texture wrapping will be active when rendering the RigidBody area and outline.
		/// </summary>
		public bool WrapTexture
		{
			get { return this.wrapTexture; }
			set { this.wrapTexture = value; }
		}


		public override void Draw(IDrawDevice device)
		{
			Transform tranform = this.GameObj.Transform;
			RigidBody body = this.GameObj.GetComponent<RigidBody>();

			this.canvas.Begin(device);

			// Draw Shape Areas
			this.canvas.State.DepthOffset = this.offset;
			if (this.customAreaMaterial != null)
				this.canvas.State.SetMaterial(this.customAreaMaterial);
			else
				this.canvas.State.SetMaterial(this.areaMaterial);
			foreach (ShapeInfo shape in body.Shapes)
			{
				if (!shape.IsValid)
					this.canvas.State.ColorTint = this.colorTint * ColorRgba.Red;
				else
					this.canvas.State.ColorTint = this.colorTint;
				this.DrawShapeArea(canvas, tranform, shape);
			}

			// Draw Shape Outlines
			if (this.outlineWidth > 0.0f)
			{
				this.canvas.State.DepthOffset = this.offset - 0.01f;
				if (this.customOutlineMaterial != null)
					this.canvas.State.SetMaterial(this.customOutlineMaterial);
				else
					this.canvas.State.SetMaterial(this.outlineMaterial);
				foreach (ShapeInfo shape in body.Shapes)
				{
					if (!shape.IsValid)
						this.canvas.State.ColorTint = this.colorTint * ColorRgba.Red;
					else
						this.canvas.State.ColorTint = this.colorTint;
					this.DrawShapeOutline(canvas, tranform, shape);
				}
			}

			this.canvas.End();
		}

		private void DrawShapeArea(Canvas canvas, Transform transform, ShapeInfo shape)
		{
			canvas.PushState();
			if (shape is CircleShapeInfo)
			{
				this.DrawShapeArea(canvas, transform, shape as CircleShapeInfo);
			}
			else if (shape is PolyShapeInfo)
			{
				PolyShapeInfo polyShape = shape as PolyShapeInfo;
				Rect bounds = polyShape.Vertices.BoundingBox();
				Rect texRect = new Rect(1.0f, 1.0f);
				if (this.wrapTexture)
				{
					texRect.W = bounds.W / canvas.State.TextureBaseSize.X;
					texRect.H = bounds.H / canvas.State.TextureBaseSize.Y;
				}
				if (polyShape.ConvexPolygons != null)
				{
					foreach (Vector2[] convexPolygon in polyShape.ConvexPolygons)
					{
						Rect localBounds = convexPolygon.BoundingBox();
						Rect localTexRect = new Rect(
							texRect.X + texRect.W * (localBounds.X - bounds.X) / bounds.W,
							texRect.Y + texRect.H * (localBounds.Y - bounds.Y) / bounds.H,
							texRect.W * localBounds.W / bounds.W,
							texRect.H * localBounds.H / bounds.H);
						this.DrawShapeArea(canvas, transform, convexPolygon, localTexRect);
					}
				}
			}
			else if (shape is VertexBasedShapeInfo)
			{
				VertexBasedShapeInfo vertexShape = shape as VertexBasedShapeInfo;
				bool isSolid = (vertexShape.ShapeTraits & VertexShapeTrait.IsSolid) != VertexShapeTrait.None;
				if (isSolid || this.fillHollowShapes)
				{
					this.DrawShapeArea(canvas, transform, vertexShape.Vertices);
				}
			}
			canvas.PopState();
		}
		private void DrawShapeArea(Canvas canvas, Transform transform, CircleShapeInfo shape)
		{
			Vector3 pos = transform.Pos;
			float angle = transform.Angle;
			float scale = transform.Scale;

			if (this.wrapTexture)
			{
				canvas.State.TextureCoordinateRect = new Rect(
					shape.Radius * 2.0f / canvas.State.TextureBaseSize.X,
					shape.Radius * 2.0f / canvas.State.TextureBaseSize.Y);
			}
			canvas.State.TransformScale = new Vector2(scale, scale);
			canvas.State.TransformAngle = angle;
			canvas.State.TransformHandle = -shape.Position;
			canvas.FillCircle(
				pos.X, 
				pos.Y, 
				pos.Z, 
				shape.Radius);
		}
		private void DrawShapeArea(Canvas canvas, Transform transform, Vector2[] shapeVertices)
		{
			if (shapeVertices.Length < 3) return;

			Rect bounds = shapeVertices.BoundingBox();
			Rect texRect = new Rect(1.0f, 1.0f);
			if (this.wrapTexture)
			{
				texRect.W = bounds.W / canvas.State.TextureBaseSize.X;
				texRect.H = bounds.H / canvas.State.TextureBaseSize.Y;
			}
			this.DrawShapeArea(canvas, transform, shapeVertices, texRect);
		}
		private void DrawShapeArea(Canvas canvas, Transform transform, Vector2[] shapeVertices, Rect texRect)
		{
			if (shapeVertices.Length < 3) return;

			Vector3 pos = transform.Pos;
			float angle = transform.Angle;
			float scale = transform.Scale;

			canvas.State.TextureCoordinateRect = texRect;
			canvas.State.TransformAngle = angle;
			canvas.State.TransformScale = new Vector2(scale, scale);
			canvas.FillPolygon(shapeVertices, pos.X, pos.Y, pos.Z);
		}

		private void DrawShapeOutline(Canvas canvas, Transform transform, ShapeInfo shape)
		{
			canvas.PushState();
			if (shape is CircleShapeInfo)
			{
				this.DrawShapeOutline(canvas, transform, shape as CircleShapeInfo);
			}
			else if (shape is VertexBasedShapeInfo)
			{
				VertexBasedShapeInfo vertexShape = shape as VertexBasedShapeInfo;
				bool isLoop = (vertexShape.ShapeTraits & VertexShapeTrait.IsLoop) != VertexShapeTrait.None;
				this.DrawShapeOutline(canvas, transform, vertexShape.Vertices, isLoop);
			}
			canvas.PopState();
		}
		private void DrawShapeOutline(Canvas canvas, Transform transform, CircleShapeInfo shape)
		{
			Vector3 pos = transform.Pos;
			float angle = transform.Angle;
			float scale = transform.Scale;

			if (this.wrapTexture)
			{
				canvas.State.TextureCoordinateRect = new Rect(
					shape.Radius * 2.0f / canvas.State.TextureBaseSize.X,
					shape.Radius * 2.0f / canvas.State.TextureBaseSize.Y);
			}
			canvas.State.TransformScale = new Vector2(scale, scale);
			canvas.State.TransformAngle = angle;
			canvas.State.TransformHandle = -shape.Position;
			canvas.FillCircleSegment(
				pos.X, 
				pos.Y, 
				pos.Z, 
				shape.Radius,
				0.0f,
				MathF.RadAngle360,
				this.outlineWidth);
		}
		private void DrawShapeOutline(Canvas canvas, Transform transform, Vector2[] shapeVertices, bool closedLoop)
		{
			Vector3 pos = transform.Pos;
			float angle = transform.Angle;
			float scale = transform.Scale;

			if (this.wrapTexture)
			{
				Rect pointBoundingRect = shapeVertices.BoundingBox();
				canvas.State.TextureCoordinateRect = new Rect(
					pointBoundingRect.W / canvas.State.TextureBaseSize.X,
					pointBoundingRect.H / canvas.State.TextureBaseSize.Y);
			}
			canvas.State.TransformAngle = angle;
			canvas.State.TransformScale = new Vector2(scale, scale);

			float inOutFactor = -1.0f + 0.3f / MathF.Max(1.0f, this.outlineWidth);
			if (closedLoop)
				canvas.FillPolygonOutline(shapeVertices, this.outlineWidth, inOutFactor, pos.X, pos.Y, pos.Z);
			else
				canvas.FillThickLineStrip(shapeVertices, this.outlineWidth, inOutFactor, pos.X, pos.Y, pos.Z);
		}
	}
}
