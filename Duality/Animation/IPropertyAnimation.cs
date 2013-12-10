using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Duality.Animation
{
	public interface IPropertyAnimation : IAnimation
	{
		PropertyInfo Property { get; }
	}
}
