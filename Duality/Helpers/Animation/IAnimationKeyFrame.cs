using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Animation
{
	public interface IAnimationKeyFrame
	{
		float Time { get; set; }
		object Value { get; set; }

		T GetValue<T>();
		void SetValue<T>(T value);
	}
}
