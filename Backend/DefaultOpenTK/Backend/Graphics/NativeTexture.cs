using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

using OpenTK.Graphics.OpenGL;

using GLTexMagFilter = OpenTK.Graphics.OpenGL.TextureMagFilter;
using GLTexMinFilter = OpenTK.Graphics.OpenGL.TextureMinFilter;
using GLTexWrapMode = OpenTK.Graphics.OpenGL.TextureWrapMode;
using GLPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using TextureMagFilter = Duality.Drawing.TextureMagFilter;
using TextureMinFilter = Duality.Drawing.TextureMinFilter;
using TextureWrapMode = Duality.Drawing.TextureWrapMode;

namespace Duality.Backend.DefaultOpenTK
{
	[DontSerialize]
	public class NativeTexture : INativeTexture
	{
		private	static	bool			texInit			= false;
		private	static	int				activeTexUnit	= 0;
		private	static	TextureUnit[]	texUnits		= null;
		private	static	NativeTexture[]	curBound		= null;

		private static void InitTextureFields()
		{
			if (texInit) return;
			
			int numTexUnits;
			GL.GetInteger(GetPName.MaxTextureImageUnits, out numTexUnits);
			texUnits = new TextureUnit[numTexUnits];
			curBound = new NativeTexture[numTexUnits];

			for (int i = 0; i < numTexUnits; i++)
			{
				texUnits[i] = (TextureUnit)((int)TextureUnit.Texture0 + i);
			}

			texInit = true;
		}
		public static void Bind(ContentRef<Duality.Resources.Texture> target, int texUnit = 0)
		{
			Bind((target.Res != null ? target.Res.Native : null) as NativeTexture);
		}
		public static void Bind(NativeTexture tex, int texUnit = 0)
		{
			if (!texInit) InitTextureFields();

			if (curBound[texUnit] == tex) return;
			if (activeTexUnit != texUnit) GL.ActiveTexture(texUnits[texUnit]);
			activeTexUnit = texUnit;

			if (tex == null)
			{
				GL.BindTexture(TextureTarget.Texture2D, 0);
				GL.Disable(EnableCap.Texture2D);
				curBound[texUnit] = null;
			}
			else
			{
				GL.Enable(EnableCap.Texture2D);
				GL.BindTexture(TextureTarget.Texture2D, tex.Handle);
				curBound[texUnit] = tex;
			}
		}
		public static void ResetBinding(int beginAtIndex = 0)
		{
			if (!texInit) InitTextureFields();
			for (int i = beginAtIndex; i < texUnits.Length; i++)
			{
				Bind(null as NativeTexture, i);
			}
		}


		private int		handle	= 0;
		private int		width	= 0;
		private int		height	= 0;
		private bool	mipmaps	= false;
		private TexturePixelFormat format	= TexturePixelFormat.Rgba;

		public int Handle
		{
			get { return this.handle; }
		}
		public int Width
		{
			get { return this.width; }
		}
		public int Height
		{
			get { return this.height; }
		}
		public bool HasMipmaps
		{
			get { return this.mipmaps; }
		}
		public TexturePixelFormat Format
		{
			get { return this.format; }
		}

		public NativeTexture()
		{
			this.handle = GL.GenTexture();
		}

		void INativeTexture.SetupEmpty(TexturePixelFormat format, int width, int height, TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapX, TextureWrapMode wrapY, int anisoLevel, bool mipmaps)
		{
			DefaultOpenTKBackendPlugin.GuardSingleThreadState();

			int lastTexId;
			GL.GetInteger(GetPName.TextureBinding2D, out lastTexId);
			if (lastTexId != this.handle) GL.BindTexture(TextureTarget.Texture2D, this.handle);

			// Set texture parameters
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)ToOpenTKTextureMinFilter(minFilter));
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)ToOpenTKTextureMagFilter(magFilter));
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)ToOpenTKTextureWrapMode(wrapX));
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)ToOpenTKTextureWrapMode(wrapY));

			// Anisotropic filtering
			GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName) ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, (float)anisoLevel);

			// If needed, care for Mipmaps
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, mipmaps ? 1 : 0);

			// Setup pixel format
			GL.TexImage2D(TextureTarget.Texture2D, 0,
				ToOpenTKPixelFormat(format), width, height, 0,
				GLPixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

			this.width = width;
			this.height = height;
			this.format = format;
			this.mipmaps = mipmaps;

			if (lastTexId != this.handle) GL.BindTexture(TextureTarget.Texture2D, lastTexId);
		}
		void INativeTexture.LoadData<T>(TexturePixelFormat format, int width, int height, T[] data, ColorDataLayout dataLayout, ColorDataElementType dataElementType)
		{
			DefaultOpenTKBackendPlugin.GuardSingleThreadState();

			int lastTexId;
			GL.GetInteger(GetPName.TextureBinding2D, out lastTexId);
			GL.BindTexture(TextureTarget.Texture2D, this.handle);

			// Load pixel data to video memory
			GL.TexImage2D(TextureTarget.Texture2D, 0, 
				ToOpenTKPixelFormat(format), width, height, 0, 
				dataLayout.ToOpenTK(), dataElementType.ToOpenTK(), 
				data);

			this.width = width;
			this.height = height;
			this.format = format;

			GL.BindTexture(TextureTarget.Texture2D, lastTexId);
		}
		void INativeTexture.GetData<T>(T[] target, ColorDataLayout dataLayout, ColorDataElementType dataElementType)
		{
			DefaultOpenTKBackendPlugin.GuardSingleThreadState();

			int lastTexId;
			GL.GetInteger(GetPName.TextureBinding2D, out lastTexId);
			GL.BindTexture(TextureTarget.Texture2D, this.handle);
			
			GL.GetTexImage(TextureTarget.Texture2D, 0, 
				dataLayout.ToOpenTK(), dataElementType.ToOpenTK(), 
				target);

			GL.BindTexture(TextureTarget.Texture2D, lastTexId);
		}
		void IDisposable.Dispose()
		{
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated &&
				this.handle != 0)
			{
				DefaultOpenTKBackendPlugin.GuardSingleThreadState();
				GL.DeleteTexture(this.handle);
				this.handle = 0;
			}
		}
		
		private static PixelInternalFormat ToOpenTKPixelFormat(TexturePixelFormat format)
		{
			switch (format)
			{
				case TexturePixelFormat.Single:				return PixelInternalFormat.R8;
				case TexturePixelFormat.Dual:				return PixelInternalFormat.Rg8;
				case TexturePixelFormat.Rgb:				return PixelInternalFormat.Rgb;
				case TexturePixelFormat.Rgba:				return PixelInternalFormat.Rgba;

				case TexturePixelFormat.FloatSingle:		return PixelInternalFormat.R16f;
				case TexturePixelFormat.FloatDual:			return PixelInternalFormat.Rg16f;
				case TexturePixelFormat.FloatRgb:			return PixelInternalFormat.Rgb16f;
				case TexturePixelFormat.FloatRgba:			return PixelInternalFormat.Rgba16f;

				case TexturePixelFormat.CompressedSingle:	return PixelInternalFormat.CompressedRed;
				case TexturePixelFormat.CompressedDual:		return PixelInternalFormat.CompressedRg;
				case TexturePixelFormat.CompressedRgb:		return PixelInternalFormat.CompressedRgb;
				case TexturePixelFormat.CompressedRgba:		return PixelInternalFormat.CompressedRgba;
			}

			return PixelInternalFormat.Rgba;
		}
		private static GLTexMagFilter ToOpenTKTextureMagFilter(TextureMagFilter value)
		{
			switch (value)
			{
				case TextureMagFilter.Nearest:	return GLTexMagFilter.Nearest;
				case TextureMagFilter.Linear:	return GLTexMagFilter.Linear;
			}

			return GLTexMagFilter.Nearest;
		}
		private static GLTexMinFilter ToOpenTKTextureMinFilter(TextureMinFilter value)
		{
			switch (value)
			{
				case TextureMinFilter.Nearest:				return GLTexMinFilter.Nearest;
				case TextureMinFilter.Linear:				return GLTexMinFilter.Linear;
				case TextureMinFilter.NearestMipmapNearest:	return GLTexMinFilter.NearestMipmapNearest;
				case TextureMinFilter.LinearMipmapNearest:	return GLTexMinFilter.LinearMipmapNearest;
				case TextureMinFilter.NearestMipmapLinear:	return GLTexMinFilter.NearestMipmapLinear;
				case TextureMinFilter.LinearMipmapLinear:	return GLTexMinFilter.LinearMipmapLinear;
			}

			return GLTexMinFilter.Nearest;
		}
		private static GLTexWrapMode ToOpenTKTextureWrapMode(TextureWrapMode value)
		{
			switch (value)
			{
				case TextureWrapMode.Clamp:				return GLTexWrapMode.ClampToEdge;
				case TextureWrapMode.Repeat:			return GLTexWrapMode.Repeat;
				case TextureWrapMode.MirroredRepeat:	return GLTexWrapMode.MirroredRepeat;
			}

			return GLTexWrapMode.Clamp;
		}
	}
}
