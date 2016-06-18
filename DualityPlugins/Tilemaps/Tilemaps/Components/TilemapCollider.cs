using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Editor;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Uses the information from the local <see cref="Tilemap"/> to configure the local <see cref="RigidBody"/> for 
	/// simulating physical interaction with the <see cref="Tilemap"/>.
	/// </summary>
	[RequiredComponent(typeof(RigidBody))]
	[EditorHintCategory(TilemapsResNames.CategoryTilemaps)]
	[EditorHintImage(TilemapsResNames.ImageTilemapCollider)]
	public class TilemapCollider : Component, ICmpInitializable
	{
		private static readonly TilemapCollisionSource[] DefaultSource = new TilemapCollisionSource[] 
		{
			new TilemapCollisionSource
			{
				SourceTilemap = null, 
				Layers = TileCollisionLayer.Layer0
			}
		};

		private TilemapCollisionSource[] source = DefaultSource;

		/// <summary>
		/// [GET / SET] Specifies which <see cref="Tilemap"/> components and collision layers to use
		/// to generate the collision shape.
		/// </summary>
		public IReadOnlyList<TilemapCollisionSource> CollisionSource
		{
			get { return this.source; }
			set
			{
				if (this.source != value)
				{
					this.UnsubscribeSourceEvents();
					this.source = value.ToArray() ?? DefaultSource;
					this.SubscribeSourceEvents();
				}
			}
		}

		private void SubscribeSourceEvents()
		{
			EventHandler<TilemapChangedEventArgs> handler = this.SourceTilemap_EventTilemapChanged;
			for (int i = 0; i < this.source.Length; i++)
			{
				if (this.source[i].SourceTilemap != null)
				{
					// Use the unsubscribe-subscribe pattern to avoid subscribing twice
					this.source[i].SourceTilemap.EventTilemapChanged -= handler;
					this.source[i].SourceTilemap.EventTilemapChanged += handler;
				}
			}
		}
		private void UnsubscribeSourceEvents()
		{
			EventHandler<TilemapChangedEventArgs> handler = this.SourceTilemap_EventTilemapChanged;
			for (int i = 0; i < this.source.Length; i++)
			{
				if (this.source[i].SourceTilemap != null)
				{
					this.source[i].SourceTilemap.EventTilemapChanged -= handler;
				}
			}
		}

		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate)
			{
				this.SubscribeSourceEvents();
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
			{
				this.UnsubscribeSourceEvents();
			}
		}

		private void SourceTilemap_EventTilemapChanged(object sender, TilemapChangedEventArgs e)
		{
			Log.Core.Write("TilemapChanged: {0}, [{1}:{2}]", e.Component, e.Pos, e.Size);
		}
	}
}
