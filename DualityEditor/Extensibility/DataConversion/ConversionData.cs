using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Duality;

namespace Duality.Editor
{
	public class ConversionData : IDataObject
	{
		private	IDataObject		data		= null;
		private	DataObject		dataCache	= new DataObject();

		public ConversionData(IDataObject data)
		{
			this.data = data;
		}

		public object GetData(Type format)
		{
			if (!this.dataCache.GetDataPresent(format) && this.data.GetDataPresent(format))
				this.dataCache.SetData(format, this.data.GetData(format));
			return this.dataCache.GetData(format);
		}
		public object GetData(string format)
		{
			if (!this.dataCache.GetDataPresent(format) && this.data.GetDataPresent(format))
				this.dataCache.SetData(format, this.data.GetData(format));
			return this.dataCache.GetData(format);
		}
		public object GetData(string format, bool autoConvert)
		{
			if (!this.dataCache.GetDataPresent(format) && this.data.GetDataPresent(format, autoConvert))
				this.dataCache.SetData(format, this.data.GetData(format, autoConvert));
			return this.dataCache.GetData(format);
		}

		public bool GetDataPresent(Type format)
		{
			return this.data.GetDataPresent(format);
		}
		public bool GetDataPresent(string format)
		{
			return this.data.GetDataPresent(format);
		}
		public bool GetDataPresent(string format, bool autoConvert)
		{
			return this.data.GetDataPresent(format, autoConvert);
		}

		string[] IDataObject.GetFormats()
		{
			return this.data.GetFormats();
		}
		string[] IDataObject.GetFormats(bool autoConvert)
		{
			return this.data.GetFormats(autoConvert);
		}

		void IDataObject.SetData(object data)
		{
			throw new NotImplementedException();
		}
		void IDataObject.SetData(Type format, object data)
		{
			throw new NotImplementedException();
		}
		void IDataObject.SetData(string format, object data)
		{
			throw new NotImplementedException();
		}
		void IDataObject.SetData(string format, bool autoConvert, object data)
		{
			throw new NotImplementedException();
		}
	}
}
