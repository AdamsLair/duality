using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AdamsLair.WinForms;
using AdamsLair.WinForms.PropertyEditing;

using Duality.Components;
using Duality.Components.Physics;
using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;
using Duality.Editor.UndoRedoActions;

namespace Duality.Editor.Plugins.Tilemaps
{
	/// <summary>
	/// A user dialog that allows to quickly generate a set of <see cref="Tilemap"/> layers
	/// based on some settings.
	/// </summary>
	public partial class TilemapSetupDialog : Form
	{
		private class Settings
		{
			/// <summary>
			/// The width and height of each <see cref="Tilemap"/> layer.
			/// </summary>
			[EditorHintRange(1, 10000, 1, 500)]
			public Point2 MapSize = TilemapsSetupUtility.DefaultTilemapSize;
			/// <summary>
			/// The number of <see cref="Tilemap"/> layers that will be generated.
			/// </summary>
			[EditorHintRange(1, 20, 1, 5)]
			public int LayerCount = 3;
			/// <summary>
			/// The <see cref="Tileset"/> that will be used for the generated <see cref="Tilemap"/> layers.
			/// If you want each layer to use a different <see cref="Tileset"/>, you can assign them manually
			/// after generating the <see cref="Tilemap"/> objects.
			/// </summary>
			public ContentRef<Tileset> Tileset = null;
			/// <summary>
			/// Whether or not the generated <see cref="Tilemap"/> will produce depth values. 
			/// 
			/// Top-down and side-scrolling maps are usually flat and every tile shares the same 
			/// depth value. However, if you're setting up a tilemap that uses fake perspective, e.g.
			/// a character can walk both in front of and behind a tree, you need a deep <see cref="Tilemap"/>,
			/// where every tile has its own depth values.
			/// </summary>
			public bool DeepTilemap = true;
			/// <summary>
			/// Whether or not the generated <see cref="Tilemap"/> layers will be used as a source
			/// for generating collision shapes in a static <see cref="Duality.Components.Physics.RigidBody"/>.
			/// </summary>
			public bool GenerateCollisionShapes = true;
		}

		private Settings settings = new Settings();

		public ContentRef<Tileset> Tileset
		{
			get { return this.settings.Tileset; }
			set { this.settings.Tileset = value; this.settingsGrid.UpdateFromObjects(); }
		}

		public TilemapSetupDialog()
		{
			this.InitializeComponent();

			// Adjust the settings grid style to match our dialog
			this.settingsGrid.Renderer.ColorBackground = this.BackColor;
			this.settingsGrid.Renderer.NestedBrightnessScale = 1.0f;
			this.settingsGrid.Renderer.NestedBrightnessOffset = 0;
			this.settingsGrid.SplitterRatio = 0.5f;

			// Assign our settings object
			this.UpdateSettingsGrid();
		}

		private void UpdateSettingsGrid()
		{
			this.settingsGrid.SelectObject(this.settings);
			MemberwisePropertyEditor rootEditor = this.settingsGrid.MainEditor as MemberwisePropertyEditor;
			rootEditor.HeaderStyle = GroupedPropertyEditor.GroupHeaderStyle.Flat;
			rootEditor.HeaderHeight = 0;
			rootEditor.Hints = PropertyEditor.HintFlags.None;
			if (rootEditor.VisibleChildEditors.Count > 0)
				rootEditor.VisibleChildEditors[0].Focus();
		}
		private void GenerateTilemaps()
		{
			// Create a parent transform object, so we don't clutter the Scene too much
			GameObject rootObject = new GameObject("Map");
			rootObject.AddComponent<Transform>();

			// Generate all tilemap layers
			List<Tilemap> generatedTilemaps = new List<Tilemap>();
			for (int i = 0; i < this.settings.LayerCount; i++)
			{
				string layerName = 
					(i == 0 ? "BaseLayer" : 
					(i == this.settings.LayerCount - 1 ? "TopLayer" : 
					(this.settings.LayerCount == 3 ? "UpperLayer" :
					("UpperLayer" + i.ToString()))));

				GameObject layerObj = new GameObject(layerName, rootObject);
				layerObj.AddComponent<Transform>();

				Tilemap tilemap = layerObj.AddComponent<Tilemap>();
				TilemapsSetupUtility.SetupTilemap(
					tilemap, 
					this.settings.Tileset, 
					this.settings.MapSize.X, 
					this.settings.MapSize.Y,
					i > 0);

				TilemapRenderer renderer = layerObj.AddComponent<TilemapRenderer>();
				renderer.DepthOffset = -0.01f * i;
				if (this.settings.DeepTilemap)
				{
					renderer.TileDepthMode = TileDepthOffsetMode.World;
					renderer.TileDepthScale = 0.01f;
				}
				else
				{
					renderer.TileDepthMode = TileDepthOffsetMode.Flat;
					renderer.TileDepthScale = 0.0f;
				}

				generatedTilemaps.Add(tilemap);
			}

			// Generate a collision layer when requested
			if (this.settings.GenerateCollisionShapes)
			{
				GameObject layerObj = new GameObject("WorldGeometry", rootObject);
				layerObj.AddComponent<Transform>();

				RigidBody body = layerObj.AddComponent<RigidBody>();
				body.BodyType = BodyType.Static;

				TilemapCollider collider = layerObj.AddComponent<TilemapCollider>();
				TilemapCollisionSource[] collisionSources = new TilemapCollisionSource[generatedTilemaps.Count];
				for (int i = 0; i < generatedTilemaps.Count; i++)
				{
					collisionSources[i].Layers = TileCollisionLayer.Layer0;
					collisionSources[i].SourceTilemap = generatedTilemaps[i];
				}
				collider.RoundedCorners = true;
				collider.CollisionSource = collisionSources;
			}

			// Add the new objects to the current Scene as an UndoRedo operation.
			UndoRedoManager.Do(new CreateGameObjectAction(
				null, 
				rootObject));
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			this.settingsGrid.Focus();
		}
		private void buttonOk_Click(object sender, EventArgs e)
		{
			this.GenerateTilemaps();
			this.Close();
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
