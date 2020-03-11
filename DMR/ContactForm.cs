using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class ContactForm : DockContent, IDisp
	{
		public enum CallType
		{
			GroupCall,
			PrivateCall,
			AllCall
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct ContactOne : IVerify<ContactOne>
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			private byte[] name;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			private byte[] callId;
			private byte callType;
			private byte callRxTone;
			private byte ringStyle;
			private byte reserve1;

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

			public string CallId
			{
				get
				{
					int num = 0;
					string value = BitConverter.ToString(this.callId).Replace("-", "");
					try
					{
						num = Convert.ToInt32(value);
						if (num >= 1 && num <= 16776415)
						{
							goto IL_0038;
						}
						if (num == 16777215)
						{
							goto IL_0038;
						}
						return "1";
						IL_0038:
						return num.ToString();//.PadLeft(8, '0');
					}
					catch
					{
						return "";
					}
				}
				set
				{
					int num = 0;
					int num2 = Convert.ToInt32(value);
					if ((num2 < 1 || num2 > 16776415) && num2 != 16777215)
					{
						return;
					}
					string text = value.PadLeft(8, '0');
					for (num = 0; num < 4; num++)
					{
						this.callId[num] = Convert.ToByte(text.Substring(num * 2, 2), 16);
					}
				}
			}

			public int CallType
			{
				get
				{
					if (Enum.IsDefined(typeof(CallTypeE), this.callType))
					{
						return this.callType;
					}
					return 0;
				}
				set
				{
					if (Enum.IsDefined(typeof(CallTypeE), (byte)value))
					{
						this.callType = (byte)value;
					}
					else
					{
						this.callType = 0;
					}
				}
			}

			public string CallTypeS
			{
				get
				{
					if (this.callType < ContactForm.SZ_CALL_TYPE.Length)
					{
						return ContactForm.SZ_CALL_TYPE[this.callType];
					}
					return "";
				}
				set
				{
					int num = Array.IndexOf(ContactForm.SZ_CALL_TYPE, value);
					if (num < 0)
					{
						num = 0;
					}
					this.callType = (byte)num;
				}
			}

			public bool CallRxTone
			{
				get
				{
					return Convert.ToBoolean(this.callRxTone);
				}
				set
				{
					this.callRxTone = Convert.ToByte(value);
				}
			}

			public string CallRxToneS
			{
				get
				{
					if (this.callRxTone < ContactForm.SZ_CALL_RX_TONE.Length)
					{
						return ContactForm.SZ_CALL_RX_TONE[this.callRxTone];
					}
					return "";
				}
				set
				{
					int num = Array.IndexOf(ContactForm.SZ_CALL_RX_TONE, value);
					if (num < 0)
					{
						num = 0;
					}
					this.callRxTone = (byte)num;
				}
			}

			public int RepeaterSlot
			{
				get
				{
					if ((this.reserve1 & 0x01) == 0x01)
					{
						return 0;
					}
					else
					{
						int tmp = ((this.reserve1 & 0x02) >> 1) + 1;
						return tmp;
					}

				}
				set
				{
					if (value == 0)
					{
						this.reserve1=0x01;
					}
					else
					{
						this.reserve1 = (byte)((value - 1)<<1);
					}
				}
			}

			public string RepeaterSlotS
			{
				get
				{
					/*	if (this.reserve1 == 0xFF)
						{
							return Settings.SZ_NONE;
						}
					 */
					if (this.RepeaterSlot == 0)
					{
						return Settings.SZ_NONE;
					}
					else
					{
						return this.RepeaterSlot.ToString();
					}
				}
				set
				{
					try
					{
						if (value == Settings.SZ_NONE)
						{
							this.RepeaterSlot = 0;
						}
						else
						{
							this.RepeaterSlot = Convert.ToByte(value);
						}
					}
					catch
					{
						this.RepeaterSlot = 0;
					}
				}
			}

			public int RingStyle
			{
				get
				{
					if (this.ringStyle >= 0 && this.ringStyle <= 10)
					{
						return this.ringStyle;
					}
					return 0;
				}
				set
				{
					if (value >= 0 && value <= 10)
					{
						this.ringStyle = (byte)value;
					}
				}
			}

			public string RingStyleS
			{
				get
				{
					if (this.ringStyle == 0)
					{
						return Settings.SZ_NONE;
					}
					return this.ringStyle.ToString();
				}
				set
				{
					try
					{
						if (value == Settings.SZ_NONE)
						{
							this.ringStyle = 0;
						}
						else
						{
							this.ringStyle = Convert.ToByte(value);
						}
					}
					catch
					{
						this.ringStyle = 0;
					}
				}
			}

			public ContactOne(int index)
			{
				
				this = default(ContactOne);
				this.name = new byte[16];
				this.callId = new byte[4];
				this.callType = 255;
				this.callRxTone = 0;
				this.ringStyle = 0;
				this.reserve1 = 255;
			}

			public ContactOne Clone()
			{
				return Settings.smethod_65(this);
			}

			public void Default()
			{
				this.CallRxTone = ContactForm.DefaultContact.CallRxTone;
				this.RingStyle = ContactForm.DefaultContact.RingStyle;
			}

			public bool DataIsValid()
			{
				if (!string.IsNullOrEmpty(this.Name))
				{
					return true;
				}
				return false;
			}

			public void Verify(ContactOne def)
			{
				if (!Enum.IsDefined(typeof(CallTypeE), this.callType))
				{
					this.callType = def.callType;
				}
				Settings.smethod_11(ref this.callRxTone, (byte)0, (byte)1, def.callRxTone);
				Settings.smethod_11(ref this.ringStyle, (byte)0, (byte)10, def.ringStyle);
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class Contact : IData
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
			private ContactOne[] contactList;

			public ContactOne[] ContactList
			{
				get
				{
					return contactList;
				}
			}

			public ContactOne this[int index]
			{
				get
				{
					if (index >= 1024)
					{
						throw new ArgumentOutOfRangeException();
					}
					return this.contactList[index];
				}
				set
				{
					this.contactList[index] = value;
				}
			}

			public int Count
			{
				get
				{
					return 1024;
				}
			}

			public int ValidCount
			{
				get
				{
					int num = 0;
					for (num = 0; num < this.Count && !string.IsNullOrEmpty(this[num].Name); num++)
					{
					}
					return num;
				}
			}

			public string Format
			{
				get
				{
					return "Contact{0}";
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

			public Contact()
			{
				int num = 0;
				this.contactList = new ContactOne[1024];
				for (num = 0; num < this.contactList.Length; num++)
				{
					this.contactList[num] = new ContactOne(num);
					this.contactList[num].Name = "";
					this.contactList[num].CallId = "00000001";
				}
			}

			public bool HaveAll()
			{
				int num = 0;
				num = 0;
				while (true)
				{
					if (num < this.Count)
					{
						if (this.DataIsValid(num) && this[num].CallType == 2)
						{
							break;
						}
						num++;
						continue;
					}
					return false;
				}
				return true;
			}

			public int AllCallIndex()
			{
				return Array.FindIndex(this.contactList, Contact.smethod_0);
			}

			public int FindNextValidIndex(int index)
			{
				int num = -1;
				num = Array.FindIndex(this.contactList, index, Contact.smethod_1);
				if (num == -1)
				{
					while (--index >= 0)
					{
						if (!this.DataIsValid(index))
						{
							continue;
						}
						num = index;
						break;
					}
				}
				return num;
			}

			public bool DataIsValid(int index)
			{
				if (index < 1024 && index >= 0 && !string.IsNullOrEmpty(this[index].Name))
				{
					return true;
				}
				return false;
			}

			public void SetIndex(int index, int value)
			{
				this.contactList[index].CallType = value;
				if (value == 0)
				{
					this.SetName(index, "");
				}
			}

			public void ClearIndex(int index)
			{
				this.contactList[index].CallType = 255;
				this.SetName(index, "");
				RxGroupListForm.data.ClearByData(index);
				ChannelForm.data.ClearByContact(index);
				ButtonForm.data1.ClearByContact(index);
			}

			public int GetMinIndex()
			{
				int num = 0;
				num = 0;
				while (true)
				{
					if (num < 1024)
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

			public int GetRepeaterSlot(int index)
			{
				if (this.DataIsValid(index))
				{
					return this.contactList[index].RepeaterSlot;
				}
				return 0;
			}

			public string GetCallID(int index)
			{
				if (this.DataIsValid(index))
				{
					return this.contactList[index].CallId;
				}
				return string.Empty;
			}

			public void SetCallID(int index, string callID)
			{
				this.contactList[index].CallId = callID;
			}

			public bool CallIdExist(int index, string callId)
			{
				int num = 0;
				int callType = this.contactList[index].CallType;
				num = 0;
				while (true)
				{
					if (num < this.Count)
					{
						if (ContactForm.data.GetCallType(num) == callType && num != index && ContactForm.data.GetCallID(num) == callId)
						{
							break;
						}
						num++;
						continue;
					}
					return false;
				}
				return true;
			}

			public int GetCallIndexFromIdString(int callId)
			{
				int num = 0;
				string callIdStr = string.Format("{0:d8}", callId);

				while (num < this.Count)
				{
					if (ContactForm.data.GetCallID(num) == callIdStr)
					{
						return num+1;
					}
					num++;
				}
		
				return -1;
			}


			public bool CallIdExist(int index, int callType, string callId, int repeaterSlot)
			{
				int num = 0;
				num = 0;
				while (true)
				{
					if (num < this.Count)
					{
						if (ContactForm.data.GetCallType(num) == callType && num != index && 
							ContactForm.data.GetCallID(num) == callId &&
							ContactForm.data.GetRepeaterSlot(num) == repeaterSlot)
						{
							break;
						}
						num++;
						continue;
					}
					return false;
				}
				return true;
			}

			public string GetMinCallID()
			{
				int num = 0;
				int num2 = 0;
				bool flag = false;
				string text = "";
				int validCount = this.ValidCount;
				num = 0;
				while (true)
				{
					if (num < validCount)
					{
						flag = false;
						text = string.Format("{0:d8}", num + 1);
						num2 = 0;
						while (num2 < validCount)
						{
							if (!(ContactForm.data.GetCallID(num2) == text))
							{
								num2++;
								continue;
							}
							flag = true;
							break;
						}
						if (!flag)
						{
							break;
						}
						num++;
						continue;
					}
					return string.Empty;
				}
				return text;
			}

			public string GetMinCallID(int type)
			{
				int num = 0;
				int num2 = 0;
				bool flag = false;
				string text = "";
				int validCount = this.ValidCount;
				num = 0;
				while (true)
				{
					if (num < validCount)
					{
						flag = false;
						text = string.Format("{0:d8}", num + 1);
						num2 = 0;
						while (num2 < validCount)
						{
							if (ContactForm.data.GetCallType(num2) != type || !(ContactForm.data.GetCallID(num2) == text))
							{
								num2++;
								continue;
							}
							flag = true;
							break;
						}
						if (!flag)
						{
							break;
						}
						num++;
						continue;
					}
					return string.Empty;
				}
				return text;
			}

			public string GetMinCallID(int type, int index)
			{
				string text = string.Format("{0:d8}", index + 1);
				bool flag = false;
				int validCount = this.ValidCount;
				int num = 0;
				while (num < validCount)
				{
					if (this.contactList[num].CallType != type || !(this.contactList[num].CallId == text))
					{
						num++;
						continue;
					}
					flag = true;
					break;
				}
				if (flag)
				{
					return this.GetMinCallID(type);
				}
				return text;
			}

			public bool CallIdValid(string callId)
			{
				int num = Convert.ToInt32(callId);
				if (num >= 1 && num <= 16776415)
				{
					return true;
				}
				return false;
			}

			public int GetCallType(int index)
			{
				if (this.DataIsValid(index))
				{
					return this.contactList[index].CallType;
				}
				return 2;
			}

			public void SetCallType(int index, string callType)
			{
				this.contactList[index].CallTypeS = callType;
			}

			public void SetCallType(int index, int callType)
			{
				this.contactList[index].CallType = callType;
			}

			public bool IsGroupCall(int index)
			{
				if (index < 1024)
				{
					return this.contactList[index].CallType == 0;
				}
				return false;
			}

			public bool IsAllCall(int index)
			{
				if (index < 1024)
				{
					return this.contactList[index].CallType == 2;
				}
				return false;
			}

			public void SetCallRxTone(int index, string callRxTone)
			{
				this.contactList[index].CallRxToneS = callRxTone;
			}

			public void SetCallRxTone(int index, bool check)
			{
				this.contactList[index].CallRxTone = check;
			}

			public void SetRingStyle(int index, string ringStyle)
			{
				this.contactList[index].RingStyleS = ringStyle;
			}

			public void SetRingStyle(int index, int ringStyle)
			{
				this.contactList[index].RingStyle = ringStyle;
			}

			public void SetRepeaterSlot(int index, string repeaterSlot)
			{
				this.contactList[index].RepeaterSlotS = repeaterSlot;
			}

			public void SetRepeaterSlot(int index, int repeaterSlot)
			{
				this.contactList[index].RepeaterSlot = repeaterSlot;
			}

			public void SetName(int index, string text)
			{
				this.contactList[index].Name = text;
			}

			public string GetName(int index)
			{
				return this.contactList[index].Name;
			}

			public string GetMinName(TreeNode node)
			{
				int num = 0;
				int num2 = 0;
				string text = "";
				num2 = ContactForm.data.GetMinIndex();
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

			public bool NameExist(string name)
			{
				return this.contactList.Any((ContactOne x) => x.Name == name);
			}

			public int GetIndexForName(string name)
			{
				return Array.FindIndex(this.contactList, item => item.Name == name);
			}

			public void Default(int index)
			{
				this.contactList[index].Default();
			}

			public void Paste(int from, int to)
			{
				this.contactList[to].CallRxTone = this.contactList[from].CallRxTone;
				this.contactList[to].RingStyle = this.contactList[from].RingStyle;
			}

			public void Verify()
			{
				int num = 0;
				for (num = 0; num < this.Count; num++)
				{
					if (this.DataIsValid(num))
					{
						this.contactList[num].Verify(ContactForm.DefaultContact);
					}
				}
			}

			[CompilerGenerated]
			private static bool smethod_0(ContactOne contactOne_0)
			{
				return contactOne_0.CallType == 2;
			}

			[CompilerGenerated]
			private static bool smethod_1(ContactOne contactOne_0)
			{
				return contactOne_0.Name != "";
			}
		}

		public const int CNT_CONTACT = 1024;
		public const int LEN_CONTACT_NAME = 16;
		public const int MIN_CALL_ID = 1;
		public const int MAX_CALL_ID = 16776415;
		public const int INC_CALL_ID = 1;
		public const int SCL_CALL_ID = 1;
		public const int LEN_CALL_ID = 8;
		public const int SPC_CALL_ID = 4;
		public const int MIN_RING_STYLE = 0;
		public const int MAX_RING_STYLE = 10;
		public const int MIN_CALL_RX_TONE = 0;
		public const int MAX_CALL_RX_TONE = 1;
		public const string SZ_PRIVATE_ID = "0123456789\b";
		public const string SZ_GROUP_ID = "0123456789*\b";
		public const string SZ_ALL_ID = "*******";
		public const int ALL_CODE = 16777215;
		public const string SZ_CALL_TYPE_NAME = "CallType";
		public const string SZ_CALL_RX_TONE_NAME = "CallRxTone";

		//private IContainer components;

		private CheckBox chkCallRxTone;
		private SGTextBox txtName;
		private Label lblName;
		private Label lblCallId;
		private Label lblCallType;
		private CustomCombo cmbCallType;
		private SGTextBox txtCallId;
		private CustomCombo cmbRingStyle;
		private Label lblRingStyle;
		private CustomPanel pnlContact;
		public static readonly string[] SZ_CALL_TYPE;
		public static readonly string[] SZ_CALL_RX_TONE;
		public static ContactOne DefaultContact;
		private CustomCombo cmbRepeaterSlot;
		private Label lblRepeaterSlot;
		private GroupBox openGD77groupbox;
		public static Contact data;

		public TreeNode Node
		{
			get;
			set;
		}

		protected override void Dispose(bool disposing)
		{
			/*if (disposing && this.components != null)
			{
				this.components.Dispose();
			}*/
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.pnlContact = new CustomPanel();
			this.openGD77groupbox = new System.Windows.Forms.GroupBox();
			this.cmbRepeaterSlot = new CustomCombo();
			this.lblRepeaterSlot = new System.Windows.Forms.Label();
			this.txtCallId = new DMR.SGTextBox();
			this.chkCallRxTone = new System.Windows.Forms.CheckBox();
			this.cmbRingStyle = new CustomCombo();
			this.cmbCallType = new CustomCombo();
			this.lblRingStyle = new System.Windows.Forms.Label();
			this.txtName = new DMR.SGTextBox();
			this.lblCallType = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.lblCallId = new System.Windows.Forms.Label();
			this.pnlContact.SuspendLayout();
			this.openGD77groupbox.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlContact
			// 
			this.pnlContact.AutoScroll = true;
			this.pnlContact.AutoSize = true;
			this.pnlContact.Controls.Add(this.openGD77groupbox);
			this.pnlContact.Controls.Add(this.txtCallId);
			this.pnlContact.Controls.Add(this.chkCallRxTone);
			this.pnlContact.Controls.Add(this.cmbRingStyle);
			this.pnlContact.Controls.Add(this.cmbCallType);
			this.pnlContact.Controls.Add(this.lblRingStyle);
			this.pnlContact.Controls.Add(this.txtName);
			this.pnlContact.Controls.Add(this.lblCallType);
			this.pnlContact.Controls.Add(this.lblName);
			this.pnlContact.Controls.Add(this.lblCallId);
			this.pnlContact.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlContact.Location = new System.Drawing.Point(0, 0);
			this.pnlContact.Name = "pnlContact";
			this.pnlContact.Size = new System.Drawing.Size(314, 267);
			this.pnlContact.TabIndex = 7;
			// 
			// openGD77groupbox
			// 
			this.openGD77groupbox.Controls.Add(this.cmbRepeaterSlot);
			this.openGD77groupbox.Controls.Add(this.lblRepeaterSlot);
			this.openGD77groupbox.Location = new System.Drawing.Point(7, 173);
			this.openGD77groupbox.Name = "openGD77groupbox";
			this.openGD77groupbox.Size = new System.Drawing.Size(283, 62);
			this.openGD77groupbox.TabIndex = 7;
			this.openGD77groupbox.TabStop = false;
			this.openGD77groupbox.Text = "OpenGD77 - Channel TS override";
			// 
			// cmbRepeaterSlot
			// 
			this.cmbRepeaterSlot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbRepeaterSlot.FormattingEnabled = true;
			this.cmbRepeaterSlot.Items.AddRange(new object[] {
            "Disabled",
            "TS 1",
            "TS 2"});
			this.cmbRepeaterSlot.Location = new System.Drawing.Point(129, 32);
			this.cmbRepeaterSlot.Name = "cmbRepeaterSlot";
			this.cmbRepeaterSlot.Size = new System.Drawing.Size(120, 24);
			this.cmbRepeaterSlot.TabIndex = 5;
			this.cmbRepeaterSlot.SelectedIndexChanged += new System.EventHandler(this.cmbRepeaterSlot_SelectedIndexChanged);
			// 
			// lblRepeaterSlot
			// 
			this.lblRepeaterSlot.Location = new System.Drawing.Point(12, 32);
			this.lblRepeaterSlot.Name = "lblRepeaterSlot";
			this.lblRepeaterSlot.Size = new System.Drawing.Size(111, 24);
			this.lblRepeaterSlot.TabIndex = 4;
			this.lblRepeaterSlot.Text = "Repeater Slot";
			this.lblRepeaterSlot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

			// 
			// txtCallId
			// 
			this.txtCallId.InputString = null;
			this.txtCallId.Location = new System.Drawing.Point(137, 44);
			this.txtCallId.MaxByteLength = 0;
			this.txtCallId.Name = "txtCallId";
			this.txtCallId.Size = new System.Drawing.Size(120, 23);
			this.txtCallId.TabIndex = 3;
			this.txtCallId.Enter += new System.EventHandler(this.txtCallId_Enter);
			this.txtCallId.Leave += new System.EventHandler(this.txtCallId_Leave);
			this.txtCallId.Validating += new System.ComponentModel.CancelEventHandler(this.txtCallId_Validating);
			// 
			// chkCallRxTone
			// 
			this.chkCallRxTone.AutoSize = true;
			this.chkCallRxTone.Location = new System.Drawing.Point(137, 137);
			this.chkCallRxTone.Name = "chkCallRxTone";
			this.chkCallRxTone.Size = new System.Drawing.Size(141, 20);
			this.chkCallRxTone.TabIndex = 6;
			this.chkCallRxTone.Text = "Call Receive Tone";
			this.chkCallRxTone.UseVisualStyleBackColor = true;
			// 
			// cmbRingStyle
			// 
			this.cmbRingStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbRingStyle.FormattingEnabled = true;
			this.cmbRingStyle.Location = new System.Drawing.Point(137, 104);
			this.cmbRingStyle.Name = "cmbRingStyle";
			this.cmbRingStyle.Size = new System.Drawing.Size(120, 24);
			this.cmbRingStyle.TabIndex = 5;
			this.cmbRingStyle.SelectedIndexChanged += new System.EventHandler(this.cmbRingStyle_SelectedIndexChanged);
			// 
			// cmbCallType
			// 
			this.cmbCallType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCallType.FormattingEnabled = true;
			this.cmbCallType.Location = new System.Drawing.Point(137, 74);
			this.cmbCallType.Name = "cmbCallType";
			this.cmbCallType.Size = new System.Drawing.Size(120, 24);
			this.cmbCallType.TabIndex = 5;
			this.cmbCallType.SelectedIndexChanged += new System.EventHandler(this.cmbCallType_SelectedIndexChanged);
			// 
			// lblRingStyle
			// 
			this.lblRingStyle.Location = new System.Drawing.Point(20, 104);
			this.lblRingStyle.Name = "lblRingStyle";
			this.lblRingStyle.Size = new System.Drawing.Size(111, 24);
			this.lblRingStyle.TabIndex = 4;
			this.lblRingStyle.Text = "Ring Style";
			this.lblRingStyle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtName
			// 
			this.txtName.InputString = null;
			this.txtName.Location = new System.Drawing.Point(137, 14);
			this.txtName.MaxByteLength = 0;
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(120, 23);
			this.txtName.TabIndex = 1;
			this.txtName.Leave += new System.EventHandler(this.txtName_Leave);
			// 
			// lblCallType
			// 
			this.lblCallType.Enabled = false;
			this.lblCallType.Location = new System.Drawing.Point(23, 74);
			this.lblCallType.Name = "lblCallType";
			this.lblCallType.Size = new System.Drawing.Size(108, 24);
			this.lblCallType.TabIndex = 4;
			this.lblCallType.Text = "Call Type";
			this.lblCallType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(26, 14);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(105, 24);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name";
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblCallId
			// 
			this.lblCallId.Location = new System.Drawing.Point(26, 44);
			this.lblCallId.Name = "lblCallId";
			this.lblCallId.Size = new System.Drawing.Size(105, 24);
			this.lblCallId.TabIndex = 2;
			this.lblCallId.Text = "Call ID";
			this.lblCallId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ContactForm
			// 
			this.ClientSize = new System.Drawing.Size(314, 267);
			this.Controls.Add(this.pnlContact);
			this.Font = new System.Drawing.Font("Arial", 10F);
			this.Name = "ContactForm";
			this.Text = "Digital Contact";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ContactForm_FormClosing);
			this.Load += new System.EventHandler(this.ContactForm_Load);
			this.pnlContact.ResumeLayout(false);
			this.pnlContact.PerformLayout();
			this.openGD77groupbox.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

        string _PreCallId;
		[CompilerGenerated]
		private string method_1()
		{
			return this._PreCallId;
		}

		[CompilerGenerated]
		private void method_2(string value)
		{
			this._PreCallId = value;
		}

		public void SaveData()
		{

			// Dont save if there is a problem with duplicate names
			if (!Settings.smethod_50(this.Node, this.txtName.Text))
			{
				try
				{
					int index = Convert.ToInt32(base.Tag);
					if (index == -1)
					{
						return;
					}
					if (this.txtName.Focused)
					{
						this.txtName_Leave(this.txtName, null);
					}
					ContactOne value = new ContactOne(index);
					value.Name = this.txtName.Text;
					value.CallId = this.txtCallId.Text;
					value.CallType = this.cmbCallType.method_3();
					value.CallRxTone = this.chkCallRxTone.Checked;
					value.RingStyle = this.cmbRingStyle.SelectedIndex;
					value.RepeaterSlot = this.cmbRepeaterSlot.SelectedIndex;
					
					ContactForm.data[index] = value;
					((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType());
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		public void DispData()
		{
			try
			{
				int num = Convert.ToInt32(base.Tag);
				if (num == -1)
				{
					this.Close();
					return;
				}
				if (!ContactForm.data.DataIsValid(num))
				{
					num = ContactForm.data.FindNextValidIndex(num);
					this.Node = ((MainForm)base.MdiParent).GetTreeNodeByType(typeof(ContactsForm), num);
					base.Tag = num;
				}
				this.chkCallRxTone.CheckedChanged -= this.chkCallRxTone_CheckedChanged;
				this.cmbRingStyle.SelectedIndexChanged -= this.cmbRingStyle_SelectedIndexChanged;
				this.txtName.Text = ContactForm.data[num].Name;
				this.txtCallId.Text = ContactForm.data[num].CallId;
				this.cmbCallType.method_2(ContactForm.data[num].CallType);
				this.chkCallRxTone.Checked = ContactForm.data[num].CallRxTone;
				this.cmbRingStyle.SelectedIndex = ContactForm.data[num].RingStyle;
				this.cmbRepeaterSlot.SelectedIndex = ContactForm.data[num].RepeaterSlot;

				this.chkCallRxTone.CheckedChanged += this.chkCallRxTone_CheckedChanged;
				this.cmbRingStyle.SelectedIndexChanged += this.cmbRingStyle_SelectedIndexChanged;
				this.method_3();
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
			this.lblRingStyle.Enabled &= flag;
			this.cmbRingStyle.Enabled &= flag;
			this.chkCallRxTone.Enabled &= flag;
		}

		public void RefreshName()
		{
			int index = Convert.ToInt32(base.Tag);
			this.txtName.Text = ContactForm.data[index].Name;
		}

		public ContactForm()
		{
			
			//base._002Ector();
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		public void InitData()
		{
			int i = 0;
			this.txtName.MaxLength = 16;
			this.txtName.KeyPress += Settings.smethod_54;
			this.txtCallId.MaxLength = 8;
			i = 0;
			this.cmbCallType.Items.Clear();
			foreach (byte value in Enum.GetValues(typeof(CallTypeE)))
			{
				this.cmbCallType.method_1(ContactForm.SZ_CALL_TYPE[i++], value);
			}
			this.cmbRingStyle.Items.Clear();
			this.cmbRingStyle.Items.Add(Settings.SZ_NONE);
			for (i = 1; i <= 10; i++)
			{
				this.cmbRingStyle.Items.Add(i.ToString());
			}
		}

		private void ContactForm_Load(object sender, EventArgs e)
		{
			try
			{
				Settings.smethod_59(base.Controls);
				Settings.smethod_68(this);
				this.InitData();
				this.DispData();
				this.method_3();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void ContactForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
		}

		public static void RefreshCommonLang()
		{
			string name = typeof(ContactForm).Name;
			Settings.smethod_78("CallType", ContactForm.SZ_CALL_TYPE, name);
			Settings.smethod_78("CallRxTone", ContactForm.SZ_CALL_RX_TONE, name);
		}

		private void method_3()
		{
			int num = this.cmbCallType.method_3();
			this.txtCallId.Enabled = true;
			switch (num)
			{
			case 2:
				this.txtCallId.Enabled = false;
				this.txtCallId.Text = 16777215.ToString();
				this.cmbRingStyle.Enabled = false;
				break;
			case 1:
				this.txtCallId.InputString = "0123456789\b";
				this.cmbRingStyle.Enabled = true;
				break;
			case 0:
				this.txtCallId.InputString = "0123456789*\b";
				this.cmbRingStyle.Enabled = false;
				break;
			}
		}

		public static bool IsValidId(string strId)
		{
			string value = strId.Replace('*', '0');
			int num = Convert.ToInt32(value);
			if (num != 0 && num % 1000000 != 0)
			{
				return true;
			}
			return false;
		}

		private void cmbCallType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.method_3();
		}

		private void cmbRepeaterSlot_SelectedIndexChanged(object sender, EventArgs e)
		{
			//this.method_3();
		}

		private void txtName_Leave(object sender, EventArgs e)
		{
			this.txtName.Text = this.txtName.Text.Trim();
			if (this.Node.Text != this.txtName.Text)
			{
				if (Settings.smethod_50(this.Node, this.txtName.Text))
				{
					MessageBox.Show(Settings.dicCommon["ContactNameDuplicate"]);
					//this.txtName.Text = this.Node.Text;// Don't reinstate the old name, so that the user has chance to ammend the name they entered
				}
				else
				{
					// Only save the contact if the name is not a duplicate
					this.Node.Text = this.txtName.Text;
					this.SaveData();
				}
			}
		}

		private void txtCallId_Validating(object sender, CancelEventArgs e)
		{
			int index = Convert.ToInt32(base.Tag);
			int selectedIndex = this.cmbCallType.SelectedIndex;
			string text = this.method_1();
			int num = Convert.ToInt32(this.method_1());
			if (selectedIndex != 0 && selectedIndex != 1)
			{
				return;
			}
			int num2 = Convert.ToInt32(this.txtCallId.Text);
			if (num2 < 1 || num2 > 16776415)
			{
				e.Cancel = true;
				MessageBox.Show(Settings.dicCommon["IdOutOfRange"]);
				this.txtCallId.Focus();
				this.txtCallId.SelectAll();
				base.Activate();
			}
			string text2 = this.txtCallId.Text;//.PadLeft(8, '0');
			int repeaterSlot = ContactForm.data[index].RepeaterSlot;

			if (ContactForm.data.CallIdExist(index, selectedIndex, text2, repeaterSlot))
			{
				e.Cancel = true;
				MessageBox.Show(Settings.dicCommon["IdAlreadyExists"]);
				this.txtCallId.Focus();
				this.txtCallId.SelectAll();
				base.Activate();
			}
			if (e.Cancel)
			{
				if (num >= 1 && num <= 16776415 && !ContactForm.data.CallIdExist(index, selectedIndex, text, repeaterSlot))
				{
					this.txtCallId.Text = text;
				}
				else
				{
					this.txtCallId.Text = ContactForm.data.GetMinCallID(selectedIndex);
				}
			}
			else
			{
				this.txtCallId.Text = text2;
			}
			ContactForm.data.SetCallID(index, this.txtCallId.Text);
			((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType(), index);
		}

		private void txtCallId_Leave(object sender, EventArgs e)
		{
			if (this.txtCallId.Text.Length < 8)
			{
				string text = this.txtCallId.Text;//.PadLeft(8, '0');
				this.txtCallId.Text = text;
			}
		}

		private void txtCallId_Enter(object sender, EventArgs e)
		{
			this.method_2(this.txtCallId.Text);
		}

		private void chkCallRxTone_CheckedChanged(object sender, EventArgs e)
		{
			int index = Convert.ToInt32(base.Tag);
			ContactForm.data.SetCallRxTone(index, this.chkCallRxTone.Checked);
			((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType(), index);
		}

		private void cmbRingStyle_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = Convert.ToInt32(base.Tag);
			ContactForm.data.SetRingStyle(index, this.cmbRingStyle.SelectedIndex);
			((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType(), index);
		}

		static ContactForm()
		{
			
			ContactForm.SZ_CALL_TYPE = new string[3]
			{
				"Group Call",
				"Private Call",
				"All Call"
			};
			ContactForm.SZ_CALL_RX_TONE = new string[2]
			{
				"Off",
				"On"
			};
			ContactForm.data = new Contact();
		}
	}
}
