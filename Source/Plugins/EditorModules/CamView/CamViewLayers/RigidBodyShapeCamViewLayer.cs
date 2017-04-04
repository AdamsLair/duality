using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using Duality;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components.Physics;

using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Plugins.CamView.CamViewStates;

namespace Duality.Editor.Plugins.CamView.CamViewLayers
{
	public class RigidBodyShapeCamViewLayer : CamViewLayer
	{
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
					return ColorRgba.Lerp(new ColorRgba(255, 0, 255), ColorRgba.VeryLightGrey, 0.5f);
				else
					return ColorRgba.Lerp(new ColorRgba(255, 0, 255), ColorRgba.VeryDarkGrey, 0.5f);
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

			canvas.State.TextFont = Font.GenericMonospace10;
			canvas.State.TextInvariantScale = true;
			canvas.State.DepthOffset = -0.5f;
			Font textFont = canvas.State.TextFont.Res;

			// Draw Shape layer
			foreach (RigidBody body in visibleColliders)
			{
				if (!body.Shapes.Any()) continue;
				float colliderAlpha = body == selectedBody ? 1.0f : (selectedBody != null ? 0.25f : 0.5f);
				float maxDensity = body.Shapes.Max(s => s.Density);
				float minDensity = body.Shapes.Min(s => s.Density);
				float avgDensity = (maxDensity + minDensity) * 0.5f;
				Vector3 objPos = body.GameObj.Transform.Pos;
				float objAngle = body.GameObj.Transform.Angle;
				float objScale = body.GameObj.Transform.Scale;
				int index = 0;
				foreach (ShapeInfo shape in body.Shapes)
				{
					CircleShapeInfo circle = shape as CircleShapeInfo;
					PolyShapeInfo poly = shape as PolyShapeInfo;
					ChainShapeInfo chain = shape as ChainShapeInfo;
					LoopShapeInfo loop = shape as LoopShapeInfo;

					ObjectEditorCamViewState editorState = this.View.ActiveState as ObjectEditorCamViewState;
					float shapeAlpha = colliderAlpha * (selectedBody == null || editorState == null || editorState.SelectedObjects.Any(sel => sel.ActualObject == shape) ? 1.0f : 0.5f);
					float densityRelative = MathF.Abs(maxDensity - minDensity) < 0.01f ? 1.0f : shape.Density / avgDensity;
					ColorRgba clr = shape.IsSensor ? this.ShapeSensorColor : this.ShapeColor;
					ColorRgba fontClr = this.FgColor;
					Vector2 center = Vector2.Zero;

					if (!body.IsAwake) clr = clr.ToHsva().WithSaturation(0.0f).ToRgba();
					if (!shape.IsValid) clr = this.ShapeErrorColor;

					bool fillShape = (poly != null || circle != null);
					Vector2[] shapeVertices = null;
					if      (poly  != null) shapeVertices = poly .Vertices;
					else if (loop  != null) shapeVertices = loop .Vertices;
					else if (chain != null) shapeVertices = chain.Vertices;

					if (circle != null)
					{
						Vector2 circlePos = circle.Position * objScale;
						MathF.TransformCoord(ref circlePos.X, ref circlePos.Y, objAngle);

						if (fillShape)
						{
							canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr.WithAlpha((0.25f + densityRelative * 0.25f) * shapeAlpha)));
							canvas.FillCircle(
								objPos.X + circlePos.X,
								objPos.Y + circlePos.Y,
								objPos.Z, 
								circle.Radius * objScale);
						}
						canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr.WithAlpha(shapeAlpha)));
						canvas.DrawCircle(
							objPos.X + circlePos.X,
							objPos.Y + circlePos.Y,
							objPos.Z, 
							circle.Radius * objScale);

						center = circlePos;
					}
					else if (shapeVertices != null)
					{
						ColorRgba vertexFillColor = canvas.State.ColorTint * clr.WithAlpha((0.25f + densityRelative * 0.25f) * shapeAlpha);
						ColorRgba vertexOutlineColor = canvas.State.ColorTint * clr;

						// Prepare vertices to submit. We can't use higher-level canvas functionality
						// here, because we want direct control over the vertex mode.
						float viewSpaceScale = objScale;
						Vector3 viewSpacePos = objPos;
						canvas.DrawDevice.PreprocessCoords(ref viewSpacePos, ref viewSpaceScale);
						VertexC1P3T2[] drawVertices = new VertexC1P3T2[shapeVertices.Length];
						for (int i = 0; i < drawVertices.Length; i++)
						{
							drawVertices[i].Pos.X = shapeVertices[i].X;
							drawVertices[i].Pos.Y = shapeVertices[i].Y;
							drawVertices[i].Pos.Z = 0.0f;
							MathF.TransformCoord(ref drawVertices[i].Pos.X, ref drawVertices[i].Pos.Y, objAngle, viewSpaceScale);

							drawVertices[i].Pos.X += viewSpacePos.X;
							drawVertices[i].Pos.Y += viewSpacePos.Y;
							drawVertices[i].Pos.Z += viewSpacePos.Z;
							drawVertices[i].Color = vertexOutlineColor;
						}

						// Calculate the center coordinate 
						for (int i = 0; i < drawVertices.Length; i++)
							center += shapeVertices[i];
						center /= shapeVertices.Length;
						MathF.TransformCoord(ref center.X, ref center.Y, objAngle, objScale);

						// Make sure to render using an alpha material
						canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, ColorRgba.White));

						// Fill the shape
						if (fillShape)
						{
							VertexC1P3T2[] fillVertices = drawVertices.Clone() as VertexC1P3T2[];
							for (int i = 0; i < fillVertices.Length; i++)
								fillVertices[i].Color = vertexFillColor;
							canvas.DrawVertices(fillVertices, VertexMode.TriangleFan);
						}

						// Draw the outline
						canvas.DrawVertices(drawVertices, shape is ChainShapeInfo ? VertexMode.LineStrip : VertexMode.LineLoop);
					}
					
					// Draw shape index
					if (body == selectedBody)
					{
						string indexText = index.ToString();
						Vector2 textSize = textFont.MeasureText(indexText);
						canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, fontClr.WithAlpha((shapeAlpha + 1.0f) * 0.5f)));
						canvas.DrawText(indexText, 
							objPos.X + center.X, 
							objPos.Y + center.Y,
							objPos.Z);
					}

					index++;
				}
				
				// Draw center of mass
				if (body.BodyType == BodyType.Dynamic)
				{
					Vector2 localMassCenter = body.LocalMassCenter;
					MathF.TransformCoord(ref localMassCenter.X, ref localMassCenter.Y, objAngle, objScale);
					canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, this.MassCenterColor.WithAlpha(colliderAlpha)));
					canvas.DrawLine(
						objPos.X + localMassCenter.X - 5.0f, 
						objPos.Y + localMassCenter.Y, 
						objPos.Z,
						objPos.X + localMassCenter.X + 5.0f, 
						objPos.Y + localMassCenter.Y, 
						objPos.Z);
					canvas.DrawLine(
						objPos.X + localMassCenter.X, 
						objPos.Y + localMassCenter.Y - 5.0f, 
						objPos.Z,
						objPos.X + localMassCenter.X, 
						objPos.Y + localMassCenter.Y + 5.0f, 
						objPos.Z);
				}
			}
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
