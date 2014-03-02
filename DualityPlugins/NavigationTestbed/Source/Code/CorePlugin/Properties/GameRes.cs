/*
 * A set of static helper classes that provide easy runtime access to the games resources.
 * This file is auto-generated. Any changes made to it are lost as soon as Duality decides
 * to regenerate it.
 */
namespace GameRes
{
	public static class Data {
		public static class Textures {
			public static Duality.ContentRef<Duality.Resources.Material> Agent_Material { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Material>(@"Data\Textures\Agent.Material.res"); }}
			public static Duality.ContentRef<Duality.Resources.Pixmap> Agent_Pixmap { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Pixmap>(@"Data\Textures\Agent.Pixmap.res"); }}
			public static Duality.ContentRef<Duality.Resources.Texture> Agent_Texture { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Texture>(@"Data\Textures\Agent.Texture.res"); }}
			public static Duality.ContentRef<Duality.Resources.Material> Floor_Material { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Material>(@"Data\Textures\Floor.Material.res"); }}
			public static Duality.ContentRef<Duality.Resources.Pixmap> Floor_Pixmap { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Pixmap>(@"Data\Textures\Floor.Pixmap.res"); }}
			public static Duality.ContentRef<Duality.Resources.Texture> Floor_Texture { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Texture>(@"Data\Textures\Floor.Texture.res"); }}
			public static Duality.ContentRef<Duality.Resources.Material> RigidBody_Material { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Material>(@"Data\Textures\RigidBody.Material.res"); }}
			public static Duality.ContentRef<Duality.Resources.Pixmap> RigidBody_Pixmap { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Pixmap>(@"Data\Textures\RigidBody.Pixmap.res"); }}
			public static Duality.ContentRef<Duality.Resources.Texture> RigidBody_Texture { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Texture>(@"Data\Textures\RigidBody.Texture.res"); }}
			public static Duality.ContentRef<Duality.Resources.Material> ShapeOutline_Material { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Material>(@"Data\Textures\ShapeOutline.Material.res"); }}
			public static void LoadAll() {
				Agent_Material.MakeAvailable();
				Agent_Pixmap.MakeAvailable();
				Agent_Texture.MakeAvailable();
				Floor_Material.MakeAvailable();
				Floor_Pixmap.MakeAvailable();
				Floor_Texture.MakeAvailable();
				RigidBody_Material.MakeAvailable();
				RigidBody_Pixmap.MakeAvailable();
				RigidBody_Texture.MakeAvailable();
				ShapeOutline_Material.MakeAvailable();
			}
		}
		public static Duality.ContentRef<Duality.Resources.Scene> _2x_agents_Scene { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Scene>(@"Data\2x agents.Scene.res"); }}
		public static Duality.ContentRef<Duality.Resources.Scene> _4x_agents_Scene { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Scene>(@"Data\4x agents.Scene.res"); }}
		public static Duality.ContentRef<Duality.Resources.Scene> _8x_agents_Scene { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Scene>(@"Data\8x agents.Scene.res"); }}
		public static Duality.ContentRef<Duality.Resources.Scene> agent_collision_Scene { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Scene>(@"Data\agent_collision.Scene.res"); }}
		public static Duality.ContentRef<Duality.Resources.Scene> all_against_one_Scene { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Scene>(@"Data\all against one.Scene.res"); }}
		public static Duality.ContentRef<Duality.Resources.Prefab> DummyAgent_Prefab { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Prefab>(@"Data\DummyAgent.Prefab.res"); }}
		public static void LoadAll() {
			Textures.LoadAll();
			_2x_agents_Scene.MakeAvailable();
			_4x_agents_Scene.MakeAvailable();
			_8x_agents_Scene.MakeAvailable();
			agent_collision_Scene.MakeAvailable();
			all_against_one_Scene.MakeAvailable();
			DummyAgent_Prefab.MakeAvailable();
		}
	}

}
