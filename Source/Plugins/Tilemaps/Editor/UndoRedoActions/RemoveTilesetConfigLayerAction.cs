using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Cloning;
using Duality.Resources;
using Duality.Plugins.Tilemaps;

using Duality.Editor;
using Duality.Editor.Plugins.Tilemaps.Properties;
using Duality.Editor.Plugins.Tilemaps.CamViewStates;

namespace Duality.Editor.Plugins.Tilemaps.UndoRedoActions
{
	public class RemoveTilesetConfigLayerAction<T> : UndoRedoAction
	{
		private Tileset      tileset;
		private T            layer;
		private PropertyInfo property;

		public override string Name
		{
			get { return TilemapsRes.UndoRedo_TilesetAddConfigLayer; }
		}
		public override bool IsVoid
		{
			get { return this.tileset == null || this.property == null || object.Equals(this.layer, default(T)); }
		}

		public RemoveTilesetConfigLayerAction(Tileset tileset, PropertyInfo property, T layer)
		{
			if (property == null) throw new ArgumentNullException("property");
			if (!typeof(Tileset).IsAssignableFrom(property.DeclaringType)) throw new ArgumentException("Property needs to be a Tileset property.", "property");
			if (!typeof(ICollection<T>).IsAssignableFrom(property.PropertyType)) throw new ArgumentException("Property needs to be a matching collection type.", "property");

			this.tileset = tileset;
			this.property = property;
			this.layer = layer;
		}

		public override void Do()
		{
			ICollection<T> layerCollection = this.property.GetValue(this.tileset) as ICollection<T>;
			layerCollection.Remove(this.layer);
			this.OnNotifyPropertyChanged();
		}
		public override void Undo()
		{
			ICollection<T> layerCollection = this.property.GetValue(this.tileset) as ICollection<T>;
			layerCollection.Add(this.layer);
			this.OnNotifyPropertyChanged();
		}

		private void OnNotifyPropertyChanged()
		{
			DualityEditorApp.NotifyObjPropChanged(
				this,
				new ObjectSelection(this.tileset),
				this.property);
		}
	}
}
