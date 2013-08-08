using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using Duality;
using Duality.ColorFormat;
using Duality.Resources;
using Duality.Components.Physics;

using DualityEditor;
using DualityEditor.Forms;
using DualityEditor.CorePluginInterface;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace EditorBase.CamViewLayers
{
	public class RigidBodyJointCamViewLayer : CamViewLayer
	{
		public override string LayerName
		{
			get { return PluginRes.EditorBaseRes.CamViewLayer_RigidBodyJoint_Name; }
		}
		public override string LayerDesc
		{
			get { return PluginRes.EditorBaseRes.CamViewLayer_RigidBodyJoint_Desc; }
		}
		public ColorRgba MotorColor
		{
			get
			{
				float fgLum = this.FgColor.GetLuminance();
				if (fgLum > 0.5f)
					return new ColorRgba(144, 192, 240);
				else
					return new ColorRgba(16, 32, 96);
			}
		}
		public ColorRgba JointColor
		{
			get
			{
				float fgLum = this.FgColor.GetLuminance();
				if (fgLum > 0.5f)
					return new ColorRgba(176, 240, 112);
				else
					return new ColorRgba(16, 96, 0);
			}
		}
		public ColorRgba JointErrorColor
		{
			get
			{
				float fgLum = this.FgColor.GetLuminance();
				if (fgLum > 0.5f)
					return new ColorRgba(240, 144, 112);
				else
					return new ColorRgba(96, 8, 0);
			}
		}

		protected internal override void OnCollectDrawcalls(Canvas canvas)
		{
			base.OnCollectDrawcalls(canvas);
			canvas.CurrentState.TextInvariantScale = true;
			canvas.CurrentState.ZOffset = -1;

			RigidBody selectedBody = this.QuerySelectedCollider();

			List<RigidBody> visibleColliders = this.QueryVisibleColliders().ToList();
			List<JointInfo> visibleJoints = new List<JointInfo>();
			foreach (RigidBody c in visibleColliders)
			{
				if (c.Joints == null) continue;
				visibleJoints.AddRange(c.Joints.Where(j => !visibleJoints.Contains(j)));
			}
			foreach (JointInfo j in visibleJoints)
			{
				float jointAlpha = selectedBody != null && (j.BodyA == selectedBody || j.BodyB == selectedBody) ? 1.0f : 0.5f;
				if (!j.Enabled) jointAlpha *= 0.25f;
				canvas.CurrentState.ColorTint = canvas.CurrentState.ColorTint.WithAlpha(jointAlpha);

				if (j.BodyA == null) continue;
				if (j.DualJoint && j.BodyB == null) continue;
				this.DrawJoint(canvas, j);
			}
		}

		private void DrawJoint(Canvas canvas, JointInfo joint)
		{
			if (joint.BodyA == null) return;
			if (joint.DualJoint && joint.BodyB == null) return;

			if (joint is FixedAngleJointInfo)			this.DrawJoint(canvas, joint as FixedAngleJointInfo);
			else if (joint is FixedDistanceJointInfo)	this.DrawJoint(canvas, joint as FixedDistanceJointInfo);
			else if (joint is FixedMouseJointInfo)		this.DrawJoint(canvas, joint as FixedMouseJointInfo);
			else if (joint is FixedFrictionJointInfo)	this.DrawJoint(canvas, joint as FixedFrictionJointInfo);
			else if (joint is FixedRevoluteJointInfo)	this.DrawJoint(canvas, joint as FixedRevoluteJointInfo);
			else if (joint is FixedPrismaticJointInfo)	this.DrawJoint(canvas, joint as FixedPrismaticJointInfo);
			else if (joint is AngleJointInfo)			this.DrawJoint(canvas, joint as AngleJointInfo);
			else if (joint is DistanceJointInfo)		this.DrawJoint(canvas, joint as DistanceJointInfo);
			else if (joint is FrictionJointInfo)		this.DrawJoint(canvas, joint as FrictionJointInfo);
			else if (joint is RevoluteJointInfo)		this.DrawJoint(canvas, joint as RevoluteJointInfo);
			else if (joint is PrismaticJointInfo)		this.DrawJoint(canvas, joint as PrismaticJointInfo);
			else if (joint is WeldJointInfo)			this.DrawJoint(canvas, joint as WeldJointInfo);
			else if (joint is RopeJointInfo)			this.DrawJoint(canvas, joint as RopeJointInfo);
			else if (joint is SliderJointInfo)			this.DrawJoint(canvas, joint as SliderJointInfo);
			else if (joint is LineJointInfo)			this.DrawJoint(canvas, joint as LineJointInfo);
			else if (joint is PulleyJointInfo)			this.DrawJoint(canvas, joint as PulleyJointInfo);
			else if (joint is GearJointInfo)			this.DrawJoint(canvas, joint as GearJointInfo);
		}
		private void DrawJoint(Canvas canvas, FixedAngleJointInfo joint)
		{
			this.DrawLocalAngleConstraint(canvas, joint.BodyA, Vector2.Zero, joint.TargetAngle, joint.BodyA.GameObj.Transform.Angle, joint.BodyA.BoundRadius);
		}
		private void DrawJoint(Canvas canvas, FixedDistanceJointInfo joint)
		{
			this.DrawWorldDistConstraint(canvas, joint.BodyA, joint.LocalAnchor, joint.WorldAnchor, joint.TargetDistance, joint.TargetDistance);
			this.DrawWorldAnchor(canvas, joint.BodyA, joint.WorldAnchor);
			this.DrawLocalAnchor(canvas, joint.BodyA, joint.LocalAnchor);
		}
		private void DrawJoint(Canvas canvas, FixedMouseJointInfo joint)
		{
			this.DrawWorldPosConstraint(canvas, joint.BodyA, joint.LocalAnchor, joint.WorldAnchor);
			this.DrawWorldAnchor(canvas, joint.BodyA, joint.WorldAnchor);
			this.DrawLocalAnchor(canvas, joint.BodyA, joint.LocalAnchor);
		}
		private void DrawJoint(Canvas canvas, FixedFrictionJointInfo joint)
		{
			this.DrawLocalFrictionMarker(canvas, joint.BodyA, joint.LocalAnchor);
		}
		private void DrawJoint(Canvas canvas, FixedRevoluteJointInfo joint)
		{
			float angularCircleRad = joint.BodyA.BoundRadius * 0.25f;

			this.DrawWorldPosConstraint(canvas, joint.BodyA, joint.LocalAnchor, joint.WorldAnchor);
			if (joint.LimitEnabled)
			{
				this.DrawLocalAngleConstraint(
					canvas, joint.BodyA, joint.LocalAnchor, 
					joint.LowerLimit + joint.ReferenceAngle, joint.UpperLimit + joint.ReferenceAngle, joint.BodyA.GameObj.Transform.Angle, 
					angularCircleRad);
			}

			if (joint.MotorEnabled)
			{
				this.DrawLocalAngleMotor(canvas, joint.BodyA, Vector2.Zero, joint.MotorSpeed, joint.MaxMotorTorque, joint.BodyA.BoundRadius * 1.15f);
			}

			this.DrawWorldAnchor(canvas, joint.BodyA, joint.WorldAnchor);
			this.DrawLocalAnchor(canvas, joint.BodyA, joint.LocalAnchor);
		}
		private void DrawJoint(Canvas canvas, FixedPrismaticJointInfo joint)
		{
			float angularCircleRad = joint.BodyA.BoundRadius * 0.25f;

			if (joint.LimitEnabled)
				this.DrawWorldAxisConstraint(canvas, joint.BodyA, joint.MovementAxis, Vector2.Zero, joint.WorldAnchor, joint.LowerLimit, joint.UpperLimit);
			else
				this.DrawWorldAxisConstraint(canvas, joint.BodyA, joint.MovementAxis, Vector2.Zero, joint.WorldAnchor);

			this.DrawLocalAngleConstraint(canvas, joint.BodyA, Vector2.Zero, joint.ReferenceAngle, joint.BodyA.GameObj.Transform.Angle, joint.BodyA.BoundRadius);

			if (joint.MotorEnabled)
				this.DrawWorldAxisMotor(canvas, joint.BodyA, joint.MovementAxis, Vector2.Zero, joint.WorldAnchor, joint.MotorSpeed, joint.MaxMotorForce, joint.BodyA.BoundRadius * 1.15f);

			this.DrawWorldAnchor(canvas, joint.BodyA, joint.WorldAnchor);
		}
		private void DrawJoint(Canvas canvas, AngleJointInfo joint)
		{
			this.DrawLocalAngleConstraint(canvas, 
				joint.BodyA, 
				Vector2.Zero, 
				joint.BodyB.GameObj.Transform.Angle - joint.TargetAngle, 
				joint.BodyA.GameObj.Transform.Angle, 
				joint.BodyA.BoundRadius);
			this.DrawLocalAngleConstraint(canvas, 
				joint.BodyB, 
				Vector2.Zero, 
				joint.BodyA.GameObj.Transform.Angle + joint.TargetAngle, 
				joint.BodyB.GameObj.Transform.Angle, 
				joint.BodyB.BoundRadius);
			this.DrawLocalLooseConstraint(canvas, joint.BodyA, joint.BodyB, Vector2.Zero, Vector2.Zero);
		}
		private void DrawJoint(Canvas canvas, DistanceJointInfo joint)
		{
			this.DrawLocalDistConstraint(canvas, joint.BodyA, joint.BodyB, joint.LocalAnchorA, joint.LocalAnchorB, joint.TargetDistance, joint.TargetDistance);
			this.DrawLocalAnchor(canvas, joint.BodyB, joint.LocalAnchorB);
			this.DrawLocalAnchor(canvas, joint.BodyA, joint.LocalAnchorA);
		}
		private void DrawJoint(Canvas canvas, FrictionJointInfo joint)
		{
			this.DrawLocalFrictionMarker(canvas, joint.BodyA, joint.LocalAnchorA);
			this.DrawLocalFrictionMarker(canvas, joint.BodyB, joint.LocalAnchorB);
			this.DrawLocalLooseConstraint(canvas, joint.BodyA, joint.BodyB, joint.LocalAnchorA, joint.LocalAnchorB);
		}
		private void DrawJoint(Canvas canvas, RevoluteJointInfo joint)
		{
			float angularCircleRadA = joint.BodyA.BoundRadius * 0.25f;
			float angularCircleRadB = joint.BodyB.BoundRadius * 0.25f;

			float anchorDist = this.GetAnchorDist(joint.BodyA, joint.BodyB, joint.LocalAnchorA, joint.LocalAnchorB);
			bool displaySecondCollider = anchorDist >= angularCircleRadA + angularCircleRadB;

			this.DrawLocalPosConstraint(canvas, joint.BodyA, joint.BodyB, joint.LocalAnchorA, joint.LocalAnchorB);
			
			if (joint.LimitEnabled)
			{
				this.DrawLocalAngleConstraint(canvas, 
					joint.BodyA, 
					joint.LocalAnchorA, 
					joint.BodyB.GameObj.Transform.Angle - joint.ReferenceAngle, 
					joint.BodyA.GameObj.Transform.Angle, 
					angularCircleRadA);
				if (displaySecondCollider)
				{
					this.DrawLocalAngleConstraint(canvas, 
						joint.BodyB, 
						joint.LocalAnchorB, 
						joint.BodyA.GameObj.Transform.Angle + joint.ReferenceAngle,
						joint.BodyB.GameObj.Transform.Angle, 
						angularCircleRadB);
				}
			}

			if (joint.MotorEnabled)
			{
				this.DrawLocalAngleMotor(canvas, joint.BodyA, Vector2.Zero, joint.MotorSpeed, joint.MaxMotorTorque, joint.BodyA.BoundRadius * 1.15f);
			}

			this.DrawLocalAnchor(canvas, joint.BodyA, joint.LocalAnchorA);
			this.DrawLocalAnchor(canvas, joint.BodyB, joint.LocalAnchorB);
		}
		private void DrawJoint(Canvas canvas, PrismaticJointInfo joint)
		{
			float angularCircleRadA = joint.BodyA.BoundRadius * 0.25f;
			float angularCircleRadB = joint.BodyB.BoundRadius * 0.25f;
			bool displaySecondCollider = (joint.BodyA.GameObj.Transform.Pos - joint.BodyB.GameObj.Transform.Pos).Length >= angularCircleRadA + angularCircleRadB;

			if (joint.LimitEnabled)
			    this.DrawLocalAxisConstraint(canvas, joint.BodyA, joint.BodyB, joint.MovementAxis, joint.LocalAnchorA, joint.LocalAnchorB, joint.LowerLimit, joint.UpperLimit);
			else
			    this.DrawLocalAxisConstraint(canvas, joint.BodyA, joint.BodyB, joint.MovementAxis, joint.LocalAnchorA, joint.LocalAnchorB);

			this.DrawLocalAngleConstraint(canvas, 
				joint.BodyA, 
				joint.LocalAnchorA, 
				joint.BodyB.GameObj.Transform.Angle - joint.ReferenceAngle, 
				joint.BodyA.GameObj.Transform.Angle, 
				angularCircleRadA);
			if (displaySecondCollider)
			{
				this.DrawLocalAngleConstraint(canvas, 
					joint.BodyB, 
					joint.LocalAnchorB, 
					joint.BodyA.GameObj.Transform.Angle + joint.ReferenceAngle,
					joint.BodyB.GameObj.Transform.Angle, 
					angularCircleRadB);
			}

			if (joint.MotorEnabled)
			    this.DrawLocalAxisMotor(canvas, joint.BodyA, joint.BodyB, joint.MovementAxis, joint.LocalAnchorA, joint.LocalAnchorB, joint.MotorSpeed, joint.MaxMotorForce, joint.BodyA.BoundRadius * 1.15f);

			this.DrawLocalAnchor(canvas, joint.BodyA, joint.LocalAnchorA);
			this.DrawLocalAnchor(canvas, joint.BodyB, joint.LocalAnchorB);
		}
		private void DrawJoint(Canvas canvas, WeldJointInfo joint)
		{
			float angularCircleRadA = joint.BodyA.BoundRadius * 0.25f;
			float angularCircleRadB = joint.BodyB.BoundRadius * 0.25f;

			float anchorDist = this.GetAnchorDist(joint.BodyA, joint.BodyB, joint.LocalAnchorA, joint.LocalAnchorB);
			bool displaySecondCollider = anchorDist >= angularCircleRadA + angularCircleRadB;

			this.DrawLocalPosConstraint(canvas, joint.BodyA, joint.BodyB, joint.LocalAnchorA, joint.LocalAnchorB);

			this.DrawLocalAnchor(canvas, joint.BodyA, joint.LocalAnchorA);
			this.DrawLocalAnchor(canvas, joint.BodyB, joint.LocalAnchorB);

			this.DrawLocalAngleConstraint(canvas, 
				joint.BodyA, 
				joint.LocalAnchorA, 
				joint.BodyB.GameObj.Transform.Angle - joint.RefAngle, 
				joint.BodyA.GameObj.Transform.Angle, 
				angularCircleRadA);
			if (displaySecondCollider)
			{
				this.DrawLocalAngleConstraint(canvas, 
					joint.BodyB, 
					joint.LocalAnchorB, 
					joint.BodyA.GameObj.Transform.Angle + joint.RefAngle,
					joint.BodyB.GameObj.Transform.Angle, 
					angularCircleRadB);
			}
		}
		private void DrawJoint(Canvas canvas, RopeJointInfo joint)
		{
			this.DrawLocalDistConstraint(canvas, joint.BodyA, joint.BodyB, joint.LocalAnchorA, joint.LocalAnchorB, 0.0f, joint.MaxLength);
			this.DrawLocalAnchor(canvas, joint.BodyB, joint.LocalAnchorB);
			this.DrawLocalAnchor(canvas, joint.BodyA, joint.LocalAnchorA);
		}
		private void DrawJoint(Canvas canvas, SliderJointInfo joint)
		{
			this.DrawLocalDistConstraint(canvas, joint.BodyA, joint.BodyB, joint.LocalAnchorA, joint.LocalAnchorB, joint.MinLength, joint.MaxLength);
			this.DrawLocalAnchor(canvas, joint.BodyB, joint.LocalAnchorB);
			this.DrawLocalAnchor(canvas, joint.BodyA, joint.LocalAnchorA);
		}
		private void DrawJoint(Canvas canvas, LineJointInfo joint)
		{
			Vector2 anchorAToWorld = joint.BodyA.GameObj.Transform.GetWorldPoint(joint.CarAnchor);

			this.DrawWorldPosConstraint(canvas, joint.BodyA, joint.CarAnchor, anchorAToWorld);
			this.DrawLocalAxisConstraint(canvas, joint.BodyB, joint.BodyA, joint.MovementAxis, joint.WheelAnchor, joint.CarAnchor, -joint.BodyB.BoundRadius * 0.25f, joint.BodyB.BoundRadius * 0.25f);
			this.DrawLocalAnchor(canvas, joint.BodyA, joint.CarAnchor);
			this.DrawLocalAnchor(canvas, joint.BodyB, joint.WheelAnchor);
			
			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, this.JointColor));
			this.DrawLocalText(canvas, joint.BodyA, "Car", Vector2.Zero, 0.0f);
			this.DrawLocalText(canvas, joint.BodyB, "Wheel", Vector2.Zero, 0.0f);

			if (joint.MotorEnabled)
			{
				this.DrawLocalAngleMotor(canvas, joint.BodyB, Vector2.Zero, joint.MotorSpeed, joint.MaxMotorTorque, joint.BodyB.BoundRadius * 1.15f);
			}
		}
		private void DrawJoint(Canvas canvas, PulleyJointInfo joint)
		{
			float maxLenA = MathF.Min(joint.MaxLengthA, joint.TotalLength - (joint.Ratio * joint.LengthB));
			float maxLenB = MathF.Min(joint.MaxLengthB, joint.Ratio * (joint.TotalLength - joint.LengthA));

			this.DrawWorldDistConstraint(canvas, joint.BodyA, joint.LocalAnchorA, joint.WorldAnchorA, 0.0f, maxLenA);
			this.DrawWorldDistConstraint(canvas, joint.BodyB, joint.LocalAnchorB, joint.WorldAnchorB, 0.0f, maxLenB);
			this.DrawWorldLooseConstraint(canvas, joint.BodyA, joint.WorldAnchorA, joint.WorldAnchorB);
			this.DrawLocalAnchor(canvas, joint.BodyB, joint.LocalAnchorB);
			this.DrawLocalAnchor(canvas, joint.BodyA, joint.LocalAnchorA);
		}
		private void DrawJoint(Canvas canvas, GearJointInfo joint)
		{
			this.DrawLocalLooseConstraint(canvas, joint.BodyA, joint.BodyB, Vector2.Zero, Vector2.Zero);
		}
		
		private void DrawLocalText(Canvas canvas, RigidBody body, string text, Vector2 pos, float baseAngle)
		{
			this.DrawLocalText(canvas, body, text, pos, Vector2.Zero, baseAngle);
		}
		private void DrawLocalText(Canvas canvas, RigidBody body, string text, Vector2 pos, Vector2 handle, float baseAngle)
		{
			Vector3 bodyPos = body.GameObj.Transform.Pos;
			Vector2 textSize = canvas.MeasureText(text);
			bool flipText = MathF.TurnDir(baseAngle - canvas.DrawDevice.RefAngle, MathF.RadAngle90) < 0;
			baseAngle = MathF.NormalizeAngle(flipText ? baseAngle + MathF.RadAngle180 : baseAngle);

			handle *= textSize;
			if (flipText) handle = textSize - handle;

			canvas.CurrentState.TransformHandle = handle;
			canvas.CurrentState.TransformAngle = baseAngle;
			canvas.DrawText(text, 
				bodyPos.X + pos.X, 
				bodyPos.Y + pos.Y, 
				bodyPos.Z);
			canvas.CurrentState.TransformAngle = 0.0f;
			canvas.CurrentState.TransformHandle = Vector2.Zero;
		}
		private void DrawLocalAnchor(Canvas canvas, RigidBody body, Vector2 anchor)
		{
			Vector3 colliderPos = body.GameObj.Transform.Pos;

			ColorRgba clr = this.JointColor;
			float markerCircleRad = body.BoundRadius * 0.02f;
			Vector2 anchorToWorld = body.GameObj.Transform.GetWorldVector(anchor);

			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.FillCircle(
				colliderPos.X + anchorToWorld.X,
				colliderPos.Y + anchorToWorld.Y,
				colliderPos.Z,
				markerCircleRad);
		}
		private void DrawLocalFrictionMarker(Canvas canvas, RigidBody body, Vector2 anchor)
		{
			Vector3 colliderPos = body.GameObj.Transform.Pos;

			ColorRgba clr = this.JointColor;
			float markerCircleRad = body.BoundRadius * 0.02f;
			Vector2 anchorToWorld = body.GameObj.Transform.GetWorldVector(anchor);

			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.FillCircle(
				colliderPos.X + anchorToWorld.X,
				colliderPos.Y + anchorToWorld.Y,
				colliderPos.Z,
				markerCircleRad * 0.5f);
			canvas.DrawCircle(
				colliderPos.X + anchorToWorld.X,
				colliderPos.Y + anchorToWorld.Y,
				colliderPos.Z,
				markerCircleRad);
			canvas.DrawCircle(
				colliderPos.X + anchorToWorld.X,
				colliderPos.Y + anchorToWorld.Y,
				colliderPos.Z,
				markerCircleRad * 1.5f);
		}
		private void DrawLocalAngleConstraint(Canvas canvas, RigidBody body, Vector2 anchor, float targetAngle, float currentAngle, float radius)
		{
			Vector3 bodyPos = body.GameObj.Transform.Pos;

			ColorRgba clr = this.JointColor;
			ColorRgba clrErr = this.JointErrorColor;

			Vector2 anchorToWorld = body.GameObj.Transform.GetWorldVector(anchor);
			Vector2 angleVec = Vector2.FromAngleLength(targetAngle, radius);
			Vector2 errorVec = Vector2.FromAngleLength(currentAngle, radius);
			bool hasError = MathF.CircularDist(targetAngle, currentAngle) >= MathF.RadAngle1;

			if (hasError)
			{
				float circleBegin = currentAngle;
				float circleEnd = targetAngle;
				if (MathF.TurnDir(circleBegin, circleEnd) < 0)
				{
					MathF.Swap(ref circleBegin, ref circleEnd);
					circleEnd = circleBegin + MathF.CircularDist(circleBegin, circleEnd);
				}

				canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clrErr));
				canvas.DrawLine(
					bodyPos.X + anchorToWorld.X,
					bodyPos.Y + anchorToWorld.Y,
					bodyPos.Z, 
					bodyPos.X + anchorToWorld.X + errorVec.X,
					bodyPos.Y + anchorToWorld.Y + errorVec.Y,
					bodyPos.Z);
				canvas.DrawCircleSegment(
					bodyPos.X + anchorToWorld.X,
					bodyPos.Y + anchorToWorld.Y,
					bodyPos.Z,
					radius,
					circleBegin,
					circleEnd);
				this.DrawLocalText(canvas, body,
					string.Format("{0:F0}°", MathF.RadToDeg(MathF.NormalizeAngle(currentAngle))),
					anchorToWorld + errorVec,
					Vector2.UnitY,
					errorVec.Angle);
			}
			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.DrawLine(
				bodyPos.X + anchorToWorld.X,
				bodyPos.Y + anchorToWorld.Y,
				bodyPos.Z, 
				bodyPos.X + anchorToWorld.X + angleVec.X,
				bodyPos.Y + anchorToWorld.Y + angleVec.Y,
				bodyPos.Z);
			this.DrawLocalText(canvas, body,
				string.Format("{0:F0}°", MathF.RadToDeg(MathF.NormalizeAngle(targetAngle))),
				anchorToWorld + angleVec,
				angleVec.Angle);
		}
		private void DrawLocalAngleConstraint(Canvas canvas, RigidBody body, Vector2 anchor, float minAngle, float maxAngle, float currentAngle, float radius)
		{
			Vector3 bodyPos = body.GameObj.Transform.Pos;

			ColorRgba clr = this.JointColor;
			ColorRgba clrErr = this.JointErrorColor;

			Vector2 anchorToWorld = body.GameObj.Transform.GetWorldVector(anchor);
			Vector2 angleVecMin = Vector2.FromAngleLength(minAngle, radius);
			Vector2 angleVecMax = Vector2.FromAngleLength(maxAngle, radius);
			Vector2 errorVec = Vector2.FromAngleLength(currentAngle, radius);
			float angleDistMin = MathF.Abs(currentAngle - minAngle);
			float angleDistMax = MathF.Abs(currentAngle - maxAngle);
			float angleRange = maxAngle - minAngle;
			bool hasError = angleDistMin > angleDistMax ? angleDistMin >= angleRange : angleDistMax >= angleRange;

			if (hasError)
			{
				float circleBegin = currentAngle;
				float circleEnd = angleDistMin < angleDistMax ? minAngle : maxAngle;
				if (MathF.TurnDir(circleBegin, circleEnd) < 0)
				{
					MathF.Swap(ref circleBegin, ref circleEnd);
					circleEnd = circleBegin + MathF.CircularDist(circleBegin, circleEnd);
				}

				canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clrErr));
				canvas.DrawLine(
					bodyPos.X + anchorToWorld.X,
					bodyPos.Y + anchorToWorld.Y,
					bodyPos.Z, 
					bodyPos.X + anchorToWorld.X + errorVec.X,
					bodyPos.Y + anchorToWorld.Y + errorVec.Y,
					bodyPos.Z);
				canvas.DrawCircleSegment(
					bodyPos.X + anchorToWorld.X,
					bodyPos.Y + anchorToWorld.Y,
					bodyPos.Z,
					radius,
					circleBegin,
					circleEnd);
				this.DrawLocalText(canvas, body,
					string.Format("{0:F0}°", MathF.RadToDeg(currentAngle)),
					anchorToWorld + errorVec,
					Vector2.UnitY,
					errorVec.Angle);
			}
			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.DrawCircleSegment(
				bodyPos.X + anchorToWorld.X,
				bodyPos.Y + anchorToWorld.Y,
				bodyPos.Z,
				radius,
				minAngle,
				maxAngle);
			canvas.DrawLine(
				bodyPos.X + anchorToWorld.X,
				bodyPos.Y + anchorToWorld.Y,
				bodyPos.Z, 
				bodyPos.X + anchorToWorld.X + angleVecMin.X,
				bodyPos.Y + anchorToWorld.Y + angleVecMin.Y,
				bodyPos.Z);
			canvas.DrawLine(
				bodyPos.X + anchorToWorld.X,
				bodyPos.Y + anchorToWorld.Y,
				bodyPos.Z, 
				bodyPos.X + anchorToWorld.X + angleVecMax.X,
				bodyPos.Y + anchorToWorld.Y + angleVecMax.Y,
				bodyPos.Z);
			this.DrawLocalText(canvas, body,
				string.Format("{0:F0}°", MathF.RadToDeg(minAngle)),
				anchorToWorld + angleVecMin,
				angleVecMin.Angle);
			this.DrawLocalText(canvas, body,
				string.Format("{0:F0}°", MathF.RadToDeg(maxAngle)),
				anchorToWorld + angleVecMax,
				angleVecMax.Angle);
		}
		private void DrawLocalAngleMotor(Canvas canvas, RigidBody body, Vector2 anchor, float speed, float maxTorque, float radius)
		{
			Vector3 bodyPos = body.GameObj.Transform.Pos;

			ColorRgba clr = this.MotorColor;

			float baseAngle = body.GameObj.Transform.Angle;
			float speedAngle = baseAngle + speed;
			float maxTorqueAngle = baseAngle + MathF.Sign(speed) * maxTorque * 0.01f;
			Vector2 anchorToWorld = body.GameObj.Transform.GetWorldVector(anchor);
			Vector2 arrowBase = anchorToWorld + Vector2.FromAngleLength(speedAngle, radius);
			Vector2 arrorA = Vector2.FromAngleLength(speedAngle - MathF.RadAngle45, MathF.Sign(speed) * radius * 0.05f);
			Vector2 arrorB = Vector2.FromAngleLength(speedAngle - MathF.RadAngle45 + MathF.RadAngle270, MathF.Sign(speed) * radius * 0.05f);

			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.DrawCircleSegment(
				bodyPos.X + anchorToWorld.X,
				bodyPos.Y + anchorToWorld.Y,
				bodyPos.Z,
				radius - 2,
				MathF.Sign(speed) >= 0 ? baseAngle : maxTorqueAngle,
				MathF.Sign(speed) >= 0 ? maxTorqueAngle : baseAngle);
			canvas.DrawCircleSegment(
				bodyPos.X + anchorToWorld.X,
				bodyPos.Y + anchorToWorld.Y,
				bodyPos.Z,
				radius + 2,
				MathF.Sign(speed) >= 0 ? baseAngle : maxTorqueAngle,
				MathF.Sign(speed) >= 0 ? maxTorqueAngle : baseAngle);
			canvas.DrawCircleSegment(
				bodyPos.X + anchorToWorld.X,
				bodyPos.Y + anchorToWorld.Y,
				bodyPos.Z,
				radius,
				MathF.Sign(speed) >= 0 ? baseAngle : speedAngle,
				MathF.Sign(speed) >= 0 ? speedAngle : baseAngle);
			canvas.DrawLine(
				bodyPos.X + arrowBase.X,
				bodyPos.Y + arrowBase.Y,
				bodyPos.Z,
				bodyPos.X + arrowBase.X + arrorA.X,
				bodyPos.Y + arrowBase.Y + arrorA.Y,
				bodyPos.Z);
			canvas.DrawLine(
				bodyPos.X + arrowBase.X,
				bodyPos.Y + arrowBase.Y,
				bodyPos.Z,
				bodyPos.X + arrowBase.X + arrorB.X,
				bodyPos.Y + arrowBase.Y + arrorB.Y,
				bodyPos.Z);
		}
		private void DrawLocalPosConstraint(Canvas canvas, RigidBody bodyA, RigidBody bodyB, Vector2 anchorA, Vector2 anchorB)
		{
			Vector3 colliderPosA = bodyA.GameObj.Transform.Pos;
			Vector3 colliderPosB = bodyB.GameObj.Transform.Pos;

			ColorRgba clr = this.JointColor;
			ColorRgba clrErr = this.JointErrorColor;

			Vector2 anchorAToWorld = bodyA.GameObj.Transform.GetWorldVector(anchorA);
			Vector2 anchorBToWorld = bodyB.GameObj.Transform.GetWorldVector(anchorB);
			Vector2 errorVec = (colliderPosB.Xy + anchorBToWorld) - (colliderPosA.Xy + anchorAToWorld);
			
			bool hasError = errorVec.Length >= 1.0f;
			if (hasError)
			{
				canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clrErr));
				canvas.DrawLine(
					colliderPosA.X + anchorAToWorld.X,
					colliderPosA.Y + anchorAToWorld.Y,
					colliderPosA.Z,
					colliderPosB.X + anchorBToWorld.X,
					colliderPosB.Y + anchorBToWorld.Y,
					colliderPosB.Z);
				this.DrawLocalText(canvas, bodyA, 
					string.Format("{0:F1}", errorVec.Length), 
					anchorAToWorld + errorVec * 0.5f, 
					new Vector2(0.5f, 0.0f), 
					errorVec.PerpendicularLeft.Angle);
			}

			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.DrawLine(
				colliderPosA.X,
				colliderPosA.Y,
				colliderPosA.Z,
				colliderPosA.X + anchorAToWorld.X,
				colliderPosA.Y + anchorAToWorld.Y,
				colliderPosA.Z);
			canvas.DrawLine(
				colliderPosB.X,
				colliderPosB.Y,
				colliderPosB.Z,
				colliderPosB.X + anchorBToWorld.X,
				colliderPosB.Y + anchorBToWorld.Y,
				colliderPosB.Z);
		}
		private void DrawLocalDistConstraint(Canvas canvas, RigidBody bodyA, RigidBody bodyB, Vector2 localAnchorA, Vector2 localAnchorB, float minDist, float maxDist)
		{
			Vector3 bodyPosA = bodyA.GameObj.Transform.Pos;
			Vector3 bodyPosB = bodyB.GameObj.Transform.Pos;

			ColorRgba clr = this.JointColor;
			ColorRgba clrErr = this.JointErrorColor;
			
			float markerCircleRad = bodyA.BoundRadius * 0.02f;
			Vector2 anchorA = bodyA.GameObj.Transform.GetWorldVector(localAnchorA);
			Vector2 anchorB = bodyB.GameObj.Transform.GetWorldVector(localAnchorB);
			Vector2 errorVec = (bodyPosB.Xy + anchorB) - (bodyPosA.Xy + anchorA);
			float dist = errorVec.Length;
			Vector2 distVec = errorVec.Normalized * MathF.Clamp(dist, minDist, maxDist);
			Vector2 lineNormal = errorVec.PerpendicularRight.Normalized;
			bool hasError = (errorVec - distVec).Length >= 1.0f;

			if (hasError)
			{
				canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clrErr));
				canvas.DrawLine(
					bodyPosA.X + anchorA.X + distVec.X,
					bodyPosA.Y + anchorA.Y + distVec.Y,
					bodyPosA.Z, 
					bodyPosA.X + anchorA.X + errorVec.X,
					bodyPosA.Y + anchorA.Y + errorVec.Y,
					bodyPosA.Z);
				this.DrawLocalText(canvas, bodyA,
					string.Format("{0:F1}", dist),
					anchorA + errorVec,
					Vector2.UnitY,
					errorVec.Angle);
			}
			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.DrawLine(
				bodyPosA.X + anchorA.X,
				bodyPosA.Y + anchorA.Y,
				bodyPosA.Z, 
				bodyPosA.X + anchorA.X + distVec.X,
				bodyPosA.Y + anchorA.Y + distVec.Y,
				bodyPosA.Z);
			if (hasError)
			{
				canvas.DrawLine(
					bodyPosA.X + anchorA.X + distVec.X - lineNormal.X * 5.0f,
					bodyPosA.Y + anchorA.Y + distVec.Y - lineNormal.Y * 5.0f,
					bodyPosA.Z, 
					bodyPosA.X + anchorA.X + distVec.X + lineNormal.X * 5.0f,
					bodyPosA.Y + anchorA.Y + distVec.Y + lineNormal.Y * 5.0f,
					bodyPosA.Z);
			}
			this.DrawLocalText(canvas, bodyA,
				string.Format("{0:F1}", MathF.Clamp(dist, minDist, maxDist)),
				anchorA + distVec,
				Vector2.Zero,
				errorVec.Angle);
		}
		private void DrawLocalAxisConstraint(Canvas canvas, RigidBody bodyA, RigidBody bodyB, Vector2 localAxis, Vector2 localAnchorA, Vector2 localAnchorB, float min = 1, float max = -1)
		{
			Vector3 bodyPosA = bodyA.GameObj.Transform.Pos;
			Vector3 bodyPosB = bodyB.GameObj.Transform.Pos;
			Vector2 worldAxis = bodyB.GameObj.Transform.GetWorldVector(localAxis).Normalized;
			Vector2 worldAnchorB = bodyB.GameObj.Transform.GetWorldPoint(localAnchorB);

			this.DrawWorldAxisConstraint(canvas, bodyA, worldAxis, localAnchorA, worldAnchorB, min, max);
		}
		private void DrawLocalAxisMotor(Canvas canvas, RigidBody bodyA, RigidBody bodyB, Vector2 localAxis, Vector2 localAnchorA, Vector2 localAnchorB, float speed, float maxForce, float offset)
		{
			Vector3 bodyPosA = bodyA.GameObj.Transform.Pos;
			Vector3 bodyPosB = bodyB.GameObj.Transform.Pos;
			Vector2 worldAxis = bodyB.GameObj.Transform.GetWorldVector(localAxis).Normalized;
			Vector2 worldAnchorB = bodyB.GameObj.Transform.GetWorldPoint(localAnchorB);

			this.DrawWorldAxisMotor(canvas, bodyA, worldAxis, localAnchorA, worldAnchorB, speed, maxForce, offset);
		}
		private void DrawLocalLooseConstraint(Canvas canvas, RigidBody bodyA, RigidBody bodyB, Vector2 anchorA, Vector2 anchorB)
		{
			Vector3 bodyPosA = bodyA.GameObj.Transform.Pos;
			Vector3 bodyPosB = bodyB.GameObj.Transform.Pos;

			ColorRgba clr = this.JointColor;

			Vector2 anchorAToWorld = bodyA.GameObj.Transform.GetWorldVector(anchorA);
			Vector2 anchorBToWorld = bodyB.GameObj.Transform.GetWorldVector(anchorB);

			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.DrawDashLine(
				bodyPosA.X + anchorAToWorld.X,
				bodyPosA.Y + anchorAToWorld.Y,
				bodyPosA.Z,
				bodyPosB.X + anchorBToWorld.X,
				bodyPosB.Y + anchorBToWorld.Y,
				bodyPosB.Z);
		}
		
		private void DrawWorldAnchor(Canvas canvas, RigidBody body, Vector2 anchor)
		{
			Vector3 colliderPos = body.GameObj.Transform.Pos;
			float markerCircleRad = body.BoundRadius * 0.02f;
			ColorRgba clr = this.JointColor;

			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.FillCircle(
				anchor.X,
				anchor.Y,
				colliderPos.Z,
				markerCircleRad);
		}
		private void DrawWorldPosConstraint(Canvas canvas, RigidBody body, Vector2 localAnchor, Vector2 worldAnchor)
		{
			Vector3 bodyPos = body.GameObj.Transform.Pos;

			ColorRgba clr = this.JointColor;
			ColorRgba clrErr = this.JointErrorColor;

			float angularCircleRadA = body.BoundRadius * 0.25f;
			float markerCircleRad = body.BoundRadius * 0.02f;
			Vector2 anchorAToWorld = body.GameObj.Transform.GetWorldVector(localAnchor);
			Vector2 errorVec = worldAnchor - (bodyPos.Xy + anchorAToWorld);
			
			bool hasError = errorVec.Length >= 1.0f;
			if (hasError)
			{
				canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clrErr));
				canvas.DrawLine(
					bodyPos.X + anchorAToWorld.X,
					bodyPos.Y + anchorAToWorld.Y,
					bodyPos.Z,
					worldAnchor.X,
					worldAnchor.Y,
					bodyPos.Z);
				this.DrawLocalText(canvas, body,
					string.Format("{0:F1}", errorVec.Length),
					anchorAToWorld + errorVec * 0.5f,
					new Vector2(0.5f, 0.0f),
					errorVec.PerpendicularLeft.Angle);
			}

			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.DrawLine(
				bodyPos.X,
				bodyPos.Y,
				bodyPos.Z,
				bodyPos.X + anchorAToWorld.X,
				bodyPos.Y + anchorAToWorld.Y,
				bodyPos.Z);
		}
		private void DrawWorldDistConstraint(Canvas canvas, RigidBody body, Vector2 localAnchor, Vector2 worldAnchor, float minDist, float maxDist)
		{
			Vector3 colliderPosA = body.GameObj.Transform.Pos;

			ColorRgba clr = this.JointColor;
			ColorRgba clrErr = this.JointErrorColor;
			
			float markerCircleRad = body.BoundRadius * 0.02f;
			Vector2 anchorA = body.GameObj.Transform.GetWorldVector(localAnchor);
			Vector2 errorVec = worldAnchor - (colliderPosA.Xy + anchorA);
			Vector2 lineNormal = errorVec.PerpendicularRight.Normalized;
			float dist = errorVec.Length;
			Vector2 distVec = errorVec.Normalized * MathF.Clamp(dist, minDist, maxDist);
			bool hasError = (errorVec - distVec).Length >= 1.0f;

			if (hasError)
			{
				canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clrErr));
				canvas.DrawLine(
					colliderPosA.X + anchorA.X + distVec.X,
					colliderPosA.Y + anchorA.Y + distVec.Y,
					colliderPosA.Z, 
					colliderPosA.X + anchorA.X + errorVec.X,
					colliderPosA.Y + anchorA.Y + errorVec.Y,
					colliderPosA.Z);
				this.DrawLocalText(canvas, body,
					string.Format("{0:F1}", dist),
					anchorA + errorVec,
					Vector2.UnitY,
					errorVec.Angle);
			}
			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.DrawLine(
				colliderPosA.X + anchorA.X,
				colliderPosA.Y + anchorA.Y,
				colliderPosA.Z, 
				colliderPosA.X + anchorA.X + distVec.X,
				colliderPosA.Y + anchorA.Y + distVec.Y,
				colliderPosA.Z);
			if (hasError)
			{
				canvas.DrawLine(
					colliderPosA.X + anchorA.X + distVec.X - lineNormal.X * 5.0f,
					colliderPosA.Y + anchorA.Y + distVec.Y - lineNormal.Y * 5.0f,
					colliderPosA.Z, 
					colliderPosA.X + anchorA.X + distVec.X + lineNormal.X * 5.0f,
					colliderPosA.Y + anchorA.Y + distVec.Y + lineNormal.Y * 5.0f,
					colliderPosA.Z);
			}
			this.DrawLocalText(canvas, body,
				string.Format("{0:F1}", MathF.Clamp(dist, minDist, maxDist)),
				anchorA + distVec,
				Vector2.Zero,
				errorVec.Angle);
		}
		private void DrawWorldAxisConstraint(Canvas canvas, RigidBody body, Vector2 worldAxis, Vector2 localAnchor, Vector2 worldAnchor, float min = 1, float max = -1)
		{
			Vector3 bodyPos = body.GameObj.Transform.Pos;
			worldAxis = worldAxis.Normalized;
			bool infinite = false;
			if (min > max)
			{
				min = -10000000.0f;
				max = 10000000.0f;
				infinite = true;
			}
			Vector2 anchorToWorld = body.GameObj.Transform.GetWorldVector(localAnchor);
			float axisVal = Vector2.Dot(bodyPos.Xy + anchorToWorld - worldAnchor, worldAxis);
			Vector2 basePos = MathF.PointLineNearestPoint(
				bodyPos.X + anchorToWorld.X, 
				bodyPos.Y + anchorToWorld.Y, 
				worldAnchor.X + worldAxis.X * min, 
				worldAnchor.Y + worldAxis.Y * min, 
				worldAnchor.X + worldAxis.X * max, 
				worldAnchor.Y + worldAxis.Y * max,
				infinite);
			float errorVal = (bodyPos.Xy + anchorToWorld - basePos).Length;
			Vector2 errorVec = basePos - bodyPos.Xy;
			bool hasError = errorVal >= 1.0f;

			ColorRgba clr = this.JointColor;
			ColorRgba clrErr = this.JointErrorColor;
			
			if (hasError)
			{
				canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clrErr));
				canvas.DrawLine(
					bodyPos.X + anchorToWorld.X,
					bodyPos.Y + anchorToWorld.Y,
					bodyPos.Z,
					basePos.X,
					basePos.Y,
					bodyPos.Z);
				this.DrawLocalText(canvas, body,
					string.Format("{0:F1}", errorVal),
					errorVec * 0.5f,
					new Vector2(0.5f, 0.0f),
					errorVec.PerpendicularLeft.Angle);
			}

			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.DrawLine(
				worldAnchor.X + worldAxis.X * min,
				worldAnchor.Y + worldAxis.Y * min,
				bodyPos.Z,
				worldAnchor.X + worldAxis.X * max,
				worldAnchor.Y + worldAxis.Y * max,
				bodyPos.Z);
			if (!infinite)
			{
				canvas.DrawLine(
					worldAnchor.X + worldAxis.X * min + worldAxis.PerpendicularLeft.X * 5.0f,
					worldAnchor.Y + worldAxis.Y * min + worldAxis.PerpendicularLeft.Y * 5.0f,
					bodyPos.Z,
					worldAnchor.X + worldAxis.X * min + worldAxis.PerpendicularRight.X * 5.0f,
					worldAnchor.Y + worldAxis.Y * min + worldAxis.PerpendicularRight.Y * 5.0f,
					bodyPos.Z);
				canvas.DrawLine(
					worldAnchor.X + worldAxis.X * max + worldAxis.PerpendicularLeft.X * 5.0f,
					worldAnchor.Y + worldAxis.Y * max + worldAxis.PerpendicularLeft.Y * 5.0f,
					bodyPos.Z,
					worldAnchor.X + worldAxis.X * max + worldAxis.PerpendicularRight.X * 5.0f,
					worldAnchor.Y + worldAxis.Y * max + worldAxis.PerpendicularRight.Y * 5.0f,
					bodyPos.Z);
			}
		}
		private void DrawWorldAxisMotor(Canvas canvas, RigidBody body, Vector2 worldAxis, Vector2 localAnchor, Vector2 worldAnchor, float speed, float maxForce, float offset)
		{
			Vector3 bodyPos = body.GameObj.Transform.Pos;

			ColorRgba clr = this.MotorColor;

			Vector2 anchorToWorld = body.GameObj.Transform.GetWorldVector(localAnchor);
			float axisAngle = worldAxis.Angle;
			float maxForceTemp = MathF.Sign(speed) * maxForce * 0.1f;
			Vector2 arrowBegin = bodyPos.Xy + worldAxis.PerpendicularRight * offset;
			Vector2 arrowBase = arrowBegin + worldAxis * speed * 10.0f;
			Vector2 arrowA = Vector2.FromAngleLength(axisAngle + MathF.RadAngle45 + MathF.RadAngle180, MathF.Sign(speed) * MathF.Max(offset * 0.05f, 5.0f));
			Vector2 arrowB = Vector2.FromAngleLength(axisAngle - MathF.RadAngle45 + MathF.RadAngle180, MathF.Sign(speed) * MathF.Max(offset * 0.05f, 5.0f));
			
			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.DrawLine(
				arrowBegin.X + worldAxis.PerpendicularLeft.X * 2.0f,
				arrowBegin.Y + worldAxis.PerpendicularLeft.Y * 2.0f,
				bodyPos.Z,
				arrowBegin.X + worldAxis.PerpendicularLeft.X * 2.0f + worldAxis.X * maxForceTemp,
				arrowBegin.Y + worldAxis.PerpendicularLeft.Y * 2.0f + worldAxis.Y * maxForceTemp,
				bodyPos.Z);
			canvas.DrawLine(
				arrowBegin.X + worldAxis.PerpendicularRight.X * 2.0f,
				arrowBegin.Y + worldAxis.PerpendicularRight.Y * 2.0f,
				bodyPos.Z,
				arrowBegin.X + worldAxis.PerpendicularRight.X * 2.0f + worldAxis.X * maxForceTemp,
				arrowBegin.Y + worldAxis.PerpendicularRight.Y * 2.0f + worldAxis.Y * maxForceTemp,
				bodyPos.Z);
			canvas.DrawLine(
				arrowBegin.X,
				arrowBegin.Y,
				bodyPos.Z,
				arrowBase.X,
				arrowBase.Y,
				bodyPos.Z);
			canvas.DrawLine(
				arrowBase.X,
				arrowBase.Y,
				bodyPos.Z,
				arrowBase.X + arrowA.X,
				arrowBase.Y + arrowA.Y,
				bodyPos.Z);
			canvas.DrawLine(
				arrowBase.X,
				arrowBase.Y,
				bodyPos.Z,
				arrowBase.X + arrowB.X,
				arrowBase.Y + arrowB.Y,
				bodyPos.Z);
		}
		private void DrawWorldLooseConstraint(Canvas canvas, RigidBody bodyA, Vector2 anchorA, Vector2 anchorB)
		{
			Vector3 bodyPosA = bodyA.GameObj.Transform.Pos;

			ColorRgba clr = this.JointColor;

			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, clr));
			canvas.DrawDashLine(
				anchorA.X,
				anchorA.Y,
				bodyPosA.Z,
				anchorB.X,
				anchorB.Y,
				bodyPosA.Z);
		}

		private float GetAnchorDist(RigidBody bodyA, RigidBody bodyB, Vector2 localAnchorA, Vector2 localAnchorB)
		{
			Vector3 colliderPosA = bodyA.GameObj.Transform.Pos;
			Vector3 colliderPosB = bodyB.GameObj.Transform.Pos;

			Vector2 anchorAToWorld = bodyA.GameObj.Transform.GetWorldVector(localAnchorA);
			Vector2 anchorBToWorld = bodyB.GameObj.Transform.GetWorldVector(localAnchorB);
			Vector2 errorVec = (colliderPosB.Xy + anchorBToWorld) - (colliderPosA.Xy + anchorAToWorld);

			return errorVec.Length;
		}

		private IEnumerable<RigidBody> QueryVisibleColliders()
		{
			var allColliders = Scene.Current.FindComponents<RigidBody>();
			return allColliders.Where(r => 
				r.Active && 
				!CorePluginRegistry.GetDesignTimeData(r.GameObj).IsHidden && 
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
