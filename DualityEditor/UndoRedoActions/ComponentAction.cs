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
	public abstract class ComponentAction : UndoRedoAction
	{
		protected	List<Component>	targetObj	= null;
		
		protected abstract string NameBase { get; }
		protected abstract string NameBaseMulti { get; }
		public override string Name
		{
			get { return this.targetObj.Count == 1 ? 
				string.Format(this.NameBase, this.targetObj[0].GetType().Name) :
				string.Format(this.NameBaseMulti, this.targetObj.Count); }
		}
		public override bool IsVoid
		{
			get { return this.targetObj == null || this.targetObj.Count == 0; }
		}

		public ComponentAction(IEnumerable<Component> obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");
			this.targetObj = obj.Where(o => o != null && !o.Disposed).ToList();
		}

		[System.Diagnostics.Conditional("DEBUG")]
		protected static void DebugCheckParent(Component cmp, GameObject parent)
		{
			if (cmp.GameObj != parent)
			{
				// Suspecting bug here. Hooking Debugger...
			}
		}
	}
}
