using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality
{
	/// <summary>
	/// This attribute allows the decorated <see cref="Component"/> class to restrict its 
	/// relative position in an execution order that involves multiple <see cref="Component"/>
	/// types. Typical examples are update, initialization or (reversed) shutdown order.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ExecutionOrderAttribute : Attribute
	{
		private Type anchor;
		private ExecutionRelation relation;

		/// <summary>
		/// [GET] The class relative to which this class is placed in execution order.
		/// </summary>
		public Type Anchor
		{
			get { return this.anchor; }
		}
		/// <summary>
		/// [GET] When this class will be executed relative to the one that is referred in <see cref="Anchor"/>.
		/// </summary>
		public ExecutionRelation Relation
		{
			get { return this.relation; }
		}

		public ExecutionOrderAttribute(ExecutionRelation relation, Type anchor)
		{
			this.anchor = anchor;
			this.relation = relation;
		}
	}
}
