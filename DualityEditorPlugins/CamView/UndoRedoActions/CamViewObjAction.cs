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
	public abstract class CamViewObjAction : UndoRedoAction
	{
		public delegate void PostPerformAction(IEnumerable<CamViewState.SelObj> obj);

		protected	CamViewState.SelObj[]	targetObj	= null;
		protected	PostPerformAction		postPerform = null;
		
		public override bool IsVoid
		{
			get { return this.targetObj == null || this.targetObj.Length == 0; }
		}

		public CamViewObjAction(IEnumerable<CamViewState.SelObj> obj, PostPerformAction postPerform)
		{
			if (obj == null) throw new ArgumentNullException("obj");
			this.targetObj = obj.Where(o => o != null && !o.IsInvalid && o.HasTransform).ToArray();
			this.postPerform = postPerform;
		}

		public override bool CanAppend(UndoRedoAction action)
		{
			CamViewObjAction castAction = action as CamViewObjAction;
			if (castAction == null) return false;
			if (castAction.postPerform != this.postPerform) return false;
			return castAction.targetObj.SetEqual(this.targetObj);
		}
	}
}
