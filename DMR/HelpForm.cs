using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class HelpForm : ToolWindow
	{
		//private IContainer components;

		private Panel pnlHelp;

		private WebBrowser wbHelp;

		public HelpForm()
		{

			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
			base.AllowEndUserDocking = true;
		}

		public void ShowHelp(string help)
		{
			this.wbHelp.Navigate(help);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.pnlHelp = new Panel();
			this.wbHelp = new WebBrowser();
			this.pnlHelp.SuspendLayout();
			base.SuspendLayout();
			this.pnlHelp.BorderStyle = BorderStyle.Fixed3D;
			this.pnlHelp.Controls.Add(this.wbHelp);
			this.pnlHelp.Dock = DockStyle.Fill;
			this.pnlHelp.Location = new Point(0, 0);
			this.pnlHelp.Name = "pnlHelp";
			this.pnlHelp.Size = new Size(284, 262);
			this.pnlHelp.TabIndex = 7;
			this.wbHelp.Dock = DockStyle.Fill;
			this.wbHelp.IsWebBrowserContextMenuEnabled = false;
			this.wbHelp.Location = new Point(0, 0);
			this.wbHelp.MinimumSize = new Size(20, 20);
			this.wbHelp.Name = "wbHelp";
			this.wbHelp.Size = new Size(280, 258);
			this.wbHelp.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(284, 262);
			base.Controls.Add(this.pnlHelp);
			this.Font = new Font("Arial", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Name = "HelpForm";
			base.ShowHint = DockState.DockBottomAutoHide;
			base.TabText = "HelpView";
			this.Text = "HelpView";
			this.pnlHelp.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
