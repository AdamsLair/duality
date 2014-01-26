using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Resources;

using DualityEditor;
using DualityEditor.CorePluginInterface;


namespace EditorBase.DataConverters
{
	public class GameObjFromPrefab : DataConverter
	{
		public override int Priority
		{
			get { return CorePluginRegistry.Priority_Specialized; }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateObj) && 
				convert.Data.ContainsContentRefs<Prefab>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			if (convert.Data.ContainsContentRefs<Prefab>())
			{
				ContentRef<Prefab>[] dropdata = convert.Data.GetContentRefs<Prefab>();

				// Instantiate Prefabs
				foreach (ContentRef<Prefab> pRef in dropdata)
				{
					if (convert.IsObjectHandled(pRef.Res)) continue;
					if (!pRef.IsAvailable) continue;
					GameObject newObj = pRef.Res.Instantiate();
					if (newObj != null)
					{
						convert.AddResult(newObj);
						convert.MarkObjectHandled(pRef.Res);
					}
				}
			}
			// Don't finish convert operation - other converters miht contribute to the new GameObjects!
			return false; 
		}
	}
	public class GameObjFromComponents : DataConverter
	{
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
				foreach (Type t in Component.GetRequiredComponents(cmpType))
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
	public class ComponentFromSound : DataConverter
	{
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
	public class ComponentFromMaterial : DataConverter
	{
		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateObj) && 
				convert.CanPerform<Material>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			List<object> results = new List<object>();
			List<Material> availData = convert.Perform<Material>().ToList();

			// Generate objects
			foreach (Material mat in availData)
			{
				if (convert.IsObjectHandled(mat)) continue;
				Texture mainTex = mat.MainTexture.Res;
				Pixmap basePixmap = (mainTex != null) ? mainTex.BasePixmap.Res : null;
				GameObject gameobj = convert.Result.OfType<GameObject>().FirstOrDefault();

				if (mainTex == null || basePixmap == null || basePixmap.AnimFrames == 0)
				{
					SpriteRenderer sprite = convert.Result.OfType<SpriteRenderer>().FirstOrDefault();
					if (sprite == null && gameobj != null) sprite = gameobj.GetComponent<SpriteRenderer>();
					if (sprite == null) sprite = new SpriteRenderer();
					sprite.SharedMaterial = mat;
					if (mainTex != null) sprite.Rect = Rect.AlignCenter(0.0f, 0.0f, mainTex.PixelWidth, mainTex.PixelHeight);
					convert.SuggestResultName(sprite, mat.Name);
					results.Add(sprite);
				}
				else
				{
					AnimSpriteRenderer sprite = convert.Result.OfType<AnimSpriteRenderer>().FirstOrDefault();
					if (sprite == null && gameobj != null) sprite = gameobj.GetComponent<AnimSpriteRenderer>();
					if (sprite == null) sprite = new AnimSpriteRenderer();
					sprite.SharedMaterial = mat;
					sprite.Rect = Rect.AlignCenter(
						0.0f, 
						0.0f, 
						(mainTex.PixelWidth / basePixmap.AnimCols) - basePixmap.AnimFrameBorder * 2, 
						(mainTex.PixelHeight / basePixmap.AnimRows) - basePixmap.AnimFrameBorder * 2);
					sprite.AnimDuration = 5.0f;
					sprite.AnimFrameCount = basePixmap.AnimFrames;
					convert.SuggestResultName(sprite, mat.Name);
					results.Add(sprite);
				}

				convert.MarkObjectHandled(mat);
			}

			convert.AddResult(results);
			return false;
		}
	}
	public class ComponentFromFont : DataConverter
	{
		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateObj) && 
				convert.CanPerform<Font>();
		}
		public override bool Convert(ConvertOperation convert)
		{
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
	public class PrefabFromGameObject : DataConverter
	{
		public override int Priority
		{
			get { return CorePluginRegistry.Priority_Specialized; }
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
					prefab.SourcePath = draggedObj.Name; // Dummy "source path" that may be used as indicator where to save the Resource later.

					// Mark GameObject as handled
					convert.MarkObjectHandled(draggedObj);						
					convert.AddResult(prefab);
					finishConvertOp = true;
				}
			}

			return finishConvertOp;
		}
	}
	public class BatchInfoFromMaterial : DataConverter
	{
		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.Convert) && 
				convert.CanPerform<Material>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;

			List<Material> availData = convert.Perform<Material>().ToList();

			// Append objects
			foreach (Material mat in availData)
			{
				if (convert.IsObjectHandled(mat)) continue;

				convert.AddResult(mat.Info);
				finishConvertOp = true;
				convert.MarkObjectHandled(mat);
			}

			return finishConvertOp;
		}
	}
	public class MaterialFromBatchInfo : DataConverter
	{
		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes) && 
				convert.CanPerform<BatchInfo>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;
			List<BatchInfo> availData = convert.Perform<BatchInfo>().ToList();

			// Generate objects
			foreach (BatchInfo info in availData)
			{
				if (convert.IsObjectHandled(info)) continue;

				// Auto-Generate Material
				string matName = "Material";
				if (!info.MainTexture.IsExplicitNull) matName = info.MainTexture.FullName;
				string matPath = PathHelper.GetFreePath(matName, Material.FileExt);
				Material mat = new Material(info);
				mat.Save(matPath);

				convert.AddResult(mat);
				finishConvertOp = true;
				convert.MarkObjectHandled(info);
			}

			return finishConvertOp;
		}
	}
	public class MaterialFromTexture : DataConverter
	{
		public override bool CanConvertFrom(ConvertOperation convert)
		{
			if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
			{
				return convert.CanPerform<Texture>();
			}
			
			if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.Convert))
			{
				List<Texture> availTex = convert.Perform<Texture>(ConvertOperation.Operation.Convert).ToList();
				return availTex.Any(t => this.FindMatch(t) != null);
			}

			return false;
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;
			List<Texture> availData = convert.Perform<Texture>().ToList();

			// Generate objects
			foreach (Texture baseRes in availData)
			{
				if (convert.IsObjectHandled(baseRes)) continue;

				// Find target Resource matching the source - or create one.
				Material targetRes = this.FindMatch(baseRes);
				if (targetRes == null && convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
				{
					targetRes = Material.CreateFromTexture(baseRes).Res;
				}

				if (targetRes == null) continue;
				convert.AddResult(targetRes);
				finishConvertOp = true;
				convert.MarkObjectHandled(baseRes);
			}

			return finishConvertOp;
		}
		private Material FindMatch(Texture baseRes)
		{
			if (baseRes == null)
			{
				return null;
			}
			else if (baseRes.IsDefaultContent)
			{
				var defaultContent = ContentProvider.GetDefaultContent<Resource>();
				return defaultContent.Res().OfType<Material>().FirstOrDefault(r => r.MainTexture == baseRes);
			}
			else
			{
				// First try a direct approach
				string targetPath = baseRes.FullName + Material.FileExt;
				Material match = ContentProvider.RequestContent<Material>(targetPath).Res;
				if (match != null) return match;
				
				// If that fails, search for other matches
				string targetName = baseRes.Name + Material.FileExt;
				List<string> resFilePaths = Resource.GetResourceFiles();
				var resNameMatch = resFilePaths.Where(p => Path.GetFileName(p) == targetName);
				var resQuery = resNameMatch.Concat(resFilePaths).Distinct();
				foreach (string resFile in resQuery)
				{
					if (!resFile.EndsWith(Material.FileExt)) continue;
					match = ContentProvider.RequestContent<Material>(resFile).Res;
					if (match != null && match.MainTexture == baseRes) return match;
				}

				// Give up.
				return null;
			}
		}
	}
	public class TextureFromMaterial : DataConverter
	{
		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.Convert) && 
				convert.CanPerform<Material>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;

			List<Material> availData = convert.Perform<Material>().ToList();

			// Append objects
			foreach (Material baseRes in availData)
			{
				if (convert.IsObjectHandled(baseRes)) continue;
				if (!baseRes.MainTexture.IsAvailable) continue;

				convert.AddResult(baseRes.MainTexture.Res);
				finishConvertOp = true;
				convert.MarkObjectHandled(baseRes);
			}

			return finishConvertOp;
		}
	}
	public class TextureFromPixmap : DataConverter
	{
		public override bool CanConvertFrom(ConvertOperation convert)
		{
			if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
			{
				return convert.CanPerform<Pixmap>();
			}
			
			if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.Convert))
			{
				List<Pixmap> availData = convert.Perform<Pixmap>(ConvertOperation.Operation.Convert).ToList();
				return availData.Any(t => this.FindMatch(t) != null);
			}

			return false;
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;
			List<Pixmap> availData = convert.Perform<Pixmap>().ToList();

			// Generate objects
			foreach (Pixmap baseRes in availData)
			{
				if (convert.IsObjectHandled(baseRes)) continue;

				// Find target Resource matching the source - or create one.
				Texture targetRes = this.FindMatch(baseRes);
				if (targetRes == null && convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
				{
					targetRes = Texture.CreateFromPixmap(baseRes).Res;
				}

				if (targetRes == null) continue;
				convert.AddResult(targetRes);
				finishConvertOp = true;
				convert.MarkObjectHandled(baseRes);
			}

			return finishConvertOp;
		}
		private Texture FindMatch(Pixmap baseRes)
		{
			if (baseRes == null)
			{
				return null;
			}
			else if (baseRes.IsDefaultContent)
			{
				var defaultContent = ContentProvider.GetDefaultContent<Resource>();
				return defaultContent.Res().OfType<Texture>().FirstOrDefault(r => r.BasePixmap == baseRes);
			}
			else
			{
				// First try a direct approach
				string targetPath = baseRes.FullName + Texture.FileExt;
				Texture match = ContentProvider.RequestContent<Texture>(targetPath).Res;
				if (match != null) return match;

				// If that fails, search for other matches
				string targetName = baseRes.Name + Texture.FileExt;
				List<string> resFilePaths = Resource.GetResourceFiles();
				var resNameMatch = resFilePaths.Where(p => Path.GetFileName(p) == targetName);
				var resQuery = resNameMatch.Concat(resFilePaths).Distinct();
				foreach (string resFile in resQuery)
				{
					if (!resFile.EndsWith(Texture.FileExt)) continue;
					match = ContentProvider.RequestContent<Texture>(resFile).Res;
					if (match != null && match.BasePixmap == baseRes) return match;
				}

				// Give up.
				return null;
			}
		}
	}
	public class PixmapFromTexture : DataConverter
	{
		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.Convert) && 
				convert.CanPerform<Texture>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;

			List<Texture> availData = convert.Perform<Texture>().ToList();

			// Append objects
			foreach (Texture baseRes in availData)
			{
				if (convert.IsObjectHandled(baseRes)) continue;
				if (!baseRes.BasePixmap.IsAvailable) continue;

				convert.AddResult(baseRes.BasePixmap.Res);
				finishConvertOp = true;
				convert.MarkObjectHandled(baseRes);
			}

			return finishConvertOp;
		}
	}
	public class SoundFromAudioData : DataConverter
	{
		public override bool CanConvertFrom(ConvertOperation convert)
		{
			if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
			{
				return convert.CanPerform<AudioData>();
			}
			
			if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.Convert))
			{
				List<AudioData> availData = convert.Perform<AudioData>(ConvertOperation.Operation.Convert).ToList();
				return availData.Any(t => this.FindMatch(t) != null);
			}

			return false;
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;
			List<AudioData> availData = convert.Perform<AudioData>().ToList();
			List<AudioData> createDataSource = null;

			// Match objects
			foreach (AudioData baseRes in availData)
			{
				if (convert.IsObjectHandled(baseRes)) continue;

				// Find target Resource matching the source
				Sound targetRes = this.FindMatch(baseRes);
				if (targetRes != null)
				{
					convert.AddResult(targetRes);
				}
				else if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
				{
					if (createDataSource == null) createDataSource = new List<AudioData>();
					createDataSource.Add(baseRes);
				}
				else
				{
					// Can't handle this AudioData
					continue;
				}

				finishConvertOp = true;
				convert.MarkObjectHandled(baseRes);
			}

			// Create objects
			if (createDataSource != null)
			{
				List<ContentRef<Sound>> createdSounds = Sound.CreateMultipleFromAudioData(createDataSource.Ref());
				foreach (ContentRef<Sound> sound in createdSounds)
					convert.AddResult(sound.Res);
			}

			return finishConvertOp;
		}
		private Sound FindMatch(AudioData baseRes)
		{
			if (baseRes == null)
			{
				return null;
			}
			else if (baseRes.IsDefaultContent)
			{
				var defaultContent = ContentProvider.GetDefaultContent<Resource>();
				return defaultContent.Res().OfType<Sound>().FirstOrDefault(r => r.Data != null && r.Data.Count == 1 && r.MainData == baseRes);
			}
			else
			{
				// First try a direct approach
				string targetPath = baseRes.FullName + Sound.FileExt;
				Sound match = ContentProvider.RequestContent<Sound>(targetPath).Res;
				if (match != null) return match;

				// If that fails, search for other matches
				string targetName = baseRes.Name + Sound.FileExt;
				List<string> resFilePaths = Resource.GetResourceFiles();
				var resNameMatch = resFilePaths.Where(p => Path.GetFileName(p) == targetName);
				var resQuery = resNameMatch.Concat(resFilePaths).Distinct();
				List<Sound> matchCandidates = new List<Sound>();
				foreach (string resFile in resQuery)
				{
					if (!resFile.EndsWith(Sound.FileExt)) continue;
					match = ContentProvider.RequestContent<Sound>(resFile).Res;
					if (match != null && match.Data != null && match.Data.Contains(baseRes)) matchCandidates.Add(match);
				}
				// Found some matches? Return the narrowest one
				if (matchCandidates.Count > 0)
				{
					return matchCandidates.OrderBy(m => m.Data.Count).First();
				}

				// Give up.
				return null;
			}
		}
	}
	public class AudioDataFromSound : DataConverter
	{
		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.Convert) && 
				convert.CanPerform<Sound>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;

			List<Sound> availData = convert.Perform<Sound>().ToList();

			// Append objects
			foreach (Sound baseRes in availData)
			{
				if (convert.IsObjectHandled(baseRes)) continue;
				if (baseRes.Data != null)
				{
					for (int i = 0; i < baseRes.Data.Count; i++)
					{
						if (!baseRes.Data[i].IsAvailable) continue;
						convert.AddResult(baseRes.Data[i].Res);
						finishConvertOp = true;
					}
				}
				convert.MarkObjectHandled(baseRes);
			}

			return finishConvertOp;
		}
	}
}
