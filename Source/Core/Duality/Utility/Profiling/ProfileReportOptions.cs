﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality
{
	[Flags]
	public enum ProfileReportOptions
	{
		None            = 0x00,

		LastValue       = 0x01,
		AverageValue    = 0x02,
		SampleCount     = 0x04,
		MinValue        = 0x08,
		MaxValue        = 0x10,

		GroupHeader     = 0x1000,
		Header          = 0x2000,
		FormattedText   = 0x4000,
		OmitMinorValues = 0x8000
	}
}
