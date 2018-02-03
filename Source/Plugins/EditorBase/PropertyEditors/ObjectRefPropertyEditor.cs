using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;

using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.Drawing;
using ButtonState = AdamsLair.WinForms.Drawing.ButtonState;
using BorderStyle = AdamsLair.WinForms.Drawing.BorderStyle;

using Duality.Audio;
using Duality.Resources;
using Duality.Editor.Forms;
using Duality.Editor.Plugins.Base.Properties;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	public abstract class ObjectRefPropertyEditor : PropertyEditor
	{
		private static readonly IconImage iconSelect = new IconImage(Properties.EditorBaseResCache.IconReferenceInput);
		private static readonly IconImage iconReset  = new IconImage(Properties.EditorBaseResCache.IconAbortCross);

		protected	bool		multiple			= false;
		protected	bool		dragHover			= false;
		protected	Rectangle	rectPanel			= Rectangle.Empty;
		protected	Rectangle	rectButtonReset		= Rectangle.Empty;
		protected	Rectangle	rectButtonSelect	= Rectangle.Empty;
		protected	Rectangle	rectPrevSound		= Rectangle.Empty;
		protected	bool		buttonResetHovered	= false;
		protected	bool		buttonResetPressed	= false;
		protected	bool		buttonSelectHovered	= false;
		protected	bool		buttonSelectPressed	= false;
		protected	bool		panelHovered		= false;
		protected	Point		panelDragBegin		= Point.Empty;
		protected	Bitmap		prevImage			= null;
		protected	float		prevImageLum		= 0.0f;
		protected	Sound		prevSound			= null;
		protected	SoundInstance	prevSoundInst	= null;
		private		int			prevHash			= -1;

		public abstract string ReferenceName { get; }
		public abstract bool ReferenceBroken { get; }

		/// <summary>
		/// Type for this resource reference
		/// </summary>
		public abstract Type ReferenceType { get; }

		protected Type FilteredType
		{
			get
			{
				Type t = null;

				if (this.EditedMember != null)
				{
					PropertyInfo propertyInfo = this.EditedMember as PropertyInfo;
					FieldInfo fieldInfo = this.EditedMember as FieldInfo;
					if (propertyInfo != null)
					{
						t = propertyInfo.PropertyType;
					} else if (fieldInfo != null) {
						t = fieldInfo.FieldType;
					}
				}

				if (t == null)
				{
					t = this.EditedType != null ? this.EditedType : typeof(Component);
				}

				return t;
			}
		}

		public ObjectRefPropertyEditor()
		{
			this.Height = 22;
		}

		public abstract void ShowReferencedContent();
		public abstract void ResetReference();
		
		protected virtual int GetPreviewHash()
		{
			return 0;
		}
		protected virtual void GeneratePreview() { }
		/// <summary>
		/// Generates a new preview when required, which is determined by
		/// comparing the current <see cref="GetPreviewHash"/> value and the
		/// one from the last used preview.
		/// </summary>
		protected void SynchronizePreview()
		{
			int newPrevImageHash = this.GetPreviewHash();
			if (this.prevHash == newPrevImageHash) return;

			this.GeneratePreview();
			this.prevHash = newPrevImageHash;
		}

		public void PlayPreviewSound()
		{
			if (this.prevSound == null) return;
			if (this.prevSoundInst != null) return;

			this.prevSoundInst = DualityApp.Sound.PlaySound(this.prevSound);
			this.prevSoundInst.Looped = true;
			this.Invalidate();
		}
		public void StopPreviewSound()
		{
			if (this.prevSoundInst == null) return;
			
			this.prevSoundInst.FadeOut(0.25f);
			this.prevSoundInst = null;
			this.Invalidate();
		}

		protected abstract void SerializeToData(DataObject data);
		protected abstract void DeserializeFromData(DataObject data);
		protected abstract bool CanDeserializeFromData(DataObject data);

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			this.SynchronizePreview();

			bool linkBroken = this.ReferenceBroken;

			Color bgColorBright = Color.White;
			if (this.dragHover) bgColorBright = bgColorBright.MixWith(Color.FromArgb(192, 255, 0), 0.4f);
			else if (this.multiple) bgColorBright = Color.Bisque;
			else if (linkBroken) bgColorBright = Color.FromArgb(255, 128, 128);

			bool darkBg = false;
			Rectangle rectImage = new Rectangle(this.rectPanel.X + 2, this.rectPanel.Y + 2, this.rectPanel.Width - 4, this.rectPanel.Height - 4);
			if (this.prevImage == null)
			{
				if (this.ReadOnly || !this.Enabled)
					e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, bgColorBright)), rectImage);
				else
					e.Graphics.FillRectangle(new SolidBrush(bgColorBright), rectImage);
			}
			else
			{
				Color brightChecker = this.prevImageLum > 0.5f ? Color.FromArgb(48, 48, 48) : Color.FromArgb(224, 224, 224);
				Color darkChecker = this.prevImageLum > 0.5f ? Color.FromArgb(32, 32, 32) : Color.FromArgb(192, 192, 192);

				if (this.dragHover)
				{
					brightChecker = brightChecker.MixWith(Color.FromArgb(192, 255, 0), 0.4f);
					darkChecker = darkChecker.MixWith(Color.FromArgb(192, 255, 0), 0.4f);
				}
				else if (this.multiple)
				{
					brightChecker = brightChecker.MixWith(Color.FromArgb(255, 200, 128), 0.4f);
					darkChecker = darkChecker.MixWith(Color.FromArgb(255, 200, 128), 0.4f);
				}
				else if (linkBroken)
				{
					brightChecker = brightChecker.MixWith(Color.FromArgb(255, 128, 128), 0.4f);
					darkChecker = darkChecker.MixWith(Color.FromArgb(255, 128, 128), 0.4f);
				}

				e.Graphics.FillRectangle(new HatchBrush(HatchStyle.LargeCheckerBoard, brightChecker, darkChecker), rectImage);

				TextureBrush bgImageBrush = new TextureBrush(this.prevImage);
				bgImageBrush.ResetTransform();
				bgImageBrush.TranslateTransform(rectImage.X, rectImage.Y);
				e.Graphics.FillRectangle(bgImageBrush, rectImage);

				darkBg = this.prevImageLum > 0.5f;
				if (this.ReadOnly || !this.Enabled)
				{
					e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, SystemColors.Control)), rectImage);
					darkBg = (this.prevImageLum + SystemColors.Control.GetLuminance()) * 0.5f < 0.5f;
				}
			}
			
			if (this.prevSound != null)
			{
				if (this.prevSoundInst != null)
				{
					e.Graphics.DrawImage(darkBg ? EditorBaseResCache.IconSpeakerWhite : EditorBaseResCache.IconSpeakerBlack, 
						this.rectPrevSound.X, 
						this.rectPrevSound.Y);
				}
				else
				{
					e.Graphics.DrawImageAlpha(darkBg ? EditorBaseResCache.IconSpeakerWhite : EditorBaseResCache.IconSpeakerBlack, 
						0.5f,
						this.rectPrevSound.X, 
						this.rectPrevSound.Y);
				}
			}

			StringFormat format = StringFormat.GenericDefault;
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			format.Trimming = StringTrimming.EllipsisPath;
			SizeF textSize = e.Graphics.MeasureString(
				this.ReferenceName ?? "null",
				SystemFonts.DefaultFont,
				new SizeF(this.rectPanel.Width, this.rectPanel.Height),
				format);

			Rectangle rectText;
			if (this.prevImage == null)
				rectText = this.rectPanel;
			else
				rectText = new Rectangle(
					this.rectPanel.X, this.rectPanel.Bottom - (int)textSize.Height - 2, this.rectPanel.Width, (int)textSize.Height);

			if (this.prevImage != null)
			{
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(192, bgColorBright)), 
					rectText.X + rectText.Width / 2 - textSize.Width / 2 - 1, 
					rectText.Y + rectText.Height / 2 - textSize.Height / 2 - 2, 
					textSize.Width + 1, 
					textSize.Height + 2);
			}
			e.Graphics.DrawString(
				this.ReferenceName ?? "null",
				SystemFonts.DefaultFont,
				new SolidBrush(this.Enabled ? SystemColors.ControlText : SystemColors.GrayText),
				rectText,
				format);
			
			this.ControlRenderer.DrawBorder(e.Graphics, 
				this.rectPanel, 
				BorderStyle.ContentBox, 
				(this.ReadOnly || !this.Enabled) ? BorderState.Disabled : BorderState.Normal);

			ButtonState buttonStateReset = ButtonState.Disabled;
			if (!this.ReadOnly && this.Enabled && this.ReferenceName != null)
			{
				if (this.buttonResetPressed)		buttonStateReset = ButtonState.Pressed;
				else if (this.buttonResetHovered)	buttonStateReset = ButtonState.Hot;
				else								buttonStateReset = ButtonState.Normal;
			}
			this.ControlRenderer.DrawButton(
				e.Graphics, 
				this.rectButtonReset, 
				buttonStateReset, 
				null, 
				iconReset);

			ButtonState buttonStateSelect = ButtonState.Disabled;
			if (!this.ReadOnly && this.Enabled)
			{
				if (this.buttonSelectPressed) buttonStateSelect = ButtonState.Pressed;
				else if (this.buttonSelectHovered || this.Focused) buttonStateSelect = ButtonState.Hot;
				else buttonStateSelect = ButtonState.Normal;
			}
			this.ControlRenderer.DrawButton(
				e.Graphics, 
				this.rectButtonSelect, 
				buttonStateSelect, 
				null,
				iconSelect);
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.ShowContentSelectionDialog();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.C && e.Control)
			{
				DataObject data = new DataObject();
				this.SerializeToData(data);
				Clipboard.SetDataObject(data);
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.V && e.Control)
			{
				DataObject data = Clipboard.GetDataObject() as DataObject;
				this.DeserializeFromData(data);

				e.Handled = true;
			}
			base.OnKeyDown(e);
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			bool lastButtonResetHovered = this.buttonResetHovered;
			bool lastButtonSelectHovered = this.buttonSelectHovered;
			bool lastPanelHovered = this.panelHovered;

			this.buttonResetHovered = !this.ReadOnly && this.rectButtonReset.Contains(e.Location);
			this.buttonSelectHovered = !this.ReadOnly && this.rectButtonSelect.Contains(e.Location);
			this.panelHovered = this.rectPanel.Contains(e.Location);

			if (lastButtonResetHovered != this.buttonResetHovered ||
				lastButtonSelectHovered != this.buttonSelectHovered || 
				lastPanelHovered != this.panelHovered)
				this.Invalidate();

			if (this.rectPrevSound.Contains(e.Location))
				this.PlayPreviewSound();
			else
				this.StopPreviewSound();
			
			if (this.panelDragBegin != Point.Empty)
			{
				if (Math.Abs(this.panelDragBegin.X - e.X) > 5 || Math.Abs(this.panelDragBegin.Y - e.Y) > 5)
				{
					DataObject dragDropData = new DataObject();
					this.SerializeToData(dragDropData);
					//dragDropData.SetAllowedConvertOp(ConvertOperation.Operation.Convert);
					this.panelDragBegin = Point.Empty;
					this.ParentGrid.DoDragDrop(dragDropData, DragDropEffects.All | DragDropEffects.Link);
				}
			}
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.buttonResetHovered || this.buttonSelectHovered || this.panelHovered) this.Invalidate();
			this.buttonResetHovered = false;
			this.buttonSelectHovered = false;
			this.panelHovered = false;
			this.panelDragBegin = Point.Empty;
			this.StopPreviewSound();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.buttonResetHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.buttonResetPressed = true;
				this.Invalidate();
			}
			else if (this.buttonSelectHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.buttonSelectPressed = true;
				this.Invalidate();
			}
			else if (this.panelHovered)
			{
				this.panelDragBegin = e.Location;
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			bool leftMouseButtonUp = (e.Button & MouseButtons.Left) != MouseButtons.None;

			this.panelDragBegin = Point.Empty;
			if (this.buttonResetPressed && leftMouseButtonUp)
			{
				if (this.buttonResetPressed && this.buttonResetHovered) this.ResetReference();
				this.buttonResetPressed = false;
			}
			else if (this.buttonSelectPressed && leftMouseButtonUp)
			{
				if (this.buttonSelectPressed && this.buttonSelectHovered)
				{
					ShowContentSelectionDialog();
				}

				this.buttonSelectPressed = false;
			}
			else if (this.panelHovered && leftMouseButtonUp)
			{
				this.ShowReferencedContent();
			}

			this.Invalidate();
		}
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);

			if (this.panelHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				ShowContentSelectionDialog();
			}
		}

		protected void ShowContentSelectionDialog()
		{
			ObjectRefSelectionDialog resourceSelectionForm = new ObjectRefSelectionDialog
			{
				FilteredType = this.FilteredType,
				ResourcePath = this.ReferenceName
			};

			DialogResult result = resourceSelectionForm.ShowDialog();

			if (result == DialogResult.OK)
			{
				DataObject dataObject = new DataObject();

				if (resourceSelectionForm.ResourceReference != null)
				{
					dataObject.SetContentRefs(new[] { resourceSelectionForm.ResourceReference });
				}
				else if (this.ReferenceType == typeof(GameObject))
				{
					dataObject.SetGameObjectRefs(new[] { resourceSelectionForm.GameObjectReference });
				}
				else
				{
					dataObject.SetComponentRefs(new[] { resourceSelectionForm.ComponentReference });
				}

				DeserializeFromData(dataObject);
			}
		}

		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			if (this.ReadOnly || !this.Enabled) return;
			DataObject data = e.Data as DataObject;

			if (this.CanDeserializeFromData(data))
			{
				e.Effect = e.AllowedEffect;
				if (!this.dragHover) this.Invalidate();
				this.dragHover = true;
			}
		}
		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);
			if (this.ReadOnly || !this.Enabled) return;
			if (this.dragHover) this.Invalidate();
			this.dragHover = false;
		}
		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);
			if (this.ReadOnly || !this.Enabled) return;
			if (this.dragHover) this.Invalidate();
			this.dragHover = false;

			DataObject data = e.Data as DataObject;
			this.DeserializeFromData(data);
			e.Effect = e.AllowedEffect;
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			this.StopPreviewSound();
		}

		protected override void UpdateGeometry()
		{
			base.UpdateGeometry();

			int buttonWidth = 22;

			if (this.Height >= 44)
			{
				this.rectButtonSelect = new Rectangle(
					this.ClientRectangle.Right - buttonWidth,
					this.ClientRectangle.Top + this.ClientRectangle.Height / 2 - buttonWidth,
					buttonWidth,
					buttonWidth);
				this.rectButtonReset = new Rectangle(
					this.ClientRectangle.Right - buttonWidth,
					this.rectButtonSelect.Bottom,
					buttonWidth,
					buttonWidth);
				this.rectPanel = new Rectangle(
					this.ClientRectangle.X,
					this.ClientRectangle.Y,
					this.ClientRectangle.Width - buttonWidth,
					this.ClientRectangle.Height);
			}
			else
			{
				this.rectButtonSelect = new Rectangle(
					this.ClientRectangle.Right - buttonWidth - buttonWidth,
					this.ClientRectangle.Top,
					buttonWidth,
					this.ClientRectangle.Height);
				this.rectButtonReset = new Rectangle(
					this.ClientRectangle.Right - buttonWidth,
					this.ClientRectangle.Top,
					buttonWidth,
					this.ClientRectangle.Height);
				this.rectPanel = new Rectangle(
					this.ClientRectangle.X,
					this.ClientRectangle.Y,
					this.ClientRectangle.Width - buttonWidth * 2,
					this.ClientRectangle.Height);
			}

			this.rectPrevSound = new Rectangle(this.rectPanel.Right - 17, this.rectPanel.Y + 1, 16, 16);
		}
		
		private void prevSoundTimer_Tick(object sender, EventArgs e)
		{
			this.PlayPreviewSound();
		}
	}
}

