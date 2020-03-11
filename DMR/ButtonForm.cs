using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class ButtonForm : DockContent, IDisp
	{
		[Serializable]
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class SideKey : IVerify<SideKey>
		{
			private byte emgShortDur;

			private byte longDur;

			private byte sk1Short;

			private byte sk1Long;

			private byte sk2Short;

			private byte sk2Long;

			private byte tkShort;

			private byte tkLong;

			public decimal EmgShortDur
			{
				get
				{
					if (this.emgShortDur >= 1 && this.emgShortDur <= 15)
					{
						return this.emgShortDur * 50;
					}
					return 50m;
				}
				set
				{
					int num = (int)(value / 50m);
					if (num >= 1 && num <= 15)
					{
						this.emgShortDur = (byte)num;
					}
					else
					{
						this.emgShortDur = 1;
					}
				}
			}

			public decimal LongDur
			{
				get
				{
					if (this.longDur >= 4 && this.longDur <= 15)
					{
						return this.longDur * 250;
					}
					return 1000m;
				}
				set
				{
					int num = (int)(value / 250m);
					if (num >= 4 && num <= 15)
					{
						this.longDur = (byte)num;
					}
					else
					{
						this.LongDur = 4m;
					}
				}
			}

			public int Sk1Short
			{
				get
				{
					return this.sk1Short;
				}
				set
				{
					this.sk1Short = (byte)value;
				}
			}

			public int Sk1Long
			{
				get
				{
					return this.sk1Long;
				}
				set
				{
					this.sk1Long = (byte)value;
				}
			}

			public int Sk2Short
			{
				get
				{
					return this.sk2Short;
				}
				set
				{
					this.sk2Short = (byte)value;
				}
			}

			public int Sk2Long
			{
				get
				{
					return this.sk2Long;
				}
				set
				{
					this.sk2Long = (byte)value;
				}
			}

			public int TKShort
			{
				get
				{
					return this.tkShort;
				}
				set
				{
					this.tkShort = (byte)value;
				}
			}

			public int TKLong
			{
				get
				{
					return this.tkLong;
				}
				set
				{
					this.tkLong = (byte)value;
				}
			}

			public void ShortKeyIsValid(ref byte shortKey, ref byte longKey, byte defShortKey)
			{
				if (Settings.smethod_12(shortKey, ButtonForm.MIN_KEY, ButtonForm.MAX_KEY))
				{
					if (shortKey == 21)
					{
						shortKey = defShortKey;
					}
					else if (shortKey == 2)
					{
						longKey = 3;
					}
					else if (shortKey == 3)
					{
						longKey = 2;
					}
				}
				else
				{
					shortKey = defShortKey;
				}
			}

			public void LongKeyIsValid(ref byte longKey, ref byte shortKey, byte defLongKey)
			{
				if (Settings.smethod_12(longKey, ButtonForm.MIN_KEY, ButtonForm.MAX_KEY))
				{
					if (longKey == 2)
					{
						shortKey = 3;
					}
					else if (longKey == 3)
					{
						shortKey = 2;
					}
				}
				else
				{
					longKey = defLongKey;
				}
			}

			public void Verify(SideKey def)
			{
				Settings.smethod_11(ref this.emgShortDur, (byte)1, (byte)15, def.emgShortDur);
				Settings.smethod_11(ref this.longDur, (byte)4, (byte)15, def.longDur);
				this.ShortKeyIsValid(ref this.sk1Short, ref this.sk1Long, def.sk1Short);
				this.LongKeyIsValid(ref this.sk1Long, ref this.sk1Short, def.sk1Long);
				this.ShortKeyIsValid(ref this.sk2Short, ref this.sk2Long, def.sk2Short);
				this.LongKeyIsValid(ref this.sk2Long, ref this.sk2Short, def.sk2Long);
				this.ShortKeyIsValid(ref this.tkShort, ref this.tkLong, def.tkShort);
				this.LongKeyIsValid(ref this.tkLong, ref this.tkShort, def.tkLong);
			}

			public SideKey()
			{
				
				//base._002Ector();
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct OneTouchOne
		{
			private byte flag1;

			private ushort callList;

			private byte msg;

			public int Mode
			{
				get
				{
					return this.flag1 >> 4;
				}
				set
				{
					value = (value & 0xF) << 4;
					this.flag1 &= 15;
					this.flag1 |= (byte)value;
				}
			}

			public int CallType
			{
				get
				{
					return this.flag1 & 0xF;
				}
				set
				{
					value &= 0xF;
					this.flag1 &= 240;
					this.flag1 |= (byte)value;
				}
			}

			public int CallList
			{
				get
				{
					return this.callList;
				}
				set
				{
					this.callList = (ushort)value;
				}
			}

			public int Msg
			{
				get
				{
					return this.msg;
				}
				set
				{
					this.msg = (byte)value;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class OneTouch
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			private OneTouchOne[] oneTouchList;

			public OneTouchOne this[int index]
			{
				get
				{
					if (index >= 6)
					{
						throw new ArgumentOutOfRangeException();
					}
					return this.oneTouchList[index];
				}
				set
				{
					if (index >= 6)
					{
						throw new ArgumentOutOfRangeException();
					}
					this.oneTouchList[index] = value;
				}
			}

			public OneTouch()
			{
				
				//base._002Ector();
				int num = 0;
				this.oneTouchList = new OneTouchOne[6];
				for (num = 0; num < 6; num++)
				{
					this.oneTouchList[num] = default(OneTouchOne);
				}
			}

			public void ClearByContact(int contactIndex)
			{
				int num = 0;
				for (num = 0; num < 6; num++)
				{
					if (this.oneTouchList[num].Mode == 1 && this.oneTouchList[num].CallList == contactIndex)
					{
						this.oneTouchList[num].CallList = 0;
					}
				}
			}

			public void ClearByMessage(int msgIndex)
			{
				int num = 0;
				for (num = 0; num < 6; num++)
				{
					if (this.oneTouchList[num].Mode == 1 && this.oneTouchList[num].CallType == 1 && this.oneTouchList[num].Msg == msgIndex + 1)
					{
						this.oneTouchList[num].Msg = 0;
					}
				}
			}

			public void ClearByDtmfContact(int contactIndex)
			{
				int num = 0;
				for (num = 0; num < 6; num++)
				{
					if (this.oneTouchList[num].Mode == 0 && this.oneTouchList[num].CallList == contactIndex + 1)
					{
						this.oneTouchList[num].CallList = 0;
					}
				}
			}
		}

		private const int MIN_EMG_SHORT_DUR = 1;

		private const int MAX_EMG_SHORT_DUR = 15;

		private const int INC_EMG_SHORT_DUR = 1;

		private const int SCL_EMG_SHORT_DUR = 50;

		private const int LEN_EMG_SHORT_DUR = 3;

		private const int MIN_LONG_DUR = 4;

		private const int MAX_LONG_DUR = 15;

		private const int INC_LONG_DUR = 1;

		private const int SCL_LONG_DUR = 250;

		private const int LEN_LONG_DUR = 4;

		private const string SZ_BUTTON_ITEM_NAME = "ButtonItem";

		private const byte EMERGENCY_ON = 2;

		private const byte EMERGENCY_OFF = 3;

		private const byte FOREVER_MONITOR = 21;

		private const int CNT_ONE_TOUCH = 6;

		private const string SZ_MODE_NAME = "Mode";

		private const string SZ_CALL_TYPE_D_NAME = "CallTypeD";

		private const string SZ_CALL_TYPE_A_NAME = "CallTypeA";

		//private IContainer components;

		private CustomNumericUpDown nudLongDur;

		private CustomNumericUpDown nudEmgShortDur;

		private Label lblLongDur;

		private Label lblEmgShortDur;

		private CustomCombo cmbTkShort;

		private CustomCombo class4_0;

		private CustomCombo class4_1;

		private Label lblTK;

		private Label lblSK2;

		private Label lblSK1;

		private Label lblShortPress;

		private CustomCombo class4_2;

		private DataGridView dgvOneTouch;

		private CustomCombo class4_3;

		private CustomCombo cmbTkLong;

		private Label lblLongPress;

		private DataGridViewComboBoxColumn cmbMode;

		private DataGridViewComboBoxColumn cmbCallType;

		private DataGridViewComboBoxColumn cmbCallList;

		private DataGridViewComboBoxColumn cmbMsg;

		private CustomPanel pnlButton;

		private static readonly string[] SZ_BUTTON_ITEM;

		private static readonly byte MIN_KEY;

		private static readonly byte MAX_KEY;

		private static readonly string[] SZ_MODE;

		private static readonly string[] SZ_CALL_TYPE_D;

		private static readonly string[] SZ_CALL_TYPE_A;

		private Dictionary<string, string> dicCom;

		public static SideKey DefaultSideKey;

		public static SideKey data;

		public static OneTouch data1;

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
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			this.pnlButton = new CustomPanel();
			this.class4_1 = new CustomCombo();
			this.dgvOneTouch = new DataGridView();
			this.cmbMode = new DataGridViewComboBoxColumn();
			this.cmbCallType = new DataGridViewComboBoxColumn();
			this.cmbCallList = new DataGridViewComboBoxColumn();
			this.cmbMsg = new DataGridViewComboBoxColumn();
			this.lblEmgShortDur = new Label();
			this.class4_2 = new CustomCombo();
			this.lblLongDur = new Label();
			this.cmbTkLong = new CustomCombo();
			this.nudEmgShortDur = new CustomNumericUpDown();
			this.cmbTkShort = new CustomCombo();
			this.nudLongDur = new CustomNumericUpDown();
			this.class4_3 = new CustomCombo();
			this.lblSK1 = new Label();
			this.class4_0 = new CustomCombo();
			this.lblSK2 = new Label();
			this.lblLongPress = new Label();
			this.lblTK = new Label();
			this.lblShortPress = new Label();
			this.pnlButton.SuspendLayout();
			((ISupportInitialize)this.dgvOneTouch).BeginInit();
			((ISupportInitialize)this.nudEmgShortDur).BeginInit();
			((ISupportInitialize)this.nudLongDur).BeginInit();
			base.SuspendLayout();
			this.pnlButton.AutoScroll = true;
			this.pnlButton.AutoSize = true;
			this.pnlButton.Controls.Add(this.class4_1);
			this.pnlButton.Controls.Add(this.dgvOneTouch);
			this.pnlButton.Controls.Add(this.lblEmgShortDur);
			this.pnlButton.Controls.Add(this.class4_2);
			this.pnlButton.Controls.Add(this.lblLongDur);
			this.pnlButton.Controls.Add(this.cmbTkLong);
			this.pnlButton.Controls.Add(this.nudEmgShortDur);
			this.pnlButton.Controls.Add(this.cmbTkShort);
			this.pnlButton.Controls.Add(this.nudLongDur);
			this.pnlButton.Controls.Add(this.class4_3);
			this.pnlButton.Controls.Add(this.lblSK1);
			this.pnlButton.Controls.Add(this.class4_0);
			this.pnlButton.Controls.Add(this.lblSK2);
			this.pnlButton.Controls.Add(this.lblLongPress);
			this.pnlButton.Controls.Add(this.lblTK);
			this.pnlButton.Controls.Add(this.lblShortPress);
			this.pnlButton.Dock = DockStyle.Fill;
			this.pnlButton.Location = new Point(0, 0);
			this.pnlButton.Name = "pnlButton";
			this.pnlButton.Size = new Size(627, 499);
			this.pnlButton.TabIndex = 16;
			this.class4_1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.class4_1.FormattingEnabled = true;
			this.class4_1.Location = new Point(170, 137);
			this.class4_1.Name = "cmbSk1Short";
			this.class4_1.Size = new Size(179, 24);
			this.class4_1.TabIndex = 7;
			this.class4_1.SelectedIndexChanged += this.class4_1_SelectedIndexChanged;
			this.dgvOneTouch.AllowUserToAddRows = false;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("Arial", 10f, FontStyle.Regular);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvOneTouch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvOneTouch.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvOneTouch.Columns.AddRange(this.cmbMode, this.cmbCallType, this.cmbCallList, this.cmbMsg);
			this.dgvOneTouch.EditMode = DataGridViewEditMode.EditOnEnter;
			this.dgvOneTouch.Location = new Point(26, 259);
			this.dgvOneTouch.Name = "dgvOneTouch";
			this.dgvOneTouch.RowTemplate.Height = 23;
			this.dgvOneTouch.Size = new Size(574, 212);
			this.dgvOneTouch.TabIndex = 15;
			this.dgvOneTouch.CellValueChanged += this.dgvOneTouch_CellValueChanged;
			this.dgvOneTouch.RowPostPaint += this.dgvOneTouch_RowPostPaint;
			this.dgvOneTouch.DataError += this.dgvOneTouch_DataError;
			this.cmbMode.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
			this.cmbMode.HeaderText = "Mode";
			this.cmbMode.Name = "cmbMode";
			this.cmbCallType.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
			this.cmbCallType.HeaderText = "Call Type";
			this.cmbCallType.Name = "cmbCallType";
			this.cmbCallList.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
			this.cmbCallList.HeaderText = "Call";
			this.cmbCallList.Name = "cmbCallList";
			this.cmbMsg.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
			this.cmbMsg.HeaderText = "Text Message";
			this.cmbMsg.Name = "cmbMsg";
			this.cmbMsg.Width = 150;
			this.lblEmgShortDur.Location = new Point(106, 36);
			this.lblEmgShortDur.Name = "lblEmgShortDur";
			this.lblEmgShortDur.Size = new Size(245, 24);
			this.lblEmgShortDur.TabIndex = 0;
			this.lblEmgShortDur.Text = "Emergency Short Press Duration [ms]";
			this.lblEmgShortDur.TextAlign = ContentAlignment.MiddleRight;
			this.lblEmgShortDur.Visible = false;
			this.class4_2.DropDownStyle = ComboBoxStyle.DropDownList;
			this.class4_2.FormattingEnabled = true;
			this.class4_2.Location = new Point(362, 137);
			this.class4_2.Name = "cmbSk1Long";
			this.class4_2.Size = new Size(179, 24);
			this.class4_2.TabIndex = 8;
			this.class4_2.SelectedIndexChanged += this.class4_2_SelectedIndexChanged;
			this.lblLongDur.Location = new Point(106, 68);
			this.lblLongDur.Name = "lblLongDur";
			this.lblLongDur.Size = new Size(245, 24);
			this.lblLongDur.TabIndex = 2;
			this.lblLongDur.Text = "Long Press Duration [ms]";
			this.lblLongDur.TextAlign = ContentAlignment.MiddleRight;
			this.cmbTkLong.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbTkLong.FormattingEnabled = true;
			this.cmbTkLong.Location = new Point(362, 197);
			this.cmbTkLong.Name = "cmbTkLong";
			this.cmbTkLong.Size = new Size(179, 24);
			this.cmbTkLong.TabIndex = 14;
			this.cmbTkLong.SelectedIndexChanged += this.cmbTkLong_SelectedIndexChanged;
			this.nudEmgShortDur.Increment = new decimal(new int[4]
			{
				50,
				0,
				0,
				0
			});
			this.nudEmgShortDur.method_2(null);
			this.nudEmgShortDur.Location = new Point(367, 36);
			this.nudEmgShortDur.Maximum = new decimal(new int[4]
			{
				750,
				0,
				0,
				0
			});
			this.nudEmgShortDur.Minimum = new decimal(new int[4]
			{
				50,
				0,
				0,
				0
			});
			this.nudEmgShortDur.Name = "nudEmgShortDur";
			this.nudEmgShortDur.method_6(null);
			CustomNumericUpDown @class = this.nudEmgShortDur;
			int[] bits = new int[4];
			@class.method_4(new decimal(bits));
			this.nudEmgShortDur.Size = new Size(120, 23);
			this.nudEmgShortDur.TabIndex = 1;
			this.nudEmgShortDur.Value = new decimal(new int[4]
			{
				50,
				0,
				0,
				0
			});
			this.nudEmgShortDur.Visible = false;
			this.cmbTkShort.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbTkShort.FormattingEnabled = true;
			this.cmbTkShort.Location = new Point(170, 197);
			this.cmbTkShort.Name = "cmbTkShort";
			this.cmbTkShort.Size = new Size(179, 24);
			this.cmbTkShort.TabIndex = 13;
			this.cmbTkShort.SelectedIndexChanged += this.cmbTkShort_SelectedIndexChanged;
			this.nudLongDur.Increment = new decimal(new int[4]
			{
				250,
				0,
				0,
				0
			});
			this.nudLongDur.method_2(null);
			this.nudLongDur.Location = new Point(367, 68);
			this.nudLongDur.Maximum = new decimal(new int[4]
			{
				3750,
				0,
				0,
				0
			});
			this.nudLongDur.Minimum = new decimal(new int[4]
			{
				1500,
				0,
				0,
				0
			});
			this.nudLongDur.Name = "nudLongDur";
			this.nudLongDur.method_6(null);
			CustomNumericUpDown class2 = this.nudLongDur;
			int[] bits2 = new int[4];
			class2.method_4(new decimal(bits2));
			this.nudLongDur.Size = new Size(120, 23);
			this.nudLongDur.TabIndex = 3;
			this.nudLongDur.Value = new decimal(new int[4]
			{
				1500,
				0,
				0,
				0
			});
			this.class4_3.DropDownStyle = ComboBoxStyle.DropDownList;
			this.class4_3.FormattingEnabled = true;
			this.class4_3.Items.AddRange(new object[3]
			{
				"紧急报警",
				"紧急报警和呼叫",
				"紧急报警和语音"
			});
			this.class4_3.Location = new Point(362, 167);
			this.class4_3.Name = "cmbSk2Long";
			this.class4_3.Size = new Size(179, 24);
			this.class4_3.TabIndex = 11;
			this.class4_3.SelectedIndexChanged += this.class4_3_SelectedIndexChanged;
			this.lblSK1.Location = new Point(86, 137);
			this.lblSK1.Name = "lblSK1";
			this.lblSK1.Size = new Size(76, 24);
			this.lblSK1.TabIndex = 6;
			this.lblSK1.Text = "SK1";
			this.lblSK1.TextAlign = ContentAlignment.MiddleRight;
			this.class4_0.DropDownStyle = ComboBoxStyle.DropDownList;
			this.class4_0.FormattingEnabled = true;
			this.class4_0.Location = new Point(170, 167);
			this.class4_0.Name = "cmbSk2Short";
			this.class4_0.Size = new Size(179, 24);
			this.class4_0.TabIndex = 10;
			this.class4_0.SelectedIndexChanged += this.class4_0_SelectedIndexChanged;
			this.lblSK2.Location = new Point(86, 167);
			this.lblSK2.Name = "lblSK2";
			this.lblSK2.Size = new Size(76, 24);
			this.lblSK2.TabIndex = 9;
			this.lblSK2.Text = "SK2";
			this.lblSK2.TextAlign = ContentAlignment.MiddleRight;
			this.lblLongPress.Location = new Point(362, 113);
			this.lblLongPress.Name = "lblLongPress";
			this.lblLongPress.Size = new Size(179, 16);
			this.lblLongPress.TabIndex = 5;
			this.lblLongPress.Text = "Long Press";
			this.lblLongPress.TextAlign = ContentAlignment.MiddleCenter;
			this.lblTK.Location = new Point(86, 197);
			this.lblTK.Name = "lblTK";
			this.lblTK.Size = new Size(76, 24);
			this.lblTK.TabIndex = 12;
			this.lblTK.Text = "TK";
			this.lblTK.TextAlign = ContentAlignment.MiddleRight;
			this.lblShortPress.Location = new Point(170, 112);
			this.lblShortPress.Name = "lblShortPress";
			this.lblShortPress.Size = new Size(179, 16);
			this.lblShortPress.TabIndex = 4;
			this.lblShortPress.Text = "Short Press";
			this.lblShortPress.TextAlign = ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new SizeF(7f, 16f);
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(627, 499);
			base.Controls.Add(this.pnlButton);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.Name = "ButtonForm";
			this.Text = "Buttons";
			base.Load += this.ButtonForm_Load;
			base.FormClosing += this.ButtonForm_FormClosing;
			this.pnlButton.ResumeLayout(false);
			((ISupportInitialize)this.dgvOneTouch).EndInit();
			((ISupportInitialize)this.nudEmgShortDur).EndInit();
			((ISupportInitialize)this.nudLongDur).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public void SaveData()
		{
			ButtonForm.data.EmgShortDur = this.nudEmgShortDur.Value;
			ButtonForm.data.LongDur = this.nudLongDur.Value;
			ButtonForm.data.Sk1Short = this.class4_1.method_3();
			ButtonForm.data.Sk1Long = this.class4_2.method_3();
			ButtonForm.data.Sk2Short = this.class4_0.method_3();
			ButtonForm.data.Sk2Long = this.class4_3.method_3();
			ButtonForm.data.TKShort = this.cmbTkShort.method_3();
			ButtonForm.data.TKLong = this.cmbTkLong.method_3();
			int num = 0;
			for (num = 0; num < this.dgvOneTouch.Rows.Count; num++)
			{
				OneTouchOne value = default(OneTouchOne);
				value.Mode = (int)this.dgvOneTouch.Rows[num].Cells[0].Value;
				value.CallType = (int)this.dgvOneTouch.Rows[num].Cells[1].Value;
				value.CallList = (int)this.dgvOneTouch.Rows[num].Cells[2].Value;
				value.Msg = (int)this.dgvOneTouch.Rows[num].Cells[3].Value;
				ButtonForm.data1[num] = value;
			}
		}

		public void DispData()
		{
			this.method_2();
			this.nudEmgShortDur.Value = ButtonForm.data.EmgShortDur;
			this.nudLongDur.Value = ButtonForm.data.LongDur;
			this.class4_1.method_2(ButtonForm.data.Sk1Short);
			this.class4_2.method_2(ButtonForm.data.Sk1Long);
			this.class4_0.method_2(ButtonForm.data.Sk2Short);
			this.class4_3.method_2(ButtonForm.data.Sk2Long);
			this.cmbTkShort.method_2(ButtonForm.data.TKShort);
			this.cmbTkLong.method_2(ButtonForm.data.TKLong);
			int num = 0;
			for (num = 0; num < this.dgvOneTouch.Rows.Count; num++)
			{
				this.dgvOneTouch.Rows[num].Cells[0].Value = ButtonForm.data1[num].Mode;
				this.kRrieajYgk(num, ButtonForm.data1[num].Mode);
				this.dgvOneTouch.Rows[num].Cells[1].Value = ButtonForm.data1[num].CallType;
				this.dgvOneTouch.Rows[num].Cells[2].Value = ButtonForm.data1[num].CallList;
				this.dgvOneTouch.Rows[num].Cells[3].Value = ButtonForm.data1[num].Msg;
			}
			this.RefreshByUserMode();
		}

		public void RefreshByUserMode()
		{
			bool flag = Settings.getUserExpertSettings() == Settings.UserMode.Expert;
			this.lblLongDur.Enabled &= flag;
			this.nudLongDur.Enabled &= flag;
		}

		public void RefreshName()
		{
		}

		public ButtonForm()
		{
			
			this.dicCom = new Dictionary<string, string>();
			//base._002Ector();
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void method_1()
		{
			int num = 0;
			Settings.smethod_36(this.nudEmgShortDur, new Class13(1, 15, 1, 50m, 3));
			Settings.smethod_36(this.nudLongDur, new Class13(4, 15, 1, 250m, 4));
			this.class4_1.Items.Clear();
			this.class4_2.Items.Clear();
			this.class4_0.Items.Clear();
			this.class4_3.Items.Clear();
			this.cmbTkShort.Items.Clear();
			this.cmbTkLong.Items.Clear();
			for (num = 0; num < ButtonForm.SZ_BUTTON_ITEM.Length; num++)
			{
				if (num != 21 && num != 15)
				{
					this.class4_2.method_1(ButtonForm.SZ_BUTTON_ITEM[num], num);
					this.class4_3.method_1(ButtonForm.SZ_BUTTON_ITEM[num], num);
					this.cmbTkLong.method_1(ButtonForm.SZ_BUTTON_ITEM[num], num);
					this.class4_1.method_1(ButtonForm.SZ_BUTTON_ITEM[num], num);
					this.class4_0.method_1(ButtonForm.SZ_BUTTON_ITEM[num], num);
					this.cmbTkShort.method_1(ButtonForm.SZ_BUTTON_ITEM[num], num);
				}
			}
			num = 0;
			this.dgvOneTouch.RowCount = 6;
			this.cmbMode.Items.Clear();
			string[] sZ_MODE = ButtonForm.SZ_MODE;
			foreach (string string_ in sZ_MODE)
			{
				this.cmbMode.Items.Add(new NameValuePair(string_, num++));
			}
			this.cmbMode.DisplayMember = "Text";
			this.cmbMode.ValueMember = "Value";
		}

		private void ButtonForm_Load(object sender, EventArgs e)
		{
			try
			{
				Settings.smethod_59(base.Controls);
				Settings.smethod_68(this);
				this.method_1();
				this.DispData();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void ButtonForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
		}

		public static void RefreshCommonLang()
		{
			string name = typeof(ButtonForm).Name;
			Settings.smethod_78("ButtonItem", ButtonForm.SZ_BUTTON_ITEM, name);
			Settings.smethod_78("Mode", ButtonForm.SZ_MODE, name);
			Settings.smethod_78("CallTypeD", ButtonForm.SZ_CALL_TYPE_D, name);
			Settings.smethod_78("CallTypeA", ButtonForm.SZ_CALL_TYPE_A, name);
		}

		private void method_2()
		{
			int num = 0;
			int num2 = 0;
			string text = "";
			string text2 = "";
			this.cmbMsg.Items.Clear();
			this.cmbMsg.Items.Add(new NameValuePair(Settings.dicCommon["None"], 0));
			for (num = 0; num < 32; num++)
			{
				if (TextMsgForm.data[num] >= 1 && TextMsgForm.data[num] <= 145)
				{
					if (TextMsgForm.data[num] == 1)
					{
						text = "";
					}
					else
					{
						text2 = TextMsgForm.data.GetText(num);
						num2 = Math.Min(20, text2.Length);
						text = text2.Substring(0, num2);
					}
					this.cmbMsg.Items.Add(new NameValuePair(text, num + 1));
				}
			}
			this.cmbMsg.ValueMember = "Value";
			this.cmbMsg.DisplayMember = "Text";
		}

		private void kRrieajYgk(int int_0, int int_1)
		{
			int num = 0;
			string text = "";
			DataGridViewComboBoxCell dataGridViewComboBoxCell = this.dgvOneTouch.Rows[int_0].Cells[1] as DataGridViewComboBoxCell;
			DataGridViewComboBoxCell dataGridViewComboBoxCell2 = this.dgvOneTouch.Rows[int_0].Cells[2] as DataGridViewComboBoxCell;
			DataGridViewCell dataGridViewCell = this.dgvOneTouch.Rows[int_0].Cells[2];
			dataGridViewComboBoxCell.ValueMember = "Value";
			dataGridViewComboBoxCell.DisplayMember = "Text";
			dataGridViewComboBoxCell2.ValueMember = "Value";
			dataGridViewComboBoxCell2.DisplayMember = "Text";
			switch (int_1)
			{
			case 1:
			{
				num = 0;
				dataGridViewComboBoxCell.Items.Clear();
				string[] sZ_CALL_TYPE_D = ButtonForm.SZ_CALL_TYPE_D;
				foreach (string string_2 in sZ_CALL_TYPE_D)
				{
					dataGridViewComboBoxCell.Items.Add(new NameValuePair(string_2, num++));
				}
				dataGridViewComboBoxCell2.Items.Clear();
				dataGridViewComboBoxCell2.Items.Add(new NameValuePair(Settings.SZ_NONE, 0));
				for (num = 0; num < 1024; num++)
				{
					if (ContactForm.data.DataIsValid(num))
					{
						text = ContactForm.data[num].Name;
						dataGridViewComboBoxCell2.Items.Add(new NameValuePair(text, num + 1));
					}
				}
				break;
			}
			case 2:
			{
				num = 0;
				dataGridViewComboBoxCell.Items.Clear();
				string[] sZ_CALL_TYPE_A = ButtonForm.SZ_CALL_TYPE_A;
				foreach (string string_ in sZ_CALL_TYPE_A)
				{
					dataGridViewComboBoxCell.Items.Add(new NameValuePair(string_, num++));
				}
				dataGridViewComboBoxCell2.Items.Clear();
				dataGridViewComboBoxCell2.Items.Add(new NameValuePair(Settings.SZ_NONE, 0));
				for (num = 0; num < 32; num++)
				{
					if (DtmfContactForm.data.Valid(num))
					{
						text = DtmfContactForm.data.GetName(num);
						dataGridViewComboBoxCell2.Items.Add(new NameValuePair(text, num + 1));
					}
				}
				break;
			}
			}
		}

		private void dgvOneTouch_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
				{
					int num = int.Parse(this.dgvOneTouch.Rows[e.RowIndex].Cells[0].Value.ToString());
					if (e.ColumnIndex == 0)
					{
						if (num != 1 && num != 2)
						{
							this.dgvOneTouch.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.LightGray;
							this.dgvOneTouch.Rows[e.RowIndex].Cells[1].ReadOnly = true;
							this.dgvOneTouch.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.LightGray;
							this.dgvOneTouch.Rows[e.RowIndex].Cells[2].ReadOnly = true;
							this.dgvOneTouch.Rows[e.RowIndex].Cells[3].Style.BackColor = Color.LightGray;
							this.dgvOneTouch.Rows[e.RowIndex].Cells[3].ReadOnly = true;
						}
						else
						{
							this.dgvOneTouch.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.White;
							this.dgvOneTouch.Rows[e.RowIndex].Cells[1].ReadOnly = false;
							this.dgvOneTouch.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.White;
							this.dgvOneTouch.Rows[e.RowIndex].Cells[2].ReadOnly = false;
							this.dgvOneTouch.Rows[e.RowIndex].Cells[3].Style.BackColor = Color.LightGray;
							this.dgvOneTouch.Rows[e.RowIndex].Cells[3].ReadOnly = true;
						}
						this.kRrieajYgk(e.RowIndex, Convert.ToInt32(this.dgvOneTouch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value));
					}
					else if (e.ColumnIndex == 1)
					{
						int num2 = int.Parse(this.dgvOneTouch.Rows[e.RowIndex].Cells[1].Value.ToString());
						if (num == 1)
						{
							if (num2 == 1)
							{
								this.dgvOneTouch.Rows[e.RowIndex].Cells[3].Style.BackColor = Color.White;
								this.dgvOneTouch.Rows[e.RowIndex].Cells[3].ReadOnly = false;
							}
							else
							{
								this.dgvOneTouch.Rows[e.RowIndex].Cells[3].Style.BackColor = Color.LightGray;
								this.dgvOneTouch.Rows[e.RowIndex].Cells[3].ReadOnly = true;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void dgvOneTouch_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			try
			{
				DataGridView dataGridView = sender as DataGridView;
				if (e.RowIndex >= dataGridView.FirstDisplayedScrollingRowIndex)
				{
					using (SolidBrush brush = new SolidBrush(dataGridView.RowHeadersDefaultCellStyle.ForeColor))
					{
						string s = (e.RowIndex + 1).ToString();
						e.Graphics.DrawString(s, e.InheritedRowStyle.Font, brush, (float)(e.RowBounds.Location.X + 15), (float)(e.RowBounds.Location.Y + 5));
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void dgvOneTouch_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
		}

		private void class4_1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedIndex = this.class4_1.SelectedIndex;
			int selectedIndex2 = this.class4_2.SelectedIndex;
			if (selectedIndex != 2 && selectedIndex != 3)
			{
				if (selectedIndex2 != 2 && selectedIndex2 != 3)
				{
					return;
				}
				this.class4_2.SelectedIndex = 0;
			}
			else if (selectedIndex2 + selectedIndex != 5)
			{
				this.class4_2.SelectedIndex = 5 - selectedIndex;
			}
		}

		private void class4_2_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedIndex = this.class4_1.SelectedIndex;
			int selectedIndex2 = this.class4_2.SelectedIndex;
			if (selectedIndex2 != 2 && selectedIndex2 != 3)
			{
				if (selectedIndex != 2 && selectedIndex != 3)
				{
					return;
				}
				this.class4_1.SelectedIndex = 0;
			}
			else if (selectedIndex2 + selectedIndex != 5)
			{
				this.class4_1.SelectedIndex = 5 - selectedIndex2;
			}
		}

		private void class4_0_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedIndex = this.class4_0.SelectedIndex;
			int selectedIndex2 = this.class4_3.SelectedIndex;
			if (selectedIndex != 2 && selectedIndex != 3)
			{
				if (selectedIndex2 != 2 && selectedIndex2 != 3)
				{
					return;
				}
				this.class4_3.SelectedIndex = 0;
			}
			else if (selectedIndex2 + selectedIndex != 5)
			{
				this.class4_3.SelectedIndex = 5 - selectedIndex;
			}
		}

		private void class4_3_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedIndex = this.class4_0.SelectedIndex;
			int selectedIndex2 = this.class4_3.SelectedIndex;
			if (selectedIndex2 != 2 && selectedIndex2 != 3)
			{
				if (selectedIndex != 2 && selectedIndex != 3)
				{
					return;
				}
				this.class4_0.SelectedIndex = 0;
			}
			else if (selectedIndex2 + selectedIndex != 5)
			{
				this.class4_0.SelectedIndex = 5 - selectedIndex2;
			}
		}

		private void cmbTkShort_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedIndex = this.cmbTkShort.SelectedIndex;
			int selectedIndex2 = this.cmbTkLong.SelectedIndex;
			if (selectedIndex != 2 && selectedIndex != 3)
			{
				if (selectedIndex2 != 2 && selectedIndex2 != 3)
				{
					return;
				}
				this.cmbTkLong.SelectedIndex = 0;
			}
			else if (selectedIndex2 + selectedIndex != 5)
			{
				this.cmbTkLong.SelectedIndex = 5 - selectedIndex;
			}
		}

		private void cmbTkLong_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedIndex = this.cmbTkShort.SelectedIndex;
			int selectedIndex2 = this.cmbTkLong.SelectedIndex;
			if (selectedIndex2 != 2 && selectedIndex2 != 3)
			{
				if (selectedIndex != 2 && selectedIndex != 3)
				{
					return;
				}
				this.cmbTkShort.SelectedIndex = 0;
			}
			else if (selectedIndex2 + selectedIndex != 5)
			{
				this.cmbTkShort.SelectedIndex = 5 - selectedIndex2;
			}
		}

		static ButtonForm()
		{
			
			ButtonForm.SZ_BUTTON_ITEM = new string[23]
			{
				"Unassigned",
				"All Alert Tone On/Off",
				"Emergency On",
				"Emergency Off",
				"High/Low Power",
				"Monitor",
				"Nuisance Delete",
				"One Touch Access 1",
				"One Touch Access 2",
				"One Touch Access 3",
				"One Touch Access 4",
				"One Touch Access 5",
				"One Touch Access 6",
				"Repeater/Talkaround",
				"Scan On/Off",
				"Tight/Normal Squelch",
				"Privacy On/Off",
				"Vox On/Off",
				"Zone Select",
				"Battery Indicator",
				"Lone Work On/Off",
				"Permanent Monitor",
				"Phone Exit"
			};
			ButtonForm.MIN_KEY = 0;
			ButtonForm.MAX_KEY = (byte)ButtonForm.SZ_BUTTON_ITEM.Length;
			ButtonForm.SZ_MODE = new string[3]
			{
				"None",
				"Digital",
				"Analog"
			};
			ButtonForm.SZ_CALL_TYPE_D = new string[2]
			{
				"Call",
				"Message"
			};
			ButtonForm.SZ_CALL_TYPE_A = new string[1]
			{
				"DTMF Call"
			};
			ButtonForm.data = new SideKey();
			ButtonForm.data1 = new OneTouch();
		}
	}
}
