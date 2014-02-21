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
	public abstract class GameObjectAction : UndoRedoAction
	{
		protected	GameObject[]	targetObj	= null;
		
		protected abstract string NameBase { get; }
		protected abstract string NameBaseMulti { get; }
		public override string Name
		{
			get { return this.targetObj.Length == 1 ? 
				string.Format(this.NameBase, this.targetObj[0].Name) :
				string.Format(this.NameBaseMulti, this.targetObj.Length); }
		}
		public override bool IsVoid
		{
			get { return this.targetObj == null || this.targetObj.Length == 0; }
		}

		public GameObjectAction(IEnumerable<GameObject> obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");
			this.targetObj = obj.Where(o => o != null && !o.Disposed).ToArray();
		}
	}
}
