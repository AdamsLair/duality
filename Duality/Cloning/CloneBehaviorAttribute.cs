using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Cloning
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Field)]
	public class CloneBehaviorAttribute : Attribute
	{
		private Type		targetType;
		private CloneMode	mode;
		private	CloneFlags	flags;

		public Type TargetType
		{
			get { return this.targetType; }
		}
		public CloneMode Mode
		{
			get { return this.mode; }
		}
		public CloneFlags Flags
		{
			get { return this.flags; }
		}

		public CloneBehaviorAttribute(CloneFlags flags) : this(null, flags) {}
		public CloneBehaviorAttribute(CloneMode mode) : this(null, mode) {}
		public CloneBehaviorAttribute(CloneMode mode, CloneFlags flags) : this(null, mode, flags) {}
		public CloneBehaviorAttribute(Type targetType, CloneFlags flags) : this(targetType, CloneMode.ChildObject, flags) {}
		public CloneBehaviorAttribute(Type targetType, CloneMode mode) : this(targetType, mode, CloneFlags.None) {}
		public CloneBehaviorAttribute(Type targetType, CloneMode mode, CloneFlags flags)
		{
			this.targetType = targetType;
			this.mode = mode;
			this.flags = flags;
		}
	}
}
