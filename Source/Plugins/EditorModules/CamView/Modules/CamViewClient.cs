﻿using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Components;
using Duality.Drawing;

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
			return this.CameraComponent.PickRendererAt(x, y);
		}
		public IEnumerable<ICmpRenderer> PickRenderersIn(int x, int y, int w, int h)
		{
			this.RenderableSite.MakeCurrent();
			this.RenderPickingMap();
			return this.CameraComponent.PickRenderersIn(x, y, w, h);
		}
		public bool IsCoordInView(Vector3 c, float boundRad = 1.0f)
		{
			return this.CameraComponent.IsCoordInView(c, boundRad);
		}
		public float GetScaleAtZ(float z)
		{
			return this.CameraComponent.GetScaleAtZ(z);
		}
		public Vector3 GetSpaceCoord(Vector3 screenCoord)
		{
			return this.CameraComponent.GetSpaceCoord(screenCoord);
		}
		public Vector3 GetSpaceCoord(Vector2 screenCoord)
		{
			return this.CameraComponent.GetSpaceCoord(screenCoord);
		}
		public Vector3 GetScreenCoord(Vector3 spaceCoord)
		{
			return this.CameraComponent.GetScreenCoord(spaceCoord);
		}

		public void MakeDualityTarget()
		{
			this.view.MakeDualityTarget();
		}

		private bool RenderPickingMap()
		{
			if (this.pickingFrameLast == Time.FrameCount) return false;
			if (this.ClientSize.IsEmpty) return false;

			this.pickingFrameLast = Time.FrameCount;
			this.CameraComponent.RenderPickingMap(
				new Point2(this.ClientSize.Width, this.ClientSize.Height),
				true);

			return true;
		}
	}
}
