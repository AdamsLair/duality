using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

using AdamsLair.PropertyGrid;

using Duality;
using Duality.ColorFormat;

using DualityEditor;
using DualityEditor.CorePluginInterface;
using DualityEditor.UndoRedoActions;

namespace EditorBase.PropertyEditors
{
	public class ComponentPropertyEditor : MemberwisePropertyEditor
	{
		private	bool isInvokingDirectChild = false;

		public ComponentPropertyEditor()
		{
			this.Hints |= HintFlags.HasActiveCheck | HintFlags.ActiveEnabled;
			this.PropertyName = "Component";
			this.HeaderHeight = 20;
			this.HeaderStyle = AdamsLair.PropertyGrid.Renderer.GroupHeaderStyle.Emboss;
		}

		public void PerformSetActive(bool active)
		{
			UndoRedoManager.Do(new EditPropertyAction(this.ParentGrid, 
				ReflectionInfo.Property_Component_ActiveSingle, 
				this.GetValue(), 
				new object[] { active }));
		}

		protected override bool IsAutoCreateMember(MemberInfo info)
		{
			return base.IsAutoCreateMember(info) && info.DeclaringType != typeof(Component);
		}
		protected override bool IsChildValueModified(PropertyEditor childEditor)
		{
			return this.IsMemberInPrefabLinkChanges(childEditor.EditedMember);
		}
		protected bool IsMemberInPrefabLinkChanges(MemberInfo info)
		{
			if (info == null) return false;
			
			Component[] values = this.GetValue().Cast<Component>().NotNull().ToArray();
			return values.Any(delegate (Component c)
			{
				Duality.Resources.PrefabLink l = c.GameObj.AffectedByPrefabLink;
				return l != null && l.HasChange(c, info as PropertyInfo);
			});
		}
		protected override bool IsChildNonPublic(PropertyEditor childEditor)
		{
			if (base.IsChildNonPublic(childEditor)) return true;
			if (childEditor.EditedMember is FieldInfo) return true; // Discourage use of fields in Components
			return false;
		}
		protected override void OnUpdateFromObjects(object[] values)
		{
			base.OnUpdateFromObjects(values);

			this.Hints |= HintFlags.HasButton | HintFlags.ButtonEnabled;
			this.ButtonIcon = PluginRes.EditorBaseResCache.DropdownSettingsBlack;
			this.PropertyName = this.EditedType.GetTypeCSCodeName(true);
			this.HeaderValueText = null;
			if (!values.Any() || values.All(o => o == null))
				this.Active = false;
			else
				this.Active = (values.First(o => o is Component) as Component).ActiveSingle;
		}
		protected override void OnValueChanged(object sender, PropertyEditorValueEventArgs args)
		{
			base.OnValueChanged(sender, args);
			if (this.isInvokingDirectChild) return;

			// Find the direct descendant editor on the path to the changed one
			PropertyEditor directChild = args.Editor;
			while (directChild != null && !this.HasPropertyEditor(directChild))
				directChild = directChild.ParentEditor;
			if (directChild == args.Editor) return;

			// If an editor has changed that was NOT a direct descendant, fire the appropriate notifier events.
			// Always remember: If we don't emit a PropertyChanged event, PrefabLinks won't update their change lists!
			if (directChild != null && directChild != args.Editor && directChild.EditedMember != null)
			{
				this.isInvokingDirectChild = true;
				if (directChild.EditedMember is PropertyInfo)
					UndoRedoManager.Do(new EditPropertyAction(this.ParentGrid, directChild.EditedMember as PropertyInfo, this.GetValue(), null));
				else if (directChild.EditedMember is FieldInfo)
					UndoRedoManager.Do(new EditFieldAction(this.ParentGrid, directChild.EditedMember as FieldInfo, this.GetValue(), null));
				this.isInvokingDirectChild = false;
			}
		}
		protected override void OnActiveChanged()
		{
			base.OnActiveChanged();
			if (!this.IsUpdatingFromObject) this.PerformSetActive(this.Active);
		}
		protected override void OnEditedTypeChanged()
		{
			base.OnEditedTypeChanged();

			System.Drawing.Bitmap iconBitmap = CorePluginRegistry.GetTypeImage(this.EditedType) as System.Drawing.Bitmap;
			ColorHsva avgClr = iconBitmap != null ? 
				iconBitmap.GetAverageColor().ToHsva() : 
				Duality.ColorFormat.ColorHsva.TransparentWhite;
			if (avgClr.S <= 0.05f)
			{
				avgClr = new ColorHsva(
					0.001f * (float)(this.EditedType.Name.GetHashCode() % 1000), 
					1.0f, 
					0.5f);
			}

			this.PropertyName = this.EditedType.GetTypeCSCodeName(true);
			this.HeaderIcon = iconBitmap;
			this.HeaderColor = ExtMethodsSystemDrawingColor.ColorFromHSV(avgClr.H, 0.2f, 0.8f);
		}

		protected override void OnButtonPressed()
		{
			//base.OnButtonPressed(); // We don't want the base implementation

			// Is it safe to remove this Component?
			Component[] values = this.GetValue().Cast<Component>().NotNull().ToArray();
			bool canRemove = true;
			Component removeConflict = null;
			foreach (Component c in values)
			{
				foreach (Component r in c.GameObj.GetComponents<Component>())
				{
					if (!r.IsComponentRequirementMet(c))
					{
						canRemove = false;
						removeConflict = r;
						break;
					}
				}
				if (!canRemove) break;
			}

			// Create a ContextMenu
			ContextMenuStrip contextMenu = new ContextMenuStrip();
			Point menuPos = new Point(this.ButtonRectangle.Right, this.ButtonRectangle.Bottom);
			Point thisLoc = this.ParentEditor.GetChildLocation(this);
			menuPos.X += thisLoc.X;
			menuPos.Y += thisLoc.Y;

			// Default items
			ToolStripItem itemReset = contextMenu.Items.Add(PluginRes.EditorBaseRes.MenuItemName_ResetComponent, null, this.contextMenu_ResetComponent);
			ToolStripItem itemRemove = contextMenu.Items.Add(PluginRes.EditorBaseRes.MenuItemName_RemoveComponent, PluginRes.EditorBaseResCache.IconAbortCross, this.contextMenu_RemoveComponent);
			itemRemove.Enabled = canRemove;
			if (!canRemove) 
			{
				itemRemove.ToolTipText = string.Format(
					PluginRes.EditorBaseRes.MenuItemDesc_CantRemoveComponent, 
					values.First().GetType().Name, 
					removeConflict.GetType().Name);
			}
			ToolStripSeparator itemDefaultSep = new ToolStripSeparator();
			contextMenu.Items.Add(itemDefaultSep);

			// Custom actions
			var customActions = CorePluginRegistry.GetEditorActions(
				values.First().GetType(), 
				CorePluginRegistry.ActionContext_ContextMenu, 
				values)
				.ToArray();
			foreach (var actionEntry in customActions)
			{
				ToolStripMenuItem actionItem = new ToolStripMenuItem(actionEntry.Name, actionEntry.Icon);
				actionItem.Click += this.contextMenu_CustomAction;
				actionItem.Tag = actionEntry;
				actionItem.ToolTipText = actionEntry.Description;
				contextMenu.Items.Add(actionItem);
			}
			if (customActions.Length == 0) itemDefaultSep.Visible = false;

			contextMenu.Closed += this.contextMenu_Closed;
			contextMenu.Show(this.ParentGrid, menuPos, ToolStripDropDownDirection.BelowLeft);
		}
		private void contextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			this.ParentGrid.Focus();
		}
		private void contextMenu_ResetComponent(object sender, EventArgs e)
		{
			UndoRedoManager.Do(new ResetComponentAction(this.GetValue().Cast<Component>()));
		}
		private void contextMenu_RemoveComponent(object sender, EventArgs e)
		{
			Component[] values = this.GetValue().Cast<Component>().NotNull().ToArray();

			// Ask user if he really wants to delete stuff
			ObjectSelection objSel = new ObjectSelection(values);
			if (!DualityEditorApp.DisplayConfirmDeleteObjects(objSel)) return;
			if (!DualityEditorApp.DisplayConfirmBreakPrefabLinkStructure(objSel)) return;

			// Delete Components
			UndoRedoManager.Do(new DeleteComponentAction(values));
		}
		private void contextMenu_CustomAction(object sender, EventArgs e)
		{
			Component[] values = this.GetValue().Cast<Component>().NotNull().ToArray();
			ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
			IEditorAction action = clickedItem.Tag as IEditorAction;
			action.Perform(values);
		}
	}
}
