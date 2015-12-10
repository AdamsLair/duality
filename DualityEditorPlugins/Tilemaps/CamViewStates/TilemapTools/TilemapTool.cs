using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps.CamViewStates
{
	/// <summary>
	/// Base class for implementing <see cref="Tilemap"/> editing operations.
	/// </summary>
	public abstract class TilemapTool
	{
		private ITilemapToolEnvironment env;
		private ToolStripButton toolButton;
		private HelpInfo info;

		
		/// <summary>
		/// [GET] The editor environment in which this operation is performed.
		/// </summary>
		public ITilemapToolEnvironment Environment
		{
			get { return this.env;}
			internal set { this.env = value; }
		}
		/// <summary>
		/// [GET] The toolbar button that represents this operation.
		/// </summary>
		internal ToolStripButton ToolButton
		{
			get { return this.toolButton; }
		}
		/// <summary>
		/// [GET] The name of the editing operation as displayed to the user.
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// [GET] The icon of the editing operation as displayed to the user.
		/// </summary>
		public abstract Image Icon { get; }
		/// <summary>
		/// [GET] An optional hint that is displayed in the user interface when using this tool.
		/// </summary>
		public virtual HelpInfo HelpInfo
		{
			get
			{
				if (this.info == null && !string.IsNullOrEmpty(this.Name))
				{
					Type type = this.GetType();
					if (HelpSystem.GetXmlCodeDoc(type) != null)
					{
						this.info = HelpInfo.FromMember(type);
						this.info.Topic = this.Name;
					}
					else
					{
						this.info = HelpInfo.CreateNotAvailable(this.Name);
					}
				}
				return this.info;
			}
		}
		/// <summary>
		/// [GET] The cursor image to be used when this tool is active.
		/// </summary>
		public virtual Cursor ActionCursor
		{
			get { return CursorHelper.ArrowAction; }
		}
		/// <summary>
		/// [GET] An optional shortcut key that can be used to invoke this tool.
		/// </summary>
		public virtual Keys ShortcutKey
		{
			get { return Keys.None; }
		}
		/// <summary>
		/// [GET] An optional override key that, as long as pressed, will override the selected tool with this one.
		/// </summary>
		public virtual Keys OverrideKey
		{
			get { return Keys.None; }
		}
		/// <summary>
		/// [GET] If true, this tool always prefers the currently selected <see cref="Tilemap"/> for editing and picking operations,
		/// rather than the one the is nearest to the camera.
		/// </summary>
		public virtual bool PickPreferSelectedLayer
		{
			get { return true; }
		}
		/// <summary>
		/// [GET] If true, previews of this operation will be partially faded in to avoid visual noise.
		/// </summary>
		public virtual bool FadeInPreviews
		{
			get { return false; }
		}
		/// <summary>
		/// [GET] A relative indicator where to put this tool when displaying it to the user.
		/// Negative values offset towards the beginning, positive values offset towards the end.
		/// </summary>
		public virtual int SortOrder
		{
			get { return 0; }
		}


		/// <summary>
		/// Updates the preview of this operation.
		/// </summary>
		public abstract void UpdatePreview();
		/// <summary>
		/// Updates the active action area of this operation.
		/// </summary>
		public virtual void UpdateActiveArea()
		{
			// By default, the preview already equals the active area.
		}

		/// <summary>
		/// Begins a continuous operation with this tool. Usually invoked via left-click.
		/// </summary>
		public virtual void BeginAction() { }
		/// <summary>
		/// Updates the currently performed continuous operation of this tool.
		/// </summary>
		public virtual void UpdateAction() { }
		/// <summary>
		/// Ends a previously started continuous operation of this tool.
		/// </summary>
		public virtual void EndAction() { }

		/// <summary>
		/// Initializes the tool's internal toolbar button.
		/// </summary>
		internal void InitToolButton()
		{
			if (this.toolButton != null) return;

			string name = this.Name;
			Image icon = this.Icon;
			Keys shortcut = this.ShortcutKey;

			if (string.IsNullOrWhiteSpace(name) || icon == null)
				return;

			if (shortcut != Keys.None)
			{
				name += " (" + shortcut.ToString() + ")";
			}

			this.toolButton = new ToolStripButton(name, icon);
			this.toolButton.Click += this.toolButton_Click;
		}
		/// <summary>
		/// Disposes the tool's internal toolbar button.
		/// </summary>
		internal void DisposeToolButton()
		{
			if (this.toolButton == null) return;

			this.toolButton.Click -= this.toolButton_Click;
			this.toolButton.Dispose();
			this.toolButton = null;
		}

		private void toolButton_Click(object sender, EventArgs e)
		{
			this.env.SelectedTool = this;
		}
	}
}
