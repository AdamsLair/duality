using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Duality;
using Duality.Cloning;
using Duality.Resources;

using Duality.Editor.Controls;
using Duality.Editor.Properties;

using AdamsLair.WinForms.PropertyEditing;

namespace Duality.Editor.UndoRedoActions
{
	public class EditResourceAssetDataAction : UndoRedoAction
	{
		protected PropertyGrid parent	  = null;
		protected bool		 firstDo	 = true;
		protected string	   targetKey   = null;
		protected Resource[]   targetObj   = null;
		protected object[]	 targetValue = null;
		protected object[]	 backupValue = null;
		
		public override string Name
		{
			get { return string.Format(GeneralRes.UndoRedo_EditProperty, this.targetKey); }
		}
		public override bool IsVoid
		{
			get { return this.targetObj == null || this.targetObj.Length == 0; }
		}

		public EditResourceAssetDataAction(PropertyGrid parent, string dataKey, IEnumerable<Resource> target, IEnumerable<object> value)
		{
			if (dataKey == null) throw new ArgumentNullException("dataKey");
			if (target == null) throw new ArgumentNullException("target");
			this.targetKey = dataKey;
			this.targetObj = target.Where(o => o != null).ToArray();
			this.targetValue = (value != null) ? value.ToArray() : null;
			this.parent = parent;
		}

		public override void Do()
		{
			if (this.targetValue != null && this.targetValue.Length > 0)
			{
				if (this.backupValue == null)
				{
					this.backupValue = new object[this.targetObj.Length];
					for (int i = 0; i < this.targetObj.Length; i++)
					{
						Resource res = this.targetObj[i];
						this.TryGetValue(res, this.targetKey, out this.backupValue[i]);
					}
				}

				for (int i = 0; i < this.targetObj.Length; i++)
				{
					Resource res = this.targetObj[i];
					object value = this.targetValue[Math.Min(i, this.targetValue.Length - 1)];
					this.TrySetValue(res, this.targetKey, value);
				}
			}

			DualityEditorApp.NotifyObjPropChanged(this.parent, 
				new ObjectSelection(this.targetObj), 
				ReflectionInfo.Property_Resource_AssetInfo);

			if (!this.firstDo && this.parent != null)
				this.parent.UpdateFromObjects();
			this.firstDo = false;
		}
		public override void Undo()
		{
			if (this.targetValue != null)
			{
				if (this.backupValue == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");

				for (int i = 0; i < this.targetObj.Length; i++)
				{
					this.TrySetValue(
						this.targetObj[i], 
						this.targetKey,
						this.backupValue[i]);
				}
			}
			
			DualityEditorApp.NotifyObjPropChanged(this.parent, 
				new ObjectSelection(this.targetObj), 
				ReflectionInfo.Property_Resource_AssetInfo);

			if (this.parent != null)
				this.parent.UpdateFromObjects();
		}
		public override bool CanAppend(UndoRedoAction action)
		{
			EditResourceAssetDataAction castAction = action as EditResourceAssetDataAction;

			if (castAction == null) return false;
			if (castAction.targetKey != this.targetKey) return false;
			if (!castAction.targetObj.SequenceEqual(this.targetObj)) return false;

			return true;
		}
		public override void Append(UndoRedoAction action, bool performAction)
		{
			base.Append(action, performAction);
			EditResourceAssetDataAction castAction = action as EditResourceAssetDataAction;

			if (performAction)
			{
				castAction.backupValue = this.backupValue;
				castAction.Do();
			}
			this.targetValue = castAction.targetValue ?? this.targetValue;
		}
		
		private bool TryGetValue(Resource res, string key, out object value)
		{
			value = null;

			if (res == null) return false;
			if (res.AssetInfo == null) return false;
			if (res.AssetInfo.CustomData == null) return false;

			Dictionary<string,object> data = res.AssetInfo.CustomData;
			return data.TryGetValue(this.targetKey, out value);
		}
		private bool TrySetValue(Resource res, string key, object value)
		{
			if (res == null) return false;
			if (res.AssetInfo == null) return false;
			if (res.AssetInfo.CustomData == null) return false;

			Dictionary<string,object> data = res.AssetInfo.CustomData;
			if (!data.ContainsKey(this.targetKey)) return false;

			data[this.targetKey] = value;
			return true;
		}
	}
}
