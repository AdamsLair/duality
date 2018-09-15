using System;
using System.Collections.Generic;
using System.Linq;
using Duality.Cloning;
using Duality.Editor.Properties;
using Duality.Resources;

namespace Duality.Editor.UndoRedoActions
{
	public class PasteGameObjectAction : GameObjectAction
	{
		private GameObject[] targetParentObjects	= null;
		private GameObject[] resultObj				= null;

		protected override string NameBase
		{
			get { return GeneralRes.UndoRedo_PasteGameObject; }
		}
		protected override string NameBaseMulti
		{
			get { return GeneralRes.UndoRedo_PasteGameObjectMulti; }
		}
		public IEnumerable<GameObject> Result
		{
			get { return this.resultObj; }
		}

		public PasteGameObjectAction(IEnumerable<GameObject> obj, 
			IEnumerable<GameObject> parents) 
			: base(obj)
		{
			if (parents != null)
			{
				this.targetParentObjects = parents
					.Where(tpo => tpo != null && !tpo.Disposed)
					.ToArray();
			}

			// Null parent is equivalent to no-parent but 
			// allows us to simplify code in Do method
			if (this.targetParentObjects == null
				|| this.targetParentObjects.Length == 0)
			{
				this.targetParentObjects = new GameObject[] { null };
			}
		}

		public override void Do()
		{
			int numDuplicates = this.targetParentObjects.Length;

			// Create the clones. One set of clones for every parent.
			// this.resultObj contains the objects going to the first parent
			// followed by those going to the second parent and so on.
			if (this.resultObj == null)
			{
				this.resultObj = new GameObject[this.targetObj.Length * numDuplicates];
				for (int i = 0; i < numDuplicates * this.targetObj.Length; i++)
				{
					this.resultObj[i] = this.targetObj[i % this.targetObj.Length].DeepClone();
				}
			}
			else
			{
				for (int i = 0; i < numDuplicates * this.targetObj.Length; i++)
					this.targetObj[i % this.targetObj.Length].DeepCopyTo(this.resultObj[i]);
			}

			// Setup the clones
			for (int i = 0; i < numDuplicates * this.targetObj.Length; i++)
			{
				GameObject original = this.targetObj[i % this.targetObj.Length];
				GameObject clone = this.resultObj[i];
				clone.Parent = this.targetParentObjects[i / this.targetObj.Length];

				// Make sure the absolute transform is copied
				if (clone.Transform != null && original.Transform != null)
					clone.Transform.SetTransform(original.Transform);

				// Prevent physics from getting crazy.
				if (clone.Transform != null && clone.GetComponent<Components.Physics.RigidBody>() != null)
					clone.Transform.Pos += Vector3.UnitX * 0.001f;
			}

			Scene.Current.AddObjects(this.resultObj);
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}
		public override void Undo()
		{
			if (this.resultObj == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");
			foreach (GameObject clone in this.resultObj)
			{
				clone.Dispose();
			}
			Scene.Current.RemoveObjects(this.resultObj);
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}
	}
}
