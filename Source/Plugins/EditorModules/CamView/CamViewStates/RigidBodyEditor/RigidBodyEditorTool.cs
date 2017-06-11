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
		/// [GET] Whether this tool, while selected, indicates that there is an action that
		/// can potentially be performed at the current mouse position.
		/// </summary>
		public virtual bool IsHoveringAction
		{
			get { return false; }
		}


		/// <summary>
		/// While performing a continuous operation, this method can provide an optional
		/// action text that will be displayed in the status area of the <see cref="RigidBody"/> editor.
		/// </summary>
		/// <returns></returns>
		public virtual string GetActionText()
		{
			return null;
		}
		/// <summary>
		/// Checks whether this tool can start a continuous operation given the specified
		/// user input and environment state.
		/// </summary>
		/// <returns></returns>
		public virtual bool CanBeginAction(MouseButtons mouseButton)
		{
			return false;
		}
		/// <summary>
		/// Attempts to begin a continuous operation with this tool. Returns false when no
		/// operation is possible or associated with the specified mouse button.
		/// </summary>
		public virtual void BeginAction(MouseButtons mouseButton) { }
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
		/// <param name="mouseButton"></param>
		public virtual void OnActionKeyPressed(MouseButtons mouseButton) { }
		/// <summary>
		/// Called when the action key has been released after the action has already begun.
		/// </summary>
		/// <param name="mouseButton"></param>
		public virtual void OnActionKeyReleased(MouseButtons mouseButton) { }
		/// <summary>
		/// Called when the tool is selected and allowed to draw an overlay inside the <see cref="CamView"/>.
		/// </summary>
		/// <param name="canvas"></param>
		public virtual void OnWorldOverlayDrawcalls(Canvas canvas) { }
		/// <summary>
		/// Called when the tool is selected and allowed to handle mouse movement events. Unlike <see cref="UpdateAction"/>,
		/// this method will also be called while the tool is not performing any continuous action, allowing it to
		/// update its internal state that may then be used to make <see cref="CanBeginAction"/> and <see cref="IsHoveringAction"/>
		/// more efficient.
		/// </summary>
		public virtual void OnMouseMove() { }

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
			this.env.SelectTool(this.GetType());
		}
	}
}
