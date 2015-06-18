using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Resources;

namespace Duality.Drawing
{
	/// <summary>
	/// Specifies how a text fitting algorithm works.
	/// </summary>
	public enum FitTextMode
	{
		/// <summary>
		/// Text is fit by character, i.e. can be separated anywhere.
		/// </summary>
		ByChar,
		/// <summary>
		/// Text is fit <see cref="ByWord">by word</see>, preferring leading whitespaces.
		/// </summary>
		ByWordLeadingSpace,
		/// <summary>
		/// Text is fit <see cref="ByWord">by word</see>, preferring trailing whitespaces.
		/// </summary>
		ByWordTrailingSpace,
		/// <summary>
		/// Text is fit by word boundaries, i.e. can only be separated between words.
		/// </summary>
		ByWord = ByWordTrailingSpace
	}
}
