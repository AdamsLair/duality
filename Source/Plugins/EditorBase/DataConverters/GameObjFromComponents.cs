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
	public class GameObjFromComponents : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(GameObject); }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateObj) && 
				convert.CanPerform<Component>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			List<Component> availData = convert.Perform<Component>().ToList();
			availData.Sort((Component a, Component b) => a.RequiresComponent(b.GetType()) ? 1 : 0);

			// Generate objects
			foreach (Component cmp in availData)
			{
				if (convert.IsObjectHandled(cmp)) continue;
				Type cmpType = cmp.GetType();

				// Create or retrieve GameObject
				GameObject gameObj = null;
				{
					// First try to get one from the resultset that has an open slot for this kind of Component
					if (gameObj == null)
						gameObj = convert.Result.OfType<GameObject>().FirstOrDefault(g => g.GetComponent(cmpType) == null);
					// Still none? Create a new GameObject
					if (gameObj == null)
					{
						gameObj = new GameObject();

						// Come up with a suitable name
						string nameSuggestion = null;
						{
							// Be open for suggestions
							if (nameSuggestion == null)
								nameSuggestion = convert.TakeSuggestedResultName(cmp);
							// Use a standard name
							if (nameSuggestion == null)
								nameSuggestion = cmpType.Name;
						}

						gameObj.Name = nameSuggestion;
					}
				}

				// Make sure all requirements are met
				foreach (Type t in Component.GetRequiredComponentsToCreate(gameObj, cmpType))
					gameObj.AddComponent(t);

				// Make sure no other Component of this Type is already added
				gameObj.RemoveComponent(cmpType);

				// Add Component
				gameObj.AddComponent(cmp.GameObj == null ? cmp : cmp.Clone());

				convert.AddResult(gameObj);
				convert.MarkObjectHandled(cmp);
			}

			return false;
		}
	}
}
