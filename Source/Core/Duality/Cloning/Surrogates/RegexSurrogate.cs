using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Duality.Cloning.Surrogates
{
	public class RegexSurrogate : CloneSurrogate<Regex>
	{
		protected override bool IsImmutableTarget
		{
			get { return true; }
		}
		public override void CreateTargetObject(Regex source, ref Regex target, ICloneTargetSetup setup)
		{
			// Since Regex patterns can't be modified after creation, we'll always
			// need to set up a new one as part of the copy operation.
			if (source == null)
				target = null;
			else
				target = new Regex(source.ToString(), source.Options, source.MatchTimeout);
		}
		public override void SetupCloneTargets(Regex source, Regex target, ICloneTargetSetup setup)
		{
			// Nothing to investigate or set up
		}
		public override void CopyDataTo(Regex source, Regex target, ICloneOperation operation)
		{
			// Nothing to copy.
		}
	}
}
