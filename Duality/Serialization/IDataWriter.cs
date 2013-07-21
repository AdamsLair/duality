namespace Duality.Serialization
{
	/// <summary>
	/// Provides writing capabilities for serialization purposes
	/// </summary>
	public interface IDataWriter
	{
		/// <summary>
		/// Writes the specified name and value.
		/// </summary>
		/// <param name="name">
		/// The name to which the written value is mapped. 
		/// May, for example, be the name of a <see cref="System.Reflection.FieldInfo">Field</see>
		/// to which the written value belongs, but there are no naming restrictions, except that one name can't be used twice.
		/// </param>
		/// <param name="value">The value to write.</param>
		void WriteValue(string name, object value);
	}
}
