using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class ScanBasicForm : DockContent, IDisp
	{

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class ScanBasic
		{
			private byte digitHang;

			private byte analogHang;

			private byte voteHang;

			private byte fastVoteRssi;

			private byte startVoteRssi;

			private byte flag1;

			private byte scanTime;						//added for 3.1.1 

			private byte reserve;


			public decimal DigitHang
			{
				get
				{
					if (this.digitHang >= 1 && this.digitHang <= 20)
					{
						return this.digitHang * 500;
					}
					return 1000m;
				}
				set
				{
					this.digitHang = Convert.ToByte(value / 500m);
				}
			}

			public decimal AnalogHang
			{
				get
				{
					if (this.analogHang >= 0 && this.analogHang <= 20)
					{
						return this.analogHang * 500;
					}
					return 1000m;
				}
				set
				{
					this.analogHang = Convert.ToByte(value / 500m);
				}
			}

			public decimal VoteHang
			{
				get
				{
					if (this.voteHang >= 0 && this.voteHang <= 255)
					{
						return (decimal)this.voteHang * 0.25m;
					}
					return 3.00m;
				}
				set
				{
					this.voteHang = Convert.ToByte(value / 0.25m);
				}
			}

			public decimal FastVoteRssi
			{
				get
				{
					if (this.fastVoteRssi >= 70 && this.fastVoteRssi <= 120)
					{
						return this.fastVoteRssi * -1;
					}
					return -70m;
				}
				set
				{
					this.fastVoteRssi = Convert.ToByte(value * -1m);
				}
			}

			public decimal StartVoteRssi
			{
				get
				{
					if (this.startVoteRssi >= 70 && this.startVoteRssi <= 120)
					{
						return this.startVoteRssi * -1;
					}
					return -100m;
				}
				set
				{
					this.startVoteRssi = Convert.ToByte(value * -1m);
				}
			}

			public bool PriorityAlert
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

			public decimal ScanTime
			{
				get
				{
					if (this.scanTime >= 1 && this.scanTime <= 13)
					{
						return this.scanTime * 5;
					}
					return 5m;
				}
				set
				{
					this.scanTime = Convert.ToByte(value / 5m);
				}
			}

			public ScanBasic()
			{

				//base._002Ector();
#if CP_VER_3_0_6
				this.scanTime=255;					//not used in 3.0.6 so set it to FF
#endif
				this.reserve = 255;
			}

			public void Verify(ScanBasic def)
			{
				Settings.smethod_11(ref this.digitHang, (byte)1, (byte)20, def.digitHang);
				Settings.smethod_11(ref this.analogHang, (byte)0, (byte)20, def.analogHang);
			}
		}

		private const byte INC_DIGIT_HANG = 1;

		private const byte MIN_DIGIT_HANG = 1;

		private const byte MAX_DIGIT_HANG = 20;

		private const ushort SCL_DIGIT_HANG = 500;

		private const byte LEN_DIGIT_HANG = 4;

		private const string SZ_DIGIT_HANG = "0123456789\b";

		private const byte INC_ANALOG_HANG = 1;

		private const byte MIN_ANALOG_HANG = 0;

		private const byte MAX_ANALOG_HANG = 20;

		private const ushort SCL_ANALOG_HANG = 500;

		private const byte LEN_ANALOG_HANG = 4;

		private const string SZ_ANALOG_HANG = "0123456789\b";

		private const byte INC_VOTE_HANG = 1;

		private const byte MIN_VOTE_HANG = 0;

		private const byte MAX_VOTE_HANG = 255;

		public static  decimal SCL_VOTE_HANG = 0.25m;

		private const byte LEN_VOTE_HANG = 5;

		private const byte INC_FAST_VOTE_RSSI = 1;

		private const byte MIN_FAST_VOTE_RSSI = 70;

		private const byte MAX_FAST_VOTE_RSSI = 120;

		private const sbyte SCL_FAST_VOTE_RSSI = -1;

		private const byte LEN_FAST_VOTE_RSSI = 4;

		private const byte INC_START_VOTE_RSSI = 1;

		private const byte MIN_START_VOTE_RSSI = 70;

		private const byte MAX_START_VOTE_RSSI = 120;

		private const sbyte SCL_START_VOTE_RSSI = -1;

		private const byte LEN_START_VOTE_RSSI = 4;

		private const byte INC_SCAN_TIME = 1;

		private const byte MIN_SCAN_TIME = 1;

		private const byte MAX_SCAN_TIME = 12;

		private const ushort SCL_SCAN_TIME = 5;

		private const byte LEN_SCAN_TIME = 2;

		private const string SZ_SCAN_TIME = "0123456789\b";

		public static ScanBasic DefaultScanBasic;

		public static ScanBasic data;

		//private IContainer components;

		private Label lblDigitHang;

		private Label lblAnalogHang;

		private Label lblVoteHang;

		private Label lblFastVoteRssi;

		private Label lblStartVoteRssi;

		private Label lblScanTime;

		private CheckBox chkPriorityAlert;

		private CustomNumericUpDown nudDigitHang;

		private CustomNumericUpDown nudAnalogHang;

		private CustomNumericUpDown nudScanTime;

		private CustomNumericUpDown nudVoteHang;

		private CustomNumericUpDown nudFastVoteRssi;

		private CustomNumericUpDown nudStartVoteRssi;

		private CustomPanel pnlScanBasic;

		public TreeNode Node
		{
			get;
			set;
		}

		public ScanBasicForm()
		{
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		public void SaveData()
		{
			try
			{
				ScanBasicForm.data.DigitHang = this.nudDigitHang.Value;
				ScanBasicForm.data.AnalogHang = this.nudAnalogHang.Value;
				ScanBasicForm.data.ScanTime = this.nudScanTime.Value;
				ScanBasicForm.data.VoteHang = this.nudVoteHang.Value;
				ScanBasicForm.data.FastVoteRssi = this.nudFastVoteRssi.Value;
				ScanBasicForm.data.StartVoteRssi = this.nudStartVoteRssi.Value;
				ScanBasicForm.data.PriorityAlert = this.chkPriorityAlert.Checked;
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
				this.nudDigitHang.Value = ScanBasicForm.data.DigitHang;
				this.nudAnalogHang.Value = ScanBasicForm.data.AnalogHang;
				this.nudScanTime.Value = ScanBasicForm.data.ScanTime;
				this.nudVoteHang.Value = ScanBasicForm.data.VoteHang;
				this.nudFastVoteRssi.Value = ScanBasicForm.data.FastVoteRssi;
				this.nudStartVoteRssi.Value = ScanBasicForm.data.StartVoteRssi;
				this.chkPriorityAlert.Checked = ScanBasicForm.data.PriorityAlert;
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
			this.lblDigitHang.Enabled &= flag;
			this.nudDigitHang.Enabled &= flag;
			this.lblAnalogHang.Enabled &= flag;
			this.nudAnalogHang.Enabled &= flag;
			this.nudScanTime.Enabled &= flag;
			this.chkPriorityAlert.Enabled &= flag;
		}

		public void RefreshName()
		{
		}

		private void method_0()
		{
			this.nudDigitHang.Increment = 500m;
			this.nudDigitHang.Minimum = 500m;
			this.nudDigitHang.Maximum = 10000m;
			this.nudDigitHang.method_0(4);
			this.nudDigitHang.method_2("0123456789\b");
			this.nudAnalogHang.Increment = 500m;
			this.nudAnalogHang.Minimum = 0m;
			this.nudAnalogHang.Maximum = 10000m;
			this.nudAnalogHang.method_0(4);
			this.nudAnalogHang.method_2("0123456789\b");
			this.nudScanTime.Increment = 5m;
			this.nudScanTime.Minimum = 5m;
			this.nudScanTime.Maximum = 60m;
			this.nudScanTime.method_0(2);
			this.nudScanTime.method_2("0123456789\b");
			this.nudVoteHang.Increment = 0.25m;
			this.nudVoteHang.Minimum = 0.00m;
			this.nudVoteHang.Maximum = 63.75m;
			this.nudVoteHang.method_0(5);
			this.nudVoteHang.method_2("0123456789.\b");
			this.nudFastVoteRssi.Increment = 1m;
			this.nudFastVoteRssi.Minimum = -120m;
			this.nudFastVoteRssi.Maximum = -70m;
			this.nudFastVoteRssi.method_0(4);
			this.nudFastVoteRssi.method_2("-0123456789\b");
			this.nudStartVoteRssi.Increment = 1m;
			this.nudStartVoteRssi.Minimum = -120m;
			this.nudStartVoteRssi.Maximum = -70m;
			this.nudStartVoteRssi.method_0(4);
			this.nudStartVoteRssi.method_2("-0123456789\b");
		}

		private void ScanBasicForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);
			this.method_0();
			this.DispData();
		}

		private void ScanBasicForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
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
			this.pnlScanBasic = new CustomPanel();
			this.nudAnalogHang = new CustomNumericUpDown();
			this.nudScanTime = new CustomNumericUpDown();
			this.lblScanTime = new Label();
			this.nudStartVoteRssi = new CustomNumericUpDown();
			this.lblDigitHang = new Label();
			this.nudFastVoteRssi = new CustomNumericUpDown();
			this.lblAnalogHang = new Label();
			this.nudVoteHang = new CustomNumericUpDown();
			this.lblVoteHang = new Label();
			this.lblFastVoteRssi = new Label();
			this.nudDigitHang = new CustomNumericUpDown();
			this.lblStartVoteRssi = new Label();
			this.chkPriorityAlert = new CheckBox();
			this.pnlScanBasic.SuspendLayout();
			((ISupportInitialize)this.nudAnalogHang).BeginInit();
			((ISupportInitialize)this.nudStartVoteRssi).BeginInit();
			((ISupportInitialize)this.nudFastVoteRssi).BeginInit();
			((ISupportInitialize)this.nudVoteHang).BeginInit();
			((ISupportInitialize)this.nudDigitHang).BeginInit();
			((ISupportInitialize)this.nudScanTime).BeginInit();
			base.SuspendLayout();
			this.pnlScanBasic.AutoScroll = true;
			this.pnlScanBasic.AutoSize = true;
			this.pnlScanBasic.Controls.Add(this.nudAnalogHang);
			this.pnlScanBasic.Controls.Add(this.nudScanTime);
			this.pnlScanBasic.Controls.Add(this.nudStartVoteRssi);
			this.pnlScanBasic.Controls.Add(this.lblDigitHang);
			this.pnlScanBasic.Controls.Add(this.nudFastVoteRssi);
			this.pnlScanBasic.Controls.Add(this.lblAnalogHang);
			this.pnlScanBasic.Controls.Add(this.lblScanTime);
			this.pnlScanBasic.Controls.Add(this.nudVoteHang);
			this.pnlScanBasic.Controls.Add(this.lblVoteHang);
			this.pnlScanBasic.Controls.Add(this.lblFastVoteRssi);
			this.pnlScanBasic.Controls.Add(this.nudDigitHang);
			this.pnlScanBasic.Controls.Add(this.lblStartVoteRssi);
			this.pnlScanBasic.Controls.Add(this.chkPriorityAlert);
			this.pnlScanBasic.Dock = DockStyle.Fill;
			this.pnlScanBasic.Location = new Point(0, 0);
			this.pnlScanBasic.Name = "pnlScanBasic";
			this.pnlScanBasic.Size = new Size(465, 328);
			this.pnlScanBasic.TabIndex = 0;
			this.nudAnalogHang.Increment = new decimal(new int[4]
			{
				500,
				0,
				0,
				0
			});
			this.nudAnalogHang.method_2(null);
			this.nudAnalogHang.Location = new Point(261, 104);
			this.nudAnalogHang.Maximum = new decimal(new int[4]
			{
				10000,
				0,
				0,
				0
			});
			this.nudAnalogHang.Minimum = new decimal(new int[4]
			{
				500,
				0,
				0,
				0
			});
			this.nudAnalogHang.Name = "nudAnalogHang";
			this.nudAnalogHang.method_6(null);
			CustomNumericUpDown @class = this.nudAnalogHang;
			int[] bits = new int[4];
			this.nudAnalogHang.method_4(new decimal(bits));
			this.nudAnalogHang.Size = new Size(140, 23);
			this.nudAnalogHang.TabIndex = 3;
			this.nudAnalogHang.Value = new decimal(new int[4]
			{
				500,
				0,
				0,
				0
			});

			this.nudScanTime.Increment = new decimal(new int[4]
			{
				5,
				0,
				0,
				0
			});
			this.nudScanTime.method_2(null);
			this.nudScanTime.Location = new Point(261, 40);
			this.nudScanTime.Maximum = new decimal(new int[4]
			{
				60,
				0,
				0,
				0
			});
			this.nudScanTime.Minimum = new decimal(new int[4]
			{
				5,
				0,
				0,
				0
			});
			this.nudScanTime.Name = "nudScanTime";
			this.nudScanTime.method_6(null);
			CustomNumericUpDown class9 = this.nudScanTime;
			int[] bits9 = new int[4];
			this.nudScanTime.method_4(new decimal(bits9));
			this.nudScanTime.Size = new Size(140, 23);
			this.nudScanTime.TabIndex = 6;
			this.nudScanTime.Value = new decimal(new int[4]
			{
				5,
				0,
				0,
				0
			});
#if OpenGD77
			this.nudScanTime.Visible = true;
			this.lblScanTime.Visible = true;
#elif CP_VER_3_1_X
			this.nudScanTime.Visible = true;
			this.lblScanTime.Visible = true;
#endif

			this.nudStartVoteRssi.method_2(null);
			this.nudStartVoteRssi.Location = new Point(261, 234);
			this.nudStartVoteRssi.Maximum = new decimal(new int[4]
			{
				70,
				0,
				0,
				-2147483648
			});
			this.nudStartVoteRssi.Minimum = new decimal(new int[4]
			{
				120,
				0,
				0,
				-2147483648
			});
			this.nudStartVoteRssi.Name = "nudStartVoteRssi";
			this.nudStartVoteRssi.method_6(null);
			CustomNumericUpDown class2 = this.nudStartVoteRssi;
			int[] bits2 = new int[4];
			this.nudStartVoteRssi.method_4(new decimal(bits2));
			this.nudStartVoteRssi.Size = new Size(140, 23);
			this.nudStartVoteRssi.TabIndex = 10;
			this.nudStartVoteRssi.Value = new decimal(new int[4]
			{
				70,
				0,
				0,
				-2147483648
			});
			this.nudStartVoteRssi.Visible = false;
			this.lblDigitHang.Location = new Point(50, 72);
			this.lblDigitHang.Name = "lblDigitHang";
			this.lblDigitHang.Size = new Size(198, 23);
			this.lblDigitHang.TabIndex = 0;
			this.lblDigitHang.Text = "Digital Hang Time [ms]";
			this.lblDigitHang.TextAlign = ContentAlignment.MiddleRight;
			this.nudFastVoteRssi.method_2(null);
			this.nudFastVoteRssi.Location = new Point(261, 202);
			this.nudFastVoteRssi.Maximum = new decimal(new int[4]
			{
				70,
				0,
				0,
				-2147483648
			});
			this.nudFastVoteRssi.Minimum = new decimal(new int[4]
			{
				120,
				0,
				0,
				-2147483648
			});
			this.nudFastVoteRssi.Name = "nudFastVoteRssi";
			this.nudFastVoteRssi.method_6(null);
			CustomNumericUpDown class3 = this.nudFastVoteRssi;
			int[] bits3 = new int[4];
			this.nudFastVoteRssi.method_4(new decimal(bits3));
			this.nudFastVoteRssi.Size = new Size(140, 23);
			this.nudFastVoteRssi.TabIndex = 8;
			this.nudFastVoteRssi.Value = new decimal(new int[4]
			{
				70,
				0,
				0,
				-2147483648
			});
			this.nudFastVoteRssi.Visible = false;
			this.lblAnalogHang.Location = new Point(50, 104);
			this.lblAnalogHang.Name = "lblAnalogHang";
			this.lblAnalogHang.Size = new Size(198, 23);
			this.lblAnalogHang.TabIndex = 2;
			this.lblAnalogHang.Text = "Analog Hang Time [ms]";
			this.lblAnalogHang.TextAlign = ContentAlignment.MiddleRight;

			this.lblScanTime.Location = new Point(50, 40);
			this.lblScanTime.Name = "lblScanTime";
			this.lblScanTime.Size = new Size(198, 23);
			this.lblScanTime.TabIndex = 10;
			this.lblScanTime.Text = "Scan Time [s]";
			this.lblScanTime.TextAlign = ContentAlignment.MiddleRight;

			this.nudVoteHang.DecimalPlaces = 2;
			this.nudVoteHang.Increment = new decimal(new int[4]
			{
				25,
				0,
				0,
				131072
			});
			this.nudVoteHang.method_2(null);
			this.nudVoteHang.Location = new Point(261, 169);
			this.nudVoteHang.Maximum = new decimal(new int[4]
			{
				6375,
				0,
				0,
				131072
			});
			this.nudVoteHang.Name = "nudVoteHang";
			this.nudVoteHang.method_6(null);
			CustomNumericUpDown class4 = this.nudVoteHang;
			int[] bits4 = new int[4];
			this.nudVoteHang.method_4(new decimal(bits4));
			this.nudVoteHang.Size = new Size(140, 23);
			this.nudVoteHang.TabIndex = 6;
			this.nudVoteHang.Visible = false;
			this.lblVoteHang.Location = new Point(50, 169);
			this.lblVoteHang.Name = "lblVoteHang";
			this.lblVoteHang.Size = new Size(198, 23);
			this.lblVoteHang.TabIndex = 5;
			this.lblVoteHang.Text = "Vote Scan Hang Time [s]";
			this.lblVoteHang.TextAlign = ContentAlignment.MiddleRight;
			this.lblVoteHang.Visible = false;
			this.lblFastVoteRssi.Location = new Point(50, 202);
			this.lblFastVoteRssi.Name = "lblFastVoteRssi";
			this.lblFastVoteRssi.Size = new Size(198, 23);
			this.lblFastVoteRssi.TabIndex = 7;
			this.lblFastVoteRssi.Text = "Fast Vote Rssi Threshold [dB]";
			this.lblFastVoteRssi.TextAlign = ContentAlignment.MiddleRight;
			this.lblFastVoteRssi.Visible = false;
			this.nudDigitHang.Increment = new decimal(new int[4]
			{
				500,
				0,
				0,
				0
			});
			this.nudDigitHang.method_2(null);
			this.nudDigitHang.Location = new Point(261, 72);
			this.nudDigitHang.Maximum = new decimal(new int[4]
			{
				10000,
				0,
				0,
				0
			});
			this.nudDigitHang.Minimum = new decimal(new int[4]
			{
				500,
				0,
				0,
				0
			});
			this.nudDigitHang.Name = "nudDigitHang";
			this.nudDigitHang.method_6(null);
			CustomNumericUpDown class5 = this.nudDigitHang;
			int[] bits5 = new int[4];
			this.nudDigitHang.method_4(new decimal(bits5));
			this.nudDigitHang.Size = new Size(140, 23);
			this.nudDigitHang.TabIndex = 1;
			this.nudDigitHang.Value = new decimal(new int[4]
			{
				500,
				0,
				0,
				0
			});
			this.lblStartVoteRssi.Location = new Point(50, 234);
			this.lblStartVoteRssi.Name = "lblStartVoteRssi";
			this.lblStartVoteRssi.Size = new Size(198, 23);
			this.lblStartVoteRssi.TabIndex = 9;
			this.lblStartVoteRssi.Text = "Start Vote Rssi Threshold [dB]";
			this.lblStartVoteRssi.TextAlign = ContentAlignment.MiddleRight;
			this.lblStartVoteRssi.Visible = false;
			this.chkPriorityAlert.AutoSize = true;
			this.chkPriorityAlert.Location = new Point(261, 137);
			this.chkPriorityAlert.Name = "chkPriorityAlert";
			this.chkPriorityAlert.Size = new Size(103, 20);
			this.chkPriorityAlert.TabIndex = 4;
			this.chkPriorityAlert.Text = "Priority Alert";
			this.chkPriorityAlert.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(7f, 16f);
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(465, 328);
			base.Controls.Add(this.pnlScanBasic);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.Name = "ScanBasicForm";
			this.Text = "Scan";
			base.Load += this.ScanBasicForm_Load;
			base.FormClosing += this.ScanBasicForm_FormClosing;
			this.pnlScanBasic.ResumeLayout(false);
			this.pnlScanBasic.PerformLayout();
			((ISupportInitialize)this.nudAnalogHang).EndInit();
			((ISupportInitialize)this.nudScanTime).EndInit();
			((ISupportInitialize)this.nudStartVoteRssi).EndInit();
			((ISupportInitialize)this.nudFastVoteRssi).EndInit();
			((ISupportInitialize)this.nudVoteHang).EndInit();
			((ISupportInitialize)this.nudDigitHang).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		static ScanBasicForm()
		{
			
			ScanBasicForm.SCL_VOTE_HANG = 0.25m;
			ScanBasicForm.DefaultScanBasic = new ScanBasic();
			ScanBasicForm.data = new ScanBasic();
		}
	}
}
