using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Graphics.OpenGL;
using OpenTK;

using Duality.EditorHints;
using Duality.VertexFormat;
using Duality.ColorFormat;
using Duality.Resources;
using Duality.Profiling;

namespace Duality
{
	/// <summary>
	/// Defines a general interface for drawing devices. Its main duty is to accept and collect parameterized vertex data.
	/// </summary>
	public interface IDrawDevice
	{
		/// <summary>
		/// [GET] Reference coordinate for rendering i.e. the position of the drawing device's Camera.
		/// </summary>
		Vector3 RefCoord { get; }
		/// <summary>
		/// [GET] Reference angle for rendering i.e. the angle of the drawing device's Camera.
		/// </summary>
		float RefAngle { get; }
		/// <summary>
		/// [GET] Reference distance for calculating the perspective effect. An object this far away from
		/// the Camera will appear in its original size.
		/// </summary>
		float FocusDist { get; }
		/// <summary>
		/// [GET] A bitmask flagging all visibility groups that are considered visible to this drawing device.
		/// </summary>
		VisibilityFlag VisibilityMask { get; }
		/// <summary>
		/// [GET] The lowest Z value that can be displayed by the device.
		/// </summary>
		float NearZ { get; }
		/// <summary>
		/// [GET] The highest Z value that can be displayed by the device.
		/// </summary>
		float FarZ { get; }
		/// <summary>
		/// [GET] Returns whether the drawing device allows writing to the depth buffer
		/// </summary>
		bool DepthWrite { get; }
		/// <summary>
		/// [GET] The size of the surface this drawing device operates on.
		/// </summary>
		Vector2 TargetSize { get; }
		

		
		/// <summary>
		/// Returns the scale factor of objects that are located at the specified (world space) z-Coordinate.
		/// </summary>
		/// <param name="z"></param>
		/// <returns></returns>
		float GetScaleAtZ(float z);
		/// <summary>
		/// Transforms screen space coordinates to world space coordinates. The screen positions Z coordinate is
		/// interpreted as the target world Z coordinate.
		/// </summary>
		/// <param name="screenPos"></param>
		/// <returns></returns>
		Vector3 GetSpaceCoord(Vector3 screenPos);
		/// <summary>
		/// Transforms screen space coordinates to world space coordinates.
		/// </summary>
		/// <param name="screenPos"></param>
		/// <returns></returns>
		Vector3 GetSpaceCoord(Vector2 screenPos);
		/// <summary>
		/// Transforms world space coordinates to screen space coordinates.
		/// </summary>
		/// <param name="spacePos"></param>
		/// <returns></returns>
		Vector3 GetScreenCoord(Vector3 spacePos);
		/// <summary>
		/// Transforms world space coordinates to screen space coordinates.
		/// </summary>
		/// <param name="spacePos"></param>
		/// <returns></returns>
		Vector3 GetScreenCoord(Vector2 spacePos);

		/// <summary>
		/// Processes the specified world space position and scale values and transforms them to the IDrawDevices view space.
		/// This usually also applies a perspective effect, if applicable.
		/// </summary>
		/// <param name="pos">The position to process.</param>
		/// <param name="scale">The scale factor to process.</param>
		void PreprocessCoords(ref Vector3 pos, ref float scale);
		/// <summary>
		/// Returns whether the specified world-space position is visible in the drawing devices view space.
		/// </summary>
		/// <param name="c">The position to test.</param>
		/// <param name="boundRad">The visual bounding radius to assume for the specified position.</param>
		/// <returns>True, if the position or a portion of its bounding circle is visible, false if not.</returns>
		bool IsCoordInView(Vector3 c, float boundRad = 1.0f);

		/// <summary>
		/// Adds a parameterized set of vertices to the drawing devices rendering schedule.
		/// </summary>
		/// <typeparam name="T">The type of vertex data to add.</typeparam>
		/// <param name="material">The <see cref="Duality.Resources.Material"/> to use for rendering the vertices.</param>
		/// <param name="vertexMode">The vertices drawing mode.</param>
		/// <param name="vertices">The vertex data to add.</param>
		void AddVertices<T>(ContentRef<Material> material, VertexMode vertexMode, params T[] vertices) where T : struct, IVertexData;
		/// <summary>
		/// Adds a parameterized set of vertices to the drawing devices rendering schedule.
		/// </summary>
		/// <typeparam name="T">The type of vertex data to add.</typeparam>
		/// <param name="material">The <see cref="Duality.Resources.BatchInfo"/> to use for rendering the vertices.</param>
		/// <param name="vertexMode">The vertices drawing mode.</param>
		/// <param name="vertices">The vertex data to add.</param>
		void AddVertices<T>(BatchInfo material, VertexMode vertexMode, params T[] vertices) where T : struct, IVertexData;
		/// <summary>
		/// Adds a parameterized set of vertices to the drawing devices rendering schedule.
		/// </summary>
		/// <typeparam name="T">The type of vertex data to add.</typeparam>
		/// <param name="material">The <see cref="Duality.Resources.Material"/> to use for rendering the vertices.</param>
		/// <param name="vertexMode">The vertices drawing mode.</param>
		/// <param name="vertexBuffer">A vertex data buffer that stores the vertices to add.</param>
		/// <param name="vertexCount">The number of vertices to add, from the beginning of the buffer.</param>
		void AddVertices<T>(ContentRef<Material> material, VertexMode vertexMode, T[] vertexBuffer, int vertexCount) where T : struct, IVertexData;
		/// <summary>
		/// Adds a parameterized set of vertices to the drawing devices rendering schedule.
		/// </summary>
		/// <typeparam name="T">The type of vertex data to add.</typeparam>
		/// <param name="material">The <see cref="Duality.Resources.BatchInfo"/> to use for rendering the vertices.</param>
		/// <param name="vertexMode">The vertices drawing mode.</param>
		/// <param name="vertexBuffer">A vertex data buffer that stores the vertices to add.</param>
		/// <param name="vertexCount">The number of vertices to add, from the beginning of the buffer.</param>
		void AddVertices<T>(BatchInfo material, VertexMode vertexMode, T[] vertexBuffer, int vertexCount) where T : struct, IVertexData;
	}
}
