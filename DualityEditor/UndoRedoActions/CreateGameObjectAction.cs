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
	public class CreateGameObjectAction : GameObjectAction
	{
		private	GameObject		targetParentObj	= null;
		private	GameObject[]	sourceObj		= null;
		
		protected override string NameBase
		{
			get { return GeneralRes.UndoRedo_CreateGameObject; }
		}
		protected override string NameBaseMulti
		{
			get { return GeneralRes.UndoRedo_CreateGameObjectMulti; }
		}
		public IEnumerable<GameObject> Result
		{
			get { return this.targetObj; }
		}

		public CreateGameObjectAction(GameObject parent, IEnumerable<GameObject> obj) : base(obj.Where(o => !Scene.Current.AllObjects.Contains(o)))
		{
			this.targetParentObj = parent;
		}
		public CreateGameObjectAction(GameObject parent, params GameObject[] obj) : this(parent, obj as IEnumerable<GameObject>) {}

		public override void Do()
		{
			if (this.sourceObj == null)
			{
				this.sourceObj = new GameObject[this.targetObj.Length];
				for (int i = 0; i < this.targetObj.Length; i++)
					this.sourceObj[i] = CloneProvider.DeepClone(this.targetObj[i], BackupCloneContext);
			}
			else
			{
				for (int i = 0; i < this.sourceObj.Length; i++)
					CloneProvider.DeepCopyTo(this.sourceObj[i], this.targetObj[i], BackupCloneContext);
			}

			for (int i = 0; i < this.targetObj.Length; i++)
			{
				GameObject obj = this.targetObj[i];
				obj.Parent = this.targetParentObj;
				Scene.Current.AddObject(obj);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}
		public override void Undo()
		{
			if (this.sourceObj == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");
			foreach (GameObject obj in this.targetObj)
			{
				obj.Dispose();
				Scene.Current.RemoveObject(obj);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}
	}
}
