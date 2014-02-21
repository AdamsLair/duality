using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality.Drawing;
using Duality.Resources;

namespace Duality
{
	/// <summary>
	/// Represents a textual description of a certain point in space in the context of <see cref="VisualLog">visual logging</see>.
	/// </summary>
	public sealed class VisualLogTextEntry : VisualLogEntry
	{
		private	Vector3		pos			= Vector3.Zero;
		private	Alignment	blockAlign	= Alignment.TopLeft;
		private	string[]	lines		= new string[0];
		
		/// <summary>
		/// [GET / SET] The point in space that is described by this text.
		/// </summary>
		public Vector3 Pos
		{
			get { return this.pos; }
			set { this.pos = value; }
		}
		/// <summary>
		/// [GET / SET] The texts (multiline block) spatial alignment.
		/// </summary>
		public Alignment BlockAlignment
		{
			get { return this.blockAlign; }
			set { this.blockAlign = value; }
		}
		/// <summary>
		/// [GET / SET] The text that will be displayed. Newline characters will
		/// be parsed correctly and be correctly displayed as line breaks.
		/// </summary>
		public string Text
		{
			get { return string.Join(Environment.NewLine, this.lines); }
			set
			{
				this.lines = value.Split('\n');
				for (int i = 0; i < this.lines.Length; i++)
				{
					this.lines[i] = this.lines[i].Trim('\n', '\r');
				}
			}
		}
		/// <summary>
		/// [GET] The displayed text, broken up into distinct lines. Do not modify - use <see cref="Text"/> instead.
		/// </summary>
		public string[] TextLines
		{
			get { return this.lines; }
		}

		public override void Draw(Canvas target, Vector3 basePos, float baseRotation, float baseScale)
		{
			float borderRadius = DefaultOutlineWidth;
			float textScale = 1.0f;
			bool worldSpace;

			// Scale anti-proportional to perspective scale in order to keep a constant size 
			// in screen space even when actually drawing in world space.
			{
				float scale = 1.0f;
				Vector3 posTemp = this.pos + basePos;
				target.DrawDevice.PreprocessCoords(ref posTemp, ref scale);
				worldSpace = (posTemp != this.pos + basePos);
				borderRadius /= scale;
				textScale /= scale;
			}

			// Determine base position
			Vector3 originPos = this.pos;
			MathF.TransformCoord(ref originPos.X, ref originPos.Y, baseRotation, baseScale);
			originPos += basePos;

			// Draw text and background
			BatchInfo matBoostAlpha = target.State.Material;
			matBoostAlpha.MainColor = matBoostAlpha.MainColor.WithAlpha(matBoostAlpha.MainColor.A * 2.0f / 255.0f);
			target.State.SetMaterial(matBoostAlpha);
			target.State.ColorTint *= this.Color;
			if (worldSpace) target.State.TransformAngle = target.DrawDevice.RefAngle;
			target.State.TransformScale = new Vector2(textScale, textScale);
			target.DrawText(
				this.lines,
				originPos.X, 
				originPos.Y, 
				originPos.Z,
				this.blockAlign,
				true);
		}
	}
}
