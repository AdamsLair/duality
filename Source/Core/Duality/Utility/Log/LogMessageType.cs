using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Duality
{
	/// <summary>
	/// The type of a log message / entry.
	/// </summary>
	public enum LogMessageType
	{
		/// <summary>
		/// Just a regular message. Nothing special. Neutrally informs about what's going on.
		/// </summary>
		Message,
		/// <summary>
		/// A warning message. It informs about unexpected data or behaviour that might not have caused any errors yet, but can lead to them.
		/// It might also be used for expected errors from which Duality is likely to recover.
		/// </summary>
		Warning,
		/// <summary>
		/// An error message. It informs about an unexpected and/or critical error that has occurred.
		/// </summary>
		Error
	}
}
