using System.Xml.Linq;

namespace Duality.Serialization.Surrogates
{
	/// <summary>
	/// Ensures that serializing and deserializing a <see cref="XElement"/> works properly
	/// </summary>
	public class XElementSurrogate : SerializeSurrogate<XElement>
	{
		public override void WriteData(IDataWriter writer)
		{
			writer.WriteValue("value", this.RealObject.ToString()); 
		}

		public override void ReadData(IDataReader reader)
		{
			reader.ReadValue("value", out string xml);

			XElement xElement = XElement.Parse(xml);

			this.RealObject.Name = xElement.Name;
			foreach (XAttribute xAttribute in xElement.Attributes())
			{
				this.RealObject.SetAttributeValue(xAttribute.Name, xAttribute.Value);
			}
			this.RealObject.Add(xElement.Elements());
		}
	}
}
