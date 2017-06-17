using System;
using System.Reflection;
using System.Linq;

using Duality.Resources;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Components.Physics;
using Duality.Drawing;

namespace Duality
{
	/// <summary>
	/// Provides Reflection data on Properties and Fields.
	/// </summary>
	public static class ReflectionInfo
	{
		public static readonly PropertyInfo Property_GameObject_Name;
		public static readonly PropertyInfo Property_GameObject_ActiveSingle;
		public static readonly PropertyInfo Property_GameObject_Parent;
		public static readonly PropertyInfo Property_GameObject_PrefabLink;

		public static readonly PropertyInfo Property_Component_GameObj;
		public static readonly PropertyInfo Property_Component_ActiveSingle;

		public static readonly PropertyInfo	Property_Transform_RelativePos;
		public static readonly PropertyInfo	Property_Transform_RelativeAngle;
		public static readonly PropertyInfo	Property_Transform_RelativeScale;
		public static readonly PropertyInfo	Property_Transform_RelativeVel;
		public static readonly PropertyInfo	Property_Transform_RelativeAngleVel;
		public static readonly PropertyInfo	Property_Transform_Pos;
		public static readonly PropertyInfo	Property_Transform_Angle;
		public static readonly PropertyInfo	Property_Transform_Scale;
		public static readonly PropertyInfo	Property_Transform_Vel;
		public static readonly PropertyInfo	Property_Transform_AngleVel;
		public static readonly PropertyInfo	Property_Transform_DeriveAngle;
		public static readonly PropertyInfo	Property_Transform_IgnoreParent;

		public static readonly PropertyInfo	Property_Camera_ClearColor;
		public static readonly PropertyInfo	Property_Camera_FocusDist;

		public static readonly PropertyInfo	Property_SoundEmitter_Sources;
		
		public static readonly PropertyInfo	Property_RigidBody_Shapes;
		public static readonly PropertyInfo	Property_RigidBody_Joints;
		public static readonly PropertyInfo	Property_CircleShapeInfo_Position;
		public static readonly PropertyInfo	Property_CircleShapeInfo_Radius;

		public static readonly PropertyInfo	Property_DrawTechnique_PreferredVertexFormat;

		public static readonly PropertyInfo	Property_Resource_AssetInfo;

		public static readonly PropertyInfo	Property_Font_Family;
		public static readonly PropertyInfo	Property_Font_Size;
		public static readonly PropertyInfo	Property_Font_Style;
		public static readonly PropertyInfo	Property_Font_GlyphRenderMode;
		public static readonly PropertyInfo	Property_Font_MonoSpace;

		public static readonly PropertyInfo	Property_Pixmap_AnimCols;
		public static readonly PropertyInfo	Property_Pixmap_AnimRows;
		public static readonly PropertyInfo	Property_Pixmap_AnimFrameBorder;
		public static readonly PropertyInfo	Property_Pixmap_Atlas;

		public static readonly PropertyInfo	Property_BatchInfo_Technique;
		public static readonly PropertyInfo	Property_BatchInfo_MainColor;
		public static readonly PropertyInfo	Property_BatchInfo_Textures;
		public static readonly PropertyInfo	Property_BatchInfo_Uniforms;

		public static readonly FieldInfo Field_Material_Info;


		static ReflectionInfo()
		{
			// Retrieve PropertyInfo data
			Type gameobject = typeof(GameObject);
			Property_GameObject_Name			= GetProperty(gameobject, "Name");
			Property_GameObject_ActiveSingle	= GetProperty(gameobject, "ActiveSingle");
			Property_GameObject_Parent			= GetProperty(gameobject, "Parent");
			Property_GameObject_PrefabLink		= GetProperty(gameobject, "PrefabLink");

			Type component = typeof(Component);
			Property_Component_GameObj		= GetProperty(component, "GameObj");
			Property_Component_ActiveSingle	= GetProperty(component, "ActiveSingle");

			Type transform = typeof(Transform);
			Property_Transform_RelativePos		= GetProperty(transform, "RelativePos");
			Property_Transform_RelativeAngle	= GetProperty(transform, "RelativeAngle");
			Property_Transform_RelativeScale	= GetProperty(transform, "RelativeScale");
			Property_Transform_RelativeVel		= GetProperty(transform, "RelativeVel");
			Property_Transform_RelativeAngleVel	= GetProperty(transform, "RelativeAngleVel");
			Property_Transform_Pos				= GetProperty(transform, "Pos");
			Property_Transform_Angle			= GetProperty(transform, "Angle");
			Property_Transform_Scale			= GetProperty(transform, "Scale");
			Property_Transform_Vel				= GetProperty(transform, "Vel");
			Property_Transform_AngleVel			= GetProperty(transform, "AngleVel");
			Property_Transform_DeriveAngle		= GetProperty(transform, "DeriveAngle");
			Property_Transform_IgnoreParent		= GetProperty(transform, "IgnoreParent");
			
			Type camera = typeof(Camera);
			Property_Camera_FocusDist			= GetProperty(camera, "FocusDist");
			Property_Camera_ClearColor			= GetProperty(camera, "ClearColor");

			Type collider = typeof(RigidBody);
			Property_RigidBody_Shapes			= GetProperty(collider, "Shapes");
			Property_RigidBody_Joints			= GetProperty(collider, "Joints");

			Type circleShapeInfo = typeof(CircleShapeInfo);
			Property_CircleShapeInfo_Position	= GetProperty(circleShapeInfo, "Position");
			Property_CircleShapeInfo_Radius		= GetProperty(circleShapeInfo, "Radius");

			Type drawTech = typeof(DrawTechnique);
			Property_DrawTechnique_PreferredVertexFormat	= GetProperty(drawTech, "PreferredVertexFormat");
			
			Type resource = typeof(Resource);
			Property_Resource_AssetInfo		= GetProperty(resource, "AssetInfo");

			Type font = typeof(Font);
			Property_Font_Family			= GetProperty(font, "Family");
			Property_Font_Size				= GetProperty(font, "Size");
			Property_Font_Style				= GetProperty(font, "Style");
			Property_Font_GlyphRenderMode	= GetProperty(font, "GlyphRenderMode");
			Property_Font_MonoSpace			= GetProperty(font, "MonoSpace");

			Type pixmap = typeof(Pixmap);
			Property_Pixmap_AnimCols		= GetProperty(pixmap, "AnimCols");
			Property_Pixmap_AnimRows		= GetProperty(pixmap, "AnimRows");
			Property_Pixmap_AnimFrameBorder	= GetProperty(pixmap, "AnimFrameBorder");
			Property_Pixmap_Atlas			= GetProperty(pixmap, "Atlas");

			Type batchInfo = typeof(BatchInfo);
			Property_BatchInfo_Technique	= GetProperty(batchInfo, "Technique");
			Property_BatchInfo_MainColor	= GetProperty(batchInfo, "MainColor");
			Property_BatchInfo_Textures		= GetProperty(batchInfo, "Textures");
			Property_BatchInfo_Uniforms		= GetProperty(batchInfo, "Uniforms");
			
			Type soundEmitter = typeof(SoundEmitter);
			Property_SoundEmitter_Sources	= GetProperty(soundEmitter, "Sources");

			// Retrieve FieldInfo data
			Type material = typeof(Material);
			Field_Material_Info	= GetField(material, "info");
		}
		
		private static PropertyInfo GetProperty(Type type, string name)
		{
			PropertyInfo result = type.GetRuntimeProperties().FirstOrDefault(m => !m.IsStatic() && m.Name == name);
			if (result == null)
			{
				Log.Core.WriteError(
					"Unable to retrieve property '{0}' of type '{1}'.",
					name,
					Log.Type(type));
			}
			return result;
		}
		private static FieldInfo GetField(Type type, string name)
		{
			FieldInfo result = type.GetRuntimeFields().FirstOrDefault(m => !m.IsStatic && m.Name == name);
			if (result == null)
			{
				Log.Core.WriteError(
					"Unable to retrieve field '{0}' of type '{1}'.",
					name,
					Log.Type(type));
			}
			return result;
		}
	}
}
