using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Resources;

using Duality.Editor;
using Duality.Editor.Plugins.CamView.Properties;
using Duality.Editor.Plugins.CamView.CamViewStates;

using OpenTK;

namespace Duality.Editor.Plugins.CamView.UndoRedoActions
{
	public class MoveCamViewObjAction : CamViewObjAction
	{
		private Vector3[]	backupPos	= null;
		private	Vector3		moveBy		= Vector3.Zero;

		public override string Name
		{
			get { return this.targetObj.Length == 1 ? 
				string.Format(CamViewRes.UndoRedo_MoveCamViewObj, this.targetObj[0].DisplayObjectName) :
				string.Format(CamViewRes.UndoRedo_MoveCamViewObjMulti, this.targetObj.Length); }
		}
		public override bool IsVoid
		{
			get { return base.IsVoid || this.moveBy == Vector3.Zero; }
		}

		public MoveCamViewObjAction(IEnumerable<CamViewState.SelObj> obj, PostPerformAction postPerform, Vector3 moveBy) : base(obj, postPerform)
		{
			this.moveBy = moveBy;
		}

		public override bool CanAppend(UndoRedoAction action)
		{
			return action is MoveCamViewObjAction && base.CanAppend(action);
		}
		public override void Append(UndoRedoAction action, bool performAction)
		{
			base.Append(action, performAction);
			MoveCamViewObjAction moveAction = action as MoveCamViewObjAction;
			if (performAction)
			{
				moveAction.backupPos = this.backupPos;
				moveAction.Do();
			}
			this.moveBy += moveAction.moveBy;
		}
		public override void Do()
		{
			if (this.backupPos == null)
			{
				this.backupPos = new Vector3[this.targetObj.Length];
				for (int i = 0; i < this.targetObj.Length; i++)
					this.backupPos[i] = this.targetObj[i].Pos;
			}

			foreach (CamViewState.SelObj s in this.targetObj)
				s.Pos += this.moveBy;

			if (this.postPerform != null)
				this.postPerform(this.targetObj);
		}
		public override void Undo()
		{
			if (this.backupPos == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");

			for (int i = 0; i < this.backupPos.Length; i++)
				this.targetObj[i].Pos = this.backupPos[i];

			if (this.postPerform != null)
				this.postPerform(this.targetObj);
		}
	}
}
