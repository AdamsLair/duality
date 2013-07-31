using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Resources;

using DualityEditor;

using EditorBase.PluginRes;
using EditorBase.CamViewStates;

using OpenTK;

namespace EditorBase.UndoRedoActions
{
	public class RotateCamViewObjAction : CamViewObjAction
	{
		private Vector3[]	backupPos	= null;
		private float[]		backupAngle	= null;
		private	float		turnBy		= 0.0f;

		public override string Name
		{
			get { return this.targetObj.Length == 1 ? 
				string.Format(EditorBaseRes.UndoRedo_RotateCamViewObj, this.targetObj[0].DisplayObjectName) :
				string.Format(EditorBaseRes.UndoRedo_RotateCamViewObjMulti, this.targetObj.Length); }
		}
		public override bool IsVoid
		{
			get { return base.IsVoid || this.turnBy == 0.0f; }
		}

		public RotateCamViewObjAction(IEnumerable<CamViewState.SelObj> obj, PostPerformAction postPerform, float turnBy) : base(obj, postPerform)
		{
			this.turnBy = turnBy;
		}

		public override bool CanAppend(UndoRedoAction action)
		{
			return action is RotateCamViewObjAction && base.CanAppend(action);
		}
		public override void Append(UndoRedoAction action, bool performAction)
		{
			base.Append(action, performAction);
			RotateCamViewObjAction moveAction = action as RotateCamViewObjAction;
			if (performAction)
			{
				moveAction.backupPos = this.backupPos;
				moveAction.backupAngle = this.backupAngle;
				moveAction.Do();
			}
			this.turnBy += moveAction.turnBy;
		}
		public override void Do()
		{
			if (this.backupPos == null)
			{
				this.backupPos = new Vector3[this.targetObj.Length];
				this.backupAngle = new float[this.targetObj.Length];
				for (int i = 0; i < this.targetObj.Length; i++)
				{
					this.backupPos[i] = this.targetObj[i].Pos;
					this.backupAngle[i] = this.targetObj[i].Angle;
				}
			}
			
			Vector3 center = Vector3.Zero;
			foreach (CamViewState.SelObj s in this.targetObj)
			{
				center += s.Pos;
			}
			if (this.targetObj.Length > 0) center /= this.targetObj.Length;

			foreach (CamViewState.SelObj s in this.targetObj)
			{
				Vector3 posRelCenter = s.Pos - center;
				Vector3 posRelCenterTarget = posRelCenter;
				MathF.TransformCoord(ref posRelCenterTarget.X, ref posRelCenterTarget.Y, this.turnBy);
				s.Pos = center + posRelCenterTarget;
				s.Angle += this.turnBy;
			}

			if (this.postPerform != null)
				this.postPerform(this.targetObj);
		}
		public override void Undo()
		{
			if (this.backupPos == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");

			for (int i = 0; i < this.backupPos.Length; i++)
			{
				this.targetObj[i].Pos = this.backupPos[i];
				this.targetObj[i].Angle = this.backupAngle[i];
			}

			if (this.postPerform != null)
				this.postPerform(this.targetObj);
		}
	}
}
