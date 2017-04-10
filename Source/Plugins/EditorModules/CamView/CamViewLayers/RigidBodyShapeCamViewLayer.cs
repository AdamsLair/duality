using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using Duality;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Physics;

using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Plugins.CamView.CamViewStates;

namespace Duality.Editor.Plugins.CamView.CamViewLayers
{
	public class RigidBodyShapeCamViewLayer : CamViewLayer
	{
		private float shapeOutlineWidth = 2.0f;
		private float depthOffset = -0.5f;

		public override string LayerName
		{
			get { return Properties.CamViewRes.CamViewLayer_RigidBodyShape_Name; }
		}
		public override string LayerDesc
		{
			get { return Properties.CamViewRes.CamViewLayer_RigidBodyShape_Desc; }
		}
		public ColorRgba MassCenterColor
		{
			get
			{
				float fgLum = this.FgColor.GetLuminance();
				if (fgLum > 0.5f)
					return new ColorRgba(255, 128, 255);
				else
					return new ColorRgba(192, 0, 192);
			}
		}
		public ColorRgba ObjectCenterColor
		{
			get
			{
				float fgLum = this.FgColor.GetLuminance();
				if (fgLum > 0.5f)
					return new ColorRgba(255, 255, 128);
				else
					return new ColorRgba(192, 192, 0);
			}
		}
		public ColorRgba ShapeColor
		{
			get
			{
				float fgLum = this.FgColor.GetLuminance();
				if (fgLum > 0.5f)
					return ColorRgba.Lerp(ColorRgba.Blue, ColorRgba.VeryLightGrey, 0.5f);
				else
					return ColorRgba.Lerp(ColorRgba.Blue, ColorRgba.VeryDarkGrey, 0.5f);
			}
		}
		public ColorRgba ShapeSensorColor
		{
			get
			{
				float fgLum = this.FgColor.GetLuminance();
				if (fgLum > 0.5f)
					return ColorRgba.Lerp(new ColorRgba(255, 128, 0), ColorRgba.VeryLightGrey, 0.5f);
				else
					return ColorRgba.Lerp(new ColorRgba(255, 128, 0), ColorRgba.VeryDarkGrey, 0.5f);
			}
		}
		public ColorRgba ShapeErrorColor
		{
			get
			{
				float fgLum = this.FgColor.GetLuminance();
				if (fgLum > 0.5f)
					return ColorRgba.Lerp(ColorRgba.Red, ColorRgba.VeryLightGrey, 0.5f);
				else
					return ColorRgba.Lerp(ColorRgba.Red, ColorRgba.VeryDarkGrey, 0.5f);
			}
		}

		protected internal override void OnCollectWorldOverlayDrawcalls(Canvas canvas)
		{
			base.OnCollectWorldOverlayDrawcalls(canvas);
			List<RigidBody> visibleColliders = this.QueryVisibleColliders().ToList();

			RigidBody selectedBody = this.QuerySelectedCollider();

			canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, ColorRgba.White));
			canvas.State.TextFont = Font.GenericMonospace10;
			canvas.State.TextInvariantScale = true;
			canvas.State.ZOffset = this.depthOffset;
			Font textFont = canvas.State.TextFont.Res;

			// Retrieve selected shapes
			ObjectEditorCamViewState editorState = this.View.ActiveState as ObjectEditorCamViewState;
			object[] editorSelectedObjects = editorState != null ? editorState.SelectedObjects.Select(item => item.ActualObject).ToArray() : new object[0];
			
			bool isAnyBodySelected = (selectedBody != null);
			bool isAnyShapeSelected = isAnyBodySelected && editorSelectedObjects.OfType<ShapeInfo>().Any();

			// Draw Shape layer
			foreach (RigidBody body in visibleColliders)
			{
				if (!body.Shapes.Any()) continue;

				Vector3 objPos = body.GameObj.Transform.Pos;
				float objAngle = body.GameObj.Transform.Angle;
				float objScale = body.GameObj.Transform.Scale;

				bool isBodySelected = (body == selectedBody);

				float bodyAlpha = isBodySelected ? 1.0f : (isAnyShapeSelected ? 0.5f : 1.0f);
				float maxDensity = body.Shapes.Max(s => s.Density);
				float minDensity = body.Shapes.Min(s => s.Density);
				float avgDensity = (maxDensity + minDensity) * 0.5f;

				int shapeIndex = 0;
				foreach (ShapeInfo shape in body.Shapes)
				{
					CircleShapeInfo circle = shape as CircleShapeInfo;
					PolyShapeInfo poly = shape as PolyShapeInfo;
					ChainShapeInfo chain = shape as ChainShapeInfo;
					LoopShapeInfo loop = shape as LoopShapeInfo;

					bool isShapeSelected = isBodySelected && editorSelectedObjects.Contains(shape);

					float shapeAlpha = bodyAlpha * (isShapeSelected ? 1.0f : (isAnyShapeSelected ? 0.75f : 1.0f));
					float densityRelative = MathF.Abs(maxDensity - minDensity) < 0.01f ? 1.0f : shape.Density / avgDensity;
					ColorRgba shapeColor = shape.IsSensor ? this.ShapeSensorColor : this.ShapeColor;
					ColorRgba fontColor = this.FgColor;

					if (!body.IsAwake) shapeColor = shapeColor.ToHsva().WithSaturation(0.0f).ToRgba();
					if (!shape.IsValid) shapeColor = this.ShapeErrorColor;

					// Draw the shape itself
					ColorRgba fillColor = shapeColor.WithAlpha((0.25f + densityRelative * 0.25f) * shapeAlpha);
					ColorRgba outlineColor = ColorRgba.Lerp(shapeColor, fontColor, isShapeSelected ? 0.75f : 0.25f).WithAlpha(shapeAlpha);
					this.DrawShape(canvas, body.GameObj.Transform, shape, fillColor, outlineColor);

					// Calculate the center coordinate 
					Vector2 shapeCenter = Vector2.Zero;
					if (circle != null)
					{
						shapeCenter = circle.Position * objScale;
					}
					else
					{
						Vector2[] shapeVertices = null;
						if      (poly  != null) shapeVertices = poly .Vertices;
						else if (loop  != null) shapeVertices = loop .Vertices;
						else if (chain != null) shapeVertices = chain.Vertices;

						for (int i = 0; i < shapeVertices.Length; i++)
							shapeCenter += shapeVertices[i];

						shapeCenter /= shapeVertices.Length;
						MathF.TransformCoord(ref shapeCenter.X, ref shapeCenter.Y, objAngle, objScale);
					}
					
					// Draw shape index
					if (body == selectedBody)
					{
						string indexText = shapeIndex.ToString();
						Vector2 textSize = textFont.MeasureText(indexText);
						canvas.State.ColorTint = fontColor.WithAlpha((shapeAlpha + 1.0f) * 0.5f);
						canvas.DrawText(indexText, 
							objPos.X + shapeCenter.X, 
							objPos.Y + shapeCenter.Y,
							0.0f);
					}

					shapeIndex++;
				}
				
				// Draw center of mass
				if (body.BodyType == BodyType.Dynamic)
				{
					Vector2 localMassCenter = body.LocalMassCenter;
					MathF.TransformCoord(ref localMassCenter.X, ref localMassCenter.Y, objAngle, objScale);

					float size = this.GetScreenConstantScale(canvas, 6.0f);

					canvas.State.ColorTint = this.MassCenterColor.WithAlpha(bodyAlpha);
					canvas.DrawLine(
						objPos.X + localMassCenter.X - size, 
						objPos.Y + localMassCenter.Y, 
						0.0f,
						objPos.X + localMassCenter.X + size, 
						objPos.Y + localMassCenter.Y, 
						0.0f);
					canvas.DrawLine(
						objPos.X + localMassCenter.X, 
						objPos.Y + localMassCenter.Y - size, 
						0.0f,
						objPos.X + localMassCenter.X, 
						objPos.Y + localMassCenter.Y + size, 
						0.0f);
				}
				
				// Draw transform center
				if (body.BodyType == BodyType.Dynamic)
				{
					float size = this.GetScreenConstantScale(canvas, 4.0f);
					canvas.State.ColorTint = this.ObjectCenterColor.WithAlpha(bodyAlpha);
					canvas.FillCircle(objPos.X, objPos.Y, 0.0f, size);
				}
			}
		}
		
		private void DrawShape(Canvas canvas, Transform transform, ShapeInfo shape, ColorRgba fillColor, ColorRgba outlineColor)
		{
			if      (shape is CircleShapeInfo) this.DrawShape(canvas, transform, shape as CircleShapeInfo, fillColor, outlineColor);
			else if (shape is LoopShapeInfo)   this.DrawShape(canvas, transform, shape as LoopShapeInfo  , fillColor, outlineColor);
			else if (shape is ChainShapeInfo)  this.DrawShape(canvas, transform, shape as ChainShapeInfo , fillColor, outlineColor);
			else if (shape is PolyShapeInfo)   this.DrawShape(canvas, transform, shape as PolyShapeInfo  , fillColor, outlineColor);
		}
		private void DrawShape(Canvas canvas, Transform transform, LoopShapeInfo shape, ColorRgba fillColor, ColorRgba outlineColor)
		{
			this.DrawPolygonOutline(canvas, transform, shape.Vertices, outlineColor, true);
		}
		private void DrawShape(Canvas canvas, Transform transform, ChainShapeInfo shape, ColorRgba fillColor, ColorRgba outlineColor)
		{
			this.DrawPolygonOutline(canvas, transform, shape.Vertices, outlineColor, false);
		}
		private void DrawShape(Canvas canvas, Transform transform, PolyShapeInfo shape, ColorRgba fillColor, ColorRgba outlineColor)
		{
			// Fill each convex polygon individually
			foreach (Vector2[] polygon in shape.ConvexPolygons)
			{
				this.FillPolygon(canvas, transform, polygon, fillColor);
			}

			// Draw the polygon outline
			this.DrawPolygonOutline(canvas, transform, shape.Vertices, outlineColor, true);
		}
		private void DrawShape(Canvas canvas, Transform transform, CircleShapeInfo shape, ColorRgba fillColor, ColorRgba outlineColor)
		{
			Vector3 objPos = transform.Pos;
			float objAngle = transform.Angle;
			float objScale = transform.Scale;

			Vector2 circlePos = shape.Position * objScale;
			MathF.TransformCoord(ref circlePos.X, ref circlePos.Y, objAngle);

			if (fillColor.A > 0)
			{
				canvas.State.ColorTint = fillColor;
				canvas.FillCircle(
					objPos.X + circlePos.X,
					objPos.Y + circlePos.Y,
					0.0f, 
					shape.Radius * objScale);
			}

			float outlineWidth = this.GetScreenConstantScale(canvas, this.shapeOutlineWidth);
			canvas.State.ColorTint = outlineColor;
			canvas.FillCircleSegment(
				objPos.X + circlePos.X,
				objPos.Y + circlePos.Y,
				0.0f, 
				shape.Radius * objScale,
				0.0f,
				MathF.RadAngle360,
				outlineWidth);
		}

		private void FillPolygon(Canvas canvas, Transform transform, Vector2[] polygon, ColorRgba fillColor)
		{
			Vector3 objPos = transform.Pos;
			float objAngle = transform.Angle;
			float objScale = transform.Scale;

			canvas.State.ColorTint = fillColor;
			canvas.State.TransformAngle = objAngle;
			canvas.State.TransformScale = new Vector2(objScale, objScale);

			canvas.FillPolygon(polygon, objPos.X, objPos.Y, objPos.Z);

			canvas.State.TransformAngle = 0.0f;
			canvas.State.TransformScale = Vector2.One;
		}
		private void DrawPolygonOutline(Canvas canvas, Transform transform, Vector2[] polygon, ColorRgba outlineColor, bool closedLoop)
		{
			Vector3 objPos = transform.Pos;
			float objAngle = transform.Angle;
			float objScale = transform.Scale;

			canvas.State.TransformAngle = objAngle;
			canvas.State.TransformScale = new Vector2(objScale, objScale);
			canvas.State.ColorTint = outlineColor;

			float outlineWidth = this.GetScreenConstantScale(canvas, this.shapeOutlineWidth);
			if (closedLoop)
				canvas.FillPolygonOutline(polygon, outlineWidth, objPos.X, objPos.Y, 0.0f);
			else
				canvas.FillThickLineStrip(polygon, outlineWidth, objPos.X, objPos.Y, 0.0f);

			canvas.State.TransformAngle = 0.0f;
			canvas.State.TransformScale = Vector2.One;
		}

		private float GetScreenConstantScale(Canvas canvas, float baseScale)
		{
			return baseScale / MathF.Max(0.0001f, canvas.DrawDevice.GetScaleAtZ(0.0f));
		}

		private IEnumerable<RigidBody> QueryVisibleColliders()
		{
			var allColliders = Scene.Current.FindComponents<RigidBody>();
			return allColliders.Where(r => 
				r.Active && 
				!DesignTimeObjectData.Get(r.GameObj).IsHidden && 
				this.IsCoordInView(r.GameObj.Transform.Pos, r.BoundRadius));
		}
		private RigidBody QuerySelectedCollider()
		{
			return 
				DualityEditorApp.Selection.Components.OfType<RigidBody>().FirstOrDefault() ?? 
				DualityEditorApp.Selection.GameObjects.GetComponents<RigidBody>().FirstOrDefault();
		}
	}
}
