using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class VfoForm : DockContent, IDisp
	{

		public enum ChModeE
		{
			Analog,
			Digital
		}

		private enum RefFreqE
		{
			Low,
			Middle,
			High
		}

		private enum AdmitCritericaE
		{
			Always,
			ChFree,
			CtcssDcs,
			ColorCode = 2,
			CorectPl
		}

		private enum BandwidthE
		{
			Band12_5,
			Band25
		}

		private enum SquelchE
		{
			Tight,
			Normal
		}

		private enum VoiceEmphasisE
		{
			None,
			DeEmphasisAndPreEmphasis,
			DeEmphasis,
			PreEmphasis
		}

		private enum SteE
		{
			Frequency,
			Ste120,
			Ste180,
			Ste240
		}

		private enum NoneSteE
		{
			Off,
			Frequency
		}

		private enum SignalingSystemE
		{
			Off,
			Dtmf
		}

		private enum UnmuteRuleE
		{
			StdUnmuteMute,
			AndUnmuteMute,
			AndUnmuteOrMute
		}

		private enum PttidTypeE
		{
			None,
			Front,
			Post,
			FrontAndPost
		}

		private enum TimingPreference
		{
			First,
			Qualified,
			Unqualified
		}

		public class ChModeChangeEventArgs : EventArgs
		{
			public int ChIndex
			{
				get;
				set;
			}

			public int ChMode
			{
				get;
				set;
			}

			public ChModeChangeEventArgs(int chIndex, int chMode)
			{
				
				//base._002Ector();
				this.ChIndex = chIndex;
				this.ChMode = chMode;
			}
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct ChannelOne : IVerify<ChannelOne>
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			private byte[] name;

			private uint rxFreq;

			private uint txFreq;

			private byte chMode;

			private byte rxRefFreq;

			private byte txRefFreq;

			private byte tot;

			private byte totRekey;

			private byte admitCriteria;

			private byte rssiThreshold;

			private byte scanList;

			private ushort rxTone;

			private ushort txTone;

			private byte voiceEmphasis;

			private byte txSignaling;

			private byte unmuteRule;

			private byte rxSignaling;

			private byte artsInterval;

			private byte encrypt;

			private byte rxColor;

			private byte rxGroupList;

			private byte txColor;

			private byte emgSystem;

			private ushort contact;

			private byte flag1;

			private byte flag2;

			private byte flag3;

			private byte flag4;

			private ushort offsetFreq;

			private byte flag5;

			private byte sql;

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

			public string RxFreq
			{
				get
				{
					try
					{
						uint num = 0u;
						string s = string.Format("{0:x}", this.rxFreq);
						double double_ = (double)int.Parse(s) / 100000.0;
						if (Settings.smethod_19(double_, ref num) == -1)
						{
							double_ = (double)num;
						}
						return double_.ToString("f5");
					}
					catch (Exception)
					{
						return "";
					}
				}
				set
				{
					try
					{
						string s = value.Replace(".", "");
						this.rxFreq = uint.Parse(s, NumberStyles.HexNumber);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
						this.rxFreq = 4294967295u;
					}
				}
			}

			public uint UInt32_0
			{
				get
				{
					return this.rxFreq;
				}
				set
				{
					this.rxFreq = value;
				}
			}

			public uint RxFreqDec
			{
				get
				{
					return Settings.smethod_34(this.rxFreq);
				}
				set
				{
					this.rxFreq = Settings.smethod_35(value);
				}
			}

			public string TxFreq
			{
				get
				{
					try
					{
						uint num = 0u;
						string s = string.Format("{0:x}", this.txFreq);
						double double_ = (double)int.Parse(s) / 100000.0;
						if (Settings.smethod_19(double_, ref num) == -1)
						{
							double_ = (double)num;
						}
						return double_.ToString("f5");
					}
					catch (Exception)
					{
						return "";
					}
				}
				set
				{
					try
					{
						string s = value.Replace(".", "");
						this.txFreq = uint.Parse(s, NumberStyles.HexNumber);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
						this.rxFreq = 4294967295u;
					}
				}
			}

			public uint UInt32_1
			{
				get
				{
					return this.txFreq;
				}
				set
				{
					this.txFreq = value;
				}
			}

			public uint TxFreqDec
			{
				get
				{
					return Settings.smethod_34(this.txFreq);
				}
				set
				{
					this.txFreq = Settings.smethod_35(value);
				}
			}

			public int ChMode
			{
				get
				{
					if (Enum.IsDefined(typeof(ChModeE), (int)this.chMode))
					{
						return this.chMode;
					}
					return 0;
				}
				set
				{
					if (Enum.IsDefined(typeof(ChModeE), value))
					{
						this.chMode = (byte)value;
					}
				}
			}

			public int RxRefFreq
			{
				get
				{
					if (Enum.IsDefined(typeof(RefFreqE), (int)this.rxRefFreq))
					{
						return this.rxRefFreq;
					}
					return 0;
				}
				set
				{
					if (Enum.IsDefined(typeof(RefFreqE), value))
					{
						this.rxRefFreq = (byte)value;
					}
				}
			}

			public int TxRefFreq
			{
				get
				{
					if (Enum.IsDefined(typeof(RefFreqE), (int)this.txRefFreq))
					{
						return this.txRefFreq;
					}
					return 0;
				}
				set
				{
					if (Enum.IsDefined(typeof(RefFreqE), value))
					{
						this.txRefFreq = (byte)value;
					}
				}
			}

			public decimal Tot
			{
				get
				{
					if (this.tot >= 0 && this.tot <= 33)
					{
						return this.tot * 15;
					}
					return 0m;
				}
				set
				{
					int num = (int)(value / 15m);
					if (num >= 0 && num <= 33)
					{
						this.tot = (byte)num;
					}
					else
					{
						this.tot = 0;
					}
				}
			}

			public decimal TotRekey
			{
				get
				{
					if (this.totRekey >= 0 && this.totRekey <= 255)
					{
						return (int)this.totRekey;
					}
					return 0m;
				}
				set
				{
					int num = (int)(value / 1m);
					if (num >= 0 && num <= 255)
					{
						this.totRekey = (byte)num;
					}
					else
					{
						this.totRekey = 0;
					}
				}
			}

			public int AdmitCriteria
			{
				get
				{
					if (Enum.IsDefined(typeof(AdmitCritericaE), (int)this.admitCriteria))
					{
						return this.admitCriteria;
					}
					return 0;
				}
				set
				{
					if (Enum.IsDefined(typeof(AdmitCritericaE), value))
					{
						this.admitCriteria = (byte)value;
					}
				}
			}

			public decimal RssiThreshold
			{
				get
				{
					if (this.rssiThreshold >= 80 && this.rssiThreshold <= 124)
					{
						return this.rssiThreshold * -1;
					}
					return -80m;
				}
				set
				{
					decimal num = value * -1m;
					if (num >= 80m && num <= 124m)
					{
						this.rssiThreshold = (byte)num;
					}
					else
					{
						this.rssiThreshold = 80;
					}
				}
			}

			public int ScanList
			{
				get
				{
					return this.scanList;
				}
				set
				{
					if (value <= NormalScanForm.data.Count)
					{
						this.scanList = (byte)value;
					}
				}
			}

			public string RxTone
			{
				get
				{
					if (this.rxTone != 65535 && this.rxTone != 0)
					{
						if ((this.rxTone & 0xC000) == 49152)
						{
							return string.Format("D{0:x3}I", this.rxTone & 0xFFF);
						}
						if ((this.rxTone & 0xC000) == 32768)
						{
							return string.Format("D{0:x3}N", this.rxTone & 0xFFF);
						}
						string text = string.Format("{0:x}", this.rxTone);
						return text.Insert(text.Length - 1, ".");
					}
					return Settings.SZ_NONE;
				}
				set
				{
					string text = "";
					this.rxTone = 65535;
					if (!string.IsNullOrEmpty(value) && value != Settings.SZ_NONE)
					{
						string pattern = "D[0-7]{3}N$";
						Regex regex = new Regex(pattern);
						if (regex.IsMatch(value))
						{
							text = value.Substring(1, 3);
							this.rxTone = (ushort)(ushort.Parse(text, NumberStyles.HexNumber) + 32768);
						}
						else
						{
							pattern = "D[0-7]{3}I$";
							regex = new Regex(pattern);
							if (regex.IsMatch(value))
							{
								text = value.Substring(1, 3);
								this.rxTone = (ushort)(ushort.Parse(text, NumberStyles.HexNumber) + 49152);
							}
							else
							{
								double num = double.Parse(value);
								if (num > 60.0 && num < 260.0)
								{
									text = value.Replace(".", "");
									this.rxTone = ushort.Parse(text, NumberStyles.HexNumber);
								}
							}
						}
					}
				}
			}

			public string TxTone
			{
				get
				{
					if (this.txTone != 65535 && this.txTone != 0)
					{
						if ((this.txTone & 0xC000) == 49152)
						{
							return string.Format("D{0:x3}I", this.txTone & 0xFFF);
						}
						if ((this.txTone & 0xC000) == 32768)
						{
							return string.Format("D{0:x3}N", this.txTone & 0xFFF);
						}
						string text = string.Format("{0:x}", this.txTone);
						return text.Insert(text.Length - 1, ".");
					}
					return Settings.SZ_NONE;
				}
				set
				{
					string text = "";
					this.txTone = 65535;
					if (!string.IsNullOrEmpty(value) && value != Settings.SZ_NONE)
					{
						string pattern = "D[0-7]{3}N$";
						Regex regex = new Regex(pattern);
						if (regex.IsMatch(value))
						{
							text = value.Substring(1, 3);
							this.txTone = (ushort)(ushort.Parse(text, NumberStyles.HexNumber) + 32768);
						}
						else
						{
							pattern = "D[0-7]{3}I$";
							regex = new Regex(pattern);
							if (regex.IsMatch(value))
							{
								text = value.Substring(1, 3);
								this.txTone = (ushort)(ushort.Parse(text, NumberStyles.HexNumber) + 49152);
							}
							else
							{
								double num = double.Parse(value);
								if (num > 60.0 && num < 260.0)
								{
									text = value.Replace(".", "");
									this.txTone = ushort.Parse(text, NumberStyles.HexNumber);
								}
							}
						}
					}
				}
			}

			public int VoiceEmphasis
			{
				get
				{
					if (Enum.IsDefined(typeof(VoiceEmphasisE), (int)this.voiceEmphasis))
					{
						return this.voiceEmphasis;
					}
					return 0;
				}
				set
				{
					if (Enum.IsDefined(typeof(VoiceEmphasisE), value))
					{
						this.voiceEmphasis = (byte)value;
					}
				}
			}

			public int TxSignaling
			{
				get
				{
					if (Enum.IsDefined(typeof(SignalingSystemE), (int)this.txSignaling))
					{
						return this.txSignaling;
					}
					return 0;
				}
				set
				{
					if (Enum.IsDefined(typeof(SignalingSystemE), value))
					{
						this.txSignaling = (byte)value;
					}
				}
			}

			public int UnmuteRule
			{
				get
				{
					if (Enum.IsDefined(typeof(UnmuteRuleE), (int)this.unmuteRule))
					{
						return this.unmuteRule;
					}
					return 0;
				}
				set
				{
					if (Enum.IsDefined(typeof(UnmuteRuleE), value))
					{
						this.unmuteRule = (byte)value;
					}
				}
			}

			public int RxSignaling
			{
				get
				{
					if (Enum.IsDefined(typeof(SignalingSystemE), (int)this.rxSignaling))
					{
						return this.rxSignaling;
					}
					return 0;
				}
				set
				{
					if (Enum.IsDefined(typeof(SignalingSystemE), value))
					{
						this.rxSignaling = (byte)value;
					}
				}
			}

			public decimal ArtsInterval
			{
				get
				{
					if (this.artsInterval >= 22 && this.artsInterval <= 55)
					{
						return this.artsInterval;
					}
					return 22m;
				}
				set
				{
					byte b = (byte)(value * 1m);
					if (b >= 22 && b <= 55)
					{
						this.artsInterval = b;
					}
					else
					{
						this.artsInterval = 22;
					}
				}
			}

			public int Key
			{
				get
				{
					if (this.encrypt <= EncryptForm.data.Count)
					{
						return this.encrypt;
					}
					return 0;
				}
				set
				{
					if (value <= EncryptForm.data.Count)
					{
						this.encrypt = (byte)value;
					}
				}
			}

			public decimal RxColor
			{
				get
				{
					if (this.rxColor >= 0 && this.rxColor <= 15)
					{
						return this.rxColor;
					}
					return 0m;
				}
				set
				{
					if (value >= 0m && value <= 15m)
					{
						this.rxColor = (byte)value;
					}
				}
			}

			public int RxGroupList
			{
				get
				{
					return this.rxGroupList;
				}
				set
				{
					if (value <= RxGroupListForm.data.Count)
					{
						this.rxGroupList = (byte)value;
					}
				}
			}

			public decimal TxColor
			{
				get
				{
					if (this.txColor >= 0 && this.txColor <= 15)
					{
						return this.txColor;
					}
					return 0m;
				}
				set
				{
					if (value >= 0m && value <= 15m)
					{
						this.txColor = (byte)value;
						this.rxColor = (byte)value;
					}
				}
			}

			public int EmgSystem
			{
				get
				{
					if (this.emgSystem <= EmergencyForm.data.Count)
					{
						return this.emgSystem;
					}
					return 0;
				}
				set
				{
					if (value <= EmergencyForm.data.Count)
					{
						this.emgSystem = (byte)value;
					}
				}
			}

			public int Contact
			{
				get
				{
					if (this.contact <= ContactForm.data.Count)
					{
						return this.contact;
					}
					return 0;
				}
				set
				{
					if (value <= ContactForm.data.Count)
					{
						this.contact = (byte)value;
					}
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

			public byte Flag2
			{
				get
				{
					return this.flag2;
				}
				set
				{
					this.flag2 = value;
				}
			}

			public byte Flag3
			{
				get
				{
					return this.flag3;
				}
				set
				{
					this.flag3 = value;
				}
			}

			public byte Flag4
			{
				get
				{
					return this.flag4;
				}
				set
				{
					this.flag4 = value;
				}
			}

			public int Power
			{
				get
				{
					return (this.flag4 & 0x80) >> 7;
				}
				set
				{
					this.flag4 &= 127;
					value = (value << 7 & 0x80);
					this.flag4 |= (byte)value;
				}
			}

			public bool Vox
			{
				get
				{
					return Convert.ToBoolean(this.flag4 & 0x40);
				}
				set
				{
					if (value)
					{
						this.flag4 |= 64;
					}
					else
					{
						this.flag4 &= 191;
					}
				}
			}

			public bool AutoScan
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

			public bool LoneWoker
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

			public bool AllowTalkaround
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

			public bool OnlyRx
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

			public int Bandwidth
			{
				get
				{
					return (this.flag4 & 2) >> 1;
				}
				set
				{
					this.flag4 &= 253;
					value = (value << 1 & 2);
					this.flag4 |= (byte)value;
				}
			}

			public int Squelch
			{
				get
				{
					return this.flag4 & 1;
				}
				set
				{
					this.flag4 &= 254;
					this.flag4 |= (byte)(value & 1);
				}
			}

			public int Ste
			{
				get
				{
					return (this.flag3 & 0xC0) >> 6;
				}
				set
				{
					value = (value << 6 & 0xC0);
					this.flag3 &= 63;
					this.flag3 |= (byte)value;
				}
			}

			public int NonSte
			{
				get
				{
					return (this.flag3 & 0x20) >> 5;
				}
				set
				{
					value = (value << 5 & 0x20);
					this.flag3 &= 223;
					this.flag3 |= (byte)value;
				}
			}

			public bool DataPl
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

			public int PttidType
			{
				get
				{
					return (this.flag3 & 0xC) >> 2;
				}
				set
				{
					value = (value << 2 & 0xC);
					this.flag3 &= 243;
					this.flag3 |= (byte)value;
				}
			}

			public bool DualCapacity
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

			public int TimingPreference
			{
				get
				{
					return (this.flag2 & 0x80) >> 7;
				}
				set
				{
					value = (value << 7 & 0x80);
					this.flag2 &= 127;
					this.flag2 |= (byte)value;
				}
			}

			public int RepateSlot
			{
				get
				{
					return (this.flag2 & 0x40) >> 6;
				}
				set
				{
					value = (value << 6 & 0x40);
					this.flag2 &= 191;
					this.flag2 |= (byte)value;
				}
			}

			public int Ars
			{
				get
				{
					return (this.flag2 & 0x20) >> 5;
				}
				set
				{
					value = (value << 5 & 0x20);
					this.flag2 &= 223;
					this.flag2 |= (byte)value;
				}
			}

			public int KeySwitch
			{
				get
				{
					return (this.flag2 & 0x10) >> 4;
				}
				set
				{
					value = (value << 4 & 0x10);
					this.flag2 &= 239;
					this.flag2 |= (byte)value;
				}
			}

			public bool UdpDataHead
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

			public bool AllowTxInterupt
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

			public bool TxInteruptFreq
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

			public bool PrivateCall
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

			public bool DataCall
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

			public bool EmgConfirmed
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

			public bool EnchancedChAccess
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

			public int BandType
			{
				get
				{
					return (this.flag1 & 4) >> 2;
				}
				set
				{
					value = (value << 2 & 4);
					this.flag1 &= 251;
					this.flag1 |= (byte)value;
				}
			}

			public int Arts
			{
				get
				{
					return this.flag1 & 3;
				}
				set
				{
					value &= 3;
					this.flag1 &= 252;
					this.flag1 |= (byte)value;
				}
			}

			public decimal OffsetFreq
			{
				get
				{
					if (this.offsetFreq >= 1 && this.offsetFreq <= 38400)
					{
						return (decimal)this.offsetFreq * 0.01m;
					}
					return 10.00m;
				}
				set
				{
					int num = Convert.ToInt32(value / 0.01m);
					if (num >= 1 && num <= 38400)
					{
						this.offsetFreq = (ushort)num;
					}
					else
					{
						this.offsetFreq = 1000;
					}
				}
			}

			public int OffsetStep
			{
				get
				{
					int num = this.flag5 >> 4;
					if (num >= VfoForm.SZ_OFFSET_STEP.Length)
					{
						return 0;
					}
					return num;
				}
				set
				{
					value &= 0xF;
					this.flag5 &= 15;
					this.flag5 |= (byte)(value << 4);
				}
			}

			public int OffsetDirection
			{
				get
				{
					int num = (this.flag5 & 0xC) >> 2;
					if (num >= VfoForm.SZ_OFFSET_DIRECTION.Length)
					{
						return 0;
					}
					return num;
				}
				set
				{
					value &= 3;
					this.flag5 &= 243;
					this.flag5 |= (byte)(value << 2);
				}
			}

			public int Sql
			{
				get
				{
					if (this.sql >= 0 && this.sql <= 21)
					{
						return this.sql;
					}
					return 0;
				}
				set
				{
					if (value >= 0 && this.sql <= 21)
					{
						this.sql = (byte)value;
					}
				}
			}

			public ChannelOne(int index)
			{
				
				this = default(ChannelOne);
				this.name = new byte[16];
			}

			public void Default()
			{
				this.ChMode = VfoForm.DefaultCh.ChMode;
				this.RxRefFreq = VfoForm.DefaultCh.RxRefFreq;
				this.Tot = VfoForm.DefaultCh.Tot;
				this.TotRekey = VfoForm.DefaultCh.TotRekey;
				this.AdmitCriteria = VfoForm.DefaultCh.AdmitCriteria;
				this.RssiThreshold = VfoForm.DefaultCh.RssiThreshold;
				this.ScanList = VfoForm.DefaultCh.ScanList;
				this.RxTone = VfoForm.DefaultCh.RxTone;
				this.TxTone = VfoForm.DefaultCh.TxTone;
				this.VoiceEmphasis = VfoForm.DefaultCh.VoiceEmphasis;
				this.TxSignaling = VfoForm.DefaultCh.TxSignaling;
				this.UnmuteRule = VfoForm.DefaultCh.UnmuteRule;
				this.RxSignaling = VfoForm.DefaultCh.RxSignaling;
				this.ArtsInterval = VfoForm.DefaultCh.ArtsInterval;
				this.Key = VfoForm.DefaultCh.Key;
				this.RxColor = VfoForm.DefaultCh.RxColor;
				this.TxColor = VfoForm.DefaultCh.TxColor;
				this.EmgSystem = VfoForm.DefaultCh.EmgSystem;
				this.Contact = VfoForm.DefaultCh.Contact;
				this.Flag1 = VfoForm.DefaultCh.Flag1;
				this.Flag2 = VfoForm.DefaultCh.Flag2;
				this.Flag3 = VfoForm.DefaultCh.Flag3;
				this.Flag4 = VfoForm.DefaultCh.Flag4;
				this.Sql = VfoForm.DefaultCh.Sql;
			}

			public ChannelOne Clone()
			{
				return Settings.smethod_65(this);
			}

			public void Verify(ChannelOne def)
			{
				if (this.OffsetDirection >= VfoForm.SZ_OFFSET_DIRECTION.Length)
				{
					this.OffsetDirection = def.OffsetDirection;
				}
				if (this.OffsetStep >= VfoForm.SZ_OFFSET_STEP.Length)
				{
					this.OffsetStep = def.OffsetStep;
				}
				if (!Enum.IsDefined(typeof(ChModeE), this.ChMode))
				{
					this.ChMode = def.ChMode;
				}
				if (!Enum.IsDefined(typeof(RefFreqE), this.TxRefFreq))
				{
					this.TxRefFreq = def.TxRefFreq;
				}
				if (!Enum.IsDefined(typeof(RefFreqE), this.RxRefFreq))
				{
					this.RxRefFreq = def.RxRefFreq;
				}
				Settings.smethod_11(ref this.tot, (byte)0, (byte)33, def.tot);
				Settings.smethod_11(ref this.rssiThreshold, (byte)80, (byte)124, def.rssiThreshold);
				if (this.scanList != 0 && this.scanList <= 64)
				{
					if (!NormalScanForm.data.DataIsValid(this.scanList - 1))
					{
						this.scanList = 0;
					}
				}
				else
				{
					this.scanList = 0;
				}
				if (!Enum.IsDefined(typeof(VoiceEmphasisE), this.VoiceEmphasis))
				{
					this.VoiceEmphasis = def.VoiceEmphasis;
				}
				if (!Enum.IsDefined(typeof(UnmuteRuleE), this.UnmuteRule))
				{
					this.UnmuteRule = def.UnmuteRule;
				}
				if (!Enum.IsDefined(typeof(SignalingSystemE), this.TxSignaling))
				{
					this.TxSignaling = def.TxSignaling;
				}
				if (!Enum.IsDefined(typeof(SignalingSystemE), this.RxSignaling))
				{
					this.RxSignaling = def.RxSignaling;
				}
				Settings.smethod_11(ref this.artsInterval, (byte)22, (byte)55, def.artsInterval);
				if (!Enum.IsDefined(typeof(TimingPreference), this.TimingPreference))
				{
					this.TimingPreference = def.TimingPreference;
				}
				if (this.encrypt != 0 && !EncryptForm.data.DataIsValid(this.encrypt - 1))
				{
					this.encrypt = 0;
				}
				Settings.smethod_11(ref this.rxColor, (byte)0, (byte)15, def.rxColor);
				this.txColor = this.rxColor;
				if (this.rxGroupList != 0 && this.rxGroupList <= RxListData.CNT_RX_LIST)
				{
					if (!RxGroupListForm.data.DataIsValid(this.rxGroupList - 1))
					{
						this.rxGroupList = 0;
					}
				}
				else
				{
					this.rxGroupList = 0;
				}
				if (this.emgSystem != 0 && this.emgSystem <= 32)
				{
					if (!EmergencyForm.data.DataIsValid(this.emgSystem - 1))
					{
						this.emgSystem = 0;
					}
				}
				else
				{
					this.emgSystem = 0;
				}
				string pattern = "D[0-7]{3}[N|I]$";
				Regex regex = new Regex(pattern);
				if (regex.IsMatch(this.RxTone))
				{
					this.Ste = 0;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class Vfo : IData
		{
			public delegate void ChModeDelegate(object sender, ChModeChangeEventArgs e);

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			private ChannelOne[] chList;

			public ChannelOne this[int index]
			{
				get
				{
					if (index >= 2)
					{
						throw new ArgumentOutOfRangeException();
					}
					return this.chList[index];
				}
				set
				{
					if (index >= 2)
					{
						throw new ArgumentOutOfRangeException();
					}
					this.chList[index] = value;
				}
			}

			public int Count
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public string Format
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public bool ListIsEmpty
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public event ChModeDelegate ChModeChangeEvent;

			public Vfo()
			{
				
				//base._002Ector();
				int num = 0;
				this.chList = new ChannelOne[2];
				for (num = 0; num < this.chList.Length; num++)
				{
					this.chList[num] = new ChannelOne(num);
				}
			}

			public void Verify()
			{
				int num = 0;
				uint num2 = 0u;
				uint num3 = 0u;
				uint num4 = 0u;
				for (num = 0; num < 2; num++)
				{
					num2 = this.chList[num].RxFreqDec / 100000u;
					if (Settings.smethod_19((double)num2, ref num4) < 0)
					{
						num2 = num4 * 100000;
						this.chList[num].RxFreqDec = num2;
					}
					num3 = this.chList[num].TxFreqDec / 100000u;
					if (Settings.smethod_19((double)num3, ref num4) < 0)
					{
						this.chList[num].TxFreqDec = this.chList[num].RxFreqDec;
					}
					switch (this.chList[num].OffsetDirection)
					{
					case 0:
						this.chList[num].TxFreqDec = this.chList[num].RxFreqDec;
						break;
					case 1:
						num3 = this.chList[num].RxFreqDec - (uint)this.chList[num].OffsetFreq;
						num3 /= 100000u;
						if (Settings.smethod_19((double)num3, ref num4) < 0)
						{
							this.chList[num].OffsetDirection = 0;
						}
						break;
					case 2:
						num3 = this.chList[num].RxFreqDec + (uint)this.chList[num].OffsetFreq;
						num3 /= 100000u;
						if (Settings.smethod_19((double)num3, ref num4) < 0)
						{
							this.chList[num].OffsetDirection = 0;
						}
						break;
					}
					this.chList[num].Verify(VfoForm.DefaultCh);
				}
			}

			public void SetDefaultFreq(int index)
			{
				this.chList[index].UInt32_0 = Settings.smethod_35(Settings.MIN_FREQ[0] * 100000);
				this.chList[index].UInt32_1 = this.chList[index].UInt32_0;
			}

			public byte[] ToEerom()
			{
				int num = 0;
				int num2 = 0;
				byte[] array = new byte[2 * VfoForm.SPACE_CH];
				for (num = 0; num < 2; num++)
				{
					byte[] array2 = Settings.objectToByteArray(this.chList[num], Marshal.SizeOf(this.chList[num]));
					Array.Copy(array2, 0, array, num2, array2.Length);
					num2 += array2.Length;
				}
				return array;
			}

			public void FromEerom(byte[] data)
			{
				int num = 0;
				int num2 = 0;
				for (num = 0; num < 2; num++)
				{
					byte[] array = new byte[VfoForm.SPACE_CH];
					Array.Copy(data, num2, array, 0, array.Length);
					this.chList[num] = (ChannelOne)Settings.smethod_62(array, typeof(ChannelOne));

                    // Roger Clark. Workaround for bug in firmware 3.0.6 where Dual Direct Capacity Mode must NOT be enabled in Analog mode
                    if (this.chList[num].ChMode == (int)ChModeE.Analog && this.chList[num].DualCapacity == true)
                    {
                        this.chList[num].DualCapacity = false;
                    }

					num2 += array.Length;
				}
			}

			public int GetMinIndex()
			{
				throw new NotImplementedException();
			}

			public string GetMinName(TreeNode node)
			{
				throw new NotImplementedException();
			}

			public bool DataIsValid(int index)
			{
				throw new NotImplementedException();
			}

			public void SetIndex(int index, int value)
			{
				throw new NotImplementedException();
			}

			public void ClearIndex(int index)
			{
				throw new NotImplementedException();
			}

			public void SetName(int index, string text)
			{
				throw new NotImplementedException();
			}

			public string GetName(int index)
			{
				throw new NotImplementedException();
			}

			public void Default(int index)
			{
				throw new NotImplementedException();
			}

			public void Paste(int from, int to)
			{
				throw new NotImplementedException();
			}

			public int GetChMode(int index)
			{
				return this.chList[index].ChMode;
			}
		}

		public const int CNT_VFO_CH = 2;
		public const int LEN_CH_NAME = 16;
		public const string SZ_CH_MODE_NAME = "ChMode";
		private const int LEN_FREQ = 9;
		private const int SCL_FREQ = 100000;
		public const string SZ_REF_FREQ_NAME = "RefFreq";
		public const string SZ_POWER_NAME = "Power";
		private const string SZ_INFINITE = "无穷";
		private const int MIN_TOT = 0;
		private const int MAX_TOT = 33;
		private const int INC_TOT = 1;
		private const int SCL_TOT = 15;
		private const int LEN_TOT = 3;
		private const int MIN_TOT_REKEY = 0;
		private const int MAX_TOT_REKEY = 255;
		private const int INC_TOT_REKEY = 1;
		private const int SCL_TOT_REKEY = 1;
		private const int LEN_TOT_REKEY = 3;
		public const string SZ_ADMIT_CRITERICA_NAME = "AdmitCriterica";
		public const string SZ_ADMIT_CRITERICA_D_NAME = "AdmitCritericaD";
		private const int MIN_RSSI_THRESHOLD = 80;
		private const int MAX_RSSI_THRESHOLD = 124;
		private const int INC_RSSI_THRESHOLD = 1;
		private const int SCL_RSSI_THRESHOLD = -1;
		private const int LEN_RSSI_THRESHOLD = 4;
		public const string SZ_SQUELCH_NAME = "Squelch";
		public const string SZ_SQUELCH_LEVEL_NAME = "SquelchLevel";
		public const string SZ_VOICE_EMPHASIS_NAME = "VoiceEmphasis";
		public const string SZ_STE_NAME = "Ste";
		public const string SZ_NON_STE_NAME = "NonSte";
		public const string SZ_SIGNALING_SYSTEM_NAME = "SignalingSystem";
		public const string SZ_UNMUTE_RULE_NAME = "UnmuteRule";
		public const string SZ_PTTID_TYPE_NAME = "PttidType";
		public const string SZ_ARTS_NAME = "Arts";
		private const int MIN_COLOR_CODE = 0;
		private const int MAX_COLOR_CODE_DUAL_CAPACITY = 14;
		private const int MAX_COLOR_CODE = 15;
		private const int INC_COLOR_CODE = 1;
		private const int SCL_COLOR_CODE = 1;
		private const int LEN_COLOR_CODE = 2;
		private const int MIN_ARTS_INTERVAL = 22;
		private const int MAX_ARTS_INTERVAL = 55;
		private const int INC_ARTS_INTERVAL = 1;
		private const int SCL_ARTS_INTERVAL = 1;
		private const int LEN_ARTS_INTERVAL = 2;
		public const string SZ_TIMING_PREFERENCE_NAME = "TimingPreference";
		public const string SZ_ARS_NAME = "Ars";
		public const string SZ_KEY_SWITCH_NAME = "KeySwitch";
		public const string SZ_OFFSET_DIRECTION_NAME = "OffsetDirection";
		private const int INC_OFFSET_FREQ = 1;
		public  static decimal SCL_OFFSET_FREQ = 0.01m;
		private const int LEN_OFFSET_FREQ = 6;
		private const int MIN_OFFSET_FREQ = 1;
		private const int MAX_OFFSET_FREQ = 38400;
		private const int DEF_OFFSET_FREQ = 1000;
		private const int SCL_OFFSET_FREQ_MHZ = 1000;
		//private IContainer components;
		private CheckBox chkEnhancedChAccess;
		private CheckBox chkEmgConfirmed;
		private CheckBox chkDataCall;
		private CheckBox chkPrivateCall;
		private CheckBox chkTxInteruptFreq;
		private CheckBox chkAllowTxInterupt;
		private Label lblContact;
		private CustomCombo cmbContact;
		private Label lblEmgSystem;
		private CustomCombo cmbEmgSystem;
		private Label lblTxColor;
		private Label lblRxGroup;

		private CustomCombo cmbRxGroup;

		private Label lblRxColor;

		private CheckBox chkUdpDataHead;

		private Label lblKey;

		private CustomCombo cmbKey;

		private Label lblKeySwitch;

		private ComboBox cmbKeySwitch;

		private Label lblArs;

		private ComboBox cmbArs;

		private Label lblRepeaterSlot;

		private ComboBox cmbRepeaterSlot;

		private Label lblTimingPreference;

		private ComboBox cmbTimingPreference;

		private CheckBox chkDualCapacity;

		private Label lblTxTone;

		private ComboBox cmbTxTone;

		private Label lblTxSignaling;

		private ComboBox cmbTxSignaling;

		private Label lblPttidType;

		private ComboBox cmbPttidType;

		private Label lblArtsInterval;

		private CustomNumericUpDown nudArtsInterval;

		private CheckBox chkDataPl;

		private Label lblUnmuteRule;

		private ComboBox cmbUnmuteRule;

		private Label lblRxSignaling;

		private ComboBox cmbRxSignaling;

		private Label lblRxTone;

		private ComboBox cmbRxTone;

		private Label lblNonSte;

		private ComboBox cmbNonSte;

		private Label lblSte;

		private ComboBox cmbSte;

		private Label lblVoiceEmphasis;

		private ComboBox cmbVoiceEmphasis;

		private Label lblSquelch;

		private ComboBox cmbSquelch;

		private Label lblChBandwidth;

		private ComboBox cmbChBandwidth;

		private Label lblChMode;

		private ComboBox cmbChMode;

		private Label lblChName;

		private SGTextBox txtName;

		private Label lblRxFreq;

		private TextBox txtRxFreq;

		private Label lblRxRefFreq;

		private ComboBox cmbRxRefFreq;

		private Label lblTxRefFreq;

		private Label lblTxFreq;

		private ComboBox cmbTxRefFreq;

		private TextBox txtTxFreq;

		private Label lblPower;

		private ComboBox cmbPower;

		private Label lblTot;

		private CustomNumericUpDown nudTot;

		private Label lblTotRekey;

		private CustomNumericUpDown nudTotRekey;

		private CheckBox chkVox;

		private Label lblAdmitCriteria;

		private ComboBox cmbAdmitCriteria;

		private Label lblRssiThreshold;

		private CustomNumericUpDown nudRssiThreshold;

		private Label lblScanList;

		private CustomCombo cmbScanList;

		private CheckBox chkAutoScan;

		private CheckBox chkLoneWoker;

		private CheckBox chkAllowTalkaround;

		private CheckBox chkRxOnly;

		private DoubleClickGroupBox grpAnalog;

		private DoubleClickGroupBox grpDigit;

		private CustomNumericUpDown nudTxColor;

		private CustomNumericUpDown nudRxColor;

		private CustomCombo cmbArts;

		private Label lblArts;

		private ComboBox cmbSql;

		private Label lblSql;

		private MenuStrip mnsCh;

		private ToolStripMenuItem tsmiCh;

		private ToolStripMenuItem tsmiFirst;

		private ToolStripMenuItem tsmiPrev;

		private ToolStripMenuItem tsmiNext;

		private ToolStripMenuItem tsmiLast;

		private ToolStripMenuItem tsmiAdd;

		private ToolStripMenuItem tsmiDel;

		private CustomPanel pnlChannel;

		private Button btnCopy;

		private ComboBox cmbOffsetDirection;

		private Label lblOffsetDirection;

		private ComboBox cmbOffsetStep;

		private Label lblOffsetStep;

		private CustomNumericUpDown nudOffsetFreq;

		private Label lblOffsetFreq;

		private Label lblBandType;

		private ComboBox cmbBandType;

		public static readonly int SPACE_CH;

		public static readonly string[] SZ_CH_MODE;

		private static readonly string[] SZ_REF_FREQ;

		public static readonly string[] SZ_POWER;

		private static readonly string[] SZ_BAND_TYPE;

		private static readonly string[] SZ_ADMIT_CRITERICA;

		private static readonly string[] SZ_ADMIT_CRITERICA_D;

		private static readonly string[] SZ_BANDWIDTH;

		private static readonly string[] SZ_SQUELCH;
		private static readonly string[] SZ_SQUELCH_LEVEL;

		private static readonly string[] SZ_VOICE_EMPHASIS;

		private static readonly string[] SZ_STE;

		private static readonly string[] SZ_NON_STE;

		private static readonly string[] SZ_SIGNALING_SYSTEM;

		private static readonly string[] SZ_UNMUTE_RULE;

		private static readonly string[] SZ_PTTID_TYPE;

		private static readonly string[] SZ_ARTS;

		private static readonly string[] SZ_TIMING_PREFERENCE;

		private static readonly string[] SZ_REPEATER_SOLT;

		private static readonly string[] SZ_ARS;

		private static readonly string[] SZ_KEY_SWITCH;

		private static readonly string[] SZ_OFFSET_DIRECTION;

		private static readonly string[] SZ_OFFSET_STEP;

		public static ChannelOne DefaultCh;

		public static Vfo data;

		public static int CurCntCh
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
			this.mnsCh = new MenuStrip();
			this.tsmiCh = new ToolStripMenuItem();
			this.tsmiFirst = new ToolStripMenuItem();
			this.tsmiPrev = new ToolStripMenuItem();
			this.tsmiNext = new ToolStripMenuItem();
			this.tsmiLast = new ToolStripMenuItem();
			this.tsmiAdd = new ToolStripMenuItem();
			this.tsmiDel = new ToolStripMenuItem();
			this.pnlChannel = new CustomPanel();
			this.nudOffsetFreq = new CustomNumericUpDown();
			this.lblOffsetFreq = new Label();
			this.cmbOffsetDirection = new ComboBox();
			this.lblOffsetDirection = new Label();
			this.cmbOffsetStep = new ComboBox();
			this.lblOffsetStep = new Label();
			this.btnCopy = new Button();
			this.cmbScanList = new CustomCombo();
			this.txtName = new SGTextBox();
			this.cmbSquelch = new ComboBox();
			this.cmbSql = new ComboBox();
			this.cmbAdmitCriteria = new ComboBox();
			this.lblSquelch = new Label();
			this.nudRssiThreshold = new CustomNumericUpDown();
			this.lblSql = new Label();
			this.grpDigit = new DoubleClickGroupBox();
			this.nudTxColor = new CustomNumericUpDown();
			this.nudRxColor = new CustomNumericUpDown();
			this.cmbTimingPreference = new ComboBox();
			this.cmbRepeaterSlot = new ComboBox();
			this.lblTimingPreference = new Label();
			this.cmbArs = new ComboBox();
			this.lblRepeaterSlot = new Label();
			this.cmbKeySwitch = new ComboBox();
			this.lblArs = new Label();
			this.cmbKey = new CustomCombo();
			this.lblKeySwitch = new Label();
			this.lblKey = new Label();
			this.cmbRxGroup = new CustomCombo();
			this.lblTxColor = new Label();
			this.cmbEmgSystem = new CustomCombo();
			this.lblEmgSystem = new Label();
			this.cmbContact = new CustomCombo();
			this.lblContact = new Label();
			this.chkDualCapacity = new CheckBox();
			this.chkUdpDataHead = new CheckBox();
			this.chkAllowTxInterupt = new CheckBox();
			this.chkTxInteruptFreq = new CheckBox();
			this.chkPrivateCall = new CheckBox();
			this.chkDataCall = new CheckBox();
			this.chkEmgConfirmed = new CheckBox();
			this.chkEnhancedChAccess = new CheckBox();
			this.lblRxColor = new Label();
			this.lblRxGroup = new Label();
			this.chkRxOnly = new CheckBox();
			this.cmbRxRefFreq = new ComboBox();
			this.chkAllowTalkaround = new CheckBox();
			this.grpAnalog = new DoubleClickGroupBox();
			this.nudArtsInterval = new CustomNumericUpDown();
			this.cmbChBandwidth = new ComboBox();
			this.lblChBandwidth = new Label();
			this.cmbVoiceEmphasis = new ComboBox();
			this.cmbSte = new ComboBox();
			this.lblVoiceEmphasis = new Label();
			this.cmbNonSte = new ComboBox();
			this.lblSte = new Label();
			this.cmbRxTone = new ComboBox();
			this.lblNonSte = new Label();
			this.cmbRxSignaling = new ComboBox();
			this.lblRxTone = new Label();
			this.cmbUnmuteRule = new ComboBox();
			this.lblRxSignaling = new Label();
			this.cmbArts = new CustomCombo();
			this.cmbPttidType = new ComboBox();
			this.lblUnmuteRule = new Label();
			this.lblArtsInterval = new Label();
			this.lblArts = new Label();
			this.lblPttidType = new Label();
			this.cmbTxSignaling = new ComboBox();
			this.lblTxSignaling = new Label();
			this.cmbTxTone = new ComboBox();
			this.lblTxTone = new Label();
			this.chkDataPl = new CheckBox();
			this.chkLoneWoker = new CheckBox();
			this.chkVox = new CheckBox();
			this.chkAutoScan = new CheckBox();
			this.cmbChMode = new ComboBox();
			this.lblChName = new Label();
			this.txtTxFreq = new TextBox();
			this.lblChMode = new Label();
			this.lblTot = new Label();
			this.txtRxFreq = new TextBox();
			this.lblTotRekey = new Label();
			this.lblRssiThreshold = new Label();
			this.lblRxRefFreq = new Label();
			this.lblBandType = new Label();
			this.lblTxRefFreq = new Label();
			this.cmbPower = new ComboBox();
			this.lblRxFreq = new Label();
			this.cmbBandType = new ComboBox();
			this.cmbTxRefFreq = new ComboBox();
			this.lblPower = new Label();
			this.lblAdmitCriteria = new Label();
			this.nudTotRekey = new CustomNumericUpDown();
			this.lblTxFreq = new Label();
			this.nudTot = new CustomNumericUpDown();
			this.lblScanList = new Label();
			this.mnsCh.SuspendLayout();
			this.pnlChannel.SuspendLayout();
			((ISupportInitialize)this.nudOffsetFreq).BeginInit();
			((ISupportInitialize)this.nudRssiThreshold).BeginInit();
			this.grpDigit.SuspendLayout();
			((ISupportInitialize)this.nudTxColor).BeginInit();
			((ISupportInitialize)this.nudRxColor).BeginInit();
			this.grpAnalog.SuspendLayout();
			((ISupportInitialize)this.nudArtsInterval).BeginInit();
			((ISupportInitialize)this.nudTotRekey).BeginInit();
			((ISupportInitialize)this.nudTot).BeginInit();
			base.SuspendLayout();
			this.mnsCh.AllowMerge = false;
			this.mnsCh.Items.AddRange(new ToolStripItem[1]
			{
				this.tsmiCh
			});
			this.mnsCh.Location = new Point(0, 0);
			this.mnsCh.Name = "mnsCh";
			this.mnsCh.Size = new Size(1104, 25);
			this.mnsCh.TabIndex = 32;
			this.mnsCh.Text = "menuStrip1";
			this.mnsCh.Visible = false;
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
			this.tsmiCh.Size = new Size(44, 21);
			this.tsmiCh.Text = "操作";
			this.tsmiFirst.Name = "tsmiFirst";
			this.tsmiFirst.Size = new Size(159, 22);
			this.tsmiFirst.Text = "First";
			this.tsmiPrev.Name = "tsmiPrev";
			this.tsmiPrev.Size = new Size(159, 22);
			this.tsmiPrev.Text = "Previous";
			this.tsmiNext.Name = "tsmiNext";
			this.tsmiNext.Size = new Size(159, 22);
			this.tsmiNext.Text = "Next";
			this.tsmiLast.Name = "tsmiLast";
			this.tsmiLast.Size = new Size(159, 22);
			this.tsmiLast.Text = "Last";
			this.tsmiAdd.Name = "tsmiAdd";
			this.tsmiAdd.Size = new Size(159, 22);
			this.tsmiAdd.Text = "Add";
			this.tsmiDel.Name = "tsmiDel";
			this.tsmiDel.Size = new Size(159, 22);
			this.tsmiDel.Text = "Delete";
			this.pnlChannel.AutoScroll = true;
			this.pnlChannel.AutoSize = true;
			this.pnlChannel.Controls.Add(this.nudOffsetFreq);
			this.pnlChannel.Controls.Add(this.lblOffsetFreq);
			this.pnlChannel.Controls.Add(this.cmbOffsetDirection);
			this.pnlChannel.Controls.Add(this.lblOffsetDirection);
			this.pnlChannel.Controls.Add(this.cmbOffsetStep);
			this.pnlChannel.Controls.Add(this.lblOffsetStep);
			this.pnlChannel.Controls.Add(this.btnCopy);
			this.pnlChannel.Controls.Add(this.cmbScanList);
			this.pnlChannel.Controls.Add(this.txtName);
			this.pnlChannel.Controls.Add(this.cmbSquelch);
			this.pnlChannel.Controls.Add(this.cmbSql);
			this.pnlChannel.Controls.Add(this.cmbAdmitCriteria);
			this.pnlChannel.Controls.Add(this.lblSquelch);
			this.pnlChannel.Controls.Add(this.nudRssiThreshold);
			this.pnlChannel.Controls.Add(this.lblSql);
			this.pnlChannel.Controls.Add(this.grpDigit);
			this.pnlChannel.Controls.Add(this.chkRxOnly);
			this.pnlChannel.Controls.Add(this.cmbRxRefFreq);
			this.pnlChannel.Controls.Add(this.chkAllowTalkaround);
			this.pnlChannel.Controls.Add(this.grpAnalog);
			this.pnlChannel.Controls.Add(this.chkLoneWoker);
			this.pnlChannel.Controls.Add(this.chkVox);
			this.pnlChannel.Controls.Add(this.chkAutoScan);
			this.pnlChannel.Controls.Add(this.cmbChMode);
			this.pnlChannel.Controls.Add(this.lblChName);
			this.pnlChannel.Controls.Add(this.txtTxFreq);
			this.pnlChannel.Controls.Add(this.lblChMode);
			this.pnlChannel.Controls.Add(this.lblTot);
			this.pnlChannel.Controls.Add(this.txtRxFreq);
			this.pnlChannel.Controls.Add(this.lblTotRekey);
			this.pnlChannel.Controls.Add(this.lblRssiThreshold);
			this.pnlChannel.Controls.Add(this.lblRxRefFreq);
			this.pnlChannel.Controls.Add(this.lblBandType);
			this.pnlChannel.Controls.Add(this.lblTxRefFreq);
			this.pnlChannel.Controls.Add(this.cmbPower);
			this.pnlChannel.Controls.Add(this.lblRxFreq);
			this.pnlChannel.Controls.Add(this.cmbBandType);
			this.pnlChannel.Controls.Add(this.cmbTxRefFreq);
			this.pnlChannel.Controls.Add(this.lblPower);
			this.pnlChannel.Controls.Add(this.lblAdmitCriteria);
			this.pnlChannel.Controls.Add(this.nudTotRekey);
			this.pnlChannel.Controls.Add(this.lblTxFreq);
			this.pnlChannel.Controls.Add(this.nudTot);
			this.pnlChannel.Controls.Add(this.lblScanList);
			this.pnlChannel.Dock = DockStyle.Fill;
			this.pnlChannel.Location = new Point(0, 0);
			this.pnlChannel.Name = "pnlChannel";
			this.pnlChannel.Size = new Size(1104, 684);
			this.pnlChannel.TabIndex = 0;
			this.pnlChannel.TabStop = true;
			this.nudOffsetFreq.DecimalPlaces = 2;
			this.nudOffsetFreq.Increment = new decimal(new int[4]
			{
				1,
				0,
				0,
				131072
			});
			this.nudOffsetFreq.method_2(null);
			this.nudOffsetFreq.Location = new Point(364, 199);
			this.nudOffsetFreq.Name = "nudOffsetFreq";
			this.nudOffsetFreq.method_6(null);
			CustomNumericUpDown @class = this.nudOffsetFreq;
			int[] bits = new int[4];
			this.nudOffsetFreq.method_4(new decimal(bits));
			this.nudOffsetFreq.Size = new Size(120, 21);
			this.nudOffsetFreq.TabIndex = 37;
			this.lblOffsetFreq.Location = new Point(245, 199);
			this.lblOffsetFreq.Name = "lblOffsetFreq";
			this.lblOffsetFreq.Size = new Size(113, 20);
			this.lblOffsetFreq.TabIndex = 36;
			this.lblOffsetFreq.Text = "Offset Freq [k]";
			this.lblOffsetFreq.TextAlign = ContentAlignment.MiddleRight;
			this.cmbOffsetDirection.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbOffsetDirection.FormattingEnabled = true;
			this.cmbOffsetDirection.Location = new Point(364, 173);
			this.cmbOffsetDirection.Name = "cmbOffsetDirection";
			this.cmbOffsetDirection.Size = new Size(120, 20);
			this.cmbOffsetDirection.TabIndex = 35;
			this.lblOffsetDirection.Location = new Point(245, 173);
			this.lblOffsetDirection.Name = "lblOffsetDirection";
			this.lblOffsetDirection.Size = new Size(113, 20);
			this.lblOffsetDirection.TabIndex = 34;
			this.lblOffsetDirection.Text = "Offset Direction";
			this.lblOffsetDirection.TextAlign = ContentAlignment.MiddleRight;
			this.cmbOffsetStep.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbOffsetStep.FormattingEnabled = true;
			this.cmbOffsetStep.Location = new Point(364, 144);
			this.cmbOffsetStep.Name = "cmbOffsetStep";
			this.cmbOffsetStep.Size = new Size(120, 20);
			this.cmbOffsetStep.TabIndex = 33;
			this.lblOffsetStep.Location = new Point(245, 144);
			this.lblOffsetStep.Name = "lblOffsetStep";
			this.lblOffsetStep.Size = new Size(113, 20);
			this.lblOffsetStep.TabIndex = 32;
			this.lblOffsetStep.Text = "Offset Step";
			this.lblOffsetStep.TextAlign = ContentAlignment.MiddleRight;
			this.btnCopy.Location = new Point(490, 26);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new Size(33, 23);
			this.btnCopy.TabIndex = 31;
			this.btnCopy.Text = ">>";
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += this.btnCopy_Click;
			this.cmbScanList.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbScanList.FormattingEnabled = true;
			this.cmbScanList.Location = new Point(900, 56);
			this.cmbScanList.Name = "cmbScanList";
			this.cmbScanList.Size = new Size(120, 20);
			this.cmbScanList.TabIndex = 24;
			this.cmbScanList.SelectedIndexChanged += this.cmbScanList_SelectedIndexChanged;
			this.txtName.InputString = null;
			this.txtName.Location = new Point(94, 56);
			this.txtName.MaxByteLength = 0;
			this.txtName.Name = "txtName";
			this.txtName.Size = new Size(120, 21);
			this.txtName.TabIndex = 3;
			this.txtName.Leave += this.txtName_Leave;
			this.cmbSquelch.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbSquelch.FormattingEnabled = true;
			this.cmbSquelch.Location = new Point(364, 56);
			this.cmbSquelch.Name = "cmbSquelch";
			this.cmbSquelch.Size = new Size(120, 20);
			this.cmbSquelch.TabIndex = 3;
			this.cmbSql.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbSql.FormattingEnabled = true;
			this.cmbSql.Location = new Point(364, 117);
			this.cmbSql.Name = "cmbSql";
			this.cmbSql.Size = new Size(120, 20);
			this.cmbSql.TabIndex = 3;
			this.cmbSql.Visible = true;
			this.cmbAdmitCriteria.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbAdmitCriteria.FormattingEnabled = true;
			this.cmbAdmitCriteria.Location = new Point(900, 26);
			this.cmbAdmitCriteria.Name = "cmbAdmitCriteria";
			this.cmbAdmitCriteria.Size = new Size(120, 20);
			this.cmbAdmitCriteria.TabIndex = 20;
			this.lblSquelch.Location = new Point(245, 56);
			this.lblSquelch.Name = "lblSquelch";
			this.lblSquelch.Size = new Size(113, 20);
			this.lblSquelch.TabIndex = 2;
			this.lblSquelch.Text = "Squelch";
			this.lblSquelch.TextAlign = ContentAlignment.MiddleRight;
			this.nudRssiThreshold.method_2(null);
			this.nudRssiThreshold.Location = new Point(900, -3);
			this.nudRssiThreshold.Name = "nudRssiThreshold";
			this.nudRssiThreshold.method_6(null);
			CustomNumericUpDown class2 = this.nudRssiThreshold;
			int[] bits2 = new int[4];
			this.nudRssiThreshold.method_4(new decimal(bits2));
			this.nudRssiThreshold.Size = new Size(120, 21);
			this.nudRssiThreshold.TabIndex = 22;
			this.nudRssiThreshold.Visible = false;
			this.lblSql.Location = new Point(180, 117);
			this.lblSql.Name = "lblSql";
			this.lblSql.Size = new Size(180, 20);
			this.lblSql.TabIndex = 2;
			this.lblSql.Text = "Squelch Level";
			this.lblSql.TextAlign = ContentAlignment.MiddleRight;
			this.lblSql.Visible = true;
			this.grpDigit.method_3(true);
			this.grpDigit.Controls.Add(this.nudTxColor);
			this.grpDigit.Controls.Add(this.nudRxColor);
			this.grpDigit.Controls.Add(this.cmbTimingPreference);
			this.grpDigit.Controls.Add(this.cmbRepeaterSlot);
			this.grpDigit.Controls.Add(this.lblTimingPreference);
			this.grpDigit.Controls.Add(this.cmbArs);
			this.grpDigit.Controls.Add(this.lblRepeaterSlot);
			this.grpDigit.Controls.Add(this.cmbKeySwitch);
			this.grpDigit.Controls.Add(this.lblArs);
			this.grpDigit.Controls.Add(this.cmbKey);
			this.grpDigit.Controls.Add(this.lblKeySwitch);
			this.grpDigit.Controls.Add(this.lblKey);
			this.grpDigit.Controls.Add(this.cmbRxGroup);
			this.grpDigit.Controls.Add(this.lblTxColor);
			this.grpDigit.Controls.Add(this.cmbEmgSystem);
			this.grpDigit.Controls.Add(this.lblEmgSystem);
			this.grpDigit.Controls.Add(this.cmbContact);
			this.grpDigit.Controls.Add(this.lblContact);
			this.grpDigit.Controls.Add(this.chkDualCapacity);
			this.grpDigit.Controls.Add(this.chkUdpDataHead);
			this.grpDigit.Controls.Add(this.chkAllowTxInterupt);
			this.grpDigit.Controls.Add(this.chkTxInteruptFreq);
			this.grpDigit.Controls.Add(this.chkPrivateCall);
			this.grpDigit.Controls.Add(this.chkDataCall);
			this.grpDigit.Controls.Add(this.chkEmgConfirmed);
			this.grpDigit.Controls.Add(this.chkEnhancedChAccess);
			this.grpDigit.Controls.Add(this.lblRxColor);
			this.grpDigit.Controls.Add(this.lblRxGroup);
			this.grpDigit.method_1(false);
			this.grpDigit.Location = new Point(578, 237);
			this.grpDigit.Name = "grpDigit";
			this.grpDigit.Size = new Size(499, 408);
			this.grpDigit.TabIndex = 30;
			this.grpDigit.TabStop = false;
			this.grpDigit.Text = "Digital";
			this.nudTxColor.method_2(null);
			this.nudTxColor.Location = new Point(233, 118);
			this.nudTxColor.Name = "nudTxColor";
			this.nudTxColor.method_6(null);
			CustomNumericUpDown class3 = this.nudTxColor;
			int[] bits3 = new int[4];
			this.nudTxColor.method_4(new decimal(bits3));
			this.nudTxColor.Size = new Size(120, 21);
			this.nudTxColor.TabIndex = 17;
			this.nudRxColor.method_2(null);
			this.nudRxColor.Location = new Point(101, 314);
			this.nudRxColor.Name = "nudRxColor";
			this.nudRxColor.method_6(null);
			CustomNumericUpDown class4 = this.nudRxColor;
			int[] bits4 = new int[4];
			this.nudRxColor.method_4(new decimal(bits4));
			this.nudRxColor.Size = new Size(120, 21);
			this.nudRxColor.TabIndex = 13;
			this.nudRxColor.Visible = false;
			this.cmbTimingPreference.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbTimingPreference.FormattingEnabled = true;
			this.cmbTimingPreference.Location = new Point(101, 344);
			this.cmbTimingPreference.Name = "cmbTimingPreference";
			this.cmbTimingPreference.Size = new Size(120, 20);
			this.cmbTimingPreference.TabIndex = 2;
			this.cmbTimingPreference.Visible = false;
			this.cmbRepeaterSlot.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbRepeaterSlot.FormattingEnabled = true;
			this.cmbRepeaterSlot.Location = new Point(233, 206);
			this.cmbRepeaterSlot.Name = "cmbRepeaterSlot";
			this.cmbRepeaterSlot.Size = new Size(120, 20);
			this.cmbRepeaterSlot.TabIndex = 4;
			this.lblTimingPreference.Location = new Point(-52, 344);
			this.lblTimingPreference.Name = "lblTimingPreference";
			this.lblTimingPreference.Size = new Size(143, 20);
			this.lblTimingPreference.TabIndex = 1;
			this.lblTimingPreference.Text = "Timing Leader Prefernce";
			this.lblTimingPreference.TextAlign = ContentAlignment.MiddleRight;
			this.lblTimingPreference.Visible = false;
			this.cmbArs.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbArs.FormattingEnabled = true;
			this.cmbArs.Location = new Point(101, 370);
			this.cmbArs.Name = "cmbArs";
			this.cmbArs.Size = new Size(120, 20);
			this.cmbArs.TabIndex = 6;
			this.cmbArs.Visible = false;
			this.lblRepeaterSlot.Location = new Point(55, 206);
			this.lblRepeaterSlot.Name = "lblRepeaterSlot";
			this.lblRepeaterSlot.Size = new Size(166, 20);
			this.lblRepeaterSlot.TabIndex = 3;
			this.lblRepeaterSlot.Text = "Repeater/Time Slot";
			this.lblRepeaterSlot.TextAlign = ContentAlignment.MiddleRight;
			this.cmbKeySwitch.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbKeySwitch.FormattingEnabled = true;
			this.cmbKeySwitch.Location = new Point(233, 30);
			this.cmbKeySwitch.Name = "cmbKeySwitch";
			this.cmbKeySwitch.Size = new Size(120, 20);
			this.cmbKeySwitch.TabIndex = 8;
			this.cmbKeySwitch.SelectedIndexChanged += this.cmbKeySwitch_SelectedIndexChanged;
			this.lblArs.Location = new Point(-52, 370);
			this.lblArs.Name = "lblArs";
			this.lblArs.Size = new Size(143, 20);
			this.lblArs.TabIndex = 5;
			this.lblArs.Text = "ARS";
			this.lblArs.TextAlign = ContentAlignment.MiddleRight;
			this.lblArs.Visible = false;
			this.cmbKey.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbKey.FormattingEnabled = true;
			this.cmbKey.Location = new Point(233, 60);
			this.cmbKey.Name = "cmbKey";
			this.cmbKey.Size = new Size(120, 20);
			this.cmbKey.TabIndex = 10;
			this.lblKeySwitch.Location = new Point(55, 30);
			this.lblKeySwitch.Name = "lblKeySwitch";
			this.lblKeySwitch.Size = new Size(166, 20);
			this.lblKeySwitch.TabIndex = 7;
			this.lblKeySwitch.Text = "Privacy";
			this.lblKeySwitch.TextAlign = ContentAlignment.MiddleRight;
			this.lblKey.Location = new Point(55, 60);
			this.lblKey.Name = "lblKey";
			this.lblKey.Size = new Size(166, 20);
			this.lblKey.TabIndex = 9;
			this.lblKey.Text = "Privacy Group";
			this.lblKey.TextAlign = ContentAlignment.MiddleRight;
			this.cmbRxGroup.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbRxGroup.FormattingEnabled = true;
			this.cmbRxGroup.Location = new Point(233, 89);
			this.cmbRxGroup.Name = "cmbRxGroup";
			this.cmbRxGroup.Size = new Size(120, 20);
			this.cmbRxGroup.TabIndex = 15;
			this.lblTxColor.Location = new Point(55, 118);
			this.lblTxColor.Name = "lblTxColor";
			this.lblTxColor.Size = new Size(166, 20);
			this.lblTxColor.TabIndex = 16;
			this.lblTxColor.Text = "Color Code";
			this.lblTxColor.TextAlign = ContentAlignment.MiddleRight;
			this.cmbEmgSystem.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbEmgSystem.FormattingEnabled = true;
			this.cmbEmgSystem.Location = new Point(233, 148);
			this.cmbEmgSystem.Name = "cmbEmgSystem";
			this.cmbEmgSystem.Size = new Size(120, 20);
			this.cmbEmgSystem.TabIndex = 19;
			this.cmbEmgSystem.SelectedIndexChanged += this.cmbEmgSystem_SelectedIndexChanged;
			this.lblEmgSystem.Location = new Point(55, 148);
			this.lblEmgSystem.Name = "lblEmgSystem";
			this.lblEmgSystem.Size = new Size(166, 20);
			this.lblEmgSystem.TabIndex = 18;
			this.lblEmgSystem.Text = "Emergency System";
			this.lblEmgSystem.TextAlign = ContentAlignment.MiddleRight;
			this.cmbContact.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbContact.FormattingEnabled = true;
			this.cmbContact.Location = new Point(233, 178);
			this.cmbContact.Name = "cmbContact";
			this.cmbContact.Size = new Size(120, 20);
			this.cmbContact.TabIndex = 21;
			this.lblContact.Location = new Point(55, 178);
			this.lblContact.Name = "lblContact";
			this.lblContact.Size = new Size(166, 20);
			this.lblContact.TabIndex = 20;
			this.lblContact.Text = "Contact Name";
			this.lblContact.TextAlign = ContentAlignment.MiddleRight;
			this.chkDualCapacity.AutoSize = true;
			this.chkDualCapacity.Location = new Point(155, 329);
			this.chkDualCapacity.Name = "chkDualCapacity";
			this.chkDualCapacity.Size = new Size(174, 16);
			this.chkDualCapacity.TabIndex = 0;
			this.chkDualCapacity.Text = "Dual Capacity Direct Mode";
			this.chkDualCapacity.UseVisualStyleBackColor = true;
			this.chkDualCapacity.CheckedChanged += this.chkDualCapacity_CheckedChanged;
			this.chkUdpDataHead.AutoSize = true;
			this.chkUdpDataHead.Location = new Point(285, 273);
			this.chkUdpDataHead.Name = "chkUdpDataHead";
			this.chkUdpDataHead.Size = new Size(180, 16);
			this.chkUdpDataHead.TabIndex = 11;
			this.chkUdpDataHead.Text = "Compressed UDP Data Header";
			this.chkUdpDataHead.UseVisualStyleBackColor = true;
			this.chkUdpDataHead.Visible = false;
			this.chkAllowTxInterupt.AutoSize = true;
			this.chkAllowTxInterupt.Location = new Point(285, 303);
			this.chkAllowTxInterupt.Name = "chkAllowTxInterupt";
			this.chkAllowTxInterupt.Size = new Size(132, 16);
			this.chkAllowTxInterupt.TabIndex = 22;
			this.chkAllowTxInterupt.Text = "Allow Interruption";
			this.chkAllowTxInterupt.UseVisualStyleBackColor = true;
			this.chkAllowTxInterupt.Visible = false;
			this.chkTxInteruptFreq.AutoSize = true;
			this.chkTxInteruptFreq.Location = new Point(285, 329);
			this.chkTxInteruptFreq.Name = "chkTxInteruptFreq";
			this.chkTxInteruptFreq.Size = new Size(192, 16);
			this.chkTxInteruptFreq.TabIndex = 23;
			this.chkTxInteruptFreq.Text = "Tx Interruptible Frequencies";
			this.chkTxInteruptFreq.UseVisualStyleBackColor = true;
			this.chkTxInteruptFreq.Visible = false;
			this.chkPrivateCall.AutoSize = true;
			this.chkPrivateCall.Location = new Point(155, 243);
			this.chkPrivateCall.Name = "chkPrivateCall";
			this.chkPrivateCall.Size = new Size(156, 16);
			this.chkPrivateCall.TabIndex = 24;
			this.chkPrivateCall.Text = "Private Call Confirmed";
			this.chkPrivateCall.UseVisualStyleBackColor = true;
			this.chkDataCall.AutoSize = true;
			this.chkDataCall.Location = new Point(155, 273);
			this.chkDataCall.Name = "chkDataCall";
			this.chkDataCall.Size = new Size(138, 16);
			this.chkDataCall.TabIndex = 25;
			this.chkDataCall.Text = "Data Call Confirmed";
			this.chkDataCall.UseVisualStyleBackColor = true;
			this.chkEmgConfirmed.AutoSize = true;
			this.chkEmgConfirmed.Location = new Point(155, 303);
			this.chkEmgConfirmed.Name = "chkEmgConfirmed";
			this.chkEmgConfirmed.Size = new Size(138, 16);
			this.chkEmgConfirmed.TabIndex = 26;
			this.chkEmgConfirmed.Text = "Emergency Alarm Ack";
			this.chkEmgConfirmed.UseVisualStyleBackColor = true;
			this.chkEnhancedChAccess.AutoSize = true;
			this.chkEnhancedChAccess.Location = new Point(285, 354);
			this.chkEnhancedChAccess.Name = "chkEnhancedChAccess";
			this.chkEnhancedChAccess.Size = new Size(162, 16);
			this.chkEnhancedChAccess.TabIndex = 27;
			this.chkEnhancedChAccess.Text = "Enhanced Channel Access";
			this.chkEnhancedChAccess.UseVisualStyleBackColor = true;
			this.chkEnhancedChAccess.Visible = false;
			this.lblRxColor.Location = new Point(-52, 314);
			this.lblRxColor.Name = "lblRxColor";
			this.lblRxColor.Size = new Size(143, 20);
			this.lblRxColor.TabIndex = 12;
			this.lblRxColor.Text = "Rx Color Code";
			this.lblRxColor.TextAlign = ContentAlignment.MiddleRight;
			this.lblRxColor.Visible = false;
			this.lblRxGroup.Location = new Point(55, 89);
			this.lblRxGroup.Name = "lblRxGroup";
			this.lblRxGroup.Size = new Size(166, 20);
			this.lblRxGroup.TabIndex = 14;
			this.lblRxGroup.Text = "Rx Group List";
			this.lblRxGroup.TextAlign = ContentAlignment.MiddleRight;
			this.chkRxOnly.AutoSize = true;
			this.chkRxOnly.Location = new Point(900, 156);
			this.chkRxOnly.Name = "chkRxOnly";
			this.chkRxOnly.Size = new Size(66, 16);
			this.chkRxOnly.TabIndex = 28;
			this.chkRxOnly.Text = "Rx Only";
			this.chkRxOnly.UseVisualStyleBackColor = true;
			this.chkRxOnly.CheckedChanged += this.chkRxOnly_CheckedChanged;
			this.cmbRxRefFreq.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbRxRefFreq.FormattingEnabled = true;
			this.cmbRxRefFreq.Location = new Point(364, 86);
			this.cmbRxRefFreq.Name = "cmbRxRefFreq";
			this.cmbRxRefFreq.Size = new Size(120, 20);
			this.cmbRxRefFreq.TabIndex = 7;
			this.cmbRxRefFreq.Visible = false;
			this.chkAllowTalkaround.AutoSize = true;
			this.chkAllowTalkaround.Location = new Point(900, 134);
			this.chkAllowTalkaround.Name = "chkAllowTalkaround";
			this.chkAllowTalkaround.Size = new Size(120, 16);
			this.chkAllowTalkaround.TabIndex = 27;
			this.chkAllowTalkaround.Text = "Allow Talkaround";
			this.chkAllowTalkaround.UseVisualStyleBackColor = true;
			this.grpAnalog.method_3(true);
			this.grpAnalog.Controls.Add(this.nudArtsInterval);
			this.grpAnalog.Controls.Add(this.cmbChBandwidth);
			this.grpAnalog.Controls.Add(this.lblChBandwidth);
			this.grpAnalog.Controls.Add(this.cmbVoiceEmphasis);
			this.grpAnalog.Controls.Add(this.cmbSte);
			this.grpAnalog.Controls.Add(this.lblVoiceEmphasis);
			this.grpAnalog.Controls.Add(this.cmbNonSte);
			this.grpAnalog.Controls.Add(this.lblSte);
			this.grpAnalog.Controls.Add(this.cmbRxTone);
			this.grpAnalog.Controls.Add(this.lblNonSte);
			this.grpAnalog.Controls.Add(this.cmbRxSignaling);
			this.grpAnalog.Controls.Add(this.lblRxTone);
			this.grpAnalog.Controls.Add(this.cmbUnmuteRule);
			this.grpAnalog.Controls.Add(this.lblRxSignaling);
			this.grpAnalog.Controls.Add(this.cmbArts);
			this.grpAnalog.Controls.Add(this.cmbPttidType);
			this.grpAnalog.Controls.Add(this.lblUnmuteRule);
			this.grpAnalog.Controls.Add(this.lblArtsInterval);
			this.grpAnalog.Controls.Add(this.lblArts);
			this.grpAnalog.Controls.Add(this.lblPttidType);
			this.grpAnalog.Controls.Add(this.cmbTxSignaling);
			this.grpAnalog.Controls.Add(this.lblTxSignaling);
			this.grpAnalog.Controls.Add(this.cmbTxTone);
			this.grpAnalog.Controls.Add(this.lblTxTone);
			this.grpAnalog.Controls.Add(this.chkDataPl);
			this.grpAnalog.method_1(false);
			this.grpAnalog.Location = new Point(28, 237);
			this.grpAnalog.Name = "grpAnalog";
			this.grpAnalog.Size = new Size(531, 408);
			this.grpAnalog.TabIndex = 29;
			this.grpAnalog.TabStop = false;
			this.grpAnalog.Text = "Analog";
			this.nudArtsInterval.method_2(null);
			this.nudArtsInterval.Location = new Point(401, 263);
			this.nudArtsInterval.Name = "nudArtsInterval";
			this.nudArtsInterval.method_6(null);
			CustomNumericUpDown class5 = this.nudArtsInterval;
			int[] bits5 = new int[4];
			this.nudArtsInterval.method_4(new decimal(bits5));
			this.nudArtsInterval.Size = new Size(120, 21);
			this.nudArtsInterval.TabIndex = 25;
			this.nudArtsInterval.Visible = false;
			this.cmbChBandwidth.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbChBandwidth.FormattingEnabled = true;
			this.cmbChBandwidth.Location = new Point(250, 29);
			this.cmbChBandwidth.Name = "cmbChBandwidth";
			this.cmbChBandwidth.Size = new Size(120, 20);
			this.cmbChBandwidth.TabIndex = 1;
			this.lblChBandwidth.Location = new Point(96, 29);
			this.lblChBandwidth.Name = "lblChBandwidth";
			this.lblChBandwidth.Size = new Size(143, 20);
			this.lblChBandwidth.TabIndex = 0;
			this.lblChBandwidth.Text = "Channel Bandwidth [KHz]";
			this.lblChBandwidth.TextAlign = ContentAlignment.MiddleRight;
			this.cmbVoiceEmphasis.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbVoiceEmphasis.FormattingEnabled = true;
			this.cmbVoiceEmphasis.Location = new Point(250, 119);
			this.cmbVoiceEmphasis.Name = "cmbVoiceEmphasis";
			this.cmbVoiceEmphasis.Size = new Size(120, 20);
			this.cmbVoiceEmphasis.TabIndex = 5;
			this.cmbVoiceEmphasis.Visible = false;
			this.cmbSte.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbSte.FormattingEnabled = true;
			this.cmbSte.Location = new Point(250, 60);
			this.cmbSte.Name = "cmbSte";
			this.cmbSte.Size = new Size(120, 20);
			this.cmbSte.TabIndex = 7;
			this.lblVoiceEmphasis.Location = new Point(96, 119);
			this.lblVoiceEmphasis.Name = "lblVoiceEmphasis";
			this.lblVoiceEmphasis.Size = new Size(143, 20);
			this.lblVoiceEmphasis.TabIndex = 4;
			this.lblVoiceEmphasis.Text = "Voice Emphasis";
			this.lblVoiceEmphasis.TextAlign = ContentAlignment.MiddleRight;
			this.lblVoiceEmphasis.Visible = false;
			this.cmbNonSte.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbNonSte.FormattingEnabled = true;
			this.cmbNonSte.Location = new Point(250, 90);
			this.cmbNonSte.Name = "cmbNonSte";
			this.cmbNonSte.Size = new Size(120, 20);
			this.cmbNonSte.TabIndex = 9;
			this.lblSte.Location = new Point(96, 60);
			this.lblSte.Name = "lblSte";
			this.lblSte.Size = new Size(143, 20);
			this.lblSte.TabIndex = 6;
			this.lblSte.Text = "STE";
			this.lblSte.TextAlign = ContentAlignment.MiddleRight;
			this.cmbRxTone.FormattingEnabled = true;
			this.cmbRxTone.Location = new Point(137, 151);
			this.cmbRxTone.MaxLength = 5;
			this.cmbRxTone.Name = "cmbRxTone";
			this.cmbRxTone.Size = new Size(120, 20);
			this.cmbRxTone.TabIndex = 11;
			this.cmbRxTone.Validating += this.cmbRxTone_Validating;
			this.cmbRxTone.SelectedIndexChanged += this.cmbRxTone_SelectedIndexChanged;
			this.cmbRxTone.KeyDown += this.cmbRxTone_KeyDown;
			this.lblNonSte.Location = new Point(96, 90);
			this.lblNonSte.Name = "lblNonSte";
			this.lblNonSte.Size = new Size(143, 20);
			this.lblNonSte.TabIndex = 8;
			this.lblNonSte.Text = "Non STE";
			this.lblNonSte.TextAlign = ContentAlignment.MiddleRight;
			this.cmbRxSignaling.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbRxSignaling.FormattingEnabled = true;
			this.cmbRxSignaling.Location = new Point(137, 181);
			this.cmbRxSignaling.Name = "cmbRxSignaling";
			this.cmbRxSignaling.Size = new Size(120, 20);
			this.cmbRxSignaling.TabIndex = 13;
			this.cmbRxSignaling.SelectedIndexChanged += this.cmbRxSignaling_SelectedIndexChanged;
			this.lblRxTone.Location = new Point(9, 151);
			this.lblRxTone.Name = "lblRxTone";
			this.lblRxTone.Size = new Size(119, 20);
			this.lblRxTone.TabIndex = 10;
			this.lblRxTone.Text = "Rx CTCSS/DCS [Hz]";
			this.lblRxTone.TextAlign = ContentAlignment.MiddleRight;
			this.cmbUnmuteRule.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbUnmuteRule.FormattingEnabled = true;
			this.cmbUnmuteRule.Location = new Point(137, 239);
			this.cmbUnmuteRule.Name = "cmbUnmuteRule";
			this.cmbUnmuteRule.Size = new Size(120, 20);
			this.cmbUnmuteRule.TabIndex = 15;
			this.cmbUnmuteRule.Visible = false;
			this.lblRxSignaling.Location = new Point(9, 181);
			this.lblRxSignaling.Name = "lblRxSignaling";
			this.lblRxSignaling.Size = new Size(119, 20);
			this.lblRxSignaling.TabIndex = 12;
			this.lblRxSignaling.Text = "Rx Signaling System";
			this.lblRxSignaling.TextAlign = ContentAlignment.MiddleRight;
			this.cmbArts.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbArts.FormattingEnabled = true;
			this.cmbArts.Location = new Point(401, 237);
			this.cmbArts.Name = "cmbArts";
			this.cmbArts.Size = new Size(120, 20);
			this.cmbArts.TabIndex = 22;
			this.cmbPttidType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbPttidType.FormattingEnabled = true;
			this.cmbPttidType.Location = new Point(401, 211);
			this.cmbPttidType.Name = "cmbPttidType";
			this.cmbPttidType.Size = new Size(120, 20);
			this.cmbPttidType.TabIndex = 22;
			this.lblUnmuteRule.Location = new Point(9, 239);
			this.lblUnmuteRule.Name = "lblUnmuteRule";
			this.lblUnmuteRule.Size = new Size(119, 20);
			this.lblUnmuteRule.TabIndex = 14;
			this.lblUnmuteRule.Text = "Unmute Rule";
			this.lblUnmuteRule.TextAlign = ContentAlignment.MiddleRight;
			this.lblUnmuteRule.Visible = false;
			this.lblArtsInterval.Location = new Point(271, 263);
			this.lblArtsInterval.Name = "lblArtsInterval";
			this.lblArtsInterval.Size = new Size(119, 20);
			this.lblArtsInterval.TabIndex = 24;
			this.lblArtsInterval.Text = "ARTS Interval [s]";
			this.lblArtsInterval.TextAlign = ContentAlignment.MiddleRight;
			this.lblArtsInterval.Visible = false;
			this.lblArts.Location = new Point(271, 237);
			this.lblArts.Name = "lblArts";
			this.lblArts.Size = new Size(119, 20);
			this.lblArts.TabIndex = 21;
			this.lblArts.Text = "ARTS";
			this.lblArts.TextAlign = ContentAlignment.MiddleRight;
			this.lblPttidType.Location = new Point(271, 211);
			this.lblPttidType.Name = "lblPttidType";
			this.lblPttidType.Size = new Size(119, 20);
			this.lblPttidType.TabIndex = 21;
			this.lblPttidType.Text = "PTTID Type";
			this.lblPttidType.TextAlign = ContentAlignment.MiddleRight;
			this.cmbTxSignaling.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbTxSignaling.FormattingEnabled = true;
			this.cmbTxSignaling.Location = new Point(401, 181);
			this.cmbTxSignaling.Name = "cmbTxSignaling";
			this.cmbTxSignaling.Size = new Size(120, 20);
			this.cmbTxSignaling.TabIndex = 20;
			this.cmbTxSignaling.SelectedIndexChanged += this.cmbTxSignaling_SelectedIndexChanged;
			this.lblTxSignaling.Location = new Point(271, 181);
			this.lblTxSignaling.Name = "lblTxSignaling";
			this.lblTxSignaling.Size = new Size(119, 20);
			this.lblTxSignaling.TabIndex = 19;
			this.lblTxSignaling.Text = "Tx Signaling System";
			this.lblTxSignaling.TextAlign = ContentAlignment.MiddleRight;
			this.cmbTxTone.FormattingEnabled = true;
			this.cmbTxTone.Location = new Point(401, 151);
			this.cmbTxTone.MaxLength = 5;
			this.cmbTxTone.Name = "cmbTxTone";
			this.cmbTxTone.Size = new Size(120, 20);
			this.cmbTxTone.TabIndex = 18;
			this.cmbTxTone.Validating += this.cmbTxTone_Validating;
			this.cmbTxTone.SelectedIndexChanged += this.SwsqRwFuko;
			this.cmbTxTone.KeyDown += this.cmbTxTone_KeyDown;
			this.lblTxTone.Location = new Point(271, 151);
			this.lblTxTone.Name = "lblTxTone";
			this.lblTxTone.Size = new Size(119, 20);
			this.lblTxTone.TabIndex = 17;
			this.lblTxTone.Text = "Tx CTCSS/DCS [Hz]";
			this.lblTxTone.TextAlign = ContentAlignment.MiddleRight;
			this.chkDataPl.AutoSize = true;
			this.chkDataPl.Location = new Point(137, 215);
			this.chkDataPl.Name = "chkDataPl";
			this.chkDataPl.Size = new Size(90, 16);
			this.chkDataPl.TabIndex = 16;
			this.chkDataPl.Text = "PL for Data";
			this.chkDataPl.UseVisualStyleBackColor = true;
			this.chkLoneWoker.AutoSize = true;
			this.chkLoneWoker.Location = new Point(900, 110);
			this.chkLoneWoker.Name = "chkLoneWoker";
			this.chkLoneWoker.Size = new Size(90, 16);
			this.chkLoneWoker.TabIndex = 26;
			this.chkLoneWoker.Text = "Lone Worker";
			this.chkLoneWoker.UseVisualStyleBackColor = true;
			this.chkVox.AutoSize = true;
			this.chkVox.Location = new Point(651, 146);
			this.chkVox.Name = "chkVox";
			this.chkVox.Size = new Size(42, 16);
			this.chkVox.TabIndex = 18;
			this.chkVox.Text = "Vox";
			this.chkVox.UseVisualStyleBackColor = true;
			this.chkAutoScan.AutoSize = true;
			this.chkAutoScan.Location = new Point(900, 87);
			this.chkAutoScan.Name = "chkAutoScan";
			this.chkAutoScan.Size = new Size(78, 16);
			this.chkAutoScan.TabIndex = 25;
			this.chkAutoScan.Text = "Auto Scan";
			this.chkAutoScan.UseVisualStyleBackColor = true;
			this.cmbChMode.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbChMode.FormattingEnabled = true;
			this.cmbChMode.Location = new Point(93, 26);
			this.cmbChMode.Name = "cmbChMode";
			this.cmbChMode.Size = new Size(120, 20);
			this.cmbChMode.TabIndex = 1;
			this.cmbChMode.SelectedIndexChanged += this.modeChangedHandler;
			this.lblChName.Location = new Point(38, 56);
			this.lblChName.Name = "lblChName";
			this.lblChName.Size = new Size(47, 20);
			this.lblChName.TabIndex = 2;
			this.lblChName.Text = "Name";
			this.lblChName.TextAlign = ContentAlignment.MiddleRight;
			this.txtTxFreq.Location = new Point(651, 26);
			this.txtTxFreq.Name = "txtTxFreq";
			this.txtTxFreq.Size = new Size(120, 21);
			this.txtTxFreq.TabIndex = 9;
			this.txtTxFreq.Validating += this.txtTxFreq_Validating;
			this.lblChMode.Location = new Point(38, 26);
			this.lblChMode.Name = "lblChMode";
			this.lblChMode.Size = new Size(47, 20);
			this.lblChMode.TabIndex = 0;
			this.lblChMode.Text = "Mode";
			this.lblChMode.TextAlign = ContentAlignment.MiddleRight;
			this.lblTot.Location = new Point(524, 86);
			this.lblTot.Name = "lblTot";
			this.lblTot.Size = new Size(119, 20);
			this.lblTot.TabIndex = 14;
			this.lblTot.Text = "TOT [s]";
			this.lblTot.TextAlign = ContentAlignment.MiddleRight;
			this.txtRxFreq.BackColor = SystemColors.Window;
			this.txtRxFreq.Location = new Point(364, 26);
			this.txtRxFreq.Name = "txtRxFreq";
			this.txtRxFreq.Size = new Size(120, 21);
			this.txtRxFreq.TabIndex = 5;
			this.txtRxFreq.Validating += this.txtRxFreq_Validating;
			this.lblTotRekey.Location = new Point(524, 116);
			this.lblTotRekey.Name = "lblTotRekey";
			this.lblTotRekey.Size = new Size(119, 20);
			this.lblTotRekey.TabIndex = 16;
			this.lblTotRekey.Text = "TOT Rekey Delay [s]";
			this.lblTotRekey.TextAlign = ContentAlignment.MiddleRight;
			this.lblRssiThreshold.Location = new Point(767, -3);
			this.lblRssiThreshold.Name = "lblRssiThreshold";
			this.lblRssiThreshold.Size = new Size(125, 20);
			this.lblRssiThreshold.TabIndex = 21;
			this.lblRssiThreshold.Text = "RSSI Threshold [dBm]";
			this.lblRssiThreshold.TextAlign = ContentAlignment.MiddleRight;
			this.lblRssiThreshold.Visible = false;
			this.lblRxRefFreq.Location = new Point(221, 86);
			this.lblRxRefFreq.Name = "lblRxRefFreq";
			this.lblRxRefFreq.Size = new Size(137, 20);
			this.lblRxRefFreq.TabIndex = 6;
			this.lblRxRefFreq.Text = "Rx Reference Frequency";
			this.lblRxRefFreq.TextAlign = ContentAlignment.MiddleRight;
			this.lblRxRefFreq.Visible = false;
			this.lblBandType.Location = new Point(524, 200);
			this.lblBandType.Name = "lblBandType";
			this.lblBandType.Size = new Size(119, 20);
			this.lblBandType.TabIndex = 10;
			this.lblBandType.Text = "Band Type";
			this.lblBandType.TextAlign = ContentAlignment.MiddleRight;
			this.lblBandType.Visible = false;
			this.lblTxRefFreq.Location = new Point(506, 172);
			this.lblTxRefFreq.Name = "lblTxRefFreq";
			this.lblTxRefFreq.Size = new Size(137, 20);
			this.lblTxRefFreq.TabIndex = 10;
			this.lblTxRefFreq.Text = "Tx Reference Frequency";
			this.lblTxRefFreq.TextAlign = ContentAlignment.MiddleRight;
			this.lblTxRefFreq.Visible = false;
			this.cmbPower.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbPower.FormattingEnabled = true;
			this.cmbPower.Location = new Point(651, 56);
			this.cmbPower.Name = "cmbPower";
			this.cmbPower.Size = new Size(120, 20);
			this.cmbPower.TabIndex = 13;
			this.lblRxFreq.Location = new Point(245, 26);
			this.lblRxFreq.Name = "lblRxFreq";
			this.lblRxFreq.Size = new Size(113, 20);
			this.lblRxFreq.TabIndex = 4;
			this.lblRxFreq.Text = "Rx Frequency [MHz]";
			this.lblRxFreq.TextAlign = ContentAlignment.MiddleRight;
			this.cmbBandType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbBandType.FormattingEnabled = true;
			this.cmbBandType.Location = new Point(651, 200);
			this.cmbBandType.Name = "cmbBandType";
			this.cmbBandType.Size = new Size(120, 20);
			this.cmbBandType.TabIndex = 11;
			this.cmbBandType.Visible = false;
			this.cmbTxRefFreq.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbTxRefFreq.FormattingEnabled = true;
			this.cmbTxRefFreq.Location = new Point(651, 172);
			this.cmbTxRefFreq.Name = "cmbTxRefFreq";
			this.cmbTxRefFreq.Size = new Size(120, 20);
			this.cmbTxRefFreq.TabIndex = 11;
			this.cmbTxRefFreq.Visible = false;
			this.lblPower.Location = new Point(524, 56);
			this.lblPower.Name = "lblPower";
			this.lblPower.Size = new Size(119, 20);
			this.lblPower.TabIndex = 12;
			this.lblPower.Text = "Power Level";
			this.lblPower.TextAlign = ContentAlignment.MiddleRight;
			this.lblAdmitCriteria.Location = new Point(767, 26);
			this.lblAdmitCriteria.Name = "lblAdmitCriteria";
			this.lblAdmitCriteria.Size = new Size(125, 20);
			this.lblAdmitCriteria.TabIndex = 19;
			this.lblAdmitCriteria.Text = "Admit Criteria";
			this.lblAdmitCriteria.TextAlign = ContentAlignment.MiddleRight;
			this.nudTotRekey.method_2(null);
			this.nudTotRekey.Location = new Point(651, 116);
			this.nudTotRekey.Name = "nudTotRekey";
			this.nudTotRekey.method_6(null);
			CustomNumericUpDown class6 = this.nudTotRekey;
			int[] bits6 = new int[4];
			this.nudTotRekey.method_4(new decimal(bits6));
			this.nudTotRekey.Size = new Size(120, 21);
			this.nudTotRekey.TabIndex = 17;
			this.lblTxFreq.Location = new Point(524, 26);
			this.lblTxFreq.Name = "lblTxFreq";
			this.lblTxFreq.Size = new Size(119, 20);
			this.lblTxFreq.TabIndex = 8;
			this.lblTxFreq.Text = "Tx Frequency [MHz]";
			this.lblTxFreq.TextAlign = ContentAlignment.MiddleRight;
			this.nudTot.method_2(null);
			this.nudTot.Location = new Point(651, 86);
			this.nudTot.Name = "nudTot";
			this.nudTot.method_6(null);
			CustomNumericUpDown class7 = this.nudTot;
			int[] bits7 = new int[4];
			this.nudTot.method_4(new decimal(bits7));
			this.nudTot.Size = new Size(120, 21);
			this.nudTot.TabIndex = 15;
			this.nudTot.Value = new decimal(new int[4]
			{
				50,
				0,
				0,
				0
			});
			this.nudTot.ValueChanged += this.nudTot_ValueChanged;
			this.lblScanList.Location = new Point(767, 56);
			this.lblScanList.Name = "lblScanList";
			this.lblScanList.Size = new Size(125, 20);
			this.lblScanList.TabIndex = 23;
			this.lblScanList.Text = "Scan List";
			this.lblScanList.TextAlign = ContentAlignment.MiddleRight;

			/*
			 * 
			 * Disable unused items for VFO mode.
			 * 
			 * Radioddity did this in version 3.1.x of the official CPS but this issue also applies to existing version(s) e.g. 3.0.6
			 * So I have removed it as a bug fix to both.
			 * 
			this.lblScanList.Visible = false;					
			this.chkAutoScan.Visible = false;
			this.cmbScanList.Visible = false;
			*/

			base.AutoScaleDimensions = new SizeF(6f, 12f);
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(1104, 684);
			base.Controls.Add(this.pnlChannel);
			base.Controls.Add(this.mnsCh);
            this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.MainMenuStrip = this.mnsCh;
			base.Name = "VfoForm";
			this.Text = "VFO";
			base.Load += this.VfoForm_Load;
			base.FormClosing += this.VfoForm_FormClosing;
			this.mnsCh.ResumeLayout(false);
			this.mnsCh.PerformLayout();
			this.pnlChannel.ResumeLayout(false);
			this.pnlChannel.PerformLayout();
			((ISupportInitialize)this.nudOffsetFreq).EndInit();
			((ISupportInitialize)this.nudRssiThreshold).EndInit();
			this.grpDigit.ResumeLayout(false);
			this.grpDigit.PerformLayout();
			((ISupportInitialize)this.nudTxColor).EndInit();
			((ISupportInitialize)this.nudRxColor).EndInit();
			this.grpAnalog.ResumeLayout(false);
			this.grpAnalog.PerformLayout();
			((ISupportInitialize)this.nudArtsInterval).EndInit();
			((ISupportInitialize)this.nudTotRekey).EndInit();
			((ISupportInitialize)this.nudTot).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public void SaveData()
		{
			int index = Convert.ToInt32(base.Tag);
			this.ValidateChildren();
			ChannelOne value = new ChannelOne(index);
			if (this.txtName.Focused)
			{
				this.txtName_Leave(this.txtName, null);
			}
			value.ChMode = this.cmbChMode.SelectedIndex;
			value.Name = this.txtName.Text;
			value.RxFreq = this.txtRxFreq.Text;
			value.RxRefFreq = this.cmbRxRefFreq.SelectedIndex;
			value.TxFreq = this.txtTxFreq.Text;
			value.TxRefFreq = this.cmbTxRefFreq.SelectedIndex;
			value.Power = this.cmbPower.SelectedIndex;
			value.Tot = this.nudTot.Value;
			value.TotRekey = this.nudTotRekey.Value;
			value.Vox = this.chkVox.Checked;
			value.AdmitCriteria = this.cmbAdmitCriteria.SelectedIndex;
			value.RssiThreshold = this.nudRssiThreshold.Value;
			value.ScanList = this.cmbScanList.method_3();
			value.AutoScan = this.chkAutoScan.Checked;
			value.LoneWoker = this.chkLoneWoker.Checked;
			value.AllowTalkaround = this.chkAllowTalkaround.Checked;
			value.OnlyRx = this.chkRxOnly.Checked;
			value.Bandwidth = this.cmbChBandwidth.SelectedIndex;
			value.Squelch = this.cmbSquelch.SelectedIndex;
			value.Sql = this.cmbSql.SelectedIndex;
			value.VoiceEmphasis = this.cmbVoiceEmphasis.SelectedIndex;
			value.Ste = this.cmbSte.SelectedIndex;
			value.NonSte = this.cmbNonSte.SelectedIndex;
			value.RxTone = this.cmbRxTone.Text;
			value.TxSignaling = this.cmbTxSignaling.SelectedIndex;
			value.UnmuteRule = this.cmbUnmuteRule.SelectedIndex;
			value.DataPl = this.chkDataPl.Checked;
			value.TxTone = this.cmbTxTone.Text;
			value.RxSignaling = this.cmbRxSignaling.SelectedIndex;
			value.PttidType = this.cmbPttidType.SelectedIndex;
			value.Arts = this.cmbArts.method_3();
			value.ArtsInterval = this.nudArtsInterval.Value;
			value.DualCapacity = this.chkDualCapacity.Checked;
			value.TimingPreference = this.cmbTimingPreference.SelectedIndex;
			value.RepateSlot = this.cmbRepeaterSlot.SelectedIndex;
			value.Ars = this.cmbArs.SelectedIndex;
			value.KeySwitch = this.cmbKeySwitch.SelectedIndex;
			value.Key = this.cmbKey.method_3();
			value.UdpDataHead = this.chkUdpDataHead.Checked;
			value.RxColor = this.nudRxColor.Value;
			value.RxGroupList = this.cmbRxGroup.method_3();
			value.TxColor = this.nudTxColor.Value;
			value.EmgSystem = this.cmbEmgSystem.method_3();
			value.Contact = this.cmbContact.method_3();
			value.AllowTxInterupt = this.chkAllowTxInterupt.Checked;
			value.TxInteruptFreq = this.chkTxInteruptFreq.Checked;
			value.PrivateCall = this.chkPrivateCall.Checked;
			value.DataCall = this.chkDataCall.Checked;
			value.EmgConfirmed = this.chkEmgConfirmed.Checked;
			value.EnchancedChAccess = this.chkEnhancedChAccess.Checked;
			value.OffsetDirection = this.cmbOffsetDirection.SelectedIndex;
			value.OffsetStep = this.cmbOffsetStep.SelectedIndex;
			value.OffsetFreq = this.nudOffsetFreq.Value;
			if (value.OffsetDirection == 1)
			{
				value.TxFreqDec = value.RxFreqDec + (uint)(value.OffsetFreq * 1000m);
			}
			else if (value.OffsetDirection == 2)
			{
				value.TxFreqDec = value.RxFreqDec - (uint)(value.OffsetFreq * 1000m);
			}
			else if (value.OffsetDirection == 0)
			{
				value.TxFreqDec = value.RxFreqDec;
			}
			VfoForm.data[index] = value;
		}

		public void DispData()
		{
			int index = Convert.ToInt32(base.Tag);
			this.Text = this.Node.Text;
			ChannelOne channelOne = VfoForm.data[index];
			this.method_2();
			this.cmbChMode.SelectedIndex = channelOne.ChMode;
			this.txtName.Text = channelOne.Name;
			this.txtRxFreq.Text = channelOne.RxFreq;
			this.cmbRxRefFreq.SelectedIndex = channelOne.RxRefFreq;
			this.txtTxFreq.Text = channelOne.TxFreq;
			this.cmbTxRefFreq.SelectedIndex = channelOne.TxRefFreq;
			this.cmbPower.SelectedIndex = channelOne.Power;
			this.nudTot.Value = channelOne.Tot;
			this.nudTotRekey.Value = channelOne.TotRekey;
			this.chkVox.Checked = channelOne.Vox;
			this.cmbAdmitCriteria.SelectedIndex = channelOne.AdmitCriteria;
			this.nudRssiThreshold.Value = channelOne.RssiThreshold;
			this.cmbScanList.method_2(channelOne.ScanList);
			this.chkAutoScan.Checked = channelOne.AutoScan;
			this.chkLoneWoker.Checked = channelOne.LoneWoker;
			this.chkAllowTalkaround.Checked = channelOne.AllowTalkaround;
			this.chkRxOnly.Checked = channelOne.OnlyRx;
			this.method_13();
			this.method_14(channelOne.RxTone);
			this.cmbChBandwidth.SelectedIndex = channelOne.Bandwidth;
			this.cmbSquelch.SelectedIndex = channelOne.Squelch;
			this.cmbSql.SelectedIndex = channelOne.Sql;
			this.cmbVoiceEmphasis.SelectedIndex = channelOne.VoiceEmphasis;
			this.cmbSte.SelectedIndex = channelOne.Ste;
			this.cmbNonSte.SelectedIndex = channelOne.NonSte;
			this.cmbRxTone.Text = channelOne.RxTone;
			this.cmbTxSignaling.SelectedIndex = channelOne.TxSignaling;
			this.cmbUnmuteRule.SelectedIndex = channelOne.UnmuteRule;
			this.chkDataPl.Checked = channelOne.DataPl;
			this.cmbTxTone.Text = channelOne.TxTone;
			this.cmbRxSignaling.SelectedIndex = channelOne.RxSignaling;
			this.cmbPttidType.SelectedIndex = channelOne.PttidType;
			this.cmbArts.method_2(channelOne.Arts);
			this.nudArtsInterval.Value = channelOne.ArtsInterval;
			this.chkDualCapacity.Checked = channelOne.DualCapacity;
			this.cmbTimingPreference.SelectedIndex = channelOne.TimingPreference;
			this.cmbRepeaterSlot.SelectedIndex = channelOne.RepateSlot;
			this.cmbArs.SelectedIndex = channelOne.Ars;
			this.cmbKeySwitch.SelectedIndex = channelOne.KeySwitch;
			this.cmbKey.method_2(channelOne.Key);
			this.chkUdpDataHead.Checked = channelOne.UdpDataHead;
			this.nudRxColor.Value = channelOne.RxColor;
			this.cmbRxGroup.method_2(channelOne.RxGroupList);
			this.nudTxColor.Value = channelOne.TxColor;
			this.cmbEmgSystem.method_2(channelOne.EmgSystem);
			this.cmbContact.method_2(channelOne.Contact);
			this.chkAllowTxInterupt.Checked = channelOne.AllowTxInterupt;
			this.chkTxInteruptFreq.Checked = channelOne.TxInteruptFreq;
			this.chkPrivateCall.Checked = channelOne.PrivateCall;
			this.chkDataCall.Checked = channelOne.DataCall;
			this.chkEmgConfirmed.Checked = channelOne.EmgConfirmed;
			this.chkEnhancedChAccess.Checked = channelOne.EnchancedChAccess;
			this.cmbOffsetStep.SelectedIndex = channelOne.OffsetStep;
			this.cmbOffsetDirection.SelectedIndex = channelOne.OffsetDirection;
			this.nudOffsetFreq.Value = channelOne.OffsetFreq;
			this.method_8();
			this.method_11();
			this.method_10();
			this.method_12();
			this.method_15();
			this.method_16();
			this.method_17();
		}

		public void RefreshName()
		{
			int index = Convert.ToInt32(base.Tag);
			this.txtName.Text = VfoForm.data[index].Name;
		}

		public VfoForm()
		{
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			VfoForm.CurCntCh = 1024;
		}

		private void method_1()
		{
			Settings.smethod_37(this.cmbChMode, VfoForm.SZ_CH_MODE);
			this.txtName.MaxByteLength = 15;
			this.txtName.KeyPress += Settings.smethod_54;
			this.txtRxFreq.MaxLength = 9;
			this.txtRxFreq.KeyPress += Settings.smethod_55;
			this.txtTxFreq.MaxLength = 9;
			this.txtTxFreq.KeyPress += Settings.smethod_55;
			Settings.smethod_37(this.cmbTxRefFreq, VfoForm.SZ_REF_FREQ);
			Settings.smethod_37(this.cmbRxRefFreq, VfoForm.SZ_REF_FREQ);
			Settings.smethod_37(this.cmbPower, VfoForm.SZ_POWER);
			Settings.smethod_36(this.nudTot, new Class13(0, 33, 1, 15m, 3));
			this.nudTot.method_4(0m);
			this.nudTot.method_6("∞");
			Settings.smethod_36(this.nudTotRekey, new Class13(0, 255, 1, 1m, 3));
			Settings.smethod_36(this.nudRssiThreshold, new Class13(80, 124, 1, -1m, 4));
			Settings.smethod_37(this.cmbChBandwidth, VfoForm.SZ_BANDWIDTH);
			Settings.smethod_37(this.cmbSquelch, VfoForm.SZ_SQUELCH);
			Settings.smethod_37(this.cmbSql, VfoForm.SZ_SQUELCH_LEVEL);
			Settings.smethod_37(this.cmbVoiceEmphasis, VfoForm.SZ_VOICE_EMPHASIS);
			Settings.smethod_37(this.cmbSte, VfoForm.SZ_STE);
			Settings.smethod_37(this.cmbNonSte, VfoForm.SZ_NON_STE);
			Settings.smethod_37(this.cmbTxSignaling, VfoForm.SZ_SIGNALING_SYSTEM);
			Settings.smethod_37(this.cmbUnmuteRule, VfoForm.SZ_UNMUTE_RULE);
			Settings.smethod_37(this.cmbRxSignaling, VfoForm.SZ_SIGNALING_SYSTEM);
			Settings.smethod_37(this.cmbPttidType, VfoForm.SZ_PTTID_TYPE);
			Settings.smethod_39(this.cmbArts, VfoForm.SZ_ARTS);
			Settings.smethod_36(this.nudArtsInterval, new Class13(22, 55, 1, 1m, 2));
			Settings.smethod_37(this.cmbTimingPreference, VfoForm.SZ_TIMING_PREFERENCE);
			Settings.smethod_37(this.cmbRepeaterSlot, VfoForm.SZ_REPEATER_SOLT);
			Settings.smethod_37(this.cmbArs, VfoForm.SZ_ARS);
			Settings.smethod_37(this.cmbKeySwitch, VfoForm.SZ_KEY_SWITCH);
			Settings.smethod_36(this.nudRxColor, new Class13(0, 15, 1, 1m, 2));
			Settings.smethod_36(this.nudTxColor, new Class13(0, 15, 1, 1m, 2));
			Settings.smethod_37(this.cmbOffsetDirection, VfoForm.SZ_OFFSET_DIRECTION);
			Settings.smethod_37(this.cmbOffsetStep, VfoForm.SZ_OFFSET_STEP);
			Settings.smethod_36(this.nudOffsetFreq, new Class13(1, 38400, 1, 0.01m, 6));
		}

		private void method_2()
		{
			Settings.smethod_44(this.cmbScanList, NormalScanForm.data);
			Settings.smethod_44(this.cmbRxGroup, RxGroupListForm.data);
			Settings.smethod_44(this.cmbKey, EncryptForm.data);
			Settings.smethod_44(this.cmbEmgSystem, EmergencyForm.data);
			Settings.smethod_44(this.cmbContact, ContactForm.data);
			int index = Convert.ToInt32(base.Tag);
			if (VfoForm.data[index].ChMode == 0)
			{
				Settings.smethod_37(this.cmbAdmitCriteria, VfoForm.SZ_ADMIT_CRITERICA);
			}
			else
			{
				Settings.smethod_37(this.cmbAdmitCriteria, VfoForm.SZ_ADMIT_CRITERICA_D);
			}
		}

		public static void RefreshCommonLang()
		{
			string name = typeof(VfoForm).Name;
			Settings.smethod_78("ChMode", VfoForm.SZ_CH_MODE, name);
			Settings.smethod_78("RefFreq", VfoForm.SZ_REF_FREQ, name);
			Settings.smethod_78("Power", VfoForm.SZ_POWER, name);
			Settings.smethod_78("AdmitCriterica", VfoForm.SZ_ADMIT_CRITERICA, name);
			Settings.smethod_78("AdmitCritericaD", VfoForm.SZ_ADMIT_CRITERICA_D, name);
			Settings.smethod_78("Squelch", VfoForm.SZ_SQUELCH, name);
			Settings.smethod_78("SquelchLevel", VfoForm.SZ_SQUELCH_LEVEL, name);
			Settings.smethod_78("VoiceEmphasis", VfoForm.SZ_VOICE_EMPHASIS, name);
			Settings.smethod_78("Ste", VfoForm.SZ_STE, name);
			Settings.smethod_78("NonSte", VfoForm.SZ_NON_STE, name);
			Settings.smethod_78("SignalingSystem", VfoForm.SZ_SIGNALING_SYSTEM, name);
			Settings.smethod_78("UnmuteRule", VfoForm.SZ_UNMUTE_RULE, name);
			Settings.smethod_78("PttidType", VfoForm.SZ_PTTID_TYPE, name);
			Settings.smethod_78("Arts", VfoForm.SZ_ARTS, name);
			Settings.smethod_78("TimingPreference", VfoForm.SZ_TIMING_PREFERENCE, name);
			Settings.smethod_78("Ars", VfoForm.SZ_ARS, name);
			Settings.smethod_78("KeySwitch", VfoForm.SZ_KEY_SWITCH, name);
			Settings.smethod_78("OffsetDirection", VfoForm.SZ_OFFSET_DIRECTION, name);
		}

		private void VfoForm_Load(object sender, EventArgs e)
		{
			try
			{
				Settings.smethod_59(base.Controls);
				Settings.smethod_68(this);
				VfoForm.data.ChModeChangeEvent += this.method_3;
				this.method_9();
				this.method_1();
				this.DispData();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void VfoForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
		}

		private void txtName_Leave(object sender, EventArgs e)
		{
		}

		private void modeChangedHandler(object sender, EventArgs e)
		{
			int num = 0;
			int selectedIndex = this.cmbChMode.SelectedIndex;
			int selectedIndex2 = this.cmbAdmitCriteria.SelectedIndex;
			switch (selectedIndex)
			{
			case 0:
				num = 2;
				this.grpAnalog.Enabled = true;
				this.grpDigit.Enabled = false;
                this.chkDualCapacity.Checked = false;// Roger Clark. Fix for bug in firmware, where Dual Direct Capacity must not be enabled in Analog mode
				Settings.smethod_37(this.cmbAdmitCriteria, VfoForm.SZ_ADMIT_CRITERICA);
				break;
			case 1:
				num = 6;
				this.grpAnalog.Enabled = false;
				this.grpDigit.Enabled = true;
				Settings.smethod_37(this.cmbAdmitCriteria, VfoForm.SZ_ADMIT_CRITERICA_D);
				break;
			case 2:
				num = 54;
				this.grpAnalog.Enabled = true;
				this.grpDigit.Enabled = true;
				Settings.smethod_37(this.cmbAdmitCriteria, VfoForm.SZ_ADMIT_CRITERICA_D);
				break;
			case 3:
				num = 54;
				this.grpAnalog.Enabled = true;
				this.grpDigit.Enabled = true;
				Settings.smethod_37(this.cmbAdmitCriteria, VfoForm.SZ_ADMIT_CRITERICA);
				break;
			}
			this.method_15();
			if (selectedIndex2 < this.cmbAdmitCriteria.Items.Count)
			{
				this.cmbAdmitCriteria.SelectedIndex = selectedIndex2;
			}
			else
			{
				this.cmbAdmitCriteria.SelectedIndex = 0;
			}
			this.method_5();
			this.Node.SelectedImageIndex = num;
			this.Node.ImageIndex = num;
		}

		private void method_3(object sender, ChModeChangeEventArgs e)
		{
			MainForm mainForm = base.MdiParent as MainForm;
			if (mainForm != null)
			{
				TreeNode treeNodeByTypeAndIndex = mainForm.GetTreeNodeByTypeAndIndex(typeof(ChannelForm), e.ChIndex, this.Node.Parent.Nodes);
				if (e.ChMode == 0)
				{
					treeNodeByTypeAndIndex.ImageIndex = 2;
					treeNodeByTypeAndIndex.SelectedImageIndex = 2;
				}
				else if (e.ChMode == 1)
				{
					treeNodeByTypeAndIndex.ImageIndex = 6;
					treeNodeByTypeAndIndex.SelectedImageIndex = 6;
				}
			}
		}

		private void txtRxFreq_Validating(object sender, CancelEventArgs e)
		{
			int num = 0;
			double num2 = 0.0;
			string text = this.txtRxFreq.Text;
			double num3 = 0.0;
			string text2 = this.txtTxFreq.Text;
			try
			{
				uint num4 = 0u;
				num2 = double.Parse(text);
				if (Settings.smethod_19(num2, ref num4) >= 0)
				{
					num = (int)(num2 * 100000.0);
					Settings.smethod_29(ref num, 250, 625);
					num2 = (double)num / 100000.0;
					this.txtRxFreq.Text = num2.ToString("0.00000");
				}
				else
				{
					this.txtRxFreq.Text = string.Format("{0:f5}", num4);
				}
				num2 = double.Parse(this.txtRxFreq.Text);
				num3 = double.Parse(this.txtTxFreq.Text);
				if (Settings.smethod_20(num2, num3) < 0)
				{
					this.txtTxFreq.Text = this.txtRxFreq.Text;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				this.txtRxFreq.Text = string.Format("{0:f5}", Settings.MIN_FREQ[0]);
			}
		}

		private void txtTxFreq_Validating(object sender, CancelEventArgs e)
		{
			int num = 0;
			double num2 = 0.0;
			string text = this.txtTxFreq.Text;
			double num3 = 0.0;
			string text2 = this.txtRxFreq.Text;
			try
			{
				uint num4 = 0u;
				num2 = double.Parse(text);
				if (Settings.smethod_19(num2, ref num4) >= 0)
				{
					num = (int)(num2 * 100000.0);
					Settings.smethod_29(ref num, 250, 625);
					num2 = (double)num / 100000.0;
					this.txtTxFreq.Text = num2.ToString("0.00000");
				}
				else
				{
					this.txtTxFreq.Text = string.Format("{0:f5}", num4);
				}
				num3 = double.Parse(this.txtRxFreq.Text);
				num2 = double.Parse(this.txtTxFreq.Text);
				if (Settings.smethod_20(num3, num2) < 0)
				{
					this.txtRxFreq.Text = this.txtTxFreq.Text;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				this.txtTxFreq.Text = string.Format("{0:f5}", Settings.MIN_FREQ[0]);
			}
		}

		private void chkRxOnly_CheckedChanged(object sender, EventArgs e)
		{
			this.method_13();
			this.method_4();
		}

		private void method_4()
		{
			bool flag = !this.chkRxOnly.Checked;
			this.txtTxFreq.Enabled = flag;
			this.cmbTxRefFreq.Enabled = flag;
			this.cmbPower.Enabled = flag;
			this.nudTot.Enabled = flag;
			this.nudTotRekey.Enabled = (flag && this.nudTot.Value != 0m);
			this.chkVox.Enabled = flag;
			this.cmbAdmitCriteria.Enabled = flag;
		}

		private void nudTot_ValueChanged(object sender, EventArgs e)
		{
			this.nudTotRekey.Enabled = (this.nudTot.Enabled && this.nudTot.Value != 0m);
		}

		private void method_5()
		{
			this.chkLoneWoker.Enabled = (this.cmbEmgSystem.SelectedIndex != 0 && this.cmbChMode.SelectedIndex == 1);
			if (!this.chkLoneWoker.Enabled)
			{
				this.chkLoneWoker.Checked = false;
			}
		}

		private void method_6()
		{
			MainForm mainForm = base.MdiParent as MainForm;
			if (mainForm != null)
			{
				mainForm.RefreshRelatedForm(typeof(ChannelForm));
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void chkDualCapacity_CheckedChanged(object sender, EventArgs e)
		{
            Console.Write("chkDualCapacity_CheckedChanged");
		}

		private void method_7()
		{
			if (this.chkDualCapacity.Checked)
			{
				this.nudRxColor.Maximum = 14m;
				this.nudTxColor.Maximum = 14m;
			}
			else
			{
				this.nudTxColor.Maximum = 15m;
				this.nudRxColor.Maximum = 15m;
			}
			this.cmbTimingPreference.Enabled = this.chkDualCapacity.Checked;
		}

		private void cmbKeySwitch_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.method_8();
		}

		private void method_8()
		{
			this.cmbKey.Enabled = (this.cmbKey.Parent.Enabled && this.cmbKeySwitch.SelectedIndex > 0);
		}

		private void cmbEmgSystem_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.method_5();
		}

		private void cmbScanList_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.chkAutoScan.Enabled = (this.cmbScanList.SelectedIndex > 0);
		}

		private void method_9()
		{
			string text = "";
			this.cmbRxTone.Items.Clear();
			this.cmbTxTone.Items.Clear();
			StreamReader streamReader = new StreamReader(Application.StartupPath + "\\Tone.txt", Encoding.Default);
			while ((text = streamReader.ReadLine()) != null)
			{
				this.cmbRxTone.Items.Add(text);
				this.cmbTxTone.Items.Add(text);
			}
			streamReader.Close();
		}

		private void cmbRxTone_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.method_11();
			this.method_12();
			this.method_17();
			this.method_16();
		}

		private void cmbRxTone_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				SendKeys.Send("{tab}");
			}
		}

		private void cmbRxTone_Validating(object sender, CancelEventArgs e)
		{
			ushort num = 16;
			string empty = string.Empty;
			string text = this.cmbRxTone.Text;
			try
			{
				string pattern;
				Regex regex;
				if (!(text == Settings.SZ_NONE) && !string.IsNullOrEmpty(text))
				{
					pattern = "D[0-7]{3}N$";
					regex = new Regex(pattern);
					if (regex.IsMatch(text))
					{
						empty = text.Substring(1, 3);
						num = Convert.ToUInt16(empty, 8);
						if (num >= 777)
						{
							this.cmbRxTone.Text = Settings.SZ_NONE;
							goto IL_0076;
						}
						goto end_IL_0015;
					}
					goto IL_0076;
				}
				e.Cancel = false;
				goto end_IL_0015;
				IL_00bc:
				double num2 = double.Parse(text);
				if (num2 >= 60.0 && num2 < 260.0)
				{
					this.cmbRxTone.Text = num2.ToString("0.0");
				}
				else
				{
					this.cmbRxTone.Text = Settings.SZ_NONE;
				}
				goto end_IL_0015;
				IL_0076:
				pattern = "D[0-7]{3}I$";
				regex = new Regex(pattern);
				if (regex.IsMatch(text))
				{
					empty = text.Substring(1, 3);
					num = Convert.ToUInt16(empty, 8);
					if (num >= 777)
					{
						this.cmbRxTone.Text = Settings.SZ_NONE;
						goto IL_00bc;
					}
					goto end_IL_0015;
				}
				goto IL_00bc;
				end_IL_0015:;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				this.cmbRxTone.Text = Settings.SZ_NONE;
			}
			finally
			{
				this.method_11();
				this.method_12();
				this.method_17();
				this.method_16();
			}
		}

		private void SwsqRwFuko(object sender, EventArgs e)
		{
			this.method_12();
		}

		private void cmbTxTone_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				SendKeys.Send("{tab}");
			}
		}

		private void cmbTxTone_Validating(object sender, CancelEventArgs e)
		{
			ushort num = 16;
			string empty = string.Empty;
			string text = this.cmbTxTone.Text;
			try
			{
				string pattern;
				Regex regex;
				if (!(text == Settings.SZ_NONE))
				{
					pattern = "D[0-7]{3}N$";
					regex = new Regex(pattern);
					if (regex.IsMatch(text))
					{
						empty = text.Substring(1, 3);
						num = Convert.ToUInt16(empty, 8);
						if (num >= 777)
						{
							this.cmbTxTone.Text = Settings.SZ_NONE;
							goto IL_006b;
						}
						goto end_IL_0015;
					}
					goto IL_006b;
				}
				goto end_IL_0015;
				IL_00b1:
				double num2 = double.Parse(text);
				if (num2 > 60.0 && num2 < 260.0)
				{
					this.cmbTxTone.Text = num2.ToString("0.0");
				}
				else
				{
					this.cmbTxTone.Text = Settings.SZ_NONE;
				}
				goto end_IL_0015;
				IL_006b:
				pattern = "D[0-7]{3}I$";
				regex = new Regex(pattern);
				if (regex.IsMatch(text))
				{
					empty = text.Substring(1, 3);
					num = Convert.ToUInt16(empty, 8);
					if (num >= 777)
					{
						this.cmbTxTone.Text = Settings.SZ_NONE;
						goto IL_00b1;
					}
					goto end_IL_0015;
				}
				goto IL_00b1;
				end_IL_0015:;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				this.cmbTxTone.Text = Settings.SZ_NONE;
			}
			finally
			{
				this.method_12();
				this.method_15();
			}
		}

		private void cmbTxSignaling_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.method_10();
		}

		private void method_10()
		{
			this.cmbPttidType.Enabled = (this.cmbTxSignaling.Parent.Enabled && this.cmbTxSignaling.SelectedIndex > 0);
		}

		private void cmbRxSignaling_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.method_11();
		}

		private void method_11()
		{
			this.chkDataPl.Enabled = (this.cmbRxSignaling.SelectedIndex > 0 && this.chkDataPl.Parent.Enabled && this.cmbRxTone.Text != Settings.SZ_NONE);
			if (this.cmbRxSignaling.SelectedIndex != 0 && !(this.cmbRxTone.Text == Settings.SZ_NONE))
			{
				return;
			}
			this.chkDataPl.Checked = false;
		}

		private void method_12()
		{
			this.cmbArts.Enabled = (this.cmbRxTone.Text != Settings.SZ_NONE && this.cmbTxTone.Text != Settings.SZ_NONE && this.cmbArts.Parent.Enabled);
		}

		private void method_13()
		{
			string text = this.cmbArts.Text;
			if (this.chkRxOnly.Checked)
			{
				Settings.smethod_40(this.cmbArts, VfoForm.SZ_ARTS, new int[2]
				{
					0,
					2
				});
			}
			else
			{
				Settings.smethod_39(this.cmbArts, VfoForm.SZ_ARTS);
			}
			int num = this.cmbArts.FindStringExact(text);
			if (num < 0)
			{
				this.cmbArts.SelectedIndex = 0;
			}
			else
			{
				this.cmbArts.SelectedIndex = num;
			}
		}

		private void method_14(string string_0)
		{
			string pattern = "D[0-7]{3}[N|I]$";
			Regex regex = new Regex(pattern);
			if (regex.IsMatch(string_0))
			{
				Settings.smethod_37(this.cmbSte, new string[1]
				{
					VfoForm.SZ_STE[0]
				});
			}
			else
			{
				Settings.smethod_37(this.cmbSte, VfoForm.SZ_STE);
			}
		}

		private void method_15()
		{
			if (this.cmbChMode.SelectedIndex == 0)
			{
				if (this.cmbTxTone.Text == Settings.SZ_NONE)
				{
					if (this.cmbAdmitCriteria.Text == VfoForm.SZ_ADMIT_CRITERICA[2])
					{
						this.cmbAdmitCriteria.SelectedIndex = 0;
					}
					this.cmbAdmitCriteria.Items.Remove(VfoForm.SZ_ADMIT_CRITERICA[2]);
				}
				else if (this.cmbAdmitCriteria.FindStringExact(VfoForm.SZ_ADMIT_CRITERICA[2]) < 0)
				{
					this.cmbAdmitCriteria.Items.Add(VfoForm.SZ_ADMIT_CRITERICA[2]);
				}
			}
		}

		private void method_16()
		{
			this.cmbUnmuteRule.Enabled = (this.cmbRxTone.Text != Settings.SZ_NONE && this.cmbUnmuteRule.Parent.Enabled);
		}

		private void method_17()
		{
			double num = 0.0;
			string text = this.cmbRxTone.Text;
			string pattern = "D[0-7]{3}[N|I]$";
			Regex regex = new Regex(pattern);
			if (text == Settings.SZ_NONE)
			{
				this.cmbSte.Enabled = false;
				this.cmbNonSte.Enabled = true;
			}
			else
			{
				this.cmbSte.Enabled = true;
				this.cmbNonSte.Enabled = false;
				if (regex.IsMatch(text))
				{
					Settings.smethod_37(this.cmbSte, new string[1]
					{
						VfoForm.SZ_STE[0]
					});
					this.cmbSte.SelectedIndex = 0;
				}
				else if (double.TryParse(text, out num))
				{
					string text2 = this.cmbSte.Text;
					Settings.smethod_37(this.cmbSte, VfoForm.SZ_STE);
					if (this.cmbSte.FindString(text2) < 0)
					{
						this.cmbSte.SelectedIndex = 0;
					}
					else
					{
						this.cmbSte.SelectedItem = text2;
					}
				}
			}
		}

		private void btnCopy_Click(object sender, EventArgs e)
		{
			this.txtTxFreq.Text = this.txtRxFreq.Text;
		}

		static VfoForm()
		{
			
			VfoForm.SPACE_CH = Marshal.SizeOf(typeof(ChannelOne));
			VfoForm.SZ_CH_MODE = new string[2]
			{
				"Analog",
				"Digital"
			};
			VfoForm.SZ_REF_FREQ = new string[3]
			{
				"Low",
				"Middle",
				"High"
			};
			VfoForm.SZ_POWER = new string[2]
			{
				"Low",
				"High"
			};
			VfoForm.SZ_BAND_TYPE = new string[2]
			{
				"V",
				"U"
			};
			VfoForm.SZ_ADMIT_CRITERICA = new string[3]
			{
				"Always",
				"Channel Free",
				"CTCSS/DCS"
			};
			VfoForm.SZ_ADMIT_CRITERICA_D = new string[3]
			{
				"Always",
				"Channel Free",
				"Color Code"
			};
			VfoForm.SZ_BANDWIDTH = new string[2]
			{
				"12.5",
				"25"
			};
			VfoForm.SZ_SQUELCH = new string[2]
			{
				"Tight",
				"Normal"
			};
			VfoForm.SZ_SQUELCH_LEVEL = new string[]
			{
				"Disabled",
				"Open",
				"5%",
				"10%",
				"15%",
				"20%",
				"25%",
				"30%",
				"35%",
				"40%",
				"45%",
				"50%",
				"55%",
				"60%",
				"65%",
				"70%",
				"75%",
				"80%",
				"85%",
				"90%",
				"95%",
				"Closed"
			};
			VfoForm.SZ_VOICE_EMPHASIS = new string[4]
			{
				"None",
				"De & Pre",
				"De Only",
				"Pre Only"
			};
			VfoForm.SZ_STE = new string[4]
			{
				"Frequency",
				"120°",
				"180°",
				"240°"
			};
			VfoForm.SZ_NON_STE = new string[2]
			{
				"Off",
				"Frequency"
			};
			VfoForm.SZ_SIGNALING_SYSTEM = new string[2]
			{
				"Off",
				"DTMF"
			};
			VfoForm.SZ_UNMUTE_RULE = new string[3]
			{
				"Std Unmute, Mute",
				"And Unmute, Mute",
				"And Unmute, Or Mute"
			};
			VfoForm.SZ_PTTID_TYPE = new string[4]
			{
				"None",
				"Only Front",
				"Only Post",
				"Front & Post"
			};
			VfoForm.SZ_ARTS = new string[4]
			{
				"Disable",
				"Tx",
				"Rx",
				"Tx & Rx"
			};
			VfoForm.SZ_TIMING_PREFERENCE = new string[3]
			{
				"Preferred",
				"Eligibel",
				"Ineligibel"
			};
			VfoForm.SZ_REPEATER_SOLT = new string[2]
			{
				"1",
				"2"
			};
			VfoForm.SZ_ARS = new string[2]
			{
				"Disable",
				"On System Change"
			};
			VfoForm.SZ_KEY_SWITCH = new string[2]
			{
				"Off",
				"On"
			};
			VfoForm.SZ_OFFSET_DIRECTION = new string[3]
			{
				"None",
				"+",
				"-"
			};
			VfoForm.SZ_OFFSET_STEP = new string[8]
			{
				"2.5",
				"5",
				"6.25",
				"10",
				"12.5",
				"25",
				"30",
				"50"
			};
			VfoForm.SCL_OFFSET_FREQ = 0.01m;
			VfoForm.data = new Vfo();
		}
	}
}
