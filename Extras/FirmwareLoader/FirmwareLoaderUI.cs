using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace DMR
{
	public partial class FirmwareLoaderUI : Form
	{
		private string _filename;
		public bool IsLoading = false;
		public FirmwareLoaderUI(string filename)
		{
			_filename = filename;
			InitializeComponent();
			this.lblMessage.Text = "";
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
		}

		public void SetLabel(string txt)
		{
			if (this.lblMessage.InvokeRequired)
			{
				this.Invoke(new Action(() => SetLabel(txt)));
			}
			else
			{
				this.lblMessage.Text = txt;
			}
		}
		public void SetProgressPercentage(int perc)
		{
			if (this.progressBar1.InvokeRequired)
			{
				this.Invoke(new Action(() => SetProgressPercentage(perc)));
			}
			else
			{
				this.progressBar1.Value = perc;
			}
		}

		private void FirmwareLoaderUI_Load(object sender, EventArgs e)
		{

		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			if (!IsLoading)
			{
				this.Close();
			}
			else
			{
				MessageBox.Show("You can't interrupt the upload process");
			}
		}

		private void btnUploadFirmware_Click(object sender, EventArgs e)
		{
			if (IsLoading)
			{
				return;
			}

			//if (MessageBox.Show("This feature is experimental.\nYou use it at your own risk\n\nAre you sure you want to upload firmware to the radio?","Warning", MessageBoxButtons.YesNo)==DialogResult.Yes)
			{
				Action<object> action = (object obj) =>
				{
					IsLoading = true;
					FirmwareLoader.UploadFirmare(_filename, this);
					IsLoading = false;
				};
				try
				{
					Task t1 = new Task(action, "LoaderUSB");
					t1.Start();
				}
				catch (Exception)
				{
					IsLoading = false;
				}
			}
		}

		private void FirmwareLoaderUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (IsLoading)
			{
				MessageBox.Show("You can't interrupt the upload process");
			}
			e.Cancel = IsLoading;
		}
	}
}

