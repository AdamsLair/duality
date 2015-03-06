using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Net;

using Duality.Editor.Properties;

namespace Duality.Editor.Forms
{
	public partial class LicenseAcceptDialog : Form
	{
		private Uri licenseUrl = null;
		private Uri retrievedTextUrl = null;

		public string DescriptionText
		{
			get { return this.labelDescription.Text; }
			set { this.labelDescription.Text = value; }
		}
		public Uri LicenseUrl
		{
			get { return this.licenseUrl; }
			set
			{
				if (this.licenseUrl != value)
				{
					this.licenseUrl = value;
					this.linkLabelLicenseUrl.Enabled = this.licenseUrl != null;
					this.RetrieveLicenseText();
				}
			}
		}

		public LicenseAcceptDialog()
		{
			this.InitializeComponent();
			this.textBoxLicenseText.ScrollBars = ScrollBars.Both;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.RetrieveLicenseText();
		}

		private void linkLabelLicenseUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(this.licenseUrl.AbsoluteUri);
		}
		private void buttonDecline_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		private void buttonAgree_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void RetrieveLicenseText()
		{
			if (this.retrievedTextUrl == this.licenseUrl) return;
			this.retrievedTextUrl = this.licenseUrl;

			string licenseTranscript = null;
			if (this.licenseUrl != null)
			{
				try
				{
					Uri rawTextUrl = this.GetRawLicenseTextUrl(this.licenseUrl);
					HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(rawTextUrl);
					httpWebRequest.Timeout = 3000;
					using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse())
					{
						using (Stream stream = httpWebReponse.GetResponseStream())
						using (StreamReader reader = new StreamReader(stream))
						{
							string rawText = reader.ReadToEnd().Trim();

							bool isRawText = true;

							// If it contains certain control characters, it's likely binary
							if (rawText.Contains('\0'))
								isRawText = false;
							// If it contains certain HTML marks, it's not raw text either
							else if (rawText.StartsWith("<!DOCTYPE html>", StringComparison.InvariantCultureIgnoreCase))
								isRawText = false;
							else if (
								rawText.IndexOf("<html>", StringComparison.OrdinalIgnoreCase) >= 0 &&
								rawText.IndexOf("</html>", StringComparison.OrdinalIgnoreCase) >= 0)
								isRawText = false;

							// Is it likely raw text? Fix line breaks and display it then.
							if (isRawText)
							{ 
								string transformedText = Regex.Replace(rawText, @"\r\n?|\n", Environment.NewLine);
								licenseTranscript = transformedText;
							}
						}
					}
				}
				catch (Exception)
				{
					licenseTranscript = null;
				}
			}

			if (!string.IsNullOrWhiteSpace(licenseTranscript))
			{
				this.textBoxLicenseText.Text = licenseTranscript;
				if (!this.textBoxLicenseText.Enabled)
				{
					this.labelTranscriptInfo.Visible = true;
					this.textBoxLicenseText.Visible = true;
					this.Height += this.labelTranscriptInfo.Height + this.textBoxLicenseText.Height;
				}
				this.textBoxLicenseText.Enabled = true;
			}
			else
			{ 
				this.textBoxLicenseText.Text = GeneralRes.LicenseAcceptDialog_TranscriptUnavailable;
				if (this.textBoxLicenseText.Enabled)
				{
					this.Height -= this.labelTranscriptInfo.Height + this.textBoxLicenseText.Height;
					this.labelTranscriptInfo.Visible = false;
					this.textBoxLicenseText.Visible = false;
				}
				this.textBoxLicenseText.Enabled = false;
			}
		}
		private Uri GetRawLicenseTextUrl(Uri url)
		{
			if (url.Host == "github.com" && url.Segments.Contains("blob/"))
			{
				UriBuilder builder = new UriBuilder(url);
				builder.Path = builder.Path.Replace("/blob/", "/raw/");
				url = builder.Uri;
			}
			return url;
		}
	}
}
