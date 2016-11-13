using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

using Duality;
using Duality.Resources;

namespace Duality.Editor
{
	public interface IPreviewQuery
	{
		object OriginalSource { get; }
		object Source { get; }
		object Result { get; }

		bool SourceFits(Type sourceType);
		bool TransformSource(Type sourceType);
	}
}
