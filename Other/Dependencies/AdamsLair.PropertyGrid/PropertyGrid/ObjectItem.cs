using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdamsLair.PropertyGrid
{
	public class ObjectItem
	{
		private object	value	= null;
		private string	caption	= null;

		public string Caption
		{
			get { return this.caption; }
		}
		public object Value
		{
			get { return this.value; }
		}

		public ObjectItem(object v, string c)
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
