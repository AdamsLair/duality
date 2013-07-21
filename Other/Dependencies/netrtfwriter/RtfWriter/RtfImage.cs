using System;
using System.Configuration;
using System.Text;
using System.Drawing;
using System.IO;
using Image = System.Drawing.Image;

namespace DW.RtfWriter
{
	/// <summary>
	/// Summary description for RtfImage
	/// </summary>
	public class RtfImage : RtfBlock
	{
		private string _imgFname;
		private ImageFileType _imgType;
		private Align _alignment;
		private Margins _margins;
		private float _width;
		private float _height;
		private bool _keepAspectRatio;
		private string _blockHead;
		private string _blockTail;
		private bool _startNewPage;

		internal RtfImage(string fileName, ImageFileType type)
		{
			_imgFname = fileName;
			_imgType = type;
			_alignment = Align.None;
			_margins = new Margins();
			_keepAspectRatio = true;
			_blockHead = @"{\pard";
			_blockTail = @"\par}";
			_startNewPage = false;
			
			Image image = Image.FromFile(fileName);
			_width = (image.Width / image.HorizontalResolution) * 72;
			_height = (image.Height / image.VerticalResolution) * 72;
		}

		public override Align Alignment
		{
			get
			{
				return _alignment;
			}
			set
			{
				_alignment = value;
			}
		}

		public override Margins Margins
		{
			get
			{
				return _margins;
			}
		}

		public override bool StartNewPage
		{
			get
			{
				return _startNewPage;
			}
			set
			{
				_startNewPage = value;
			}
		}

		public float Width
		{
			get
			{
				return _width;
			}
			set
			{
				if (_keepAspectRatio && _width > 0) {
					float ratio = _height / _width;
					_height = value * ratio;
				}
				_width = value;
			}
		}

		public float Heigth
		{
			get
			{
				return _height;
			}
			set
			{
				if (_keepAspectRatio && _height > 0) {
					float ratio = _width / _height;
					_width = value * ratio;
				}
				_height = value;
			}
		}
		
		public bool KeepAspectRatio
		{
			get
			{
				return _keepAspectRatio;
			}
			set
			{
				_keepAspectRatio = value;
			}
		}

		public override RtfCharFormat DefaultCharFormat
		{
			// DefaultCharFormat is meaningless for RtfImage.
			get
			{
				return null;
			}
		}

		private string extractImage()
		{
			Byte[] bin = File.ReadAllBytes(_imgFname);
			StringBuilder result = new StringBuilder();

			for (int i = 0; i < bin.Length; i++) {
				if (i != 0 && i % 60 == 0) {
					result.AppendLine();
				}
				result.AppendFormat("{0:x2}", bin[i]);
			}
			return result.ToString();
		}

		internal override string BlockHead
		{
			set
			{
				_blockHead = value;
			}
		}

		internal override string BlockTail
		{
			set
			{
				_blockTail = value;
			}
		}

		internal override string render()
		{
			StringBuilder result = new StringBuilder(_blockHead);

			if (_startNewPage) {
				result.Append(@"\pagebb");
			}

			if (_margins[Direction.Top] >= 0) {
				result.Append(@"\sb" + RtfUtility.pt2Twip(_margins[Direction.Top]));
			}
			if (_margins[Direction.Bottom] >= 0) {
				result.Append(@"\sa" + RtfUtility.pt2Twip(_margins[Direction.Bottom]));
			}
			if (_margins[Direction.Left] >= 0) {
				result.Append(@"\li" + RtfUtility.pt2Twip(_margins[Direction.Left]));
			}
			if (_margins[Direction.Right] >= 0) {
				result.Append(@"\ri" + RtfUtility.pt2Twip(_margins[Direction.Right]));
			}
			switch (_alignment) {
			case Align.Left:
				result.Append(@"\ql");
				break;
			case Align.Right:
				result.Append(@"\qr");
				break;
			case Align.Center:
				result.Append(@"\qc");
				break;
			}
			result.AppendLine();

			result.Append(@"{\*\shppict{\pict");
			if (_imgType == ImageFileType.Jpg) {
				result.Append(@"\jpegblip");
			} else if (_imgType == ImageFileType.Png || _imgType == ImageFileType.Gif) {
				result.Append(@"\pngblip");
			} else {
				throw new Exception("Image type not supported.");
			}
			if (_height > 0) {
				result.Append(@"\pichgoal" + RtfUtility.pt2Twip(_height));
			}
			if (_width > 0) {
				result.Append(@"\picwgoal" + RtfUtility.pt2Twip(_width));
			}
			result.AppendLine();
			
			result.AppendLine(extractImage());
			result.AppendLine("}}");
			result.AppendLine(_blockTail);
			return result.ToString();
		}
	}
}