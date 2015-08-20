using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;

using Duality.IO;
using Duality.Resources;
using Duality.Properties;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.UndoRedoActions;

namespace Duality.Editor.Plugins.Base.EditorActions
{
	public class ShaderToProgram : EditorAction<AbstractShader>
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
			get { return typeof(ShaderProgram).GetEditorImage(); }
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

			string programPath = PathHelper.GetFreePath(Path.Combine(dirTemp, nameTemp), Resource.GetFileExtByType<ShaderProgram>());
			ShaderProgram program = new ShaderProgram(vert, frag);
			program.Save(programPath);
		}
	}
}
