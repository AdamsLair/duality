using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality.EditorHints;
using Duality.Resources;
using Duality.VertexFormat;
using Duality.ColorFormat;

using OpenTK;


namespace Duality
{
	/// <summary>
	/// Provides functionality for analyzing, handling and displaying formatted text.
	/// </summary>
	[Serializable]
	public class FormattedText
	{
		/// <summary>
		/// Format string for displaying a slash (/) character.
		/// </summary>
		public	const	string	FormatSlash			= "//";
		/// <summary>
		/// Format string for beginning a new text element.
		/// </summary>
		public	const	string	FormatElement		= "/e";
		/// <summary>
		/// Format string for changing the current text color.
		/// </summary>
		public	const	string	FormatColorTag		= "/c";
		/// <summary>
		/// Format string for changing the current <see cref="Duality.Resources.Font"/>.
		/// </summary>
		public	const	string	FormatFont			= "/f";
		/// <summary>
		/// Format string for inserting an icon.
		/// </summary>
		public	const	string	FormatIcon			= "/i";
		/// <summary>
		/// Format string for changing the current text alignment to "Left".
		/// </summary>
		public	const	string	FormatAlignLeft		= "/al";
		/// <summary>
		/// Format string for changing the current text alignment to "Right".
		/// </summary>
		public	const	string	FormatAlignRight	= "/ar";
		/// <summary>
		/// Format string for changing the current text alignment to "Center".
		/// </summary>
		public	const	string	FormatAlignCenter	= "/ac";
		/// <summary>
		/// Format string for inserting a line break.
		/// </summary>
		public	const	string	FormatNewline		= "/n";
		/// <summary>
		/// Returns a format string for changing the current text color to the specified one.
		/// </summary>
		/// <returns></returns>
		public static string FormatColor(ColorRgba color)
		{
			int intClr = color.ToIntRgba();
			return string.Format("{1}{0:X8}", intClr, FormatColorTag);
		}


		/// <summary>
		/// Represents an element of a formatted text.
		/// </summary>
		[Serializable] public abstract class Element {}
		/// <summary>
		/// Contains a text string.
		/// </summary>
		[Serializable] public class TextElement : Element
		{
			private	string	text;
			/// <summary>
			/// [GET] text string this element contains.
			/// </summary>
			public string Text
			{
				get { return this.text; }
			}
			public TextElement(string text)
			{
				this.text = text;
			}
		}
		/// <summary>
		/// Contains an icon.
		/// </summary>
		[Serializable] public class IconElement : Element
		{
			private	int	iconIndex;
			/// <summary>
			/// [GET] The index of the icon to display at this elements position.
			/// </summary>
			public int IconIndex
			{
				get { return this.iconIndex; }
			}
			public IconElement(int icon)
			{
				this.iconIndex = icon;
			}
		}
		/// <summary>
		/// Forces a line break at this position.
		/// </summary>
		[Serializable] public class NewLineElement : Element
		{
		}
		/// <summary>
		/// Changes the currently used <see cref="Duality.Resources.Font"/>.
		/// </summary>
		[Serializable] public class FontChangeElement : Element
		{
			private	int	fontIndex;
			/// <summary>
			/// [GET] The index of the Font to switch to.
			/// </summary>
			public int FontIndex
			{
				get { return this.fontIndex; }
			}
			public FontChangeElement(int font)
			{
				this.fontIndex = font;
			}
		}
		/// <summary>
		/// Changes the currently used <see cref="Duality.ColorFormat.ColorRgba"/>.
		/// </summary>
		[Serializable] public class ColorChangeElement : Element
		{
			private ColorRgba color;
			/// <summary>
			/// [GET] The new color to use.
			/// </summary>
			public ColorRgba Color
			{
				get { return this.color; }
			}
			public ColorChangeElement(ColorRgba color)
			{
				this.color = color;
			}
		}
		/// <summary>
		/// Changes the current lines' alignment. May be <see cref="Alignment.Left"/>, <see cref="Alignment.Right"/> or <see cref="Alignment.Center"/>.
		/// </summary>
		[Serializable] public class AlignChangeElement : Element
		{
			private	Alignment align;
			/// <summary>
			/// [GET] The alignment to use for the current line.
			/// </summary>
			public Alignment Align
			{
				get { return this.align; }
			}
			public AlignChangeElement(Alignment align)
			{
				this.align = align;
			}
		}

		/// <summary>
		/// An icon that can be displayed inside the formatted text.
		/// </summary>
		[Serializable] public struct Icon
		{
			/// <summary>
			/// The icons UV-Coordinates on the icon texture that will be used for rendering icons.
			/// </summary>
			public	Rect	uvRect;
			/// <summary>
			/// The icons display size.
			/// </summary>
			public	Vector2	size;

			public Icon(Rect uvRect, Vector2 size)
			{
				this.uvRect = uvRect;
				this.size = size;
			}
		}
		/// <summary>
		/// An rectangular area that will be avoided by the text flow.
		/// </summary>
		[Serializable] public struct FlowArea
		{
			/// <summary>
			/// The areas width.
			/// </summary>
			public	int		width;
			/// <summary>
			/// The areas height.
			/// </summary>
			public	int		height;
			/// <summary>
			/// The areas y-Coordinate.
			/// </summary>
			public	int		y;
			/// <summary>
			/// Whether the area is located at the right edge of the text area, instead of the left edge.
			/// </summary>
			public	bool	alignRight;

			public FlowArea(int y, int height, int width, bool alignRight)
			{
				this.y = y;
				this.height = height;
				this.width = width;
				this.alignRight = alignRight;
			}
		}
		/// <summary>
		/// Provides information about a formatted texts metrics.
		/// </summary>
		public class Metrics
		{
			private	Vector2		size;
			private	IList<Rect>	lineBounds;
			private	IList<Rect>	elementBounds;

			/// <summary>
			/// [GET] The size of the formatted text block as whole.
			/// </summary>
			public Vector2 Size 
			{
				get { return this.size; }
			}
			/// <summary>
			/// [GET] The number of lines.
			/// </summary>
			public int LineCount
			{
				get { return this.lineBounds.Count; }
			}
			/// <summary>
			/// [GET] Each lines boundary.
			/// </summary>
			public IList<Rect> LineBounds
			{
				get { return this.lineBounds; }
			}
			/// <summary>
			/// [GET] Each formatted text elements individual boundary.
			/// </summary>
			public IList<Rect> ElementBounds
			{
				get { return this.elementBounds; }
			}

			public Metrics(Vector2 size, List<Rect> lineBounds, List<Rect> elementBounds)
			{
				this.size = size;
				this.lineBounds = lineBounds.AsReadOnly();
				this.elementBounds = elementBounds.AsReadOnly();
			}
		}
		/// <summary>
		/// Describes word wrap behaviour.
		/// </summary>
		public enum WrapMode
		{
			/// <summary>
			/// Word wrap is allowed at each glyph.
			/// </summary>
			Glyph,
			/// <summary>
			/// Word wrap is allowed after / before each word, but not in the middle of one.
			/// </summary>
			Word,
			/// <summary>
			/// Word wrap is only allowed between two separate formatted text elements.
			/// </summary>
			Element
		}

		private class RenderState
		{
			// General state data
			private	FormattedText	parent;
			private	int[]			vertTextIndex;
			private	int				vertIconIndex;
			private	int				elemIndex;
			private	int				lineIndex;
			private	Vector2			offset;
			// Format state
			private	int				fontIndex;
			private	Font			font;
			private	ColorRgba		color;
			// Line stats
			private	float			lineBeginX;
			private	float			lineAvailWidth;
			private	float			lineWidth;
			private	float			lineHeight;
			private	int				lineBaseLine;
			private	Alignment		lineAlign;
			// Current element data. Current == just 'been processed in NextElement()
			private	Vector2			curElemOffset;
			private	int				curElemVertTextIndex;
			private	int				curElemVertIconIndex;
			private	int				curElemWrapIndex;
			private	string			curElemText;


			public int FontIndex
			{
				get { return this.fontIndex; }
			}
			public Font Font
			{
				get { return this.font; }
			}
			public ColorRgba Color
			{
				get { return this.color; }
			}
			public int LineBaseLine
			{
				get { return this.lineBaseLine; }
			}
			public int CurrentElemIndex
			{
				get { return this.elemIndex - 1; }
			}
			public int CurrentLineIndex
			{
				get { return this.lineIndex; }
			}
			public Vector2 CurrentElemOffset
			{
				get { return this.curElemOffset; }
			}
			public int CurrentElemTextVertexIndex
			{
				get { return this.curElemVertTextIndex; }
			}
			public int CurrentElemIconVertexIndex
			{
				get { return this.curElemVertIconIndex; }
			}
			public string CurrentElemText
			{
				get { return this.curElemText; }
			}


			public RenderState(FormattedText parent)
			{
				this.parent = parent;
				this.vertTextIndex = new int[this.parent.fonts != null ? this.parent.fonts.Length : 0];
				this.font = (this.parent.fonts != null && this.parent.fonts.Length > 0) ? this.parent.fonts[0].Res : null;
				this.color = ColorRgba.White;
				this.lineAlign = parent.lineAlign;

				this.PeekLineStats();
				this.offset.X = this.lineBeginX;
			}
			public RenderState(RenderState other)
			{
				this.parent = other.parent;
				this.vertTextIndex = other.vertTextIndex.Clone() as int[];
				this.vertIconIndex = other.vertIconIndex;
				this.offset = other.offset;
				this.elemIndex = other.elemIndex;
				this.lineIndex = other.lineIndex;

				this.fontIndex = other.fontIndex;
				this.font = other.font;
				this.color = other.color;

				this.lineBeginX = other.lineBeginX;
				this.lineWidth = other.lineWidth;
				this.lineAvailWidth = other.lineAvailWidth;
				this.lineHeight = other.lineHeight;
				this.lineBaseLine = other.lineBaseLine;
				this.lineAlign = other.lineAlign;

				this.curElemOffset = other.curElemOffset;
				this.curElemVertTextIndex = other.curElemVertTextIndex;
				this.curElemVertIconIndex = other.curElemVertIconIndex;
				this.curElemWrapIndex = other.curElemWrapIndex;
				this.curElemText = other.curElemText;
			}
			public RenderState Clone()
			{
				return new RenderState(this);
			}

			public Element NextElement(bool stopAtLineBreak = false)
			{
				if (this.elemIndex >= this.parent.elements.Length) return null;
				Element elem = this.parent.elements[this.elemIndex];

				if (elem is TextElement && this.font != null)
				{
					TextElement textElem = elem as TextElement;

					string textToDisplay;
					string fittingText;
					// Word wrap by glyph / word
					if (this.parent.maxWidth > 0 && this.parent.wrapMode != WrapMode.Element)
					{
						Font.FitTextMode fitMode = Resources.Font.FitTextMode.ByChar;
						if (this.parent.wrapMode == WrapMode.Word)
							fitMode = (this.lineAlign == Alignment.Right) ? Font.FitTextMode.ByWordLeadingSpace : Font.FitTextMode.ByWordTrailingSpace;
						textToDisplay = textElem.Text.Substring(this.curElemWrapIndex, textElem.Text.Length - this.curElemWrapIndex);
						fittingText = this.font.FitText(textToDisplay, this.lineAvailWidth - (this.offset.X - this.lineBeginX), fitMode);

						// If by-word results in instant line break: Do it by glyph instead
						if (this.offset.X == this.lineBeginX && fittingText.Length == 0 && this.parent.wrapMode == WrapMode.Word) 
							fittingText = this.font.FitText(textToDisplay, this.lineAvailWidth - (this.offset.X - this.lineBeginX), Font.FitTextMode.ByChar);

						// If doing it by glyph results in an instant line break: Use at least one glyph anyway
						if (this.lineAvailWidth == this.parent.maxWidth && 
							this.offset.X == this.lineBeginX && 
							this.parent.maxHeight == 0 &&
							fittingText.Length == 0) fittingText = textToDisplay.Substring(0, 1);
					}
					// No word wrap (or by whole element)
					else
					{
						textToDisplay = textElem.Text;
						fittingText = textElem.Text;
					}
					Vector2 textElemSize = this.font.MeasureText(fittingText);

					// Perform word wrap by whole Element
					if (this.parent.maxWidth > 0 && this.parent.wrapMode == WrapMode.Element)
					{
						if ((this.lineAvailWidth < this.parent.maxWidth || this.offset.X > this.lineBeginX || this.parent.maxHeight > 0) && 
							this.offset.X + textElemSize.X > this.lineAvailWidth)
						{
							if (stopAtLineBreak)	return null;
							else					this.PerformNewLine();
							if (this.offset.Y + this.lineHeight > this.parent.maxHeight) return null;
						}
					}

					this.curElemText = fittingText;
					this.curElemVertTextIndex = this.vertTextIndex[this.fontIndex];
					this.curElemOffset = this.offset;

					// If it all fits: Stop wrap mode, proceed with next element
					if (fittingText.Length == textToDisplay.Length)
					{
						this.curElemWrapIndex = 0;
						this.elemIndex++;
					}
					// If only some part fits: Move wrap index & return
					else if (fittingText.Length > 0)
					{
						this.curElemWrapIndex += fittingText.Length;
					}
					// If nothing fits: Begin a new line & return
					else
					{
						if (stopAtLineBreak)	return null;
						else					this.PerformNewLine();
						if (this.parent.maxHeight != 0 && this.offset.Y + this.lineHeight > this.parent.maxHeight) return null;
					}

					this.vertTextIndex[this.fontIndex] += fittingText.Length * 4;
					this.offset.X += textElemSize.X;
					this.lineWidth += textElemSize.X;
					this.lineHeight = Math.Max(this.lineHeight, this.font.LineSpacing);
					this.lineBaseLine = Math.Max(this.lineBaseLine, this.font.BaseLine);
				}
				else if (elem is TextElement && this.font == null)
				{
					this.elemIndex++;
				}
				else if (elem is IconElement)
				{
					IconElement iconElem = elem as IconElement;
					bool iconValid = this.parent.icons != null && iconElem.IconIndex >= 0 && iconElem.IconIndex < this.parent.icons.Length;
					Icon icon = iconValid ? this.parent.icons[iconElem.IconIndex] : new Icon();

					// Word Wrap
					if (this.parent.maxWidth > 0)
					{
						while ((this.lineAvailWidth < this.parent.maxWidth || this.offset.X > this.lineBeginX || this.parent.maxHeight > 0) && 
							this.offset.X - this.lineBeginX + icon.size.X > this.lineAvailWidth)
						{
							if (stopAtLineBreak)	return null;
							else					this.PerformNewLine();
							if (this.offset.Y + this.lineHeight > this.parent.maxHeight) return null;
						}
					}

					this.curElemVertIconIndex = this.vertIconIndex;
					this.curElemOffset = this.offset;

					this.vertIconIndex += 4;
					this.offset.X += icon.size.X;
					this.lineWidth += icon.size.X;
					this.lineHeight = Math.Max(this.lineHeight, icon.size.Y);
					this.lineBaseLine = Math.Max(this.lineBaseLine, (int)Math.Round(icon.size.Y));
					this.elemIndex++;
				}
				else if (elem is FontChangeElement)
				{
					FontChangeElement fontChangeElem = elem as FontChangeElement;
					this.fontIndex = fontChangeElem.FontIndex;

					bool fontValid = this.parent.fonts != null && this.fontIndex >= 0 && this.fontIndex < this.parent.fonts.Length;
					ContentRef<Font> font = fontValid ? this.parent.fonts[this.fontIndex] : ContentRef<Font>.Null;
					this.font = font.Res;
					this.elemIndex++;
				}
				else if (elem is ColorChangeElement)
				{
					ColorChangeElement colorChangeElem = elem as ColorChangeElement;
					this.color = colorChangeElem.Color;
					this.elemIndex++;
				}
				else if (elem is AlignChangeElement)
				{
					AlignChangeElement alignChangeElem = elem as AlignChangeElement;
					this.lineAlign = alignChangeElem.Align;
					this.elemIndex++;
				}
				else if (elem is NewLineElement)
				{
					this.elemIndex++;
					if (stopAtLineBreak)	return null;
					else					this.PerformNewLine();
					if (this.parent.maxHeight != 0 && this.offset.Y + this.lineHeight > this.parent.maxHeight) return null;
				}

				return elem;
			}

			private int GetFlowAreaMinXAt(int y, int h)
			{
				if (this.parent.flowAreas == null) return 0;
				int minX = 0;
				for (int i = 0; i < this.parent.flowAreas.Length; i++)
				{
					if (this.parent.flowAreas[i].alignRight) continue;
					if (y + h < this.parent.flowAreas[i].y || y > this.parent.flowAreas[i].y + this.parent.flowAreas[i].height) continue;
					minX = Math.Max(minX, this.parent.flowAreas[i].width);
				}
				return minX;
			}
			private int GetFlowAreaMaxXAt(int y, int h)
			{
				if (this.parent.flowAreas == null) return this.parent.maxWidth;
				int maxX = this.parent.maxWidth;
				for (int i = 0; i < this.parent.flowAreas.Length; i++)
				{
					if (!this.parent.flowAreas[i].alignRight) continue;
					if (y + h < this.parent.flowAreas[i].y || y > this.parent.flowAreas[i].y + this.parent.flowAreas[i].height) continue;
					maxX = Math.Min(maxX, this.parent.maxWidth - this.parent.flowAreas[i].width);
				}
				return maxX;
			}
			private void PerformNewLine()
			{
				// In empty lines, initialize line height
				if (this.lineHeight == 0 && this.font != null)
					this.lineHeight = this.font.LineSpacing;
				// Advance to new line
				this.offset.Y += this.lineHeight;
				this.lineIndex++;
				// Init new line
				this.PeekLineStats();
				this.offset.X = this.lineBeginX;
			}
			private void PeekLineStats()
			{
				// First pass: Determine line width, height & base line
				this.lineBaseLine = 0; //this.font != null ? this.font.BaseLine : 0;
				this.lineHeight = 0; //this.font != null ? this.font.LineSpacing : 0;
				this.lineBeginX = 0.0f;
				this.lineWidth = 0.0f;
				this.lineAvailWidth = this.parent.maxWidth;
				this.offset.X = this.lineBeginX;

				RenderState peekState = this.Clone();
				while (peekState.NextElement(true) != null);

				this.lineBaseLine = peekState.lineBaseLine;
				this.lineWidth = peekState.lineWidth;
				this.lineHeight = peekState.lineHeight;
				this.lineAlign = peekState.lineAlign;

				// Second pass: Obey flow areas
				if (this.parent.flowAreas != null && this.parent.flowAreas.Length > 0)
				{
					this.lineBeginX = this.GetFlowAreaMinXAt((int)this.offset.Y, (int)this.lineHeight);
					this.lineAvailWidth = this.GetFlowAreaMaxXAt((int)this.offset.Y, (int)this.lineHeight) - this.lineBeginX;
					this.lineBaseLine = 0;//this.font != null ? this.font.BaseLine : 0;
					this.lineHeight = 0;//this.font != null ? this.font.LineSpacing : 0;
					this.lineWidth = 0.0f;
					this.offset.X = this.lineBeginX;

					peekState = this.Clone();
					while (peekState.NextElement(true) != null);

					this.lineBaseLine = peekState.lineBaseLine;
					this.lineWidth = peekState.lineWidth;
					this.lineHeight = peekState.lineHeight;
					this.lineAlign = peekState.lineAlign;
				}

				// Apply alignment
				if (this.parent.maxWidth != 0)
				{
					if (this.lineAlign == Alignment.Right)
						this.lineBeginX += this.lineAvailWidth - this.lineWidth;
					else if (this.lineAlign == Alignment.Center)
						this.lineBeginX += (this.lineAvailWidth - this.lineWidth) / 2;

					this.lineBeginX = MathF.Round(this.lineBeginX);
				}
			}
		}


		private	string				sourceText		= null;
		private	Icon[]				icons			= null;
		private	FlowArea[]			flowAreas		= null;
		private	ContentRef<Font>[]	fonts			= new ContentRef<Font>[] { Font.GenericMonospace10 };
		private	int					maxWidth		= 0;
		private	int					maxHeight		= 0;
		private	WrapMode			wrapMode		= WrapMode.Word;
		private	Alignment			lineAlign		= Alignment.Left;

		private	string				displayedText	= null;
		private	int[]				fontGlyphCount	= null;
		private	int					iconCount		= 0;
		private	Element[]			elements		= null;

		[NonSerialized] private bool				updateVertexCache	= true;
		[NonSerialized] private VertexC1P3T2[][]	vertTextCache		= null;
		[NonSerialized] private VertexC1P3T2[]		vertIconsCache		= null;
		[NonSerialized] private int[]				vertCountCache		= null;
		[NonSerialized] private	Metrics				metricsCache		= null;


		/// <summary>
		/// [GET / SET] The source text that is used, containing all format strings as well as the displayed text.
		/// </summary>
		public string SourceText
		{
			get { return this.sourceText; }
			set
			{
				if (value == null) value = "";
				if (this.sourceText != value)
				{
					this.ApplySource(value);
				}
			}
		}
		/// <summary>
		/// [GET] Returns whether the text is empty.
		/// </summary>
		public bool IsEmpty
		{
			get { return string.IsNullOrEmpty(this.sourceText); }
		}
		/// <summary>
		/// [GET / SET] A set of icons that is available in the text.
		/// If you modify this value without re-assigning it, be sure to call <see cref="UpdateVertexCache"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		public Icon[] Icons
		{
			get { return this.icons; }
			set { this.icons = value; this.updateVertexCache = true; }
		}
		/// <summary>
		/// [GET / SET] A set of flow areas to be considered in word wrap.
		/// If you modify this value without re-assigning it, be sure to call <see cref="UpdateVertexCache"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		public FlowArea[] FlowAreas
		{
			get { return this.flowAreas; }
			set { this.flowAreas = value; this.updateVertexCache = true; }
		}
		/// <summary>
		/// [GET / SET] A set of <see cref="Duality.Resources.Font">Fonts</see> that is available in the text.
		/// If you modify this value without re-assigning it, be sure to call <see cref="UpdateVertexCache"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		public ContentRef<Font>[] Fonts
		{
			get { return this.fonts; }
			set
			{
				if (value == null || value.Length == 0) value = new ContentRef<Font>[] { Font.GenericMonospace10 };
				this.fonts = value;
				this.updateVertexCache = true;
			}
		}
		/// <summary>
		/// [GET / SET] The maximum width of the displayed text block. Zero deactivates maximum width.
		/// </summary>
		public int MaxWidth
		{
			get { return this.maxWidth; }
			set { this.maxWidth = value; this.updateVertexCache = true; }
		}
		/// <summary>
		/// [GET / SET] The maximum height of the displayed text block. Zero deactivates maximum height.
		/// </summary>
		public int MaxHeight
		{
			get { return this.maxHeight; }
			set { this.maxHeight = value; this.updateVertexCache = true; }
		}
		/// <summary>
		/// [GET / SET] The word wrapping behaviour to use.
		/// </summary>
		public WrapMode WordWrap
		{
			get { return this.wrapMode; }
			set { this.wrapMode = value; this.updateVertexCache = true; }
		}
		/// <summary>
		/// [GET / SET] Specifies the default horizontal alignment of each line, unless changed by format tags.
		/// </summary>
		public Alignment LineAlign
		{
			get { return this.lineAlign; }
			set
			{
				value = value & (Alignment.Left | Alignment.Right);
				if (this.lineAlign != value)
				{
					this.lineAlign = value;
					this.updateVertexCache = true;
				}
			}
		}
		/// <summary>
		/// [GET] The text blocks metrics.
		/// </summary>
		public Metrics TextMetrics
		{
			get
			{
				this.ValidateVertexCache();
				return this.metricsCache;
			}
		}
		/// <summary>
		/// [GET] The text blocks boundary size.
		/// </summary>
		public Vector2 Size
		{
			get { return this.TextMetrics.Size; }
		}
		/// <summary>
		/// [GET] The number of lines in the formatted text block.
		/// </summary>
		public int LineCount
		{
			get { return this.TextMetrics.LineCount; }
		}

		/// <summary>
		/// [GET] The text that is actually displayed.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public string DisplayedText
		{
			get { return this.displayedText; }
		}
		/// <summary>
		/// [GET] The formatted text elements that have been generated analyzing the incoming source text.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public Element[] Elements
		{
			get { return this.elements; }
		}



		public FormattedText() : this((string)null) {}
		public FormattedText(string text)
		{
			this.ApplySource(text);
		}
		public FormattedText(FormattedText other)
		{
			this.sourceText = other.sourceText;
			this.icons		= other.icons != null ? (Icon[])other.icons.Clone() : null;
			this.flowAreas	= other.flowAreas != null ? (FlowArea[])other.flowAreas.Clone() : null;
			this.fonts		= other.fonts != null ? (ContentRef<Font>[])other.fonts.Clone() : null;
			this.maxWidth	= other.maxWidth;
			this.maxHeight	= other.maxHeight;
			this.wrapMode	= other.wrapMode;
			this.lineAlign = other.lineAlign;

			this.ApplySource(this.sourceText);
		}
		/// <summary>
		/// Creates a deep copy of the FormattedText and returns it.
		/// </summary>
		/// <returns></returns>
		public FormattedText Clone()
		{
			return new FormattedText(this);
		}

		/// <summary>
		/// Applies a new source text.
		/// </summary>
		/// <param name="text">The new source text to apply. If null, the current source text is re-applied.</param>
		public void ApplySource(string text = null)
		{
			if (text == null) text = this.sourceText;

			this.updateVertexCache = true;
			this.sourceText = text;
			this.iconCount = 0;

			List<int> fontGlyphCounter = new List<int>();
			List<Element> elemList = new List<Element>();
			StringBuilder displayedTextBuilder = new StringBuilder();
			StringBuilder curTextElemTextBuilder = new StringBuilder();
			int curTextElemBegin = 0;
			int curTextElemLen = 0;
			int curFontIndex = 0;
			int srcTextLen = this.sourceText != null ? this.sourceText.Length : 0;
			for (int i = 0; i < srcTextLen; i++)
			{
				if (this.sourceText[i] == '/' && i + 1 < this.sourceText.Length)
				{
					i++;

					curTextElemTextBuilder.Append(this.sourceText, curTextElemBegin, curTextElemLen);

					if (this.sourceText[i] == '/')
					{
						curTextElemTextBuilder.Append('/');
					}
					else
					{
						if (curTextElemTextBuilder.Length > 0)
						{
							string textElem = curTextElemTextBuilder.ToString();
							elemList.Add(new TextElement(textElem));
							displayedTextBuilder.Append(textElem);
							curTextElemTextBuilder.Clear();

							while (fontGlyphCounter.Count <= curFontIndex) fontGlyphCounter.Add(0);
							fontGlyphCounter[curFontIndex] += textElem.Length;
						}

						if (this.sourceText[i] == 'c')
						{
							if (this.sourceText.Length > i + 8)
							{
								int	clr;
								string	clrString = new StringBuilder().Append(this.sourceText, i + 1, 8).ToString();
								if (int.TryParse(clrString, System.Globalization.NumberStyles.HexNumber, System.Globalization.NumberFormatInfo.InvariantInfo, out clr))
									elemList.Add(new ColorChangeElement(ColorRgba.FromIntRgba(clr)));
								else
									elemList.Add(new ColorChangeElement(ColorRgba.White));

								i += 8;
							}
						}
						if (this.sourceText[i] == 'e')
						{
							// Just separates elements
						}
						else if (this.sourceText[i] == 'a')
						{
							if (this.sourceText.Length > i + 1)
							{
								string alignString = new StringBuilder().Append(this.sourceText, i + 1, 1).ToString();
								if (alignString == "l")
									elemList.Add(new AlignChangeElement(Alignment.Left));
								else if (alignString == "r")
									elemList.Add(new AlignChangeElement(Alignment.Right));
								else
									elemList.Add(new AlignChangeElement(Alignment.Center));

								i += 1;
							}
						}
						else if (this.sourceText[i] == 'f')
						{
							int indexOfClose = this.sourceText.IndexOf(']', i + 1);
							if (indexOfClose != -1)
							{
								string numStr = this.sourceText.Substring(i + 2, indexOfClose - (i + 2));
								int num;
								if (int.TryParse(numStr, out num))
									elemList.Add(new FontChangeElement(num));
								else
									elemList.Add(new FontChangeElement(0));

								curFontIndex = num;
								i += 2 + numStr.Length;
							}
						}
						else if (this.sourceText[i] == 'i')
						{
							int indexOfClose = this.sourceText.IndexOf(']', i + 1);
							if (indexOfClose != -1)
							{
								string numStr = this.sourceText.Substring(i + 2, indexOfClose - (i + 2));
								int num;
								if (int.TryParse(numStr, out num))
									elemList.Add(new IconElement(num));
								else
									elemList.Add(new IconElement(0));

								this.iconCount++;
								i += 2 + numStr.Length;
							}
						}
						else if (this.sourceText[i] == 'n')
						{
							elemList.Add(new NewLineElement());
						}

					}

					curTextElemLen = 0;
					curTextElemBegin = i + 1;
				}
				else
				{
					curTextElemLen++;
				}
			}

			if (curTextElemLen > 0) curTextElemTextBuilder.Append(this.sourceText, curTextElemBegin, curTextElemLen);
			if (curTextElemTextBuilder.Length > 0)
			{
				string textElem = curTextElemTextBuilder.ToString();
				elemList.Add(new TextElement(textElem));
				displayedTextBuilder.Append(textElem);

				while (fontGlyphCounter.Count <= curFontIndex) fontGlyphCounter.Add(0);
				fontGlyphCounter[curFontIndex] += textElem.Length;
			}

			this.fontGlyphCount = fontGlyphCounter.ToArray();
			this.displayedText = displayedTextBuilder.ToString();
			this.elements = elemList.ToArray();
		}
		/// <summary>
		/// Clears the text.
		/// </summary>
		public void Clear()
		{
			this.ApplySource("");
		}
		
		/// <summary>
		/// Emits sets of vertices for glyphs and icons based on this formatted text. To render it, use each set of vertices combined with
		/// the corresponding Fonts <see cref="Material"/>.
		/// </summary>
		/// <param name="vertText">One set of vertices for each Font that is available to this ForattedText.</param>
		/// <param name="vertIcons">A set of icon vertices.</param>
		/// <param name="x">An X-Offset applied to the position of each emitted vertex.</param>
		/// <param name="y">An Y-Offset applied to the position of each emitted vertex.</param>
		/// <param name="z">An Z-Offset applied to the position of each emitted vertex.</param>
		/// <returns>
		/// Returns an array of vertex counts for each emitted vertex array. 
		/// Index 0 represents the number of emitted icon vertices, Index n represents the number of vertices emitted using Font n - 1.
		/// </returns>
		public int[] EmitVertices(ref VertexC1P3T2[][] vertText, ref VertexC1P3T2[] vertIcons, float x, float y, float z = 0.0f)
		{
			return this.EmitVertices(ref vertText, ref vertIcons, x, y, z, ColorRgba.White);
		}
		/// <summary>
		/// Emits sets of vertices for glyphs and icons based on this formatted text. To render it, use each set of vertices combined with
		/// the corresponding Fonts <see cref="Material"/>.
		/// </summary>
		/// <param name="vertText">One set of vertices for each Font that is available to this ForattedText.</param>
		/// <param name="vertIcons">A set of icon vertices.</param>
		/// <param name="x">An X-Offset applied to the position of each emitted vertex.</param>
		/// <param name="y">An Y-Offset applied to the position of each emitted vertex.</param>
		/// <param name="clr">The color value that is applied to each emitted vertex.</param>
		/// <returns>
		/// Returns an array of vertex counts for each emitted vertex array. 
		/// Index 0 represents the number of emitted icon vertices, Index n represents the number of vertices emitted using Font n - 1.
		/// </returns>
		public int[] EmitVertices(ref VertexC1P3T2[][] vertText, ref VertexC1P3T2[] vertIcons, float x, float y, ColorRgba clr)
		{
			int[] vertLen = this.EmitVertices(ref vertText, ref vertIcons);
			
			Vector3 offset = new Vector3(x, y, 0);

			if (clr == ColorRgba.White)
			{
				for (int i = 0; i < vertText.Length; i++)
				{
					for (int j = 0; j < vertLen[i + 1]; j++)
					{
						Vector3.Add(ref vertText[i][j].Pos, ref offset, out vertText[i][j].Pos);
					}
				}
				for (int i = 0; i < vertLen[0]; i++)
				{
					Vector3.Add(ref vertIcons[i].Pos, ref offset, out vertIcons[i].Pos);
				}
			}
			else
			{
				for (int i = 0; i < vertText.Length; i++)
				{
					for (int j = 0; j < vertLen[i + 1]; j++)
					{
						Vector3.Add(ref vertText[i][j].Pos, ref offset, out vertText[i][j].Pos);
						ColorRgba.Multiply(ref vertText[i][j].Color, ref clr, out vertText[i][j].Color);
					}
				}
				for (int i = 0; i < vertLen[0]; i++)
				{
					Vector3.Add(ref vertIcons[i].Pos, ref offset, out vertIcons[i].Pos);
					ColorRgba.Multiply(ref vertIcons[i].Color, ref clr, out vertIcons[i].Color);
				}
			}

			return vertLen;
		}
		/// <summary>
		/// Emits sets of vertices for glyphs and icons based on this formatted text. To render it, use each set of vertices combined with
		/// the corresponding Fonts <see cref="Material"/>.
		/// </summary>
		/// <param name="vertText">One set of vertices for each Font that is available to this ForattedText.</param>
		/// <param name="vertIcons">A set of icon vertices.</param>
		/// <param name="x">An X-Offset applied to the position of each emitted vertex.</param>
		/// <param name="y">An Y-Offset applied to the position of each emitted vertex.</param>
		/// <param name="z">An Z-Offset applied to the position of each emitted vertex.</param>
		/// <param name="clr">The color value that is applied to each emitted vertex.</param>
		/// <param name="angle">An angle by which the text is rotated (before applying the offset).</param>
		/// <param name="scale">A factor by which the text is scaled (before applying the offset).</param>
		/// <returns>
		/// Returns an array of vertex counts for each emitted vertex array. 
		/// Index 0 represents the number of emitted icon vertices, Index n represents the number of vertices emitted using Font n - 1.
		/// </returns>
		public int[] EmitVertices(ref VertexC1P3T2[][] vertText, ref VertexC1P3T2[] vertIcons, float x, float y, float z, ColorRgba clr, float angle = 0.0f, float scale = 1.0f)
		{
			Vector2 xDot, yDot;
			MathF.GetTransformDotVec(angle, scale, out xDot, out yDot);
			return this.EmitVertices(ref vertText, ref vertIcons, x, y, z, clr, xDot, yDot);
		}
		/// <summary>
		/// Emits sets of vertices for glyphs and icons based on this formatted text. To render it, use each set of vertices combined with
		/// the corresponding Fonts <see cref="Material"/>.
		/// </summary>
		/// <param name="vertText">One set of vertices for each Font that is available to this ForattedText.</param>
		/// <param name="vertIcons">A set of icon vertices.</param>
		/// <param name="x">An X-Offset applied to the position of each emitted vertex.</param>
		/// <param name="y">An Y-Offset applied to the position of each emitted vertex.</param>
		/// <param name="z">An Z-Offset applied to the position of each emitted vertex.</param>
		/// <param name="clr">The color value that is applied to each emitted vertex.</param>
		/// <param name="xDot">Dot product base for the transformed vertices.</param>
		/// <param name="yDot">Dot product base for the transformed vertices.</param>
		/// <returns>
		/// Returns an array of vertex counts for each emitted vertex array. 
		/// Index 0 represents the number of emitted icon vertices, Index n represents the number of vertices emitted using Font n - 1.
		/// </returns>
		public int[] EmitVertices(ref VertexC1P3T2[][] vertText, ref VertexC1P3T2[] vertIcons, float x, float y, float z, ColorRgba clr, Vector2 xDot, Vector2 yDot)
		{
			int[] vertLen = this.EmitVertices(ref vertText, ref vertIcons);
			
			Vector3 offset = new Vector3(x, y, z);

			if (clr == ColorRgba.White)
			{
				for (int i = 0; i < vertText.Length; i++)
				{
					for (int j = 0; j < vertLen[i + 1]; j++)
					{
						MathF.TransformDotVec(ref vertText[i][j].Pos, ref xDot, ref yDot);
						Vector3.Add(ref vertText[i][j].Pos, ref offset, out vertText[i][j].Pos);
					}
				}
				for (int i = 0; i < vertLen[0]; i++)
				{
					MathF.TransformDotVec(ref vertIcons[i].Pos, ref xDot, ref yDot);
					Vector3.Add(ref vertIcons[i].Pos, ref offset, out vertIcons[i].Pos);
				}
			}
			else
			{
				for (int i = 0; i < vertText.Length; i++)
				{
					for (int j = 0; j < vertLen[i + 1]; j++)
					{
						MathF.TransformDotVec(ref vertText[i][j].Pos, ref xDot, ref yDot);
						Vector3.Add(ref vertText[i][j].Pos, ref offset, out vertText[i][j].Pos);
						ColorRgba.Multiply(ref vertText[i][j].Color, ref clr, out vertText[i][j].Color);
					}
				}
				for (int i = 0; i < vertLen[0]; i++)
				{
					MathF.TransformDotVec(ref vertIcons[i].Pos, ref xDot, ref yDot);
					Vector3.Add(ref vertIcons[i].Pos, ref offset, out vertIcons[i].Pos);
					ColorRgba.Multiply(ref vertIcons[i].Color, ref clr, out vertIcons[i].Color);
				}
			}

			return vertLen;
		}
		/// <summary>
		/// Emits sets of vertices for glyphs and icons based on this formatted text. To render it, use each set of vertices combined with
		/// the corresponding Fonts <see cref="Material"/>.
		/// </summary>
		/// <param name="vertText">One set of vertices for each Font that is available to this FormattedText.</param>
		/// <param name="vertIcons">A set of icon vertices.</param>
		/// <returns>
		/// Returns an array of vertex counts for each emitted vertex array. 
		/// Index 0 represents the number of emitted icon vertices, Index n represents the number of vertices emitted using Font n - 1.
		/// </returns>
		public int[] EmitVertices(ref VertexC1P3T2[][] vertText, ref VertexC1P3T2[] vertIcons)
		{
			this.ValidateVertexCache();
			
			// Allocate memory
			if (vertIcons == null || vertIcons.Length < this.vertCountCache[0]) vertIcons = new VertexC1P3T2[this.vertCountCache[0]];
			if (vertText == null || vertText.Length != this.vertTextCache.Length) vertText = new VertexC1P3T2[this.vertTextCache.Length][];
			for (int i = 0; i < this.vertTextCache.Length; i++)
			{
				if (vertText[i] == null || vertText[i].Length < this.vertCountCache[i + 1])
					vertText[i] = new VertexC1P3T2[this.vertCountCache[i + 1]];
			}

			// Copy actual data
			int[] vertLen = new int[this.vertCountCache.Length];
			Array.Copy(this.vertCountCache, vertLen, this.vertCountCache.Length);
			Array.Copy(this.vertIconsCache, vertIcons, vertLen[0]);
			for (int i = 0; i < this.vertTextCache.Length; i++)
				Array.Copy(this.vertTextCache[i], vertText[i], vertLen[i + 1]);

			return vertLen;
		}

		/// <summary>
		/// Updates the vertex cache that is used to optimize calls to <see cref="EmitVertices"/>. However, this is normally done automatically.
		/// </summary>
		public void UpdateVertexCache()
		{
			this.updateVertexCache = true;
		}
		private void ValidateVertexCache()
		{
			if (this.vertTextCache != null && 
				this.vertIconsCache != null && 
				this.vertCountCache != null &&
				this.metricsCache != null && 
				!this.updateVertexCache)
			{
				// No need to update.
				return;
			}
			this.updateVertexCache = false;

			int fontNum = this.fonts != null ? this.fonts.Length : 0;

			// Setting up buffers
			{
				int countCacheLen = 1 + fontNum;
				if (this.vertCountCache == null || this.vertCountCache.Length != countCacheLen)
					this.vertCountCache = new int[countCacheLen];

				int iconVertCount = this.iconCount * 4;
				if (this.vertIconsCache == null || this.vertIconsCache.Length < iconVertCount)
					this.vertIconsCache = new VertexC1P3T2[iconVertCount];
				this.vertCountCache[0] = iconVertCount;

				if (this.vertTextCache == null || this.vertTextCache.Length != fontNum) 
					this.vertTextCache = new VertexC1P3T2[fontNum][];
				for (int i = 0; i < this.vertTextCache.Length; i++)
				{
					int textVertCount = this.fontGlyphCount.Length > i ? this.fontGlyphCount[i] * 4 : 0;
					this.vertCountCache[i + 1] = textVertCount;
					if (this.vertTextCache[i] == null || this.vertTextCache[i].Length < textVertCount) 
						this.vertTextCache[i] = new VertexC1P3T2[textVertCount];
				}
			}

			// Rendering
			{
				RenderState state = new RenderState(this);
				Element elem;
				int[] vertTextLen = new int[fontNum];
				int vertIconLen = 0;
				while ((elem = state.NextElement()) != null)
				{
					if (elem is TextElement && state.Font != null)
					{
						TextElement textElem = elem as TextElement;
						VertexC1P3T2[] textElemVert = null;
						int count = state.Font.EmitTextVertices(
							state.CurrentElemText, 
							ref textElemVert, 
							state.CurrentElemOffset.X, 
							state.CurrentElemOffset.Y + state.LineBaseLine - state.Font.BaseLine, 
							state.Color);
						Array.Copy(textElemVert, 0, this.vertTextCache[state.FontIndex], state.CurrentElemTextVertexIndex, count);
						vertTextLen[state.FontIndex] = state.CurrentElemTextVertexIndex + count;
					}
					else if (elem is IconElement)
					{
						IconElement iconElem = elem as IconElement;
						Icon icon = iconElem.IconIndex >= 0 && iconElem.IconIndex < this.icons.Length ? this.icons[iconElem.IconIndex] : new Icon();
						Vector2 iconSize = icon.size;
						Rect iconUvRect = icon.uvRect;

						this.vertIconsCache[state.CurrentElemIconVertexIndex + 0].Pos.X = state.CurrentElemOffset.X;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 0].Pos.Y = state.CurrentElemOffset.Y + state.LineBaseLine - iconSize.Y;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 0].Pos.Z = 0;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 0].Color = state.Color;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 0].TexCoord = iconUvRect.TopLeft;

						this.vertIconsCache[state.CurrentElemIconVertexIndex + 1].Pos.X = state.CurrentElemOffset.X + iconSize.X;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 1].Pos.Y = state.CurrentElemOffset.Y + state.LineBaseLine - iconSize.Y;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 1].Pos.Z = 0;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 1].Color = state.Color;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 1].TexCoord = iconUvRect.TopRight;

						this.vertIconsCache[state.CurrentElemIconVertexIndex + 2].Pos.X = state.CurrentElemOffset.X + iconSize.X;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 2].Pos.Y = state.CurrentElemOffset.Y + state.LineBaseLine;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 2].Pos.Z = 0;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 2].Color = state.Color;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 2].TexCoord = iconUvRect.BottomRight;

						this.vertIconsCache[state.CurrentElemIconVertexIndex + 3].Pos.X = state.CurrentElemOffset.X;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 3].Pos.Y = state.CurrentElemOffset.Y + state.LineBaseLine;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 3].Pos.Z = 0;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 3].Color = state.Color;
						this.vertIconsCache[state.CurrentElemIconVertexIndex + 3].TexCoord = iconUvRect.BottomLeft;

						vertIconLen = state.CurrentElemIconVertexIndex + 4;
					}
				}

				this.vertCountCache[0] = vertIconLen;
				for (int i = 0; i < fontNum; i++)
					this.vertCountCache[i + 1] = vertTextLen[i];
			}

			// Updating the metrics cache
			{
				Vector2 size = Vector2.Zero;
				List<Rect> lineBounds = new List<Rect>(16);
				List<Rect> elementBounds = new List<Rect>(this.elements.Length);

				RenderState state = new RenderState(this);
				Element elem;
				Vector2 elemSize;
				Vector2 elemOffset;
				int lastElemIndex = -1;
				int lastLineIndex = 0;
				bool elemIndexChanged = true;
				bool lineChanged = true;
				bool hasBounds;
				while ((elem = state.NextElement()) != null)
				{
					if (elem is TextElement && state.Font != null)
					{
						TextElement textElem = elem as TextElement;
						elemSize = state.Font.MeasureText(state.CurrentElemText);
						elemOffset = new Vector2(state.CurrentElemOffset.X, state.CurrentElemOffset.Y/* + state.LineBaseLine - state.Font.Ascent*/);
						//if (elemSize.Y != 0.0f) elemSize.Y -= state.LineBaseLine - state.Font.Ascent;
					}
					else if (elem is IconElement && this.icons != null)
					{
						IconElement iconElem = elem as IconElement;
						bool iconValid = iconElem.IconIndex > 0 && iconElem.IconIndex < this.icons.Length;
						elemSize = iconValid ? this.icons[iconElem.IconIndex].size : Vector2.Zero;
						elemOffset = new Vector2(state.CurrentElemOffset.X, state.CurrentElemOffset.Y/* + state.LineBaseLine - elemSize.Y*/);
						//if (elemSize.Y != 0.0f) elemSize.Y -= state.LineBaseLine - elemSize.Y;
					}
					else
					{
						elemSize = Vector2.Zero;
						elemOffset = Vector2.Zero;
					}
					hasBounds = elemSize != Vector2.Zero;

					if (elemIndexChanged) elementBounds.Add(Rect.Empty);
					if (hasBounds && elementBounds[elementBounds.Count - 1] == Rect.Empty)
						elementBounds[elementBounds.Count - 1] = new Rect(elemOffset.X, elemOffset.Y, elemSize.X, elemSize.Y);
					else if (hasBounds)
						elementBounds[elementBounds.Count - 1] = elementBounds[elementBounds.Count - 1].ExpandToContain(elemOffset.X, elemOffset.Y, elemSize.X, elemSize.Y);
				
					if (lineChanged) lineBounds.Add(Rect.Empty);
					if (hasBounds && lineBounds[lineBounds.Count - 1] == Rect.Empty)
						lineBounds[lineBounds.Count - 1] = new Rect(elemOffset.X, elemOffset.Y, elemSize.X, elemSize.Y);
					else if (hasBounds)
						lineBounds[lineBounds.Count - 1] = lineBounds[lineBounds.Count - 1].ExpandToContain(elemOffset.X, elemOffset.Y, elemSize.X, elemSize.Y);

					size.X = Math.Max(size.X, elemOffset.X + elemSize.X);
					size.Y = Math.Max(size.Y, elemOffset.Y + elemSize.Y);

					elemIndexChanged = lastElemIndex != state.CurrentElemIndex;
					lineChanged = lastLineIndex != state.CurrentLineIndex;
					lastElemIndex = state.CurrentElemIndex;
					lastLineIndex = state.CurrentLineIndex;
				}

				this.metricsCache = new Metrics(size, lineBounds, elementBounds);
			}
		}
		
		/// <summary>
		/// Renders a text to the specified target Image.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="target"></param>
		public void RenderToBitmap(string text, System.Drawing.Image target, float x = 0.0f, float y = 0.0f, System.Drawing.Image icons = null)
		{
			using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(target))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

				// Rendering
				int fontNum = this.fonts != null ? this.fonts.Length : 0;
				RenderState state = new RenderState(this);
				Element elem;
				while ((elem = state.NextElement()) != null)
				{
					if (elem is TextElement && state.Font != null)
					{
						TextElement textElem = elem as TextElement;
						state.Font.RenderToBitmap(
							state.CurrentElemText, 
							target, 
							x + state.CurrentElemOffset.X, 
							y + state.CurrentElemOffset.Y + state.LineBaseLine - state.Font.BaseLine, 
							state.Color);
					}
					else if (elem is IconElement)
					{
						IconElement iconElem = elem as IconElement;
						Icon icon = iconElem.IconIndex >= 0 && iconElem.IconIndex < this.icons.Length ? this.icons[iconElem.IconIndex] : new Icon();
						Vector2 iconSize = icon.size;
						Rect iconUvRect = icon.uvRect;
						Vector2 dataCoord = iconUvRect.Pos * new Vector2(icons.Width, icons.Height);
						Vector2 dataSize = iconUvRect.Size * new Vector2(icons.Width, icons.Height);
						
						var attrib = new System.Drawing.Imaging.ImageAttributes();
						attrib.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(new[] {
							new[] {state.Color.R / 255.0f, 0, 0, 0},
							new[] {0, state.Color.G / 255.0f, 0, 0},
							new[] {0, 0, state.Color.B / 255.0f, 0},
							new[] {0, 0, 0, state.Color.A / 255.0f} }));
						g.DrawImage(icons,
							new System.Drawing.Rectangle(
								MathF.RoundToInt(x + state.CurrentElemOffset.X), 
								MathF.RoundToInt(y + state.CurrentElemOffset.Y + state.LineBaseLine - iconSize.Y), 
								MathF.RoundToInt(iconSize.X), 
								MathF.RoundToInt(iconSize.Y)),
							dataCoord.X, dataCoord.Y, dataSize.X, dataSize.Y,
							System.Drawing.GraphicsUnit.Pixel,
							attrib);
					}
				}
			}
		}
		/// <summary>
		/// Renders a text to the specified target Image.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="target"></param>
		public void RenderToBitmap(string text, Pixmap.Layer target, float x = 0.0f, float y = 0.0f, Pixmap.Layer icons = null)
		{
			// Rendering
			int fontNum = this.fonts != null ? this.fonts.Length : 0;
			RenderState state = new RenderState(this);
			Element elem;
			while ((elem = state.NextElement()) != null)
			{
				if (elem is TextElement && state.Font != null)
				{
					TextElement textElem = elem as TextElement;
					state.Font.RenderToBitmap(
						state.CurrentElemText, 
						target, 
						x + state.CurrentElemOffset.X, 
						y + state.CurrentElemOffset.Y + state.LineBaseLine - state.Font.BaseLine, 
						state.Color);
				}
				else if (elem is IconElement)
				{
					IconElement iconElem = elem as IconElement;
					Icon icon = iconElem.IconIndex >= 0 && iconElem.IconIndex < this.icons.Length ? this.icons[iconElem.IconIndex] : new Icon();
					Vector2 iconSize = icon.size;
					Rect iconUvRect = icon.uvRect;
					Vector2 dataCoord = iconUvRect.Pos * new Vector2(icons.Width, icons.Height);
					Vector2 dataSize = iconUvRect.Size * new Vector2(icons.Width, icons.Height);
					
					Pixmap.Layer iconLayer = icons.CloneSubImage(
						MathF.RoundToInt(dataCoord.X), 
						MathF.RoundToInt(dataCoord.Y),
						MathF.RoundToInt(dataSize.X), 
						MathF.RoundToInt(dataSize.Y));
					iconLayer.Rescale(
						MathF.RoundToInt(iconSize.X), 
						MathF.RoundToInt(iconSize.Y));
					iconLayer.DrawOnto(target,
						BlendMode.Alpha,
						MathF.RoundToInt(x + state.CurrentElemOffset.X), 
						MathF.RoundToInt(y + state.CurrentElemOffset.Y + state.LineBaseLine - iconSize.Y), 
						iconLayer.Width, 
						iconLayer.Height,
						0, 
						0,
						state.Color);

				}
			}
		}
	}
}
