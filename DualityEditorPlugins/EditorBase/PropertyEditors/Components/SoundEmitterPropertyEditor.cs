using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using AdamsLair.PropertyGrid;

using Duality;
using Duality.Components;
using Duality.Resources;

using DualityEditor;
using DualityEditor.UndoRedoActions;
using DualityEditor.CorePluginInterface;

using EditorBase.UndoRedoActions;

namespace EditorBase.PropertyEditors
{
	public class SoundEmitterPropertyEditor : ComponentPropertyEditor
	{
		private	List<SoundEmitterSourcePropertyEditor>	soundSourceEditors	= new List<SoundEmitterSourcePropertyEditor>();

		public override void ClearContent()
		{
			base.ClearContent();
			this.soundSourceEditors.Clear();
		}
		public override MemberInfo MapEditorToMember(PropertyEditor editor)
		{
			if (editor is SoundEmitterSourcePropertyEditor)
				return ReflectionInfo.Property_SoundEmitter_Sources;
			else
				return base.MapEditorToMember(editor);
		}

		protected override void BeforeAutoCreateEditors()
		{
			base.BeforeAutoCreateEditors();
			this.UpdateSourceEditors(this.GetValue().Cast<SoundEmitter>());
		}
		protected override bool IsAutoCreateMember(MemberInfo info)
		{
			if (ReflectionHelper.MemberInfoEquals(info, ReflectionInfo.Property_SoundEmitter_Sources)) return false;
			return base.IsAutoCreateMember(info);
		}
		protected override void OnUpdateFromObjects(object[] values)
		{
			base.OnUpdateFromObjects(values);
			this.UpdateSourceEditors(values.Cast<SoundEmitter>());
		}
		
		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			if (e.Effect != DragDropEffects.None) return;

			DataObject dragDropData = e.Data as DataObject;
			if (!this.ReadOnly && dragDropData != null && new ConvertOperation(dragDropData, ConvertOperation.Operation.All).CanPerform<Sound>())
			{
				// Accept drop
				e.Effect = e.AllowedEffect;
			}
		}
		protected override void OnDragDrop(DragEventArgs e)
		{
			// Check children first. Only accept the drop if they don't
			e.Effect = DragDropEffects.None;
			base.OnDragDrop(e);
			if (e.Effect != DragDropEffects.None) return;

			DataObject dragDropData = e.Data as DataObject;
			if (!this.ReadOnly && dragDropData != null)
			{
				ConvertOperation convert = new ConvertOperation(dragDropData, ConvertOperation.Operation.All);
				if (convert.CanPerform<Sound>())
				{
					// Accept drop
					e.Effect = e.AllowedEffect;

					UndoRedoManager.Do(new CreateSoundEmitterSourceAction(
						this.GetValue().Cast<SoundEmitter>(), 
						convert.Perform<Sound>().Select(s => new SoundEmitter.Source(s))));
				}
			}
		}

		protected void UpdateSourceEditors(IEnumerable<SoundEmitter> values)
		{
			int visibleElementCount = values.Where(o => o != null).Min(o => o.Sources.Count);

			// Add missing editors
			for (int i = 0; i < visibleElementCount; i++)
			{
				SoundEmitterSourcePropertyEditor elementEditor;
				if (i < this.soundSourceEditors.Count)
					elementEditor = this.soundSourceEditors[i];
				else
				{
					elementEditor = new SoundEmitterSourcePropertyEditor();
					this.ParentGrid.ConfigureEditor(elementEditor);
					this.soundSourceEditors.Add(elementEditor);
					this.AddPropertyEditor(elementEditor);
				}
				elementEditor.PropertyName = string.Format("Sources[{0}]", i);
				elementEditor.Getter = this.CreateSourceValueGetter(i);
				elementEditor.Setter = this.CreateSourceValueSetter(i);
				elementEditor.ParentEmitter = values;
				elementEditor.PerformGetValue();
			}
			// Remove overflowing editors
			for (int i = this.soundSourceEditors.Count - 1; i >= visibleElementCount; i--)
			{
				this.RemovePropertyEditor(this.soundSourceEditors[i]);
				this.soundSourceEditors.RemoveAt(i);
			}
		}

		protected Func<IEnumerable<object>> CreateSourceValueGetter(int index)
		{
			return () => this.GetValue().Cast<SoundEmitter>().Select(o => (o != null && o.Sources.Count > index) ? o.Sources[index] : null);
		}
		protected Action<IEnumerable<object>> CreateSourceValueSetter(int index)
		{
			return delegate(IEnumerable<object> values)
			{
				IEnumerable<SoundEmitter.Source> valuesCast = values.Cast<SoundEmitter.Source>();
				SoundEmitter[] targetArray = this.GetValue().Cast<SoundEmitter>().ToArray();

				// Explicitly setting the values to null: Remove corresponding source list entry
				if (valuesCast.All(v => v == null))
				{
					UndoRedoManager.Do(new DeleteSoundEmitterSourceAction(
						targetArray, 
						targetArray.Select(e => e.Sources[index])));
				}
			};
		}
	}

	public class SoundEmitterSourcePropertyEditor : MemberwisePropertyEditor
	{
		private	SoundEmitter[] parentEmitter = null;

		public IEnumerable<SoundEmitter> ParentEmitter
		{
			get { return this.parentEmitter; }
			set { this.parentEmitter = value.ToArray(); }
		}

		public SoundEmitterSourcePropertyEditor()
		{
			this.EditedType = typeof(SoundEmitter.Source);
			this.HeaderStyle = AdamsLair.PropertyGrid.Renderer.GroupHeaderStyle.SmoothSunken;
			this.HeaderHeight = 30;
		}

		protected override void OnPropertySet(PropertyInfo property, IEnumerable<object> targets)
		{
			base.OnPropertySet(property, targets);
			
			UndoRedoManager.Do(new EditPropertyAction(this.ParentGrid, ReflectionInfo.Property_SoundEmitter_Sources, this.parentEmitter, null));
		}
		protected override void OnUpdateFromObjects(object[] values)
		{
			base.OnUpdateFromObjects(values);
			IEnumerable<SoundEmitter.Source> sources = values.Cast<SoundEmitter.Source>();

			this.HeaderValueText = null;
			if (sources.NotNull().Any())
				this.HeaderValueText = sources.NotNull().First().Sound.FullName;
			else
				this.HeaderValueText = "null";
		}
	}
}
