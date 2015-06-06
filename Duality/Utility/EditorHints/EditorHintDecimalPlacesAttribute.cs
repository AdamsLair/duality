using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace Duality.Editor
{
	/// <summary>
	/// Provides information about a numerical members decimal accuracy
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class EditorHintDecimalPlacesAttribute : EditorHintAttribute
	{
		private	int places;
		/// <summary>
		/// [GET] The preferred number of displayed decimal places
		/// </summary>
		public int Places
		{
			get { return this.places; }
		}
		public EditorHintDecimalPlacesAttribute(int places)
		{
			this.places = places;
		}
	}
}
