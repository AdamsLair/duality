using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;


namespace Duality.Editor.Plugins.Base.DataConverters
{
	public class GameObjFromPrefab : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(GameObject); }
		}
		public override int Priority
		{
			get { return PrioritySpecialized; }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateObj) && 
				convert.Data.ContainsContentRefs(typeof(Prefab));
		}
		public override bool Convert(ConvertOperation convert)
		{
			IContentRef[] dropData;
			if (convert.Data.TryGetContentRefs(typeof(Prefab), out dropData))
			{
				// Instantiate Prefabs
				foreach (IContentRef contentRef in dropData)
				{
					ContentRef<Prefab> prefabRef = contentRef.As<Prefab>();

					if (convert.IsObjectHandled(prefabRef.Res)) continue;
					if (!prefabRef.IsAvailable) continue;

					GameObject newObj = prefabRef.Res.Instantiate();
					if (newObj != null)
					{
						convert.AddResult(newObj);
						convert.MarkObjectHandled(prefabRef.Res);
					}
				}
			}
			// Don't finish convert operation - other converters miht contribute to the new GameObjects!
			return false; 
		}
	}
}
