using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Components;

using Duality.Editor;
using Duality.Editor.Plugins.Base.Properties;

namespace Duality.Editor.Plugins.Base.UndoRedoActions
{
	public class DeleteSoundEmitterSourceAction : SoundEmitterSourceAction
	{
		private	SoundEmitter[]	targetParentObj	= null;

		protected override string NameBase
		{
			get { return EditorBaseRes.UndoRedo_CreateSoundEmitterSource; }
		}
		protected override string NameBaseMulti
		{
			get { return EditorBaseRes.UndoRedo_CreateSoundEmitterSourceMulti; }
		}

		public DeleteSoundEmitterSourceAction(IEnumerable<SoundEmitter> parent, IEnumerable<SoundEmitter.Source> obj) : base(obj)
		{
			this.targetParentObj = parent.NotNull().ToArray();
		}

		public override void Do()
		{
			for (int i = 0; i < this.targetObj.Length; i++)
			{
				SoundEmitter emitter = this.targetParentObj[i];
				SoundEmitter.Source source = this.targetObj[i];
				if (source == null) continue;
				if (emitter == null) continue;
				emitter.Sources.Remove(source);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetParentObj), ReflectionInfo.Property_SoundEmitter_Sources);
		}
		public override void Undo()
		{
			for (int i = 0; i < this.targetObj.Length; i++)
			{
				SoundEmitter emitter = this.targetParentObj[i];
				SoundEmitter.Source source = this.targetObj[i];
				if (source == null) continue;
				if (emitter == null) continue;
				emitter.Sources.Add(source);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetParentObj), ReflectionInfo.Property_SoundEmitter_Sources);
		}
	}
}
