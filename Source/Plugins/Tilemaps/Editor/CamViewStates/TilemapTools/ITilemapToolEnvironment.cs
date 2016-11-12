using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.UndoRedoActions;


namespace Duality.Editor.Plugins.Tilemaps.CamViewStates
{
	/// <summary>
	/// Provides an interface for <see cref="TilemapTool"/> instances to access <see cref="TilemapEditorCamViewState"/> internals and perform editing operations.
	/// </summary>
	public interface ITilemapToolEnvironment
	{
		/// <summary>
		/// [GET / SET] When drawing tiles, this is the source from which the drawn pattern is retrieved.
		/// This property is never null. Assigning null will reset it to its default non-null value.
		/// </summary>
		ITileDrawSource TileDrawSource { get; set; }
		/// <summary>
		/// [GET / SET] The currently selected <see cref="TilemapTool"/>.
		/// </summary>
		TilemapTool SelectedTool { get; set; }
		/// <summary>
		/// [GET] The coordinate of the <see cref="Tile"/> that is currently hovered by the mouse cursor.
		/// </summary>
		Point2 HoveredTile { get; }
		/// <summary>
		/// [GET] The currently active <see cref="Tilemap"/>.
		/// </summary>
		Tilemap ActiveTilemap { get; }
		/// <summary>
		/// [GET / SET] The <see cref="Tilemap"/> coordinates of the actions proposed <see cref="Tile"/> editing area.
		/// </summary>
		Point2 ActiveOrigin { get; set; }
		/// <summary>
		/// [GET] A grid mask that represents the actions proposed <see cref="Tile"/> editing area.
		/// </summary>
		Grid<bool> ActiveArea { get; }
		/// <summary>
		/// [GET] A list of outlines that enclose the actions proposed <see cref="Tile"/> editing area.
		/// Will be determined automatically when cleared.
		/// </summary>
		IList<Vector2[]> ActiveAreaOutlines { get; }
		/// <summary>
		/// [GET] The coordinate of the <see cref="Tile"/> where the current action was started.
		/// </summary>
		Point2 ActionBeginTile { get; }
		
		/// <summary>
		/// Submits a set of previous changes to <see cref="ActiveArea"/> or <see cref="ActiveOrigin"/> to be used in the editor.
		/// </summary>
		/// <param name="isFullPreview">
		/// Specifies whether the currently set editing area represents the actual area the action will be performed in.
		/// False is for incomplete or invalid previews.
		/// </param>
		void SubmitActiveAreaChanges(bool isFullPreview);
		/// <summary>
		/// Performs an editing operation to the specified <see cref="Tilemap"/>.
		/// </summary>
		/// <param name="actionType"></param>
		/// <param name="tilemap"></param>
		/// <param name="pos"></param>
		/// <param name="brush"></param>
		/// <param name="source"></param>
		/// <param name="sourceOffset"></param>
		void PerformEditTiles(EditTilemapActionType actionType, Tilemap tilemap, Point2 pos, Grid<bool> brush, ITileDrawSource source, Point2 sourceOffset);
	}
}
