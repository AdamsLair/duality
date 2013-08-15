using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Resources;

using DualityEditor.EditorRes;

using OpenTK;

namespace DualityEditor.UndoRedoActions
{
	public class CloneGameObjectAction : GameObjectAction
	{
		private	GameObject[]	targetParentObj	= null;
		private	GameObject[]	resultObj		= null;

		protected override string NameBase
		{
			get { return GeneralRes.UndoRedo_CloneGameObject; }
		}
		protected override string NameBaseMulti
		{
			get { return GeneralRes.UndoRedo_CloneGameObjectMulti; }
		}
		public IEnumerable<GameObject> Result
		{
			get { return this.resultObj; }
		}

		public CloneGameObjectAction(IEnumerable<GameObject> obj) : base(obj) {}

		public override void Do()
		{
			if (this.resultObj == null)
			{
				this.resultObj = new GameObject[this.targetObj.Length];
				this.targetParentObj = new GameObject[this.targetObj.Length];
				for (int i = 0; i < this.targetObj.Length; i++)
				{
					this.resultObj[i] = CloneProvider.DeepClone(this.targetObj[i]);
					this.targetParentObj[i] = this.targetObj[i].Parent;
				}
			}
			else
			{
				for (int i = 0; i < this.targetObj.Length; i++)
					CloneProvider.DeepCopyTo(this.targetObj[i], this.resultObj[i]);
			}

			for (int i = 0; i < this.targetObj.Length; i++)
			{
				GameObject original = this.targetObj[i];
				GameObject clone = this.resultObj[i];
				clone.Parent = this.targetParentObj[i];

				// Make sure the absolute transform is copied
				if (clone.Transform != null && original.Transform != null)
					clone.Transform.SetTransform(original.Transform);

				// Prevent physics from getting crazy.
				if (clone.Transform != null && clone.RigidBody != null)
					clone.Transform.Pos += Vector3.UnitX * 0.001f;

				Scene.Current.AddObject(clone);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}
		public override void Undo()
		{
			if (this.resultObj == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");
			foreach (GameObject clone in this.resultObj)
			{
				clone.Dispose();
				Scene.Current.RemoveObject(clone);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}
	}
}
