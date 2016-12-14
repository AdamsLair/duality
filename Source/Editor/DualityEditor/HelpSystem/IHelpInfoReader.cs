using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace Duality.Editor
{
	public interface IHelpInfoReader
	{
		string Id { get; }
		string Topic { get; }
		string Description { get; }
		HelpAction PerformHelpAction { get; }
	}
}
