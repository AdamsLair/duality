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
	public class EditRigidBodyPolyShapeAction : UndoRedoAction
	{
		private Vector2[] originalVertices = null;
		private Vector2[] newVertices      = null;
		private ShapeInfo targetShape      = null;
		private bool      sameVertices     = false;

		public override string Name
		{
			get
			{
				if (originalVertices.Length == newVertices.Length)
					return CamViewRes.UndoRedo_MoveRigidBodyShapeVertex;
				else if (originalVertices.Length > newVertices.Length)
					return CamViewRes.UndoRedo_DeleteRigidBodyShapeVertex;
				else 
					return CamViewRes.UndoRedo_CreateRigidBodyShapeVertex;
			}
		}
		public override bool IsVoid
		{
			get { return this.sameVertices; }
		}

		public EditRigidBodyPolyShapeAction(ShapeInfo shape, Vector2[] originalVertices, Vector2[] newVertices)
		{
			this.targetShape = shape;
			this.originalVertices = (Vector2[])originalVertices.Clone();
			this.newVertices = (Vector2[])newVertices.Clone();
			this.sameVertices = this.AreVerticesEqual(this.originalVertices, this.newVertices);
		}

		public override void Do()
		{
			Vector2[] temp = (Vector2[])this.newVertices.Clone();
			this.SetVertices(this.targetShape, temp);
            
			DualityEditorApp.NotifyObjPropChanged(
				this, 
				new ObjectSelection(targetShape.Parent), 
				ReflectionInfo.Property_RigidBody_Shapes);
		}
		public override void Undo()
		{
			Vector2[] temp = (Vector2[])this.originalVertices.Clone();
			this.SetVertices(this.targetShape, temp);

			DualityEditorApp.NotifyObjPropChanged(
				this, 
				new ObjectSelection(targetShape.Parent), 
				ReflectionInfo.Property_RigidBody_Shapes);
		}
		
		private void SetVertices(ShapeInfo shapeInfo, Vector2[] vertices)
		{
			if (shapeInfo is PolyShapeInfo)
				(shapeInfo as PolyShapeInfo).Vertices = vertices;
			else if (shapeInfo is LoopShapeInfo)
				(shapeInfo as LoopShapeInfo).Vertices = vertices;
			else if (shapeInfo is ChainShapeInfo)
				(shapeInfo as ChainShapeInfo).Vertices = vertices;
			else
				throw new NotImplementedException();
		}

		private bool AreVerticesEqual(Vector2[] a, Vector2[] b)
		{
			if (a == b) return true;
			if (a == null || b == null) return false;
			if (a.Length != b.Length) return false;
			
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i]) return false;
			}

			return true;
		}
	}
}
