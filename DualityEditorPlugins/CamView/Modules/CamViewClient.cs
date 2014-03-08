using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Components;
using Duality.Drawing;

using Duality.Editor;
using Duality.Editor.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Duality.Editor.Plugins.CamView
{
	public abstract class CamViewClient
	{
		private	CamView	view	= null;


		public CamView View
		{
			get { return this.view; }
			internal set { this.view = value; }
		}
		public Size ClientSize
		{
			get { return this.view.LocalGLControl.ClientSize; }
		}
		public Cursor Cursor
		{
			get { return this.view.LocalGLControl.Cursor; }
			set { this.view.LocalGLControl.Cursor = value; }
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
		internal GLControl LocalGLControl
		{
			get { return this.view == null ? null : this.view.LocalGLControl; }
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
			return this.view.LocalGLControl.PointToClient(screenCoord);
		}
		public Point PointToScreen(Point clientCoord)
		{
			return this.view.LocalGLControl.PointToScreen(clientCoord);
		}
		public void Invalidate()
		{
			if (this.view == null || this.view.LocalGLControl == null) return;
			this.view.LocalGLControl.Invalidate();
		}
		public void Focus()
		{
			this.view.LocalGLControl.Focus();
		}

		public ICmpRenderer PickRendererAt(int x, int y)
		{
			DualityEditorApp.GLMakeCurrent(this.LocalGLControl);
			var result = this.CameraComponent.PickRendererAt(new Rect(this.ClientSize.Width, this.ClientSize.Height), x, y);
			return result;
		}
		public HashSet<ICmpRenderer> PickRenderersIn(int x, int y, int w, int h)
		{
			DualityEditorApp.GLMakeCurrent(this.LocalGLControl);
			var result = this.CameraComponent.PickRenderersIn(new Rect(this.ClientSize.Width, this.ClientSize.Height), x, y, w, h);
			return result;
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
			DualityApp.TargetResolution = new Vector2(this.ClientSize.Width, this.ClientSize.Height);
			DualityApp.Mouse.Source = this.view;
			DualityApp.Keyboard.Source = this.view;
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated)
			{
				if (this.CameraObj.GetComponent<SoundListener>() != null)
					this.CameraObj.GetComponent<SoundListener>().MakeCurrent();
			}
		}
	}
}
