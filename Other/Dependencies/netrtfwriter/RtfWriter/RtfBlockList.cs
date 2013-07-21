using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace DW.RtfWriter
{
	/// <summary>
	/// A container for an array of content blocks. For example, a footnote
	/// is a RtfBlockList because it may contains a paragraph and an image.
	/// </summary>
	public class RtfBlockList : RtfRenderable
	{
		/// <summary>
		/// Storage for array of content blocks.
		/// </summary>
		protected List<RtfBlock> _blocks;
		/// <summary>
		/// Default character formats within this container.
		/// </summary>
		protected RtfCharFormat _defaultCharFormat;
		
		private bool _allowParagraph;
		private bool _allowFootnote;
		private bool _allowControlWord;
		private bool _allowImage;
		private bool _allowTable;
		
		/// <summary>
		/// Internal use only.
		/// Default constructor that allows containing all types of content blocks.
		/// </summary>
		internal RtfBlockList()
			: this(true, true, true, true, true)
		{
		}
		
		/// <summary>
		/// Internal use only.
		/// Constructor specifying allowed content blocks to be contained.
		/// </summary>
		/// <param name="allowParagraph">Whether an RtfParagraph is allowed.</param>
		/// <param name="allowFootnote">Whether an RtfFootnote is allowed in contained RtfParagraph.</param>
		/// <param name="allowControlWord">Whether an field control word is allowed in contained
		/// RtfParagraph.</param>
		/// <param name="allowImage">Whether RtfImage is allowed.</param>
		/// <param name="allowTable">Whether RtfTable is allowed.</param>
		internal RtfBlockList(bool allowParagraph, bool allowFootnote, bool allowControlWord,
			bool allowImage, bool allowTable)
		{
			_blocks = new List<RtfBlock>();
			_allowParagraph = allowParagraph;
			_allowFootnote = allowFootnote;
			_allowControlWord = allowControlWord;
			_allowImage = allowImage;
			_allowTable = allowTable;
			_defaultCharFormat = null;
		}
		
		/// <summary>
		/// Get default character formats within this container.
		/// </summary>
		public RtfCharFormat DefaultCharFormat
		{
			get
			{
				if (_defaultCharFormat == null) {
					_defaultCharFormat = new RtfCharFormat(-1, -1, 1);
				}
				return _defaultCharFormat;
			}
		}

		private void addBlock(RtfBlock block)
		{
			if (block != null) {
				_blocks.Add(block);
			}
		}
		
		/// <summary>
		/// Add a paragraph to this container.
		/// </summary>
		/// <returns>Paragraph being added.</returns>
		public RtfParagraph addParagraph()
		{
			if (!_allowParagraph) {
				throw new Exception("Paragraph is not allowed.");
			}
			RtfParagraph block = new RtfParagraph(_allowFootnote, _allowControlWord);
			addBlock(block);
			return block;
		}

		/// <summary>
		/// Add an image to this container.
		/// </summary>
		/// <param name="imgFname">Filename of the image.</param>
		/// <param name="imgType">File type of the image.</param>
		/// <returns>Image being added.</returns>
		public RtfImage addImage(string imgFname, ImageFileType imgType)
		{
			if (!_allowImage) {
				throw new Exception("Image is not allowed.");
			}
			RtfImage block = new RtfImage(imgFname, imgType);
			addBlock(block);
			return block;
		}
		
		/// <summary>
		/// Add an image to this container.
		/// </summary>
		/// <param name="imgFname">Filename of the image.</param>
		/// <returns>Image being added.</returns>
		public RtfImage addImage(string imgFname)
		{
			int dot = imgFname.LastIndexOf(".");
			if (dot < 0) {
				throw new Exception("Cannot determine image type from the filename extension: " 
					+ imgFname);
			}
			
			string ext = imgFname.Substring(dot + 1).ToLower();
			switch (ext)
			{
			case "jpg":
			case "jpeg":
				return addImage(imgFname, ImageFileType.Jpg);
			case "gif":
				return addImage(imgFname, ImageFileType.Gif);
			case "png":
				return addImage(imgFname, ImageFileType.Png);
			default:
				throw new Exception("Cannot determine image type from the filename extension: "
					+ imgFname);
			}
		}

		/// <summary>
		/// Add a table to this container.
		/// </summary>
		/// <param name="rowCount">Number of rows in the table.</param>
		/// <param name="colCount">Number of columns in the table.</param>
		/// <param name="horizontalWidth">Horizontabl width (in points) of the table.</param>
		/// <returns>Table begin added.</returns>
		public RtfTable addTable(int rowCount, int colCount, float horizontalWidth)
		{
			if (!_allowTable) {
				throw new Exception("Table is not allowed.");
			}
			RtfTable block = new RtfTable(rowCount, colCount, horizontalWidth);
			addBlock(block);
			return block;
		}
		
		/// <summary>
		/// Internal use only. 
		/// Transfer all content blocks to another RtfBlockList object.
		/// </summary>
		/// <param name="target">Target RtfBlockList object to transfer to.</param>
		internal void transferBlocksTo(RtfBlockList target)
		{
			for (int i = 0; i < _blocks.Count; i++) {
				target.addBlock(_blocks[i]);
			}
			_blocks.Clear();
		}

		/// <summary>
		/// Internal use only. 
		/// Emit RTF code.
		/// </summary>
		/// <returns>Resulting RTF code for this object.</returns>
		internal override string render()
		{
			StringBuilder result = new StringBuilder();
			
			result.AppendLine();
			for (int i = 0; i < _blocks.Count; i++) {
				if (_defaultCharFormat != null && _blocks[i].DefaultCharFormat != null) {
					_blocks[i].DefaultCharFormat.copyFrom(_defaultCharFormat);
				}
				result.AppendLine(_blocks[i].render());
			}
			return result.ToString();
		}
	}
}