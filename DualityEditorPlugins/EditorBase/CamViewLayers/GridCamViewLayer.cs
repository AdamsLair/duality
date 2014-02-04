using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.VertexFormat;
using Duality.ColorFormat;
using Duality.Resources;

using OpenTK;

using EditorBase.CamViewStates;

namespace EditorBase.CamViewLayers
{
	public class GridCamViewLayer : CamViewLayer
	{
	    private float gridSize	= 100.0f;

	    public override string LayerName
	    {
	        get { return PluginRes.EditorBaseRes.CamViewLayer_Grid_Name; }
	    }
	    public override string LayerDesc
	    {
	        get { return PluginRes.EditorBaseRes.CamViewLayer_Grid_Desc; }
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

	        float scaleTemp = 1.0f;
	        Vector3 posTemp = Vector3.Zero;
	        device.PreprocessCoords(ref posTemp, ref scaleTemp);
	        if (posTemp.Z <= canvas.DrawDevice.NearZ) return;

	        float alphaTemp = 0.5f;
	        alphaTemp *= (float)Math.Min(1.0d, ((posTemp.Z - device.NearZ) / (device.NearZ * 5.0f)));
	        if (alphaTemp <= 0.005f) return;

	        float stepTemp = 4.0f * this.gridSize * MathF.Max(0.25f, MathF.Pow(2.0f, -MathF.Round(1.0f - MathF.Log(1.0f / scaleTemp, 2.0f))));
	        float scaledStep = stepTemp * scaleTemp;
	        float viewBoundRad = device.TargetSize.Length * 0.5f;
	        int lineCount = (2 + (int)MathF.Ceiling(viewBoundRad * 2 / scaledStep)) * 4;

	        ColorRgba gridColor = this.FgColor.WithAlpha(alphaTemp);
	        VertexC1P3[] vertices = new VertexC1P3[lineCount * 4];
			
	        float beginPos;
			float pos;
			int lineIndex;
			int vertOff = 0;

	        beginPos = posTemp.X % scaledStep - (lineCount / 8) * scaledStep;
			pos = beginPos;
			lineIndex = 0;
	        for (int x = 0; x < lineCount; x++)
	        {
	            bool primaryLine = lineIndex % 4 == 0;
	            bool secondaryLine = lineIndex % 4 == 2;

	            vertices[vertOff + x * 2 + 0].Color = primaryLine ? gridColor : gridColor.WithAlpha(alphaTemp * (secondaryLine ? 0.5f : 0.25f));

	            vertices[vertOff + x * 2 + 0].Pos.X = pos;
	            vertices[vertOff + x * 2 + 0].Pos.Y = -viewBoundRad;
	            vertices[vertOff + x * 2 + 0].Pos.Z = posTemp.Z + 1;

	            vertices[vertOff + x * 2 + 1] = vertices[vertOff + x * 2 + 0];
	            vertices[vertOff + x * 2 + 1].Pos.Y = viewBoundRad;

				pos += scaledStep / 4;
				lineIndex++;
	        }
			vertOff += lineCount * 2;

	        beginPos = posTemp.Y % scaledStep - (lineCount / 8) * scaledStep;
			pos = beginPos;
			lineIndex = 0;
	        for (int y = 0; y < lineCount; y++)
	        {
	            bool primaryLine = lineIndex % 4 == 0;
	            bool secondaryLine = lineIndex % 4 == 2;

	            vertices[vertOff + y * 2 + 0].Color = primaryLine ? gridColor : gridColor.WithAlpha(alphaTemp * (secondaryLine ? 0.5f : 0.25f));

	            vertices[vertOff + y * 2 + 0].Pos.X = -viewBoundRad;
	            vertices[vertOff + y * 2 + 0].Pos.Y = pos;
	            vertices[vertOff + y * 2 + 0].Pos.Z = posTemp.Z + 1;

	            vertices[vertOff + y * 2 + 1] = vertices[vertOff + y * 2 + 0];
	            vertices[vertOff + y * 2 + 1].Pos.X = viewBoundRad;

				pos += scaledStep / 4;
				lineIndex++;
	        }
			vertOff += lineCount * 2;

	        device.AddVertices(new BatchInfo(DrawTechnique.Alpha, ColorRgba.White), VertexMode.Lines, vertices);
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
				var cursorSpacePos = this.GetSpaceCoord(new Vector2(cursorPos.X, cursorPos.Y));

				cursorPos.X += 30;
				cursorPos.Y += 10;

				string[] text = new string[]
				{
					string.Format("X:{0,7:0}", cursorSpacePos.X),
					string.Format("Y:{0,7:0}", cursorSpacePos.Y)
				};
				canvas.DrawText(text, cursorPos.X, cursorPos.Y, drawBackground: true);
			}
		}
	}
}
