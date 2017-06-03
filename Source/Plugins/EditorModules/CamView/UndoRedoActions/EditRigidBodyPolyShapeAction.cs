using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Components.Physics;
using Duality.Editor;
using Duality.Editor.Plugins.CamView.Properties;


namespace Duality.Editor.Plugins.CamView.UndoRedoActions
{
	public class EditRigidBodyPolyShapeAction : RigidBodyShapeVertexAction
	{
		private PolyShapeInfo targetParentObj = null;

		protected override string NameBase
		{
			get
			{
				if (originalVertices.Length == newVertices.Length)
				{
					return CamViewRes.UndoRedo_MoveRigidBodyShapeVertex;
				}
				else
				{
					return originalVertices.Length > newVertices.Length ? CamViewRes.UndoRedo_DeleteRigidBodyShapeVertex : CamViewRes.UndoRedo_CreateRigidBodyShapeVertex;
				}
			}
		}
		public Vector2[] Result
		{
			get { return this.newVertices; }
		}

		public EditRigidBodyPolyShapeAction(PolyShapeInfo parent, Vector2[] originalVertices) : base(originalVertices, parent.Vertices)
		{
			this.targetParentObj = parent;
		}

		public override void Do()
		{
			Vector2[] temp = new Vector2[newVertices.Length];
			for (int i = 0; i < newVertices.Length; i++)
			{
				temp[i] = new Vector2(newVertices[i].X, newVertices[i].Y);
			}
			this.targetParentObj.Vertices = temp;
            
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(targetParentObj.Parent), ReflectionInfo.Property_RigidBody_Shapes);
		}
		public override void Undo()
		{
			Vector2[] temp = new Vector2[originalVertices.Length];
			for (int i = 0; i < originalVertices.Length; i++)
			{
				temp[i] = new Vector2(originalVertices[i].X, originalVertices[i].Y);
			}
			this.targetParentObj.Vertices = temp;
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(targetParentObj.Parent), ReflectionInfo.Property_RigidBody_Shapes);
		}
	}
}
