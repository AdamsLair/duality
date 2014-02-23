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
				else if (fixedTypeId.EndsWith("Duality.VisibilityFlag"))				resolveTypeError.ResolvedType = typeof(Duality.Drawing.VisibilityFlag);
				else if (fixedTypeId.EndsWith("Duality.VertexMode"))					resolveTypeError.ResolvedType = typeof(Duality.Drawing.VertexMode);
				else if (fixedTypeId.EndsWith("Duality.RenderMatrix"))					resolveTypeError.ResolvedType = typeof(Duality.Drawing.RenderMatrix);
				else if (fixedTypeId.EndsWith("Duality.PerspectiveMode"))				resolveTypeError.ResolvedType = typeof(Duality.Drawing.PerspectiveMode);
				else if (fixedTypeId.EndsWith("Duality.DashPattern"))					resolveTypeError.ResolvedType = typeof(Duality.Drawing.DashPattern);
				else if (fixedTypeId.EndsWith("Duality.ClearFlag"))						resolveTypeError.ResolvedType = typeof(Duality.Drawing.ClearFlag);
				else if (fixedTypeId.EndsWith("Duality.BlendMode"))						resolveTypeError.ResolvedType = typeof(Duality.Drawing.BlendMode);
				else if (fixedTypeId.EndsWith("Duality.Canvas"))						resolveTypeError.ResolvedType = typeof(Duality.Drawing.Canvas);
				else if (fixedTypeId.EndsWith("Duality.CanvasState"))					resolveTypeError.ResolvedType = typeof(Duality.Drawing.CanvasState);
				else if (fixedTypeId.EndsWith("Duality.CanvasBuffer"))					resolveTypeError.ResolvedType = typeof(Duality.Drawing.CanvasBuffer);
				else if (fixedTypeId.EndsWith("Duality.DrawDevice"))					resolveTypeError.ResolvedType = typeof(Duality.Drawing.DrawDevice);
				else if (fixedTypeId.EndsWith("Duality.IDrawDevice"))					resolveTypeError.ResolvedType = typeof(Duality.Drawing.IDrawDevice);
				else if (fixedTypeId.EndsWith("Duality.Profiling.Profile"))				resolveTypeError.ResolvedType = typeof(Duality.Profile);
				else if (fixedTypeId.EndsWith("Duality.Profiling.ProfileCounter"))		resolveTypeError.ResolvedType = typeof(Duality.ProfileCounter);
				else if (fixedTypeId.EndsWith("Duality.Profiling.ReportCounterData"))	resolveTypeError.ResolvedType = typeof(Duality.ProfileReportCounterData);
				else if (fixedTypeId.EndsWith("Duality.Profiling.ReportOptions"))		resolveTypeError.ResolvedType = typeof(Duality.ProfileReportOptions);
				else if (fixedTypeId.EndsWith("Duality.Profiling.StatCounter"))			resolveTypeError.ResolvedType = typeof(Duality.StatCounter);
				else if (fixedTypeId.EndsWith("Duality.Profiling.TimeCounter"))			resolveTypeError.ResolvedType = typeof(Duality.TimeCounter);
				else if (fixedTypeId.Contains("Duality.FormattedText") && !fixedTypeId.Contains("Duality.Drawing.FormattedText"))
				{
					resolveTypeError.ResolvedType = ReflectionHelper.ResolveType(
						fixedTypeId.Replace("Duality.FormattedText", "Duality.Drawing.FormattedText"), 
						false);
				}
			}

			return;
		}
	}
}
