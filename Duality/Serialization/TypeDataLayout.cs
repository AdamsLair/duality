using System.IO;

namespace Duality.Serialization
{
	/// <summary>
	/// This class provides information about the data layout when de/serializing an object.
	/// </summary>
	/// <seealso cref="Duality.Serialization.BinaryFormatter"/>
	public class TypeDataLayout
	{
		/// <summary>
		/// Holds information about a single field.
		/// </summary>
		/// <see cref="System.Reflection.FieldInfo"/>
		public struct FieldDataInfo
		{
			/// <summary>
			/// The fields name
			/// </summary>
			public	string	name;
			/// <summary>
			/// A string referring to the fields type.
			/// </summary>
			public	string	typeString;

			public FieldDataInfo(string name, string typeString)
			{
				this.name = name;
				this.typeString = typeString;
			}

			public override string ToString()
			{
				return string.Format("{0} {1}", this.typeString, this.name);
			}
		}

		private	FieldDataInfo[]	fields;

		/// <summary>
		/// [GET / SET] An array of all the necessary field information, typically one <see cref="FieldDataInfo"/> 
		/// entry per <see cref="System.Reflection.FieldInfo">field</see>.
		/// </summary>
		public FieldDataInfo[] Fields
		{
			get { return this.fields; }
			set { this.fields = value; }
		}

		/// <summary>
		/// Initializes a TypeDataLayout from the specified <see cref="System.IO.BinaryReader"/>.
		/// </summary>
		/// <param name="r">The BinaryReader from which the type information is read.</param>
		public TypeDataLayout(BinaryReader r)
		{
			int count = r.ReadInt32();
			this.fields = new FieldDataInfo[count];

			for (int i = 0; i < count; i++)
			{
				this.fields[i].name = r.ReadString();
				this.fields[i].typeString = r.ReadString();
			}
		}
		/// <summary>
		/// Initializes a TypeDataLayout by cloning an existing TypeDataLayout.
		/// </summary>
		/// <param name="t">The source layout</param>
		public TypeDataLayout(TypeDataLayout t)
		{
			this.fields = t.fields != null ? t.fields.Clone() as FieldDataInfo[] : null;
		}
		/// <summary>
		/// Initializes a TypeDataLayout by extracting necessary information from the specified <see cref="Duality.Serialization.SerializeType"/>.
		/// </summary>
		/// <param name="t">The source SerializeType.</param>
		public TypeDataLayout(SerializeType t)
		{
			this.fields = new FieldDataInfo[t.Fields.Length];
			for (int i = 0; i < t.Fields.Length; i++)
			{
				this.fields[i].name = t.Fields[i].Name;
				this.fields[i].typeString = t.Fields[i].FieldType.GetTypeId();
			}
		}

		/// <summary>
		/// Writes the TypeDataLayout to the specified <see cref="System.IO.BinaryWriter"/>.
		/// </summary>
		/// <param name="w">The BinaryWriter to store the type information.</param>
		public void Write(BinaryWriter w)
		{
			w.Write(this.fields.Length);
			for (int i = 0; i < this.fields.Length; i++)				
			{
				w.Write(this.fields[i].name);
				w.Write(this.fields[i].typeString);
			}
		}
	}
}
