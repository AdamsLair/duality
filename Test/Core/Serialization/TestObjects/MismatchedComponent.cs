using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Duality.Serialization;

namespace Duality.Tests.Serialization
{
	/// <summary>
	/// This <see cref="Component"/> is used for serializing test data, then modified to
	/// no longer be a Component in order to test the deserialization of mismatched Component types.
	/// </summary>
#if false
	public class MismatchedComponent : Component { }
#else
	public class MismatchedComponent { }
#endif
}
