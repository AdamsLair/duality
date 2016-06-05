using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.Drawing;

using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Plugins.CamView.Properties;
using Duality.Editor.Plugins.CamView.UndoRedoActions;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	public abstract class ObjectEditorSelObj : IEquatable<ObjectEditorSelObj>
	{
		public abstract object ActualObject { get; }
		public abstract bool HasTransform { get; }
		public abstract float BoundRadius { get; }
		public abstract Vector3 Pos { get; set; }
		public virtual Vector3 Scale
		{
			get { return Vector3.One; }
			set {}
		}
		public virtual float Angle
		{
			get { return 0.0f; }
			set {}
		}
		public virtual bool ShowBoundRadius
		{
			get { return true; }
		}
		public virtual bool ShowPos
		{
			get { return true; }
		}
		public virtual bool ShowAngle
		{
			get { return false; }
		}
		public virtual string DisplayObjectName
		{
			get { return this.ActualObject != null ? this.ActualObject.ToString() : "null"; }
		}
		public bool IsInvalid
		{
			get { return this.ActualObject == null; }
		}

		public virtual bool IsActionAvailable(ObjectEditorAction action)
		{
			if (action == ObjectEditorAction.Move) return true;
			return false;
		}
		public virtual string UpdateActionText(ObjectEditorAction action, bool performing)
		{
			return null;
		}
			
		public override bool Equals(object obj)
		{
			if (obj is ObjectEditorSelObj)
				return this == (ObjectEditorSelObj)obj;
			else
				return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return this.ActualObject.GetHashCode();
		}
		public bool Equals(ObjectEditorSelObj other)
		{
			return this == other;
		}

		public static bool operator ==(ObjectEditorSelObj first, ObjectEditorSelObj second)
		{
			if (object.ReferenceEquals(first, null))
			{
				if (object.ReferenceEquals(second, null)) return true;
				else return false;
			}
			else if (object.ReferenceEquals(second, null))
				return false;

			return first.ActualObject == second.ActualObject;
		}
		public static bool operator !=(ObjectEditorSelObj first, ObjectEditorSelObj second)
		{
			return !(first == second);
		}
	}
}
