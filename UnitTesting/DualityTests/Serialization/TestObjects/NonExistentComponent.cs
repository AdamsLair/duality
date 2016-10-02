using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Duality.Serialization;

namespace Duality.Tests.Serialization
{
	/// <summary>
	/// This <see cref="Component"/> is used for serializing test data, then removed
	/// from source code in order to test the deserialization of non-existent Component types.
	/// </summary>
#if false
	public class NonExistentComponent : Component { }
#endif
}
