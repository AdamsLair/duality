using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Components.Diagnostics;
using Duality.Serialization;

namespace Duality.Plugins.Compatibility
{
	public class OldNamespacesErrorHandler : SerializeErrorHandler
	{
		public override void HandleError(SerializeError error)
		{
			ResolveTypeError resolveTypeError = error as ResolveTypeError;
			if (resolveTypeError != null)
			{
				string fixedTypeId = resolveTypeError.TypeId;
				if (fixedTypeId.EndsWith("Duality.VertexFormat.VertexC1P3"))			resolveTypeError.ResolvedType = typeof(Duality.Drawing.VertexC1P3);
				else if (fixedTypeId.EndsWith("Duality.VertexFormat.VertexC1P3T2"))		resolveTypeError.ResolvedType = typeof(Duality.Drawing.VertexC1P3T2);
				else if (fixedTypeId.EndsWith("Duality.VertexFormat.VertexC1P3T4A1"))	resolveTypeError.ResolvedType = typeof(Duality.Drawing.VertexC1P3T4A1);
				else if (fixedTypeId.EndsWith("Duality.VertexFormat.IVertexData"))		resolveTypeError.ResolvedType = typeof(Duality.Drawing.IVertexData);
				else if (fixedTypeId.EndsWith("Duality.ColorFormat.ColorHsva"))			resolveTypeError.ResolvedType = typeof(Duality.Drawing.ColorHsva);
				else if (fixedTypeId.EndsWith("Duality.ColorFormat.ColorRgba"))			resolveTypeError.ResolvedType = typeof(Duality.Drawing.ColorRgba);
				else if (fixedTypeId.EndsWith("Duality.ColorFormat.IColorData"))		resolveTypeError.ResolvedType = typeof(Duality.Drawing.IColorData);
				else if (fixedTypeId.EndsWith("Duality.Profiling.Profile"))				resolveTypeError.ResolvedType = typeof(Duality.Profile);
				else if (fixedTypeId.EndsWith("Duality.Profiling.ProfileCounter"))		resolveTypeError.ResolvedType = typeof(Duality.ProfileCounter);
				else if (fixedTypeId.EndsWith("Duality.Profiling.ReportCounterData"))	resolveTypeError.ResolvedType = typeof(Duality.ProfileReportCounterData);
				else if (fixedTypeId.EndsWith("Duality.Profiling.ReportOptions"))		resolveTypeError.ResolvedType = typeof(Duality.ProfileReportOptions);
				else if (fixedTypeId.EndsWith("Duality.Profiling.StatCounter"))			resolveTypeError.ResolvedType = typeof(Duality.StatCounter);
				else if (fixedTypeId.EndsWith("Duality.Profiling.TimeCounter"))			resolveTypeError.ResolvedType = typeof(Duality.TimeCounter);
			}

			return;
		}
	}
}
