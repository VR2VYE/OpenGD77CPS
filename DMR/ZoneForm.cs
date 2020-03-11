using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;   

namespace DMR
{
	public class ZoneForm : DockContent, IDisp
	{
		const int ZONE_NAME_LENGTH = 16;
		const int ZONES_IN_USE_DATA_LENGTH = 32;
#if OpenGD77
		public const int NUM_CHANNELS_PER_ZONE = 80;
		public const int NUM_ZONES = 68;
#elif CP_VER_3_1_X
		public const int NUM_CHANNELS_PER_ZONE	= 16;
		public const int NUM_ZONES				= 250;
#endif
		const int UNKNOWN_VAR_OF_32 = NUM_CHANNELS_PER_ZONE + ZONE_NAME_LENGTH;

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct ZoneOne
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = ZONE_NAME_LENGTH)]
			private byte[] name;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_CHANNELS_PER_ZONE)]
			private ushort[] chList;


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

			public ushort[] ChList
			{
				get
				{
					return this.chList;
				}
				set
				{
					this.chList.Fill((ushort)65535);
					Array.Copy(value, 0, this.chList, 0, value.Length);
				}
			}

			public ZoneOne(int index)
			{
				
				this = default(ZoneOne);
				this.name = new byte[ZONE_NAME_LENGTH];
				this.chList = new ushort[NUM_CHANNELS_PER_ZONE];
			}

			// Roger Clark. Added copy constructor
			public ZoneOne(ZoneOne zone)
			{

				this = default(ZoneOne);
				this.name = new byte[ZONE_NAME_LENGTH];
				this.chList = new ushort[NUM_CHANNELS_PER_ZONE];
				Array.Copy(zone.name, this.name, ZONE_NAME_LENGTH);
				Array.Copy(zone.chList, this.chList, NUM_CHANNELS_PER_ZONE);
			}

			public byte[] ToEerom()
			{
				int num = 0;
				byte[] array = new byte[UNKNOWN_VAR_OF_32];// Was 32 for the offical CPS, but I don't know why its 32 really
				array.Fill((byte)0);
				Array.Copy(this.name, array, ZONE_NAME_LENGTH);
				ushort[] array2 = this.chList;
				for (int i = 0; i < array2.Length; i++)
				{
					array[ZONE_NAME_LENGTH + num] = (byte)this.chList[num];
				}
				return array;
			}

			public void Verify()
			{
				List<ushort> list = new List<ushort>(this.chList);
				List<ushort> list2 = list.FindAll(ZoneOne.smethod_0);
				while (list2.Count < this.chList.Length)
				{
					list2.Add(0);
				}
				this.chList = list2.ToArray();
			}

			public bool AddChannelToZone(ushort channel)
			{
				channel++;// 
				// check if channel is already in the Zone
				if (Array.FindIndex(chList, item => item == channel) != -1)
				{
					return true;// Channel is already in the zone, so we'll return as if we've added it.
				}
	
				// channel is not in this Zone.
				int foundIndex = Array.FindIndex(chList, item => (item == 0 || item == (ushort)65535));
				if (foundIndex == -1)
				{
					return false;
				}

				chList[foundIndex] = channel;// put the channel into the zone
				return true;
			}			

			[CompilerGenerated]
			private static bool smethod_0(ushort ushort_0)
			{
				if (ushort_0 != 0)
				{
					return ChannelForm.data.DataIsValid(ushort_0 - 1);
				}
				return false;
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class Zone : IData
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = ZONES_IN_USE_DATA_LENGTH)]
			private byte[] zoneIndex;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_ZONES)]
			private ZoneOne[] zoneList;

			public byte[] ZoneIndex
			{
				get
				{
					return this.zoneIndex;
				}
			}

			public ZoneOne[] ZoneList
			{
				get
				{
					return this.zoneList;
				}
			}

			public ZoneOne this[int index]
			{
				get
				{
					if (index >= this.Count)
					{
						throw new ArgumentOutOfRangeException();
					}
					return this.zoneList[index];
				}
				set
				{
					if (index >= this.Count)
					{
						throw new ArgumentOutOfRangeException();
					}
					this.zoneList[index] = value;
				}
			}

			public int Count
			{
				get
				{
					return NUM_ZONES;
				}
			}

			public int ValidCount
			{
				get
				{
					int num = 0;
					int num2 = 0;
					BitArray bitArray = new BitArray(this.zoneIndex);
					for (num = 0; num < bitArray.Count; num++)
					{
						if (bitArray[num])
						{
							num2++;
						}
					}
					return num2;
				}
			}

			public string Format
			{
				get
				{
					return "Zone{0}";
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

			public int FstZoneFstCh
			{
				get
				{
					return this.zoneList[0].ChList[0];
				}
			}

			public Zone()
			{
				int num = 0;
				this.zoneIndex = new byte[ZONES_IN_USE_DATA_LENGTH];
				this.zoneList = new ZoneOne[NUM_ZONES];
				for (num = 0; num < this.zoneList.Length; num++)
				{
					this.zoneList[num] = new ZoneOne(num);
				}
			}

			public int GetDispIndex(int index)
			{
				int num = 0;
				int num2 = 0;
				BitArray bitArray = new BitArray(this.zoneIndex);
				for (num = 0; num <= index; num++)
				{
					if (bitArray[num])
					{
						num2++;
					}
				}
				return num2;
			}

			public int GetMinIndex()
			{
				int num = 0;
				BitArray bitArray = new BitArray(this.zoneIndex);
				num = 0;
				while (true)
				{
					if (num < this.Count)
					{
						if (!bitArray[num])
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

			public int GetMinIndexIncludeCh()
			{
				int num = 0;
				BitArray bitArray = new BitArray(this.zoneIndex);
				num = 0;
				while (true)
				{
					if (num < this.Count)
					{
						if (bitArray[num] && this.zoneList[num].ChList[0] != 0)
						{
							break;
						}
						num++;
						continue;
					}
					return this.GetMinIndex();
				}
				return num;
			}

			public int GetMinValidIndex()
			{
				int num = 0;
				num = 0;
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
					return 0;
				}
				return num;
			}

			public bool DataIsValid(int index)
			{
				if (index < NUM_ZONES)
				{
					BitArray bitArray = new BitArray(this.zoneIndex);
					return bitArray[index];
				}
				return false;
			}

			public bool ZoneChIsValid(int index)
			{
				if (index < NUM_ZONES)
				{
					BitArray bitArray = new BitArray(this.zoneIndex);
					if (bitArray[index] && this.zoneList[index].ChList[0] != 0)
					{
						return true;
					}
				}
				return false;
			}

			public void SetIndex(int index, int value)
			{
				int num = index / 8;
				int num2 = index % 8;
				if (Convert.ToBoolean(value))
				{
					this.zoneIndex[num] |= (byte)(1 << num2);
				}
				else
				{
					this.zoneIndex[num] &= (byte)(~(1 << num2));
				}
			}

			public void ClearIndex(int index)
			{
				int num = index / 8;
				int num2 = index % 8;
				this.zoneIndex[num] &= (byte)(~(1 << num2));
			}

			public void ClearByData(int chIndex)
			{
				int num = 0;
				int num2 = 0;
				ZoneForm.basicData.ClearByData(chIndex);
				for (num = 0; num < this.Count; num++)
				{
					if (this.DataIsValid(num))
					{
						num2 = Array.IndexOf(this.zoneList[num].ChList, (ushort)(chIndex + 1));
						if (num2 >= 0)
						{
							this.zoneList[num].ChList.RemoveItemFromArray(num2);
						}
					}
				}
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
				this.zoneList[index].Name = text;
			}

			public string GetName(int index)
			{
				return this.zoneList[index].Name;
			}

			public void Default(int index)
			{
				this.zoneList[index].ChList.Fill((ushort)0);
			}

			public void Paste(int from, int to)
			{
				Array.Copy(this.zoneList[from].ChList, 0, this.zoneList[to].ChList, 0, this.zoneList[to].ChList.Length);
			}

			public byte[] ToEerom(int index, int length)
			{
				int num = 0;
				byte[] array = new byte[UNKNOWN_VAR_OF_32 * length];// I'm not sure why this is 32 
				for (num = 0; num < length; num++)
				{
					byte[] array2 = this.ZoneList[index + num].ToEerom();
					Array.Copy(array2, 0, array, num * array2.Length, array2.Length);
				}
				return array;
			}

			public bool ZoneChIsValid(int _zoneIndex, int _zoneChIndex)
			{
				try
				{
					int ch = this.ZoneList[_zoneIndex].ChList[_zoneChIndex];
					return this.ChIsValid(ch);
				}
				catch
				{
					return false;
				}
			}

			public bool ChIsValid(int ch)
			{
				if (ch >= 1)
				{
					return ch <= ChannelForm.CurCntCh;
				}
				return false;
			}

			public int GetZoneChCnt(int _zoneIndex)
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				for (num = 0; num < this.ZoneList[_zoneIndex].ChList.Length; num++)
				{
					num2 = this.ZoneList[_zoneIndex].ChList[num];
					if (this.ChIsValid(num2))
					{
						num3++;
					}
				}
				return num3;
			}

			public void Verify()
			{
				int num = 0;
				for (num = 0; num < this.Count; num++)
				{
					if (this.DataIsValid(num))
					{
						this.zoneList[num].Verify();
					}
				}
			}
		}

		public static void SwapZones(int zoneId1, int zoneId2)
		{
		}

		public static void MoveZoneDown(int zoneIndex)
		{
			
			if (ZoneForm.data.DataIsValid(zoneIndex+1))
			{
				ZoneOne tmpZone = ZoneForm.data.ZoneList[zoneIndex + 1];
				ZoneForm.data.ZoneList[zoneIndex + 1] = ZoneForm.data.ZoneList[zoneIndex];
				ZoneForm.data.ZoneList[zoneIndex] = tmpZone;
			}
		}

		public static void MoveZoneUp(int zoneIndex)
		{
			if (zoneIndex > 0)
			{
				ZoneOne tmpZone = ZoneForm.data.ZoneList[zoneIndex-1];
				ZoneForm.data.ZoneList[zoneIndex-1] = ZoneForm.data.ZoneList[zoneIndex];
				ZoneForm.data.ZoneList[zoneIndex] = tmpZone;
			}
		}

		public static void copyZonesDownwards(int sourceZoneNum,int destZoneNum)
		{
			ZoneOne zn;
			bool srcIsValid;
			for (int i = sourceZoneNum; i < ZoneForm.data.ZoneList.Length; i++)
			{
				zn = ZoneForm.data.ZoneList[i];
				srcIsValid = ZoneForm.data.DataIsValid(i);
				ZoneForm.data.ZoneList[destZoneNum] =  new ZoneOne(zn);// Roger Clark. Had to reinstate using the copy constructor, because just assigning objects could cause all Zones to point to (use) the same zone object.
				if (srcIsValid)
				{
					ZoneForm.data.SetIndex(destZoneNum, 1);
				}
				else
				{
					ZoneForm.data.ClearIndex(destZoneNum);
				}
				destZoneNum++;
			}
		}

		// Roger Clark. Added function to shrink the zone list so that there are no empty zone array indexes
		// This is needed so that when adding a new zone it always goes at the end
		// and for the new MoveUp and MoveDown feature
		public static void CompactZones()
		{
			int nextFreeIndex = -1;
			//ZoneOne zn;
			for (int i = 0; i < ZoneForm.data.ZoneList.Length;i++)
			{
				//zn = ZoneForm.data.ZoneList[i];

				if (nextFreeIndex == -1 && !ZoneForm.data.DataIsValid(i))
				{
					nextFreeIndex = i;
				}
				else
				{
					// This zone has channels
					if (nextFreeIndex != -1)
					{
						// There is a free zone before this zone
						copyZonesDownwards(i, nextFreeIndex);
						nextFreeIndex = -1;// Space has been removed
					}
					
				}

			}
		}


		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class BasicZone
		{
			private ushort curZone;

			private ushort mainCh;

			private ushort subCh;

			private ushort subZone;

			public int CurZone
			{
				get
				{
					if (this.curZone < NUM_ZONES)
					{
						if (ZoneForm.data.ZoneChIsValid(this.curZone))
						{
							return this.curZone;
						}
						return 0;
					}
					return 0;
				}
				set
				{
					if (value < NUM_ZONES)
					{
						this.curZone = (ushort)value;
					}
				}
			}

			public int MainCh
			{
				get
				{
					if (this.mainCh < ChannelForm.CurCntCh)
					{
						return this.mainCh;
					}
					return 0;
				}
				set
				{
					if (value < ChannelForm.CurCntCh)
					{
						this.mainCh = (ushort)value;
					}
				}
			}

			public int SubCh
			{
				get
				{
					if (this.subCh < ChannelForm.CurCntCh)
					{
						return this.subCh;
					}
					return 0;
				}
				set
				{
					if (value < ChannelForm.CurCntCh)
					{
						this.subCh = (ushort)value;
					}
				}
			}

			public int SubZone
			{
				get
				{
					if (this.subZone < NUM_ZONES)
					{
						if (ZoneForm.data.ZoneChIsValid(this.subZone))
						{
							return this.subZone;
						}
						return 0;
					}
					return 0;
				}
				set
				{
					if (value < NUM_ZONES)
					{
						this.subZone = (ushort)value;
					}
				}
			}

			public BasicZone()
			{
				
				//base._002Ector();
			}

			public byte[] ToEerom()
			{
				return new byte[4]
				{
					(byte)this.CurZone,
					(byte)this.MainCh,
					(byte)this.SubCh,
					(byte)this.SubZone
				};
			}

			public void FromEerom(byte[] data)
			{
				if (data.Length >= 4)
				{
					this.CurZone = data[0];
					this.MainCh = data[1];
					this.SubCh = data[2];
					this.SubZone = data[3];
				}
			}

			public void ClearByData(int index)
			{
				int num = 0;
				int num2 = index + 1;
				num = Array.IndexOf(ZoneForm.data.ZoneList[this.CurZone].ChList, (ushort)num2);
				if (num >= 0)
				{
					int zoneChCnt = ZoneForm.data.GetZoneChCnt(this.CurZone);
					if (zoneChCnt == 1)
					{
						this.CurZone = ZoneForm.data.GetMinIndexIncludeCh();
						this.MainCh = 0;
					}
					else if (num == this.MainCh)
					{
						this.MainCh = 0;
					}
					else
					{
						this.MainCh = Math.Max(0, this.MainCh - 1);
					}
				}
				num = Array.IndexOf(ZoneForm.data.ZoneList[this.SubZone].ChList, (ushort)num2);
				if (num >= 0)
				{
					int zoneChCnt2 = ZoneForm.data.GetZoneChCnt(this.SubZone);
					if (zoneChCnt2 == 1)
					{
						this.SubZone = ZoneForm.data.GetMinIndexIncludeCh();
						this.SubCh = 0;
					}
					else if (num == this.SubCh)
					{
						this.SubCh = 0;
					}
					else
					{
						this.SubCh = Math.Max(0, this.SubCh - 1);
					}
				}
			}

			public void Verify()
			{
				if (ZoneForm.data.ZoneChIsValid(this.CurZone))
				{
					if (ZoneForm.data.ZoneList[this.CurZone].ChList[this.MainCh] == 0)
					{
						this.MainCh = 0;
					}
				}
				else
				{
					this.CurZone = ZoneForm.data.GetMinIndexIncludeCh();
					this.MainCh = 0;
				}
				if (ZoneForm.data.ZoneChIsValid(this.SubZone))
				{
					if (ZoneForm.data.ZoneList[this.SubZone].ChList[this.SubCh] == 0)
					{
						this.SubCh = 0;
					}
				}
				else
				{
					this.SubZone = ZoneForm.data.GetMinIndexIncludeCh();
					this.SubCh = 0;
				}
			}
		}
		/* These constants don't seem to be used
		public const int CNT_ZONE_GROUP = 8;

		public const int CNT_ZONE_PER_ZONE_GROUP = 32;

		public const int CNT_BASIC_ZONE = 2;

		public const int CNT_ZONE = 250;

		public const int SPACE_ZONE_INDEX = 32;

		public const int CNT_ZONE_LAST_ZONE_GROUP = 26;

		public const int LEN_ZONE_NAME = 16;

		public const int CNT_CH_PER_ZONE = 16;
		*/
		//private IContainer components;

		private Button btnDel;

		private Button btnAdd;

		private ListBox lstSelected;

		private ListBox lstUnselected;

		private Label lblName;

		private SGTextBox txtName;

		private GroupBox grpUnselected;

		private GroupBox grpSelected;

		private Button btnDown;

		private Button btnUp;

		private ToolStrip tsrZone;

		private ToolStripButton tsbtnFirst;

		private ToolStripButton tsbtnPrev;

		private ToolStripButton tsbtnNext;

		private ToolStripButton tsbtnLast;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripButton tsbtnAdd;

		private ToolStripButton tsbtnDel;

		private MenuStrip mnsZone;

		private ToolStripMenuItem tsmiCh;

		private ToolStripMenuItem tsmiFirst;

		private ToolStripMenuItem tsmiPrev;

		private ToolStripMenuItem tsmiNext;

		private ToolStripMenuItem tsmiLast;

		private ToolStripMenuItem tsmiAdd;

		private ToolStripMenuItem tsmiDel;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripLabel tslblInfo;

		private CustomPanel pnlZone;

		public static readonly int SPACE_ZONE;

		public static BasicZone basicData;

		public static Zone data;

		public static readonly int SPACE_BASIC_ZONE;

		public static int MainChIndex
		{
			get;
			set;
		}

		public static int SubChIndex
		{
			get;
			set;
		}

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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ZoneForm));
			this.pnlZone = new CustomPanel();
			this.tsrZone = new ToolStrip();
			this.tslblInfo = new ToolStripLabel();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.tsbtnFirst = new ToolStripButton();
			this.tsbtnPrev = new ToolStripButton();
			this.tsbtnNext = new ToolStripButton();
			this.tsbtnLast = new ToolStripButton();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.tsbtnAdd = new ToolStripButton();
			this.tsbtnDel = new ToolStripButton();
			this.mnsZone = new MenuStrip();
			this.tsmiCh = new ToolStripMenuItem();
			this.tsmiFirst = new ToolStripMenuItem();
			this.tsmiPrev = new ToolStripMenuItem();
			this.tsmiNext = new ToolStripMenuItem();
			this.tsmiLast = new ToolStripMenuItem();
			this.tsmiAdd = new ToolStripMenuItem();
			this.tsmiDel = new ToolStripMenuItem();
			this.btnDown = new Button();
			this.btnUp = new Button();
			this.txtName = new SGTextBox();
			this.grpSelected = new GroupBox();
			this.lstSelected = new ListBox();
			this.btnAdd = new Button();
			this.grpUnselected = new GroupBox();
			this.lstUnselected = new ListBox();
			this.btnDel = new Button();
			this.lblName = new Label();
			this.pnlZone.SuspendLayout();
			this.tsrZone.SuspendLayout();
			this.mnsZone.SuspendLayout();
			this.grpSelected.SuspendLayout();
			this.grpUnselected.SuspendLayout();
			base.SuspendLayout();
			this.pnlZone.AutoScroll = true;
			this.pnlZone.AutoSize = true;
			
			this.pnlZone.Controls.Add(this.tsrZone);
			this.pnlZone.Controls.Add(this.mnsZone);
			this.pnlZone.Controls.Add(this.btnDown);
			this.pnlZone.Controls.Add(this.btnUp);
			this.pnlZone.Controls.Add(this.txtName);
			this.pnlZone.Controls.Add(this.grpSelected);
			this.pnlZone.Controls.Add(this.btnAdd);
			this.pnlZone.Controls.Add(this.grpUnselected);
			this.pnlZone.Controls.Add(this.btnDel);
			this.pnlZone.Controls.Add(this.lblName);
			this.pnlZone.Dock = DockStyle.Fill;
			this.pnlZone.Location = new Point(0, 0);
			this.pnlZone.Name = "pnlZone";
			this.pnlZone.Size = new Size(794, 560);
			this.pnlZone.TabIndex = 8;
			this.tsrZone.Items.AddRange(new ToolStripItem[9]
			{
				this.tslblInfo,
				this.toolStripSeparator2,
				this.tsbtnFirst,
				this.tsbtnPrev,
				this.tsbtnNext,
				this.tsbtnLast,
				this.toolStripSeparator1,
				this.tsbtnAdd,
				this.tsbtnDel
			});
			this.tsrZone.Location = new Point(0, 0);
			this.tsrZone.Name = "tsrZone";
			this.tsrZone.Size = new Size(794, 25);
			this.tsrZone.TabIndex = 33;
			this.tsrZone.Text = "toolStrip1";
			this.tslblInfo.AutoSize = false;
			this.tslblInfo.Name = "tslblInfo";
			this.tslblInfo.Size = new Size(100, 52);
			this.tslblInfo.Text = " 0 / 0";
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new Size(6, 25);
			this.tsbtnFirst.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbtnFirst.Image = (Image)componentResourceManager.GetObject("tsbtnFirst.Image");
			this.tsbtnFirst.ImageTransparentColor = Color.Magenta;
			this.tsbtnFirst.Name = "tsbtnFirst";
			this.tsbtnFirst.Size = new Size(23, 22);
			this.tsbtnFirst.Text = "First";
			this.tsbtnFirst.Click += new EventHandler(this.tsmiFirst_Click);
			this.tsbtnPrev.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbtnPrev.Image = (Image)componentResourceManager.GetObject("tsbtnPrev.Image");
			this.tsbtnPrev.ImageTransparentColor = Color.Magenta;
			this.tsbtnPrev.Name = "tsbtnPrev";
			this.tsbtnPrev.Size = new Size(23, 22);
			this.tsbtnPrev.Text = "Previous";
			this.tsbtnPrev.Click += new EventHandler(this.tsmiPrev_Click);
			this.tsbtnNext.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbtnNext.Image = (Image)componentResourceManager.GetObject("tsbtnNext.Image");
			this.tsbtnNext.ImageTransparentColor = Color.Magenta;
			this.tsbtnNext.Name = "tsbtnNext";
			this.tsbtnNext.Size = new Size(23, 22);
			this.tsbtnNext.Text = "Next";
			this.tsbtnNext.Click += new EventHandler(this.tsmiNext_Click);
			this.tsbtnLast.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbtnLast.Image = (Image)componentResourceManager.GetObject("tsbtnLast.Image");
			this.tsbtnLast.ImageTransparentColor = Color.Magenta;
			this.tsbtnLast.Name = "tsbtnLast";
			this.tsbtnLast.Size = new Size(23, 22);
			this.tsbtnLast.Text = "Last";
			this.tsbtnLast.Click += new EventHandler(this.tsmiLast_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new Size(6, 25);
			this.tsbtnAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbtnAdd.Image = (Image)componentResourceManager.GetObject("tsbtnAdd.Image");
			this.tsbtnAdd.ImageTransparentColor = Color.Magenta;
			this.tsbtnAdd.Name = "tsbtnAdd";
			this.tsbtnAdd.Size = new Size(23, 22);
			this.tsbtnAdd.Text = "Add..";
			this.tsbtnAdd.Click += new EventHandler(this.tsmiAdd_Click);
			this.tsbtnDel.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbtnDel.Image = (Image)componentResourceManager.GetObject("tsbtnDel.Image");
			this.tsbtnDel.ImageTransparentColor = Color.Magenta;
			this.tsbtnDel.Name = "tsbtnDel";
			this.tsbtnDel.Size = new Size(23, 22);
			this.tsbtnDel.Text = "Delete";
			this.tsbtnDel.Click += new EventHandler(this.tsmiDel_Click);
			this.mnsZone.AllowMerge = false;
			this.mnsZone.Items.AddRange(new ToolStripItem[1]
			{
				this.tsmiCh
			});
			this.mnsZone.Location = new Point(0, 0);
			this.mnsZone.Name = "mnsZone";
			this.mnsZone.Size = new Size(794, 25);
			this.mnsZone.TabIndex = 34;
			this.mnsZone.Text = "menuStrip1";
			this.mnsZone.Visible = false;
			this.tsmiCh.DropDownItems.AddRange(new ToolStripItem[6]
			{
				this.tsmiFirst,
				this.tsmiPrev,
				this.tsmiNext,
				this.tsmiLast,
				this.tsmiAdd,
				this.tsmiDel
			});
			this.tsmiCh.Name = "tsmiCh";
			this.tsmiCh.Size = new Size(79, 21);
			this.tsmiCh.Text = "Operation";
			this.tsmiFirst.Name = "tsmiFirst";
			this.tsmiFirst.Size = new Size(159, 22);
			this.tsmiFirst.Text = "Fist";
			this.tsmiFirst.Click += new EventHandler(this.tsmiFirst_Click);
			this.tsmiPrev.Name = "tsmiPrev";
			this.tsmiPrev.Size = new Size(159, 22);
			this.tsmiPrev.Text = "Previous";
			this.tsmiPrev.Click += new EventHandler(this.tsmiPrev_Click);
			this.tsmiNext.Name = "tsmiNext";
			this.tsmiNext.Size = new Size(159, 22);
			this.tsmiNext.Text = "Next";
			this.tsmiNext.Click += new EventHandler(this.tsmiNext_Click);
			this.tsmiLast.Name = "tsmiLast";
			this.tsmiLast.Size = new Size(159, 22);
			this.tsmiLast.Text = "Last";
			this.tsmiLast.Click += new EventHandler(this.tsmiLast_Click);
			this.tsmiAdd.Name = "tsmiAdd";
			this.tsmiAdd.Size = new Size(159, 22);
			this.tsmiAdd.Text = "Add";
			this.tsmiAdd.Click += new EventHandler(this.tsmiAdd_Click);
			this.tsmiDel.Name = "tsmiDel";
			this.tsmiDel.Size = new Size(159, 22);
			this.tsmiDel.Text = "Delete";
			this.tsmiDel.Click += new EventHandler(this.tsmiDel_Click);
			this.btnDown.Location = new Point(676, 310);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new Size(75, 23);
			this.btnDown.TabIndex = 11;
			this.btnDown.Text = "Down";
			this.btnDown.UseVisualStyleBackColor = true;
			this.btnDown.Click += new EventHandler(this.btnDown_Click);
			this.btnUp.Location = new Point(676, 258);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new Size(75, 23);
			this.btnUp.TabIndex = 10;
			this.btnUp.Text = "Up";
			this.btnUp.UseVisualStyleBackColor = true;
			this.btnUp.Click += new EventHandler(this.btnUp_Click);
			this.txtName.InputString = null;
			this.txtName.Location = new Point(316, 62);
			this.txtName.MaxByteLength = 0;
			this.txtName.Name = "txtName";
			this.txtName.Size = new Size(115, 23);
			this.txtName.TabIndex = 1;
			this.txtName.Leave += new EventHandler(this.txtName_Leave);
			this.grpSelected.Controls.Add(this.lstSelected);
			this.grpSelected.Location = new Point(419, 110);
			this.grpSelected.Name = "grpSelected";
			this.grpSelected.Size = new Size(215, 388);
			this.grpSelected.TabIndex = 7;
			this.grpSelected.TabStop = false;
            this.grpSelected.Text = "Member";
            this.lstSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstSelected.FormattingEnabled = true;
			this.lstSelected.ItemHeight = 16;
			this.lstSelected.Location = new Point(47, 37);
			this.lstSelected.Name = "lstSelected";
			this.lstSelected.SelectionMode = SelectionMode.MultiExtended;
			this.lstSelected.Size = new Size(120, 324);
			this.lstSelected.TabIndex = 5;
			this.lstSelected.SelectedIndexChanged += new EventHandler(this.lstSelected_SelectedIndexChanged);
			this.lstSelected.DoubleClick += new EventHandler(this.lstSelected_DoubleClick);
			this.btnAdd.Location = new Point(327, 258);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new Size(75, 23);
			this.btnAdd.TabIndex = 3;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
			this.grpUnselected.Controls.Add(this.lstUnselected);
			this.grpUnselected.Location = new Point(86, 110);
			this.grpUnselected.Name = "grpUnselected";
            this.grpUnselected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpUnselected.Size = new Size(215, 388);
			this.grpUnselected.TabIndex = 6;
			this.grpUnselected.TabStop = false;
			this.grpUnselected.Text = "Available";
            this.lstUnselected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
| System.Windows.Forms.AnchorStyles.Left)));
            this.lstUnselected.FormattingEnabled = true;
			this.lstUnselected.ItemHeight = 16;
			this.lstUnselected.Location = new Point(52, 37);
			this.lstUnselected.Name = "lstUnselected";
			this.lstUnselected.SelectionMode = SelectionMode.MultiExtended;
			this.lstUnselected.Size = new Size(120, 324);
			this.lstUnselected.TabIndex = 2;
			this.btnDel.Location = new Point(327, 310);
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new Size(75, 23);
			this.btnDel.TabIndex = 4;
			this.btnDel.Text = "Delete";
			this.btnDel.UseVisualStyleBackColor = true;
			this.btnDel.Click += new EventHandler(this.btnDel_Click);
			this.lblName.Location = new Point(216, 63);
			this.lblName.Name = "lblName";
			this.lblName.Size = new Size(90, 23);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name";
			this.lblName.TextAlign = ContentAlignment.MiddleRight;
			base.AutoScaleDimensions = new SizeF(7f, 16f);
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(794, 560);
			base.Controls.Add(this.pnlZone);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.Name = "ZoneForm";
			this.Text = "Zone";
			base.Load += new EventHandler(this.ZoneForm_Load);
			base.FormClosing += new FormClosingEventHandler(this.ZoneForm_FormClosing);
			this.pnlZone.ResumeLayout(false);
			this.pnlZone.PerformLayout();
			this.tsrZone.ResumeLayout(false);
			this.tsrZone.PerformLayout();
			this.mnsZone.ResumeLayout(false);
			this.mnsZone.PerformLayout();
			this.grpSelected.ResumeLayout(false);
			this.grpUnselected.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public void SaveData()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			try
			{
				num3 = Convert.ToInt32(base.Tag);
				if (num3 == -1)
				{
					return;
				}
				ZoneOne value = new ZoneOne(num3);
				if (this.txtName.Focused)
				{
					this.txtName_Leave(this.txtName, null);
				}
				value.Name = this.txtName.Text;
				num2 = this.lstSelected.Items.Count;
				ushort[] array = new ushort[num2];
				foreach (SelectedItemUtils item in this.lstSelected.Items)
				{
					array[num++] = (ushort)item.Value;
				}
				value.ChList = array;
				ZoneForm.data[num3] = value;
				if (ZoneForm.basicData.CurZone == num3 || ZoneForm.basicData.SubZone == num3)
				{
					this.RefreshCh();
					ZoneForm.basicData.Verify();
					((MainForm)base.MdiParent).RefreshForm(typeof(ZoneBasicForm));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void RefreshCh()
		{
			int num = Convert.ToInt32(base.Tag);
			int num2 = 0;
			if (ZoneForm.basicData.CurZone == num)
			{
				num2 = Array.IndexOf(ZoneForm.data.ZoneList[num].ChList, (ushort)ZoneForm.MainChIndex);
				if (num2 >= 0)
				{
					ZoneForm.basicData.MainCh = num2;
				}
			}
			if (ZoneForm.basicData.SubZone == num)
			{
				num2 = Array.IndexOf(ZoneForm.data.ZoneList[num].ChList, (ushort)ZoneForm.SubChIndex);
				if (num2 >= 0)
				{
					ZoneForm.basicData.SubCh = num2;
				}
			}
		}

		public void DispData()
		{
			int num = 0;
			int num2 = 0;
			string text = "";
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			try
			{
				num3 = Convert.ToInt32(base.Tag);
				if (num3 == -1)
				{
					this.Close();
					return;
				}
				ZoneOne zoneOne = ZoneForm.data[num3];
				this.txtName.Text = zoneOne.Name;
				this.lstSelected.Items.Clear();
				for (num = 0; num < zoneOne.ChList.Length; num++)
				{
					num2 = zoneOne.ChList[num];
					if (num2 == 0)
					{
						break;
					}
					if (num2 <= 1024 && ChannelForm.data.DataIsValid(num2 - 1))
					{
						text = ChannelForm.data.GetName(num2 - 1);
						this.lstSelected.Items.Add(new SelectedItemUtils(num, num2, text));
					}
				}
				if (this.lstSelected.Items.Count > 0)
				{
					this.lstSelected.SelectedIndex = 0;
				}
				this.lstUnselected.Items.Clear();
				for (num = 0; num < 1024; num++)
				{
					if (!zoneOne.ChList.Contains((ushort)(num + 1)) && ChannelForm.data.DataIsValid(num))
					{
						num2 = num + 1;
						text = ChannelForm.data.GetName(num);
						this.lstUnselected.Items.Add(new SelectedItemUtils(-1, num2, text));
					}
				}
				if (this.lstUnselected.Items.Count > 0)
				{
					this.lstUnselected.SelectedIndex = 0;
				}
				this.method_5();
				this.method_6();
				num4 = ZoneForm.basicData.CurZone;
				num5 = ZoneForm.basicData.MainCh;
				ZoneForm.MainChIndex = ZoneForm.data.ZoneList[num4].ChList[num5];
				num4 = ZoneForm.basicData.SubZone;
				num5 = ZoneForm.basicData.SubCh;
				ZoneForm.SubChIndex = ZoneForm.data.ZoneList[num4].ChList[num5];
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void RefreshName()
		{
			int index = Convert.ToInt32(base.Tag);
			this.txtName.Text = ZoneForm.data[index].Name;
		}

		public ZoneForm()
		{
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void method_1()
		{
			this.txtName.MaxByteLength = 15;
			this.txtName.KeyPress += Settings.smethod_54;
		}

		private void ZoneForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);
			Settings.smethod_71(this.tsrZone.smethod_10(), base.Name);
			this.method_1();
			this.DispData();
		}

		private void ZoneForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			int num = 0;
			int count = this.lstUnselected.SelectedIndices.Count;
			int num2 = this.lstUnselected.SelectedIndices[count - 1];
			this.lstSelected.SelectedItems.Clear();
			while (this.lstUnselected.SelectedItems.Count > 0 && this.lstSelected.Items.Count < NUM_CHANNELS_PER_ZONE)
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
			this.method_4();
			this.method_5();
			if (!this.btnAdd.Enabled)
			{
				this.lstSelected.Focus();
			}
		}

		private void btnDel_Click(object sender, EventArgs e)
		{
			int num = 0;
			int count = this.lstSelected.SelectedIndices.Count;
			int num2 = this.lstSelected.SelectedIndices[count - 1];
			this.lstUnselected.SelectedItems.Clear();
			while (this.lstSelected.SelectedItems.Count > 0)
			{
				SelectedItemUtils @class = (SelectedItemUtils)this.lstSelected.SelectedItems[0];
				num = this.method_3(@class);
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
			this.method_4();
			this.method_5();
		}

		private bool method_2()
		{
			if (Convert.ToInt32(base.Tag) == 0)
			{
				return this.lstSelected.SelectedIndices.Contains(0);
			}
			return false;
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			int count = this.lstSelected.SelectedIndices.Count;
			int num3 = this.lstSelected.SelectedIndices[count - 1];
			for (num = 0; num < count; num++)
			{
				num2 = this.lstSelected.SelectedIndices[num];
				if (num != num2)
				{
					object value = this.lstSelected.Items[num2];
					this.lstSelected.Items[num2] = this.lstSelected.Items[num2 - 1];
					this.lstSelected.Items[num2 - 1] = value;
					this.lstSelected.SetSelected(num2, false);
					this.lstSelected.SetSelected(num2 - 1, true);
				}
			}
			this.method_4();
		}

		private void btnDown_Click(object sender, EventArgs e)
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
			this.method_4();
		}

		private int method_3(SelectedItemUtils class14_0)
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

		private void method_4()
		{
			int num = 0;
			bool flag = false;
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
		}

		private void method_5()
		{
			int num = Convert.ToInt32(base.Tag);
			this.btnAdd.Enabled = (this.lstUnselected.Items.Count > 0 && this.lstSelected.Items.Count < NUM_CHANNELS_PER_ZONE);
			if (num == 0 && this.lstSelected.SelectedIndices.Contains(0))
			{
				this.btnDel.Enabled = false;
			}
			else
			{
				this.btnDel.Enabled = (this.lstSelected.Items.Count > 0);
			}
			int count = this.lstSelected.Items.Count;
			int count2 = this.lstSelected.SelectedIndices.Count;
			this.btnUp.Enabled = (this.lstSelected.SelectedItems.Count > 0 && this.lstSelected.Items.Count > 0 && this.lstSelected.SelectedIndices[count2 - 1] != count2 - 1);
			this.btnDown.Enabled = (this.lstSelected.Items.Count > 0 && this.lstSelected.SelectedItems.Count > 0 && this.lstSelected.SelectedIndices[0] != count - count2);
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

		private void lstSelected_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.method_5();
		}

		private void lstSelected_DoubleClick(object sender, EventArgs e)
		{
			if (this.lstSelected.SelectedItem != null)
			{
				SelectedItemUtils @class = this.lstSelected.SelectedItem as SelectedItemUtils;
				MainForm mainForm = base.MdiParent as MainForm;
				if (mainForm != null)
				{
					mainForm.DispChildForm(typeof(ChannelForm), @class.Value - 1);
				}
			}
		}

		private void tsmiFirst_Click(object sender, EventArgs e)
		{
			this.SaveData();
			this.Node = this.Node.Parent.FirstNode;
			TreeNodeItem treeNodeItem = this.Node.Tag as TreeNodeItem;
			base.Tag = treeNodeItem.Index;
			this.DispData();
		}

		private void tsmiPrev_Click(object sender, EventArgs e)
		{
			this.SaveData();
			this.Node = this.Node.PrevNode;
			TreeNodeItem treeNodeItem = this.Node.Tag as TreeNodeItem;
			base.Tag = treeNodeItem.Index;
			this.DispData();
		}

		private void tsmiNext_Click(object sender, EventArgs e)
		{
			this.SaveData();
			this.Node = this.Node.NextNode;
			TreeNodeItem treeNodeItem = this.Node.Tag as TreeNodeItem;
			base.Tag = treeNodeItem.Index;
			this.DispData();
		}

		private void tsmiLast_Click(object sender, EventArgs e)
		{
			this.SaveData();
			this.Node = this.Node.Parent.LastNode;
			TreeNodeItem treeNodeItem = this.Node.Tag as TreeNodeItem;
			base.Tag = treeNodeItem.Index;
			this.DispData();
		}

		private void tsmiAdd_Click(object sender, EventArgs e)
		{
			if (this.Node.Parent.Nodes.Count < NUM_ZONES)
			{
				this.SaveData();
				TreeNodeItem treeNodeItem = this.Node.Tag as TreeNodeItem;
				int minIndex = ZoneForm.data.GetMinIndex();
				string minName = ZoneForm.data.GetMinName(this.Node);
				ZoneForm.data.SetIndex(minIndex, 1);
				TreeNodeItem tag = new TreeNodeItem(treeNodeItem.Cms, treeNodeItem.Type, null, 0, minIndex, treeNodeItem.ImageIndex, treeNodeItem.Data);
				TreeNode treeNode = new TreeNode(minName);
				treeNode.Tag = tag;
				treeNode.ImageIndex = 25;
				treeNode.SelectedImageIndex = 25;
				this.Node.Parent.Nodes.Insert(minIndex, treeNode);
				ZoneForm.data.SetName(minIndex, minName);
				this.Node = treeNode;
				base.Tag = minIndex;
				this.DispData();
				this.method_7();
			}
		}

		private void tsmiDel_Click(object sender, EventArgs e)
		{
			if (this.Node.Parent.Nodes.Count > 1 && this.Node.Index != 0)
			{
				this.SaveData();
				TreeNode node = this.Node.NextNode ?? this.Node.PrevNode;
				TreeNodeItem treeNodeItem = this.Node.Tag as TreeNodeItem;
				ZoneForm.data.ClearIndex(treeNodeItem.Index);
				this.Node.Remove();
				this.Node = node;
				TreeNodeItem treeNodeItem2 = this.Node.Tag as TreeNodeItem;
				base.Tag = treeNodeItem2.Index;
				this.DispData();
				this.method_7();
			}
			else
			{
				MessageBox.Show(Settings.dicCommon["FirstNotDelete"]);
			}
		}

		private void method_6()
		{
			this.tsbtnAdd.Enabled = (this.Node.Parent.Nodes.Count != NUM_ZONES);
			this.tsbtnDel.Enabled = (this.Node.Parent.Nodes.Count != 1 && this.Node.Index != 0);
			this.tsbtnFirst.Enabled = (this.Node != this.Node.Parent.FirstNode);
			this.tsbtnPrev.Enabled = (this.Node != this.Node.Parent.FirstNode);
			this.tsbtnNext.Enabled = (this.Node != this.Node.Parent.LastNode);
			this.tsbtnLast.Enabled = (this.Node != this.Node.Parent.LastNode);
			this.tslblInfo.Text = string.Format(" {0} / {1}", ZoneForm.data.GetDispIndex(Convert.ToInt32(base.Tag)), ZoneForm.data.ValidCount);
		}

		private void method_7()
		{
			MainForm mainForm = base.MdiParent as MainForm;
			if (mainForm != null)
			{
				mainForm.RefreshRelatedForm(typeof(ZoneForm));
			}
		}

		static ZoneForm()
		{
			
			ZoneForm.SPACE_ZONE = Marshal.SizeOf(typeof(ZoneOne));
			ZoneForm.basicData = new BasicZone();
			ZoneForm.data = new Zone();
			ZoneForm.SPACE_BASIC_ZONE = Marshal.SizeOf(typeof(BasicZone));
		}
	}
}
