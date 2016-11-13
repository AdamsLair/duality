using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Cloning
{
	/// <summary>
	/// Describes the context of a cloning operation
	/// </summary>
	public class CloneProviderContext
	{
		/// <summary>
		/// A standard cloning operation.
		/// </summary>
		public static readonly CloneProviderContext Default = new CloneProviderContext();
		protected bool preserveIdentity;

		/// <summary>
		/// [GET] Should the operation preserve each objects identity? If false, specific identity-preserving data
		/// field such as Guid or Id fields will be copied as well. This might result in duplicate IDs.
		/// </summary>
		public bool PreserveIdentity
		{
			get { return this.preserveIdentity; }
		}

		public CloneProviderContext(bool preserveId = true)
		{
			this.preserveIdentity = preserveId;
		}
	}
}
