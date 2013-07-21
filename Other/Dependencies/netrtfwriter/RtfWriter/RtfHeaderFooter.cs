using System;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Collections.Generic;

namespace DW.RtfWriter
{
	/// <summary>
	/// Summary description for RtfHeaderFooter
	/// </summary>
	public class RtfHeaderFooter : RtfBlockList
	{
		private Hashtable _magicWords;
		private HeaderFooterType _type;
		
		internal RtfHeaderFooter(HeaderFooterType type)
			: base(true, false, true, true, false)
		{
			_magicWords = new Hashtable();
			_type = type;
		}

		internal new string render()
		{
			StringBuilder result = new StringBuilder();

			if (_type == HeaderFooterType.Header) {
				result.AppendLine(@"{\header");
			} else if (_type == HeaderFooterType.Footer) {
				result.AppendLine(@"{\footer");
			} else {
				throw new Exception("Invalid HeaderFooterType");
			}
			result.AppendLine();
			for (int i = 0; i < base._blocks.Count; i++) {
				if (base._defaultCharFormat != null
						&& ((RtfBlock)base._blocks[i]).DefaultCharFormat != null) {
					((RtfBlock)base._blocks[i]).DefaultCharFormat.copyFrom(base._defaultCharFormat);
				}
				result.AppendLine(((RtfBlock)_blocks[i]).render());
			}
			result.AppendLine("}");
			return result.ToString();
		}		
	}
}