using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

using AdamsLair.WinForms;
using AdamsLair.WinForms.Renderer;
using ButtonState = AdamsLair.WinForms.Renderer.ButtonState;

using Duality;
using Duality.Editor;
using Duality.Editor.UndoRedoActions;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	public partial class GameObjectPropertyEditor : PropertyEditor, IHelpProvider
	{
		private static Font headerPrefabFont = SystemFonts.DefaultFont;
		private static Font headerNameExtFont = SystemFonts.DefaultFont;
		private static Font headerNameFont = new Font(SystemFonts.DefaultFont, FontStyle.Bold);

		private	Rectangle	rectHeader				= Rectangle.Empty;
		private	Rectangle	rectPrefab				= Rectangle.Empty;
		private	Rectangle	rectCheckActive			= Rectangle.Empty;
		private	Rectangle	rectLabelName			= Rectangle.Empty;
		private	Rectangle	rectLabelPrefab			= Rectangle.Empty;
		private	Rectangle	rectButtonsPrefab		= Rectangle.Empty;
		private	Rectangle	rectButtonPrefabShow	= Rectangle.Empty;
		private	Rectangle	rectButtonPrefabRevert	= Rectangle.Empty;
		private	Rectangle	rectButtonPrefabApply	= Rectangle.Empty;
		private	Rectangle	rectButtonPrefabBreak	= Rectangle.Empty;
		private	string		displayedName		= "GameObject";
		private	string		displayedNameExt	= "";
		private	bool?		active				= false;
		private	bool		activeCheckHovered	= false;
		private	bool		activeCheckPressed	= false;
		private	bool		prefabLinked		= false;
		private	bool		prefabLinkAvailable	= false;
		private	int			curButton			= -1;
		private	bool		curButtonHovered	= false;
		private	bool		curButtonPressed	= false;
		
		public override object DisplayedValue
		{
			get { return this.GetValue(); }
		}

		public GameObjectPropertyEditor()
		{
			this.Height = 45;
			this.Hints = HintFlags.None;
			this.EditedType = typeof(GameObject);
			this.PropertyName = "GameObject";
		}

		public void PerformSetActive(bool active)
		{
			UndoRedoManager.Do(new EditPropertyAction(this.ParentGrid, 
				ReflectionInfo.Property_GameObject_ActiveSingle, 
				this.GetValue(), 
				new object[] { active }));
		}

		protected override void OnGetValue()
		{
			base.OnGetValue();
			GameObject[] values = this.GetValue().Cast<GameObject>().ToArray();

			this.BeginUpdate();
			if (values.Any())
			{
				GameObject parent = values.First().Parent;

				this.active = values.First().ActiveSingle;
				if (!values.All(o => o.ActiveSingle == active.Value)) this.active = null;

				if (values.Count() == 1)
				{
					this.displayedName = values.First().Name;
					this.displayedNameExt = values.First().Parent != null ? " in " + parent.FullName : "";
				}
				else
				{
					this.displayedName = string.Format(Duality.Editor.Properties.GeneralRes.PropertyGrid_N_Objects, values.Count());
					this.displayedNameExt = "";
				}
				this.prefabLinked = values.Any(o => o.PrefabLink != null);
				this.prefabLinkAvailable = values.All(o => o.PrefabLink == null || o.PrefabLink.Prefab.IsAvailable);

				this.Invalidate();
			}
			this.EndUpdate();
		}
		protected override void UpdateGeometry()
		{
			base.UpdateGeometry();

			// General
			this.rectHeader = new Rectangle(
				this.ClientRectangle.X, 
				this.ClientRectangle.Y, 
				this.ClientRectangle.Width, 
				this.ClientRectangle.Height * 20 / 45);
			this.rectPrefab = new Rectangle(
				this.ClientRectangle.X, 
				this.ClientRectangle.Y + this.rectHeader.Height, 
				this.ClientRectangle.Width, 
				this.ClientRectangle.Height - this.rectHeader.Height);

			// Header
			this.rectCheckActive = new Rectangle(
				this.rectHeader.X + 2,
				this.rectHeader.Y + this.rectHeader.Height / 2 - ControlRenderer.CheckBoxSize.Height / 2 - 1,
				ControlRenderer.CheckBoxSize.Width,
				ControlRenderer.CheckBoxSize.Height);
			this.rectLabelName = new Rectangle(
				this.rectCheckActive.Right + 2,
				this.rectHeader.Y,
				this.rectHeader.Width - this.rectCheckActive.Right - 4,
				this.rectHeader.Height);

			// PrefabLink
			this.rectLabelPrefab = new Rectangle(
				this.rectPrefab.X + 2,
				this.rectPrefab.Y,
				65,
				this.rectPrefab.Height);
			this.rectButtonsPrefab = new Rectangle(
				this.rectLabelPrefab.Right + 2,
				this.rectPrefab.Y,
				this.rectPrefab.Width - this.rectLabelPrefab.Right - 3,
				this.rectPrefab.Height);
			int buttonSpacing = 2;
			int buttonWidth = (this.rectButtonsPrefab.Width - (buttonSpacing * 3)) / 4;
			this.rectButtonPrefabShow = new Rectangle(
				this.rectButtonsPrefab.X,
				this.rectButtonsPrefab.Y + 3,
				buttonWidth,
				this.rectButtonsPrefab.Height - 6);
			this.rectButtonPrefabRevert = new Rectangle(
				this.rectButtonsPrefab.X + buttonWidth + buttonSpacing,
				this.rectButtonsPrefab.Y + 3,
				buttonWidth,
				this.rectButtonsPrefab.Height - 6);
			this.rectButtonPrefabApply = new Rectangle(
				this.rectButtonsPrefab.X + (buttonWidth + buttonSpacing) * 2,
				this.rectButtonsPrefab.Y + 3,
				buttonWidth,
				this.rectButtonsPrefab.Height - 6);
			this.rectButtonPrefabBreak = new Rectangle(
				this.rectButtonsPrefab.X + (buttonWidth + buttonSpacing) * 3,
				this.rectButtonsPrefab.Y + 3,
				buttonWidth,
				this.rectButtonsPrefab.Height - 6);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			ControlRenderer.DrawGroupHeaderBackground(e.Graphics, this.rectHeader, SystemColors.Control, GroupHeaderStyle.Emboss);
			ControlRenderer.DrawGroupHeaderBackground(e.Graphics, this.rectPrefab, SystemColors.Control.ScaleBrightness(0.95f), GroupHeaderStyle.SmoothSunken);
			if (this.Focused)
				ControlRenderer.DrawBorder(e.Graphics, this.ClientRectangle, AdamsLair.WinForms.Renderer.BorderStyle.Simple, BorderState.Normal);

			CheckBoxState activeState = CheckBoxState.UncheckedDisabled;
			if (!this.ReadOnly && this.Enabled)
			{
				if (!this.active.HasValue)
				{
					if (this.activeCheckPressed)		activeState = CheckBoxState.MixedPressed;
					else if (this.activeCheckHovered)	activeState = CheckBoxState.MixedHot;
					else								activeState = CheckBoxState.MixedNormal;
				}
				else if (this.active.Value)
				{
					if (this.activeCheckPressed)		activeState = CheckBoxState.CheckedPressed;
					else if (this.activeCheckHovered)	activeState = CheckBoxState.CheckedHot;
					else								activeState = CheckBoxState.CheckedNormal;
				}
				else
				{
					if (this.activeCheckPressed)		activeState = CheckBoxState.UncheckedPressed;
					else if (this.activeCheckHovered)	activeState = CheckBoxState.UncheckedHot;
					else								activeState = CheckBoxState.UncheckedNormal;
				}
			}
			else
			{
				if (!this.active.HasValue)	activeState = CheckBoxState.MixedDisabled;
				else if (this.active.Value)	activeState = CheckBoxState.CheckedDisabled;
				else						activeState = CheckBoxState.UncheckedDisabled;
			}
			ControlRenderer.DrawCheckBox(e.Graphics, this.rectCheckActive.Location, activeState);

			Region[] nameLabelRegion = ControlRenderer.MeasureStringLine(e.Graphics, this.displayedName, new[] { new CharacterRange(0, this.displayedName.Length) }, headerNameFont, this.rectLabelName);
			ControlRenderer.DrawStringLine(e.Graphics, this.displayedName, headerNameFont, this.rectLabelName, SystemColors.ControlText);

			if (nameLabelRegion.Length > 0)
			{
				SizeF nameLabelSize = nameLabelRegion[0].GetBounds(e.Graphics).Size;
				Rectangle extLabelRect = new Rectangle(
					this.rectLabelName.X + (int)nameLabelSize.Width, 
					this.rectLabelName.Y, 
					this.rectLabelName.Width - (int)nameLabelSize.Width, 
					this.rectLabelName.Height);
				ControlRenderer.DrawStringLine(e.Graphics, this.displayedNameExt, headerNameExtFont, extLabelRect, SystemColors.ControlText);
			}

			ControlRenderer.DrawStringLine(e.Graphics, "PrefabLink", headerPrefabFont, this.rectLabelPrefab, !this.prefabLinked ? SystemColors.GrayText : (this.prefabLinkAvailable ? Color.Blue : Color.DarkRed));
			
			ButtonState buttonState = ButtonState.Disabled;
			ButtonState buttonStateDefault = ButtonState.Disabled;
			ButtonState buttonStateDefaultBreak = ButtonState.Disabled;
			if (!this.ReadOnly && this.Enabled && this.prefabLinked)
			{
				if (this.prefabLinkAvailable)
				{
					buttonState = ButtonState.Normal;
					buttonStateDefault = ButtonState.Normal;
				}
				buttonStateDefaultBreak = ButtonState.Normal;
			}

			if (this.curButtonPressed)		buttonState = ButtonState.Pressed;
			else if (this.curButtonHovered)	buttonState = ButtonState.Hot;

			ControlRenderer.DrawButton(e.Graphics, this.rectButtonPrefabShow, this.curButton == 0 ? buttonState : buttonStateDefault, "Show");
			ControlRenderer.DrawButton(e.Graphics, this.rectButtonPrefabRevert, this.curButton == 1 ? buttonState : buttonStateDefault, "Revert");
			ControlRenderer.DrawButton(e.Graphics, this.rectButtonPrefabApply, this.curButton == 2 ? buttonState : buttonStateDefault, "Apply");
			ControlRenderer.DrawButton(e.Graphics, this.rectButtonPrefabBreak, this.curButton == 3 ? buttonState : buttonStateDefaultBreak, "Break");
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.activeCheckHovered || this.curButtonHovered) this.Invalidate();
			this.activeCheckHovered = false;
			this.curButtonHovered = false;
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			bool lastActiveHovered = this.activeCheckHovered;
			this.activeCheckHovered = !this.ReadOnly && this.rectCheckActive.Contains(e.Location);
			if (lastActiveHovered != this.activeCheckHovered) this.Invalidate();

			bool lastButtonHovered = this.curButtonHovered;
			int lastButton = this.curButton;
			if (this.ReadOnly || !this.prefabLinked)
			{
				this.curButton = -1;
				this.curButtonHovered = false;
			}
			else if (this.prefabLinkAvailable && (!this.curButtonPressed || this.curButton == 0) && this.rectButtonPrefabShow.Contains(e.Location))
			{
				this.curButton = 0;
				this.curButtonHovered = true;
			}
			else if (this.prefabLinkAvailable && (!this.curButtonPressed || this.curButton == 1) && this.rectButtonPrefabRevert.Contains(e.Location))
			{
				this.curButton = 1;
				this.curButtonHovered = true;
			}
			else if (this.prefabLinkAvailable && (!this.curButtonPressed || this.curButton == 2) && this.rectButtonPrefabApply.Contains(e.Location))
			{
				this.curButton = 2;
				this.curButtonHovered = true;
			}
			else if ((!this.curButtonPressed || this.curButton == 3) && this.rectButtonPrefabBreak.Contains(e.Location))
			{
				this.curButton = 3;
				this.curButtonHovered = true;
			}
			else
			{
				this.curButton = -1;
				this.curButtonHovered = false;
			}
			if (lastActiveHovered != this.curButtonHovered || lastButton != this.curButton) this.Invalidate();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.curButtonHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.curButtonPressed = true;
				this.Invalidate();
				if (this.curButton == 0)		this.OnPrefabLinkShowPressed();
				else if (this.curButton == 1)	this.OnPrefabLinkRevertPressed();
				else if (this.curButton == 2)	this.OnPrefabLinkApplyPressed();
				else if (this.curButton == 3)	this.OnPrefabLinkBreakPressed();
			}
			if (this.activeCheckHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.activeCheckPressed = true;
				this.Invalidate();
				this.OnActiveCheckPressed();
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (this.curButtonPressed && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.curButtonPressed = false;
				this.Invalidate();
			}
			if (this.activeCheckPressed && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.activeCheckPressed = false;
				this.Invalidate();
			}
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				GameObject[] values = this.GetValue().Cast<GameObject>().ToArray();
				var actions = DualityEditorApp.GetEditorActions(typeof(GameObject), values, DualityEditorApp.ActionContextOpenRes);
				var action = actions.FirstOrDefault();
				if (action != null)
				{
					action.Perform(values);
				}
				e.Handled = true;
			}
			base.OnKeyDown(e);
		}

		protected void OnActiveCheckPressed()
		{
			if (!this.active.HasValue) this.active = true;
			this.active = !this.active.Value;
		    this.PerformSetActive(this.active.Value);
		    this.PerformGetValue();
		}

		private void OnPrefabLinkShowPressed()
		{
			GameObject[] values = this.GetValue().Cast<GameObject>().Where(o => o.PrefabLink != null).ToArray();
			Duality.Resources.PrefabLink link = values.First().PrefabLink;

			DualityEditorApp.Highlight(this, new ObjectSelection(link.Prefab.Res));
		}
		private void OnPrefabLinkRevertPressed()
		{
			UndoRedoManager.Do(new ResetGameObjectAction(this.GetValue().Cast<GameObject>()));

			this.PerformGetValue();
			this.ParentGrid.Invalidate();
		}
		private void OnPrefabLinkApplyPressed()
		{
			UndoRedoManager.Do(new ApplyToPrefabAction(this.GetValue().Cast<GameObject>()));

			this.ParentGrid.Invalidate();
		}
		private void OnPrefabLinkBreakPressed()
		{
			UndoRedoManager.Do(new BreakPrefabLinkAction(this.GetValue().Cast<GameObject>()));

			this.PerformGetValue();
			this.ParentGrid.Invalidate();
		}

		HelpInfo IHelpProvider.ProvideHoverHelp(Point localPos, ref bool captured)
		{
			HelpInfo result = null;

			if (this.rectHeader.Contains(localPos))
				result = HelpInfo.FromSelection(new ObjectSelection(this.GetValue()));
			else if (this.rectPrefab.Contains(localPos))
				result = HelpInfo.FromMember(typeof(Duality.Resources.PrefabLink));

			return result;
		}
	}
}
