using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class NormalScanForm : DockContent, IDisp
	{
		private enum ScanCh
		{
			None,
			Selected
		}

		private enum TxDesignatedCh
		{
			LastActiveCh,
			SelectCh
		}

		private enum PlTypeE
		{
			NonPriorityCh,
			Disable,
			PriorityCh,
			PriorityChAndNonPriorityCh
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct NormalScanOne : IVerify<NormalScanOne>
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
			private byte[] name;

			private byte flag1;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			private ushort[] chList;

			private ushort priorityCh1;

			private ushort priorityCh2;

			private ushort txDesignatedCh;

			private byte signalingHold;

			private byte prioritySample;

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

			public byte Flag1
			{
				get
				{
					return this.flag1;
				}
				set
				{
					this.flag1 = value;
				}
			}

			public bool Talkback
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

			public int PlType
			{
				get
				{
					return (this.flag1 & 0x60) >> 5;
				}
				set
				{
					value = (value << 5 & 0x60);
					this.flag1 &= 159;
					this.flag1 |= (byte)value;
				}
			}

			public bool ChMark
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

			public ushort[] ChList
			{
				get
				{
					return this.chList;
				}
				set
				{
					this.chList.Fill((ushort)0);
					Array.Copy(value, 0, this.chList, 0, Math.Min(32, value.Length));
				}
			}

			public int PriorityCh1
			{
				get
				{
					if (this.priorityCh1 <= 1024)
					{
						return this.priorityCh1;
					}
					return 0;
				}
				set
				{
					if (value <= 1024)
					{
						this.priorityCh1 = (ushort)value;
					}
				}
			}

			public int PriorityCh2
			{
				get
				{
					if (this.priorityCh2 <= 1024)
					{
						return this.priorityCh2;
					}
					return 0;
				}
				set
				{
					if (value <= 1024)
					{
						this.priorityCh2 = (ushort)value;
					}
				}
			}

			public int TxDesignatedCh
			{
				get
				{
					return this.txDesignatedCh;
				}
				set
				{
					this.txDesignatedCh = (ushort)value;
				}
			}

			public decimal SignalingHold
			{
				get
				{
					if (this.signalingHold >= 2 && this.signalingHold <= 255)
					{
						return this.signalingHold * 25;
					}
					return 500m;
				}
				set
				{
					byte b = (byte)(value / 25m);
					if (b >= 2 && b <= 255)
					{
						this.signalingHold = b;
					}
					else
					{
						this.signalingHold = 20;
					}
				}
			}

			public decimal PrioritySample
			{
				get
				{
					if (this.prioritySample >= 3 && this.prioritySample <= 31)
					{
						return this.prioritySample * 250;
					}
					return 2000m;
				}
				set
				{
					int num = (int)(value / 250m);
					if (num >= 3 && num <= 31)
					{
						this.prioritySample = (byte)num;
					}
					else
					{
						this.prioritySample = 8;
					}
				}
			}

			public NormalScanOne(int index)
			{
				
				this = default(NormalScanOne);
				this.name = new byte[15];
				this.chList = new ushort[32];
				this.chList[0] = 1;
			}

			public void Default()
			{
				this.chList.Fill((ushort)0);
				this.chList[0] = 1;
				this.priorityCh1 = 0;
				this.priorityCh2 = 0;
				this.txDesignatedCh = 1;
				this.SignalingHold = NormalScanForm.DefaultScan.SignalingHold;
				this.PrioritySample = NormalScanForm.DefaultScan.PrioritySample;
				this.Flag1 = NormalScanForm.DefaultScan.Flag1;
			}

			public NormalScanOne Clone()
			{
				return Settings.smethod_65(this);
			}

			public void Verify(NormalScanOne def)
			{
				int num = 0;
				int num2 = 0;
				List<ushort> list = new List<ushort>(this.chList);
				list[0] = 1;
				for (num = 1; num < list.Count; num++)
				{
					if (list[num] == 1)
					{
						list.RemoveAt(num);
						list.Add(0);
						num--;
					}
					else if (list[num] != 0)
					{
						num2 = list[num] - 2;
						if (!ChannelForm.data.DataIsValid(num2))
						{
							list.RemoveAt(num);
							list.Add(0);
							num--;
						}
					}
				}
				this.chList = list.ToArray();
				if (this.priorityCh1 == 0)
				{
					this.priorityCh2 = 0;
				}
				else if (this.priorityCh1 == this.priorityCh2)
				{
					this.priorityCh2 = 0;
				}
				else if (!list.Contains(this.priorityCh1))
				{
					this.priorityCh1 = 0;
				}
				if (this.priorityCh2 != 0 && !list.Contains(this.priorityCh1))
				{
					this.priorityCh2 = 0;
				}
				if (!Enum.IsDefined(typeof(TxDesignatedCh), (int)this.txDesignatedCh))
				{
					num2 = this.txDesignatedCh - 2;
					if (!ChannelForm.data.DataIsValid(num2))
					{
						this.txDesignatedCh = 0;
					}
				}
				Settings.smethod_11(ref this.signalingHold, (byte)2, (byte)255, def.signalingHold);
				Settings.smethod_11(ref this.prioritySample, (byte)3, (byte)31, def.prioritySample);
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class NormalScan : IData
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			private byte[] scanListIndex;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			private NormalScanOne[] scanList;

			public NormalScanOne this[int index]
			{
				get
				{
					if (index >= 64)
					{
						throw new ArgumentOutOfRangeException();
					}
					return this.scanList[index];
				}
				set
				{
					if (index >= this.Count)
					{
						throw new ArgumentOutOfRangeException();
					}
					this.scanList[index] = value;
				}
			}

			public int Count
			{
				get
				{
					return 64;
				}
			}

			public string Format
			{
				get
				{
					return "ScanList{0}";
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

			public NormalScan()
			{
				int num = 0;
				this.scanListIndex = new byte[64];
				this.scanListIndex.Fill((byte)0);
				this.scanList = new NormalScanOne[64];
				for (num = 0; num < this.scanList.Length; num++)
				{
					this.scanList[num] = new NormalScanOne(num);
				}
			}

			public void ClearByData(int chIndex)
			{
				int num = 0;
				int num2 = 0;
				for (num = 0; num < this.Count; num++)
				{
					if (this.DataIsValid(num))
					{
						num2 = Array.IndexOf(this.scanList[num].ChList, (byte)(chIndex + 2));
						if (num2 >= 0)
						{
							this.scanList[num].ChList.RemoveItemFromArray(num2);
							if (!this.scanList[num].ChList.Contains((byte)this.scanList[num].PriorityCh1))
							{
								this.scanList[num].PriorityCh1 = 0;
								this.scanList[num].PriorityCh2 = 0;
							}
						}
						if (this.scanList[num].TxDesignatedCh == chIndex + 2)
						{
							this.scanList[num].TxDesignatedCh = 0;
						}
					}
				}
			}

			public bool DataIsValid(int index)
			{
				if (index < 64)
				{
					if (this.scanListIndex[index] != 0)
					{
						return this.scanListIndex[index] <= 2;
					}
					return false;
				}
				return false;
			}

			public int GetMinIndex()
			{
				int num = 0;
				num = 0;
				while (true)
				{
					if (num < this.Count)
					{
						if (this.scanListIndex[num] == 0)
						{
							break;
						}
						if (this.scanListIndex[num] > 2)
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

			public void SetIndex(int index, int value)
			{
				if (index < 64)
				{
					this.scanListIndex[index] = (byte)value;
				}
			}

			public void ClearIndex(int index)
			{
				this.SetIndex(index, 0);
				ChannelForm.data.ClearByScan(index);
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
				if (index < 64)
				{
					this.scanList[index].Name = text;
				}
			}

			public string GetName(int index)
			{
				return this.scanList[index].Name;
			}

			public void Default(int index)
			{
				this.scanList[index].Default();
			}

			public void Paste(int from, int to)
			{
				Array.Copy(this.scanList[from].ChList, this.scanList[to].ChList, this.scanList[to].ChList.Length);
				this.scanList[to].PriorityCh1 = this.scanList[from].PriorityCh1;
				this.scanList[to].PriorityCh2 = this.scanList[from].PriorityCh2;
				this.scanList[to].TxDesignatedCh = this.scanList[from].TxDesignatedCh;
				this.scanList[to].SignalingHold = this.scanList[from].SignalingHold;
				this.scanList[to].PrioritySample = this.scanList[from].PrioritySample;
				this.scanList[to].Flag1 = this.scanList[from].Flag1;
			}

			public void Verify()
			{
				int num = 0;
				for (num = 0; num < this.Count; num++)
				{
					if (this.DataIsValid(num))
					{
						this.scanList[num].Verify(NormalScanForm.DefaultScan);
					}
					else
					{
						this.scanListIndex[num] = 0;
					}
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class NormalScanEx
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			private ushort[] priorityCh1;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			private ushort[] priorityCh2;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			private ushort[] specifyCh;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2048)]
			private ushort[] scanChList;

			public ushort[] PriorityCh1
			{
				get
				{
					return this.priorityCh1;
				}
			}

			public ushort[] PriorityCh2
			{
				get
				{
					return this.priorityCh2;
				}
			}

			public ushort[] SpecifyCh
			{
				get
				{
					return this.specifyCh;
				}
			}

			public ushort[] ScanChList
			{
				get
				{
					return this.scanChList;
				}
			}

			public NormalScanEx()
			{
				
				//base._002Ector();
				this.priorityCh1 = new ushort[64];
				this.priorityCh2 = new ushort[64];
				this.specifyCh = new ushort[64];
				this.scanChList = new ushort[2048];
				int num = 0;
				for (num = 0; num < 64; num++)
				{
					this.scanChList[31] = 1;
				}
			}

			public void ClearByData(int chIndex)
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				for (num = 0; num < 64; num++)
				{
					if (this.specifyCh[num] == chIndex + 2)
					{
						this.specifyCh[num] = 0;
					}
					if (this.priorityCh1[num] == chIndex + 2)
					{
						this.priorityCh1[num] = 0;
						this.priorityCh2[num] = 0;
					}
					else if (this.priorityCh2[num] == chIndex + 2)
					{
						this.priorityCh2[num] = 0;
					}
					for (num2 = 0; num2 < 32; num2++)
					{
						num3 = num * 32;
						ushort[] array = new ushort[32];
						Array.Copy(this.scanChList, num3, array, 0, array.Length);
						num4 = Array.IndexOf(array, Convert.ToUInt16(chIndex + 2));
						if (num4 >= 0)
						{
							array.smethod_3(num4, (ushort)0);
							Array.Copy(array, 0, this.scanChList, num3, array.Length);
						}
					}
				}
			}

			public void Default(int index)
			{
				this.priorityCh1[index] = 0;
				this.priorityCh2[index] = 0;
				this.specifyCh[index] = 1;
				this.scanChList.FillFromPositionWithLength((ushort)0, index * 32, 32);
				this.scanChList[index * 32] = 1;
			}

			public void Verify()
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				for (num = 0; num < 32; num++)
				{
					ushort[] array = new ushort[32];
					Array.Copy(this.scanChList, num * 32, array, 0, array.Length);
					List<ushort> list = new List<ushort>(array);
					list[0] = 1;
					for (num2 = 1; num2 < list.Count; num2++)
					{
						if (list[num2] == 1)
						{
							list.RemoveAt(num2);
							list.Add(0);
							num2--;
						}
						else if (list[num2] != 0)
						{
							num3 = list[num2] - 2;
							if (!ChannelForm.data.DataIsValid(num3))
							{
								list.RemoveAt(num2);
								list.Add(0);
								num2--;
							}
						}
					}
					array = list.ToArray();
					Array.Copy(array, 0, this.scanChList, num * 32, array.Length);
					if (this.priorityCh1[num] == 0)
					{
						this.priorityCh2[num] = 0;
					}
					else if (this.priorityCh1[num] == this.priorityCh2[num])
					{
						this.priorityCh2[num] = 0;
					}
					else if (!list.Contains(this.priorityCh1[num]))
					{
						this.priorityCh1[num] = 0;
					}
					if (this.priorityCh2[num] != 0 && !list.Contains(this.priorityCh1[num]))
					{
						this.priorityCh2[num] = 0;
					}
				}
				for (num = 0; num < this.specifyCh.Length; num++)
				{
					if (!Enum.IsDefined(typeof(TxDesignatedCh), (int)this.specifyCh[num]))
					{
						num3 = this.specifyCh[num] - 2;
						if (!ChannelForm.data.DataIsValid(num3))
						{
							this.specifyCh[num] = 0;
						}
					}
				}
			}
		}

		public const int CNT_SCAN_LIST = 64;

		public const int LEN_SCAN_LIST_NAME = 15;

		private const int MAX_CH_PER_SCAN_LIST = 31;

		private const int CNT_CH_PER_SCAN_LIST = 32;

		public const string SZ_PRIORITY_CH_NAME = "PriorityCh";

		public const string SZ_TX_DESIGNATED_CH_NAME = "TxDesignatedCh";

		public const string SZ_PL_TYPE_NAME = "PlType";

		private const int MIN_SIGNALING_HOLD = 2;

		private const int MAX_SIGNALING_HOLD = 255;

		private const int INC_SIGNALING_HOLD = 1;

		private const int SCL_SIGNALING_HOLD = 25;

		private const int LEN_SIGNALING_HOLD = 4;

		private const int MIN_PRIORITY_SAMPLE = 3;

		private const int MAX_PRIORITY_SAMPLE = 31;

		private const int INC_PRIORITY_SAMPLE = 1;

		private const int SCL_PRIORITY_SAMPLE = 250;

		private const int LEN_PRIORITY_SAMPLE = 4;

		private static readonly string[] SZ_PRIORITY_CH;

		private static readonly string[] SZ_TX_DESIGNATED_CH;

		private static readonly string[] SZ_PL_TYPE;

		public static NormalScanOne DefaultScan;

		public static NormalScan data;

		//private IContainer components;

		private CustomNumericUpDown nudSignalingHold;

		private Button btnDel;

		private Button btnAdd;

		private ListBox lstSelected;

		private ListBox lstUnselected;

		private CustomCombo cmbTxDesignatedCh;

		private Label lblSignalingHold;

		private Label lblTxDesignatedCh;

		private CheckBox chkChMark;

		private CheckBox chkTalkback;

		private Label lblPrioritySample;

		private CustomNumericUpDown nudPrioritySample;

		private Label label_0;

		private CustomCombo cmbPlType;

		private SGTextBox txtName;

		private Label lblName;

		private CustomCombo cmbPriorityCh1;

		private CustomCombo cmbPriorityCh2;

		private Label lblPriorityCh1;

		private Label lblPriorityCh2;

		private GroupBox grpUnselected;

		private GroupBox grpSelected;

        private GroupBox grpListParam;

        private Button btnDown;

		private Button btnUp;

		private CustomPanel panel1;

		public TreeNode Node
		{
			get;
			set;
		}

		public void SaveData()
		{
			int num = 0;
			int num2 = 0;
			try
			{
				num2 = Convert.ToInt32(base.Tag);
				if (num2 == -1)
				{
					return;
				}
				if (this.txtName.Focused)
				{
					this.txtName_Leave(this.txtName, null);
				}
				NormalScanOne value = new NormalScanOne(num2);
				value.Name = this.txtName.Text;
				value.ChMark = this.chkChMark.Checked;
				value.Talkback = this.chkTalkback.Checked;
				value.PlType = this.cmbPlType.SelectedIndex;
				value.TxDesignatedCh = this.cmbTxDesignatedCh.method_3();
				value.PriorityCh1 = this.cmbPriorityCh1.method_3();
				value.PriorityCh2 = this.cmbPriorityCh2.method_3();
				value.SignalingHold = this.nudSignalingHold.Value;
				value.PrioritySample = this.nudPrioritySample.Value;
				for (num = 0; num < value.ChList.Length; num++)
				{
					value.ChList[num] = 0;
				}
				num = 0;
				foreach (SelectedItemUtils item in this.lstSelected.Items)
				{
					value.ChList[num] = (ushort)item.Value;     //G4EML Correct type of scan list item from byte to ushort
					num++;
				}
				NormalScanForm.data[num2] = value;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void DispData()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			string text = "";
			try
			{
				num2 = Convert.ToInt32(base.Tag);
				if (num2 == -1)
				{
					this.Close();
					return;
				}
				this.method_0();
				this.txtName.Text = NormalScanForm.data[num2].Name;
				this.chkChMark.Checked = NormalScanForm.data[num2].ChMark;
				this.chkTalkback.Checked = NormalScanForm.data[num2].Talkback;
				this.cmbTxDesignatedCh.method_2(NormalScanForm.data[num2].TxDesignatedCh);
				this.cmbPlType.SelectedIndex = NormalScanForm.data[num2].PlType;
				this.nudSignalingHold.Value = NormalScanForm.data[num2].SignalingHold;
				this.nudPrioritySample.Value = NormalScanForm.data[num2].PrioritySample;
				this.lstSelected.Items.Clear();
				for (num = 0; num < NormalScanForm.data[num2].ChList.Length; num++)
				{
					num3 = NormalScanForm.data[num2].ChList[num];
					if (num3 == 1)
					{
						this.lstSelected.Items.Add(new SelectedItemUtils(num, num3, Settings.SZ_SELECTED));
					}
					else if (num3 > 1 && num3 <= 1025)
					{
						num4 = num3 - 2;
						if (ChannelForm.data.DataIsValid(num4))
						{
							text = ChannelForm.data.GetName(num4);
							this.lstSelected.Items.Add(new SelectedItemUtils(num, num3, text));
						}
					}
				}
				if (this.lstSelected.Items.Count > 0)
				{
					this.lstSelected.SelectedIndex = 0;
				}
				int[] array = new int[32];
				for (num = 0; num < array.Length; num++)
				{
					array[num] = NormalScanForm.data[num2].ChList[num];
				}
				this.lstUnselected.Items.Clear();
				for (num = 0; num < ChannelForm.CurCntCh; num++)
				{
					if (ChannelForm.data.DataIsValid(num) && !array.Contains(num + 2))
					{
						num3 = num + 1;
						this.lstUnselected.Items.Add(new SelectedItemUtils(-1, num3 + 1, ChannelForm.data.GetName(num)));
					}
				}
				if (this.lstUnselected.Items.Count > 0)
				{
					this.lstUnselected.SelectedIndex = 0;
				}
				this.method_1();
				this.cmbPriorityCh1.method_2(NormalScanForm.data[num2].PriorityCh1);
				this.cmbPriorityCh2.method_2(NormalScanForm.data[num2].PriorityCh2);
				this.method_4();
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
			this.chkChMark.Enabled &= flag;
			this.lblSignalingHold.Enabled &= flag;
			this.nudSignalingHold.Enabled &= flag;
			this.lblPrioritySample.Enabled &= flag;
			this.nudPrioritySample.Enabled &= flag;
		}

		public void RefreshName()
		{
			int index = Convert.ToInt32(base.Tag);
			this.txtName.Text = NormalScanForm.data[index].Name;
		}

		public NormalScanForm()
		{
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void method_0()
		{
			Settings.smethod_45(this.cmbTxDesignatedCh, NormalScanForm.SZ_TX_DESIGNATED_CH, ChannelForm.data);
		}

		private void method_1()
		{
			Settings.smethod_46(this.cmbPriorityCh1, NormalScanForm.SZ_PRIORITY_CH, this.lstSelected);
			Settings.smethod_46(this.cmbPriorityCh2, NormalScanForm.SZ_PRIORITY_CH, this.lstSelected);
		}

		private void method_2()
		{
			int num = 0;
			string priCh3 = this.cmbPriorityCh1.Text;
			string priCh2 = this.cmbPriorityCh2.Text;
			this.method_1();
			if (this.cmbPriorityCh1.FindStringExact(priCh3) < 0)
			{
				num = Array.FindIndex(this.cmbPriorityCh1.Items.Cast<NameValuePair>().ToArray(), delegate(NameValuePair x)
				{
					if (x.Text == priCh3)
					{
						return true;
					}
					if (x.Text.Contains(':') && priCh3.Contains(':'))
					{
						return x.Text.Split(':')[1] == priCh3.Split(':')[1];
					}
					return false;
				});
				if (num < 0)
				{
					this.cmbPriorityCh1.SelectedIndex = 0;
					this.cmbPriorityCh2.SelectedIndex = 0;
				}
				else
				{
					this.cmbPriorityCh1.SelectedIndex = num;
					num = Array.FindIndex(this.cmbPriorityCh2.Items.Cast<NameValuePair>().ToArray(), delegate(NameValuePair x)
					{
						if (x.Text == priCh2)
						{
							return true;
						}
						if (x.Text.Contains(':') && priCh2.Contains(':'))
						{
							return x.Text.Split(':')[1] == priCh2.Split(':')[1];
						}
						return false;
					});
					if (num < 0)
					{
						this.cmbPriorityCh2.SelectedIndex = 0;
					}
					else
					{
						this.cmbPriorityCh2.SelectedIndex = num;
					}
				}
			}
			else
			{
				this.cmbPriorityCh1.Text = priCh3;
				if (this.cmbPriorityCh2.FindStringExact(priCh2) < 0)
				{
					num = Array.FindIndex(this.cmbPriorityCh2.Items.Cast<NameValuePair>().ToArray(), delegate(NameValuePair x)
					{
						if (x.Text == priCh2)
						{
							return true;
						}
						if (x.Text.Contains(':') && priCh2.Contains(':'))
						{
							return x.Text.Split(':')[1] == priCh2.Split(':')[1];
						}
						return false;
					});
					if (num < 0)
					{
						this.cmbPriorityCh2.SelectedIndex = 0;
					}
					else
					{
						this.cmbPriorityCh2.SelectedIndex = num;
					}
				}
				else
				{
					this.cmbPriorityCh2.Text = priCh2;
				}
			}
		}

		private void method_3()
		{
			this.txtName.MaxByteLength = 15;
			this.txtName.KeyPress += new KeyPressEventHandler(Settings.smethod_54);
			Settings.smethod_37(this.cmbPlType, NormalScanForm.SZ_PL_TYPE);
			Settings.smethod_36(this.nudSignalingHold, new Class13(2, 255, 1, 25m, 4));
			Settings.smethod_36(this.nudPrioritySample, new Class13(3, 31, 1, 250m, 4));
		}

		public static void RefreshCommonLang()
		{
			string name = typeof(NormalScanForm).Name;
			Settings.smethod_78("PriorityCh", NormalScanForm.SZ_PRIORITY_CH, name);
			Settings.smethod_78("TxDesignatedCh", NormalScanForm.SZ_TX_DESIGNATED_CH, name);
			Settings.smethod_78("PlType", NormalScanForm.SZ_PL_TYPE, name);
		}

		private void NormalScanForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);
			this.method_3();
			this.DispData();
		}

		private void NormalScanForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
		}

		private void method_4()
		{
			this.btnAdd.Enabled = (this.lstUnselected.Items.Count > 0 && this.lstSelected.Items.Count < 32);
			this.btnDel.Enabled = (this.lstSelected.Items.Count > 0 && !this.lstSelected.GetSelected(0));
			int count = this.lstSelected.Items.Count;
			int count2 = this.lstSelected.SelectedIndices.Count;
			this.btnUp.Enabled = (this.lstSelected.SelectedItems.Count > 0 && this.lstSelected.Items.Count > 0 && this.lstSelected.SelectedIndices[count2 - 1] != count2 && !this.lstSelected.GetSelected(0));
			this.btnDown.Enabled = (this.lstSelected.Items.Count > 0 && this.lstSelected.SelectedItems.Count > 0 && this.lstSelected.SelectedIndices[0] != count - count2 && !this.lstSelected.GetSelected(0));
		}

		private bool method_5()
		{
			if (this.lstSelected.GetSelected(0))
			{
				MessageBox.Show("Unable to operate selected");
				this.lstSelected.SetSelected(0, false);
				return false;
			}
			return true;
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			int num = 0;
			int count = this.lstUnselected.SelectedIndices.Count;
			int num2 = this.lstUnselected.SelectedIndices[count - 1];
			this.lstSelected.SelectedItems.Clear();
			while (this.lstUnselected.SelectedItems.Count > 0 && this.lstSelected.Items.Count < 32)
			{
				SelectedItemUtils @class = (SelectedItemUtils)this.lstUnselected.SelectedItems[0];
				@class.method_1(this.lstSelected.Items.Count);
				num = this.lstSelected.Items.Add(@class);
				this.lstSelected.SetSelected(num, true);
				this.lstUnselected.Items.RemoveAt(this.lstUnselected.SelectedIndices[0]);
			}
			if (this.lstUnselected.SelectedItems.Count == 0)
			{
				int num3 = num2 - count + 1;
				if (num3 >= this.lstUnselected.Items.Count)
				{
					num3 = this.lstUnselected.Items.Count - 1;
				}
				this.lstUnselected.SelectedIndex = num3;
			}
			this.method_7();
			this.method_2();
			this.method_4();
			if (!this.btnAdd.Enabled)
			{
				this.lstSelected.Focus();
			}
		}

		private void btnDel_Click(object sender, EventArgs e)
		{
			if (this.method_5())
			{
				int num = 0;
				int count = this.lstSelected.SelectedIndices.Count;
				int num2 = this.lstSelected.SelectedIndices[count - 1];
				this.lstUnselected.SelectedItems.Clear();
				while (this.lstSelected.SelectedItems.Count > 0)
				{
					SelectedItemUtils @class = (SelectedItemUtils)this.lstSelected.SelectedItems[0];
					num = this.method_6(@class);
					@class.method_1(-1);
					this.lstUnselected.Items.Insert(num, @class);
					this.lstUnselected.SetSelected(num, true);
					this.lstSelected.Items.RemoveAt(this.lstSelected.SelectedIndices[0]);
				}
				int num3 = num2 - count + 1;
				if (num3 >= this.lstSelected.Items.Count)
				{
					num3 = this.lstSelected.Items.Count - 1;
				}
				this.lstSelected.SelectedIndex = num3;
				this.method_7();
				this.method_2();
				this.method_4();
			}
		}

		private int method_6(SelectedItemUtils class14_0)
		{
			int num = 0;
			num = 0;
			while (true)
			{
				if (num < this.lstUnselected.Items.Count)
				{
					SelectedItemUtils @class = (SelectedItemUtils)this.lstUnselected.Items[num];
					if (class14_0.Value < @class.Value)
					{
						break;
					}
					num++;
					continue;
				}
				return num;
			}
			return num;
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			if (this.method_5())
			{
				int num = 0;
				int num2 = 0;
				int count = this.lstSelected.SelectedIndices.Count;
				int num3 = this.lstSelected.SelectedIndices[count - 1];
				for (num = 0; num < count; num++)
				{
					num2 = this.lstSelected.SelectedIndices[num];
					if (num + 1 != num2)
					{
						object value = this.lstSelected.Items[num2];
						this.lstSelected.Items[num2] = this.lstSelected.Items[num2 - 1];
						this.lstSelected.Items[num2 - 1] = value;
						this.lstSelected.SetSelected(num2, false);
						this.lstSelected.SetSelected(num2 - 1, true);
					}
				}
				this.method_7();
				this.method_2();
			}
		}

		private void btnDown_Click(object sender, EventArgs e)
		{
			if (this.method_5())
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int count = this.lstSelected.Items.Count;
				int count2 = this.lstSelected.SelectedIndices.Count;
				int num4 = this.lstSelected.SelectedIndices[count2 - 1];
				num = count2 - 1;
				while (num >= 0)
				{
					num3 = this.lstSelected.SelectedIndices[num];
					if (count - 1 - num2 != num3)
					{
						object value = this.lstSelected.Items[num3];
						this.lstSelected.Items[num3] = this.lstSelected.Items[num3 + 1];
						this.lstSelected.Items[num3 + 1] = value;
						this.lstSelected.SetSelected(num3, false);
						this.lstSelected.SetSelected(num3 + 1, true);
					}
					num--;
					num2++;
				}
				this.method_7();
				this.method_2();
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

		private void cmbPriorityCh1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbPriorityCh1.SelectedIndex != 0 && this.cmbPriorityCh1.SelectedIndex != this.cmbPriorityCh2.SelectedIndex)
			{
				this.cmbPriorityCh2.Enabled = true;
			}
			else
			{
				this.cmbPriorityCh2.SelectedIndex = 0;
				this.cmbPriorityCh2.Enabled = false;
			}
		}

		private void cmbPriorityCh2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbPriorityCh2.SelectedIndex != 0 && this.cmbPriorityCh1.SelectedIndex == this.cmbPriorityCh2.SelectedIndex)
			{
				this.cmbPriorityCh2.SelectedIndex = 0;
			}
		}

		private void method_7()
		{
			int num = 0;
			bool flag = false;
			this.lstSelected.BeginUpdate();
			for (num = 0; num < this.lstSelected.Items.Count; num++)
			{
				SelectedItemUtils @class = (SelectedItemUtils)this.lstSelected.Items[num];
				if (@class.method_0() != num)
				{
					@class.method_1(num);
					flag = this.lstSelected.GetSelected(num);
					this.lstSelected.Items[num] = @class;
					if (flag)
					{
						this.lstSelected.SetSelected(num, true);
					}
				}
			}
			this.lstSelected.EndUpdate();
		}

		private void lstSelected_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.method_4();
		}

		private void lstSelected_DoubleClick(object sender, EventArgs e)
		{
			if (this.lstSelected.SelectedItem != null)
			{
				SelectedItemUtils @class = this.lstSelected.SelectedItem as SelectedItemUtils;
				MainForm mainForm = base.MdiParent as MainForm;
				if (mainForm != null)
				{
					mainForm.DispChildForm(typeof(ChannelForm), @class.Value - 2);
				}
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
			this.panel1 = new CustomPanel();
			this.grpListParam = new System.Windows.Forms.GroupBox();
			this.chkTalkback = new System.Windows.Forms.CheckBox();
			this.chkChMark = new System.Windows.Forms.CheckBox();
			this.cmbPriorityCh2 = new CustomCombo();
			this.lblTxDesignatedCh = new System.Windows.Forms.Label();
			this.cmbPriorityCh1 = new CustomCombo();
			this.lblPriorityCh1 = new System.Windows.Forms.Label();
			this.lblSignalingHold = new System.Windows.Forms.Label();
			this.label_0 = new System.Windows.Forms.Label();
			this.nudPrioritySample = new CustomNumericUpDown();
			this.lblPriorityCh2 = new System.Windows.Forms.Label();
			this.nudSignalingHold = new CustomNumericUpDown();
			this.cmbTxDesignatedCh = new CustomCombo();
			this.cmbPlType = new CustomCombo();
			this.lblPrioritySample = new System.Windows.Forms.Label();
			this.btnDown = new System.Windows.Forms.Button();
			this.btnUp = new System.Windows.Forms.Button();
			this.txtName = new DMR.SGTextBox();
			this.grpSelected = new System.Windows.Forms.GroupBox();
			this.lstSelected = new System.Windows.Forms.ListBox();
			this.grpUnselected = new System.Windows.Forms.GroupBox();
			this.lstUnselected = new System.Windows.Forms.ListBox();
			this.lblName = new System.Windows.Forms.Label();
			this.btnDel = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.grpListParam.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudPrioritySample)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSignalingHold)).BeginInit();
			this.grpSelected.SuspendLayout();
			this.grpUnselected.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.AutoSize = true;
			this.panel1.Controls.Add(this.grpListParam);
			this.panel1.Controls.Add(this.btnDown);
			this.panel1.Controls.Add(this.btnUp);
			this.panel1.Controls.Add(this.txtName);
			this.panel1.Controls.Add(this.grpSelected);
			this.panel1.Controls.Add(this.grpUnselected);
			this.panel1.Controls.Add(this.lblName);
			this.panel1.Controls.Add(this.btnDel);
			this.panel1.Controls.Add(this.btnAdd);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(760, 744);
			this.panel1.TabIndex = 0;
			// 
			// grpListParam
			// 
			this.grpListParam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.grpListParam.Controls.Add(this.chkTalkback);
			this.grpListParam.Controls.Add(this.chkChMark);
			this.grpListParam.Controls.Add(this.cmbPriorityCh2);
			this.grpListParam.Controls.Add(this.lblTxDesignatedCh);
			this.grpListParam.Controls.Add(this.cmbPriorityCh1);
			this.grpListParam.Controls.Add(this.lblPriorityCh1);
			this.grpListParam.Controls.Add(this.lblSignalingHold);
			this.grpListParam.Controls.Add(this.label_0);
			this.grpListParam.Controls.Add(this.nudPrioritySample);
			this.grpListParam.Controls.Add(this.lblPriorityCh2);
			this.grpListParam.Controls.Add(this.nudSignalingHold);
			this.grpListParam.Controls.Add(this.cmbTxDesignatedCh);
			this.grpListParam.Controls.Add(this.cmbPlType);
			this.grpListParam.Controls.Add(this.lblPrioritySample);
			this.grpListParam.Location = new System.Drawing.Point(113, 458);
			this.grpListParam.Name = "grpListParam";
			this.grpListParam.Size = new System.Drawing.Size(517, 269);
			this.grpListParam.TabIndex = 24;
			this.grpListParam.TabStop = false;
			this.grpListParam.Text = "Scanparameter";
			// 
			// chkTalkback
			// 
			this.chkTalkback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkTalkback.AutoSize = true;
			this.chkTalkback.Location = new System.Drawing.Point(251, 13);
			this.chkTalkback.Name = "chkTalkback";
			this.chkTalkback.Size = new System.Drawing.Size(82, 20);
			this.chkTalkback.TabIndex = 6;
			this.chkTalkback.Text = "Talkback";
			this.chkTalkback.UseVisualStyleBackColor = true;
			// 
			// chkChMark
			// 
			this.chkChMark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkChMark.AutoSize = true;
			this.chkChMark.Location = new System.Drawing.Point(251, 43);
			this.chkChMark.Name = "chkChMark";
			this.chkChMark.Size = new System.Drawing.Size(128, 20);
			this.chkChMark.TabIndex = 7;
			this.chkChMark.Text = "Channel Marker";
			this.chkChMark.UseVisualStyleBackColor = true;
			// 
			// cmbPriorityCh2
			// 
			this.cmbPriorityCh2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmbPriorityCh2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPriorityCh2.FormattingEnabled = true;
			this.cmbPriorityCh2.Location = new System.Drawing.Point(251, 224);
			this.cmbPriorityCh2.Name = "cmbPriorityCh2";
			this.cmbPriorityCh2.Size = new System.Drawing.Size(166, 24);
			this.cmbPriorityCh2.TabIndex = 19;
			this.cmbPriorityCh2.SelectedIndexChanged += new System.EventHandler(this.cmbPriorityCh2_SelectedIndexChanged);
			// 
			// lblTxDesignatedCh
			// 
			this.lblTxDesignatedCh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblTxDesignatedCh.Location = new System.Drawing.Point(73, 73);
			this.lblTxDesignatedCh.Name = "lblTxDesignatedCh";
			this.lblTxDesignatedCh.Size = new System.Drawing.Size(167, 24);
			this.lblTxDesignatedCh.TabIndex = 8;
			this.lblTxDesignatedCh.Text = "Tx Designated Channel";
			this.lblTxDesignatedCh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cmbPriorityCh1
			// 
			this.cmbPriorityCh1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmbPriorityCh1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPriorityCh1.FormattingEnabled = true;
			this.cmbPriorityCh1.Location = new System.Drawing.Point(251, 194);
			this.cmbPriorityCh1.Name = "cmbPriorityCh1";
			this.cmbPriorityCh1.Size = new System.Drawing.Size(166, 24);
			this.cmbPriorityCh1.TabIndex = 17;
			this.cmbPriorityCh1.SelectedIndexChanged += new System.EventHandler(this.cmbPriorityCh1_SelectedIndexChanged);
			// 
			// lblPriorityCh1
			// 
			this.lblPriorityCh1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPriorityCh1.Location = new System.Drawing.Point(73, 193);
			this.lblPriorityCh1.Name = "lblPriorityCh1";
			this.lblPriorityCh1.Size = new System.Drawing.Size(167, 24);
			this.lblPriorityCh1.TabIndex = 16;
			this.lblPriorityCh1.Text = "Priority Channel 1";
			this.lblPriorityCh1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblSignalingHold
			// 
			this.lblSignalingHold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblSignalingHold.Location = new System.Drawing.Point(73, 133);
			this.lblSignalingHold.Name = "lblSignalingHold";
			this.lblSignalingHold.Size = new System.Drawing.Size(167, 24);
			this.lblSignalingHold.TabIndex = 12;
			this.lblSignalingHold.Text = "Signaling Hold Time [ms]";
			this.lblSignalingHold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label_0
			// 
			this.label_0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label_0.Location = new System.Drawing.Point(73, 103);
			this.label_0.Name = "label_0";
			this.label_0.Size = new System.Drawing.Size(167, 24);
			this.label_0.TabIndex = 10;
			this.label_0.Text = "PL Type";
			this.label_0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// nudPrioritySample
			// 
			this.nudPrioritySample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.nudPrioritySample.Increment = new decimal(new int[] {
            250,
            0,
            0,
            0});
			this.nudPrioritySample.Location = new System.Drawing.Point(251, 164);
			this.nudPrioritySample.Maximum = new decimal(new int[] {
            7750,
            0,
            0,
            0});
			this.nudPrioritySample.Minimum = new decimal(new int[] {
            750,
            0,
            0,
            0});
			this.nudPrioritySample.Name = "nudPrioritySample";
			this.nudPrioritySample.Size = new System.Drawing.Size(166, 23);
			this.nudPrioritySample.TabIndex = 15;
			this.nudPrioritySample.Value = new decimal(new int[] {
            750,
            0,
            0,
            0});
			// 
			// lblPriorityCh2
			// 
			this.lblPriorityCh2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPriorityCh2.Location = new System.Drawing.Point(73, 223);
			this.lblPriorityCh2.Name = "lblPriorityCh2";
			this.lblPriorityCh2.Size = new System.Drawing.Size(167, 24);
			this.lblPriorityCh2.TabIndex = 18;
			this.lblPriorityCh2.Text = "Priority Channel 2";
			this.lblPriorityCh2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// nudSignalingHold
			// 
			this.nudSignalingHold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.nudSignalingHold.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
			this.nudSignalingHold.Location = new System.Drawing.Point(251, 134);
			this.nudSignalingHold.Maximum = new decimal(new int[] {
            6375,
            0,
            0,
            0});
			this.nudSignalingHold.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.nudSignalingHold.Name = "nudSignalingHold";
			this.nudSignalingHold.Size = new System.Drawing.Size(166, 23);
			this.nudSignalingHold.TabIndex = 13;
			this.nudSignalingHold.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// cmbTxDesignatedCh
			// 
			this.cmbTxDesignatedCh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmbTxDesignatedCh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTxDesignatedCh.FormattingEnabled = true;
			this.cmbTxDesignatedCh.Location = new System.Drawing.Point(251, 74);
			this.cmbTxDesignatedCh.Name = "cmbTxDesignatedCh";
			this.cmbTxDesignatedCh.Size = new System.Drawing.Size(166, 24);
			this.cmbTxDesignatedCh.TabIndex = 9;
			// 
			// cmbPlType
			// 
			this.cmbPlType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmbPlType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPlType.FormattingEnabled = true;
			this.cmbPlType.Items.AddRange(new object[] {
            "Disabled",
            "Priority channel",
            "Non-priority channel",
            "Priority channel and non-priority channel"});
			this.cmbPlType.Location = new System.Drawing.Point(251, 104);
			this.cmbPlType.Name = "cmbPlType";
			this.cmbPlType.Size = new System.Drawing.Size(166, 24);
			this.cmbPlType.TabIndex = 11;
			// 
			// lblPrioritySample
			// 
			this.lblPrioritySample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPrioritySample.Location = new System.Drawing.Point(73, 163);
			this.lblPrioritySample.Name = "lblPrioritySample";
			this.lblPrioritySample.Size = new System.Drawing.Size(167, 24);
			this.lblPrioritySample.TabIndex = 14;
			this.lblPrioritySample.Text = "Priority Sample Time [ms]";
			this.lblPrioritySample.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnDown
			// 
			this.btnDown.Location = new System.Drawing.Point(665, 223);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(75, 23);
			this.btnDown.TabIndex = 23;
			this.btnDown.Text = "Down";
			this.btnDown.UseVisualStyleBackColor = true;
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			// 
			// btnUp
			// 
			this.btnUp.Location = new System.Drawing.Point(665, 171);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(75, 23);
			this.btnUp.TabIndex = 22;
			this.btnUp.Text = "Up";
			this.btnUp.UseVisualStyleBackColor = true;
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// txtName
			// 
			this.txtName.InputString = null;
			this.txtName.Location = new System.Drawing.Point(322, 22);
			this.txtName.MaxByteLength = 0;
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(146, 23);
			this.txtName.TabIndex = 1;
			this.txtName.Leave += new System.EventHandler(this.txtName_Leave);
			// 
			// grpSelected
			// 
			this.grpSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.grpSelected.Controls.Add(this.lstSelected);
			this.grpSelected.Location = new System.Drawing.Point(425, 59);
			this.grpSelected.Name = "grpSelected";
			this.grpSelected.Size = new System.Drawing.Size(205, 412);
			this.grpSelected.TabIndex = 21;
			this.grpSelected.TabStop = false;
			this.grpSelected.Text = "Member";
			// 
			// lstSelected
			// 
			this.lstSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lstSelected.FormattingEnabled = true;
			this.lstSelected.ItemHeight = 16;
			this.lstSelected.Location = new System.Drawing.Point(30, 31);
			this.lstSelected.Name = "lstSelected";
			this.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstSelected.Size = new System.Drawing.Size(150, 324);
			this.lstSelected.TabIndex = 5;
			this.lstSelected.SelectedIndexChanged += new System.EventHandler(this.lstSelected_SelectedIndexChanged);
			this.lstSelected.DoubleClick += new System.EventHandler(this.lstSelected_DoubleClick);
			// 
			// grpUnselected
			// 
			this.grpUnselected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.grpUnselected.Controls.Add(this.lstUnselected);
			this.grpUnselected.Location = new System.Drawing.Point(113, 59);
			this.grpUnselected.Name = "grpUnselected";
			this.grpUnselected.Size = new System.Drawing.Size(205, 412);
			this.grpUnselected.TabIndex = 2;
			this.grpUnselected.TabStop = false;
			this.grpUnselected.Text = "Available";
			// 
			// lstUnselected
			// 
			this.lstUnselected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lstUnselected.FormattingEnabled = true;
			this.lstUnselected.ItemHeight = 16;
			this.lstUnselected.Location = new System.Drawing.Point(32, 31);
			this.lstUnselected.Name = "lstUnselected";
			this.lstUnselected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstUnselected.Size = new System.Drawing.Size(150, 324);
			this.lstUnselected.TabIndex = 0;
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(258, 22);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(60, 24);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name";
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnDel
			// 
			this.btnDel.Location = new System.Drawing.Point(333, 223);
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new System.Drawing.Size(75, 23);
			this.btnDel.TabIndex = 4;
			this.btnDel.Text = "Delete";
			this.btnDel.UseVisualStyleBackColor = true;
			this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(333, 171);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 3;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// NormalScanForm
			// 
			this.ClientSize = new System.Drawing.Size(760, 744);
			this.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("Arial", 10F);
			this.Name = "NormalScanForm";
			this.Text = "Normal Scan";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NormalScanForm_FormClosing);
			this.Load += new System.EventHandler(this.NormalScanForm_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.grpListParam.ResumeLayout(false);
			this.grpListParam.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudPrioritySample)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSignalingHold)).EndInit();
			this.grpSelected.ResumeLayout(false);
			this.grpUnselected.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		static NormalScanForm()
		{
			
			NormalScanForm.SZ_PRIORITY_CH = new string[2]
			{
				"None",
				"Selected"
			};
			NormalScanForm.SZ_TX_DESIGNATED_CH = new string[2]
			{
				"Last Active Channel",
				"Selected"
			};
			NormalScanForm.SZ_PL_TYPE = new string[4]
			{
				"Non-Priority Channel",
				"Disable",
				"Priority Channel",
				"Priority and Non-Priority Channel"
			};
			NormalScanForm.data = new NormalScan();
		}
	}
}
