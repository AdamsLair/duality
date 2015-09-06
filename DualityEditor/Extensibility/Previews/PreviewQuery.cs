using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

using Duality;
using Duality.Resources;

namespace Duality.Editor
{
	public abstract class PreviewQuery<T> : IPreviewQuery where T : class
	{
		private		object	source	= null;
		protected	T		result	= null;

		private		object				sourceTransformed	= null;
		private		ConvertOperation	sourceConv			= null;
		
		public object OriginalSource
		{
			get { return this.source; }
		}
		public object Source
		{
			get { return this.sourceTransformed; }
		}
		public T Result
		{
			get { return this.result; }
			set { this.result = value; }
		}

		public PreviewQuery(object src)
		{
			this.source = src;
			this.sourceTransformed = this.source;
		}
		
		private void CreateConvert()
		{
			if (this.sourceConv != null) return;
			this.sourceConv = new ConvertOperation(new[] { this.source }, ConvertOperation.Operation.Convert);
		}
		bool IPreviewQuery.SourceFits(Type sourceType)
		{
			this.CreateConvert();
			return this.sourceConv.CanPerform(sourceType, ConvertOperation.Operation.None);
		}
		bool IPreviewQuery.TransformSource(Type sourceType)
		{
			this.CreateConvert();

			if (this.sourceConv.CanPerform(sourceType))
				this.sourceTransformed = this.sourceConv.Perform(sourceType).FirstOrDefault();
			else
				this.sourceTransformed = null;

			return this.sourceTransformed != null;
		}
		object IPreviewQuery.Result
		{
			get { return this.result; }
		}
	}
}
