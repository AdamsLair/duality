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
			canvas.State.ZOffset = -1;
			Font textFont = canvas.State.TextFont.Res;

			// Draw Shape layer
			foreach (RigidBody c in visibleColliders)
			{
				if (!c.Shapes.Any()) continue;
				float colliderAlpha = c == selectedBody ? 1.0f : (selectedBody != null ? 0.25f : 0.5f);
				float maxDensity = c.Shapes.Max(s => s.Density);
				float minDensity = c.Shapes.Min(s => s.Density);
				float avgDensity = (maxDensity + minDensity) * 0.5f;
				Vector3 objPos = c.GameObj.Transform.Pos;
				float objScale = c.GameObj.Transform.Scale;
				int index = 0;
				foreach (ShapeInfo s in c.Shapes)
				{
					CircleShapeInfo circle = s as CircleShapeInfo;
					PolyShapeInfo poly = s as PolyShapeInfo;
					ChainShapeInfo chain = s as ChainShapeInfo;
					LoopShapeInfo loop = s as LoopShapeInfo;

					ObjectEditorCamViewState editorState = this.View.ActiveState as ObjectEditorCamViewState;
					float shapeAlpha = colliderAlpha * (selectedBody == null || editorState == null || editorState.SelectedObjects.Any(sel => sel.ActualObject == s) ? 1.0f : 0.5f);
					float densityRelative = MathF.Abs(maxDensity - minDensity) < 0.01f ? 1.0f : s.Density / avgDensity;
					ColorRgba clr = s.IsSensor ? this.ShapeSensorColor : this.ShapeColor;
					ColorRgba fontClr = this.FgColor;
					Vector2 center = Vector2.Zero;

					if (!c.IsAwake) clr = clr.ToHsva().WithSaturation(0.0f).ToRgba();
					if (!s.IsValid) clr = this.ShapeErrorColor;

					bool fillShape = (poly != null || circle != null);
					Vector2[] shapeVertices = null;
					if      (poly  != null) shapeVertices = poly .Vertices;
					else if (loop  != null) shapeVertices = loop .Vertices;
					else if (chain != null) shapeVertices = chain.Vertices;

					if (circle != null)
					{
						Vector2 circlePos = circle.Position * objScale;
						MathF.TransformCoord(ref circlePos.X, ref circlePos.Y, c.GameObj.Transform.Angle);

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
						Vector2[] drawVertices = shapeVertices.ToArray();
						for (int i = 0; i < drawVertices.Length; i++)
						{
							center += drawVertices[i];
							Vector2.Multiply(ref drawVertices[i], objScale, out drawVertices[i]);
							MathF.TransformCoord(ref drawVertices[i].X, ref drawVertices[i].Y, c.GameObj.Transform.Angle);
						}
						center /= drawVertices.Length;
						Vector2.Multiply(ref center, objScale, out center);
						MathF.TransformCoord(ref center.X, ref center.Y, c.GameObj.Transform.Angle);

						if (fillShape)
						{
							canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr.WithAlpha((0.25f + densityRelative * 0.25f) * shapeAlpha)));
							canvas.FillPolygon(drawVertices, objPos.X, objPos.Y, objPos.Z);
						}
						canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr.WithAlpha(shapeAlpha)));
						canvas.DrawPolygon(drawVertices, objPos.X, objPos.Y, objPos.Z);
					}
					
					// Draw shape index
					if (c == selectedBody)
					{
						string indexText = index.ToString();
						Vector2 textSize = textFont.MeasureText(indexText);
						canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, fontClr.WithAlpha((shapeAlpha + 1.0f) * 0.5f)));
						canvas.State.TransformHandle = textSize * 0.5f;
						canvas.DrawText(indexText, 
							objPos.X + center.X, 
							objPos.Y + center.Y,
							objPos.Z);
						canvas.State.TransformHandle = Vector2.Zero;
					}

					index++;
				}
				
				// Draw center of mass
				if (c.BodyType == BodyType.Dynamic)
				{
					Vector2 localMassCenter = c.LocalMassCenter;
					MathF.TransformCoord(ref localMassCenter.X, ref localMassCenter.Y, c.GameObj.Transform.Angle, c.GameObj.Transform.Scale);
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
