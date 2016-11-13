using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

namespace Duality.Editor
{
	public class PrefabAppliedEventArgs : ObjectPropertyChangedEventArgs
	{
		public PrefabAppliedEventArgs(ObjectSelection obj) : base(obj.HierarchyExpand(), new PropertyInfo[0]) {}
	}
}
