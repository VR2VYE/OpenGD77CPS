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
	public struct RxListOneData
	{
#if OpenGD77
		public const int LEN_RX_LIST_NAME = 16;
		public const int CNT_CONTACT_PER_RX_LIST = 32;

#elif CP_VER_3_1_X
		public const int LEN_RX_LIST_NAME = 16;
		public const int CNT_CONTACT_PER_RX_LIST = 32;
#endif

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = LEN_RX_LIST_NAME)]
		private byte[] name;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = CNT_CONTACT_PER_RX_LIST)]
		private ushort[] contactList;

#if CP_VER_3_0_6
		private ushort reserve;					//Spare Ushort only present in 3-0-6 
#endif

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

		public ushort[] ContactList
		{
			get
			{
				return this.contactList;
			}
			set
			{
				this.contactList.Fill((ushort)0);
				Array.Copy(value, 0, this.contactList, 0, value.Length);
			}
		}

		public byte ValidCount
		{
			get
			{
				List<ushort> list = new List<ushort>(this.contactList);
				List<ushort> list2 = list.FindAll(RxListOneData.smethod_0);
				return (byte)list2.Count;
			}
		}

		public RxListOneData(int index)
		{

			this = default(RxListOneData);
			this.name = new byte[LEN_RX_LIST_NAME];
			this.contactList = new ushort[CNT_CONTACT_PER_RX_LIST];
		}

		public void Verify()
		{
			List<ushort> list = new List<ushort>(this.contactList);
			List<ushort> list2 = list.FindAll(RxListOneData.smethod_1);
			while (list2.Count < this.contactList.Length)
			{
				list2.Add(0);
			}
			this.contactList = list2.ToArray();
		}

		public bool ContainsContact(ushort id)
		{
			return (Array.FindIndex(contactList, item => item == id) != -1);
		}

		[CompilerGenerated]
		private static bool smethod_0(ushort ushort_0)
		{
			if (ushort_0 != 0 && ContactForm.data.DataIsValid(ushort_0 - 1))
			{
				return ContactForm.data.IsGroupCall(ushort_0 - 1);
			}
			return false;
		}

		[CompilerGenerated]
		private static bool smethod_1(ushort ushort_0)
		{
			if (ushort_0 != 0 && ContactForm.data.DataIsValid(ushort_0 - 1))
			{
				return ContactForm.data.IsGroupCall(ushort_0 - 1);
			}
			return false;
		}
	}

}
