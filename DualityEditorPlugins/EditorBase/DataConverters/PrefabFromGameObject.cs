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
	public class PrefabFromGameObject : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(Prefab); }
		}
		public override int Priority
		{
			get { return PrioritySpecialized; }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes) && 
				convert.Data.ContainsGameObjectRefs();
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;

			if (convert.Data.ContainsGameObjectRefs())
			{
				GameObject[] draggedObjArray = convert.Data.GetGameObjectRefs();

				// Filter out GameObjects that are children of others
				draggedObjArray = draggedObjArray.Where(o => !draggedObjArray.Any(o2 => o.IsChildOf(o2))).ToArray();

				// Generate Prefabs
				foreach (GameObject draggedObj in draggedObjArray)
				{
					if (convert.IsObjectHandled(draggedObj)) continue;

					// Create Prefab
					Prefab prefab = new Prefab(draggedObj);

					// Add a dummy "source path" that may be used as indicator where to save the Resource later.
					prefab.AssetInfo = new AssetInfo();
					prefab.AssetInfo.NameHint = draggedObj.Name;

					// Mark GameObject as handled
					convert.MarkObjectHandled(draggedObj);						
					convert.AddResult(prefab);
					finishConvertOp = true;
				}
			}

			return finishConvertOp;
		}
	}
}
