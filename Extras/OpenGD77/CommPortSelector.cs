using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DMR
{
	public partial class CommPortSelector : Form
	{
		public string SelectedPort = null;
		public CommPortSelector()
		{
			InitializeComponent();
			List<string> ports = SetupDiWrap.GetComPortNames();
			foreach (string s in ports)
			{
				cmbPorts.Items.Add(s);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			if (cmbPorts.SelectedItem != null)
			{
				SelectedPort = cmbPorts.SelectedItem.ToString();
				this.DialogResult = DialogResult.OK;
			}
			this.Close();
		}
	}
}
