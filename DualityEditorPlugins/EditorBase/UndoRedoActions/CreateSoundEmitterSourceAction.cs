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
	public class CreateSoundEmitterSourceAction : SoundEmitterSourceAction
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
		public IEnumerable<SoundEmitter.Source> Result
		{
			get { return this.targetObj; }
		}

		public CreateSoundEmitterSourceAction(IEnumerable<SoundEmitter> parent, IEnumerable<SoundEmitter.Source> obj) : base(obj)
		{
			this.targetParentObj = parent.NotNull().ToArray();
		}

		public override void Do()
		{
			foreach (SoundEmitter emitter in this.targetParentObj)
			{
				foreach (SoundEmitter.Source source in this.targetObj)
				{
					emitter.Sources.Add(source);
				}
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetParentObj), ReflectionInfo.Property_SoundEmitter_Sources);
		}
		public override void Undo()
		{
			foreach (SoundEmitter emitter in this.targetParentObj)
			{
				foreach (SoundEmitter.Source source in this.targetObj)
				{
					if (source.Instance != null) source.Instance.Stop();
					emitter.Sources.Remove(source);
				}
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetParentObj), ReflectionInfo.Property_SoundEmitter_Sources);
		}
	}
}
