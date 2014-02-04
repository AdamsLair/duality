using System;
using System.Collections.Generic;
using System.Linq;

using Duality.ColorFormat;
using Duality.EditorHints;
using Duality.VertexFormat;
using Duality.Resources;
using Duality.Components.Physics;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Duality.Components.Diagnostics
{
	/// <summary>
	/// A diagnostic <see cref="Duality.Component"/> that renders a RigidBodies shape for debugging purposes.
	/// </summary>
	[Serializable]
	[RequiredComponent(typeof(RigidBody))]
	public class RigidBodyRenderer : Renderer
	{
		private	ContentRef<Material>	areaMaterial			= Material.Checkerboard;
		private	ContentRef<Material>	outlineMaterial			= Material.SolidWhite;
		private	BatchInfo				customAreaMaterial		= null;
		private	BatchInfo				customOutlineMaterial	= null;
		private	ColorRgba				colorTint				= ColorRgba.White;
		private	int						offset					= 0;


		public override float BoundRadius
		{
			get { return this.GameObj.RigidBody.BoundRadius; }
		}
		/// <summary>
		/// [GET / SET] The <see cref="Duality.Resources.Material"/> that is used for rendering the RigidBodies shape areaa.
		/// </summary>
		public ContentRef<Material> AreaMaterial
		{
			get { return this.areaMaterial; }
			set { this.areaMaterial = value; }
		}
		/// <summary>
		/// [GET / SET] The <see cref="Duality.Resources.Material"/> that is used for rendering the RigidBodies shape outlines.
		/// </summary>
		public ContentRef<Material> OutlineMaterial
		{
			get { return this.outlineMaterial; }
			set { this.outlineMaterial = value; }
		}
		/// <summary>
		/// [GET / SET] A custom, local <see cref="Duality.Resources.BatchInfo"/> overriding the <see cref="AreaMaterial"/>.
		/// </summary>
		public BatchInfo CustomAreaMaterial
		{
			get { return this.customAreaMaterial; }
			set { this.customAreaMaterial = value; }
		}
		/// <summary>
		/// [GET / SET] A custom, local <see cref="Duality.Resources.BatchInfo"/> overriding the <see cref="OutlineMaterial"/>.
		/// </summary>
		public BatchInfo CustomOutlineMaterial
		{
			get { return this.customOutlineMaterial; }
			set { this.customOutlineMaterial = value; }
		}
		/// <summary>
		/// [GET / SET] A color by which the rendered debug output is tinted.
		/// </summary>
		public ColorRgba ColorTint
		{
			get { return this.colorTint; }
			set { this.colorTint = value; }
		}
		/// <summary>
		/// [GET / SET] A virtual Z offset that affects the order in which objects are drawn. If you want to assure an object is drawn after another one,
		/// just assign a higher Offset value to the background object.
		/// </summary>
		public int Offset
		{
			get { return this.offset; }
			set { this.offset = value; }
		}
		/// <summary>
		/// [GET] The internal Z-Offset added to the renderers vertices based on its <see cref="Offset"/> value.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float VertexZOffset
		{
			get { return this.offset * 0.01f; }
		}


		public override void Draw(IDrawDevice device)
		{
			Vector3 pos = this.gameobj.Transform.Pos;
			Canvas canvas = new Canvas(device);
			canvas.CurrentState.TransformAngle = this.gameobj.Transform.Angle;
			canvas.CurrentState.SetMaterial(this.areaMaterial);

			canvas.FillCircle(pos.X, pos.Y, pos.Z, 50.0f);

			canvas.CurrentState.TransformHandle = new Vector2(125.0f, 0.0f);
			canvas.FillRect(pos.X, pos.Y, pos.Z, 128.0f, 128.0f);

			canvas.CurrentState.TransformHandle = new Vector2(-125.0f, 0.0f);
			canvas.FillCircleSegment(pos.X, pos.Y, pos.Z, 50.0f, 0.0f, MathF.RadAngle45 + MathF.RadAngle90, 32.0f);

			canvas.CurrentState.TransformHandle = new Vector2(-250.0f, 0.0f);
			canvas.FillCircleSegment(pos.X, pos.Y, pos.Z, 50.0f, 0.0f, MathF.RadAngle45 + MathF.RadAngle90, 16.0f);

			canvas.CurrentState.TransformHandle = new Vector2(-375.0f, 0.0f);
			canvas.FillCircleSegment(pos.X, pos.Y, pos.Z, 50.0f, 0.0f, MathF.RadAngle45 + MathF.RadAngle90, 8.0f);

			canvas.CurrentState.TransformHandle = new Vector2(-500.0f, 0.0f);
			canvas.FillCircleSegment(pos.X, pos.Y, pos.Z, 50.0f, 0.0f, MathF.RadAngle45 + MathF.RadAngle90, 49.0f);

			canvas.CurrentState.TransformHandle = new Vector2(-125.0f, -125.0f);
			canvas.FillCircleSegment(pos.X, pos.Y, pos.Z, 50.0f, 0.0f, MathF.RadAngle360, 32.0f);

			canvas.CurrentState.TransformHandle = new Vector2(-250.0f, -125.0f);
			canvas.FillCircleSegment(pos.X, pos.Y, pos.Z, 50.0f, 0.0f, MathF.RadAngle360, 16.0f);

			canvas.CurrentState.TransformHandle = new Vector2(-375.0f, -125.0f);
			canvas.FillCircleSegment(pos.X, pos.Y, pos.Z, 50.0f, 0.0f, MathF.RadAngle360, 8.0f);

			canvas.CurrentState.TransformHandle = new Vector2(-500.0f, -125.0f);
			canvas.FillCircleSegment(pos.X, pos.Y, pos.Z, 50.0f, 0.0f, MathF.RadAngle360, 49.0f);
		}
		protected override void OnCopyTo(Component target, Cloning.CloneProvider provider)
		{
			base.OnCopyTo(target, provider);
			RigidBodyRenderer t = target as RigidBodyRenderer;
			t.areaMaterial			= this.areaMaterial;
			t.outlineMaterial		= this.outlineMaterial;
			t.customAreaMaterial	= this.customAreaMaterial;
			t.customOutlineMaterial	= this.customOutlineMaterial;
			t.colorTint				= this.colorTint;
			t.offset				= this.offset;
		}
	}
}
