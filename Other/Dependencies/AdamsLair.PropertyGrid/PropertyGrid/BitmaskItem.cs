using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdamsLair.PropertyGrid
{
	public class BitmaskItem
	{
		private ulong	value	= 0;
		private string	caption	= null;

		public string Caption
		{
			get { return this.caption; }
		}
		public ulong Value
		{
			get { return this.value; }
		}

		public BitmaskItem(ulong v, string c)
		{
			this.value = v;
			this.caption = c;
		}

		public override string ToString()
		{
			return this.caption;
		}
	}
}
