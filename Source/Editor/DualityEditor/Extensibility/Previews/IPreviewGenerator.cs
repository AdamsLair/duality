using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

using Duality;
using Duality.Resources;

namespace Duality.Editor
{
	public interface IPreviewGenerator
	{
		int Priority { get; }
		Type ObjectType { get; }

		void Perform(IPreviewQuery settings);
	}
}
