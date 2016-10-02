using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Duality.Serialization;

namespace Duality.Tests.Serialization
{
	public class MemberInfoTestObject
	{
		public int Field;
		public event EventHandler Event;
		public int Property { get { return 0; } }
		public int Method(string param) { return 0; }
		public MemberInfoTestObject() { }
	}
}
