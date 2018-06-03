using System.Windows.Forms;

namespace Duality.Editor
{
	/// <summary>
	/// Indicates whether an object is stored inside a <see cref="DataObject"/> as 
	/// a reference or serialized value.
	/// </summary>
	public enum DataObjectStorage
	{
		/// <summary>
		/// Indicates that an object is stored by reference inside a <see cref="DataObject"/>,
		/// so it will not be cloned or copied when moved to the <see cref="Clipboard"/>.
		/// 
		/// Note that storing a value type (ex. <see cref="Vector3"/>) by reference
		/// will still lead to a "copy" of the data due to boxing and unboxing operations.
		/// 
		/// Objects stored with this storage type can still be retrieved as <see cref="Value"/>
		/// items, as a deep clone operation is done internally when inserting references into
		/// a <see cref="DataObject"/>. The cloning is done so the <see cref="Value"/> result
		/// will be an otherwise unused instance.
		/// </summary>
		Reference,
		/// <summary>
		/// Indicates that an object is stored by value inside a <see cref="DataObject"/>,
		/// so any retrieved instance will essentially be a clone of the original, but
		/// not the original itself.
		/// </summary>
		Value
	}
}
