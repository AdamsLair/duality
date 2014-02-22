using OpenTK;

namespace Duality.Drawing
{
	/// <summary>
	/// A general interface for different types of vertex data.
	/// </summary>
	public interface IVertexData
	{
		#region Static Members
		/// <summary>
		/// [GET] An integer id representing this kind of vertex data. Usually equals the respective VertexDataFormat constant in <see="Duality.Resources.DrawTechnique" />.
		/// This member is static by design.
		/// </summary>
		int TypeIndex { get; }

		/// <summary>
		/// Sets up the currently bound OpenGL VertexBufferObject and injects actual vertex data.
		/// </summary>
		/// <param name="mat">
		/// The <see cref="Duality.Resources.Material"/> that is currently active. Usually only needed
		/// for custom vertex attributes in order to access <see cref="Duality.Resources.ShaderVarInfo">shader variables</see>.
		/// </param>
		void SetupVBO(Resources.BatchInfo mat);
		/// <summary>
		/// Uploads vertex data to the currently bound OpenGL VertexBufferObject.
		/// </summary>
		/// <typeparam name="T">The type of input vertex data to use.</typeparam>
		/// <param name="vertexData">The vertex data to be uploaded into the VBO.</param>
		void UploadToVBO<T>(T[] vertexData, int vertexCount) where T : struct, IVertexData;
		/// <summary>
		/// Resets the VBO configuration.
		/// </summary>
		/// <param name="mat">The <see cref="Duality.Resources.Material"/> that was active when setting it up.</param>
		void FinishVBO(Resources.BatchInfo mat);
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
