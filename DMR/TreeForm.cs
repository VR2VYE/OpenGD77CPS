using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class TreeForm : ToolWindow
	{
		//private IContainer components;

		protected override void Dispose(bool disposing)
		{
            /*
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
             * */
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(284, 262);
			base.DockAreas = (DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.Name = "TreeForm";
			base.Padding = new Padding(0, 2, 0, 2);
			base.ShowHint = DockState.DockLeftAutoHide;
			base.TabText = "TreeView";
			this.Text = "TreeView";
			base.ResumeLayout(false);
		}

		public TreeForm()
		{
			this.InitializeComponent();
			base.Scale(Settings.smethod_6());
		}
	}
}
