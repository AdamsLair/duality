﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.CamView.UndoRedoActions;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	public class SceneEditorSelGameObj : ObjectEditorSelObj
	{
		private	GameObject	gameObj;

		public override object ActualObject
		{
			get { return this.gameObj == null || this.gameObj.Disposed ? null : this.gameObj; }
		}
		public override bool HasTransform
		{
			get { return this.gameObj != null && !this.gameObj.Disposed && this.gameObj.Transform != null; }
		}
		public override Vector3 Pos
		{
			get { return this.gameObj.Transform.Pos; }
			set { this.gameObj.Transform.Pos = value; }
		}
		public override float Angle
		{
			get { return this.gameObj.Transform.Angle; }
			set { this.gameObj.Transform.Angle = value; }
		}
		public override Vector3 Scale
		{
			get { return Vector3.One * this.gameObj.Transform.Scale; }
			set { this.gameObj.Transform.Scale = value.Length / MathF.Sqrt(3.0f); }
		}
		public override float BoundRadius
		{
			get
			{
				ICmpRenderer renderer = this.gameObj.GetComponent<ICmpRenderer>();
				if (renderer == null)
				{
					if (this.gameObj.Transform != null)
						return CamView.DefaultDisplayBoundRadius * this.gameObj.Transform.Scale;
					else
						return CamView.DefaultDisplayBoundRadius;
				}

				CullingInfo info;
				renderer.GetCullingInfo(out info);
				return info.Radius;
			}
		}
		public override bool ShowAngle
		{
			get { return true; }
		}

		public SceneEditorSelGameObj(GameObject obj)
		{
			this.gameObj = obj;
		}

		public override bool IsActionAvailable(ObjectEditorAction action)
		{
			if (action == ObjectEditorAction.Move ||
				action == ObjectEditorAction.Rotate ||
				action == ObjectEditorAction.Scale)
				return this.HasTransform;
			return false;
		}
		public override string UpdateActionText(ObjectEditorAction action, bool performing)
		{
			if (action == ObjectEditorAction.Move)
			{
				return
					string.Format("X:{0,7:0}/n", this.gameObj.Transform.LocalPos.X) +
					string.Format("Y:{0,7:0}/n", this.gameObj.Transform.LocalPos.Y) +
					string.Format("Z:{0,7:0}", this.gameObj.Transform.LocalPos.Z);
			}
			else if (action == ObjectEditorAction.Scale)
			{
				return string.Format("Scale:{0,5:0.00}", this.gameObj.Transform.LocalScale);
			}
			else if (action == ObjectEditorAction.Rotate)
			{
				return string.Format("Angle:{0,5:0}°", MathF.RadToDeg(this.gameObj.Transform.LocalAngle));
			}

			return base.UpdateActionText(action, performing);
		}
	}
}
