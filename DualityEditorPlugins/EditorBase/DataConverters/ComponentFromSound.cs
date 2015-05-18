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
	public class ComponentFromSound : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(SoundEmitter); }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateObj) && 
				convert.CanPerform<Sound>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			List<Sound> availData = convert.Perform<Sound>().ToList();

			// Generate objects
			foreach (Sound snd in availData)
			{
				if (convert.IsObjectHandled(snd)) continue;

				GameObject gameobj = convert.Result.OfType<GameObject>().FirstOrDefault();
				SoundEmitter emitter = convert.Result.OfType<SoundEmitter>().FirstOrDefault();
				if (emitter == null && gameobj != null) emitter = gameobj.GetComponent<SoundEmitter>();
				if (emitter == null) emitter = new SoundEmitter();
				convert.SuggestResultName(emitter, snd.Name);
					
				SoundEmitter.Source source = new SoundEmitter.Source(snd);
				emitter.Sources.Add(source);

				convert.AddResult(emitter);
				convert.MarkObjectHandled(snd);
			}
			return false;
		}
	}
}
