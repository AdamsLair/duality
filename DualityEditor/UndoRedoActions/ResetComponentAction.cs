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
	public class ResetComponentAction : ComponentAction
	{
		private Component[]		backupObj	= null;
		private PrefabLink[]	backupLink	= null;
		
		protected override string NameBase
		{
			get { return GeneralRes.UndoRedo_ResetComponent; }
		}
		protected override string NameBaseMulti
		{
			get { return GeneralRes.UndoRedo_ResetComponentMulti; }
		}

		public ResetComponentAction(IEnumerable<Component> obj) : base(obj) {}

		public override void Do()
		{
			if (this.backupObj == null)
			{
				this.backupObj = new Component[this.targetObj.Count];
				this.backupLink = new PrefabLink[this.targetObj.Count];
				for (int i = 0; i < this.backupObj.Length; i++)
				{
					PrefabLink link = this.targetObj[i].GameObj.AffectedByPrefabLink;
					this.backupObj[i] = CloneProvider.DeepClone(this.targetObj[i], BackupCloneContext);
					this.backupLink[i] = CloneProvider.DeepClone(link, BackupCloneContext);
				}
			}
			
			List<PrefabLink> affectedLinks = new List<PrefabLink>();
			foreach (Component component in this.targetObj)
			{
				Type cmpType = component.GetType();
				PrefabLink link = component.GameObj.AffectedByPrefabLink;
				if (link != null)
				{
					if (link.Prefab.IsAvailable) link.Prefab.Res.CopyTo(link.Obj.IndexPathOfChild(component.GameObj), component);
					link.ClearChanges(component.GameObj, cmpType, null);
					affectedLinks.Add(link);
				}
				else
				{
					Component resetBase = (cmpType.CreateInstanceOf() ?? cmpType.CreateInstanceOf(true)) as Component;
					resetBase.CopyTo(component);
				}
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetObj));
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
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetObj));
		}
	}
}
