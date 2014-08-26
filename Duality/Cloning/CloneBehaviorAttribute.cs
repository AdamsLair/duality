using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Cloning
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Field, AllowMultiple = false)]
	public class CloneBehaviorAttribute : Attribute
	{
		private Type			targetType;
		private CloneBehavior	behavior;

		public Type TargetType
		{
			get { return this.targetType; }
		}
		public CloneBehavior Behavior
		{
			get { return this.behavior; }
		}

		public CloneBehaviorAttribute(CloneBehavior behavior) : this(null, behavior) {}
		public CloneBehaviorAttribute(Type targetType, CloneBehavior behavior)
		{
			this.targetType = targetType;
			this.behavior = behavior;
		}
	}
}
