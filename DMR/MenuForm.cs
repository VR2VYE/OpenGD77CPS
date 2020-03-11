using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class MenuForm : DockContent, IDisp
	{
		private enum WorkModeE
		{
			VFO,
			MR,
			CH
		}

		private enum ChDispModeE
		{
			FrequencyNum,
			FrequencyName,
			Frequency
		}

		private enum KeyToneE
		{
			KeyToneOff,
			KeyToneOn
		}

		private enum DoubleWaitE
		{
			DoubleWaitOff,
			DoubleSectionDoubleWait,
			DoubleSectionSingleWait
		}

		private enum MenuLangE
		{
			English,
			Chinese
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class MenuSet : IVerify<MenuSet>
		{
			private byte menuHangTime;

			private byte flag1;

			private byte flag2;

			private byte flag3;

			private byte flag4;

			private byte reverse;

			private byte flag5;

			private byte flag6;

			public int MenuHangTime
			{
				get
				{
					return this.menuHangTime;
				}
				set
				{
					this.menuHangTime = (byte)value;
				}
			}

			public bool Info
			{
				get
				{
					return Convert.ToBoolean(this.flag1 & 1);
				}
				set
				{
					if (value)
					{
						this.flag1 |= 1;
					}
					else
					{
						this.flag1 &= 254;
					}
				}
			}

			public bool Scan
			{
				get
				{
					return Convert.ToBoolean(this.flag1 & 2);
				}
				set
				{
					if (value)
					{
						this.flag1 |= 2;
					}
					else
					{
						this.flag1 &= 253;
					}
				}
			}

			public bool ScanEditList
			{
				get
				{
					return Convert.ToBoolean(this.flag1 & 4);
				}
				set
				{
					if (value)
					{
						this.flag1 |= 4;
					}
					else
					{
						this.flag1 &= 251;
					}
				}
			}

			public bool CallPrompt
			{
				get
				{
					return Convert.ToBoolean(this.flag1 & 8);
				}
				set
				{
					if (value)
					{
						this.flag1 |= 8;
					}
					else
					{
						this.flag1 &= 247;
					}
				}
			}

			public bool ContactEdit
			{
				get
				{
					return Convert.ToBoolean(this.flag1 & 0x10);
				}
				set
				{
					if (value)
					{
						this.flag1 |= 16;
					}
					else
					{
						this.flag1 &= 239;
					}
				}
			}

			public bool ManualDial
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

			public bool RadioDetect
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

			public bool RemoteMonitor
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

			public bool RadioActive
			{
				get
				{
					return Convert.ToBoolean(this.flag2 & 1);
				}
				set
				{
					if (value)
					{
						this.flag2 |= 1;
					}
					else
					{
						this.flag2 &= 254;
					}
				}
			}

			public bool RadioKill
			{
				get
				{
					return Convert.ToBoolean(this.flag2 & 2);
				}
				set
				{
					if (value)
					{
						this.flag2 |= 2;
					}
					else
					{
						this.flag2 &= 253;
					}
				}
			}

			public bool OneKeyDial
			{
				get
				{
					return Convert.ToBoolean(this.flag2 & 4);
				}
				set
				{
					if (value)
					{
						this.flag2 |= 4;
					}
					else
					{
						this.flag2 &= 251;
					}
				}
			}

			public bool TalkAround
			{
				get
				{
					return Convert.ToBoolean(this.flag2 & 8);
				}
				set
				{
					if (value)
					{
						this.flag2 |= 8;
					}
					else
					{
						this.flag2 &= 247;
					}
				}
			}

			public bool ToneHint
			{
				get
				{
					return Convert.ToBoolean(this.flag2 & 0x10);
				}
				set
				{
					if (value)
					{
						this.flag2 |= 16;
					}
					else
					{
						this.flag2 &= 239;
					}
				}
			}

			public bool Power
			{
				get
				{
					return Convert.ToBoolean(this.flag2 & 0x20);
				}
				set
				{
					if (value)
					{
						this.flag2 |= 32;
					}
					else
					{
						this.flag2 &= 223;
					}
				}
			}

			public bool Backlight
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

			public bool BootScreen
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

			public bool KeyboardLock
			{
				get
				{
					return Convert.ToBoolean(this.flag3 & 1);
				}
				set
				{
					if (value)
					{
						this.flag3 |= 1;
					}
					else
					{
						this.flag3 &= 254;
					}
				}
			}

			public bool Led
			{
				get
				{
					return Convert.ToBoolean(this.flag3 & 2);
				}
				set
				{
					if (value)
					{
						this.flag3 |= 2;
					}
					else
					{
						this.flag3 &= 253;
					}
				}
			}

			public bool Sql
			{
				get
				{
					return Convert.ToBoolean(this.flag3 & 4);
				}
				set
				{
					if (value)
					{
						this.flag3 |= 4;
					}
					else
					{
						this.flag3 &= 251;
					}
				}
			}

			public bool Encrypt
			{
				get
				{
					return Convert.ToBoolean(this.flag3 & 8);
				}
				set
				{
					if (value)
					{
						this.flag3 |= 8;
					}
					else
					{
						this.flag3 &= 247;
					}
				}
			}

			public bool Vox
			{
				get
				{
					return Convert.ToBoolean(this.flag3 & 0x10);
				}
				set
				{
					if (value)
					{
						this.flag3 |= 16;
					}
					else
					{
						this.flag3 &= 239;
					}
				}
			}

			public bool PwdLock
			{
				get
				{
					return Convert.ToBoolean(this.flag3 & 0x20);
				}
				set
				{
					if (value)
					{
						this.flag3 |= 32;
					}
					else
					{
						this.flag3 &= 223;
					}
				}
			}

			public bool MissedCall
			{
				get
				{
					return Convert.ToBoolean(this.flag3 & 0x40);
				}
				set
				{
					if (value)
					{
						this.flag3 |= 64;
					}
					else
					{
						this.flag3 &= 191;
					}
				}
			}

			public bool ReceivedCall
			{
				get
				{
					return Convert.ToBoolean(this.flag3 & 0x80);
				}
				set
				{
					if (value)
					{
						this.flag3 |= 128;
					}
					else
					{
						this.flag3 &= 127;
					}
				}
			}

			public bool DialedCall
			{
				get
				{
					return Convert.ToBoolean(this.flag4 & 1);
				}
				set
				{
					if (value)
					{
						this.flag4 |= 1;
					}
					else
					{
						this.flag4 &= 254;
					}
				}
			}

			public bool ChDisp
			{
				get
				{
					return Convert.ToBoolean(this.flag4 & 2);
				}
				set
				{
					if (value)
					{
						this.flag4 |= 2;
					}
					else
					{
						this.flag4 &= 253;
					}
				}
			}

			public bool DoubleWait
			{
				get
				{
					return Convert.ToBoolean(this.flag4 & 4);
				}
				set
				{
					if (value)
					{
						this.flag4 |= 4;
					}
					else
					{
						this.flag4 &= 251;
					}
				}
			}

			public bool Gps
			{
				get
				{
					return Convert.ToBoolean(this.flag4 & 8);
				}
				set
				{
					if (value)
					{
						this.flag4 |= 8;
					}
					else
					{
						this.flag4 &= 247;
					}
				}
			}

			public bool Fm
			{
				get
				{
					return Convert.ToBoolean(this.flag4 & 0x10);
				}
				set
				{
					if (value)
					{
						this.flag4 |= 16;
					}
					else
					{
						this.flag4 &= 239;
					}
				}
			}

			public bool P1Key
			{
				get
				{
					return Convert.ToBoolean(this.flag4 & 0x20);
				}
				set
				{
					if (value)
					{
						this.flag4 |= 32;
					}
					else
					{
						this.flag4 &= 223;
					}
				}
			}

			public int KeyLockTime
			{
				get
				{
					return this.flag5 & 3;
				}
				set
				{
					this.flag5 &= 252;
					this.flag5 |= Convert.ToByte(value & 3);
				}
			}

			public int BacklightTime
			{
				get
				{
					return (this.flag5 & 0xC) >> 2;
				}
				set
				{
					this.flag5 &= 243;
					value &= 3;
					this.flag5 |= Convert.ToByte((value & 3) << 2);
				}
			}

			public int WorkMode
			{
				get
				{
					int num = (this.flag5 & 0x30) >> 4;
					if (Enum.IsDefined(typeof(WorkModeE), num))
					{
						return num;
					}
					return 1;
				}
				set
				{
					this.flag5 &= 207;
					if (!Enum.IsDefined(typeof(WorkModeE), value))
					{
						value = 1;
					}
					this.flag5 |= Convert.ToByte((value & 3) << 4);
				}
			}

			public int ChDispMode
			{
				get
				{
					int num = (this.flag5 & 0xC0) >> 6;
					if (Enum.IsDefined(typeof(ChDispModeE), num))
					{
						return num;
					}
					return 1;
				}
				set
				{
					this.flag5 &= 63;
					if (!Enum.IsDefined(typeof(ChDispModeE), value))
					{
						value = 1;
					}
					this.flag5 |= Convert.ToByte((value & 3) << 6);
				}
			}

			public int Brightness
			{
				get
				{
					int num = (this.flag6 & 0xE) >> 1;
					if (num >= 0 && num <= 4)
					{
						return num;
					}
					return 2;
				}
				set
				{
					this.flag6 &= 241;
					if (value < 0 || value > 4)
					{
						value = 2;
					}
					this.flag6 |= Convert.ToByte(value << 1);
				}
			}

			public int MenuLang
			{
				get
				{
					int num = (this.flag6 & 0x10) >> 4;
					if (Enum.IsDefined(typeof(MenuLangE), num))
					{
						return num;
					}
					return 1;
				}
				set
				{
					this.flag6 &= 239;
					if (!Enum.IsDefined(typeof(MenuLangE), value))
					{
						value = 1;
					}
					this.flag6 |= Convert.ToByte(value << 4);
				}
			}

			public int KeyTone
			{
				get
				{
					int num = (this.flag6 & 0x20) >> 5;
					if (Enum.IsDefined(typeof(KeyToneE), num))
					{
						return num;
					}
					return 1;
				}
				set
				{
					this.flag6 &= 223;
					if (!Enum.IsDefined(typeof(KeyToneE), value))
					{
						value = 1;
					}
					this.flag6 |= Convert.ToByte(value << 5);
				}
			}

			public int DoubleWaitSwitch
			{
				get
				{
					int num = (this.flag6 & 0xC0) >> 6;
					if (Enum.IsDefined(typeof(DoubleWaitE), num))
					{
						return num;
					}
					return 1;
				}
				set
				{
					this.flag6 &= 63;
					if (!Enum.IsDefined(typeof(DoubleWaitE), value))
					{
						value = 1;
					}
					this.flag6 |= Convert.ToByte(value << 6);
				}
			}

			public void Verify(MenuSet def)
			{
				Settings.smethod_11(ref this.menuHangTime, (byte)0, (byte)30, def.menuHangTime);
				if (!Enum.IsDefined(typeof(WorkModeE), this.WorkMode))
				{
					this.WorkMode = def.WorkMode;
				}
				if (!Enum.IsDefined(typeof(ChDispModeE), this.ChDispMode))
				{
					this.ChDispMode = def.ChDispMode;
				}
			}

			public MenuSet()
			{
				
			}
		}

		private const int MIN_MENU_HANG_TIME = 0;

		private const int MAX_MENU_HANG_TIME = 30;

		public const string SZ_KEY_LOCK_TIME_NAME = "KeyLockTime";

		public const string SZ_BACKLIGHT_TIME_NAME = "BacklightTime";

		public const string SZ_WORK_MODE_NAME = "WorkMode";

		public const string SZ_CH_DISP_MODE_NAME = "ChDispMode";

		public const string SZ_KEY_TONE_NAME = "KeyTone";

		public const string SZ_DOUBLE_WAIT_NAME = "DoubleWait";

		public const string SZ_MENU_LANG_NAME = "MenuLang";

		private const int MIN_BRIGHTNESS = 0;

		private const int MAX_BRIGHTNESS = 4;

		private static readonly string[] SZ_KEY_LOCK_TIME;

		private static readonly string[] SZ_BACKLIGHT_TIME;

		private static readonly string[] SZ_WORK_MODE;

		private static readonly string[] SZ_CH_DISP_MODE;

		private static readonly string[] SZ_KEY_TONE;

		private static readonly string[] SZ_DOUBLE_WAIT;

		private static readonly string[] SZ_MENU_LANG;

		public static MenuSet DefaultMenu;

		public static MenuSet data;

		//private IContainer components;

		private CheckBox chkInfo;

		private CheckBox chkScan;

		private CheckBox chkScanEditList;

		private CheckBox chkCallPrompt;

		private CheckBox chkManualDial;

		private CheckBox chkContactEdit;

		private CheckBox chkRadioDetect;

		private CheckBox chkRemoteMonitor;

		private CheckBox chkRadioKill;

		private CheckBox chkRadioActive;

		private CheckBox chkOneKeyDial;

		private CheckBox chkMissedCall;

		private CheckBox chkDialedCall;

		private CheckBox chkReceivedCall;

		private CheckBox chkTalkAround;

		private CheckBox chkEncrypt;

		private CheckBox chkBootScreen;

		private CheckBox chkPower;

		private CheckBox chkPwdLock;

		private CheckBox chkLed;

		private CheckBox chkToneHint;

		private CheckBox chkVox;

		private CheckBox chkKeyboardLock;

		private CheckBox chkBacklight;

		private CheckBox chkSql;

		private CheckBox chkDoubleWait;

		private CustomCombo cmbKeyLockTime;

		private CustomCombo cmbBacklightTime;

		private CustomCombo cmbWorkMode;

		private CustomCombo cmbChDispMode;

		private CustomCombo cmbMenuHangTime;

		private Label lblMenuHangTime;

		private Label lblKeyLockTime;

		private Label lblBacklightTime;

		private Label lblWorkMode;

		private Label lblChDispMode;

		private CheckBox chkChDisp;

		private DoubleClickGroupBox grpBasic;

		private DoubleClickGroupBox grpScan;

		private DoubleClickGroupBox grpContact;

		private DoubleClickGroupBox grpSetting;

		private DoubleClickGroupBox grpCall;

		private Label lblDwSwitch;

		private Label lblKeyTone;

		private CustomCombo cmbDwSwitch;

		private CustomCombo cmbKeyTone;

		private Label lblMenuLang;

		private CustomCombo cmbMenuLang;

		private Label lblBrightness;

		private CustomCombo cmbBrightness;

		private CheckBox chkFm;

		private CheckBox chkP1Key;

		private CheckBox chkGps;

		public TreeNode Node
		{
			get;
			set;
		}

		public void SaveData()
		{
			MenuForm.data.MenuHangTime = this.cmbMenuHangTime.SelectedIndex;
			MenuForm.data.Info = this.chkInfo.Checked;
			MenuForm.data.Scan = this.chkScan.Checked;
			MenuForm.data.ScanEditList = this.chkScanEditList.Checked;
			MenuForm.data.CallPrompt = this.chkCallPrompt.Checked;
			MenuForm.data.ContactEdit = this.chkContactEdit.Checked;
			MenuForm.data.ManualDial = this.chkManualDial.Checked;
			MenuForm.data.RadioDetect = this.chkRadioDetect.Checked;
			MenuForm.data.RemoteMonitor = this.chkRemoteMonitor.Checked;
			MenuForm.data.RadioActive = this.chkRadioActive.Checked;
			MenuForm.data.RadioKill = this.chkRadioKill.Checked;
			MenuForm.data.OneKeyDial = this.chkOneKeyDial.Checked;
			MenuForm.data.TalkAround = this.chkTalkAround.Checked;
			MenuForm.data.ToneHint = this.chkToneHint.Checked;
			MenuForm.data.Power = this.chkPower.Checked;
			MenuForm.data.Backlight = this.chkBacklight.Checked;
			MenuForm.data.BootScreen = this.chkBootScreen.Checked;
			MenuForm.data.KeyboardLock = this.chkKeyboardLock.Checked;
			MenuForm.data.Led = this.chkLed.Checked;
			MenuForm.data.Sql = this.chkSql.Checked;
			MenuForm.data.Encrypt = this.chkEncrypt.Checked;
			MenuForm.data.Vox = this.chkVox.Checked;
			MenuForm.data.PwdLock = this.chkPwdLock.Checked;
			MenuForm.data.MissedCall = this.chkMissedCall.Checked;
			MenuForm.data.ReceivedCall = this.chkReceivedCall.Checked;
			MenuForm.data.DialedCall = this.chkDialedCall.Checked;
			MenuForm.data.ChDisp = this.chkChDisp.Checked;
			MenuForm.data.DoubleWait = this.chkDoubleWait.Checked;
			MenuForm.data.KeyLockTime = this.cmbKeyLockTime.SelectedIndex;
			MenuForm.data.BacklightTime = this.cmbBacklightTime.SelectedIndex;
			MenuForm.data.WorkMode = this.cmbWorkMode.SelectedIndex;
			MenuForm.data.ChDispMode = this.cmbChDispMode.SelectedIndex;
			MenuForm.data.KeyTone = this.cmbKeyTone.SelectedIndex;
			MenuForm.data.MenuLang = this.cmbMenuLang.SelectedIndex;
			MenuForm.data.DoubleWaitSwitch = this.cmbDwSwitch.SelectedIndex;
			MenuForm.data.Gps = this.chkGps.Checked;
			MenuForm.data.Fm = this.chkFm.Checked;
			MenuForm.data.P1Key = this.chkP1Key.Checked;
			MenuForm.data.Brightness = this.cmbBrightness.SelectedIndex;
		}

		public void DispData()
		{
			this.cmbMenuHangTime.SelectedIndex = MenuForm.data.MenuHangTime;
			this.chkInfo.Checked = MenuForm.data.Info;
			this.chkScan.Checked = MenuForm.data.Scan;
			this.chkScanEditList.Checked = MenuForm.data.ScanEditList;
			this.chkCallPrompt.Checked = MenuForm.data.CallPrompt;
			this.chkContactEdit.Checked = MenuForm.data.ContactEdit;
			this.chkManualDial.Checked = MenuForm.data.ManualDial;
			this.chkRadioDetect.Checked = MenuForm.data.RadioDetect;
			this.chkRemoteMonitor.Checked = MenuForm.data.RemoteMonitor;
			this.chkRadioActive.Checked = MenuForm.data.RadioActive;
			this.chkRadioKill.Checked = MenuForm.data.RadioKill;
			this.chkOneKeyDial.Checked = MenuForm.data.OneKeyDial;
			this.chkTalkAround.Checked = MenuForm.data.TalkAround;
			this.chkToneHint.Checked = MenuForm.data.ToneHint;
			this.chkPower.Checked = MenuForm.data.Power;
			this.chkBacklight.Checked = MenuForm.data.Backlight;
			this.chkBootScreen.Checked = MenuForm.data.BootScreen;
			this.chkKeyboardLock.Checked = MenuForm.data.KeyboardLock;
			this.chkLed.Checked = MenuForm.data.Led;
			this.chkSql.Checked = MenuForm.data.Sql;
			this.chkEncrypt.Checked = MenuForm.data.Encrypt;
			this.chkVox.Checked = MenuForm.data.Vox;
			this.chkPwdLock.Checked = MenuForm.data.PwdLock;
			this.chkMissedCall.Checked = MenuForm.data.MissedCall;
			this.chkReceivedCall.Checked = MenuForm.data.ReceivedCall;
			this.chkDialedCall.Checked = MenuForm.data.DialedCall;
			this.chkChDisp.Checked = MenuForm.data.ChDisp;
			this.cmbChDispMode.SelectedIndex = MenuForm.data.ChDispMode;
			this.chkDoubleWait.Checked = MenuForm.data.DoubleWait;
			this.cmbDwSwitch.SelectedIndex = MenuForm.data.DoubleWaitSwitch;
			this.cmbKeyLockTime.SelectedIndex = MenuForm.data.KeyLockTime;
			this.cmbBacklightTime.SelectedIndex = MenuForm.data.BacklightTime;
			this.cmbWorkMode.SelectedIndex = MenuForm.data.WorkMode;
			this.cmbMenuLang.SelectedIndex = MenuForm.data.MenuLang;
			this.cmbKeyTone.SelectedIndex = MenuForm.data.KeyTone;
			this.chkGps.Checked = MenuForm.data.Gps;
			this.chkFm.Checked = MenuForm.data.Fm;
			this.chkP1Key.Checked = MenuForm.data.P1Key;
			this.cmbBrightness.SelectedIndex = MenuForm.data.Brightness;
			this.RefreshByUserMode();
		}

		public void RefreshByUserMode()
		{
			bool flag = Settings.getUserExpertSettings() == Settings.UserMode.Expert;
			this.lblMenuHangTime.Enabled &= flag;
			this.cmbMenuHangTime.Enabled &= flag;
			this.chkContactEdit.Enabled &= flag;
			this.chkDialedCall.Enabled &= flag;
			this.chkRadioDetect.Enabled &= flag;
			this.chkRemoteMonitor.Enabled &= flag;
			this.chkRadioActive.Enabled &= flag;
			this.chkRadioKill.Enabled &= flag;
			this.chkOneKeyDial.Enabled &= flag;
			this.chkToneHint.Enabled &= flag;
			this.lblKeyTone.Enabled &= flag;
			this.cmbKeyTone.Enabled &= flag;
			this.chkBacklight.Enabled &= flag;
			this.lblBacklightTime.Enabled &= flag;
			this.cmbBacklightTime.Enabled &= flag;
			this.chkBootScreen.Enabled &= flag;
			this.chkKeyboardLock.Enabled &= flag;
			this.lblKeyLockTime.Enabled &= flag;
			this.cmbKeyLockTime.Enabled &= flag;
			this.chkLed.Enabled &= flag;
			this.chkEncrypt.Enabled &= flag;
			this.lblMenuLang.Enabled &= flag;
			this.cmbMenuLang.Enabled &= flag;
			this.chkPwdLock.Enabled &= flag;
			this.chkChDisp.Enabled &= flag;
			this.chkMissedCall.Enabled &= flag;
			this.chkReceivedCall.Enabled &= flag;
			this.chkDialedCall.Enabled &= flag;
		}

		public void RefreshName()
		{
		}

		public MenuForm()
		{
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void method_0()
		{
			Settings.smethod_41(this.cmbMenuHangTime, 1, 30);
			this.cmbMenuHangTime.Items.Insert(0, "∞");
			Settings.smethod_37(this.cmbKeyLockTime, MenuForm.SZ_KEY_LOCK_TIME);
			Settings.smethod_37(this.cmbBacklightTime, MenuForm.SZ_BACKLIGHT_TIME);
			Settings.smethod_37(this.cmbWorkMode, MenuForm.SZ_WORK_MODE);
			Settings.smethod_37(this.cmbChDispMode, MenuForm.SZ_CH_DISP_MODE);
			Settings.smethod_37(this.cmbMenuLang, MenuForm.SZ_MENU_LANG);
			Settings.smethod_37(this.cmbKeyTone, MenuForm.SZ_KEY_TONE);
			Settings.smethod_37(this.cmbDwSwitch, MenuForm.SZ_DOUBLE_WAIT);
			this.grpBasic.method_1(true);
			this.grpCall.method_1(true);
			this.grpContact.method_1(true);
			this.grpScan.method_1(true);
			this.grpSetting.method_1(true);
			Settings.smethod_41(this.cmbBrightness, 1, 5);
		}

		public static void RefreshCommonLang()
		{
			string name = typeof(MenuForm).Name;
			Settings.smethod_78("BacklightTime", MenuForm.SZ_BACKLIGHT_TIME, name);
			Settings.smethod_78("KeyLockTime", MenuForm.SZ_KEY_LOCK_TIME, name);
			Settings.smethod_78("WorkMode", MenuForm.SZ_WORK_MODE, name);
			Settings.smethod_78("ChDispMode", MenuForm.SZ_CH_DISP_MODE, name);
			Settings.smethod_78("KeyTone", MenuForm.SZ_KEY_TONE, name);
			Settings.smethod_78("DoubleWait", MenuForm.SZ_DOUBLE_WAIT, name);
			Settings.smethod_78("MenuLang", MenuForm.SZ_MENU_LANG, name);
		}

		private void MenuForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);
			this.method_0();
			this.DispData();
		}

		private void MenuForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
		}

		private void chkFm_CheckedChanged(object sender, EventArgs e)
		{
			MenuForm.data.Fm = this.chkFm.Checked;
			if (!this.chkFm.Checked)
			{
				AttachmentForm.data.RefreshP1Key();
			}
			this.method_1();
		}

		private void method_1()
		{
			MainForm mainForm = base.MdiParent as MainForm;
			if (mainForm != null)
			{
				mainForm.RefreshRelatedForm(typeof(MenuForm));
			}
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
			this.lblBrightness = new Label();
			this.chkFm = new CheckBox();
			this.chkP1Key = new CheckBox();
			this.chkGps = new CheckBox();
			this.cmbBrightness = new CustomCombo();
			this.grpCall = new DoubleClickGroupBox();
			this.chkReceivedCall = new CheckBox();
			this.chkDialedCall = new CheckBox();
			this.chkMissedCall = new CheckBox();
			this.grpSetting = new DoubleClickGroupBox();
			this.chkDoubleWait = new CheckBox();
			this.lblDwSwitch = new Label();
			this.lblKeyTone = new Label();
			this.lblBacklightTime = new Label();
			this.chkSql = new CheckBox();
			this.chkBacklight = new CheckBox();
			this.chkKeyboardLock = new CheckBox();
			this.chkVox = new CheckBox();
			this.chkToneHint = new CheckBox();
			this.chkLed = new CheckBox();
			this.lblChDispMode = new Label();
			this.chkPwdLock = new CheckBox();
			this.lblMenuLang = new Label();
			this.lblWorkMode = new Label();
			this.chkPower = new CheckBox();
			this.chkBootScreen = new CheckBox();
			this.chkEncrypt = new CheckBox();
			this.cmbChDispMode = new CustomCombo();
			this.lblKeyLockTime = new Label();
			this.chkTalkAround = new CheckBox();
			this.cmbMenuLang = new CustomCombo();
			this.cmbWorkMode = new CustomCombo();
			this.cmbKeyLockTime = new CustomCombo();
			this.cmbDwSwitch = new CustomCombo();
			this.cmbKeyTone = new CustomCombo();
			this.cmbBacklightTime = new CustomCombo();
			this.chkChDisp = new CheckBox();
			this.grpContact = new DoubleClickGroupBox();
			this.chkOneKeyDial = new CheckBox();
			this.chkRadioDetect = new CheckBox();
			this.chkRadioActive = new CheckBox();
			this.chkContactEdit = new CheckBox();
			this.chkRadioKill = new CheckBox();
			this.chkManualDial = new CheckBox();
			this.chkRemoteMonitor = new CheckBox();
			this.chkCallPrompt = new CheckBox();
			this.grpScan = new DoubleClickGroupBox();
			this.chkScanEditList = new CheckBox();
			this.chkScan = new CheckBox();
			this.grpBasic = new DoubleClickGroupBox();
			this.lblMenuHangTime = new Label();
			this.cmbMenuHangTime = new CustomCombo();
			this.chkInfo = new CheckBox();
			this.grpCall.SuspendLayout();
			this.grpSetting.SuspendLayout();
			this.grpContact.SuspendLayout();
			this.grpScan.SuspendLayout();
			this.grpBasic.SuspendLayout();
			base.SuspendLayout();
			this.lblBrightness.Location = new Point(799, 149);
			this.lblBrightness.Name = "lblBrightness";
			this.lblBrightness.Size = new Size(75, 24);
			this.lblBrightness.TabIndex = 18;
			this.lblBrightness.Text = "Brightness";
			this.lblBrightness.TextAlign = ContentAlignment.MiddleRight;
			this.lblBrightness.Visible = false;
			this.chkFm.AutoSize = true;
			this.chkFm.Location = new Point(816, 76);
			this.chkFm.Name = "chkFm";
			this.chkFm.Size = new Size(47, 20);
			this.chkFm.TabIndex = 16;
			this.chkFm.Text = "FM";
			this.chkFm.UseVisualStyleBackColor = true;
			this.chkFm.Visible = false;
			this.chkFm.CheckedChanged += this.chkFm_CheckedChanged;
			this.chkP1Key.AutoSize = true;
			this.chkP1Key.Location = new Point(816, 122);
			this.chkP1Key.Name = "chkP1Key";
			this.chkP1Key.Size = new Size(103, 20);
			this.chkP1Key.TabIndex = 17;
			this.chkP1Key.Text = "P1 Function";
			this.chkP1Key.UseVisualStyleBackColor = true;
			this.chkP1Key.Visible = false;
			this.chkGps.AutoSize = true;
			this.chkGps.Location = new Point(816, 100);
			this.chkGps.Name = "chkGps";
			this.chkGps.Size = new Size(53, 20);
			this.chkGps.TabIndex = 15;
			this.chkGps.Text = "Gps";
			this.chkGps.TextImageRelation = TextImageRelation.TextBeforeImage;
			this.chkGps.UseVisualStyleBackColor = true;
			this.chkGps.Visible = false;
			this.cmbBrightness.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbBrightness.FormattingEnabled = true;
			this.cmbBrightness.Location = new Point(878, 149);
			this.cmbBrightness.Name = "cmbBrightness";
			this.cmbBrightness.Size = new Size(84, 24);
			this.cmbBrightness.TabIndex = 19;
			this.cmbBrightness.Visible = false;
			this.grpCall.method_3(true);
			this.grpCall.Controls.Add(this.chkReceivedCall);
			this.grpCall.Controls.Add(this.chkDialedCall);
			this.grpCall.Controls.Add(this.chkMissedCall);
			this.grpCall.method_1(false);
			this.grpCall.Location = new Point(299, 368);
			this.grpCall.Name = "grpCall";
			this.grpCall.Size = new Size(486, 102);
			this.grpCall.TabIndex = 4;
			this.grpCall.TabStop = false;
			this.grpCall.Text = "Call Log";
			this.chkReceivedCall.AutoSize = true;
			this.chkReceivedCall.Location = new Point(40, 50);
			this.chkReceivedCall.Name = "chkReceivedCall";
			this.chkReceivedCall.Size = new Size(89, 20);
			this.chkReceivedCall.TabIndex = 1;
			this.chkReceivedCall.Text = "Answered";
			this.chkReceivedCall.UseVisualStyleBackColor = true;
			this.chkDialedCall.AutoSize = true;
			this.chkDialedCall.Location = new Point(40, 72);
			this.chkDialedCall.Name = "chkDialedCall";
			this.chkDialedCall.Size = new Size(126, 20);
			this.chkDialedCall.TabIndex = 2;
			this.chkDialedCall.Text = "Outgoing Radio";
			this.chkDialedCall.UseVisualStyleBackColor = true;
			this.chkMissedCall.AutoSize = true;
			this.chkMissedCall.Location = new Point(40, 28);
			this.chkMissedCall.Name = "chkMissedCall";
			this.chkMissedCall.Size = new Size(71, 20);
			this.chkMissedCall.TabIndex = 0;
			this.chkMissedCall.Text = "Missed";
			this.chkMissedCall.UseVisualStyleBackColor = true;
			this.grpSetting.method_3(true);
			this.grpSetting.Controls.Add(this.chkDoubleWait);
			this.grpSetting.Controls.Add(this.lblDwSwitch);
			this.grpSetting.Controls.Add(this.lblKeyTone);
			this.grpSetting.Controls.Add(this.lblBacklightTime);
			this.grpSetting.Controls.Add(this.chkSql);
			this.grpSetting.Controls.Add(this.chkBacklight);
			this.grpSetting.Controls.Add(this.chkKeyboardLock);
			this.grpSetting.Controls.Add(this.chkVox);
			this.grpSetting.Controls.Add(this.chkToneHint);
			this.grpSetting.Controls.Add(this.chkLed);
			this.grpSetting.Controls.Add(this.lblChDispMode);
			this.grpSetting.Controls.Add(this.chkPwdLock);
			this.grpSetting.Controls.Add(this.lblMenuLang);
			this.grpSetting.Controls.Add(this.lblWorkMode);
			this.grpSetting.Controls.Add(this.chkPower);
			this.grpSetting.Controls.Add(this.chkBootScreen);
			this.grpSetting.Controls.Add(this.chkEncrypt);
			this.grpSetting.Controls.Add(this.cmbChDispMode);
			this.grpSetting.Controls.Add(this.lblKeyLockTime);
			this.grpSetting.Controls.Add(this.chkTalkAround);
			this.grpSetting.Controls.Add(this.cmbMenuLang);
			this.grpSetting.Controls.Add(this.cmbWorkMode);
			this.grpSetting.Controls.Add(this.cmbKeyLockTime);
			this.grpSetting.Controls.Add(this.cmbDwSwitch);
			this.grpSetting.Controls.Add(this.cmbKeyTone);
			this.grpSetting.Controls.Add(this.cmbBacklightTime);
			this.grpSetting.Controls.Add(this.chkChDisp);
			this.grpSetting.method_1(false);
			this.grpSetting.Location = new Point(299, 27);
			this.grpSetting.Name = "grpSetting";
			this.grpSetting.Size = new Size(486, 328);
			this.grpSetting.TabIndex = 3;
			this.grpSetting.TabStop = false;
			this.grpSetting.Text = "Utilities";
			this.chkDoubleWait.AutoSize = true;
			this.chkDoubleWait.Location = new Point(40, 289);
			this.chkDoubleWait.Name = "chkDoubleWait";
			this.chkDoubleWait.Size = new Size(128, 20);
			this.chkDoubleWait.TabIndex = 0;
			this.chkDoubleWait.Text = "Double Standby";
			this.chkDoubleWait.UseVisualStyleBackColor = true;
			this.lblDwSwitch.Location = new Point(226, 290);
			this.lblDwSwitch.Name = "lblDwSwitch";
			this.lblDwSwitch.Size = new Size(109, 24);
			this.lblDwSwitch.TabIndex = 4;
			this.lblDwSwitch.Text = "Double Standby";
			this.lblDwSwitch.TextAlign = ContentAlignment.MiddleRight;
			this.lblKeyTone.Location = new Point(268, 49);
			this.lblKeyTone.Name = "lblKeyTone";
			this.lblKeyTone.Size = new Size(67, 24);
			this.lblKeyTone.TabIndex = 4;
			this.lblKeyTone.Text = "Key Tone";
			this.lblKeyTone.TextAlign = ContentAlignment.MiddleRight;
			this.lblBacklightTime.Location = new Point(217, 91);
			this.lblBacklightTime.Name = "lblBacklightTime";
			this.lblBacklightTime.Size = new Size(118, 24);
			this.lblBacklightTime.TabIndex = 4;
			this.lblBacklightTime.Text = "Backlight Time [s]";
			this.lblBacklightTime.TextAlign = ContentAlignment.MiddleRight;
			this.chkSql.AutoSize = true;
			this.chkSql.Location = new Point(40, 181);
			this.chkSql.Name = "chkSql";
			this.chkSql.Size = new Size(78, 20);
			this.chkSql.TabIndex = 11;
			this.chkSql.Text = "Squelch";
			this.chkSql.UseVisualStyleBackColor = true;
			this.chkBacklight.AutoSize = true;
			this.chkBacklight.Location = new Point(40, 93);
			this.chkBacklight.Name = "chkBacklight";
			this.chkBacklight.Size = new Size(84, 20);
			this.chkBacklight.TabIndex = 3;
			this.chkBacklight.Text = "Backlight";
			this.chkBacklight.UseVisualStyleBackColor = true;
			this.chkKeyboardLock.AutoSize = true;
			this.chkKeyboardLock.Location = new Point(40, 137);
			this.chkKeyboardLock.Name = "chkKeyboardLock";
			this.chkKeyboardLock.Size = new Size(109, 20);
			this.chkKeyboardLock.TabIndex = 7;
			this.chkKeyboardLock.Text = "Keypad Lock";
			this.chkKeyboardLock.UseVisualStyleBackColor = true;
			this.chkVox.AutoSize = true;
			this.chkVox.Location = new Point(40, 223);
			this.chkVox.Name = "chkVox";
			this.chkVox.Size = new Size(49, 20);
			this.chkVox.TabIndex = 13;
			this.chkVox.Text = "Vox";
			this.chkVox.UseVisualStyleBackColor = true;
			this.chkToneHint.AutoSize = true;
			this.chkToneHint.Location = new Point(40, 49);
			this.chkToneHint.Name = "chkToneHint";
			this.chkToneHint.Size = new Size(105, 20);
			this.chkToneHint.TabIndex = 1;
			this.chkToneHint.Text = "Tones/Alerts";
			this.chkToneHint.UseVisualStyleBackColor = true;
			this.chkLed.AutoSize = true;
			this.chkLed.Location = new Point(40, 159);
			this.chkLed.Name = "chkLed";
			this.chkLed.Size = new Size(112, 20);
			this.chkLed.TabIndex = 10;
			this.chkLed.Text = "LED Indicator";
			this.chkLed.UseVisualStyleBackColor = true;
			this.lblChDispMode.Location = new Point(185, 265);
			this.lblChDispMode.Name = "lblChDispMode";
			this.lblChDispMode.Size = new Size(150, 24);
			this.lblChDispMode.TabIndex = 18;
			this.lblChDispMode.Text = "Channel Display Mode";
			this.lblChDispMode.TextAlign = ContentAlignment.MiddleRight;
			this.chkPwdLock.AutoSize = true;
			this.chkPwdLock.Location = new Point(40, 245);
			this.chkPwdLock.Name = "chkPwdLock";
			this.chkPwdLock.Size = new Size(150, 20);
			this.chkPwdLock.TabIndex = 14;
			this.chkPwdLock.Text = "Password And Lock";
			this.chkPwdLock.UseVisualStyleBackColor = true;
			this.lblMenuLang.Location = new Point(224, 214);
			this.lblMenuLang.Name = "lblMenuLang";
			this.lblMenuLang.Size = new Size(111, 24);
			this.lblMenuLang.TabIndex = 15;
			this.lblMenuLang.Text = "Menu Language";
			this.lblMenuLang.TextAlign = ContentAlignment.MiddleRight;
			this.lblMenuLang.Visible = false;
			this.lblWorkMode.Location = new Point(255, 240);
			this.lblWorkMode.Name = "lblWorkMode";
			this.lblWorkMode.Size = new Size(80, 24);
			this.lblWorkMode.TabIndex = 15;
			this.lblWorkMode.Text = "Work Mode";
			this.lblWorkMode.TextAlign = ContentAlignment.MiddleRight;
			this.lblWorkMode.Visible = false;
			this.chkPower.AutoSize = true;
			this.chkPower.Location = new Point(40, 71);
			this.chkPower.Name = "chkPower";
			this.chkPower.Size = new Size(66, 20);
			this.chkPower.TabIndex = 2;
			this.chkPower.Text = "Power";
			this.chkPower.UseVisualStyleBackColor = true;
			this.chkBootScreen.AutoSize = true;
			this.chkBootScreen.Location = new Point(40, 115);
			this.chkBootScreen.Name = "chkBootScreen";
			this.chkBootScreen.Size = new Size(104, 20);
			this.chkBootScreen.TabIndex = 6;
			this.chkBootScreen.Text = "Intro Screen";
			this.chkBootScreen.UseVisualStyleBackColor = true;
			this.chkEncrypt.AutoSize = true;
			this.chkEncrypt.Location = new Point(40, 201);
			this.chkEncrypt.Name = "chkEncrypt";
			this.chkEncrypt.Size = new Size(73, 20);
			this.chkEncrypt.TabIndex = 12;
			this.chkEncrypt.Text = "Privacy";
			this.chkEncrypt.UseVisualStyleBackColor = true;
			this.cmbChDispMode.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbChDispMode.FormattingEnabled = true;
			this.cmbChDispMode.Location = new Point(346, 265);
			this.cmbChDispMode.Name = "cmbChDispMode";
			this.cmbChDispMode.Size = new Size(124, 24);
			this.cmbChDispMode.TabIndex = 19;
			this.lblKeyLockTime.Location = new Point(192, 135);
			this.lblKeyLockTime.Name = "lblKeyLockTime";
			this.lblKeyLockTime.Size = new Size(143, 24);
			this.lblKeyLockTime.TabIndex = 8;
			this.lblKeyLockTime.Text = "Keypad Lock Time [s]";
			this.lblKeyLockTime.TextAlign = ContentAlignment.MiddleRight;
			this.chkTalkAround.AutoSize = true;
			this.chkTalkAround.Location = new Point(40, 27);
			this.chkTalkAround.Name = "chkTalkAround";
			this.chkTalkAround.Size = new Size(97, 20);
			this.chkTalkAround.TabIndex = 0;
			this.chkTalkAround.Text = "Talkaround";
			this.chkTalkAround.TextImageRelation = TextImageRelation.TextBeforeImage;
			this.chkTalkAround.UseVisualStyleBackColor = true;
			this.cmbMenuLang.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbMenuLang.FormattingEnabled = true;
			this.cmbMenuLang.Location = new Point(346, 214);
			this.cmbMenuLang.Name = "cmbMenuLang";
			this.cmbMenuLang.Size = new Size(124, 24);
			this.cmbMenuLang.TabIndex = 16;
			this.cmbMenuLang.Visible = false;
			this.cmbWorkMode.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbWorkMode.FormattingEnabled = true;
			this.cmbWorkMode.Location = new Point(346, 240);
			this.cmbWorkMode.Name = "cmbWorkMode";
			this.cmbWorkMode.Size = new Size(124, 24);
			this.cmbWorkMode.TabIndex = 16;
			this.cmbWorkMode.Visible = false;
			this.cmbKeyLockTime.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbKeyLockTime.FormattingEnabled = true;
			this.cmbKeyLockTime.Location = new Point(346, 135);
			this.cmbKeyLockTime.Name = "cmbKeyLockTime";
			this.cmbKeyLockTime.Size = new Size(124, 24);
			this.cmbKeyLockTime.TabIndex = 9;
			this.cmbDwSwitch.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbDwSwitch.FormattingEnabled = true;
			this.cmbDwSwitch.Location = new Point(346, 290);
			this.cmbDwSwitch.Name = "cmbDwSwitch";
			this.cmbDwSwitch.Size = new Size(124, 24);
			this.cmbDwSwitch.TabIndex = 5;
			this.cmbKeyTone.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbKeyTone.FormattingEnabled = true;
			this.cmbKeyTone.Location = new Point(346, 49);
			this.cmbKeyTone.Name = "cmbKeyTone";
			this.cmbKeyTone.Size = new Size(124, 24);
			this.cmbKeyTone.TabIndex = 5;
			this.cmbBacklightTime.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbBacklightTime.FormattingEnabled = true;
			this.cmbBacklightTime.Location = new Point(346, 91);
			this.cmbBacklightTime.Name = "cmbBacklightTime";
			this.cmbBacklightTime.Size = new Size(124, 24);
			this.cmbBacklightTime.TabIndex = 5;
			this.chkChDisp.AutoSize = true;
			this.chkChDisp.Location = new Point(40, 267);
			this.chkChDisp.Name = "chkChDisp";
			this.chkChDisp.Size = new Size(130, 20);
			this.chkChDisp.TabIndex = 17;
			this.chkChDisp.Text = "Channel Display";
			this.chkChDisp.UseVisualStyleBackColor = true;
			this.grpContact.method_3(true);
			this.grpContact.Controls.Add(this.chkOneKeyDial);
			this.grpContact.Controls.Add(this.chkRadioDetect);
			this.grpContact.Controls.Add(this.chkRadioActive);
			this.grpContact.Controls.Add(this.chkContactEdit);
			this.grpContact.Controls.Add(this.chkRadioKill);
			this.grpContact.Controls.Add(this.chkManualDial);
			this.grpContact.Controls.Add(this.chkRemoteMonitor);
			this.grpContact.Controls.Add(this.chkCallPrompt);
			this.grpContact.method_1(false);
			this.grpContact.Location = new Point(20, 226);
			this.grpContact.Name = "grpContact";
			this.grpContact.Size = new Size(264, 245);
			this.grpContact.TabIndex = 2;
			this.grpContact.TabStop = false;
			this.grpContact.Text = "Contact";
			this.chkOneKeyDial.AutoSize = true;
			this.chkOneKeyDial.Location = new Point(30, 184);
			this.chkOneKeyDial.Name = "chkOneKeyDial";
			this.chkOneKeyDial.Size = new Size(110, 20);
			this.chkOneKeyDial.TabIndex = 7;
			this.chkOneKeyDial.Text = "One Key Dial";
			this.chkOneKeyDial.UseVisualStyleBackColor = true;
			this.chkRadioDetect.AutoSize = true;
			this.chkRadioDetect.Location = new Point(30, 96);
			this.chkRadioDetect.Name = "chkRadioDetect";
			this.chkRadioDetect.Size = new Size(108, 20);
			this.chkRadioDetect.TabIndex = 3;
			this.chkRadioDetect.Text = "Radio Check";
			this.chkRadioDetect.UseVisualStyleBackColor = true;
			this.chkRadioActive.AutoSize = true;
			this.chkRadioActive.Location = new Point(30, 140);
			this.chkRadioActive.Name = "chkRadioActive";
			this.chkRadioActive.Size = new Size(112, 20);
			this.chkRadioActive.TabIndex = 5;
			this.chkRadioActive.Text = "Radio Enable";
			this.chkRadioActive.UseVisualStyleBackColor = true;
			this.chkContactEdit.AutoSize = true;
			this.chkContactEdit.Location = new Point(30, 52);
			this.chkContactEdit.Name = "chkContactEdit";
			this.chkContactEdit.Size = new Size(51, 20);
			this.chkContactEdit.TabIndex = 1;
			this.chkContactEdit.Text = "Edit";
			this.chkContactEdit.UseVisualStyleBackColor = true;
			this.chkRadioKill.AutoSize = true;
			this.chkRadioKill.Location = new Point(30, 162);
			this.chkRadioKill.Name = "chkRadioKill";
			this.chkRadioKill.Size = new Size(115, 20);
			this.chkRadioKill.TabIndex = 6;
			this.chkRadioKill.Text = "Radio Disable";
			this.chkRadioKill.UseVisualStyleBackColor = true;
			this.chkManualDial.AutoSize = true;
			this.chkManualDial.Location = new Point(30, 74);
			this.chkManualDial.Name = "chkManualDial";
			this.chkManualDial.Size = new Size(101, 20);
			this.chkManualDial.TabIndex = 2;
			this.chkManualDial.Text = "Manual Dial";
			this.chkManualDial.UseVisualStyleBackColor = true;
			this.chkRemoteMonitor.AutoSize = true;
			this.chkRemoteMonitor.Location = new Point(30, 118);
			this.chkRemoteMonitor.Name = "chkRemoteMonitor";
			this.chkRemoteMonitor.Size = new Size(127, 20);
			this.chkRemoteMonitor.TabIndex = 4;
			this.chkRemoteMonitor.Text = "Remote Monitor";
			this.chkRemoteMonitor.UseVisualStyleBackColor = true;
			this.chkCallPrompt.AutoSize = true;
			this.chkCallPrompt.Location = new Point(30, 30);
			this.chkCallPrompt.Name = "chkCallPrompt";
			this.chkCallPrompt.Size = new Size(83, 20);
			this.chkCallPrompt.TabIndex = 0;
			this.chkCallPrompt.Text = "Call Alert";
			this.chkCallPrompt.UseVisualStyleBackColor = true;
			this.grpScan.method_3(true);
			this.grpScan.Controls.Add(this.chkScanEditList);
			this.grpScan.Controls.Add(this.chkScan);
			this.grpScan.method_1(false);
			this.grpScan.Location = new Point(20, 122);
			this.grpScan.Name = "grpScan";
			this.grpScan.Size = new Size(264, 87);
			this.grpScan.TabIndex = 1;
			this.grpScan.TabStop = false;
			this.grpScan.Text = "Scan";
			this.chkScanEditList.AutoSize = true;
			this.chkScanEditList.Location = new Point(30, 53);
			this.chkScanEditList.Name = "chkScanEditList";
			this.chkScanEditList.Size = new Size(77, 20);
			this.chkScanEditList.TabIndex = 1;
			this.chkScanEditList.Text = "Edit List";
			this.chkScanEditList.UseVisualStyleBackColor = true;
			this.chkScan.AutoSize = true;
			this.chkScan.Location = new Point(30, 27);
			this.chkScan.Name = "chkScan";
			this.chkScan.Size = new Size(59, 20);
			this.chkScan.TabIndex = 0;
			this.chkScan.Text = "Scan";
			this.chkScan.UseVisualStyleBackColor = true;
			this.grpBasic.method_3(true);
			this.grpBasic.Controls.Add(this.lblMenuHangTime);
			this.grpBasic.Controls.Add(this.cmbMenuHangTime);
			this.grpBasic.Controls.Add(this.chkInfo);
			this.grpBasic.method_1(false);
			this.grpBasic.Location = new Point(20, 27);
			this.grpBasic.Name = "grpBasic";
			this.grpBasic.Size = new Size(264, 83);
			this.grpBasic.TabIndex = 0;
			this.grpBasic.TabStop = false;
			this.grpBasic.Text = "Basic";
			this.lblMenuHangTime.Location = new Point(19, 24);
			this.lblMenuHangTime.Name = "lblMenuHangTime";
			this.lblMenuHangTime.Size = new Size(133, 24);
			this.lblMenuHangTime.TabIndex = 0;
			this.lblMenuHangTime.Text = "Menu Hang Time [s]";
			this.lblMenuHangTime.TextAlign = ContentAlignment.MiddleRight;
			this.cmbMenuHangTime.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbMenuHangTime.FormattingEnabled = true;
			this.cmbMenuHangTime.Location = new Point(156, 24);
			this.cmbMenuHangTime.Name = "cmbMenuHangTime";
			this.cmbMenuHangTime.Size = new Size(84, 24);
			this.cmbMenuHangTime.TabIndex = 1;
			this.chkInfo.AutoSize = true;
			this.chkInfo.Location = new Point(30, 51);
			this.chkInfo.Name = "chkInfo";
			this.chkInfo.Size = new Size(97, 20);
			this.chkInfo.TabIndex = 2;
			this.chkInfo.Text = "Information";
			this.chkInfo.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(7f, 16f);
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(978, 508);
			base.Controls.Add(this.lblBrightness);
			base.Controls.Add(this.cmbBrightness);
			base.Controls.Add(this.chkFm);
			base.Controls.Add(this.chkP1Key);
			base.Controls.Add(this.chkGps);
			base.Controls.Add(this.grpCall);
			base.Controls.Add(this.grpSetting);
			base.Controls.Add(this.grpContact);
			base.Controls.Add(this.grpScan);
			base.Controls.Add(this.grpBasic);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.Name = "MenuForm";
			this.Text = "Menu";
			base.Load += this.MenuForm_Load;
			base.FormClosing += this.MenuForm_FormClosing;
			this.grpCall.ResumeLayout(false);
			this.grpCall.PerformLayout();
			this.grpSetting.ResumeLayout(false);
			this.grpSetting.PerformLayout();
			this.grpContact.ResumeLayout(false);
			this.grpContact.PerformLayout();
			this.grpScan.ResumeLayout(false);
			this.grpScan.PerformLayout();
			this.grpBasic.ResumeLayout(false);
			this.grpBasic.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		static MenuForm()
		{
			
			MenuForm.SZ_KEY_LOCK_TIME = new string[4]
			{
				"Manual",
				"5",
				"10",
				"15"
			};
			MenuForm.SZ_BACKLIGHT_TIME = new string[4]
			{
				"Always Open",
				"Always Close",
				"5",
				"10"
			};
			MenuForm.SZ_WORK_MODE = new string[3]
			{
				"VFO",
				"MR",
				"CH"
			};
			MenuForm.SZ_CH_DISP_MODE = new string[3]
			{
				"Number",
				"Name",
				"Frequency"
			};
			MenuForm.SZ_KEY_TONE = new string[2]
			{
				"Off",
				"On"
			};
			MenuForm.SZ_DOUBLE_WAIT = new string[3]
			{
				"Off",
				"Double Double",
				"Double Single"
			};
			MenuForm.SZ_MENU_LANG = new string[2]
			{
				"English",
				"Chinese"
			};
			MenuForm.data = new MenuSet();
		}
	}
}
