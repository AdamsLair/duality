using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using NUnit.Framework;

using Duality.IO;
using Duality.Backend;

namespace Duality.Editor.Tests.PluginManager
{
	public class MockAssembly : Assembly
	{
		private bool invalid = false;
		private string location = null;
		private string fullName = null;
		private List<Type> privateTypes = new List<Type>();
		private List<Type> exportedTypes = new List<Type>();

		public override string FullName
		{
			get { return this.fullName; }
		}
		public override string CodeBase
		{
			get { return this.location; }
		}
		public override string Location
		{
			get { return this.location; }
		}
		public override IEnumerable<Type> ExportedTypes
		{
			get { this.ThrowIfInvalid(); return this.exportedTypes; }
		}
		public override IEnumerable<TypeInfo> DefinedTypes
		{
			get { this.ThrowIfInvalid(); return this.exportedTypes.Concat(this.privateTypes).Select(type => type.GetTypeInfo()); }
		}

		public MockAssembly(string location, params Type[] exportedTypes)
		{
			this.fullName = PathOp.GetFileNameWithoutExtension(location);
			this.location = location;
			this.exportedTypes.AddRange(exportedTypes);
		}
		
		public void AddPrivateType(Type type)
		{
			this.privateTypes.Add(type);
		}
		public void AddExportedType(Type type)
		{
			this.exportedTypes.Add(type);
		}

		public override Type[] GetExportedTypes()
		{
			this.ThrowIfInvalid();
			return this.exportedTypes
				.ToArray();
		}
		public override Type[] GetTypes()
		{
			this.ThrowIfInvalid();
			return this.exportedTypes
				.Concat(this.privateTypes)
				.ToArray();
		}

		private void ThrowIfInvalid()
		{
			if (this.invalid)
				throw new TypeLoadException("This is a mocked invalid Assembly");
		}

		public static MockAssembly CreateInvalid(string location)
		{
			MockAssembly assembly = new MockAssembly(location);
			assembly.invalid = true;
			return assembly;
		}
	}
}
