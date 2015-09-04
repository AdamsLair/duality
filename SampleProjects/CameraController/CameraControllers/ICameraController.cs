using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Input;
using Duality.Components;
using Duality.Drawing;

namespace CameraController
{
	/// <summary>
	/// This is a public interface that allows this sample project to
	/// communicate with all its different CameraController implementations,
	/// without knowing them personally. If you just want to pick one specific
	/// CameraController for your project, you don't need this interface at all.
	/// Just remove it.
	/// </summary>
	public interface ICameraController
	{
		/// <summary>
		/// [GET / SET] The object which is followed by this controller.
		/// </summary>
		GameObject TargetObject { get; set; }
	}
}
