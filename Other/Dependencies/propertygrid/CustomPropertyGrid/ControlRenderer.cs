using System;
using System.Collections.Generic;
using System.Linq;

using System.Drawing;
using System.Drawing.Drawing2D;

using System.Windows.Forms;

using AdamsLair.PropertyGrid.EmbeddedResources;

namespace AdamsLair.PropertyGrid.Renderer
{
	public enum TextBoxStyle
	{
		Plain,
		Flat,
		Sunken
	}
	public enum GroupHeaderStyle
	{
		Flat,
		Simple,
		Emboss,
		SmoothSunken
	}

	[Flags]
	public enum TextBoxState
	{
		Disabled		= 0x1,
		Normal			= 0x2,
		Hot				= 0x4,
		Focus			= 0x8,

		ReadOnlyFlag	= 0x100
	}
	public enum ExpandNodeState
	{
		OpenedDisabled,
		OpenedNormal,
		OpenedHot,
		OpenedPressed,
		ClosedDisabled,
		ClosedNormal,
		ClosedHot,
		ClosedPressed
	}
	public enum CheckBoxState
	{
		CheckedDisabled		= System.Windows.Forms.VisualStyles.CheckBoxState.CheckedDisabled,
		CheckedPressed		= System.Windows.Forms.VisualStyles.CheckBoxState.CheckedPressed,
		CheckedHot			= System.Windows.Forms.VisualStyles.CheckBoxState.CheckedHot,
		CheckedNormal		= System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal,

		UncheckedDisabled	= System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedDisabled,
		UncheckedPressed	= System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedPressed,
		UncheckedHot		= System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedHot,
		UncheckedNormal		= System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal,

		MixedDisabled		= System.Windows.Forms.VisualStyles.CheckBoxState.MixedDisabled,
		MixedPressed		= System.Windows.Forms.VisualStyles.CheckBoxState.MixedPressed,
		MixedHot			= System.Windows.Forms.VisualStyles.CheckBoxState.MixedHot,
		MixedNormal			= System.Windows.Forms.VisualStyles.CheckBoxState.MixedNormal,

		PlusDisabled,
		PlusPressed,
		PlusHot,
		PlusNormal,

		MinusDisabled,
		MinusPressed,
		MinusHot,
		MinusNormal
	}
	public enum ButtonState
	{
		Disabled,
		Normal,
		Hot,
		Pressed
	}
	public enum BorderState
	{
		Normal,
		Disabled
	}
	public enum BorderStyle
	{
		Simple,
		Focus,
		ContentBox,
		Sunken
	}

	public class ControlRenderer
	{
		private const int DrawStringWidthAdd = 5;

		private bool								smallMode			= false;
		private	Size								expandNodeSize		= Size.Empty;
		private	Dictionary<ExpandNodeState,Bitmap>	expandNodeImages	= null;
		private	Size								checkBoxSize		= Size.Empty;
		private	Dictionary<CheckBoxState,Bitmap>	checkBoxImages		= null;
		private	IconImage	dropDownIcon	= new IconImage(Resources.DropDownIcon);


		public bool SmallMode
		{
			get { return this.smallMode; }
			internal set { this.smallMode = value; }
		}
		public Size CheckBoxSize
		{
			get
			{
				if (checkBoxSize.IsEmpty)
					checkBoxSize = CheckBoxRenderer.GetGlyphSize(Graphics.FromImage(new Bitmap(1, 1)), System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal);
				return checkBoxSize;
			}
		}
		public Size ExpandNodeSize
		{
			get
			{
				if (expandNodeSize.IsEmpty)
				{
					InitExpandNode(ExpandNodeState.ClosedNormal);
					expandNodeSize = expandNodeImages[ExpandNodeState.ClosedNormal].Size;
				}
				return expandNodeSize;
			}
		}
		public Font DefaultFont
		{
			get
			{
				return this.smallMode ? EmbeddedResources.Resources.DefaultFontSmall : EmbeddedResources.Resources.DefaultFont;
			}
		}
		public Font DefaultFontBold
		{
			get
			{
				return this.smallMode ? EmbeddedResources.Resources.DefaultFontBoldSmall : EmbeddedResources.Resources.DefaultFontBold;
			}
		}
		public Color ColorHightlight { get; set; }
		public Color ColorVeryDarkBackground { get; set; }
		public Color ColorDarkBackground { get; set; }
		public Color ColorBackground { get; set; }
		public Color ColorLightBackground { get; set; }
		public Color ColorVeryLightBackground { get; set; }
		public Color ColorMultiple { get; set; }
		public Color ColorText { get; set; }
		public Color ColorGrayText { get; set; }


		public ControlRenderer(bool smallMode)
		{
			this.smallMode = smallMode;
			this.ResetColors();
		}
		public void ResetColors()
		{
			this.ColorHightlight = SystemColors.Highlight;
			this.ColorVeryDarkBackground = SystemColors.ControlDarkDark;
			this.ColorDarkBackground = SystemColors.ControlDark;
			this.ColorLightBackground = SystemColors.ControlLightLight;
			this.ColorVeryLightBackground = SystemColors.Window;
			this.ColorBackground = SystemColors.Control;
			this.ColorText = SystemColors.ControlText;
			this.ColorMultiple = Color.Bisque;
			this.ColorGrayText = SystemColors.GrayText;
		}


		public void DrawCheckBox(Graphics g, Point loc, CheckBoxState state)
		{
			InitCheckBox(state);
			g.DrawImageUnscaled(checkBoxImages[state], loc);
		}
		public void DrawExpandNode(Graphics g, Point loc, ExpandNodeState state)
		{
			InitExpandNode(state);
			g.DrawImageUnscaled(expandNodeImages[state], loc);
		}

		public void DrawGroupHeaderBackground(Graphics g, Rectangle rect, Color baseColor, GroupHeaderStyle style)
		{
			if (rect.Height == 0 || rect.Width == 0) return;
			Color lightColor = baseColor.ScaleBrightness(style == GroupHeaderStyle.SmoothSunken ? 0.85f : 1.1f);
			Color darkColor = baseColor.ScaleBrightness(style == GroupHeaderStyle.SmoothSunken ? 0.95f : 0.85f);
			LinearGradientBrush gradientBrush = new LinearGradientBrush(rect, lightColor, darkColor, 90.0f);

			if (style != GroupHeaderStyle.Simple && style != GroupHeaderStyle.Flat)
				g.FillRectangle(gradientBrush, rect);
			else
				g.FillRectangle(new SolidBrush(baseColor), rect);

			if (style == GroupHeaderStyle.Flat) return;

			g.DrawLine(new Pen(Color.FromArgb(128, Color.White)), rect.Left, rect.Top, rect.Right, rect.Top);
			g.DrawLine(new Pen(Color.FromArgb(64, Color.Black)), rect.Left, rect.Bottom - 1, rect.Right, rect.Bottom - 1);

			g.DrawLine(new Pen(Color.FromArgb(64, Color.White)), rect.Left, rect.Top, rect.Left, rect.Bottom - 1);
			g.DrawLine(new Pen(Color.FromArgb(32, Color.Black)), rect.Right, rect.Top, rect.Right, rect.Bottom - 1);
		}

		public void DrawStringLine(Graphics g, string text, Font font, Rectangle textRect, Color textColor, StringAlignment align = StringAlignment.Near, StringAlignment lineAlign = StringAlignment.Center, StringTrimming trimming = StringTrimming.EllipsisCharacter)
		{
			if (textRect.Width < 1 || textRect.Height < 1) return;
			if (text == null) return;

			// Expand text rect, because DrawString stops too soon
			textRect.Y += GetFontYOffset(font);
			textRect.Width += DrawStringWidthAdd;
			textRect.Height = Math.Max(textRect.Height, font.Height);

			bool manualEllipsis = trimming == StringTrimming.EllipsisCharacter || trimming == StringTrimming.EllipsisWord;
			if (trimming == StringTrimming.EllipsisCharacter)	trimming = StringTrimming.Character;
			if (trimming == StringTrimming.EllipsisWord)		trimming = StringTrimming.Word;

			if (manualEllipsis) textRect.Width -= 5;
			StringFormat nameLabelFormat = StringFormat.GenericDefault;
			nameLabelFormat.Alignment = align;
			nameLabelFormat.LineAlignment = lineAlign;
			nameLabelFormat.Trimming = trimming;
			nameLabelFormat.FormatFlags |= StringFormatFlags.NoWrap;

			int charsFit, lines;
			SizeF nameLabelSize = g.MeasureString(text, font, textRect.Size, nameLabelFormat, out charsFit, out lines);
			if (textRect.Width >= 1)
				g.DrawString(text, font, new SolidBrush(textColor), textRect, nameLabelFormat);

			if (charsFit < text.Length && manualEllipsis)
			{
				Pen ellipsisPen = new Pen(textColor);
				ellipsisPen.DashStyle = DashStyle.Dot;
				g.DrawLine(ellipsisPen, 
					textRect.Right - DrawStringWidthAdd, 
					(textRect.Y + textRect.Height * 0.5f) + (nameLabelSize.Height * 0.3f), 
					textRect.Right - DrawStringWidthAdd + 3, 
					(textRect.Y + textRect.Height * 0.5f) + (nameLabelSize.Height * 0.3f));
			}
		}
		public Region[] MeasureStringLine(Graphics g, string text, CharacterRange[] measureRanges, Font font, Rectangle textRect, StringAlignment align = StringAlignment.Near, StringAlignment lineAlign = StringAlignment.Center)
		{
			// Expand text rect, because DrawString stops too soon
			textRect.Width += DrawStringWidthAdd;
			textRect.Height = Math.Max(textRect.Height, font.Height);

			// Assume manual ellipsis
			textRect.Width -= 5;
			StringFormat nameLabelFormat = StringFormat.GenericDefault;
			nameLabelFormat.Alignment = align;
			nameLabelFormat.LineAlignment = lineAlign;
			nameLabelFormat.Trimming = StringTrimming.Character;
			nameLabelFormat.FormatFlags |= StringFormatFlags.NoWrap | StringFormatFlags.MeasureTrailingSpaces;
			nameLabelFormat.SetMeasurableCharacterRanges(measureRanges);

			return g.MeasureCharacterRanges(text, font, textRect, nameLabelFormat);
		}
		public int PickCharStringLine(string text, Font font, Rectangle textRect, Point pickLoc, StringAlignment align = StringAlignment.Near, StringAlignment lineAlign = StringAlignment.Center)
		{
			if (text == null) return -1;
			if (!textRect.Contains(pickLoc)) return -1;
			
			// Expand text rect, because DrawString stops too soon
			textRect.Width += DrawStringWidthAdd;
			textRect.Height = Math.Max(textRect.Height, font.Height);

			// Assume manual ellipsis
			textRect.Width -= 5;
			StringFormat nameLabelFormat = StringFormat.GenericDefault;
			nameLabelFormat.Alignment = align;
			nameLabelFormat.LineAlignment = lineAlign;
			nameLabelFormat.Trimming = StringTrimming.Character;
			nameLabelFormat.FormatFlags |= StringFormatFlags.LineLimit;
			nameLabelFormat.FormatFlags |= StringFormatFlags.NoWrap | StringFormatFlags.MeasureTrailingSpaces;

			RectangleF pickRect = new RectangleF(textRect.X, textRect.Y, pickLoc.X - textRect.X, textRect.Height);

			using (var image = new Bitmap(1, 1)) { using (var g = Graphics.FromImage(image)) {
				// Pick chars
				float oldUsedWidth = 0.0f;
				SizeF usedSize = SizeF.Empty;
				for (int i = 0; i <= text.Length; i++)
				{
					oldUsedWidth = usedSize.Width;
					nameLabelFormat.SetMeasurableCharacterRanges(new [] { new CharacterRange(0, i) });
					Region[] usedSizeRegions = g.MeasureCharacterRanges(text, font, textRect, nameLabelFormat);
					usedSize = usedSizeRegions.Length > 0 ? usedSizeRegions[0].GetBounds(g).Size : SizeF.Empty;
					if (usedSize.Width > pickRect.Width)
					{
						if (Math.Abs(oldUsedWidth - pickRect.Width) < Math.Abs(usedSize.Width - pickRect.Width))
							return i - 1;
						else
							return i;
					}
				}
			}}

			return -1;
		}
		private int GetFontYOffset(Font font)
		{
			int fontOffset = font.Height - 2 * (int)(font.Height * 0.5f);
			return fontOffset - 1;
		}

		public Rectangle DrawTextBoxBorder(Graphics g, Rectangle rect, TextBoxState state, TextBoxStyle style, Color backColor)
		{
			if (rect.Width < 4 || rect.Height < 4) return rect;
			Rectangle clientRect = rect;

			Color borderColor = this.ColorDarkBackground.ScaleBrightness(1.2f);
			Color borderColorDark = this.ColorDarkBackground.ScaleBrightness(0.85f);

			if (style == TextBoxStyle.Plain)
			{
				clientRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);

				Pen innerPen;
				if (state == TextBoxState.Normal)
				{
					innerPen = new Pen(Color.Transparent);
				}
				else if (state == TextBoxState.Hot)
				{
					innerPen = new Pen(Color.FromArgb(32, this.ColorHightlight));
				}
				else if (state == TextBoxState.Focus)
				{
					innerPen = new Pen(Color.FromArgb(64, this.ColorHightlight));
				}
				else //if (state == TextBoxState.Disabled)
				{
					innerPen = new Pen(Color.Transparent);
				}

				g.FillRectangle(new SolidBrush(backColor), rect);
				g.DrawRectangle(innerPen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
			}
			else if (style == TextBoxStyle.Flat)
			{
				clientRect = new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height - 4);

				Pen borderPen;
				Pen innerPen;
				if (state == TextBoxState.Normal)
				{
					borderPen = new Pen(borderColorDark);
					innerPen = new Pen(Color.Transparent);
				}
				else if (state == TextBoxState.Hot)
				{
					borderPen = new Pen(borderColorDark.MixWith(this.ColorHightlight, 0.25f, true));
					innerPen = new Pen(Color.FromArgb(32, this.ColorHightlight));
				}
				else if (state == TextBoxState.Focus)
				{
					borderPen = new Pen(borderColorDark.MixWith(this.ColorHightlight, 0.5f, true));
					innerPen = new Pen(Color.FromArgb(48, this.ColorHightlight));
				}
				else //if (state == TextBoxState.Disabled)
				{
					borderPen = new Pen(Color.FromArgb(128, borderColorDark));
					innerPen = new Pen(Color.Transparent);
				}

				g.FillRectangle(new SolidBrush(backColor), rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);

				g.DrawRectangle(borderPen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
				g.DrawRectangle(innerPen, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
			}
			else if (style == TextBoxStyle.Sunken)
			{
				clientRect = new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height - 4);

				Pen borderPenDark;
				Pen borderPen;
				Pen innerPen;
				if (state == TextBoxState.Normal)
				{
					borderPenDark = new Pen(borderColorDark);
					borderPen = new Pen(borderColor);
					innerPen = new Pen(Color.Transparent);
				}
				else if (state == TextBoxState.Hot)
				{
					borderPenDark = new Pen(borderColorDark.MixWith(this.ColorHightlight, 0.25f, true));
					borderPen = new Pen(borderColor.MixWith(this.ColorHightlight, 0.25f, true));
					innerPen = new Pen(Color.FromArgb(32, this.ColorHightlight));
				}
				else if (state == TextBoxState.Focus)
				{
					borderPenDark = new Pen(borderColorDark.MixWith(this.ColorHightlight, 0.5f, true));
					borderPen = new Pen(borderColor.MixWith(this.ColorHightlight, 0.5f, true));
					innerPen = new Pen(Color.FromArgb(48, this.ColorHightlight));
				}
				else //if (state == TextBoxState.Disabled)
				{
					borderPenDark = new Pen(Color.FromArgb(128, borderColorDark));
					borderPen = new Pen(Color.FromArgb(128, borderColor));
					innerPen = new Pen(Color.Transparent);
				}

				g.FillRectangle(new SolidBrush(backColor), rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);

				g.DrawLine(borderPenDark, rect.X + 1, rect.Y, rect.Right - 2, rect.Y);
				g.DrawLine(borderPen, rect.X, rect.Y + 1, rect.X, rect.Bottom - 2);
				g.DrawLine(borderPen, rect.Right - 1, rect.Y + 1, rect.Right - 1, rect.Bottom - 2);
				g.DrawLine(borderPen, rect.X + 1, rect.Bottom - 1, rect.Right - 2, rect.Bottom - 1);
				g.DrawRectangle(innerPen, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
			}

			return clientRect;
		}
		public void DrawTextField(Graphics g, Rectangle rect, string text, Font font, Color textColor, Color backColor, TextBoxState state, TextBoxStyle style, int scroll = 0, int cursorPos = -1, int selLength = 0)
		{
			if (rect.Width < 4 || rect.Height < 4) return;
			GraphicsState oldState = g.Save();

			// Draw Background
			Rectangle textRect = DrawTextBoxBorder(g, rect, state, style, backColor);
			Rectangle textRectScrolled = new Rectangle(
				textRect.X - scroll,
				textRect.Y,
				textRect.Width + scroll,
				textRect.Height);
			
			RectangleF clipRect = g.ClipBounds;
			clipRect = textRect;
			g.SetClip(clipRect);

			if (text != null)
			{
				// Draw Selection
				if ((state & TextBoxState.Focus) == TextBoxState.Focus && cursorPos >= 0 && selLength != 0)
				{
					int selPos = Math.Min(cursorPos + selLength, cursorPos);
					CharacterRange[] charRanges = new [] { new CharacterRange(selPos, Math.Abs(selLength)) };
					Region[] charRegions = MeasureStringLine(g, text, charRanges, font, textRectScrolled);
					RectangleF selectionRect = charRegions.Length > 0 ? charRegions[0].GetBounds(g) : RectangleF.Empty;
					selectionRect.Inflate(0, 2);
					selectionRect.Y += GetFontYOffset(font);
					if (selPos == 0)
					{
						selectionRect.X -= 2;
						selectionRect.Width += 2;
					}
					if (selPos + Math.Abs(selLength) == text.Length)
					{
						selectionRect.Width += 2;
					}

					if ((state & TextBoxState.ReadOnlyFlag) == TextBoxState.ReadOnlyFlag)
						g.FillRectangle(new SolidBrush(Color.FromArgb(128, this.ColorGrayText)), selectionRect);
					else
						g.FillRectangle(new SolidBrush(Color.FromArgb(128, this.ColorHightlight)), selectionRect);
				}

				// Draw Text
				if ((state & TextBoxState.Disabled) == TextBoxState.Disabled ||
					(state & TextBoxState.ReadOnlyFlag) == TextBoxState.ReadOnlyFlag)
					textColor = Color.FromArgb(128, textColor);
				DrawStringLine(g, text, font, textRectScrolled, textColor);
			}

			// Draw Cursor
			if ((state & TextBoxState.ReadOnlyFlag) != TextBoxState.ReadOnlyFlag && cursorPos >= 0 && selLength == 0)
			{
				CharacterRange[] charRanges = new [] { new CharacterRange(0, cursorPos) };
				Region[] charRegions = MeasureStringLine(g, text ?? "", charRanges, font, textRectScrolled);
				RectangleF textRectUntilCursor = charRegions.Length > 0 ? charRegions[0].GetBounds(g) : RectangleF.Empty;
				int curPixelPos = textRectScrolled.X + (int)textRectUntilCursor.Width + 2;
				DrawCursor(g, new Rectangle(curPixelPos, textRectScrolled.Top + 1, 1, textRectScrolled.Height - 2));
			}

			g.Restore(oldState);
		}
		public int PickCharTextField(Rectangle rect, string text, Font font, TextBoxStyle style, Point pickLoc, int scroll = 0)
		{

			if (style == TextBoxStyle.Plain)
				rect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
			else if (style == TextBoxStyle.Flat)
				rect = new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height - 4);
			else if (style == TextBoxStyle.Sunken)
				rect = new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height - 4);

			pickLoc.X += scroll;
			rect.Width += scroll;
			return PickCharStringLine(text, font, rect, pickLoc);

		}
		public int GetCharPosTextField(Rectangle rect, string text, Font font, TextBoxStyle style, int index, int scroll = 0)
		{
			if (style == TextBoxStyle.Plain)
				rect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
			else if (style == TextBoxStyle.Flat)
				rect = new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height - 4);
			else if (style == TextBoxStyle.Sunken)
				rect = new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height - 4);
			
			Rectangle rectScrolled = new Rectangle(
				rect.X - scroll,
				rect.Y,
				rect.Width + scroll + 10000000,
				rect.Height);
			if (index == 0) return rectScrolled.Left;

			using (var image = new Bitmap(1, 1)) { using (var g = Graphics.FromImage(image)) {
				StringFormat nameLabelFormat = StringFormat.GenericDefault;
				nameLabelFormat.Alignment = StringAlignment.Near;
				nameLabelFormat.LineAlignment = StringAlignment.Center;
				nameLabelFormat.Trimming = StringTrimming.Character;
				nameLabelFormat.FormatFlags |= StringFormatFlags.NoWrap | StringFormatFlags.MeasureTrailingSpaces;
				nameLabelFormat.SetMeasurableCharacterRanges(new [] {new CharacterRange(0, index)});

				Region[] measured = g.MeasureCharacterRanges(text, font, rectScrolled, nameLabelFormat);
				RectangleF bound = measured[0].GetBounds(g);
				return (int)bound.Right;
			}}
		}

		public void DrawCursor(Graphics g, Rectangle rect)
		{
			g.FillRectangle(Brushes.Black, rect);
		}

		public void DrawBorder(Graphics g, Rectangle rect, BorderStyle style, BorderState state)
		{
			Color darkColor = this.ColorDarkBackground;
			Color lightColor = this.ColorLightBackground;
			
			if (style == BorderStyle.Simple || style == BorderStyle.Focus)
				darkColor = this.ColorVeryDarkBackground;
			else if (style == BorderStyle.Sunken)
			{
				darkColor = Color.FromArgb(128, this.ColorDarkBackground);
				lightColor = this.ColorLightBackground;
			}

			Pen darkPen = new Pen(state == BorderState.Disabled ? Color.FromArgb(darkColor.A / 2, darkColor) : darkColor);
			Pen lightPen = new Pen(state == BorderState.Disabled ? Color.FromArgb(lightColor.A / 2, lightColor) : lightColor);

			if (style == BorderStyle.ContentBox)
			{
				g.DrawRectangle(lightPen, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
				g.DrawRectangle(darkPen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
			}
			else if (style == BorderStyle.Simple)
			{
				g.DrawRectangle(darkPen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
			}
			else if (style == BorderStyle.Focus)
			{
				darkPen.DashStyle = DashStyle.Dot;
				g.DrawRectangle(darkPen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
			}
			else if (style == BorderStyle.Sunken)
			{
				g.DrawRectangle(lightPen, rect.X + 1, rect.Y + 1, rect.Width - 1, rect.Height - 1);
				g.DrawRectangle(darkPen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
			}
		}
		
		public void DrawButtonBackground(Graphics g, Rectangle rect, ButtonState state)
		{
			if (rect.Width < 4 || rect.Height < 4) return;

			GraphicsPath borderPath = new GraphicsPath();
			borderPath.AddPolygon(new [] {
				new Point(rect.Left, rect.Top + 2),
				new Point(rect.Left + 2, rect.Top),
				new Point(rect.Right - 3, rect.Top),
				new Point(rect.Right - 1, rect.Top + 2),
				new Point(rect.Right - 1, rect.Bottom - 3),
				new Point(rect.Right - 3, rect.Bottom - 1),
				new Point(rect.Left + 2, rect.Bottom - 1),
				new Point(rect.Left, rect.Bottom - 3) });
			Rectangle outerRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
			GraphicsPath outerPath = new GraphicsPath();
			outerPath.AddPolygon(new [] {
				new Point(outerRect.Left, outerRect.Top + 1),
				new Point(outerRect.Left + 1, outerRect.Top),
				new Point(outerRect.Right - 2, outerRect.Top),
				new Point(outerRect.Right - 1, outerRect.Top + 1),
				new Point(outerRect.Right - 1, outerRect.Bottom - 2),
				new Point(outerRect.Right - 2, outerRect.Bottom - 1),
				new Point(outerRect.Left + 1, outerRect.Bottom - 1),
				new Point(outerRect.Left, outerRect.Bottom - 2) });
			Rectangle innerRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
			Rectangle innerRectUpper = new Rectangle(innerRect.X, innerRect.Y, innerRect.Width, innerRect.Height / 2);
			Rectangle innerRectLower = new Rectangle(innerRect.X, innerRect.Y + innerRectUpper.Height, innerRect.Width, innerRect.Height - innerRectUpper.Height);
			
			Color colorInner;
			Color colorBorder;
			Brush upperBrush;
			Brush lowerBrush;

			if (state == ButtonState.Normal)
			{
				colorInner = this.ColorVeryLightBackground;
				colorBorder = this.ColorVeryDarkBackground;

				Color colorGradBase = this.ColorVeryDarkBackground;
				Color gradLight2 = colorGradBase.MixWith(colorInner, 0.9f);
				Color gradLight = colorGradBase.MixWith(colorInner, 0.8f);
				Color gradDark = colorGradBase.MixWith(colorInner, 0.725f);
				Color gradDark2 = colorGradBase.MixWith(colorInner, 0.625f);

				upperBrush = new LinearGradientBrush(innerRectUpper, gradLight2, gradLight, 90.0f);
				lowerBrush = new LinearGradientBrush(innerRectUpper, gradDark, gradDark2, 90.0f);
			}
			else if (state == ButtonState.Hot)
			{
				colorInner = this.ColorVeryLightBackground;
				colorBorder = this.ColorVeryDarkBackground.MixWith(this.ColorHightlight, 0.4f);

				Color colorGradBase = this.ColorHightlight;
				Color gradLight2 = colorGradBase.MixWith(colorInner, 0.9f);
				Color gradLight = colorGradBase.MixWith(colorInner, 0.8f);
				Color gradDark = colorGradBase.MixWith(colorInner, 0.7f);
				Color gradDark2 = colorGradBase.MixWith(colorInner, 0.6f);

				upperBrush = new LinearGradientBrush(innerRectUpper, gradLight2, gradLight, 90.0f);
				lowerBrush = new LinearGradientBrush(innerRectUpper, gradDark, gradDark2, 90.0f);
			}
			else if (state == ButtonState.Pressed)
			{
				colorBorder = this.ColorVeryDarkBackground.MixWith(this.ColorHightlight, 1.0f, true);
				colorInner = colorBorder;

				Color colorGradBase = this.ColorHightlight;
				Color colorGradBase2 = this.ColorVeryLightBackground;
				Color gradLight2 = colorGradBase.MixWith(colorGradBase2, 0.9f);
				Color gradLight = colorGradBase.MixWith(colorGradBase2, 0.7f);
				Color gradDark = colorGradBase.MixWith(colorGradBase2, 0.5f);
				Color gradDark2 = colorGradBase.MixWith(colorGradBase2, 0.2f);

				innerRectUpper.Height += 1;
				innerRectLower.Y += 1;
				innerRectLower.Height -= 1;

				upperBrush = new LinearGradientBrush(innerRectUpper, gradLight2, gradLight, 90.0f);
				lowerBrush = new LinearGradientBrush(innerRectUpper, gradDark, gradDark2, 90.0f);
			}
			else
			{
				colorInner = Color.FromArgb(128, this.ColorVeryLightBackground);
				colorBorder = Color.FromArgb(128, this.ColorVeryDarkBackground);
				upperBrush = new SolidBrush(Color.Transparent);
				lowerBrush = new SolidBrush(Color.Transparent);
			}

			g.FillRectangle(lowerBrush, innerRectLower);
			g.FillRectangle(upperBrush, innerRectUpper);

			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.DrawPath(new Pen(colorBorder), borderPath);
			g.DrawPath(new Pen(Color.FromArgb(128, colorInner)), outerPath);
			g.SmoothingMode = SmoothingMode.Default;

			if (state == ButtonState.Pressed)
				g.DrawLine(new Pen((lowerBrush as LinearGradientBrush).LinearColors[1]), innerRectLower.X + 1, innerRectLower.Bottom - 1, innerRectLower.Right - 2, innerRectLower.Bottom - 1);
		}
		public void DrawButton(Graphics g, Rectangle rect, ButtonState state, string text, IconImage icon)
		{
			DrawButton(g, rect, state, text, state == ButtonState.Disabled ? icon.Disabled : icon.Normal);
		}
		public void DrawButton(Graphics g, Rectangle rect, ButtonState state, string text, Image icon = null)
		{
			if (rect.Width < 4 || rect.Height < 4) return;

			GraphicsState graphicsState = g.Save();

			DrawButtonBackground(g, rect, state);

			Rectangle innerRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
			Color colorText;
			if (state == ButtonState.Disabled)
				colorText = this.ColorGrayText;
			else
				colorText = this.ColorText;

			RectangleF clipRect = innerRect;
			clipRect.Intersect(g.ClipBounds);
			g.SetClip(clipRect);

			if (icon == null && !string.IsNullOrEmpty(text))
			{
				DrawStringLine(g, text, this.DefaultFont, innerRect, colorText, StringAlignment.Center);
			}
			else if (string.IsNullOrEmpty(text))
			{
				Rectangle iconRect;
				iconRect = new Rectangle(
					innerRect.X + innerRect.Width / 2 - icon.Width / 2, 
					innerRect.Y + innerRect.Height / 2 - icon.Height / 2, 
					icon.Width, 
					icon.Height);
				g.DrawImageUnscaled(icon, iconRect);
			}
			else
			{
				Region[] charRegions = MeasureStringLine(g, text, new [] { new CharacterRange(0, text.Length) }, this.DefaultFont, innerRect);
				SizeF textSize = charRegions[0].GetBounds(g).Size;
				Size iconTextSize;
				Rectangle textRect;
				Rectangle iconRect;

				iconTextSize = new Size(icon.Width + (int)textSize.Width, innerRect.Height);
				iconRect = new Rectangle(
					innerRect.X + innerRect.Width / 2 - (int)textSize.Width / 2 - icon.Width * 3 / 4, 
					innerRect.Y + innerRect.Height / 2 - icon.Height / 2, 
					icon.Width, 
					icon.Height);
				textRect = new Rectangle(
					iconRect.Right, 
					innerRect.Y, 
					innerRect.Width - iconRect.Width, 
					innerRect.Height);

				g.DrawImageUnscaled(icon, iconRect);
				DrawStringLine(g, text, this.DefaultFont, textRect, colorText);
			}

			g.Restore(graphicsState);
		}
		public void DrawComboButton(Graphics g, Rectangle rect, ButtonState state, string text, IconImage icon)
		{
			DrawComboButton(g, rect, state, text, state == ButtonState.Disabled ? icon.Disabled : icon.Normal);
		}
		public void DrawComboButton(Graphics g, Rectangle rect, ButtonState state, string text, Image icon = null)
		{
			if (rect.Width < 8 + dropDownIcon.Width || rect.Height < 4) return;
			GraphicsState graphicsState = g.Save();

			DrawButtonBackground(g, rect, state);

			Rectangle innerRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
			Color colorText;
			if (state == ButtonState.Disabled)
				colorText = this.ColorGrayText;
			else
				colorText = this.ColorText;

			Rectangle dropDownIconRect = new Rectangle(
				innerRect.Right - dropDownIcon.Width - 4,
				innerRect.Y + innerRect.Height / 2 - dropDownIcon.Height / 2,
				dropDownIcon.Width,
				dropDownIcon.Height);
			innerRect = new Rectangle(innerRect.X + 2, innerRect.Y, innerRect.Width - dropDownIconRect.Width - 6, innerRect.Height);

			Image stateDropDownIcon = dropDownIcon.Normal;
			if (state == ButtonState.Disabled) stateDropDownIcon = dropDownIcon.Disabled;
			g.DrawImageUnscaled(stateDropDownIcon, dropDownIconRect.Location);

			RectangleF clipRect = innerRect;
			clipRect.Intersect(g.ClipBounds);
			g.SetClip(clipRect);
			
			if (icon != null && !string.IsNullOrEmpty(text))
			{
				Region[] charRegions = MeasureStringLine(g, text, new [] { new CharacterRange(0, text.Length) }, this.DefaultFont, innerRect);
				SizeF textSize = charRegions[0].GetBounds(g).Size;
				Size iconTextSize;
				Rectangle textRect;
				Rectangle iconRect;

				iconTextSize = new Size(icon.Width + (int)textSize.Width, innerRect.Height);
				iconRect = new Rectangle(
					innerRect.X, 
					innerRect.Y + innerRect.Height / 2 - icon.Height / 2, 
					icon.Width, 
					icon.Height);
				textRect = new Rectangle(
					iconRect.Right, 
					innerRect.Y, 
					innerRect.Width - iconRect.Width, 
					innerRect.Height);

				g.DrawImageUnscaled(icon, iconRect);
				DrawStringLine(g, text, this.DefaultFont, textRect, colorText);
			}
			else if (!string.IsNullOrEmpty(text))
			{
				DrawStringLine(g, text, this.DefaultFont, innerRect, colorText);
			}
			else if (icon != null)
			{
				Rectangle iconRect;
				iconRect = new Rectangle(
					innerRect.X + innerRect.Width / 2 - icon.Width / 2, 
					innerRect.Y + innerRect.Height / 2 - icon.Height / 2, 
					icon.Width, 
					icon.Height);
				g.DrawImageUnscaled(icon, iconRect);
			}

			g.Restore(graphicsState);
		}

		private void InitCheckBox(CheckBoxState checkState)
		{
			if (checkBoxImages != null && checkBoxImages.ContainsKey(checkState)) return;
			if (checkBoxImages == null) checkBoxImages = new Dictionary<CheckBoxState,Bitmap>();

			Bitmap image = new Bitmap(CheckBoxSize.Width, CheckBoxSize.Height);
			using (Graphics checkBoxGraphics = Graphics.FromImage(image))
			{
				if (checkState == CheckBoxState.PlusDisabled || 
					checkState == CheckBoxState.PlusHot ||
					checkState == CheckBoxState.PlusNormal ||
					checkState == CheckBoxState.PlusPressed ||
					checkState == CheckBoxState.MinusDisabled || 
					checkState == CheckBoxState.MinusHot ||
					checkState == CheckBoxState.MinusNormal ||
					checkState == CheckBoxState.MinusPressed)
				{
					Color plusSignColor;
					Pen expandLineShadowPen;
					Pen expandLinePen;

					if (checkState == CheckBoxState.PlusNormal || checkState == CheckBoxState.MinusNormal)
					{
						plusSignColor = Color.FromArgb(24, 32, 82);
						expandLinePen = new Pen(Color.FromArgb(255, plusSignColor));
						expandLineShadowPen = new Pen(Color.FromArgb(64, plusSignColor));
						CheckBoxRenderer.DrawCheckBox(checkBoxGraphics, Point.Empty, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
					}
					else if (checkState == CheckBoxState.PlusHot || checkState == CheckBoxState.MinusHot)
					{
						plusSignColor = Color.FromArgb(32, 48, 123);
						expandLinePen = new Pen(Color.FromArgb(255, plusSignColor));
						expandLineShadowPen = new Pen(Color.FromArgb(64, plusSignColor));
						CheckBoxRenderer.DrawCheckBox(checkBoxGraphics, Point.Empty, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedHot);
					}
					else if (checkState == CheckBoxState.PlusPressed || checkState == CheckBoxState.MinusPressed)
					{
						plusSignColor = Color.FromArgb(48, 64, 164);
						expandLinePen = new Pen(Color.FromArgb(255, plusSignColor));
						expandLineShadowPen = new Pen(Color.FromArgb(96, plusSignColor));
						CheckBoxRenderer.DrawCheckBox(checkBoxGraphics, Point.Empty, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedPressed);
					}
					else //if (checkState == CheckBoxState.PlusDisabled)
					{
						plusSignColor = Color.FromArgb(24, 28, 41);
						expandLinePen = new Pen(Color.FromArgb(128, plusSignColor));
						expandLineShadowPen = new Pen(Color.FromArgb(32, plusSignColor));
						CheckBoxRenderer.DrawCheckBox(checkBoxGraphics, Point.Empty, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedDisabled);
					}

					// Plus Shadow
					checkBoxGraphics.DrawLine(expandLineShadowPen, 
						3,
						image.Height / 2 + 1,
						image.Width - 4,
						image.Height / 2 + 1);
					checkBoxGraphics.DrawLine(expandLineShadowPen, 
						3,
						image.Height / 2 - 1,
						image.Width - 4,
						image.Height / 2 - 1);
					if (checkState == CheckBoxState.PlusDisabled ||
						checkState == CheckBoxState.PlusHot ||
						checkState == CheckBoxState.PlusNormal ||
						checkState == CheckBoxState.PlusPressed)
					{
						checkBoxGraphics.DrawLine(expandLineShadowPen, 
							image.Width / 2 + 1,
							3,
							image.Width / 2 + 1,
							image.Height - 4);
						checkBoxGraphics.DrawLine(expandLineShadowPen, 
							image.Width / 2 - 1,
							3,
							image.Width / 2 - 1,
							image.Height - 4);
					}
					// Plus
					checkBoxGraphics.DrawLine(expandLinePen, 
						3,
						image.Height / 2,
						image.Width - 4,
						image.Height / 2);
					if (checkState == CheckBoxState.PlusDisabled ||
						checkState == CheckBoxState.PlusHot ||
						checkState == CheckBoxState.PlusNormal ||
						checkState == CheckBoxState.PlusPressed)
					{
						checkBoxGraphics.DrawLine(expandLinePen, 
							image.Width / 2,
							3,
							image.Width / 2,
							image.Height - 4);
					}
				}
				else
				{
					CheckBoxRenderer.DrawCheckBox(checkBoxGraphics, Point.Empty, (System.Windows.Forms.VisualStyles.CheckBoxState)checkState);
				}
			}
			checkBoxImages[checkState] = image;
		}
		private void InitExpandNode(ExpandNodeState expandState)
		{
			if (expandNodeImages != null && expandNodeImages.ContainsKey(expandState)) return;
			if (expandNodeImages == null) expandNodeImages = new Dictionary<ExpandNodeState,Bitmap>();

			Bitmap image = null;
			switch (expandState)
			{
				case ExpandNodeState.OpenedDisabled:	image = Resources.ExpandNodeOpenedDisabled;	break;
				case ExpandNodeState.OpenedNormal:		image = Resources.ExpandNodeOpenedNormal;	break;
				case ExpandNodeState.OpenedHot:			image = Resources.ExpandNodeOpenedHot;		break;
				case ExpandNodeState.OpenedPressed:		image = Resources.ExpandNodeOpenedPressed;	break;
				case ExpandNodeState.ClosedDisabled:	image = Resources.ExpandNodeClosedDisabled;	break;
				case ExpandNodeState.ClosedNormal:		image = Resources.ExpandNodeClosedNormal;	break;
				case ExpandNodeState.ClosedHot:			image = Resources.ExpandNodeClosedHot;		break;
				case ExpandNodeState.ClosedPressed:		image = Resources.ExpandNodeClosedPressed;	break;
			}
			expandNodeImages[expandState] = image;
		}
	}

	public class IconImage
	{
		private	Image		sourceImage	= null;
		private	Bitmap[]	images		= new Bitmap[4];
		
		public Image SourceImage
		{
			get { return this.sourceImage; }
		}
		public Image Passive
		{
			get { return this.images[0]; }
		}
		public Image Normal
		{
			get { return this.images[1]; }
		}
		public Image Active
		{
			get { return this.images[2]; }
		}
		public Image Disabled
		{
			get { return this.images[3]; }
		}

		public int Width
		{
			get { return this.sourceImage.Width; }
		}
		public int Height
		{
			get { return this.sourceImage.Height; }
		}
		public Size Size
		{
			get { return this.sourceImage.Size; }
		}

		public IconImage(Image source)
		{
			this.sourceImage = source;

			// Generate specific images
			var imgAttribs = new System.Drawing.Imaging.ImageAttributes();
			System.Drawing.Imaging.ColorMatrix colorMatrix = null;
			{
				colorMatrix = new System.Drawing.Imaging.ColorMatrix(new float[][] {
					new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
					new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
					new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
					new float[] {0.0f, 0.0f, 0.0f, 0.65f, 0.0f},
					new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}});
				imgAttribs.SetColorMatrix(colorMatrix);
				this.images[0] = new Bitmap(source.Width, source.Height);
				using (Graphics g = Graphics.FromImage(this.images[0]))
				{
					g.DrawImage(source, 
						new Rectangle(Point.Empty, source.Size), 
						0, 0, source.Width, source.Height, GraphicsUnit.Pixel, 
						imgAttribs);
				}
			}
			{
				colorMatrix = new System.Drawing.Imaging.ColorMatrix(new float[][] {
					new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
					new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
					new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
					new float[] {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
					new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}});
				imgAttribs.SetColorMatrix(colorMatrix);
				this.images[1] = new Bitmap(source.Width, source.Height);
				using (Graphics g = Graphics.FromImage(this.images[1]))
				{
					g.DrawImage(source, 
						new Rectangle(Point.Empty, source.Size), 
						0, 0, source.Width, source.Height, GraphicsUnit.Pixel, 
						imgAttribs);
				}
			}
			{
				colorMatrix = new System.Drawing.Imaging.ColorMatrix(new float[][] {
					new float[] {1.3f, 0.0f, 0.0f, 0.0f, 0.0f},
					new float[] {0.0f, 1.3f, 0.0f, 0.0f, 0.0f},
					new float[] {0.0f, 0.0f, 1.3f, 0.0f, 0.0f},
					new float[] {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
					new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}});
				imgAttribs.SetColorMatrix(colorMatrix);
				this.images[2] = new Bitmap(source.Width, source.Height);
				using (Graphics g = Graphics.FromImage(this.images[2]))
				{
					g.DrawImage(source, 
						new Rectangle(Point.Empty, source.Size), 
						0, 0, source.Width, source.Height, GraphicsUnit.Pixel, 
						imgAttribs);
				}
			}
			{
				colorMatrix = new System.Drawing.Imaging.ColorMatrix(new float[][] {
					new float[] {0.34f, 0.34f, 0.34f, 0.0f, 0.0f},
					new float[] {0.34f, 0.34f, 0.34f, 0.0f, 0.0f},
					new float[] {0.34f, 0.34f, 0.34f, 0.0f, 0.0f},
					new float[] {0.0f, 0.0f, 0.0f, 0.5f, 0.0f},
					new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}});
				imgAttribs.SetColorMatrix(colorMatrix);
				this.images[3] = new Bitmap(source.Width, source.Height);
				using (Graphics g = Graphics.FromImage(this.images[3]))
				{
					g.DrawImage(source, 
						new Rectangle(Point.Empty, source.Size), 
						0, 0, source.Width, source.Height, GraphicsUnit.Pixel, 
						imgAttribs);
				}
			}
		}
	}

	public static class ExtMethodsSystemColor
	{
		public static Color ScaleBrightness(this Color c, float ratio)
		{
			return Color.FromArgb(c.A,
				(byte)Math.Min(Math.Max((float)c.R * ratio, 0.0f), 255.0f),
				(byte)Math.Min(Math.Max((float)c.G * ratio, 0.0f), 255.0f),
				(byte)Math.Min(Math.Max((float)c.B * ratio, 0.0f), 255.0f));
		}
		public static Color MixWith(this Color c, Color other, float ratio, bool lockBrightness = false)
		{
			float myRatio = 1.0f - ratio;
			if (lockBrightness)
			{
				int oldBrightness = Math.Max(c.R, Math.Max(c.G, c.B));
				int newBrightness = Math.Max(other.R, Math.Max(other.G, other.B));
				other = other.ScaleBrightness((float)oldBrightness / (float)newBrightness);
			}
			return Color.FromArgb(c.A,
				(byte)Math.Min(Math.Max((float)c.R * myRatio + (float)other.R * ratio, 0.0f), 255.0f),
				(byte)Math.Min(Math.Max((float)c.G * myRatio + (float)other.G * ratio, 0.0f), 255.0f),
				(byte)Math.Min(Math.Max((float)c.B * myRatio + (float)other.B * ratio, 0.0f), 255.0f));
		}
	}
}
