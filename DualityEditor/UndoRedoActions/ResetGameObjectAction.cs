using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Resources;

using Duality.Editor.Properties;

namespace Duality.Editor.UndoRedoActions
{
	public class ResetGameObjectAction : GameObjectAction
	{
		private GameObject[]	backupObj	= null;
		private PrefabLink[]	backupLink	= null;
		
		protected override string NameBase
		{
			get { return GeneralRes.UndoRedo_ResetGameObject; }
		}
		protected override string NameBaseMulti
		{
			get { return GeneralRes.UndoRedo_ResetGameObjectMulti; }
		}

		public ResetGameObjectAction(IEnumerable<GameObject> obj) : base(obj.Where(
			o => o != null && 
			o.PrefabLink != null && 
			o.PrefabLink.Prefab.IsAvailable)) {}

		public override void Do()
		{
			if (this.backupObj == null)
			{
				this.backupObj = new GameObject[this.targetObj.Length];
				this.backupLink = new PrefabLink[this.targetObj.Length];
				for (int i = 0; i < this.backupObj.Length; i++)
				{
					PrefabLink link = this.targetObj[i].AffectedByPrefabLink;
					this.backupObj[i] = CloneProvider.DeepClone(this.targetObj[i], BackupCloneContext);
					this.backupLink[i] = CloneProvider.DeepClone(link, BackupCloneContext);
				}
			}
			
			// Clear all changes and re-apply Prefabs
			foreach (GameObject o in this.targetObj)
				o.PrefabLink.ClearChanges();
			Duality.Resources.PrefabLink.ApplyAllLinks(this.targetObj);

			DualityEditorApp.NotifyObjPrefabApplied(this, new ObjectSelection(this.targetObj));
		}
		public override void Undo()
		{
			if (this.backupObj == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");

			List<PrefabLink> affectedLinks = new List<PrefabLink>();
			for (int i = this.backupObj.Length - 1; i >= 0; i--)
			{
				CloneProvider.DeepCopyTo(this.backupObj[i], this.targetObj[i], BackupCloneContext);
				if (this.backupLink[i] != null && 
					this.backupLink[i].Obj != null && 
					this.backupLink[i].Obj.PrefabLink != null)
				{
					PrefabLink link = this.backupLink[i].Obj.PrefabLink;
					CloneProvider.DeepCopyTo(this.backupLink[i], link, BackupCloneContext);
					affectedLinks.Add(link);
				}
			}

			DualityEditorApp.NotifyObjPrefabApplied(this, new ObjectSelection(this.targetObj));
		}
	}
}
