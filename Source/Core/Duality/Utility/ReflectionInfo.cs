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

		public static readonly PropertyInfo	Property_Transform_LocalPos;
		public static readonly PropertyInfo	Property_Transform_LocalAngle;
		public static readonly PropertyInfo	Property_Transform_LocalScale;
		public static readonly PropertyInfo	Property_Transform_Pos;
		public static readonly PropertyInfo	Property_Transform_Angle;
		public static readonly PropertyInfo	Property_Transform_Scale;
		public static readonly PropertyInfo	Property_Transform_IgnoreParent;

		public static readonly PropertyInfo	Property_Camera_ClearColor;
		public static readonly PropertyInfo	Property_Camera_FocusDist;
		public static readonly PropertyInfo	Property_Camera_RenderingSetup;

		public static readonly PropertyInfo	Property_SoundEmitter_Sources;
		
		public static readonly PropertyInfo	Property_RigidBody_Shapes;
		public static readonly PropertyInfo	Property_RigidBody_Joints;
		public static readonly PropertyInfo	Property_CircleShapeInfo_Position;
		public static readonly PropertyInfo	Property_CircleShapeInfo_Radius;

		public static readonly PropertyInfo	Property_DrawTechnique_PreferredVertexFormat;

		public static readonly PropertyInfo	Property_Resource_AssetInfo;

		public static readonly PropertyInfo	Property_Pixmap_Atlas;

		public static readonly PropertyInfo	Property_BatchInfo_Technique;
		public static readonly PropertyInfo	Property_BatchInfo_MainColor;

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
			Property_Transform_LocalPos		= GetProperty(transform, "LocalPos");
			Property_Transform_LocalAngle	= GetProperty(transform, "LocalAngle");
			Property_Transform_LocalScale	= GetProperty(transform, "LocalScale");
			Property_Transform_Pos				= GetProperty(transform, "Pos");
			Property_Transform_Angle			= GetProperty(transform, "Angle");
			Property_Transform_Scale			= GetProperty(transform, "Scale");
			Property_Transform_IgnoreParent		= GetProperty(transform, "IgnoreParent");
			
			Type camera = typeof(Camera);
			Property_Camera_FocusDist			= GetProperty(camera, "FocusDist");
			Property_Camera_ClearColor			= GetProperty(camera, "ClearColor");
			Property_Camera_RenderingSetup		= GetProperty(camera, "RenderingSetup");

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

			Type pixmap = typeof(Pixmap);
			Property_Pixmap_Atlas			= GetProperty(pixmap, "Atlas");

			Type batchInfo = typeof(BatchInfo);
			Property_BatchInfo_Technique	= GetProperty(batchInfo, "Technique");
			Property_BatchInfo_MainColor	= GetProperty(batchInfo, "MainColor");
			
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
				Logs.Core.WriteError(
					"Unable to retrieve property '{0}' of type '{1}'.",
					name,
					LogFormat.Type(type));
		}
			return result;
		}
		private static FieldInfo GetField(Type type, string name)
		{
			FieldInfo result = type.GetRuntimeFields().FirstOrDefault(m => !m.IsStatic && m.Name == name);
			if (result == null)
			{
				Logs.Core.WriteError(
					"Unable to retrieve field '{0}' of type '{1}'.",
					name,
					LogFormat.Type(type));
		}
			return result;
	}
	}
}
