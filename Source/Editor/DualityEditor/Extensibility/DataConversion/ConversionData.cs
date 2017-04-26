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
		private	IDataObject data	  = null;
		private	DataObject  dataCache = new DataObject();

		public ConversionData(IDataObject data)
		{
			this.data = data;
		}

		public object GetData(Type format)
		{
			return (this as IDataObject).GetData(format.FullName, true);
		}
		public bool GetDataPresent(Type format)
		{
			return (this as IDataObject).GetDataPresent(format.FullName, true);
		}

		object IDataObject.GetData(string format)
		{
			return (this as IDataObject).GetData(format, true);
		}
		object IDataObject.GetData(string format, bool autoConvert)
		{
			bool isCached = 
				this.dataCache.GetDataPresent(format, autoConvert) || 
				this.dataCache.GetWrappedDataPresent(format);

			if (!isCached)
			{
				object obj;
				if (this.data.GetDataPresent(format, autoConvert))
					obj = this.data.GetData(format, autoConvert);
				else if (this.data.GetWrappedDataPresent(format))
					obj = this.data.GetWrappedData(format);
				else
					obj = null;

				if (obj != null)
					this.dataCache.SetData(format, obj);
			}

			return this.dataCache.GetData(format, autoConvert);
		}
		bool IDataObject.GetDataPresent(string format)
		{
			return (this as IDataObject).GetDataPresent(format, true);
		}
		bool IDataObject.GetDataPresent(string format, bool autoConvert)
		{
			return 
				this.data.GetDataPresent(format, autoConvert) || 
				this.data.GetWrappedDataPresent(format);
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
