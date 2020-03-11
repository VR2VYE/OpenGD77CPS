using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DMR
{
	public class PasswordForm : Form
	{
		//private IContainer components;

		private Label lblPwd;

		private SGTextBox txtPwd;

		private Button btnOk;

		private Button btnCancel;

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.lblPwd = new Label();
			this.btnOk = new Button();
			this.btnCancel = new Button();
			this.txtPwd = new SGTextBox();
			base.SuspendLayout();
			this.lblPwd.AutoSize = true;
			this.lblPwd.Location = new Point(39, 50);
			this.lblPwd.Name = "lblPwd";
			this.lblPwd.Size = new Size(69, 16);
			this.lblPwd.TabIndex = 0;
			this.lblPwd.Text = "Password";
//			this.btnOk.DialogResult = DialogResult.OK;
			this.btnOk.Location = new Point(45, 102);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new Size(75, 23);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += this.btnOk_Click;
//			this.btnCancel.DialogResult = DialogResult.Cancel;
			this.btnCancel.Location = new Point(159, 102);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.txtPwd.InputString = null;
			this.txtPwd.Location = new Point(113, 47);
			this.txtPwd.MaxByteLength = 0;
			this.txtPwd.Name = "txtPwd";
			this.txtPwd.PasswordChar = '*';
			this.txtPwd.Size = new Size(129, 23);
			this.txtPwd.TabIndex = 1;
			base.AcceptButton = this.btnOk;
			base.AutoScaleDimensions = new SizeF(7f, 16f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new Size(268, 153);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.txtPwd);
			base.Controls.Add(this.lblPwd);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.Name = "PasswordForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Password";
			base.Load += this.PasswordForm_Load;
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public PasswordForm()
		{
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void PasswordForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_68(this);
			this.txtPwd.MaxByteLength = 8;
			this.txtPwd.InputString = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz\b";
			this.txtPwd.Text = Settings.CUR_PWD;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			Settings.CUR_PWD = this.txtPwd.Text;
		}
	}
}
