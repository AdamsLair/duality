using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Text;
using System;

using OpenTK;

using Duality.Resources;
using Duality.Drawing;

namespace Duality
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
		private		bool			hasData		= false;
		protected	int				sampleCount	= 0;


		/// <summary>
		/// [GET / SET] The counters individual name, without any ancestor names included.
		/// </summary>
		public string Name
		{
			get { return this.name; }
			set { this.FullName = Path.Combine(this.parentName, value); }
		}
		/// <summary>
		/// [GET / SET] The counters full name, including possibly existing parent counters.
		/// </summary>
		public string FullName
		{
			get { return this.path; }
			set
			{
				this.path = value;
				this.name = Path.GetFileName(value);
				this.parentName = Path.GetDirectoryName(value);
			}
		}
		/// <summary>
		/// [GET] The name that this ProfileCounter uses for value display.
		/// </summary>
		public string DisplayName
		{
			get { return this.Parent == null ? this.FullName : this.Name; }
		}
		/// <summary>
		/// [GET] This counters parent counter.
		/// </summary>
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
		/// <summary>
		/// [GET] Returns the depths of this counters <see cref="Parent">ancestry</see>.
		/// </summary>
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
		/// <summary>
		/// [GET] Returns whether this ProfileCounter was used last frame.
		/// </summary>
		public bool WasUsed
		{
			get { return this.lastUsed; }
		}
		/// <summary>
		/// [GET] Returns whether this ProfileCounter has gathered and processed any usable data yet.
		/// </summary>
		public bool HasData
		{
			get { return this.hasData; }
		}
		/// <summary>
		/// [GET / SET] Specifies whether this is a single value counter that doesn't need any MinMax processing.
		/// </summary>
		public bool IsSingleValue
		{
			get { return this.singleVal; }
			set { this.singleVal = value; }
		}


		/// <summary>
		/// Resets the counters frame-local measurement values.
		/// </summary>
		public abstract void Reset();
		/// <summary>
		/// Gathers ProfileCounter data for generating a profile report.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="options"></param>
		public abstract void GetReportData(out ProfileReportCounterData data, ProfileReportOptions options);
		protected virtual void OnFrameTick() {}

		internal void TickFrame()
		{
			this.OnFrameTick();
			if (this.lastUsed) this.hasData = true;
		}
	}
}
