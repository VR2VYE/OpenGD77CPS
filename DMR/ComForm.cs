using System;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace DMR
{
	public class ComForm : Form
	{
		//private IContainer components;

		private Label lblPort;

		private ComboBox cmbPort;

		private Button btnCancel;

		private Button btnOK;

		private Button btnRefresh;

		protected override void Dispose(bool disposing)
		{
            /*
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}*/
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.lblPort = new Label();
			this.cmbPort = new ComboBox();
			this.btnCancel = new Button();
			this.btnOK = new Button();
			this.btnRefresh = new Button();
			base.SuspendLayout();
			this.lblPort.Location = new Point(65, 80);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new Size(55, 24);
			this.lblPort.TabIndex = 0;
			this.lblPort.Text = "Port";
			this.lblPort.TextAlign = ContentAlignment.MiddleRight;
			this.cmbPort.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbPort.FormattingEnabled = true;
			this.cmbPort.Location = new Point(135, 80);
			this.cmbPort.Margin = new Padding(3, 4, 3, 4);
			this.cmbPort.Name = "cmbPort";
			this.cmbPort.Size = new Size(140, 24);
			this.cmbPort.TabIndex = 1;
			this.btnCancel.Location = new Point(210, 161);
			this.btnCancel.Margin = new Padding(3, 4, 3, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(87, 31);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += this.btnCancel_Click;
			this.btnOK.DialogResult = DialogResult.OK;
			this.btnOK.Location = new Point(65, 161);
			this.btnOK.Margin = new Padding(3, 4, 3, 4);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new Size(87, 31);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "Save";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += this.btnOK_Click;
			this.btnRefresh.Location = new Point(65, 219);
			this.btnRefresh.Margin = new Padding(3, 4, 3, 4);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new Size(87, 30);
			this.btnRefresh.TabIndex = 4;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += this.btnRefresh_Click;
			base.AutoScaleDimensions = new SizeF(7f, 16f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(362, 286);
			base.Controls.Add(this.btnRefresh);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.cmbPort);
			base.Controls.Add(this.lblPort);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Margin = new Padding(3, 4, 3, 4);
			base.Name = "ComForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Port Setting";
			base.Load += this.ComForm_Load;
			base.ResumeLayout(false);
		}

		public ComForm()
		{
			
			//base._002Ector();
			this.InitializeComponent();
			base.Scale(Settings.smethod_6());
		}

		private void method_0()
		{
			this.cmbPort.Items.Clear();
			string[] portNames = SerialPort.GetPortNames();
			string[] array = portNames;
			foreach (string item in array)
			{
				this.cmbPort.Items.Add(item);
			}
		}

		private void ComForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_68(this);
			this.method_0();
			this.cmbPort.SelectedItem = MainForm.CurCom;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				MainForm.CurCom = this.cmbPort.SelectedItem.ToString();
				//IniFileUtils.WriteProfileString("Setup", "Com", MainForm.CurCom);
				base.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			object selectedItem = this.cmbPort.SelectedItem;
			this.method_0();
			if (selectedItem != null && this.cmbPort.Items.Contains(selectedItem))
			{
				this.cmbPort.SelectedItem = selectedItem;
			}
			else if (this.cmbPort.Items.Count > 0)
			{
				this.cmbPort.SelectedIndex = 0;
			}
		}
	}
}
