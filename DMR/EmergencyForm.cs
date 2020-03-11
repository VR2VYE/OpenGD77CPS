using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class EmergencyForm : DockContent, IDisp
	{
		private enum AlarmType : byte
		{
			Disable,
			Normal = 2,
			Silent,
			SilentWithVoice
		}

		private enum AlarmMode : byte
		{
			Alarm,
			AlarmAndCall,
			AlarmAndVoice
		}

		private enum RevertChE
		{
			None,
			Selected
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct EmergencyOne : IVerify<EmergencyOne>
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			private byte[] name;

			private byte alarmType;

			private byte alarmMode;

			private byte revertCh;

			private byte impoliteRetries;

			private byte politeRetries;

			private byte cycles;

			private byte txCycle;

			private byte rxCycle;

			public string Name
			{
				get
				{
					return Settings.smethod_25(this.name);
				}
				set
				{
					byte[] array = Settings.smethod_23(value);
					this.name.Fill((byte)255);
					Array.Copy(array, 0, this.name, 0, Math.Min(array.Length, this.name.Length));
				}
			}

			public byte AlarmType
			{
				get
				{
					if (Enum.IsDefined(typeof(AlarmType), this.alarmType))
					{
						return this.alarmType;
					}
					return 2;
				}
				set
				{
					if (Enum.IsDefined(typeof(AlarmType), value))
					{
						this.alarmType = value;
					}
					else
					{
						this.alarmType = 2;
					}
				}
			}

			public byte AlarmMode
			{
				get
				{
					if (Enum.IsDefined(typeof(AlarmMode), this.alarmMode))
					{
						return this.alarmMode;
					}
					return 0;
				}
				set
				{
					if (Enum.IsDefined(typeof(AlarmMode), value))
					{
						this.alarmMode = value;
					}
					else
					{
						this.alarmMode = 0;
					}
				}
			}

			public int RevertCh
			{
				get
				{
					return this.revertCh;
				}
				set
				{
					this.revertCh = Convert.ToByte(value);
				}
			}

			public decimal ImpoliteRetries
			{
				get
				{
					if (this.impoliteRetries >= 1 && this.impoliteRetries <= 15)
					{
						return this.impoliteRetries;
					}
					return 15m;
				}
				set
				{
					if (value >= 1m && value <= this.ImpoliteRetries)
					{
						this.impoliteRetries = Convert.ToByte(value);
					}
					else
					{
						this.impoliteRetries = 15;
					}
				}
			}

			public decimal PoliteRetries
			{
				get
				{
					if (this.politeRetries >= 0 && this.politeRetries <= 15)
					{
						return this.politeRetries;
					}
					return 5m;
				}
				set
				{
					if (value >= 0m && this.politeRetries <= 15)
					{
						this.politeRetries = Convert.ToByte(value);
					}
					else
					{
						this.politeRetries = 5;
					}
				}
			}

			public decimal Cycles
			{
				get
				{
					if (this.cycles >= 1 && this.cycles <= 10)
					{
						return this.cycles;
					}
					return 3m;
				}
				set
				{
					if (value >= 1m && value <= 10m)
					{
						this.cycles = Convert.ToByte(value);
					}
					else
					{
						this.cycles = 3;
					}
				}
			}

			public decimal TxCycle
			{
				get
				{
					if (this.txCycle >= 1 && this.txCycle <= 12)
					{
						return this.txCycle * 10;
					}
					return 20m;
				}
				set
				{
					value /= 10m;
					if (value >= 1m && value <= 12m)
					{
						this.txCycle = Convert.ToByte(value);
					}
					else
					{
						this.txCycle = 2;
					}
				}
			}

			public decimal RxCycle
			{
				get
				{
					if (this.rxCycle >= 1 && this.rxCycle <= 12)
					{
						return this.rxCycle * 10;
					}
					return 20m;
				}
				set
				{
					value /= 10m;
					if (value >= 1m && value <= 12m)
					{
						this.rxCycle = Convert.ToByte(value);
					}
					else
					{
						this.rxCycle = 2;
					}
				}
			}

			public EmergencyOne(int index)
			{
				
				this = default(EmergencyOne);
				this.name = new byte[8];
			}

			public void Default()
			{
				this.AlarmType = EmergencyForm.DefaultEmg.AlarmType;
				this.AlarmMode = EmergencyForm.DefaultEmg.AlarmMode;
				this.RevertCh = EmergencyForm.DefaultEmg.RevertCh;
				this.ImpoliteRetries = EmergencyForm.DefaultEmg.ImpoliteRetries;
				this.PoliteRetries = EmergencyForm.DefaultEmg.PoliteRetries;
				this.Cycles = EmergencyForm.DefaultEmg.Cycles;
				this.TxCycle = EmergencyForm.DefaultEmg.TxCycle;
				this.RxCycle = EmergencyForm.DefaultEmg.RxCycle;
			}

			public EmergencyOne Clone()
			{
				return Settings.smethod_65(this);
			}

			public void Verify(EmergencyOne def)
			{
				if (this.name[0] != 255)
				{
					if (!Enum.IsDefined(typeof(AlarmType), this.alarmType))
					{
						this.alarmType = def.alarmType;
					}
					if (!Enum.IsDefined(typeof(AlarmMode), this.alarmMode))
					{
						this.alarmMode = def.alarmMode;
					}
					if (!Enum.IsDefined(typeof(RevertChE), (int)this.revertCh))
					{
						int index = this.revertCh - 2;
						if (!ChannelForm.data.DataIsValid(index))
						{
							this.revertCh = 0;
						}
					}
					Settings.smethod_11(ref this.impoliteRetries, (byte)1, (byte)15, def.impoliteRetries);
					Settings.smethod_11(ref this.politeRetries, (byte)0, (byte)15, def.politeRetries);
					Settings.smethod_11(ref this.cycles, (byte)1, (byte)10, def.cycles);
					Settings.smethod_11(ref this.txCycle, (byte)1, (byte)12, def.txCycle);
					Settings.smethod_11(ref this.rxCycle, (byte)1, (byte)12, def.rxCycle);
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class Emergency : IData
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			private EmergencyOne[] emgList;

			public EmergencyOne this[int index]
			{
				get
				{
					if (index >= this.Count)
					{
						throw new ArgumentOutOfRangeException();
					}
					return this.emgList[index];
				}
				set
				{
					this.emgList[index] = value;
				}
			}

			public int Count
			{
				get
				{
					return 32;
				}
			}

			public string Format
			{
				get
				{
					return "System{0}";
				}
			}

			public bool ListIsEmpty
			{
				get
				{
					int num = 0;
					while (true)
					{
						if (num < this.Count)
						{
							if (this.DataIsValid(num))
							{
								break;
							}
							num++;
							continue;
						}
						return true;
					}
					return false;
				}
			}

			public Emergency()
			{
				
				//base._002Ector();
				int num = 0;
				this.emgList = new EmergencyOne[32];
				for (num = 0; num < this.emgList.Length; num++)
				{
					this.emgList[num] = new EmergencyOne(num);
					this.emgList[num].Name = "";
				}
			}

			public int GetMinIndex()
			{
				int num = 0;
				num = 0;
				while (true)
				{
					if (num < this.Count)
					{
						if (string.IsNullOrEmpty(this[num].Name))
						{
							break;
						}
						num++;
						continue;
					}
					return -1;
				}
				return num;
			}

			public bool DataIsValid(int index)
			{
				return !string.IsNullOrEmpty(this[index].Name);
			}

			public void SetIndex(int index, int value)
			{
				if (value == 0)
				{
					this.SetName(index, "");
				}
			}

			public void ClearIndex(int index)
			{
				this.SetName(index, "");
				ChannelForm.data.ClearByEmergency(index);
			}

			public string GetMinName(TreeNode node)
			{
				int num = 0;
				int num2 = 0;
				string text = "";
				num2 = this.GetMinIndex();
				text = string.Format(this.Format, num2 + 1);
				if (!Settings.smethod_51(node, text))
				{
					return text;
				}
				num = 0;
				while (true)
				{
					if (num < this.Count)
					{
						text = string.Format(this.Format, num + 1);
						if (!Settings.smethod_51(node, text))
						{
							break;
						}
						num++;
						continue;
					}
					return "";
				}
				return text;
			}

			public void SetName(int index, string text)
			{
				this.emgList[index].Name = text;
			}

			public string GetName(int index)
			{
				return this.emgList[index].Name;
			}

			public void Default(int index)
			{
				this.emgList[index].Default();
				EmergencyForm.dataEx.Default(index);
			}

			public void Paste(int from, int to)
			{
				this.emgList[to].AlarmType = this.emgList[from].AlarmType;
				this.emgList[to].AlarmMode = this.emgList[from].AlarmMode;
				this.emgList[to].RevertCh = this.emgList[from].RevertCh;
				this.emgList[to].ImpoliteRetries = this.emgList[from].ImpoliteRetries;
				this.emgList[to].PoliteRetries = this.emgList[from].PoliteRetries;
				this.emgList[to].Cycles = this.emgList[from].Cycles;
				this.emgList[to].TxCycle = this.emgList[from].TxCycle;
				this.emgList[to].RxCycle = this.emgList[from].RxCycle;
				EmergencyForm.dataEx.RevertCh[to] = EmergencyForm.dataEx.RevertCh[from];
			}

			public void ClearByData(int chIndex)
			{
				int num = 0;
				for (num = 0; num < this.Count; num++)
				{
					if (this.emgList[num].RevertCh == chIndex + 2)
					{
						this.emgList[num].RevertCh = 0;
					}
				}
			}

			public void Verify()
			{
				int num = 0;
				for (num = 0; num < this.emgList.Length; num++)
				{
					this.emgList[num].Verify(EmergencyForm.DefaultEmg);
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class EmergencyEx
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			private ushort[] revertCh;

			public ushort[] RevertCh
			{
				get
				{
					return this.revertCh;
				}
				set
				{
					this.revertCh = value;
				}
			}

			public EmergencyEx()
			{
				
				//base._002Ector();
				this.revertCh = new ushort[32];
			}

			public void ClearByData(int chIndex)
			{
				int num = 0;
				for (num = 0; num < this.revertCh.Length; num++)
				{
					if (this.revertCh[num] == chIndex + 2)
					{
						this.revertCh[num] = 0;
					}
				}
			}

			public void Default(int index)
			{
				this.RevertCh[index] = (ushort)EmergencyForm.DefaultEmg.RevertCh;
			}

			public void Verify()
			{
				int num = 0;
				int num2 = 0;
				for (num = 0; num < this.revertCh.Length; num++)
				{
					num2 = this.revertCh[num] - 2;
					if (!ChannelForm.data.DataIsValid(num2))
					{
						this.revertCh[num] = 0;
					}
				}
			}
		}

		public const int CNT_EMG = 32;

		public const byte LEN_EMG_NAME = 8;

		public const string SZ_ALARM_TYPE_NAME = "AlarmType";

		public const string SZ_ALARM_MODE_NAME = "AlarmMode";

		public const string SZ_REVERT_CH_NAME = "RevertCh";

		private const byte INC_IMPOLITE_RETRIES = 1;

		private const byte MIN_IMPOLITE_RETRIES = 1;

		private const byte MAX_IMPOLITE_RETRIES = 15;

		private const byte LEN_IMPOLITE_RETRIES = 2;

		private const byte INC_POLITE_RETRIES = 1;

		private const byte MIN_POLITE_RETRIES = 0;

		private const byte MAX_POLITE_RETRIES = 15;

		private const byte LEN_POLITE_RETRIES = 2;

		private const byte REP_POLITE_RETRIES = 15;

		private const string SZ_REP_POLITE_RETRIES = "∞";

		private const byte INC_CYCLES = 1;

		private const byte MIN_CYCLES = 1;

		private const byte MAX_CYCLES = 10;

		private const byte LEN_CYCLES = 2;

		private const byte INC_TX_CYCLE = 1;

		private const byte MIN_TX_CYCLE = 1;

		private const byte MAX_TX_CYCLE = 12;

		private const byte SCL_TX_CYCLE = 10;

		private const byte LEN_TX_CYCLE = 3;

		private const byte INC_RX_CYCLE = 1;

		public const byte MIN_RX_CYCLE = 1;

		private const byte MAX_RX_CYCLE = 12;

		private const byte SCL_RX_CYCLE = 10;

		private const byte LEN_RX_CYCLE = 3;

		//private IContainer components;

		private Label lblAlias;

		private Label lblAlarmType;

		private Label lblAlarmMode;

		private Label lblRevertCh;

		private Label lblImpoliteRetries;

		private Label lblPoliteRetries;

		private Label lblCycles;

		private Label lblTxCycle;

		private Label lblRxCycle;

		private SGTextBox txtName;

		private CustomCombo cmbAlarmType;

		private ComboBox cmbAlarmMode;

		private CustomCombo cmbRevertCh;

		private CustomNumericUpDown nudCycles;

		private CustomNumericUpDown nudTxCycle;

		private CustomNumericUpDown nudRxCycle;

		private CustomNumericUpDown nudImpoliteRetries;

		private CustomNumericUpDown nudPoliteRetries;

		private CustomPanel pnlEmergency;

		private static readonly string[] SZ_ALARM_TYPE;

		private static readonly string[] SZ_ALARM_MODE;

		private static readonly string[] SZ_REVERT_CH;

		public static EmergencyOne DefaultEmg;

		public static Emergency data;

		public static EmergencyEx dataEx;

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

		private void method_0()
		{
			this.lblAlias = new Label();
			this.lblAlarmType = new Label();
			this.lblAlarmMode = new Label();
			this.lblRevertCh = new Label();
			this.lblImpoliteRetries = new Label();
			this.lblPoliteRetries = new Label();
			this.lblCycles = new Label();
			this.lblTxCycle = new Label();
			this.lblRxCycle = new Label();
			this.cmbAlarmMode = new ComboBox();
			this.pnlEmergency = new CustomPanel();
			this.cmbRevertCh = new CustomCombo();
			this.nudPoliteRetries = new CustomNumericUpDown();
			this.nudImpoliteRetries = new CustomNumericUpDown();
			this.nudRxCycle = new CustomNumericUpDown();
			this.nudTxCycle = new CustomNumericUpDown();
			this.nudCycles = new CustomNumericUpDown();
			this.cmbAlarmType = new CustomCombo();
			this.txtName = new SGTextBox();
			this.pnlEmergency.SuspendLayout();
			((ISupportInitialize)this.nudPoliteRetries).BeginInit();
			((ISupportInitialize)this.nudImpoliteRetries).BeginInit();
			((ISupportInitialize)this.nudRxCycle).BeginInit();
			((ISupportInitialize)this.nudTxCycle).BeginInit();
			((ISupportInitialize)this.nudCycles).BeginInit();
			base.SuspendLayout();
			this.lblAlias.Location = new Point(25, 58);
			this.lblAlias.Name = "lblAlias";
			this.lblAlias.Size = new Size(131, 24);
			this.lblAlias.TabIndex = 0;
			this.lblAlias.Text = "Name";
			this.lblAlias.TextAlign = ContentAlignment.MiddleRight;
			this.lblAlarmType.Location = new Point(25, 88);
			this.lblAlarmType.Name = "lblAlarmType";
			this.lblAlarmType.Size = new Size(131, 24);
			this.lblAlarmType.TabIndex = 2;
			this.lblAlarmType.Text = "Alarm Type";
			this.lblAlarmType.TextAlign = ContentAlignment.MiddleRight;
			this.lblAlarmMode.Location = new Point(25, 118);
			this.lblAlarmMode.Name = "lblAlarmMode";
			this.lblAlarmMode.Size = new Size(131, 24);
			this.lblAlarmMode.TabIndex = 4;
			this.lblAlarmMode.Text = "Mode";
			this.lblAlarmMode.TextAlign = ContentAlignment.MiddleRight;
			this.lblRevertCh.Location = new Point(25, 148);
			this.lblRevertCh.Name = "lblRevertCh";
			this.lblRevertCh.Size = new Size(131, 24);
			this.lblRevertCh.TabIndex = 6;
			this.lblRevertCh.Text = "Revert Channel";
			this.lblRevertCh.TextAlign = ContentAlignment.MiddleRight;
			this.lblImpoliteRetries.Location = new Point(25, 178);
			this.lblImpoliteRetries.Name = "lblImpoliteRetries";
			this.lblImpoliteRetries.Size = new Size(131, 24);
			this.lblImpoliteRetries.TabIndex = 8;
			this.lblImpoliteRetries.Text = "Impolite Retries";
			this.lblImpoliteRetries.TextAlign = ContentAlignment.MiddleRight;
			this.lblPoliteRetries.Location = new Point(25, 208);
			this.lblPoliteRetries.Name = "lblPoliteRetries";
			this.lblPoliteRetries.Size = new Size(131, 24);
			this.lblPoliteRetries.TabIndex = 10;
			this.lblPoliteRetries.Text = "Polite Retries";
			this.lblPoliteRetries.TextAlign = ContentAlignment.MiddleRight;
			this.lblCycles.Location = new Point(25, 238);
			this.lblCycles.Name = "lblCycles";
			this.lblCycles.Size = new Size(131, 24);
			this.lblCycles.TabIndex = 12;
			this.lblCycles.Text = "Cycles";
			this.lblCycles.TextAlign = ContentAlignment.MiddleRight;
			this.lblTxCycle.Location = new Point(25, 268);
			this.lblTxCycle.Name = "lblTxCycle";
			this.lblTxCycle.Size = new Size(131, 24);
			this.lblTxCycle.TabIndex = 14;
			this.lblTxCycle.Text = "Tx Cycle Time [s]";
			this.lblTxCycle.TextAlign = ContentAlignment.MiddleRight;
			this.lblRxCycle.Location = new Point(25, 298);
			this.lblRxCycle.Name = "lblRxCycle";
			this.lblRxCycle.Size = new Size(131, 24);
			this.lblRxCycle.TabIndex = 16;
			this.lblRxCycle.Text = "Rx Cycle Time [s]";
			this.lblRxCycle.TextAlign = ContentAlignment.MiddleRight;
			this.cmbAlarmMode.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbAlarmMode.FormattingEnabled = true;
			this.cmbAlarmMode.Items.AddRange(new object[3]
			{
				"紧急报警",
				"紧急报警和呼叫",
				"紧急报警和语音"
			});
			this.cmbAlarmMode.Location = new Point(167, 118);
			this.cmbAlarmMode.Name = "cmbAlarmMode";
			this.cmbAlarmMode.Size = new Size(120, 24);
			this.cmbAlarmMode.TabIndex = 5;
			this.cmbAlarmMode.SelectedIndexChanged += this.cmbAlarmMode_SelectedIndexChanged;
			this.pnlEmergency.AutoScroll = true;
			this.pnlEmergency.AutoSize = true;
			this.pnlEmergency.Controls.Add(this.cmbRevertCh);
			this.pnlEmergency.Controls.Add(this.nudPoliteRetries);
			this.pnlEmergency.Controls.Add(this.lblAlias);
			this.pnlEmergency.Controls.Add(this.nudImpoliteRetries);
			this.pnlEmergency.Controls.Add(this.lblAlarmType);
			this.pnlEmergency.Controls.Add(this.nudRxCycle);
			this.pnlEmergency.Controls.Add(this.lblAlarmMode);
			this.pnlEmergency.Controls.Add(this.nudTxCycle);
			this.pnlEmergency.Controls.Add(this.lblRevertCh);
			this.pnlEmergency.Controls.Add(this.nudCycles);
			this.pnlEmergency.Controls.Add(this.lblImpoliteRetries);
			this.pnlEmergency.Controls.Add(this.lblPoliteRetries);
			this.pnlEmergency.Controls.Add(this.cmbAlarmMode);
			this.pnlEmergency.Controls.Add(this.lblCycles);
			this.pnlEmergency.Controls.Add(this.cmbAlarmType);
			this.pnlEmergency.Controls.Add(this.lblTxCycle);
			this.pnlEmergency.Controls.Add(this.txtName);
			this.pnlEmergency.Controls.Add(this.lblRxCycle);
			this.pnlEmergency.Dock = DockStyle.Fill;
			this.pnlEmergency.Location = new Point(0, 0);
			this.pnlEmergency.Name = "pnlEmergency";
			this.pnlEmergency.Size = new Size(327, 376);
			this.pnlEmergency.TabIndex = 0;
			this.cmbRevertCh.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbRevertCh.FormattingEnabled = true;
			this.cmbRevertCh.Items.AddRange(new object[6]
			{
				"1",
				"2",
				"3",
				"4",
				"5",
				"6"
			});
			this.cmbRevertCh.Location = new Point(167, 148);
			this.cmbRevertCh.Name = "cmbRevertCh";
			this.cmbRevertCh.Size = new Size(120, 24);
			this.cmbRevertCh.TabIndex = 7;
			this.nudPoliteRetries.method_2(null);
			this.nudPoliteRetries.Location = new Point(167, 208);
			this.nudPoliteRetries.Maximum = new decimal(new int[4]
			{
				15,
				0,
				0,
				0
			});
			this.nudPoliteRetries.Name = "nudPoliteRetries";
			this.nudPoliteRetries.method_6(null);
			CustomNumericUpDown @class = this.nudPoliteRetries;
			int[] bits = new int[4];
			@class.method_4(new decimal(bits));
			this.nudPoliteRetries.Size = new Size(120, 23);
			this.nudPoliteRetries.TabIndex = 11;
			this.nudImpoliteRetries.method_2(null);
			this.nudImpoliteRetries.Location = new Point(167, 178);
			this.nudImpoliteRetries.Maximum = new decimal(new int[4]
			{
				15,
				0,
				0,
				0
			});
			this.nudImpoliteRetries.Minimum = new decimal(new int[4]
			{
				1,
				0,
				0,
				0
			});
			this.nudImpoliteRetries.Name = "nudImpoliteRetries";
			this.nudImpoliteRetries.method_6(null);
			CustomNumericUpDown class2 = this.nudImpoliteRetries;
			int[] bits2 = new int[4];
			class2.method_4(new decimal(bits2));
			this.nudImpoliteRetries.Size = new Size(120, 23);
			this.nudImpoliteRetries.TabIndex = 9;
			this.nudImpoliteRetries.Value = new decimal(new int[4]
			{
				1,
				0,
				0,
				0
			});
			this.nudRxCycle.Increment = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			this.nudRxCycle.method_2(null);
			this.nudRxCycle.Location = new Point(167, 298);
			this.nudRxCycle.Maximum = new decimal(new int[4]
			{
				120,
				0,
				0,
				0
			});
			this.nudRxCycle.Minimum = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			this.nudRxCycle.Name = "nudRxCycle";
			this.nudRxCycle.method_6(null);
			CustomNumericUpDown class3 = this.nudRxCycle;
			int[] bits3 = new int[4];
			class3.method_4(new decimal(bits3));
			this.nudRxCycle.Size = new Size(120, 23);
			this.nudRxCycle.TabIndex = 17;
			this.nudRxCycle.Value = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			this.nudRxCycle.ValueChanged += this.nudRxCycle_ValueChanged;
			this.nudTxCycle.Increment = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			this.nudTxCycle.method_2(null);
			this.nudTxCycle.Location = new Point(167, 268);
			this.nudTxCycle.Maximum = new decimal(new int[4]
			{
				120,
				0,
				0,
				0
			});
			this.nudTxCycle.Minimum = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			this.nudTxCycle.Name = "nudTxCycle";
			this.nudTxCycle.method_6(null);
			CustomNumericUpDown class4 = this.nudTxCycle;
			int[] bits4 = new int[4];
			class4.method_4(new decimal(bits4));
			this.nudTxCycle.Size = new Size(120, 23);
			this.nudTxCycle.TabIndex = 15;
			this.nudTxCycle.Value = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			this.nudTxCycle.ValueChanged += this.nudTxCycle_ValueChanged;
			this.nudCycles.method_2(null);
			this.nudCycles.Location = new Point(167, 238);
			this.nudCycles.Maximum = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			this.nudCycles.Minimum = new decimal(new int[4]
			{
				1,
				0,
				0,
				0
			});
			this.nudCycles.Name = "nudCycles";
			this.nudCycles.method_6(null);
			CustomNumericUpDown class5 = this.nudCycles;
			int[] bits5 = new int[4];
			class5.method_4(new decimal(bits5));
			this.nudCycles.Size = new Size(120, 23);
			this.nudCycles.TabIndex = 13;
			this.nudCycles.Value = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			this.cmbAlarmType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbAlarmType.FormattingEnabled = true;
			this.cmbAlarmType.Items.AddRange(new object[4]
			{
				"禁用",
				"常规",
				"静默",
				"静默带语音"
			});
			this.cmbAlarmType.Location = new Point(167, 88);
			this.cmbAlarmType.Name = "cmbAlarmType";
			this.cmbAlarmType.Size = new Size(120, 24);
			this.cmbAlarmType.TabIndex = 3;
			this.cmbAlarmType.SelectedIndexChanged += this.cmbAlarmType_SelectedIndexChanged;
			this.txtName.InputString = null;
			this.txtName.Location = new Point(167, 58);
			this.txtName.MaxByteLength = 0;
			this.txtName.MaxLength = 8;
			this.txtName.Name = "txtName";
			this.txtName.Size = new Size(120, 23);
			this.txtName.TabIndex = 1;
			this.txtName.Leave += this.txtName_Leave;
			base.AutoScaleDimensions = new SizeF(7f, 16f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(327, 376);
			base.Controls.Add(this.pnlEmergency);
			this.Font = new Font("Arial", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Name = "EmergencyForm";
			this.Text = "Emergency System";
			base.Load += this.EmergencyForm_Load;
			base.FormClosing += this.EmergencyForm_FormClosing;
			this.pnlEmergency.ResumeLayout(false);
			this.pnlEmergency.PerformLayout();
			((ISupportInitialize)this.nudPoliteRetries).EndInit();
			((ISupportInitialize)this.nudImpoliteRetries).EndInit();
			((ISupportInitialize)this.nudRxCycle).EndInit();
			((ISupportInitialize)this.nudTxCycle).EndInit();
			((ISupportInitialize)this.nudCycles).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public void SaveData()
		{
			try
			{
				int num = Convert.ToInt32(base.Tag);
				if (this.txtName.Focused)
				{
					this.txtName_Leave(this.txtName, null);
				}
				EmergencyOne value = new EmergencyOne(num);
				value.Name = this.txtName.Text;
				value.AlarmType = (byte)this.cmbAlarmType.method_3();
				value.AlarmMode = (byte)this.cmbAlarmMode.SelectedIndex;
				value.RevertCh = (byte)this.cmbRevertCh.method_3();
				value.ImpoliteRetries = this.nudImpoliteRetries.Value;
				value.PoliteRetries = this.nudPoliteRetries.Value;
				value.Cycles = this.nudCycles.Value;
				value.TxCycle = this.nudTxCycle.Value;
				value.RxCycle = this.nudRxCycle.Value;
				EmergencyForm.data[num] = value;
				EmergencyForm.dataEx.RevertCh[num] = (ushort)this.cmbRevertCh.method_3();
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
				int num = Convert.ToInt32(base.Tag);
				this.method_2();
				this.txtName.Text = EmergencyForm.data[num].Name;
				this.cmbAlarmType.method_2(EmergencyForm.data[num].AlarmType);
				this.cmbAlarmMode.SelectedIndex = EmergencyForm.data[num].AlarmMode;
				if (ChannelForm.CurCntCh > 128)
				{
					this.cmbRevertCh.method_2(EmergencyForm.dataEx.RevertCh[num]);
				}
				else
				{
					this.cmbRevertCh.method_2(EmergencyForm.data[num].RevertCh);
				}
				this.nudImpoliteRetries.Value = EmergencyForm.data[num].ImpoliteRetries;
				this.nudPoliteRetries.Value = EmergencyForm.data[num].PoliteRetries;
				this.nudCycles.Value = EmergencyForm.data[num].Cycles;
				this.nudTxCycle.Value = EmergencyForm.data[num].TxCycle;
				this.nudRxCycle.Value = EmergencyForm.data[num].RxCycle;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			this.method_3();
			this.RefreshByUserMode();
		}

		public void RefreshByUserMode()
		{
			bool flag = Settings.getUserExpertSettings() == Settings.UserMode.Expert;
			this.lblCycles.Enabled &= flag;
			this.nudCycles.Enabled &= flag;
			this.lblTxCycle.Enabled &= flag;
			this.nudTxCycle.Enabled &= flag;
			this.lblRxCycle.Enabled &= flag;
			this.nudRxCycle.Enabled &= flag;
		}

		public void RefreshName()
		{
			int index = Convert.ToInt32(base.Tag);
			this.txtName.Text = EmergencyForm.data[index].Name;
		}

		public EmergencyForm()
		{
			this.method_0();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void method_1()
		{
			int num = 0;
			this.txtName.MaxByteLength = 15;
			this.txtName.KeyPress += Settings.smethod_54;
			this.cmbAlarmType.Items.Clear();
			foreach (byte value in Enum.GetValues(typeof(AlarmType)))
			{
				this.cmbAlarmType.method_1(EmergencyForm.SZ_ALARM_TYPE[num++], value);
			}
			this.cmbAlarmMode.Items.Clear();
			string[] sZ_ALARM_MODE = EmergencyForm.SZ_ALARM_MODE;
			foreach (string item in sZ_ALARM_MODE)
			{
				this.cmbAlarmMode.Items.Add(item);
			}
			this.nudImpoliteRetries.Minimum = 1m;
			this.nudImpoliteRetries.Maximum = 15m;
			this.nudImpoliteRetries.Increment = 1m;
			this.nudImpoliteRetries.method_0(2);
			this.nudImpoliteRetries.method_2("0123456789\b");
			this.nudPoliteRetries.Minimum = 0m;
			this.nudPoliteRetries.Maximum = 15m;
			this.nudPoliteRetries.Increment = 1m;
			this.nudPoliteRetries.method_4(15m);
			this.nudPoliteRetries.method_6("∞");
			this.nudPoliteRetries.method_0(2);
			this.nudPoliteRetries.method_2("0123456789\b");
			this.nudCycles.Minimum = 1m;
			this.nudCycles.Maximum = 10m;
			this.nudCycles.Increment = 1m;
			this.nudCycles.method_0(2);
			this.nudCycles.method_2("0123456789\b");
			this.nudTxCycle.Minimum = 10m;
			this.nudTxCycle.Maximum = 120m;
			this.nudTxCycle.Increment = 10m;
			this.nudTxCycle.method_0(3);
			this.nudTxCycle.method_2("0123456789\b");
			this.nudRxCycle.Minimum = 10m;
			this.nudRxCycle.Maximum = 120m;
			this.nudRxCycle.Increment = 10m;
			this.nudRxCycle.method_0(3);
			this.nudRxCycle.method_2("0123456789\b");
		}

		private void method_2()
		{
			int num = 0;
			string text = "";
			this.cmbRevertCh.method_0();
			for (num = 0; num < EmergencyForm.SZ_REVERT_CH.Length; num++)
			{
				this.cmbRevertCh.method_1(EmergencyForm.SZ_REVERT_CH[num], num);
			}
			for (num = 0; num < ChannelForm.data.Count; num++)
			{
				if (ChannelForm.data.GetChMode(num) == 1 && ChannelForm.data.IsGroupCall(num))
				{
					text = string.Format("{0:d3}:{1}", num + 1, ChannelForm.data.GetName(num));
					this.cmbRevertCh.method_1(text, num + EmergencyForm.SZ_REVERT_CH.Length);
				}
			}
		}

		public static void RefreshCommonLang()
		{
			string name = typeof(EmergencyForm).Name;
			Settings.smethod_78("AlarmType", EmergencyForm.SZ_ALARM_TYPE, name);
			Settings.smethod_78("AlarmMode", EmergencyForm.SZ_ALARM_MODE, name);
			Settings.smethod_78("RevertCh", EmergencyForm.SZ_REVERT_CH, name);
		}

		private void EmergencyForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);
			this.method_1();
			this.DispData();
		}

		private void EmergencyForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
		}

		private void cmbAlarmType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.method_3();
		}

		private void cmbAlarmMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.method_3();
		}

		private void method_3()
		{
			int selectedIndex = this.cmbAlarmType.SelectedIndex;
			int selectedIndex2 = this.cmbAlarmMode.SelectedIndex;
			if (selectedIndex > 0)
			{
				this.cmbAlarmMode.Enabled = true;
				this.cmbRevertCh.Enabled = true;
				if (selectedIndex2 > 1)
				{
					this.nudPoliteRetries.Enabled = false;
					this.nudCycles.Enabled = true;
					this.nudRxCycle.Enabled = true;
					this.nudTxCycle.Enabled = true;
				}
				else
				{
					this.nudPoliteRetries.Enabled = true;
					this.nudCycles.Enabled = false;
					this.nudRxCycle.Enabled = false;
					this.nudTxCycle.Enabled = false;
				}
				this.nudImpoliteRetries.Enabled = true;
			}
			else
			{
				this.cmbAlarmMode.Enabled = false;
				this.cmbRevertCh.Enabled = false;
				this.nudImpoliteRetries.Enabled = false;
				this.nudPoliteRetries.Enabled = false;
				this.nudCycles.Enabled = false;
				this.nudRxCycle.Enabled = false;
				this.nudTxCycle.Enabled = false;
			}
		}

		private void nudTxCycle_ValueChanged(object sender, EventArgs e)
		{
			decimal num = this.nudTxCycle.Value % this.nudTxCycle.Increment;
			if (num != 0m)
			{
				CustomNumericUpDown @class = this.nudTxCycle;
				@class.Value -= num;
			}
		}

		private void nudRxCycle_ValueChanged(object sender, EventArgs e)
		{
			decimal num = this.nudRxCycle.Value % this.nudRxCycle.Increment;
			if (num != 0m)
			{
				CustomNumericUpDown @class = this.nudRxCycle;
				@class.Value -= num;
			}
		}

		private void txtName_Leave(object sender, EventArgs e)
		{
			this.txtName.Text = this.txtName.Text.Trim();
			if (this.Node.Text != this.txtName.Text)
			{
				if (Settings.smethod_50(this.Node, this.txtName.Text))
				{
					this.txtName.Text = this.Node.Text;
				}
				else
				{
					this.Node.Text = this.txtName.Text;
				}
			}
		}

		static EmergencyForm()
		{
			
			EmergencyForm.SZ_ALARM_TYPE = new string[4]
			{
				"Disable",
				"Regular",
				"Silent",
				"Silent w/ Voice"
			};
			EmergencyForm.SZ_ALARM_MODE = new string[3]
			{
				"Emergency Alarm",
				"Emergency Alarm w/ Call",
				"Emergency Alarm w/ Voice to Follow"
			};
			EmergencyForm.SZ_REVERT_CH = new string[2]
			{
				"None",
				"Selected"
			};
			EmergencyForm.data = new Emergency();
			EmergencyForm.dataEx = new EmergencyEx();
		}
	}
}
