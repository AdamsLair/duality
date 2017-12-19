using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Resources;

namespace Duality.Backend
{
	public class RenderOptions
	{
		private INativeRenderTarget       target           = null;
		private ClearFlag                 clearFlags       = ClearFlag.All;
		private ColorRgba                 clearColor       = ColorRgba.TransparentBlack;
		private float                     clearDepth       = 1.0f;
		private Rect                      viewport         = new Rect(0, 0, 256, 256);
		private RenderMode              renderMode       = RenderMode.Screen;
		private Matrix4                   viewMatrix       = Matrix4.Identity;
		private Matrix4                   projectionMatrix = Matrix4.Identity;
		private ShaderParameterCollection shaderParameters = new ShaderParameterCollection();
		
		public INativeRenderTarget Target
		{
			get { return this.target; }
			set { this.target = value; }
		}
		public ClearFlag ClearFlags
		{
			get { return this.clearFlags; }
			set { this.clearFlags = value; }
		}
		public ColorRgba ClearColor
		{
			get { return this.clearColor; }
			set { this.clearColor = value; }
		}
		public float ClearDepth
		{
			get { return this.clearDepth; }
			set { this.clearDepth = value; }
		}
		public Rect Viewport
		{
			get { return this.viewport; }
			set { this.viewport = value; }
		}
		public RenderMode RenderMode
		{
			get { return this.renderMode; }
			set { this.renderMode = value; }
		}
		public Matrix4 ViewMatrix
		{
			get { return this.viewMatrix; }
			set { this.viewMatrix = value; }
		}
		public Matrix4 ProjectionMatrix
		{
			get { return this.projectionMatrix; }
			set { this.projectionMatrix = value; }
		}
		public ShaderParameterCollection ShaderParameters
		{
			get { return this.shaderParameters; }
			set { this.shaderParameters = value; }
		}
	}
}
