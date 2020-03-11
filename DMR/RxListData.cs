using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class RxListData : IData
	{
#if OpenGD77
		public const int CNT_RX_LIST = 76;// works if you set this to 128
		public const int CNT_RX_LIST_INDEX = 128; //List index remains at 128 even though only 76 are used. 
#elif CP_VER_3_1_X
		public const int CNT_RX_LIST = 76;// works if you set this to 128
		public const int CNT_RX_LIST_INDEX = 128; //List index remains at 128 even though only 76 are used. 
#endif
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = CNT_RX_LIST_INDEX)]
		private byte[] rxListIndex;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = CNT_RX_LIST)]
		private RxListOneData[] rxList;

		public RxListOneData[] RxList
		{
			get
			{
				return rxList;
			}
		}

		public RxListOneData this[int index]
		{
			get
			{
				if (index >= this.Count)
				{
					throw new ArgumentOutOfRangeException();
				}
				return this.rxList[index];
			}
			set
			{
				if (index >= this.Count)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.rxList[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return CNT_RX_LIST;
			}
		}

		public string Format
		{
			get
			{
				return "GroupList{0}";
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

		public RxListData()
		{
			int num = 0;
			this.rxListIndex = new byte[CNT_RX_LIST_INDEX];
			this.rxList = new RxListOneData[CNT_RX_LIST];
			for (num = 0; num < this.Count; num++)
			{
				this.rxList[num] = new RxListOneData(num);
			}
		}

		public void Clear()
		{
		}

		public int GetContactCntByIndex(int index)
		{
			if (index < CNT_RX_LIST)
			{
				if (this.rxListIndex[index] >= 2 && this.rxListIndex[index] <= RxListOneData.CNT_CONTACT_PER_RX_LIST + 1)
				{
					return this.rxListIndex[index] - 1;
				}
				return 0;
			}
			return 0;
		}

		public void SetRxListIndex(int index, bool add)
		{
			if (add)
			{
				this.rxListIndex[index] = 1;
			}
			else
			{
				this.rxListIndex[index] = 0;
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
					if (this.rxListIndex[num] == 0)
					{
						break;
					}
					if (this.rxListIndex[num] > RxListOneData.CNT_CONTACT_PER_RX_LIST+1)
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
			if (this.rxListIndex[index] != 0)
			{
				return this.rxListIndex[index] <= RxListOneData.CNT_CONTACT_PER_RX_LIST + 1;
			}
			return false;
		}

		public void SetIndex(int index, int value)
		{
			this.rxListIndex[index] = (byte)value;
		}

		public int GetContactsCountForIndex(int index)
		{
			return Math.Max(0,this.rxListIndex[index]-1);
		}

		public void ClearIndex(int index)
		{
			this.rxListIndex[index] = 0;
			ChannelForm.data.ClearByRxGroup(index);
		}

		public void ClearByData(int contactIndex)
		{
			int num = 0;
			int num2 = 0;
			for (num = 0; num < this.Count; num++)
			{
				if (this.DataIsValid(num))
				{
					num2 = Array.IndexOf(this.rxList[num].ContactList, (ushort)(contactIndex + 1));
					if (num2 >= 0)
					{
						this.rxList[num].ContactList.RemoveItemFromArray(num2);
						this.rxListIndex[num]--;
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
			this.rxList[index].Name = text;
		}

		public string GetName(int index)
		{
			return this.rxList[index].Name;
		}

		public void Default(int index)
		{
			/* Roger Clark. Its potentially best to initialise the Rx list with an array with worst case size
			if (this.rxList[index].ContactList.Length != 32)
			{
				this.rxList[index].ContactList = new ushort[32];
			}*/
			this.rxList[index].ContactList.Fill((ushort)0);
		}

		public void Paste(int from, int to)
		{
			this.rxListIndex[to] = this.rxListIndex[from];
			Array.Copy(this.rxList[from].ContactList, this.rxList[to].ContactList, this.rxList[from].ContactList.Length);
		}

		public void Verify()
		{
			int num = 0;
			for (num = 0; num < this.Count; num++)
			{
				if (this.DataIsValid(num))
				{
					this.rxList[num].Verify();
					this.rxListIndex[num] = (byte)(this.rxList[num].ValidCount + 1);
				}
				else
				{
					this.rxListIndex[num] = 0;
				}
			}
		}

		public int AddRxGroupWithName(string newGroupName)
		{

			int groupIndex = Array.FindIndex(RxGroupListForm.data.RxList, item => item.Name == newGroupName);
			if (groupIndex == -1)
			{
				// Group does not exist, so we need to create a new one
				groupIndex = GetMinIndex();
				if (groupIndex != -1)
				{
					SetIndex(groupIndex, 1);
					Default(groupIndex);
					SetName(groupIndex, newGroupName);
				}
			}

			return groupIndex;
		}
	}
}
