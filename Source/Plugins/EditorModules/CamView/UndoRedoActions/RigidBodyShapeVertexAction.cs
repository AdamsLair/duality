using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Components.Physics;

using Duality.Editor;

namespace Duality.Editor.Plugins.CamView.UndoRedoActions
{
	public abstract class RigidBodyShapeVertexAction : UndoRedoAction
	{
        protected   Vector2[] originalVertices = null;
		protected	Vector2[] newVertices = null;
		
		protected abstract string NameBase { get; }
		//protected abstract string NameBaseMulti { get; }
		public override string Name
		{
			get { return this.NameBase; }
            //get
            //{
            //    return this.newVertices.Length == 1 ?
            //  string.Format(this.NameBase, this.newVertices[0].GetType().Name) :
            //  string.Format(this.NameBaseMulti, this.newVertices.Length);
            //}
        }
		public override bool IsVoid
		{
			get { return this.newVertices == null || this.newVertices.Length == 0; }
		}

		public RigidBodyShapeVertexAction(Vector2[] originalVertices, Vector2[] newVertices)
		{
            this.originalVertices = originalVertices;
            this.newVertices = newVertices;

            for (int i = 0; i < newVertices.Length; i++)
            {
                this.newVertices[i] = new Vector2(newVertices[i].X, newVertices[i].Y);
            }
		}
	}
}
