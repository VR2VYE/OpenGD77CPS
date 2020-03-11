using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class ToolWindow : DockContent
	{
		public ToolWindow()
		{
			this.InitializeComponent();
			base.ClientSize = new Size(292, 246);
			base.DockAreas = (DockAreas.Float | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom);
			base.Scale(Settings.smethod_6());
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.HideOnClose = true;
			base.Name = "ToolWindow";
			base.TabText = "ToolWindow";
			this.Text = "ToolWindow";
			base.ResumeLayout(false);
		}
	}
}
