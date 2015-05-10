namespace Duality.Drawing
{
	/// <summary>
	/// A general interface for different types of vertex data.
	/// </summary>
	public interface IVertexData
	{
		#region Static Members
		/// <summary>
		/// [GET] The <see cref="VertexFormatDefinition"/> that specifies size, layout and roles of the vertex fields.
		/// This member is static by design.
		/// </summary>
		VertexFormatDefinition Format { get; }
		#endregion

		/// <summary>
		/// [GET] The vertices position.
		/// </summary>
		Vector3 Pos { get; set; }
		/// <summary>
		/// [GET] The vertices color.
		/// </summary>
		ColorRgba Color { get; set; }
	}
}
