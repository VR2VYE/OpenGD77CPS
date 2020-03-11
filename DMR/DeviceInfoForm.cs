using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class DeviceInfoForm : DockContent, IDisp
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class DeviceInfo
		{
			private ushort minFreq;

			private ushort maxFreq;

			private ushort minFreq2;

			private ushort maxFreq2;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			private byte[] lastPrgTime;

			private ushort reserve2;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			private byte[] model;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			private byte[] sn;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			private byte[] cpsSwVer;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			private byte[] hardwareVer;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			private byte[] firmwareVer;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
			private byte[] dspFwVer;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			private byte[] reserve3;

			public string MinFreq
			{
				get
				{
					return this.minFreq.ToString("X");
				}
				set
				{
					this.minFreq = ushort.Parse(value, NumberStyles.HexNumber);
					Settings.MIN_FREQ[0] = ushort.Parse(value);
				}
			}

			public string MaxFreq
			{
				get
				{
					return this.maxFreq.ToString("X");
				}
				set
				{
					this.maxFreq = ushort.Parse(value, NumberStyles.HexNumber);
					Settings.MAX_FREQ[0] = ushort.Parse(value);
				}
			}

			public string MinFreq2
			{
				get
				{
					return this.minFreq2.ToString("X");
				}
				set
				{
					this.minFreq2 = ushort.Parse(value, NumberStyles.HexNumber);
					Settings.MIN_FREQ[1] = ushort.Parse(value);
				}
			}

			public string MaxFreq2
			{
				get
				{
					return this.maxFreq2.ToString("X");
				}
				set
				{
					this.maxFreq2 = ushort.Parse(value, NumberStyles.HexNumber);
					Settings.MAX_FREQ[1] = ushort.Parse(value);
				}
			}

			public string LastPrgTime
			{
				get
				{
					string text = string.Format("{0:X02}{1:X02}/{2:X}/{3:X} {4:X}:{5:X02}", this.lastPrgTime[0], this.lastPrgTime[1], this.lastPrgTime[2], this.lastPrgTime[3], this.lastPrgTime[4], this.lastPrgTime[5]);
					DateTime dateTime = default(DateTime);
					if (DateTime.TryParse(text, out dateTime))
					{
						return text;
					}
					return "";
				}
			}

			public string Model
			{
				get
				{
					int num = 0;
					StringBuilder stringBuilder = new StringBuilder(8);
					for (num = 0; num < 8 && this.model[num] != 255; num++)
					{
						stringBuilder.Append(Convert.ToChar(this.model[num]));
					}
					return stringBuilder.ToString();
				}
				set
				{
					int num = 0;
					this.model.Fill((byte)255);
					for (num = 0; num < value.Length; num++)
					{
						this.model[num] = Convert.ToByte(value[num]);
					}
				}
			}

			public string Sn
			{
				get
				{
					int num = 0;
					StringBuilder stringBuilder = new StringBuilder(16);
					for (num = 0; num < 16 && this.sn[num] != 255; num++)
					{
						stringBuilder.Append(Convert.ToChar(this.sn[num]));
					}
					return stringBuilder.ToString();
				}
				set
				{
					int num = 0;
					this.sn.Fill((byte)255);
					for (num = 0; num < value.Length; num++)
					{
						this.sn[num] = Convert.ToByte(value[num]);
					}
				}
			}

			public string CpsSwVer
			{
				get
				{
					int num = 0;
					StringBuilder stringBuilder = new StringBuilder(8);
					for (num = 0; num < 8 && this.cpsSwVer[num] != 255; num++)
					{
						stringBuilder.Append(Convert.ToChar(this.cpsSwVer[num]));
					}
					return stringBuilder.ToString();
				}
				set
				{
					int num = 0;
					this.cpsSwVer.Fill((byte)255);
					for (num = 0; num < value.Length; num++)
					{
						this.cpsSwVer[num] = Convert.ToByte(value[num]);
					}
				}
			}

			public string HardwareVer
			{
				get
				{
					int num = 0;
					StringBuilder stringBuilder = new StringBuilder(8);
					for (num = 0; num < 8 && this.hardwareVer[num] != 255; num++)
					{
						stringBuilder.Append(Convert.ToChar(this.hardwareVer[num]));
					}
					return stringBuilder.ToString();
				}
				set
				{
					int num = 0;
					this.hardwareVer.Fill((byte)255);
					for (num = 0; num < value.Length; num++)
					{
						this.hardwareVer[num] = Convert.ToByte(value[num]);
					}
				}
			}

			public string FirmwareVer
			{
				get
				{
					int num = 0;
					StringBuilder stringBuilder = new StringBuilder(8);
					for (num = 0; num < 8 && this.firmwareVer[num] != 255; num++)
					{
						stringBuilder.Append(Convert.ToChar(this.firmwareVer[num]));
					}
					return stringBuilder.ToString();
				}
			}

			public string DspFwVer
			{
				get
				{
					int num = 0;
					StringBuilder stringBuilder = new StringBuilder(24);
					for (num = 0; num < 24 && this.dspFwVer[num] != 255; num++)
					{
						stringBuilder.Append(Convert.ToChar(this.dspFwVer[num]));
					}
					return stringBuilder.ToString();
				}
			}

			public DeviceInfo()
			{
				
				//base._002Ector();
				this.lastPrgTime = new byte[6];
				this.model = new byte[8];
				this.sn = new byte[16];
				this.cpsSwVer = new byte[8];
				this.hardwareVer = new byte[8];
				this.firmwareVer = new byte[8];
				this.dspFwVer = new byte[24];
				this.reserve3 = new byte[8];
			}
		}

		public const int LEN_MIN_FREQ = 3;

		public const int LEN_MAX_FREQ = 3;

		public const string SZ_MHZ_FREQ = "0123456789\b";

		public const int LEN_LAST_PRG_TIME = 6;

		public const int LEN_MODEL = 8;

		public const int LEN_SN = 16;

		public const int LEN_CSP_SW_VER = 8;

		public const int LEN_HARDWARE_VER = 8;

		public const int LEN_FIRMWARE_VER = 8;

		public const int LEN_DSP_FW_VER = 24;

		//private IContainer components;

		private Label lblModel;

		private SGTextBox txtModel;

		private Label lblSn;

		private SGTextBox txtSn;

		private SGTextBox sgtextBox_0;

		private Label label_0;

		private SGTextBox txtHardwareVer;

		private Label lblHardwareVer;

		private SGTextBox txtFirmwareVer;

		private Label lblFirmwareVer;

		private SGTextBox sgtextBox_1;

		private Label label_1;

		private TextBox txtLastPrgTime;

		private Label lblLastPrgTime;

		private SGTextBox txtMaxFreq;

		private Label lblSection2;

		private SGTextBox txtMinFreq;

		private Label lblSection1;

		private SGTextBox txtMaxFreq2;

		private SGTextBox txtMinFreq2;

		private Label lblTo2;

		private Label lblTo1;

		private CustomPanel pnlDeviceInfo;

		public static DeviceInfo data;

		public TreeNode Node
		{
			get;
			set;
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
			this.pnlDeviceInfo = new CustomPanel();
			this.lblTo1 = new Label();
			this.lblTo2 = new Label();
			this.lblSection1 = new Label();
			this.label_1 = new Label();
			this.txtModel = new SGTextBox();
			this.lblFirmwareVer = new Label();
			this.txtLastPrgTime = new TextBox();
			this.lblHardwareVer = new Label();
			this.txtMaxFreq2 = new SGTextBox();
			this.txtMaxFreq = new SGTextBox();
			this.label_0 = new Label();
			this.txtMinFreq2 = new SGTextBox();
			this.txtMinFreq = new SGTextBox();
			this.lblSn = new Label();
			this.txtSn = new SGTextBox();
			this.sgtextBox_0 = new SGTextBox();
			this.lblSection2 = new Label();
			this.txtHardwareVer = new SGTextBox();
			this.lblLastPrgTime = new Label();
			this.txtFirmwareVer = new SGTextBox();
			this.lblModel = new Label();
			this.sgtextBox_1 = new SGTextBox();
			this.pnlDeviceInfo.SuspendLayout();
			base.SuspendLayout();
			this.pnlDeviceInfo.AutoScroll = true;
			this.pnlDeviceInfo.AutoSize = true;
			this.pnlDeviceInfo.Controls.Add(this.lblTo1);
			this.pnlDeviceInfo.Controls.Add(this.lblTo2);
			this.pnlDeviceInfo.Controls.Add(this.lblSection1);
			this.pnlDeviceInfo.Controls.Add(this.label_1);
			this.pnlDeviceInfo.Controls.Add(this.txtModel);
			this.pnlDeviceInfo.Controls.Add(this.lblFirmwareVer);
			this.pnlDeviceInfo.Controls.Add(this.txtLastPrgTime);
			this.pnlDeviceInfo.Controls.Add(this.lblHardwareVer);
			this.pnlDeviceInfo.Controls.Add(this.txtMaxFreq2);
			this.pnlDeviceInfo.Controls.Add(this.txtMaxFreq);
			this.pnlDeviceInfo.Controls.Add(this.label_0);
			this.pnlDeviceInfo.Controls.Add(this.txtMinFreq2);
			this.pnlDeviceInfo.Controls.Add(this.txtMinFreq);
			this.pnlDeviceInfo.Controls.Add(this.lblSn);
			this.pnlDeviceInfo.Controls.Add(this.txtSn);
			this.pnlDeviceInfo.Controls.Add(this.sgtextBox_0);
			this.pnlDeviceInfo.Controls.Add(this.lblSection2);
			this.pnlDeviceInfo.Controls.Add(this.txtHardwareVer);
			this.pnlDeviceInfo.Controls.Add(this.lblLastPrgTime);
			this.pnlDeviceInfo.Controls.Add(this.txtFirmwareVer);
			this.pnlDeviceInfo.Controls.Add(this.lblModel);
			this.pnlDeviceInfo.Controls.Add(this.sgtextBox_1);
			this.pnlDeviceInfo.Dock = DockStyle.Fill;
			this.pnlDeviceInfo.Location = new Point(0, 0);
			this.pnlDeviceInfo.Name = "pnlDeviceInfo";
			this.pnlDeviceInfo.Size = new Size(680, 438);
			this.pnlDeviceInfo.TabIndex = 0;
			this.lblTo1.AutoSize = true;
			this.lblTo1.Location = new Point(387, 74);
			this.lblTo1.Name = "lblTo1";
			this.lblTo1.Size = new Size(13, 16);
			this.lblTo1.TabIndex = 2;
			this.lblTo1.Text = "-";
			this.lblTo2.AutoSize = true;
			this.lblTo2.Location = new Point(387, 108);
			this.lblTo2.Name = "lblTo2";
			this.lblTo2.Size = new Size(13, 16);
			this.lblTo2.TabIndex = 6;
			this.lblTo2.Text = "-";
			this.lblSection1.Location = new Point(139, 69);
			this.lblSection1.Name = "lblSection1";
			this.lblSection1.Size = new Size(172, 23);
			this.lblSection1.TabIndex = 0;
			this.lblSection1.Text = "Frequency Range 1 [MHz]";
			this.lblSection1.TextAlign = ContentAlignment.MiddleRight;
			this.label_1.Location = new Point(139, 347);
			this.label_1.Name = "lblDspFwVer";
			this.label_1.Size = new Size(172, 23);
			this.label_1.TabIndex = 20;
			this.label_1.Text = "DSP Version";
			this.label_1.TextAlign = ContentAlignment.MiddleRight;
			this.txtModel.InputString = null;
			this.txtModel.Location = new Point(321, 173);
			this.txtModel.MaxByteLength = 0;
			this.txtModel.Name = "txtModel";
			this.txtModel.ReadOnly = true;
			this.txtModel.Size = new Size(139, 23);
			this.txtModel.TabIndex = 11;
			this.lblFirmwareVer.Location = new Point(139, 312);
			this.lblFirmwareVer.Name = "lblFirmwareVer";
			this.lblFirmwareVer.Size = new Size(172, 23);
			this.lblFirmwareVer.TabIndex = 18;
			this.lblFirmwareVer.Text = "Firmware Version";
			this.lblFirmwareVer.TextAlign = ContentAlignment.MiddleRight;
			this.txtLastPrgTime.Location = new Point(321, 139);
			this.txtLastPrgTime.Name = "txtLastPrgTime";
			this.txtLastPrgTime.ReadOnly = true;
			this.txtLastPrgTime.Size = new Size(139, 23);
			this.txtLastPrgTime.TabIndex = 9;
			this.lblHardwareVer.Location = new Point(139, 277);
			this.lblHardwareVer.Name = "lblHardwareVer";
			this.lblHardwareVer.Size = new Size(172, 23);
			this.lblHardwareVer.TabIndex = 16;
			this.lblHardwareVer.Text = "Hardware Version";
			this.lblHardwareVer.TextAlign = ContentAlignment.MiddleRight;
			this.txtMaxFreq2.InputString = null;
			this.txtMaxFreq2.Location = new Point(404, 104);
			this.txtMaxFreq2.MaxByteLength = 0;
			this.txtMaxFreq2.Name = "txtMaxFreq2";
			this.txtMaxFreq2.ReadOnly = true;
			this.txtMaxFreq2.Size = new Size(61, 23);
			this.txtMaxFreq2.TabIndex = 7;
			this.txtMaxFreq2.Validating += this.txtMaxFreq2_Validating;
			this.txtMaxFreq.InputString = null;
			this.txtMaxFreq.Location = new Point(404, 69);
			this.txtMaxFreq.MaxByteLength = 0;
			this.txtMaxFreq.Name = "txtMaxFreq";
			this.txtMaxFreq.ReadOnly = true;
			this.txtMaxFreq.Size = new Size(61, 23);
			this.txtMaxFreq.TabIndex = 3;
			this.txtMaxFreq.Validating += this.txtMaxFreq_Validating;
			this.label_0.Location = new Point(139, 243);
			this.label_0.Name = "lblCpsSwVer";
			this.label_0.Size = new Size(172, 23);
			this.label_0.TabIndex = 14;
			this.label_0.Text = "CPS Version";
			this.label_0.TextAlign = ContentAlignment.MiddleRight;
			this.txtMinFreq2.InputString = null;
			this.txtMinFreq2.Location = new Point(321, 104);
			this.txtMinFreq2.MaxByteLength = 0;
			this.txtMinFreq2.Name = "txtMinFreq2";
			this.txtMinFreq2.ReadOnly = true;
			this.txtMinFreq2.Size = new Size(61, 23);
			this.txtMinFreq2.TabIndex = 5;
			this.txtMinFreq2.Validating += this.txtMinFreq2_Validating;
			this.txtMinFreq.InputString = null;
			this.txtMinFreq.Location = new Point(321, 69);
			this.txtMinFreq.MaxByteLength = 0;
			this.txtMinFreq.Name = "txtMinFreq";
			this.txtMinFreq.ReadOnly = true;
			this.txtMinFreq.Size = new Size(61, 23);
			this.txtMinFreq.TabIndex = 1;
			this.txtMinFreq.Validating += this.txtMinFreq_Validating;
			this.lblSn.Location = new Point(139, 208);
			this.lblSn.Name = "lblSn";
			this.lblSn.Size = new Size(172, 23);
			this.lblSn.TabIndex = 12;
			this.lblSn.Text = "Serial Number";
			this.lblSn.TextAlign = ContentAlignment.MiddleRight;
			this.txtSn.InputString = null;
			this.txtSn.Location = new Point(321, 208);
			this.txtSn.MaxByteLength = 0;
			this.txtSn.Name = "txtSn";
			this.txtSn.ReadOnly = true;
			this.txtSn.Size = new Size(139, 23);
			this.txtSn.TabIndex = 13;
			this.sgtextBox_0.InputString = null;
			this.sgtextBox_0.Location = new Point(321, 243);
			this.sgtextBox_0.MaxByteLength = 0;
			this.sgtextBox_0.Name = "txtCpsSwVer";
			this.sgtextBox_0.ReadOnly = true;
			this.sgtextBox_0.Size = new Size(139, 23);
			this.sgtextBox_0.TabIndex = 15;
			this.lblSection2.Location = new Point(139, 104);
			this.lblSection2.Name = "lblSection2";
			this.lblSection2.Size = new Size(172, 23);
			this.lblSection2.TabIndex = 4;
			this.lblSection2.Text = "Frequency Range 2 [MHz]";
			this.lblSection2.TextAlign = ContentAlignment.MiddleRight;
			this.txtHardwareVer.InputString = null;
			this.txtHardwareVer.Location = new Point(321, 277);
			this.txtHardwareVer.MaxByteLength = 0;
			this.txtHardwareVer.Name = "txtHardwareVer";
			this.txtHardwareVer.ReadOnly = true;
			this.txtHardwareVer.Size = new Size(139, 23);
			this.txtHardwareVer.TabIndex = 17;
			this.lblLastPrgTime.Location = new Point(139, 139);
			this.lblLastPrgTime.Name = "lblLastPrgTime";
			this.lblLastPrgTime.Size = new Size(172, 23);
			this.lblLastPrgTime.TabIndex = 8;
			this.lblLastPrgTime.Text = "Last Programed Date";
			this.lblLastPrgTime.TextAlign = ContentAlignment.MiddleRight;
			this.txtFirmwareVer.InputString = null;
			this.txtFirmwareVer.Location = new Point(321, 312);
			this.txtFirmwareVer.MaxByteLength = 0;
			this.txtFirmwareVer.Name = "txtFirmwareVer";
			this.txtFirmwareVer.ReadOnly = true;
			this.txtFirmwareVer.Size = new Size(139, 23);
			this.txtFirmwareVer.TabIndex = 19;
			this.lblModel.Location = new Point(139, 173);
			this.lblModel.Name = "lblModel";
			this.lblModel.Size = new Size(172, 23);
			this.lblModel.TabIndex = 10;
			this.lblModel.Text = "Model Name";
			this.lblModel.TextAlign = ContentAlignment.MiddleRight;
			this.sgtextBox_1.InputString = null;
			this.sgtextBox_1.Location = new Point(321, 347);
			this.sgtextBox_1.MaxByteLength = 0;
			this.sgtextBox_1.Name = "txtDspFwVer";
			this.sgtextBox_1.ReadOnly = true;
			this.sgtextBox_1.Size = new Size(139, 23);
			this.sgtextBox_1.TabIndex = 21;
			base.AutoScaleDimensions = new SizeF(7f, 16f);
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(680, 438);
			base.Controls.Add(this.pnlDeviceInfo);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.KeyPreview = true;
			base.Name = "DeviceInfoForm";
			this.Text = "Basic Information";
			base.Load += this.DeviceInfoForm_Load;
			base.FormClosing += this.DeviceInfoForm_FormClosing;
			base.KeyDown += this.DeviceInfoForm_KeyDown;
			this.pnlDeviceInfo.ResumeLayout(false);
			this.pnlDeviceInfo.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public void SaveData()
		{
			DeviceInfoForm.data.MinFreq = this.txtMinFreq.Text;
			DeviceInfoForm.data.MaxFreq = this.txtMaxFreq.Text;
			DeviceInfoForm.data.MinFreq2 = this.txtMinFreq2.Text;
			DeviceInfoForm.data.MaxFreq2 = this.txtMaxFreq2.Text;
			if (Settings.CUR_MODE > 0)
			{
				DeviceInfoForm.data.Sn = this.txtSn.Text;
				DeviceInfoForm.data.Model = this.txtModel.Text;
				DeviceInfoForm.data.CpsSwVer = this.sgtextBox_0.Text;
				DeviceInfoForm.data.HardwareVer = this.txtHardwareVer.Text;
			}
			((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType());
		}

		public void DispData()
		{
			this.txtMinFreq.Text = DeviceInfoForm.data.MinFreq;
			this.txtMaxFreq.Text = DeviceInfoForm.data.MaxFreq;
			this.txtMinFreq2.Text = DeviceInfoForm.data.MinFreq2;
			this.txtMaxFreq2.Text = DeviceInfoForm.data.MaxFreq2;
			this.txtLastPrgTime.Text = DeviceInfoForm.data.LastPrgTime;
			this.txtModel.Text = DeviceInfoForm.data.Model;
			this.txtSn.Text = DeviceInfoForm.data.Sn;
			this.sgtextBox_0.Text = DeviceInfoForm.data.CpsSwVer;
			this.txtHardwareVer.Text = DeviceInfoForm.data.HardwareVer;
			this.txtFirmwareVer.Text = DeviceInfoForm.data.FirmwareVer;
			this.sgtextBox_1.Text = DeviceInfoForm.data.DspFwVer;
			this.RefreshByUserMode();
		}

		public void RefreshByUserMode()
		{
			Settings.getUserExpertSettings();
			bool flag = Settings.CUR_MODE > 0;
			bool flag2 = Settings.CUR_MODE > 1;
			this.txtMinFreq.ReadOnly = !flag;
			this.txtMaxFreq.ReadOnly = !flag;
			this.txtMinFreq2.ReadOnly = !flag;
			this.txtMaxFreq2.ReadOnly = !flag;
			this.txtModel.ReadOnly = !flag2;
			this.txtSn.ReadOnly = !flag2;
			this.txtHardwareVer.ReadOnly = !flag2;
			this.sgtextBox_0.ReadOnly = !flag2;
		}

		public void RefreshName()
		{
		}

		public DeviceInfoForm()
		{
			
			//base._002Ector();
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void method_1()
		{
			if (Settings.CUR_MODE > 0)
			{
				this.txtModel.ReadOnly = true;
				this.txtSn.ReadOnly = true;
			}
			this.txtMinFreq.MaxLength = 3;
			this.txtMinFreq.InputString = "0123456789\b";
			this.txtMaxFreq.MaxLength = 3;
			this.txtMaxFreq.InputString = "0123456789\b";
			this.txtMinFreq2.MaxLength = 3;
			this.txtMinFreq2.InputString = "0123456789\b";
			this.txtMaxFreq2.MaxLength = 3;
			this.txtMaxFreq2.InputString = "0123456789\b";
			this.txtModel.MaxByteLength = 8;
			this.txtSn.MaxByteLength = 16;
			this.sgtextBox_0.MaxByteLength = 8;
			this.txtHardwareVer.MaxByteLength = 8;
			this.txtFirmwareVer.MaxByteLength = 8;
			this.sgtextBox_1.MaxByteLength = 24;
		}

		private void DeviceInfoForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);
			this.method_1();
			this.DispData();
		}

		private void DeviceInfoForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
		}

		private void DeviceInfoForm_KeyDown(object sender, KeyEventArgs e)
		{
		}

		private void txtMinFreq_Validating(object sender, CancelEventArgs e)
		{
			int num = 0;
			int num2 = 0;
			uint num3 = 0u;
			int num4 = 0;
			int num5 = 0;
			uint num6 = 0u;
			num3 = uint.Parse(this.txtMinFreq.Text);
			num6 = uint.Parse(this.txtMaxFreq.Text);
			num = Settings.smethod_21(num3, ref num2);
			num4 = Settings.smethod_21(num6, ref num5);
			if (num >= 0)
			{
				if (Settings.smethod_22((double)num3, (double)num6) >= 0)
				{
					if (num3 >= num6)
					{
						this.txtMinFreq.Text = Settings.VALID_MIN_FREQ[num].ToString();
					}
				}
				else
				{
					this.txtMaxFreq.Text = Settings.VALID_MAX_FREQ[num].ToString();
					if (num3 == Settings.VALID_MAX_FREQ[num])
					{
						this.txtMinFreq.Text = Settings.VALID_MIN_FREQ[num].ToString();
					}
				}
			}
			else if (num4 >= 0)
			{
				this.txtMinFreq.Text = Settings.VALID_MIN_FREQ[num4].ToString();
			}
			else
			{
				this.txtMinFreq.Text = Settings.VALID_MIN_FREQ[num2].ToString();
				this.txtMaxFreq.Text = Settings.VALID_MAX_FREQ[num2].ToString();
			}
		}

		private void txtMaxFreq_Validating(object sender, CancelEventArgs e)
		{
			int num = 0;
			int num2 = 0;
			uint num3 = 0u;
			int num4 = 0;
			int num5 = 0;
			uint num6 = 0u;
			num3 = uint.Parse(this.txtMinFreq.Text);
			num6 = uint.Parse(this.txtMaxFreq.Text);
			num = Settings.smethod_21(num3, ref num2);
			num4 = Settings.smethod_21(num6, ref num5);
			if (num4 >= 0)
			{
				if (Settings.smethod_22((double)num3, (double)num6) >= 0)
				{
					if (num3 >= num6)
					{
						this.txtMaxFreq.Text = Settings.VALID_MAX_FREQ[num4].ToString();
					}
				}
				else
				{
					this.txtMinFreq.Text = Settings.VALID_MIN_FREQ[num4].ToString();
					if (num6 == Settings.VALID_MIN_FREQ[num4])
					{
						this.txtMaxFreq.Text = Settings.VALID_MAX_FREQ[num4].ToString();
					}
				}
			}
			else if (num >= 0)
			{
				this.txtMaxFreq.Text = Settings.VALID_MAX_FREQ[num].ToString();
			}
			else
			{
				this.txtMinFreq.Text = Settings.VALID_MIN_FREQ[num2].ToString();
				this.txtMaxFreq.Text = Settings.VALID_MAX_FREQ[num2].ToString();
			}
		}

		private void txtMinFreq2_Validating(object sender, CancelEventArgs e)
		{
			int num = 0;
			int num2 = 0;
			uint num3 = 0u;
			int num4 = 0;
			int num5 = 0;
			uint num6 = 0u;
			num3 = uint.Parse(this.txtMinFreq2.Text);
			num6 = uint.Parse(this.txtMaxFreq2.Text);
			num = Settings.smethod_21(num3, ref num2);
			num4 = Settings.smethod_21(num6, ref num5);
			if (num >= 0)
			{
				if (Settings.smethod_22((double)num3, (double)num6) >= 0)
				{
					if (num3 >= num6)
					{
						this.txtMinFreq2.Text = Settings.VALID_MIN_FREQ[num].ToString();
					}
				}
				else
				{
					this.txtMaxFreq2.Text = Settings.VALID_MAX_FREQ[num].ToString();
					if (num3 == Settings.VALID_MAX_FREQ[num])
					{
						this.txtMinFreq2.Text = Settings.VALID_MIN_FREQ[num].ToString();
					}
				}
			}
			else if (num4 >= 0)
			{
				this.txtMinFreq2.Text = Settings.VALID_MIN_FREQ[num4].ToString();
			}
			else
			{
				this.txtMinFreq2.Text = Settings.VALID_MIN_FREQ[num2].ToString();
				this.txtMaxFreq2.Text = Settings.VALID_MAX_FREQ[num2].ToString();
			}
		}

		private void txtMaxFreq2_Validating(object sender, CancelEventArgs e)
		{
			int num = 0;
			int num2 = 0;
			uint num3 = 0u;
			int num4 = 0;
			int num5 = 0;
			uint num6 = 0u;
			num3 = uint.Parse(this.txtMinFreq2.Text);
			num6 = uint.Parse(this.txtMaxFreq2.Text);
			num = Settings.smethod_21(num3, ref num2);
			num4 = Settings.smethod_21(num6, ref num5);
			if (num4 >= 0)
			{
				if (Settings.smethod_22((double)num3, (double)num6) >= 0)
				{
					if (num3 >= num6)
					{
						this.txtMaxFreq2.Text = Settings.VALID_MAX_FREQ[num4].ToString();
					}
				}
				else
				{
					this.txtMinFreq2.Text = Settings.VALID_MIN_FREQ[num4].ToString();
					if (num6 == Settings.VALID_MIN_FREQ[num4])
					{
						this.txtMaxFreq2.Text = Settings.VALID_MAX_FREQ[num4].ToString();
					}
				}
			}
			else if (num >= 0)
			{
				this.txtMaxFreq2.Text = Settings.VALID_MAX_FREQ[num].ToString();
			}
			else
			{
				this.txtMinFreq2.Text = Settings.VALID_MIN_FREQ[num2].ToString();
				this.txtMaxFreq2.Text = Settings.VALID_MAX_FREQ[num2].ToString();
			}
		}

		static DeviceInfoForm()
		{
			
			DeviceInfoForm.data = new DeviceInfo();
		}
	}
}
