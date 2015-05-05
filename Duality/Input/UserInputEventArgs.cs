using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	public class UserInputEventArgs : EventArgs
	{
		private IUserInput inputChannel;

		public IUserInput InputChannel
		{
			get { return this.inputChannel; }
		}

		public UserInputEventArgs(IUserInput inputChannel)
		{
			this.inputChannel = inputChannel;
		}
	}
}
