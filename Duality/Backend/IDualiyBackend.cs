using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend
{
	public interface IDualityBackend
	{
		string Id { get; }
		string Name { get; }
		int Priority { get; }

		bool CheckAvailable();
		void Init();
		void Shutdown();
	}
}
