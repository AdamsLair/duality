using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Properties
{
	/// <summary>
	/// This static class contains constant string representations of certain resource names.
	/// </summary>
	public static class CoreResNames
	{
		private const string ManifestBaseName		= "Duality.EmbeddedResources.EditorSupport.";

		public const string CategoryNone			= "";
		public const string CategoryGraphics		= "Graphics";
		public const string CategorySound			= "Sound";
		public const string CategoryPhysics			= "Physics";
		public const string CategoryDiagnostics		= "Diagnostics";
		public const string CategoryAI				= "AI";

		public const string ImageGameObject			= ManifestBaseName + "iconGameObj.png";
		public const string ImageComponent			= ManifestBaseName + "iconCmpUnknown.png";
		public const string ImageResource			= ManifestBaseName + "iconResUnknown.png";

		public const string ImageXmlSerializer		= ManifestBaseName + "iconXmlSerializer.png";
		public const string ImageBinarySerializer	= ManifestBaseName + "iconBinarySerializer.png";

		public const string ImageDrawTechnique		= ManifestBaseName + "iconResDrawTechnique.png";
		public const string ImageFragmentShader		= ManifestBaseName + "iconResFragmentShader.png";
		public const string ImageMaterial			= ManifestBaseName + "iconResMaterial.png";
		public const string ImagePixmap				= ManifestBaseName + "iconResPixmap.png";
		public const string ImagePrefab				= ManifestBaseName + "iconResPrefabFull.png";
		public const string ImageRenderTarget		= ManifestBaseName + "iconResRenderTarget.png";
		public const string ImageShaderProgram		= ManifestBaseName + "iconResShaderProgram.png";
		public const string ImageTexture			= ManifestBaseName + "iconResTexture.png";
		public const string ImageVertexShader		= ManifestBaseName + "iconResVertexShader.png";
		public const string ImageScene				= ManifestBaseName + "iconResScene.png";
		public const string ImageAudioData			= ManifestBaseName + "iconResAudioData.png";
		public const string ImageSound				= ManifestBaseName + "iconResSound.png";
		public const string ImageFont				= ManifestBaseName + "iconResFont.png";

		public const string ImageSpriteRenderer		= ManifestBaseName + "iconCmpSpriteRenderer.png";
		public const string ImageAnimSpriteRenderer	= ManifestBaseName + "iconCmpSpriteRenderer.png";
		public const string ImageTextRenderer		= ManifestBaseName + "iconResFont.png";
		public const string ImageTransform			= ManifestBaseName + "iconCmpTransform.png";
		public const string ImageCamera				= ManifestBaseName + "iconCmpCamera.png";
		public const string ImageSoundEmitter		= ManifestBaseName + "iconResSound.png";
		public const string ImageSoundListener		= ManifestBaseName + "iconCmpSoundListener.png";
		public const string ImageRigidBody			= ManifestBaseName + "iconCmpRectCollider.png";
		public const string ImageProfileRenderer	= ManifestBaseName + "iconCmpProfileRenderer.png";
		public const string ImageRigidBodyRenderer	= ManifestBaseName + "iconCmpRigidBodyRenderer.png";

		public const string ImageDataNode			= ManifestBaseName + "Primitive.png";
		public const string ImageArrayNode			= ManifestBaseName + "Array.png";
		public const string ImageStructNode			= ManifestBaseName + "Object.png";
		public const string ImageObjectRefNode		= ManifestBaseName + "ObjectRef.png";
		public const string ImageTypeDataLayoutNode	= ManifestBaseName + "Class.png";
		public const string ImageMemberInfoNode		= ManifestBaseName + "Method.png";
		public const string ImageDelegateNode		= ManifestBaseName + "Delegate.png";
	}
}
