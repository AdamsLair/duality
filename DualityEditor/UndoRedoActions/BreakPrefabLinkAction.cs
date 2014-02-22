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
	public class BreakPrefabLinkAction : GameObjectAction
	{
		private PrefabLink[]	backupLink	= null;
		
		protected override string NameBase
		{
			get { return GeneralRes.UndoRedo_BreakPrefabLink; }
		}
		protected override string NameBaseMulti
		{
			get { return GeneralRes.UndoRedo_BreakPrefabLinkMulti; }
		}

		public BreakPrefabLinkAction(IEnumerable<GameObject> obj) : base(obj.Where(
			o => o != null && 
			o.PrefabLink != null)) {}

		public override void Do()
		{
			if (this.backupLink == null)
			{
				this.backupLink = new PrefabLink[this.targetObj.Length];
				for (int i = 0; i < this.backupLink.Length; i++)
				{
					PrefabLink link = this.targetObj[i].AffectedByPrefabLink;
					this.backupLink[i] = CloneProvider.DeepClone(link, BackupCloneContext);
				}
			}
			
			// Destroy all PrefabLinks
			foreach (GameObject o in this.targetObj)
				o.BreakPrefabLink();

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetObj), ReflectionInfo.Property_GameObject_PrefabLink);
		}
		public override void Undo()
		{
			if (this.backupLink == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");

			for (int i = this.backupLink.Length - 1; i >= 0; i--)
			{
				if (this.backupLink[i] == null) continue;
				if (this.backupLink[i].Obj == null) continue;

				this.backupLink[i].Obj.LinkToPrefab(this.backupLink[i].Prefab);
				PrefabLink link = this.backupLink[i].Obj.PrefabLink;
				CloneProvider.DeepCopyTo(this.backupLink[i], link, BackupCloneContext);
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetObj), ReflectionInfo.Property_GameObject_PrefabLink);
		}
	}
}
