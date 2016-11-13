using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.ColorFormat;
using Duality.VertexFormat;

namespace Debug
{
	[Serializable]
	[RequiredComponent(typeof(Transform))]
    public class DebugObject : Renderer
    {
		public override float BoundRadius
		{
			get { return MathF.Sqrt(50.0f * 50.0f + 50.0f * 50.0f); }
		}
		public override void Draw(IDrawDevice device)
		{
			// Perform Camera space transformation
			Vector3 posBefore = this.GameObj.Transform.Pos;
			Vector3 posTemp = posBefore;
			float scaleTemp = 1.0f;
			device.PreprocessCoords(this, ref posTemp, ref scaleTemp);

			// Draw debug text
			VertexC1P3T2[] textVertices;
			textVertices = null;
			Font.GenericMonospace10.Res.EmitTextVertices(
				string.Format("Position (world): {0:0}, {1:0}, {2:0}", posBefore.X, posBefore.Y, posBefore.Z),
				ref textVertices, posTemp.X, posTemp.Y, posTemp.Z);
			device.AddVertices(Font.GenericMonospace10.Res.Material, BeginMode.Quads, textVertices);

			textVertices = null;
			Font.GenericMonospace10.Res.EmitTextVertices(
				string.Format("Position (cam): {0:0}, {1:0}, {2:0}", posTemp.X, posTemp.Y, posTemp.Z),
				ref textVertices, posTemp.X, posTemp.Y + 10, posTemp.Z);
			device.AddVertices(Font.GenericMonospace10.Res.Material, BeginMode.Quads, textVertices);

			textVertices = null;
			Font.GenericMonospace10.Res.EmitTextVertices(
				string.Format("Scale: {0:F}", scaleTemp),
				ref textVertices, posTemp.X, posTemp.Y + 20, posTemp.Z);
			device.AddVertices(Font.GenericMonospace10.Res.Material, BeginMode.Quads, textVertices);

			// Draw position indicator
			device.AddVertices(new BatchInfo(DrawTechnique.Alpha, ColorRgba.Red.WithAlpha(0.25f)), BeginMode.Quads, new VertexP3[] {
				new VertexP3(posTemp.X - 50.0f * scaleTemp, posTemp.Y - 50.0f * scaleTemp, posTemp.Z),
				new VertexP3(posTemp.X + 50.0f * scaleTemp, posTemp.Y - 50.0f * scaleTemp, posTemp.Z),
				new VertexP3(posTemp.X + 50.0f * scaleTemp, posTemp.Y + 50.0f * scaleTemp, posTemp.Z),
				new VertexP3(posTemp.X - 50.0f * scaleTemp, posTemp.Y + 50.0f * scaleTemp, posTemp.Z) });
		}
	}
}
