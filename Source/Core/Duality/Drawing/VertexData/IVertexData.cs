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
		/// <summary>
		/// [GET] A depth offset that is applied after the vertex has been transformed.
		/// Used for adjusting rendering order of objects without affecting projection.
		/// </summary>
		float DepthOffset { get; set; }
	}
}
