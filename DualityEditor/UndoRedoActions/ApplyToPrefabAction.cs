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
	public class ApplyToPrefabAction : GameObjectAction
	{
		private	ContentRef<Prefab>[]	targetPrefab	= null;
		private Prefab[]				backupPrefab	= null;
		private PrefabLink[]			backupLink		= null;
		
		protected override string NameBase
		{
			get { return GeneralRes.UndoRedo_ApplyToPrefab; }
		}
		protected override string NameBaseMulti
		{
			get { return GeneralRes.UndoRedo_ApplyToPrefabMulti; }
		}

		public ApplyToPrefabAction(GameObject obj, ContentRef<Prefab> target) : base(new[] { obj })
		{
			this.targetPrefab = new ContentRef<Prefab>[this.targetObj.Length];
			for (int i = 0; i < this.targetPrefab.Length; i++)
				this.targetPrefab[i] = target;
		}
		public ApplyToPrefabAction(IEnumerable<GameObject> obj) : base(obj.Where(
			o => o != null && 
			o.PrefabLink != null && 
			o.PrefabLink.Prefab.IsAvailable))
		{
			this.targetPrefab = this.targetObj.Select(o => o.PrefabLink.Prefab).ToArray();
		}
		public ApplyToPrefabAction(IEnumerable<GameObject> obj, IEnumerable<ContentRef<Prefab>> target) : base(obj)
		{
			this.targetPrefab = target.ToArray();
			Array.Resize(ref this.targetPrefab, this.targetObj.Length);
		}

		public override void Do()
		{
			if (this.backupPrefab == null)
			{
				this.backupPrefab = new Prefab[this.targetObj.Length];
				this.backupLink = new PrefabLink[this.targetObj.Length];
				for (int i = 0; i < this.backupPrefab.Length; i++)
				{
					PrefabLink link = this.targetObj[i].PrefabLink;
					Prefab prefab = this.targetPrefab[i].Res;
					this.backupPrefab[i] = CloneProvider.DeepClone(prefab, BackupCloneContext);
					this.backupLink[i] = CloneProvider.DeepClone(link, BackupCloneContext);
				}
			}
			
			List<Prefab> prefabs = new List<Prefab>();
			List<GameObject> linkedObj = new List<GameObject>();
			for (int i = 0; i < this.targetObj.Length; i++)
			{
				GameObject obj = this.targetObj[i];
				Prefab prefab = this.targetPrefab[i].Res;

				if (prefab == null) continue;

				prefab.Inject(obj);
				if (obj.PrefabLink == null)
				{
					obj.LinkToPrefab(prefab);
					linkedObj.Add(obj);
				}
				prefabs.Add(prefab);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(prefabs));
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(linkedObj), ReflectionInfo.Property_GameObject_PrefabLink);
		}
		public override void Undo()
		{
			if (this.backupPrefab == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");

			List<Prefab> prefabs = new List<Prefab>();
			List<GameObject> linkedObj = new List<GameObject>();
			for (int i = this.backupPrefab.Length - 1; i >= 0; i--)
			{
				GameObject obj = this.targetObj[i];
				Prefab prefab = this.targetPrefab[i].Res;
				PrefabLink link = obj.PrefabLink;

				if (prefab == null) continue;

				CloneProvider.DeepCopyTo(this.backupPrefab[i], prefab, BackupCloneContext);
				if (this.backupLink[i] == null)
				{
					this.targetObj[i].BreakPrefabLink();
					linkedObj.Add(obj);
				}

				prefabs.Add(link.Prefab.Res);
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(prefabs));
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(linkedObj), ReflectionInfo.Property_GameObject_PrefabLink);
		}
	}
}
