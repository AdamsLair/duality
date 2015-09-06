using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Duality;

namespace Duality.Editor
{
	public abstract class DataConverter
	{
		public const int PriorityNone			= 0;
		public const int PriorityGeneral		= 20;
		public const int PrioritySpecialized	= 50;
		public const int PriorityOverride		= 100;

		public abstract Type TargetType { get; }
		public virtual int Priority
		{
			get { return PriorityGeneral; }
		}

		public abstract bool CanConvertFrom(ConvertOperation convert);
		public abstract bool Convert(ConvertOperation convert);
	}
}
