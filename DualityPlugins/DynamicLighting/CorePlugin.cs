using Duality;
using Duality.Resources;

namespace Duality.Plugins.DynamicLighting
{
	/// <summary>
	/// Defines the dynamic lighting core plugin.
	/// </summary>
    public class DynamicLightingCorePlugin : CorePlugin
	{
		protected override void InitPlugin()
		{
			base.InitPlugin();
			VertexC1P3T2A4.vertexTypeIndex = DrawTechnique.RequestVertexTypeIndex(typeof(VertexC1P3T2A4).Name);
			VertexC1P3T4A4A1.vertexTypeIndex = DrawTechnique.RequestVertexTypeIndex(typeof(VertexC1P3T4A4A1).Name);
		}
		protected override void OnDisposePlugin()
		{
			base.OnDisposePlugin();
			DrawTechnique.ReleaseVertexTypeIndex(typeof(VertexC1P3T2A4).Name);
			DrawTechnique.ReleaseVertexTypeIndex(typeof(VertexC1P3T4A4A1).Name);
		}
	}
}
