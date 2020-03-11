using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class SignalingBasicForm : DockContent, IDisp
	{
		[Serializable]
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class SignalingBasic : IVerify<SignalingBasic>
		{
			private byte rmDuration;

			private byte txSyncWakeTot;

			private byte selCallHang;

			private byte autoResetTimer;

			private byte flag1;

			private byte flag2;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			private byte[] reserve;

			public decimal RmDuration
			{
				get
				{
					if (this.rmDuration >= 1 && this.rmDuration <= 12)
					{
						return this.rmDuration * 10;
					}
					return 10m;
				}
				set
				{
					byte b = Convert.ToByte(value / 10m);
					if (b >= 1 && b <= 12)
					{
						this.rmDuration = b;
					}
					else
					{
						this.rmDuration = 1;
					}
				}
			}

			public decimal TxSyncWakeTot
			{
				get
				{
					if (this.txSyncWakeTot >= 5 && this.txSyncWakeTot <= 15)
					{
						return this.txSyncWakeTot * 25;
					}
					return 250m;
				}
				set
				{
					byte b = Convert.ToByte(value / 25m);
					if (b >= 5 && b <= 15)
					{
						this.txSyncWakeTot = b;
					}
					else
					{
						this.txSyncWakeTot = 10;
					}
				}
			}

			public int SelCallHang
			{
				get
				{
					if (this.selCallHang >= 0 && this.selCallHang <= 14)
					{
						return this.selCallHang * 500;
					}
					return 4000;
				}
				set
				{
					value /= 500;
					if (value >= 0 && value <= 14)
					{
						this.selCallHang = Convert.ToByte(value);
					}
					else
					{
						this.selCallHang = 8;
					}
				}
			}

			public byte AutoResetTimer
			{
				get
				{
					if (this.autoResetTimer >= 1 && this.autoResetTimer <= 255)
					{
						return this.autoResetTimer;
					}
					return 10;
				}
				set
				{
					if (value >= 1 && value <= 255)
					{
						this.autoResetTimer = value;
					}
					else
					{
						this.autoResetTimer = 10;
					}
				}
			}

			public bool RadioDisable
			{
				get
				{
					return Convert.ToBoolean(this.flag1 & 0x80);
				}
				set
				{
					if (value)
					{
						this.flag1 |= 128;
					}
					else
					{
						this.flag1 &= 127;
					}
				}
			}

			public bool RemoteMonitor
			{
				get
				{
					return Convert.ToBoolean(this.flag1 & 0x40);
				}
				set
				{
					if (value)
					{
						this.flag1 |= 64;
					}
					else
					{
						this.flag1 &= 191;
					}
				}
			}

			public bool EmgRm
			{
				get
				{
					return Convert.ToBoolean(this.flag1 & 0x20);
				}
				set
				{
					if (value)
					{
						this.flag1 |= 32;
					}
					else
					{
						this.flag1 &= 223;
					}
				}
			}

			public decimal TxWakeMsgLimit
			{
				get
				{
					byte b = Convert.ToByte((this.flag1 & 0x1C) >> 2);
					if (b >= 0 && b <= 4)
					{
						return b;
					}
					return 2m;
				}
				set
				{
					if (!(value >= 0m) || !(value <= 4m))
					{
						value = 2m;
					}
					value = ((int)value << 2 & 0x1C);
					this.flag1 &= 227;
					this.flag1 |= (byte)value;
				}
			}

			public bool CallAlert
			{
				get
				{
					return Convert.ToBoolean(this.flag2 & 0x80);
				}
				set
				{
					if (value)
					{
						this.flag2 |= 128;
					}
					else
					{
						this.flag2 &= 127;
					}
				}
			}

			public bool SelCallCode
			{
				get
				{
					return Convert.ToBoolean(this.flag2 & 0x40);
				}
				set
				{
					if (value)
					{
						this.flag2 |= 64;
					}
					else
					{
						this.flag2 &= 191;
					}
				}
			}

			public int SelCallToneId
			{
				get
				{
					return (this.flag2 & 0x20) >> 5;
				}
				set
				{
					this.flag2 &= 223;
					this.flag2 |= Convert.ToByte(value << 5 & 0x20);
				}
			}

			public void Verify(SignalingBasic def)
			{
				Settings.smethod_11(ref this.rmDuration, (byte)1, (byte)12, def.rmDuration);
				Settings.smethod_11(ref this.txSyncWakeTot, (byte)5, (byte)15, def.txSyncWakeTot);
				Settings.smethod_11(ref this.selCallHang, (byte)0, (byte)14, def.selCallHang);
				Settings.smethod_11(ref this.autoResetTimer, (byte)1, (byte)255, def.autoResetTimer);
				byte b = Convert.ToByte((this.flag1 & 0x1C) >> 2);
				if (b >= 0 && b <= 4)
				{
					return;
				}
				this.flag1 &= 227;
				this.flag1 |= (byte)(def.flag1 & 0x1C);
			}

			public SignalingBasic()
			{
				
				//base._002Ector();
			}
		}

		private const byte MIN_RM_DURATION = 1;

		private const byte MAX_RM_DURATION = 12;

		private const byte SCL_RM_DURATION = 10;

		private const byte INC_RM_DURATION = 1;

		private const byte LEN_RM_DURATION = 3;

		private const string SZ_RM_DURATION = "0123456789";

		private const byte MIN_TX_SYNC_WAKE_TOT = 5;

		private const byte MAX_TX_SYNC_WAKE_TOT = 15;

		private const byte INC_TX_SYNC_WAKE_TOT = 1;

		private const byte SCL_TX_SYNC_WAKE_TOT = 25;

		private const byte LEN_TX_SYNC_WAKE_TOT = 3;

		private const string SZ_TX_SYNC_WAKE_TOT = "0123456789";

		private const byte MIN_SEL_CALL_HANG = 0;

		private const byte MAX_SEL_CALL_HANG = 14;

		private const ushort SCL_SEL_CALL_HANG = 500;

		private const byte INC_SEL_CALL_HANG = 1;

		private const byte LEN_SEL_CALL_HANG = 4;

		private const byte MIN_AUTO_RESET_TIMER = 1;

		private const byte MAX_AUTO_RESET_TIMER = 255;

		private const byte INC_AUTO_RESET_TIMER = 1;

		private const byte SCL_AUTO_RESET_TIMER = 1;

		private const byte LEN_AUTO_RESET_TIMER = 3;

		private const byte MIN_TX_WAKE_MSG_LIMIT = 0;

		private const byte MAX_TX_WAKE_MSG_LIMIT = 4;

		private const byte INC_TX_WAKE_MSG_LIMIT = 1;

		private const byte SCL_TX_WAKE_MSG_LIMIT = 1;

		private const byte LEN_TX_WAKE_MSG_LIMIT = 1;

		public static SignalingBasic DefaultSignalingBasic;

		public static SignalingBasic data;

		//private IContainer components;

		private CheckBox chkCallAlert;

		private CheckBox chkSelCall;

		private ComboBox cmbSelCallToneId;

		private Label lblSelCallToneId;

		private Label lblSelCallHang;

		private Label lblAutoResetTimer;

		private CheckBox chkRadioDisable;

		private CheckBox chkRemoteMonitor;

		private CheckBox chkEmgRM;

		private Label lblRMDuration;

		private Label lblTxSyncWakeTot;

		private Label lblTxWakeMsgLimit;

		private CustomNumericUpDown nudSelCallHang;

		private CustomNumericUpDown nudAutoResetTimer;

		private CustomNumericUpDown nudRMDuration;

		private CustomNumericUpDown nudTxSyncWakeTot;

		private CustomNumericUpDown nudTxWakeMsgLimit;

		private CustomPanel pnlSignalingBasic;

		public TreeNode Node
		{
			get;
			set;
		}

		public void SaveData()
		{
			try
			{
				SignalingBasicForm.data.RmDuration = this.nudRMDuration.Value;
				SignalingBasicForm.data.TxSyncWakeTot = this.nudTxSyncWakeTot.Value;
				SignalingBasicForm.data.RadioDisable = this.chkRadioDisable.Checked;
				SignalingBasicForm.data.RemoteMonitor = this.chkRemoteMonitor.Checked;
				SignalingBasicForm.data.EmgRm = this.chkEmgRM.Checked;
				SignalingBasicForm.data.TxWakeMsgLimit = this.nudTxWakeMsgLimit.Value;
				SignalingBasicForm.data.CallAlert = this.chkCallAlert.Checked;
				SignalingBasicForm.data.SelCallCode = this.chkSelCall.Checked;
				SignalingBasicForm.data.SelCallToneId = this.cmbSelCallToneId.SelectedIndex;
				SignalingBasicForm.data.SelCallHang = (int)this.nudSelCallHang.Value;
				SignalingBasicForm.data.AutoResetTimer = (byte)this.nudAutoResetTimer.Value;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void DispData()
		{
			try
			{
				this.method_0();
				this.chkRadioDisable.Checked = SignalingBasicForm.data.RadioDisable;
				this.chkRemoteMonitor.Checked = SignalingBasicForm.data.RemoteMonitor;
				this.chkEmgRM.Checked = SignalingBasicForm.data.EmgRm;
				this.nudRMDuration.Value = SignalingBasicForm.data.RmDuration;
				this.nudTxWakeMsgLimit.Value = SignalingBasicForm.data.TxWakeMsgLimit;
				this.nudTxSyncWakeTot.Value = SignalingBasicForm.data.TxSyncWakeTot;
				this.chkCallAlert.Checked = SignalingBasicForm.data.CallAlert;
				this.chkSelCall.Checked = SignalingBasicForm.data.SelCallCode;
				this.cmbSelCallToneId.SelectedIndex = SignalingBasicForm.data.SelCallToneId;
				this.nudSelCallHang.Value = SignalingBasicForm.data.SelCallHang;
				this.nudAutoResetTimer.Value = SignalingBasicForm.data.AutoResetTimer;
				this.RefreshByUserMode();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void RefreshByUserMode()
		{
			bool flag = Settings.getUserExpertSettings() == Settings.UserMode.Expert;
			this.chkRadioDisable.Enabled &= flag;
			this.chkRemoteMonitor.Enabled &= flag;
			this.chkEmgRM.Enabled &= flag;
			this.lblTxWakeMsgLimit.Enabled &= flag;
			this.nudTxWakeMsgLimit.Enabled &= flag;
			this.lblRMDuration.Enabled &= flag;
			this.nudRMDuration.Enabled &= flag;
			this.lblTxSyncWakeTot.Enabled &= flag;
			this.nudTxSyncWakeTot.Enabled &= flag;
		}

		public void RefreshName()
		{
		}

		public SignalingBasicForm()
		{
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void method_0()
		{
			this.nudRMDuration.Minimum = 10m;
			this.nudRMDuration.Maximum = 120m;
			this.nudRMDuration.Increment = 10m;
			this.nudRMDuration.method_0(3);
			this.nudTxSyncWakeTot.Minimum = 125m;
			this.nudTxSyncWakeTot.Maximum = 375m;
			this.nudTxSyncWakeTot.Increment = 25m;
			this.nudTxSyncWakeTot.method_0(3);
			Settings.smethod_36(this.nudTxWakeMsgLimit, new Class13(0, 4, 1, 1m, 1));
			Settings.smethod_36(this.nudSelCallHang, new Class13(0, 14, 1, 500m, 4));
			Settings.smethod_36(this.nudAutoResetTimer, new Class13(1, 255, 1, 1m, 3));
		}

		private void SignalingBasicForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);
			this.DispData();
		}

		private void SignalingBasicForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
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
			this.pnlSignalingBasic = new CustomPanel();
			this.nudTxSyncWakeTot = new CustomNumericUpDown();
			this.chkCallAlert = new System.Windows.Forms.CheckBox();
			this.nudAutoResetTimer = new CustomNumericUpDown();
			this.chkRadioDisable = new System.Windows.Forms.CheckBox();
			this.nudTxWakeMsgLimit = new CustomNumericUpDown();
			this.chkRemoteMonitor = new System.Windows.Forms.CheckBox();
			this.nudRMDuration = new CustomNumericUpDown();
			this.chkEmgRM = new System.Windows.Forms.CheckBox();
			this.lblTxSyncWakeTot = new System.Windows.Forms.Label();
			this.chkSelCall = new System.Windows.Forms.CheckBox();
			this.lblTxWakeMsgLimit = new System.Windows.Forms.Label();
			this.cmbSelCallToneId = new System.Windows.Forms.ComboBox();
			this.nudSelCallHang = new CustomNumericUpDown();
			this.lblSelCallToneId = new System.Windows.Forms.Label();
			this.lblRMDuration = new System.Windows.Forms.Label();
			this.lblSelCallHang = new System.Windows.Forms.Label();
			this.lblAutoResetTimer = new System.Windows.Forms.Label();
			this.pnlSignalingBasic.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudTxSyncWakeTot)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAutoResetTimer)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTxWakeMsgLimit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRMDuration)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSelCallHang)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlSignalingBasic
			// 
			this.pnlSignalingBasic.AutoScroll = true;
			this.pnlSignalingBasic.AutoSize = true;
			this.pnlSignalingBasic.Controls.Add(this.nudTxSyncWakeTot);
			this.pnlSignalingBasic.Controls.Add(this.chkCallAlert);
			this.pnlSignalingBasic.Controls.Add(this.nudAutoResetTimer);
			this.pnlSignalingBasic.Controls.Add(this.chkRadioDisable);
			this.pnlSignalingBasic.Controls.Add(this.nudTxWakeMsgLimit);
			this.pnlSignalingBasic.Controls.Add(this.chkRemoteMonitor);
			this.pnlSignalingBasic.Controls.Add(this.nudRMDuration);
			this.pnlSignalingBasic.Controls.Add(this.chkEmgRM);
			this.pnlSignalingBasic.Controls.Add(this.lblTxSyncWakeTot);
			this.pnlSignalingBasic.Controls.Add(this.chkSelCall);
			this.pnlSignalingBasic.Controls.Add(this.lblTxWakeMsgLimit);
			this.pnlSignalingBasic.Controls.Add(this.cmbSelCallToneId);
			this.pnlSignalingBasic.Controls.Add(this.nudSelCallHang);
			this.pnlSignalingBasic.Controls.Add(this.lblSelCallToneId);
			this.pnlSignalingBasic.Controls.Add(this.lblRMDuration);
			this.pnlSignalingBasic.Controls.Add(this.lblSelCallHang);
			this.pnlSignalingBasic.Controls.Add(this.lblAutoResetTimer);
			this.pnlSignalingBasic.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSignalingBasic.Location = new System.Drawing.Point(0, 0);
			this.pnlSignalingBasic.Name = "pnlSignalingBasic";
			this.pnlSignalingBasic.Size = new System.Drawing.Size(381, 224);
			this.pnlSignalingBasic.TabIndex = 0;
			// 
			// nudTxSyncWakeTot
			// 
			this.nudTxSyncWakeTot.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudTxSyncWakeTot.Location = new System.Drawing.Point(245, 158);
			this.nudTxSyncWakeTot.Maximum = new decimal(new int[] {
            375,
            0,
            0,
            0});
			this.nudTxSyncWakeTot.Minimum = new decimal(new int[] {
            125,
            0,
            0,
            0});
			this.nudTxSyncWakeTot.Name = "nudTxSyncWakeTot";
			this.nudTxSyncWakeTot.Size = new System.Drawing.Size(120, 23);
			this.nudTxSyncWakeTot.TabIndex = 8;
			this.nudTxSyncWakeTot.Value = new decimal(new int[] {
            125,
            0,
            0,
            0});
			// 
			// chkCallAlert
			// 
			this.chkCallAlert.AutoSize = true;
			this.chkCallAlert.Location = new System.Drawing.Point(206, 188);
			this.chkCallAlert.Name = "chkCallAlert";
			this.chkCallAlert.Size = new System.Drawing.Size(135, 20);
			this.chkCallAlert.TabIndex = 9;
			this.chkCallAlert.Text = "Call Alert Encode";
			this.chkCallAlert.UseVisualStyleBackColor = true;
			this.chkCallAlert.Visible = false;
			// 
			// nudAutoResetTimer
			// 
			this.nudAutoResetTimer.Location = new System.Drawing.Point(214, 192);
			this.nudAutoResetTimer.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nudAutoResetTimer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudAutoResetTimer.Name = "nudAutoResetTimer";
			this.nudAutoResetTimer.Size = new System.Drawing.Size(120, 23);
			this.nudAutoResetTimer.TabIndex = 16;
			this.nudAutoResetTimer.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudAutoResetTimer.Visible = false;
			// 
			// chkRadioDisable
			// 
			this.chkRadioDisable.AutoSize = true;
			this.chkRadioDisable.Location = new System.Drawing.Point(52, 12);
			this.chkRadioDisable.Name = "chkRadioDisable";
			this.chkRadioDisable.Size = new System.Drawing.Size(168, 20);
			this.chkRadioDisable.TabIndex = 0;
			this.chkRadioDisable.Text = "Radio Disable Decode";
			this.chkRadioDisable.UseVisualStyleBackColor = true;
			// 
			// nudTxWakeMsgLimit
			// 
			this.nudTxWakeMsgLimit.Location = new System.Drawing.Point(245, 98);
			this.nudTxWakeMsgLimit.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.nudTxWakeMsgLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudTxWakeMsgLimit.Name = "nudTxWakeMsgLimit";
			this.nudTxWakeMsgLimit.Size = new System.Drawing.Size(120, 23);
			this.nudTxWakeMsgLimit.TabIndex = 4;
			this.nudTxWakeMsgLimit.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
			// 
			// chkRemoteMonitor
			// 
			this.chkRemoteMonitor.AutoSize = true;
			this.chkRemoteMonitor.Location = new System.Drawing.Point(52, 36);
			this.chkRemoteMonitor.Name = "chkRemoteMonitor";
			this.chkRemoteMonitor.Size = new System.Drawing.Size(180, 20);
			this.chkRemoteMonitor.TabIndex = 1;
			this.chkRemoteMonitor.Text = "Remote Monitor Decode";
			this.chkRemoteMonitor.UseVisualStyleBackColor = true;
			// 
			// nudRMDuration
			// 
			this.nudRMDuration.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudRMDuration.Location = new System.Drawing.Point(245, 128);
			this.nudRMDuration.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
			this.nudRMDuration.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudRMDuration.Name = "nudRMDuration";
			this.nudRMDuration.Size = new System.Drawing.Size(120, 23);
			this.nudRMDuration.TabIndex = 6;
			this.nudRMDuration.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// chkEmgRM
			// 
			this.chkEmgRM.AutoSize = true;
			this.chkEmgRM.Location = new System.Drawing.Point(52, 62);
			this.chkEmgRM.Name = "chkEmgRM";
			this.chkEmgRM.Size = new System.Drawing.Size(255, 20);
			this.chkEmgRM.TabIndex = 2;
			this.chkEmgRM.Text = "Emergency Romote Monitor Decode";
			this.chkEmgRM.UseVisualStyleBackColor = true;
			// 
			// lblTxSyncWakeTot
			// 
			this.lblTxSyncWakeTot.Location = new System.Drawing.Point(49, 158);
			this.lblTxSyncWakeTot.Name = "lblTxSyncWakeTot";
			this.lblTxSyncWakeTot.Size = new System.Drawing.Size(185, 24);
			this.lblTxSyncWakeTot.TabIndex = 7;
			this.lblTxSyncWakeTot.Text = "Tx Sync Wakeup TOT [ms]";
			this.lblTxSyncWakeTot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chkSelCall
			// 
			this.chkSelCall.AutoSize = true;
			this.chkSelCall.Location = new System.Drawing.Point(208, 188);
			this.chkSelCall.Name = "chkSelCall";
			this.chkSelCall.Size = new System.Drawing.Size(131, 20);
			this.chkSelCall.TabIndex = 10;
			this.chkSelCall.Text = "Self Call Encode";
			this.chkSelCall.UseVisualStyleBackColor = true;
			this.chkSelCall.Visible = false;
			// 
			// lblTxWakeMsgLimit
			// 
			this.lblTxWakeMsgLimit.Location = new System.Drawing.Point(49, 98);
			this.lblTxWakeMsgLimit.Name = "lblTxWakeMsgLimit";
			this.lblTxWakeMsgLimit.Size = new System.Drawing.Size(185, 24);
			this.lblTxWakeMsgLimit.TabIndex = 3;
			this.lblTxWakeMsgLimit.Text = "Tx Wakeup Message Limit";
			this.lblTxWakeMsgLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cmbSelCallToneId
			// 
			this.cmbSelCallToneId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSelCallToneId.FormattingEnabled = true;
			this.cmbSelCallToneId.Items.AddRange(new object[] {
            "前置",
            "始终"});
			this.cmbSelCallToneId.Location = new System.Drawing.Point(209, 188);
			this.cmbSelCallToneId.Name = "cmbSelCallToneId";
			this.cmbSelCallToneId.Size = new System.Drawing.Size(121, 24);
			this.cmbSelCallToneId.TabIndex = 12;
			this.cmbSelCallToneId.Visible = false;
			// 
			// nudSelCallHang
			// 
			this.nudSelCallHang.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.nudSelCallHang.Location = new System.Drawing.Point(219, 190);
			this.nudSelCallHang.Maximum = new decimal(new int[] {
            7000,
            0,
            0,
            0});
			this.nudSelCallHang.Name = "nudSelCallHang";
			this.nudSelCallHang.Size = new System.Drawing.Size(120, 23);
			this.nudSelCallHang.TabIndex = 14;
			this.nudSelCallHang.Visible = false;
			// 
			// lblSelCallToneId
			// 
			this.lblSelCallToneId.Location = new System.Drawing.Point(13, 188);
			this.lblSelCallToneId.Name = "lblSelCallToneId";
			this.lblSelCallToneId.Size = new System.Drawing.Size(185, 24);
			this.lblSelCallToneId.TabIndex = 11;
			this.lblSelCallToneId.Text = "Self Call Tone/ID";
			this.lblSelCallToneId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblSelCallToneId.Visible = false;
			// 
			// lblRMDuration
			// 
			this.lblRMDuration.Location = new System.Drawing.Point(49, 128);
			this.lblRMDuration.Name = "lblRMDuration";
			this.lblRMDuration.Size = new System.Drawing.Size(185, 24);
			this.lblRMDuration.TabIndex = 5;
			this.lblRMDuration.Text = "Remote Monitor Duration [s]";
			this.lblRMDuration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblSelCallHang
			// 
			this.lblSelCallHang.Location = new System.Drawing.Point(23, 190);
			this.lblSelCallHang.Name = "lblSelCallHang";
			this.lblSelCallHang.Size = new System.Drawing.Size(185, 24);
			this.lblSelCallHang.TabIndex = 13;
			this.lblSelCallHang.Text = "Self Call Hang Time [ms]";
			this.lblSelCallHang.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblSelCallHang.Visible = false;
			// 
			// lblAutoResetTimer
			// 
			this.lblAutoResetTimer.Location = new System.Drawing.Point(18, 192);
			this.lblAutoResetTimer.Name = "lblAutoResetTimer";
			this.lblAutoResetTimer.Size = new System.Drawing.Size(185, 24);
			this.lblAutoResetTimer.TabIndex = 15;
			this.lblAutoResetTimer.Text = "Auto Reset Timer [s]";
			this.lblAutoResetTimer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblAutoResetTimer.Visible = false;
			// 
			// SignalingBasicForm
			// 
			this.ClientSize = new System.Drawing.Size(381, 224);
			this.Controls.Add(this.pnlSignalingBasic);
			this.Font = new System.Drawing.Font("Arial", 10F);
			this.Name = "SignalingBasicForm";
			this.Text = "Signaling System";
			this.pnlSignalingBasic.ResumeLayout(false);
			this.pnlSignalingBasic.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudTxSyncWakeTot)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAutoResetTimer)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTxWakeMsgLimit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRMDuration)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSelCallHang)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		static SignalingBasicForm()
		{
			
			SignalingBasicForm.DefaultSignalingBasic = new SignalingBasic();
			SignalingBasicForm.data = new SignalingBasic();
		}
	}
}
