using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Duality.Animation
{
	/// <summary>
	/// Represents the <see cref="IAnimation">animation</see> of an objects field.
	/// </summary>
	public interface IFieldAnimation : IAnimation
	{
		/// <summary>
		/// [GET] The animated field.
		/// </summary>
		FieldInfo Field { get; }
	}
}
