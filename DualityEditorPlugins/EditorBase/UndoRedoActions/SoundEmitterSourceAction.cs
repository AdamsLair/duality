using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Components;

using Duality.Editor;

namespace Duality.Editor.Plugins.Base.UndoRedoActions
{
	public abstract class SoundEmitterSourceAction : UndoRedoAction
	{
		protected	SoundEmitter.Source[]	targetObj	= null;
		
		protected abstract string NameBase { get; }
		protected abstract string NameBaseMulti { get; }
		public override string Name
		{
			get { return this.targetObj.Length == 1 ? 
				string.Format(this.NameBase, this.targetObj[0].Sound.Name) :
				string.Format(this.NameBaseMulti, this.targetObj.Length); }
		}
		public override bool IsVoid
		{
			get { return this.targetObj == null || this.targetObj.Length == 0; }
		}

		public SoundEmitterSourceAction(IEnumerable<SoundEmitter.Source> obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");
			this.targetObj = obj.NotNull().ToArray();
		}
	}
}
