using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Components.Physics;
using Duality.Drawing;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// Base class for implementing <see cref="Duality.Components.Physics.RigidBody"/> editor operations.
	/// </summary>
	public abstract class RigidBodyEditorTool
	{
		private IRigidBodyEditorToolEnvironment env;
		private ToolStripButton toolButton;
		private HelpInfo info;

		
		/// <summary>
		/// [GET] The editor environment in which this operation is performed.
		/// </summary>
		public IRigidBodyEditorToolEnvironment Environment
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
		/// [GET] A relative indicator where to put this tool when displaying it to the user.
		/// Negative values offset towards the beginning, positive values offset towards the end.
		/// </summary>
		public virtual int SortOrder
		{
			get { return 0; }
		}
		/// <summary>
		/// [GET] Whether this tool is available in the current <see cref="Environment"/>,
		/// e.g. whether it can perform a meaningful action on the selected <see cref="RigidBody"/>.
		/// </summary>
		public virtual bool IsAvailable
		{
			get { return this.env.ActiveBody != null; }
		}
		
		/// <summary>
		/// Begins a continuous operation with this tool. Usually invoked via left-click.
		/// </summary>
		public virtual void BeginAction() { }
		/// <summary>
		/// Updates the currently active continuous operation of this tool.
		/// </summary>
		public virtual void UpdateAction() { }
		/// <summary>
		/// Ends a previously started continuous operation of this tool.
		/// </summary>
		public virtual void EndAction() { }
		/// <summary>
		/// Called when the action key has been pressed after the action has already begun.
		/// </summary>
		public virtual void OnActionKeyPressed() { }
		/// <summary>
		/// While performing a continuous operation, this method can provide an optional
		/// action text that will be displayed in the status area of the <see cref="RigidBody"/> editor.
		/// </summary>
		/// <returns></returns>
		public virtual string GetActionText()
		{
			return null;
		}

		// RigidBodyEditorSelVertices Test 2
		public virtual void OnCollectStateWorldOverlayDrawcalls(Canvas canvas) { }

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
