using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Duality.Cloning;
using Duality.Drawing;

namespace Duality.Editor
{
	/// <summary>
	/// Indicates whether an object is stored inside a <see cref="DataObject"/> as 
	/// a reference or serialized value.
	/// </summary>
	public enum DataObjectStorage
	{
		/// <summary>
		/// Use this type when storing an object reference in a DataObject.
		/// The reference will be maintained even after serialization.
		/// Note that storing a .Net value type (ex. Vector3) with this data type
		/// will still lead to a copy of the data. Data stored with this type
		/// can be automatically converted to value typed data (uses deep cloning).
		/// </summary>
		Reference,
		/// <summary>
		/// Use this type when storing an object in a DataObject that is
		/// not meant to be a reference to an existing object. This type
		/// of data cannot be automatically converted to reference typed data.
		/// </summary>
		Value
	}
}
