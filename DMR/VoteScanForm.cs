using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DMR
{
	public class VoteScanForm : Form
	{
		private byte[] data;

		//private IContainer components;

		private CheckBox chkTalkback;

		private CheckBox chkChMark;

		private CheckBox chkEarlyUnmute;

		private Label lblTxDesignatedCh;

		private Label lblPretimeDelay;

		private ComboBox comTxDesignatedCh;

		private ListBox lstUnselected;

		private ListBox lstSelected;

		private Button btnAdd;

		private Button button2;

		private NumericUpDown nudPretimeDelay;

		public VoteScanForm()
		{
			this.data = new byte[40];
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void VoteScanForm_Load(object sender, EventArgs e)
		{
			try
			{
				Settings.smethod_59(base.Controls);
				Settings.smethod_68(this);
				this.chkChMark.Checked = Convert.ToBoolean(this.data[37] & 0x10);
				this.chkTalkback.Checked = Convert.ToBoolean(this.data[37] & 0x80);
				this.chkEarlyUnmute.Checked = Convert.ToBoolean(this.data[37] & 8);
				this.nudPretimeDelay.Value = this.data[38] * 25;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void VoteScanForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.chkChMark.Checked)
				{
					this.data[37] |= 16;
				}
				else
				{
					this.data[37] &= 239;
				}
				if (this.chkTalkback.Checked)
				{
					this.data[37] |= 128;
				}
				else
				{
					this.data[37] &= 127;
				}
				if (this.chkEarlyUnmute.Checked)
				{
					this.data[37] |= 8;
				}
				else
				{
					this.data[37] &= 247;
				}
				this.data[38] = (byte)(this.nudPretimeDelay.Value / 25m);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

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
			this.chkTalkback = new CheckBox();
			this.chkChMark = new CheckBox();
			this.chkEarlyUnmute = new CheckBox();
			this.lblTxDesignatedCh = new Label();
			this.lblPretimeDelay = new Label();
			this.comTxDesignatedCh = new ComboBox();
			this.lstUnselected = new ListBox();
			this.lstSelected = new ListBox();
			this.btnAdd = new Button();
			this.button2 = new Button();
			this.nudPretimeDelay = new NumericUpDown();
			((ISupportInitialize)this.nudPretimeDelay).BeginInit();
			base.SuspendLayout();
			this.chkTalkback.AutoSize = true;
			this.chkTalkback.Location = new Point(249, 322);
			this.chkTalkback.Name = "chkTalkback";
			this.chkTalkback.Size = new Size(82, 20);
			this.chkTalkback.TabIndex = 0;
			this.chkTalkback.Text = "Talkback";
			this.chkTalkback.UseVisualStyleBackColor = true;
			this.chkChMark.AutoSize = true;
			this.chkChMark.Location = new Point(249, 352);
			this.chkChMark.Name = "chkChMark";
			this.chkChMark.Size = new Size(128, 20);
			this.chkChMark.TabIndex = 1;
			this.chkChMark.Text = "Channel Marker";
			this.chkChMark.UseVisualStyleBackColor = true;
			this.chkEarlyUnmute.AutoSize = true;
			this.chkEarlyUnmute.Location = new Point(249, 442);
			this.chkEarlyUnmute.Name = "chkEarlyUnmute";
			this.chkEarlyUnmute.Size = new Size(111, 20);
			this.chkEarlyUnmute.TabIndex = 1;
			this.chkEarlyUnmute.Text = "Early Unmute";
			this.chkEarlyUnmute.UseVisualStyleBackColor = true;
			this.lblTxDesignatedCh.Location = new Point(83, 382);
			this.lblTxDesignatedCh.Name = "lblTxDesignatedCh";
			this.lblTxDesignatedCh.Size = new Size(156, 24);
			this.lblTxDesignatedCh.TabIndex = 2;
			this.lblTxDesignatedCh.Text = "Tx Designated Channel";
			this.lblTxDesignatedCh.TextAlign = ContentAlignment.MiddleRight;
			this.lblPretimeDelay.Location = new Point(83, 412);
			this.lblPretimeDelay.Name = "lblPretimeDelay";
			this.lblPretimeDelay.Size = new Size(156, 24);
			this.lblPretimeDelay.TabIndex = 2;
			this.lblPretimeDelay.Text = "Pretime Delay [ms]";
			this.lblPretimeDelay.TextAlign = ContentAlignment.MiddleRight;
			this.comTxDesignatedCh.FormattingEnabled = true;
			this.comTxDesignatedCh.Location = new Point(249, 382);
			this.comTxDesignatedCh.Name = "comTxDesignatedCh";
			this.comTxDesignatedCh.Size = new Size(121, 24);
			this.comTxDesignatedCh.TabIndex = 3;
			this.lstUnselected.FormattingEnabled = true;
			this.lstUnselected.ItemHeight = 16;
			this.lstUnselected.Location = new Point(63, 49);
			this.lstUnselected.Name = "lstUnselected";
			this.lstUnselected.Size = new Size(120, 244);
			this.lstUnselected.TabIndex = 4;
			this.lstSelected.FormattingEnabled = true;
			this.lstSelected.ItemHeight = 16;
			this.lstSelected.Location = new Point(348, 49);
			this.lstSelected.Name = "lstSelected";
			this.lstSelected.Size = new Size(120, 244);
			this.lstSelected.TabIndex = 4;
			this.btnAdd.Location = new Point(224, 92);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new Size(75, 23);
			this.btnAdd.TabIndex = 5;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.button2.Location = new Point(224, 122);
			this.button2.Name = "button2";
			this.button2.Size = new Size(75, 23);
			this.button2.TabIndex = 5;
			this.button2.Text = "Delete";
			this.button2.UseVisualStyleBackColor = true;
			this.nudPretimeDelay.Increment = new decimal(new int[4]
			{
				25,
				0,
				0,
				0
			});
			this.nudPretimeDelay.Location = new Point(249, 412);
			this.nudPretimeDelay.Maximum = new decimal(new int[4]
			{
				1500,
				0,
				0,
				0
			});
			this.nudPretimeDelay.Name = "nudPretimeDelay";
			this.nudPretimeDelay.Size = new Size(120, 23);
			this.nudPretimeDelay.TabIndex = 6;
			base.AutoScaleDimensions = new SizeF(7f, 16f);
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(531, 507);
			base.Controls.Add(this.nudPretimeDelay);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.btnAdd);
			base.Controls.Add(this.lstSelected);
			base.Controls.Add(this.lstUnselected);
			base.Controls.Add(this.comTxDesignatedCh);
			base.Controls.Add(this.lblPretimeDelay);
			base.Controls.Add(this.lblTxDesignatedCh);
			base.Controls.Add(this.chkEarlyUnmute);
			base.Controls.Add(this.chkChMark);
			base.Controls.Add(this.chkTalkback);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.Name = "VoteScanForm";
			this.Text = "Vote Scan";
			base.Load += this.VoteScanForm_Load;
			base.FormClosing += this.VoteScanForm_FormClosing;
			((ISupportInitialize)this.nudPretimeDelay).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
