using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Animation
{
	public interface IAnimation
	{
		object Target { get; }
		IAnimationTrack Track { get; }
		float Time { get; set; }
		object CurrentValue { get; }
		
        T GetCurrentValue<T>();
		void UpdateAnimatedValue();
	}
}
