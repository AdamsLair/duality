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
	public class ComponentFromFont : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(TextRenderer); }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateObj) && 
				convert.CanPerform<Font>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			// If we already have a renderer in the result set, consider generating
			// another one to be not the right course of action.
			if (convert.Result.OfType<ICmpRenderer>().Any())
				return false;

			List<object> results = new List<object>();
			List<Font> availData = convert.Perform<Font>().ToList();

			// Generate objects
			foreach (Font font in availData)
			{
				if (convert.IsObjectHandled(font)) continue;

				GameObject gameobj = convert.Result.OfType<GameObject>().FirstOrDefault();
				TextRenderer renderer = convert.Result.OfType<TextRenderer>().FirstOrDefault();
				if (renderer == null && gameobj != null) renderer = gameobj.GetComponent<TextRenderer>();
				if (renderer == null) renderer = new TextRenderer();
				convert.SuggestResultName(renderer, font.Name);
					
				if (!renderer.Text.Fonts.Contains(font))
				{
					var fonts = renderer.Text.Fonts.ToList();
					if (fonts[0] == Font.GenericMonospace10) fonts.RemoveAt(0);
					fonts.Add(font);
					renderer.Text.Fonts = fonts.ToArray();
					renderer.Text.ApplySource();
				}

				results.Add(renderer);
				convert.MarkObjectHandled(font);
			}

			convert.AddResult(results);
			return false;
		}
	}
}
