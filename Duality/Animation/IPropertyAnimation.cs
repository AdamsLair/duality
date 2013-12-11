using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Duality.Animation
{
	/// <summary>
	/// Represents the <see cref="IAnimation">animation</see> of an objects property.
	/// </summary>
	public interface IPropertyAnimation : IAnimation
	{
		/// <summary>
		/// [GET] The animated property.
		/// </summary>
		PropertyInfo Property { get; }
	}
}
