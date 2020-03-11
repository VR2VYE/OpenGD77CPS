using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DMR
{
	public class AboutForm : Form
	{
		//private IContainer components;

		private Label lblVersion;

		private Label lblCompany;
		private Label lblTranslationCredit;

		private Button btnClose;

		public AboutForm()
		{
			
			//base._002Ector();
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void AboutForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_68(this);
#if OpenGD77
			this.lblVersion.Text = "OpenGD77 CPS";
#elif CP_VER_3_1_X
			this.lblVersion.Text = "GD-77 CPS 3.1.x Community Edition";
#endif
			this.lblCompany.Text += "\n\nRoger VK3KYY/G4KYF\nColin G4EML\nJason VK7ZJA\nMike DL2MF";
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		protected override void Dispose(bool disposing)
		{
		/*	if (disposing && this.components != null)
			{
				this.components.Dispose();
			}*/
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblCompany = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.lblTranslationCredit = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblVersion
			// 
			this.lblVersion.Location = new System.Drawing.Point(31, 20);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(351, 20);
			this.lblVersion.TabIndex = 0;
			this.lblVersion.Text = "v1.0.0";
			this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCompany
			// 
			this.lblCompany.AutoSize = true;
			this.lblCompany.Location = new System.Drawing.Point(100, 61);
			this.lblCompany.Name = "lblCompany";
			this.lblCompany.Size = new System.Drawing.Size(68, 16);
			this.lblCompany.TabIndex = 0;
			this.lblCompany.Text = "Company";
			this.lblCompany.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(173, 248);
			this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(64, 27);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "OK";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// lblTranslationCredit
			// 
			this.lblTranslationCredit.Location = new System.Drawing.Point(31, 204);
			this.lblTranslationCredit.Name = "lblTranslationCredit";
			this.lblTranslationCredit.Size = new System.Drawing.Size(351, 20);
			this.lblTranslationCredit.TabIndex = 0;
			this.lblTranslationCredit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AboutForm
			// 
			this.ClientSize = new System.Drawing.Size(409, 299);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.lblCompany);
			this.Controls.Add(this.lblTranslationCredit);
			this.Controls.Add(this.lblVersion);
			this.Font = new System.Drawing.Font("Arial", 10F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "AboutForm";
			this.Text = "About";
			this.Load += new System.EventHandler(this.AboutForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
