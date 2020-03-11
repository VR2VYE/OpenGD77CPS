using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class ZoneBasicForm : DockContent, IDisp
	{
		//private IContainer components;

		private CustomCombo cmbMainZone;
		private Label lblCurZone;
		private CustomCombo cmbSubCh;
		private Label lblSubCh;
		private CustomCombo cmbMainCh;
		private Label lblMainCh;
		private CustomCombo cmbSubZone;
		private Label lblSubZone;
		private CustomPanel pnlZoneBasic;
		private GroupBox grpSub;
		private GroupBox grpMain;

		const int ZONE_NAME_LENGTH = 16;
		const int ZONES_IN_USE_DATA_LENGTH = 32;
#if OpenGD77
		const int NUM_CHANNELS_PER_ZONE	= 80;
		const int NUM_ZONES = 68;
#elif CP_VER_3_1_X
		const int NUM_CHANNELS_PER_ZONE	= 16;
		const int NUM_ZONES				= 250;
#endif
		const int UNKNOWN_VAR_OF_32 = NUM_CHANNELS_PER_ZONE + ZONE_NAME_LENGTH;

		public int MainZoneLastSelectedIndex
		{
			get;
			set;
		}

		public int SubZoneLastSelectedIndex
		{
			get;
			set;
		}

		public TreeNode Node
		{
			get;
			set;
		}

		public void SaveData()
		{
			if (ChannelForm.CurCntCh > 128)
			{
				ZoneForm.basicData.CurZone = this.cmbMainZone.method_3();
				ZoneForm.basicData.MainCh = this.cmbMainCh.method_3();
				ZoneForm.basicData.SubCh = this.cmbSubCh.method_3();
				ZoneForm.basicData.SubZone = this.cmbSubZone.method_3();
			}
			else
			{
				ZoneForm.basicData.MainCh = this.cmbMainCh.method_3();
				ZoneForm.basicData.SubCh = this.cmbSubCh.method_3();
			}
			((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType());
		}

		public void DispData()
		{
			this.method_0();
			int num = 0;
			this.method_2(this.cmbMainZone);
			num = ZoneForm.basicData.CurZone;
			this.cmbMainZone.method_2(ZoneForm.basicData.CurZone);
			this.method_3(num);
			this.cmbMainCh.method_2(ZoneForm.basicData.MainCh);
			int num2 = 0;
			this.method_2(this.cmbSubZone);
			num2 = ZoneForm.basicData.SubZone;
			this.cmbSubZone.method_2(ZoneForm.basicData.SubZone);
			this.method_4(num2);
			this.cmbSubCh.method_2(ZoneForm.basicData.SubCh);
		}

		public void RefreshName()
		{
		}

		public ZoneBasicForm()
		{
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void method_0()
		{
		}

		private void method_1()
		{
			this.method_2(this.cmbMainZone);
			this.method_2(this.cmbSubZone);
		}

		private void method_2(CustomCombo class4_0)
		{
			int num = 0;
			class4_0.method_0();
			for (num = 0; num <= 250; num++)
			{
				if (ZoneForm.data.ZoneChIsValid(num))
				{
					class4_0.method_1(ZoneForm.data.GetName(num), num);
				}
			}
		}

		private void method_3(int int_0)
		{
			this.method_5(int_0, this.cmbMainCh);
		}

		private void method_4(int int_0)
		{
			this.method_5(int_0, this.cmbSubCh);
		}

		private void method_5(int int_0, CustomCombo class4_0)
		{
			int num = 0;
			int num2 = 0;
			class4_0.method_0();
			for (num = 0; num < NUM_CHANNELS_PER_ZONE; num++)
			{
				num2 = ZoneForm.data[int_0].ChList[num] - 1;
				if (num2 >= 0 && num2 < ChannelForm.CurCntCh && ChannelForm.data.DataIsValid(num2))
				{
					class4_0.method_1(ChannelForm.data.GetName(num2), num);
				}
			}
		}

		private void method_6()
		{
			int num = 0;
			this.cmbSubCh.method_0();
			for (num = 0; num < ChannelForm.CurCntCh; num++)
			{
				if (ChannelForm.data.DataIsValid(num))
				{
					this.cmbSubCh.method_1(ChannelForm.data.GetName(num), num + 1);
				}
			}
		}

		private void ZoneBasicForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);
			this.DispData();
		}

		private void ZoneBasicForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
		}

		private void cmbMainZone_SelectedIndexChanged(object sender, EventArgs e)
		{
			int num = this.cmbMainZone.method_3();
			if (num != this.MainZoneLastSelectedIndex)
			{
				this.method_3(num);
				if (this.cmbMainCh.Items.Count > 0)
				{
					this.cmbMainCh.SelectedIndex = 0;
				}
			}
		}

		private void cmbSubZone_SelectedIndexChanged(object sender, EventArgs e)
		{
			int num = this.cmbSubZone.method_3();
			if (num != this.SubZoneLastSelectedIndex)
			{
				this.method_4(num);
				if (this.cmbSubCh.Items.Count > 0)
				{
					this.cmbSubCh.SelectedIndex = 0;
				}
			}
		}

		private void cmbMainZone_DropDown(object sender, EventArgs e)
		{
			this.MainZoneLastSelectedIndex = this.cmbMainZone.method_3();
		}

		private void cmbSubZone_DropDown(object sender, EventArgs e)
		{
			this.SubZoneLastSelectedIndex = this.cmbSubZone.method_3();
		}

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
			this.pnlZoneBasic = new CustomPanel();
			this.grpSub = new GroupBox();
			this.cmbSubCh = new CustomCombo();
			this.lblSubCh = new Label();
			this.cmbSubZone = new CustomCombo();
			this.lblSubZone = new Label();
			this.grpMain = new GroupBox();
			this.cmbMainCh = new CustomCombo();
			this.lblMainCh = new Label();
			this.cmbMainZone = new CustomCombo();
			this.lblCurZone = new Label();
			this.pnlZoneBasic.SuspendLayout();
			this.grpSub.SuspendLayout();
			this.grpMain.SuspendLayout();
			base.SuspendLayout();
			this.pnlZoneBasic.AutoScroll = true;
			this.pnlZoneBasic.AutoSize = true;
			this.pnlZoneBasic.Controls.Add(this.grpSub);
			this.pnlZoneBasic.Controls.Add(this.grpMain);
			this.pnlZoneBasic.Dock = DockStyle.Fill;
			this.pnlZoneBasic.Location = new Point(0, 0);
			this.pnlZoneBasic.Name = "pnlZoneBasic";
			this.pnlZoneBasic.Size = new Size(600, 197);
			this.pnlZoneBasic.TabIndex = 0;

			this.grpSub.Controls.Add(this.cmbSubCh);
			this.grpSub.Controls.Add(this.lblSubCh);
			this.grpSub.Controls.Add(this.cmbSubZone);
			this.grpSub.Controls.Add(this.lblSubZone);
			this.grpSub.Location = new Point(300, 35);
			this.grpSub.Name = "grpSub";
			this.grpSub.Size = new Size(250, 129);
			this.grpSub.TabIndex = 7;
			this.grpSub.TabStop = false;
			this.grpSub.Text = "Down";
			this.cmbSubCh.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbSubCh.FormattingEnabled = true;
			this.cmbSubCh.Location = new Point(98, 71);
			this.cmbSubCh.Name = "cmbSubCh";
			this.cmbSubCh.Size = new Size(120, 24);
			this.cmbSubCh.TabIndex = 5;
			this.lblSubCh.Location = new Point(19, 71);
			this.lblSubCh.Name = "lblSubCh";
			this.lblSubCh.Size = new Size(70, 24);
			this.lblSubCh.TabIndex = 4;
			this.lblSubCh.Text = "Channel";
			this.lblSubCh.TextAlign = ContentAlignment.MiddleRight;
			this.cmbSubZone.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbSubZone.FormattingEnabled = true;
			this.cmbSubZone.Location = new Point(98, 37);
			this.cmbSubZone.Name = "cmbSubZone";
			this.cmbSubZone.Size = new Size(120, 24);
			this.cmbSubZone.TabIndex = 1;
			this.cmbSubZone.SelectedIndexChanged += this.cmbSubZone_SelectedIndexChanged;
			this.cmbSubZone.DropDown += this.cmbSubZone_DropDown;
			this.lblSubZone.Location = new Point(19, 37);
			this.lblSubZone.Name = "lblSubZone";
			this.lblSubZone.Size = new Size(70, 24);
			this.lblSubZone.TabIndex = 0;
			this.lblSubZone.Text = "Zone";
			this.lblSubZone.TextAlign = ContentAlignment.MiddleRight;
			this.grpMain.Controls.Add(this.cmbMainCh);
			this.grpMain.Controls.Add(this.lblMainCh);
			this.grpMain.Controls.Add(this.cmbMainZone);
			this.grpMain.Controls.Add(this.lblCurZone);
			this.grpMain.Location = new Point(25, 35);
			this.grpMain.Name = "grpMain";
			this.grpMain.Size = new Size(250, 129);
			this.grpMain.TabIndex = 6;
			this.grpMain.TabStop = false;
			this.grpMain.Text = "Up";
			this.cmbMainCh.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbMainCh.FormattingEnabled = true;
			this.cmbMainCh.Location = new Point(98, 71);
			this.cmbMainCh.Name = "cmbMainCh";
			this.cmbMainCh.Size = new Size(120, 24);
			this.cmbMainCh.TabIndex = 3;
			this.lblMainCh.Location = new Point(19, 71);
			this.lblMainCh.Name = "lblMainCh";
			this.lblMainCh.Size = new Size(70, 24);
			this.lblMainCh.TabIndex = 2;
			this.lblMainCh.Text = "Channel";
			this.lblMainCh.TextAlign = ContentAlignment.MiddleRight;
			this.cmbMainZone.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbMainZone.FormattingEnabled = true;
			this.cmbMainZone.Location = new Point(98, 37);
			this.cmbMainZone.Name = "cmbMainZone";
			this.cmbMainZone.Size = new Size(120, 24);
			this.cmbMainZone.TabIndex = 1;
			this.cmbMainZone.SelectedIndexChanged += this.cmbMainZone_SelectedIndexChanged;
			this.cmbMainZone.DropDown += this.cmbMainZone_DropDown;
			this.lblCurZone.Location = new Point(19, 37);
			this.lblCurZone.Name = "lblCurZone";
			this.lblCurZone.Size = new Size(70, 24);
			this.lblCurZone.TabIndex = 0;
			this.lblCurZone.Text = "Zone";
			this.lblCurZone.TextAlign = ContentAlignment.MiddleRight;
			base.AutoScaleDimensions = new SizeF(7f, 16f);
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(600, 197);
			base.Controls.Add(this.pnlZoneBasic);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.Name = "ZoneBasicForm";
			this.Text = "Zone";
			base.Load += this.ZoneBasicForm_Load;
			base.FormClosing += this.ZoneBasicForm_FormClosing;
			this.pnlZoneBasic.ResumeLayout(false);
			this.grpSub.ResumeLayout(false);
			this.grpMain.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
