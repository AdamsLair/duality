namespace Duality.Drawing
{
	/// <summary>
	/// A general interface for different types of vertex data.
	/// </summary>
	public interface IVertexData
	{
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
