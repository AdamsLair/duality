using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Resources;

using Duality.Editor;
using Duality.Editor.Backend;
using Duality.Editor.Forms;

namespace Duality.Editor.Plugins.CamView
{
	public abstract class CamViewClient
	{
		private	CamView view = null;
		private	int pickingFrameLast = -1;


		public CamView View
		{
			get { return this.view; }
			internal set { this.view = value; }
		}
		/// <summary>
		/// [GET] Whether the parent <see cref="CamView"/> of this <see cref="CamViewClient"/> is currently
		/// visible to the user, or hidden away (e.g. by being in an inactive multi-document tab or similar).
		/// </summary>
		public bool IsViewVisible
		{
			get { return !this.view.IsHiddenDocument; }
		}
		public Size ClientSize
		{
			get { return this.view.RenderableControl.ClientSize; }
		}
		public Cursor Cursor
		{
			get { return this.view.RenderableControl.Cursor; }
			set { this.view.RenderableControl.Cursor = value; }
		}
		public ColorRgba BgColor
		{
			get { return this.view.BgColor; }
			set { this.view.BgColor = value; }
		}
		public ColorRgba FgColor
		{
			get { return this.view.FgColor; }
		}
		public bool Focused
		{
			get { return this.view.RenderableControl.Focused; }
		}
		public EditingGuide EditingUserGuide
		{
			get { return this.view.EditingUserGuides; }
		}
		internal INativeRenderableSite RenderableSite
		{
			get { return this.view == null ? null : this.view.RenderableSite; }
		}
		internal Control RenderableControl
		{
			get { return this.view == null ? null : this.view.RenderableControl; }
		}

		public Camera CameraComponent
		{
			get { return this.view.CameraComponent; }
		}
		public GameObject CameraObj
		{
			get { return this.view.CameraObj; }
		}
		

		public Point PointToClient(Point screenCoord)
		{
			return this.view.RenderableControl.PointToClient(screenCoord);
		}
		public Point PointToScreen(Point clientCoord)
		{
			return this.view.RenderableControl.PointToScreen(clientCoord);
		}
		public void Invalidate()
		{
			if (this.view == null || this.view.RenderableControl == null) return;
			this.view.RenderableControl.Invalidate();
		}
		public void Focus()
		{
			this.view.RenderableControl.Focus();
		}

		public ICmpRenderer PickRendererAt(int x, int y)
		{
			this.RenderableSite.MakeCurrent();
			this.RenderPickingMap();

			ICmpRenderer picked = this.CameraComponent.PickRendererAt(x / 2, y / 2);
			Component pickedComponent = picked as Component;

			if (pickedComponent == null) return null;
			if (pickedComponent.Disposed) return null;
			if (pickedComponent.GameObj == null) return null;

			return picked;
		}
		public IEnumerable<ICmpRenderer> PickRenderersIn(int x, int y, int w, int h)
		{
			this.RenderableSite.MakeCurrent();
			this.RenderPickingMap();

			IEnumerable<ICmpRenderer> picked = this.CameraComponent.PickRenderersIn(x / 2, y / 2, (w + 1) / 2, (h + 1) / 2);

			return picked.Where(r => 
				r != null && 
				!((Component)r).Disposed && 
				((Component)r).GameObj != null);
		}
		public bool IsSphereInView(Vector3 worldPos, float radius = 1.0f)
		{
			return this.CameraComponent.IsSphereInView(worldPos, radius);
		}
		public float GetScaleAtZ(float z)
		{
			return this.CameraComponent.GetScaleAtZ(z);
		}
		public Vector3 GetWorldPos(Vector3 screenPos)
		{
			return this.CameraComponent.GetWorldPos(screenPos);
		}
		public Vector3 GetWorldPos(Vector2 screenPos)
		{
			return this.CameraComponent.GetWorldPos(screenPos);
		}
		public Vector2 GetScreenPos(Vector3 worldPos)
		{
			return this.CameraComponent.GetScreenPos(worldPos);
		}
		public Vector2 GetScreenPos(Vector2 worldPos)
		{
			return this.CameraComponent.GetScreenPos(worldPos);
		}

		public void MakeDualityTarget()
		{
			this.view.MakeDualityTarget();
		}

		private bool RenderPickingMap()
		{
			if (this.pickingFrameLast == Time.FrameCount) return false;
			if (this.ClientSize.IsEmpty) return false;

			// Update culling info. Since we do not have a real game loop in edit
			// mode, we'll do this right before rendering rather than once per frame.
			if (Scene.Current.VisibilityStrategy != null)
				Scene.Current.VisibilityStrategy.Update();

			Point2 clientSize = new Point2(this.ClientSize.Width, this.ClientSize.Height);
			RenderSetup renderSetup = this.CameraComponent.PickingSetup;

			if (renderSetup != null)
				renderSetup.AddRendererFilter(this.PickingRendererFilter);

			this.pickingFrameLast = Time.FrameCount;
			this.CameraComponent.RenderPickingMap(
				clientSize / 2,
				clientSize,
				true);

			if (renderSetup != null)
				renderSetup.RemoveRendererFilter(this.PickingRendererFilter);

			return true;
		}
		private bool PickingRendererFilter(ICmpRenderer r)
		{
			GameObject obj = (r as Component).GameObj;

			if (!this.View.ObjectVisibility.Matches(obj))
				return false;

			DesignTimeObjectData data = DesignTimeObjectData.Get(obj);
			return !data.IsHidden;
		}
	}
}
