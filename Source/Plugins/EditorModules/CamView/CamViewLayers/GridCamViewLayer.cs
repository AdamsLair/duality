using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.Drawing;
using Duality.Resources;

using Duality.Editor.Plugins.CamView.CamViewStates;

namespace Duality.Editor.Plugins.CamView.CamViewLayers
{
	public class GridCamViewLayer : CamViewLayer
	{
		private RawList<VertexC1P3> vertexBuffer = null;

		public override string LayerName
		{
			get { return Properties.CamViewRes.CamViewLayer_Grid_Name; }
		}
		public override string LayerDesc
		{
			get { return Properties.CamViewRes.CamViewLayer_Grid_Desc; }
		}
		public override int Priority
		{
			get { return base.Priority - 10; }
		}
		public override bool MouseTracking
		{
			get { return true; }
		}

		protected internal override void OnCollectDrawcalls(Canvas canvas)
		{
			base.OnCollectDrawcalls(canvas);
			IDrawDevice device = canvas.DrawDevice;
			
			GridLayerData displayedData = default(GridLayerData);
			this.View.ActiveState.GetDisplayedGridData(Point.Empty, ref displayedData);

			float scaleTemp = 1.0f;
			Vector3 posTemp = Vector3.Zero;
			device.PreprocessCoords(ref posTemp, ref scaleTemp);
			if (posTemp.Z <= canvas.DrawDevice.NearZ) return;

			float alphaTemp = 0.5f;
			alphaTemp *= (float)Math.Min(1.0d, ((posTemp.Z - device.NearZ) / (device.NearZ * 5.0f)));
			if (alphaTemp <= 0.005f) return;
			ColorRgba gridColor = this.FgColor.WithAlpha(alphaTemp);

			float gridVisualMinSize = 50.0f;
			Vector2 gridBaseSize = displayedData.GridBaseSize;
			if (gridBaseSize.X <= 0.0f) gridBaseSize.X = 100.0f;
			if (gridBaseSize.Y <= 0.0f) gridBaseSize.Y = 100.0f;

			Vector2 adjustedGridBaseSize;
			adjustedGridBaseSize.X = gridBaseSize.X * MathF.NextPowerOfTwo((int)MathF.Ceiling(gridVisualMinSize / gridBaseSize.X));
			adjustedGridBaseSize.Y = gridBaseSize.Y * MathF.NextPowerOfTwo((int)MathF.Ceiling(gridVisualMinSize / gridBaseSize.Y));

			float scaleAdjustmentFactor = 4.0f * MathF.Pow(2.0f, -MathF.Round(1.0f - MathF.Log(1.0f / scaleTemp, 2.0f)));
			Vector2 adjustedGridSize;
			adjustedGridSize.X = MathF.Max(adjustedGridBaseSize.X * scaleAdjustmentFactor, gridBaseSize.X);
			adjustedGridSize.Y = MathF.Max(adjustedGridBaseSize.Y * scaleAdjustmentFactor, gridBaseSize.Y);

			Vector2 stepTemp = adjustedGridSize;
			Vector2 scaledStep = stepTemp * scaleTemp;
			float viewBoundRad = device.TargetSize.Length * 0.5f;
			int lineCountX = (2 + (int)MathF.Ceiling(viewBoundRad * 2 / scaledStep.X)) * 4;
			int lineCountY = (2 + (int)MathF.Ceiling(viewBoundRad * 2 / scaledStep.Y)) * 4;
			int vertexCount = (lineCountX * 2 + lineCountY * 2);

			if (this.vertexBuffer == null) this.vertexBuffer = new RawList<VertexC1P3>(vertexCount);
			this.vertexBuffer.Count = vertexCount;

			VertexC1P3[] vertices = this.vertexBuffer.Data;
			float beginPos;
			float pos;
			int lineIndex;
			int vertOff = 0;

			beginPos = posTemp.X % scaledStep.X - (lineCountX / 8) * scaledStep.X;
			pos = beginPos;
			lineIndex = 0;
			for (int x = 0; x < lineCountX; x++)
			{
				bool primaryLine = lineIndex % 4 == 0;
				bool secondaryLine = lineIndex % 4 == 2;

				vertices[vertOff + x * 2 + 0].Color = primaryLine ? gridColor : gridColor.WithAlpha(alphaTemp * (secondaryLine ? 0.5f : 0.25f));

				vertices[vertOff + x * 2 + 0].Pos.X = pos;
				vertices[vertOff + x * 2 + 0].Pos.Y = -viewBoundRad;
				vertices[vertOff + x * 2 + 0].Pos.Z = posTemp.Z + 1;

				vertices[vertOff + x * 2 + 1] = vertices[vertOff + x * 2 + 0];
				vertices[vertOff + x * 2 + 1].Pos.Y = viewBoundRad;

				pos += scaledStep.X / 4;
				lineIndex++;
			}
			vertOff += lineCountX * 2;

			beginPos = posTemp.Y % scaledStep.Y - (lineCountY / 8) * scaledStep.Y;
			pos = beginPos;
			lineIndex = 0;
			for (int y = 0; y < lineCountY; y++)
			{
				bool primaryLine = lineIndex % 4 == 0;
				bool secondaryLine = lineIndex % 4 == 2;

				vertices[vertOff + y * 2 + 0].Color = primaryLine ? gridColor : gridColor.WithAlpha(alphaTemp * (secondaryLine ? 0.5f : 0.25f));

				vertices[vertOff + y * 2 + 0].Pos.X = -viewBoundRad;
				vertices[vertOff + y * 2 + 0].Pos.Y = pos;
				vertices[vertOff + y * 2 + 0].Pos.Z = posTemp.Z + 1;

				vertices[vertOff + y * 2 + 1] = vertices[vertOff + y * 2 + 0];
				vertices[vertOff + y * 2 + 1].Pos.X = viewBoundRad;

				pos += scaledStep.Y / 4;
				lineIndex++;
			}
			vertOff += lineCountY * 2;

			device.AddVertices(new BatchInfo(DrawTechnique.Alpha, ColorRgba.White), VertexMode.Lines, vertices, this.vertexBuffer.Count);
		}
		protected internal override void OnCollectOverlayDrawcalls(Canvas canvas)
		{
			base.OnCollectOverlayDrawcalls(canvas);
			bool noActionText = string.IsNullOrEmpty(this.View.ActiveState.ActionText);
			bool mouseover = this.View.ActiveState.Mouseover;

			Point cursorPos = this.PointToClient(Cursor.Position);
			if (noActionText && mouseover &&
				cursorPos.X > 0 && cursorPos.X < this.ClientSize.Width &&
				cursorPos.Y > 0 && cursorPos.Y < this.ClientSize.Height)
			{
				GridLayerData displayedData = default(GridLayerData);
				this.View.ActiveState.GetDisplayedGridData(cursorPos, ref displayedData);

				cursorPos.X += 30;
				cursorPos.Y += 10;

				string[] text = new string[]
				{
					string.Format("X:{0,7:0}", displayedData.DisplayedGridPos.X),
					string.Format("Y:{0,7:0}", displayedData.DisplayedGridPos.Y)
				};
				canvas.DrawText(text, cursorPos.X, cursorPos.Y, drawBackground: true);
			}
		}
	}
}
