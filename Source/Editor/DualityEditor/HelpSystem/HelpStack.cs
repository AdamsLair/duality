using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Drawing;
using System.Diagnostics;
using System.IO;

using Duality;

namespace Duality.Editor
{
	public class HelpStack
	{
		private	List<KeyValuePair<IHelpProvider,HelpInfo>>	stack	= new List<KeyValuePair<IHelpProvider,HelpInfo>>();

		public event EventHandler<HelpStackChangedEventArgs> ActiveHelpChanged = null;

		public HelpInfo ActiveHelp
		{
			get { return this.stack.Count > 0 ? this.stack[this.stack.Count - 1].Value : null; }
		}
		public IHelpProvider ActiveHelpProvider
		{
			get { return this.stack.Count > 0 ? this.stack[this.stack.Count - 1].Key : null; }
		}

		public void Push(IHelpProvider sender, HelpInfo info)
		{
			if (sender == null) throw new ArgumentNullException("sender");
			if (info == null) throw new ArgumentNullException("info");
			HelpInfo lastActiveHelp = this.ActiveHelp;

			stack.Add(new KeyValuePair<IHelpProvider,HelpInfo>(sender, info));
			
			if (lastActiveHelp != this.ActiveHelp)
				this.OnActiveHelpChanged(lastActiveHelp, this.ActiveHelp);
		}
		public void Pop(IHelpProvider sender)
		{
			if (sender == null) throw new ArgumentNullException("sender");
			HelpInfo lastActiveHelp = this.ActiveHelp;

			for (int i = stack.Count - 1; i >= 0; i--)
			{
				if (stack[i].Key == sender)
				{
					stack.RemoveAt(i);
					break;
				}
			}

			if (lastActiveHelp != this.ActiveHelp)
				this.OnActiveHelpChanged(lastActiveHelp, this.ActiveHelp);
		}
		public void Switch(IHelpProvider sender, HelpInfo newInfo)
		{
			if (sender == null) throw new ArgumentNullException("sender");
			if (newInfo == null) throw new ArgumentNullException("newInfo");
			HelpInfo lastActiveHelp = this.ActiveHelp;

			for (int i = stack.Count - 1; i >= 0; i--)
			{
				if (stack[i].Key == sender)
				{
					stack[i] = new KeyValuePair<IHelpProvider,HelpInfo>(sender, newInfo);
					break;
				}
			}

			if (lastActiveHelp != this.ActiveHelp)
				this.OnActiveHelpChanged(lastActiveHelp, this.ActiveHelp);
		}
		public void UpdateFromProvider(IHelpProvider oldProvider, IHelpProvider newProvider, HelpInfo info)
		{
			if (oldProvider != null) this.Pop(oldProvider);
			if (info != null) this.Push(newProvider, info);
		}
		public void UpdateFromProvider(IHelpProvider provider, HelpInfo info)
		{
			if (this.ActiveHelpProvider == provider)
			{
				if (info != null)
					this.Switch(provider, info);
				else
					this.Pop(provider);
			}
			else if (info != null)
				this.Push(provider, info);
		}

		private void OnActiveHelpChanged(HelpInfo last, HelpInfo current)
		{
			if (this.ActiveHelpChanged != null)
				this.ActiveHelpChanged(this, new HelpStackChangedEventArgs(last, current));
		}
	}
}
