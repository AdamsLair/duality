using System;
using System.Linq;

using Duality.Cloning;
using Duality.Drawing;
using Duality.Resources;
using Duality.Editor;
using Duality.Properties;

namespace Duality.Components.Renderers
{
	/// <summary>
	/// Renders a text to represent the <see cref="GameObject"/>.
	/// </summary>
	[ManuallyCloned]
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryGraphics)]
	[EditorHintImage(typeof(CoreRes), CoreResNames.ImageFont)]
	public class TextRenderer : Renderer, ICmpInitializable
	{
		protected	Alignment				blockAlign	= Alignment.Center;
		protected	FormattedText			text		= new FormattedText("Hello World");
		protected	BatchInfo				customMat	= null;
		protected	ColorRgba				colorTint	= ColorRgba.White;
		protected	ContentRef<Material>	iconMat		= null;
		protected	int						offset		= 0;
		[DontSerialize] protected	VertexC1P3T2[][]	vertFont	= null;
		[DontSerialize] protected	VertexC1P3T2[]		vertIcon	= null;


		[EditorHintFlags(MemberFlags.Invisible)]
		public override float BoundRadius
		{
			get 
			{
				Rect textRect = Rect.Align(this.blockAlign, 0.0f, 0.0f, 
					MathF.Max(this.text.Size.X, this.text.MaxWidth), 
					MathF.Max(this.text.Size.Y, this.text.MaxHeight));
				return textRect.Transform(this.gameobj.Transform.Scale, this.gameobj.Transform.Scale).BoundingRadius;
			}
		}
		/// <summary>
		/// [GET / SET] The text blocks alignment relative to the <see cref="GameObject"/>.
		/// </summary>
		public Alignment BlockAlign
		{
			get { return this.blockAlign; }
			set { this.blockAlign = value; }
		}
		/// <summary>
		/// [GET / SET] The text to display..
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		public FormattedText Text
		{
			get { return this.text; }
			set { this.text = value; }
		}
		/// <summary>
		/// [GET / SET] A color by which the displayed text is tinted.
		/// </summary>
		public ColorRgba ColorTint
		{
			get { return this.colorTint; }
			set { this.colorTint = value; }
		}
		/// <summary>
		/// [GET / SET] The <see cref="Duality.Resources.Material"/> to use for displaying icons ithin the text.
		/// </summary>
		public ContentRef<Material> IconMat
		{
			get { return this.iconMat; }
			set { this.iconMat = value; }
		}
		/// <summary>
		/// [GET] The current texts metrics.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public FormattedText.Metrics Metrics
		{
			get { return this.text.TextMetrics; }
		}
		/// <summary>
		/// [GET / SET] A custom, local <see cref="Duality.Drawing.BatchInfo"/> overriding the texts own <see cref="Duality.Resources.Font.Material">
		/// Materials</see>. Note that it does not override each <see cref="Duality.Resources.Font">Fonts</see> Texture, but their DrawTechniques and
		/// main colors.
		/// </summary>
		public BatchInfo CustomMaterial
		{
			get { return this.customMat; }
			set { this.customMat = value; }
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


		public TextRenderer() 
		{
			this.text.Fonts = new[] { Font.GenericMonospace10 };
		}

		public override void Draw(IDrawDevice device)
		{
			Vector3 posTemp = this.gameobj.Transform.Pos;
			float scaleTemp = 1.0f;
			device.PreprocessCoords(ref posTemp, ref scaleTemp);

			Vector2 xDot, yDot;
			MathF.GetTransformDotVec(this.GameObj.Transform.Angle, this.gameobj.Transform.Scale * scaleTemp, out xDot, out yDot);

			// Apply block alignment
			Vector2 textOffset = Vector2.Zero;
			Vector2 textSize = this.text.Size;
			if (this.text.MaxWidth > 0) textSize.X = this.text.MaxWidth;
			this.blockAlign.ApplyTo(ref textOffset, textSize);
			MathF.TransformDotVec(ref textOffset, ref xDot, ref yDot);
			posTemp.X += textOffset.X;
			posTemp.Y += textOffset.Y;
			if (this.text.Fonts != null && this.text.Fonts.Any(r => r.IsAvailable && r.Res.IsPixelGridAligned))
			{
				posTemp.X = MathF.Round(posTemp.X);
				posTemp.Y = MathF.Round(posTemp.Y);
				if (MathF.RoundToInt(device.TargetSize.X) != (MathF.RoundToInt(device.TargetSize.X) / 2) * 2)
					posTemp.X += 0.5f;
				if (MathF.RoundToInt(device.TargetSize.Y) != (MathF.RoundToInt(device.TargetSize.Y) / 2) * 2)
					posTemp.Y += 0.5f;
			}

			ColorRgba matColor = this.customMat != null ? this.customMat.MainColor : ColorRgba.White;
			int[] vertLen = this.text.EmitVertices(ref this.vertFont, ref this.vertIcon, posTemp.X, posTemp.Y, posTemp.Z + this.VertexZOffset, this.colorTint * matColor, xDot, yDot);
			if (this.text.Fonts != null)
			{
				for (int i = 0; i < this.text.Fonts.Length; i++)
				{
					if (this.text.Fonts[i] != null && this.text.Fonts[i].IsAvailable) 
					{
						if (this.customMat == null)
						{
							device.AddVertices(this.text.Fonts[i].Res.Material, VertexMode.Quads, this.vertFont[i], vertLen[i + 1]);
						}
						else
						{
							BatchInfo cm = new BatchInfo(this.customMat);
							cm.Textures = this.text.Fonts[i].Res.Material.Textures;
							device.AddVertices(cm, VertexMode.Quads, this.vertFont[i], vertLen[i + 1]);
						}
					}
				}
			}
			if (this.text.Icons != null && this.iconMat.IsAvailable)
			{
				device.AddVertices(this.iconMat, VertexMode.Quads, this.vertIcon, vertLen[0]);
			}
		}

		void ICmpInitializable.OnInit(InitContext context)
		{
			if (context == InitContext.Loaded)
				this.text.ApplySource();
		}
		void ICmpInitializable.OnShutdown(ShutdownContext context) {}
		
		protected override void OnSetupCloneTargets(object targetObj, ICloneTargetSetup setup)
		{
			base.OnSetupCloneTargets(targetObj, setup);
			TextRenderer target = targetObj as TextRenderer;

			setup.HandleObject(this.text, target.text);
			setup.HandleObject(this.customMat, target.customMat);
		}
		protected override void OnCopyDataTo(object targetObj, ICloneOperation operation)
		{
			base.OnCopyDataTo(targetObj, operation);
			TextRenderer target = targetObj as TextRenderer;

			target.blockAlign	= this.blockAlign;
			target.colorTint	= this.colorTint;
			target.offset		= this.offset;

			operation.HandleValue(ref this.iconMat, ref target.iconMat);
			operation.HandleObject(this.text, ref target.text);
			operation.HandleObject(this.customMat, ref target.customMat);
		}
	}
}
