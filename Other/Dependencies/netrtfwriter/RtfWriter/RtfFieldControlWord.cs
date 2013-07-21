using System;
using System.Collections.Generic;
using System.Text;

namespace DW.RtfWriter
{
	public class RtfFieldControlWord : RtfRenderable
	{
		public enum FieldType
		{
			None = 0,
			Page,
			NumPages,
			Date,
			Time,
		}
		
		private int _position;
		private FieldType _type;
		
		internal RtfFieldControlWord(int position, FieldType type)
		{
			_position = position;
			_type = type;
		}
		
		internal int Position
		{
			get {
				return _position;
			}
		}
		
		internal override string render()
		{
			string[] ControlWordPool = new string[] {
				// correspond with FiledControlWords enum
				"",
				@"{\field{\*\fldinst PAGE }}",
				@"{\field{\*\fldinst NUMPAGES }}",
				@"{\field{\*\fldinst DATE }}",
				@"{\field{\*\fldinst TIME }}"
			};
			
			return ControlWordPool[(int)_type];
		}
	}
}
