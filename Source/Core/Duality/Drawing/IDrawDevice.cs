﻿using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Resources;
using Duality.Drawing;

namespace Duality.Drawing
{
	/// <summary>
	/// Defines a general interface for drawing devices. Its main duty is to accept and collect parameterized vertex data.
	/// </summary>
	public interface IDrawDevice
	{
		/// <summary>
		/// [GET] The projection type that is currently active in this drawing device.
		/// </summary>
		ProjectionMode Projection { get; }
		/// <summary>
		/// [GET] Reference coordinate for rendering i.e. the position of the drawing device's virtual camera.
		/// </summary>
		Vector3 ViewerPos { get; }
		/// <summary>
		/// [GET] Reference angle for rendering i.e. the angle of the drawing device's virtual camera.
		/// </summary>
		float ViewerAngle { get; }
		/// <summary>
		/// [GET] Reference distance for calculating the perspective effect. An object this far away from
		/// the camera will appear in its original size.
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
		/// [GET] Returns whether the drawing device is currently performing a visual picking opreation.
		/// </summary>
		bool IsPicking { get; }
		/// <summary>
		/// [GET] The size of the image that is rendered by this device.
		/// </summary>
		Vector2 TargetSize { get; }
		/// <summary>
		/// [GET] Provides access to the drawing devices shared <see cref="ShaderParameterCollection"/>,
		/// which allows to specify a parameter value globally across all materials rendered by this
		/// <see cref="DrawDevice"/>.
		/// </summary>
		ShaderParameterCollection ShaderParameters { get; }
		
		
		/// <summary>
		/// Returns the scale factor of objects that are located at the specified (world space) z-Coordinate.
		/// </summary>
		/// <param name="z"></param>
		float GetScaleAtZ(float z);
		/// <summary>
		/// Transforms screen space to world space. The screen positions Z coordinate is
		/// interpreted as the target world Z coordinate.
		/// </summary>
		/// <param name="screenPos"></param>
		Vector3 GetWorldPos(Vector3 screenPos);
		/// <summary>
		/// Transforms world space to screen space.
		/// </summary>
		/// <param name="spacePos"></param>
		Vector2 GetScreenPos(Vector3 spacePos);

		/// <summary>
		/// Determines whether a point or sphere is inside the devices viewing frustum,
		/// given a world space position and radius.
		/// </summary>
		/// <param name="worldPos">The points world space position.</param>
		/// <param name="radius">A world space radius around the point.</param>
		bool IsSphereInView(Vector3 worldPos, float radius = 1.0f);

		/// <summary>
		/// Rents a temporary material instance which can be used for rendering.
		/// The instance is returned implicitly when the device is done with the 
		/// current rendering operation.
		/// </summary>
		BatchInfo RentMaterial();

		/// <summary>
		/// Adds a parameterized set of vertices to the drawing devices rendering schedule.
		/// </summary>
		/// <typeparam name="T">The type of vertex data to add.</typeparam>
		/// <param name="material">The <see cref="Duality.Drawing.BatchInfo"/> to use for rendering the vertices.</param>
		/// <param name="vertexMode">The vertices drawing mode.</param>
		/// <param name="vertexBuffer">
		/// A vertex data buffer that stores the vertices to add. Ownership of the buffer
		/// remains at the callsite, while the <see cref="IDrawDevice"/> copies the required
		/// data into internal storage.
		/// </param>
		/// <param name="vertexCount">The number of vertices to add, from the beginning of the buffer.</param>
		void AddVertices<T>(BatchInfo material, VertexMode vertexMode, T[] vertexBuffer, int vertexCount) where T : struct, IVertexData;
		/// <summary>
		/// Adds an already prepared batch to the drawing devices rendering schedule.
		/// </summary>
		/// <param name="batch"></param>
		void AddBatch(DrawBatch batch);
	}

	/// <summary>
	/// A static class containing extension methods to extend <see cref="IDrawDevice"/> API with convenience methods.
	/// </summary>
	public static class ExtMethodsIDrawDevice
	{
		/// <summary>
		/// Transforms screen space to world space.
		/// </summary>
		/// <param name="screenPos"></param>
		/// <param name="device"></param>
		public static Vector3 GetWorldPos(this IDrawDevice device, Vector2 screenPos)
		{
			return device.GetWorldPos(new Vector3(screenPos));
		}
		/// <summary>
		/// Transforms world space to screen space.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <param name="device"></param>
		public static Vector2 GetScreenPos(this IDrawDevice device, Vector2 worldPos)
		{
			return device.GetScreenPos(new Vector3(worldPos));
		}
		
		/// <summary>
		/// Rents a temporary material instance for rendering, based on the specified <see cref="BatchInfo"/>.
		/// The instance is returned implicitly when the device is done with the current rendering operation.
		/// </summary>
		/// <param name="device"></param>
		/// <param name="baseMaterial"></param>
		public static BatchInfo RentMaterial(this IDrawDevice device, BatchInfo baseMaterial)
		{
			BatchInfo material = device.RentMaterial();
			material.InitFrom(baseMaterial);
			return material;
		}
		/// <summary>
		/// Rents a temporary material instance for rendering, based on the specified <see cref="Material"/>.
		/// The instance is returned implicitly when the device is done with the current rendering operation.
		/// </summary>
		/// <param name="device"></param>
		/// <param name="baseMaterial"></param>
		public static BatchInfo RentMaterial(this IDrawDevice device, ContentRef<Material> baseMaterial)
		{
			return device.RentMaterial(
				baseMaterial.IsAvailable ? 
				baseMaterial.Res.Info : 
				Material.Checkerboard.Res.Info);
		}

		/// <summary>
		/// Adds a parameterized set of vertices to the drawing devices rendering schedule.
		/// </summary>
		/// <typeparam name="T">The type of vertex data to add.</typeparam>
		/// <param name="device"></param>
		/// <param name="material">The <see cref="Duality.Resources.Material"/> to use for rendering the vertices.</param>
		/// <param name="vertexMode">The vertices drawing mode.</param>
		/// <param name="vertices">
		/// A vertex data buffer that stores the vertices to add. Ownership of the buffer
		/// remains at the callsite, while the <see cref="IDrawDevice"/> copies the required
		/// data into internal storage.
		/// </param>
		public static void AddVertices<T>(this IDrawDevice device, ContentRef<Material> material, VertexMode vertexMode, params T[] vertices) where T : struct, IVertexData
		{
			device.AddVertices<T>(
				material.IsAvailable ? material.Res.Info : Material.Checkerboard.Res.Info, 
				vertexMode, 
				vertices, 
				vertices.Length);
		}
		/// <summary>
		/// Adds a parameterized set of vertices to the drawing devices rendering schedule.
		/// </summary>
		/// <typeparam name="T">The type of vertex data to add.</typeparam>
		/// <param name="device"></param>
		/// <param name="material">The <see cref="Duality.Drawing.BatchInfo"/> to use for rendering the vertices.</param>
		/// <param name="vertexMode">The vertices drawing mode.</param>
		/// <param name="vertices">
		/// A vertex data buffer that stores the vertices to add. Ownership of the buffer
		/// remains at the callsite, while the <see cref="IDrawDevice"/> copies the required
		/// data into internal storage.
		/// </param>
		public static void AddVertices<T>(this IDrawDevice device, BatchInfo material, VertexMode vertexMode, params T[] vertices) where T : struct, IVertexData
		{
			device.AddVertices<T>(
				material, 
				vertexMode, 
				vertices, 
				vertices.Length);
		}
		/// <summary>
		/// Adds a parameterized set of vertices to the drawing devices rendering schedule.
		/// </summary>
		/// <typeparam name="T">The type of vertex data to add.</typeparam>
		/// <param name="device"></param>
		/// <param name="material">The <see cref="Duality.Resources.Material"/> to use for rendering the vertices.</param>
		/// <param name="vertexMode">The vertices drawing mode.</param>
		/// <param name="vertexBuffer">
		/// A vertex data buffer that stores the vertices to add. Ownership of the buffer
		/// remains at the callsite, while the <see cref="IDrawDevice"/> copies the required
		/// data into internal storage.
		/// </param>
		/// <param name="vertexCount">The number of vertices to add, from the beginning of the buffer.</param>
		public static void AddVertices<T>(this IDrawDevice device, ContentRef<Material> material, VertexMode vertexMode, T[] vertexBuffer, int vertexCount) where T : struct, IVertexData
		{
			device.AddVertices<T>(
				material.IsAvailable ? material.Res.Info : Material.Checkerboard.Res.Info, 
				vertexMode, 
				vertexBuffer, 
				vertexCount);
		}
	}
}
