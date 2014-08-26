using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Duality.Cloning;

[assembly: CloneBehavior(typeof(MemberInfo), CloneMode.Reference)]
