using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DMR
{
	public class WaitForm : Form
	{
		private IContainer components;

		private Button btnClose;

		private Label lblInfo;

		private Timer tmrClose;

		public string Title
		{
			get;
			set;
		}

		public string Info
		{
			get;
			set;
		}

		public int Timeout
		{
			get;
			set;
		}

		public int Interval
		{
			get;
			set;
		}

		public int UseTime
		{
			get;
			set;
		}

		public WaitForm()
		{
			
			//base._002Ector();
		}

		public WaitForm(string info, int timeout)
		{
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
			this.Info = info;
			this.Interval = 1000;
			this.Timeout = timeout;
		}

		private void WaitForm_Load(object sender, EventArgs e)
		{
			this.UseTime = 0;
			this.Text = string.Format(" {0}s", (this.Timeout - this.UseTime) / 1000);
			this.lblInfo.Text = this.Info;
			this.lblInfo.Left = (base.Width - this.lblInfo.Width) / 2;
			this.tmrClose.Interval = this.Interval;
			this.tmrClose.Start();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void tmrClose_Tick(object sender, EventArgs e)
		{
			this.UseTime += this.Interval;
			if (this.UseTime >= this.Timeout)
			{
				base.Close();
			}
			else
			{
				this.Text = string.Format(" {0}s", (this.Timeout - this.UseTime) / 1000);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			this.btnClose = new Button();
			this.lblInfo = new Label();
			this.tmrClose = new Timer(this.components);
			base.SuspendLayout();
			this.btnClose.Location = new Point(149, 122);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new Size(75, 23);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "OK";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += this.btnClose_Click;
			this.lblInfo.AutoSize = true;
			this.lblInfo.Location = new Point(38, 50);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new Size(41, 12);
			this.lblInfo.TabIndex = 1;
			this.lblInfo.Text = "Prompt";
			this.tmrClose.Tick += this.tmrClose_Tick;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(373, 192);
			base.Controls.Add(this.lblInfo);
			base.Controls.Add(this.btnClose);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.Name = "WaitForm";
			this.Text = "WaitForm";
			base.Load += this.WaitForm_Load;
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
