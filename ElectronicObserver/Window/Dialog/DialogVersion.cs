using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogVersion : Form
	{
		public DialogVersion()
		{
			InitializeComponent();

			TextVersion.Text = string.Format("{0} {1} ({2} release)", SoftwareInformation.SoftwareNameEnglish, SoftwareInformation.VersionEnglish, SoftwareInformation.UpdateTime.ToString("d"));
		}

		private void TextAuthor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

			System.Diagnostics.Process.Start("https://twitter.com/andanteyk");

		}

		private void ButtonClose_Click(object sender, EventArgs e)
		{

			this.Close();

		}

		private void TextInformation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

			System.Diagnostics.Process.Start("https://github.com/gre4bee/ElectronicObserver");

		}

		private void DialogVersion_Load(object sender, EventArgs e)
		{

			this.Icon = ResourceManager.Instance.AppIcon;
		}


    }
}
