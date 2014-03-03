using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;

using Duality.Resources;
using Duality.Properties;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.UndoRedoActions;

namespace Duality.Editor.Plugins.Base
{
	public class EditorActionPixmapToTexture : EditorSingleAction<Pixmap>
	{
		public override string Name
		{
			get { return EditorBaseRes.ActionName_CreateTexture; }
		}
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_CreateTexture; }
		}
		public override Image Icon
		{
			get { return CoreRes.IconResTexture; }
		}

		public override void Perform(Pixmap obj)
		{
			Texture.CreateFromPixmap(obj);
		}
	}
	public class EditorActionTextureToMaterial : EditorSingleAction<Texture>
	{
		public override string Name
		{
			get { return EditorBaseRes.ActionName_CreateMaterial; }
		}
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_CreateMaterial; }
		}
		public override Image Icon
		{
			get { return CoreRes.IconResMaterial; }
		}

		public override void Perform(Texture obj)
		{
			Material.CreateFromTexture(obj);
		}
	}
	public class EditorActionAudioDataToSound : EditorAction<AudioData>
	{
		public override string Name
		{
			get { return EditorBaseRes.ActionName_CreateSound; }
		}
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_CreateSound; }
		}
		public override Image Icon
		{
			get { return CoreRes.IconResSound; }
		}

		public override void Perform(IEnumerable<AudioData> objEnum)
		{
			Sound.CreateMultipleFromAudioData(objEnum.Ref());
		}
	}
	public class EditorActionShaderToProgram : EditorAction<AbstractShader>
	{
		public override string Name
		{
			get { return EditorBaseRes.ActionName_CreateShaderProgram; }
		}
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_CreateShaderProgram; }
		}
		public override Image Icon
		{
			get { return CoreRes.IconResShaderProgram; }
		}

		public override void Perform(IEnumerable<AbstractShader> shaderEnum)
		{
			List<VertexShader> vertexShaders = shaderEnum.OfType<VertexShader>().ToList();
			List<FragmentShader> fragmentShaders = shaderEnum.OfType<FragmentShader>().ToList();

			if (vertexShaders.Count == 1 && fragmentShaders.Count >= 1)
			{
				foreach (FragmentShader frag in fragmentShaders) this.CreateProgram(frag, vertexShaders[0]);
			}
			else if (fragmentShaders.Count == 1 && vertexShaders.Count >= 1)
			{
				foreach (VertexShader vert in vertexShaders) this.CreateProgram(fragmentShaders[0], vert);
			}
			else
			{
				for (int i = 0; i < MathF.Max(vertexShaders.Count, fragmentShaders.Count); i++)
				{
					this.CreateProgram(
						i < fragmentShaders.Count ? fragmentShaders[i] : null, 
						i < vertexShaders.Count ? vertexShaders[i] : null);
				}
			}
		}
		private void CreateProgram(FragmentShader frag, VertexShader vert)
		{
			AbstractShader refShader = (vert != null) ? (AbstractShader)vert : (AbstractShader)frag;

			string nameTemp = refShader.Name;
			string dirTemp = Path.GetDirectoryName(refShader.Path);
			if (nameTemp.Contains("Shader"))
				nameTemp = nameTemp.Replace("Shader", "Program");
			else if (nameTemp.Contains("Shader"))
				nameTemp = nameTemp.Replace("shader", "program");
			else
				nameTemp += "Program";

			string programPath = PathHelper.GetFreePath(Path.Combine(dirTemp, nameTemp), ShaderProgram.FileExt);
			ShaderProgram program = new ShaderProgram(vert, frag);
			program.Save(programPath);
		}
	}
	public class EditorActionOpenResource : EditorSingleAction<Resource>
	{
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_OpenResourceExternal; }
		}

		public override void Perform(Resource obj)
		{
			Pixmap			pixmap		= obj as Pixmap;
			AudioData		audioData	= obj as AudioData;
			AbstractShader	shader		= obj as AbstractShader;
			if (pixmap		!= null)		FileImportProvider.OpenSourceFile(pixmap, ".png", pixmap.SavePixelData);
			else if (audioData	!= null)	FileImportProvider.OpenSourceFile(audioData, ".ogg", audioData.SaveOggVorbisData);
			else if (shader		!= null)	FileImportProvider.OpenSourceFile(shader, shader is FragmentShader ? ".frag" : ".vert", shader.SaveSource);
		}
		public override bool CanPerformOn(Resource obj)
		{
			if (!base.CanPerformOn(obj)) return false;
			if (obj is Pixmap) return true;
			if (obj is AudioData) return true;
			if (obj is AbstractShader) return true;
			return false;
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextOpenRes;
		}
	}
	public class EditorActionInstantiatePrefab : EditorSingleAction<Prefab>
	{
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_InstantiatePrefab; }
		}

		public override void Perform(Prefab prefab)
		{
			try
			{
				CreateGameObjectAction undoRedoAction = new CreateGameObjectAction(null, prefab.Instantiate());
				UndoRedoManager.Do(undoRedoAction);
				DualityEditorApp.Select(this, new ObjectSelection(undoRedoAction.Result));
			}
			catch (Exception exception)
			{
				Log.Editor.WriteError("An error occurred instanciating Prefab {1}: {0}", 
					Log.Exception(exception),
					prefab != null ? prefab.Path : "null");
			}
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextOpenRes;
		}
	}
	public class EditorActionOpenScene : EditorSingleAction<Scene>
	{
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_OpenScene; }
		}

		public override void Perform(Scene scene)
		{
			string lastPath = Scene.CurrentPath;
			try
			{
				Scene.SwitchTo(scene);
			}
			catch (Exception exception)
			{
				Log.Editor.WriteError("An error occurred while switching from Scene {1} to Scene {2}: {0}", 
					Log.Exception(exception),
					lastPath,
					scene != null ? scene.Path : "null");
			}
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextOpenRes;
		}
	}
	public class EditorActionFocusGameObject : EditorSingleAction<GameObject>
	{
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_FocusGameObject; }
		}

		public override bool CanPerformOn(GameObject obj)
		{
			return base.CanPerformOn(obj) && obj.Transform != null;
		}
		public override void Perform(GameObject obj)
		{
			if (obj.Transform == null) return;
			DualityEditorApp.Highlight(this, new ObjectSelection(obj), HighlightMode.Spatial);
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextOpenRes;
		}
	}
	public class EditorActionFocusComponent : EditorSingleAction<Component>
	{
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_FocusGameObject; }
		}

		public override bool CanPerformOn(Component obj)
		{
			return base.CanPerformOn(obj) && obj.GameObj != null && obj.GameObj.Transform != null;
		}
		public override void Perform(Component obj)
		{
			if (obj.GameObj == null) return;
			if (obj.GameObj.Transform == null) return;
			DualityEditorApp.Highlight(this, new ObjectSelection(obj), HighlightMode.Spatial);
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextOpenRes;
		}
	}
}
