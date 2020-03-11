using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DMR
{
	public class CommPrgForm : Form
	{
		private Label lblPrompt;
		private ProgressBar prgComm;
		private Button btnCancel;
        private Button btnOK;
		private CodeplugComms hidComm;
		private bool _closeWhenFinished = false;

		public bool IsSucess
		{
			get;
			set;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.lblPrompt = new Label();
			this.prgComm = new ProgressBar();
			this.btnCancel = new Button();
            this.btnOK = new Button();
			base.SuspendLayout();
			this.lblPrompt.BorderStyle = BorderStyle.Fixed3D;
			this.lblPrompt.Location = new Point(43, 118);
			this.lblPrompt.Name = "lblPrompt";
			this.lblPrompt.Size = new Size(380, 26);
			this.lblPrompt.TabIndex = 0;
            this.lblPrompt.TextAlign = ContentAlignment.MiddleCenter;

			this.prgComm.Location = new Point(43, 70);
			this.prgComm.Margin = new Padding(3, 4, 3, 4);
			this.prgComm.Name = "prgComm";
			this.prgComm.Size = new Size(380, 31);
			this.prgComm.TabIndex = 1;

			this.btnCancel.Location = new Point(184, 161);
			this.btnCancel.Margin = new Padding(3, 4, 3, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(87, 31);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            this.btnOK.Location = new Point(336, 161);
            this.btnOK.Margin = new Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(87, 31);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnOK.Visible = false;


			base.AutoScaleDimensions = new SizeF(7f, 16f);
			base.ClientSize = new Size(468, 214);
			base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
			base.Controls.Add(this.prgComm);
			base.Controls.Add(this.lblPrompt);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
//			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.Margin = new Padding(3, 4, 3, 4);
			this.Name = "CommPrgForm";
			this.ShowInTaskbar = false;
			this.Load += this.CommPrgForm_Load;
			this.FormClosing += this.CommPrgForm_FormClosing;
			this.ResumeLayout(false);
		}

		public CommPrgForm(bool closeWhenFinished = false)
		{

			this._closeWhenFinished = closeWhenFinished;
			this.hidComm = new CodeplugComms();
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void CommPrgForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_68(this);
			this.prgComm.Minimum = 0;
			this.prgComm.Maximum = 100;

			switch (CodeplugComms.CommunicationMode)
			{
				case CodeplugComms.CommunicationType.codeplugRead:
					this.Text = Settings.dicCommon["CodeplugRead"];
					break;
				case CodeplugComms.CommunicationType.DMRIDRead:
					this.Text = Settings.dicCommon["DMRIDRead"];
					break;
				case CodeplugComms.CommunicationType.calibrationRead:
					this.Text = Settings.dicCommon["CalibrationRead"];
					break;

				case CodeplugComms.CommunicationType.codeplugWrite:
					this.Text = Settings.dicCommon["CodeplugWrite"];
					break;
				case CodeplugComms.CommunicationType.DMRIDWrite:
					this.Text = Settings.dicCommon["DMRIDWrite"];
					break;
				case CodeplugComms.CommunicationType.calibrationWrite:
					this.Text = Settings.dicCommon["CalibrationWrite"];
					break;
				case CodeplugComms.CommunicationType.dataRead:
					this.Text = Settings.dicCommon["dataRead"];
					break;
				case CodeplugComms.CommunicationType.dataWrite:
					this.Text = Settings.dicCommon["dataWrite"];
					break;
			}

			switch (CodeplugComms.CommunicationMode)
			{
				case CodeplugComms.CommunicationType.codeplugRead:
				case CodeplugComms.CommunicationType.DMRIDRead:
				case CodeplugComms.CommunicationType.calibrationRead:
					this.hidComm.START_ADDR = new int[7]
					{
						128,
						304,
						21392,
						29976,
						32768,
						44816,
						95776
					};
					this.hidComm.END_ADDR = new int[7]
					{
						297,
						14208,
						22056,
						30208,
						32784,
						45488,
						126624
					};
					break;

				case CodeplugComms.CommunicationType.codeplugWrite:
				case CodeplugComms.CommunicationType.DMRIDWrite:
				case CodeplugComms.CommunicationType.calibrationWrite:
					this.hidComm.START_ADDR = new int[7]
					{
						128,
						304,
						21392,
						29976,
						32768,
						44816,
						95776
					};
					this.hidComm.END_ADDR = new int[7]
					{
						297,
						14208,
						22056,
						30208,
						32784,
						45488,
						126624
					};
					break;
			}


			this.hidComm.SetProgressCallback(this.progressCallback);
            this.hidComm.startCodeplugReadOrWriteInNewThread();
		}

		private void CommPrgForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.hidComm.isThreadAlive())
			{
				this.hidComm.SetCancelComm(true);
				this.hidComm.JoinThreadIfAlive();
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			base.Close();
		}

        private void btnOK_Click(object sender, EventArgs e)
        {
			this.DialogResult = DialogResult.OK;
            base.Close();
        }


		private void progressCallback(object sender, FirmwareUpdateProgressEventArgs e)
		{
			if (this.prgComm.InvokeRequired)
			{
				base.BeginInvoke(new EventHandler<FirmwareUpdateProgressEventArgs>(this.progressCallback), sender, e);
			}
			else if (e.Failed)
			{
				if (!string.IsNullOrEmpty(e.Message))
				{
					MessageBox.Show(e.Message, Settings.SZ_PROMPT);
				}
				base.Close();
			}
			else if (e.Closed)
			{
                /* Roger Clark. Prevent the form closing immediatly after Read or Write complete
				this.Refresh();
				base.Close();
                */
			}
			else
			{
				this.prgComm.Value = (int)e.Percentage;
	            if (e.Percentage == (float)this.prgComm.Maximum)
                {
                    this.IsSucess = true;

					if (_closeWhenFinished)
					{
						this.DialogResult = DialogResult.OK;
						this.Close();
						return;
					}

					switch (CodeplugComms.CommunicationMode)
					{
						case CodeplugComms.CommunicationType.codeplugRead:
						case CodeplugComms.CommunicationType.DMRIDRead:
						case CodeplugComms.CommunicationType.calibrationRead:
							this.lblPrompt.Text = Settings.dicCommon["ReadComplete"];
							break;
						case CodeplugComms.CommunicationType.codeplugWrite:
						case CodeplugComms.CommunicationType.DMRIDWrite:
						case CodeplugComms.CommunicationType.calibrationWrite:
							this.lblPrompt.Text = Settings.dicCommon["WriteComplete"];
							break;
					}
					/*
					if (this.IsRead)
					{
						//MessageBox.Show(Class15.dicCommon["ReadComplete"]);
						this.lblPrompt.Text = Settings.dicCommon["WriteComplete"];
					}
					else
					{
						//MessageBox.Show(Class15.dicCommon["WriteComplete"]);
						this.lblPrompt.Text = Settings.dicCommon["WriteComplete"];
					}
					 */
                    this.btnOK.Visible = true;
                    this.btnCancel.Visible = false;
                     
                }
                else
                {
                    this.lblPrompt.Text = string.Format("{0}%", this.prgComm.Value);
                }
			}
		}
	}
}
