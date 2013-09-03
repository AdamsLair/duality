using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Text;
using System;

using OpenTK;

using Duality.Resources;
using Duality.ColorFormat;
using Duality.VertexFormat;

namespace Duality.Profiling
{
	public abstract class ProfileCounter
	{
		public const int ValueHistoryLen = 300;

		// Management
		private		string			path		= null;
		private		string			name		= null;
		private		string			parentName	= null;
		private		ProfileCounter	parent		= null;
		private		bool			singleVal	= false;
		// Measurement
		protected	bool			used		= true;
		protected	bool			lastUsed	= true;
		protected	int				sampleCount	= 0;


		public string Name
		{
			get { return this.name; }
			set
			{
				this.path = value;
				this.name = Path.GetFileName(value);
				this.parentName = Path.GetDirectoryName(value);
			}
		}
		public string FullName
		{
			get { return this.path; }
		}
		public string DisplayName
		{
			get { return this.Parent == null ? this.FullName : this.Name; }
		}
		public ProfileCounter Parent
		{
			get
			{
				if (this.parent == null && !string.IsNullOrEmpty(this.parentName))
				{
					this.parent = Profile.GetCounter<ProfileCounter>(this.parentName);
				}
				return this.parent;
			}
		}
		public int ParentDepth
		{
			get
			{
				return 
					this.Parent == null ? 
					0 : 
					1 + this.Parent.ParentDepth;
			}
		}
		public bool WasUsed
		{
			get { return this.lastUsed; }
		}
		public bool IsSingleValue
		{
			get { return this.singleVal; }
			set { this.singleVal = value; }
		}


		public abstract void Reset();
		public abstract void GetReportData(out ReportCounterData data, ReportOptions options);
		protected virtual void OnFrameTick() {}

		internal void TickFrame()
		{
			this.OnFrameTick();
		}
	}
}
