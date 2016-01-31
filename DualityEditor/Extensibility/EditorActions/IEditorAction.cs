using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Duality.Editor
{
	public interface IEditorAction
	{
		string Name { get; }
		string Description { get; }
		Image Icon { get; }
		Type SubjectType { get; }
		int Priority { get; }

		void Perform(object obj);
		void Perform(IEnumerable<object> obj);
		bool CanPerformOn(IEnumerable<object> obj);
		bool MatchesContext(string context);
	}
}
