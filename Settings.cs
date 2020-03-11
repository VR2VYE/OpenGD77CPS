using DMR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;

internal class Settings
{
	public enum UserMode
	{
		Basic,
		Expert
	}

	public const string SZ_PWD = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz\b";
	public const string SZ_HEX = "0123456789ABCDEF";
	public const string SZ_ATTRIBUTE_NAME = "Id";
	public const string SZ_ATTRIBUTE_VALUE = "Text";
	public const string SZ_NONE_NAME = "None";
	public const string SZ_SELECTED_NAME = "Selected";
	public const string SZ_ADD_NAME = "Add";
	public const string SZ_OFF_NAME = "Off";
	public const string SZ_DEVICE_NOT_FOUND_N = "DeviceNotFound";
	public const string SZ_OPEN_PORT_FAIL_NAME = "OpenPortFail";
	public const string SZ_COMM_ERROR_N = "CommError";
	public const string SZ_MODEL_NOT_MATCH_N = "Model does not match";
	public const string SZ_READ_NAME = "Read";
	public const string SZ_WRITE_NAME = "Write";
	public const string SZ_READ_COMPLETE_NAME = "ReadComplete";
	public const string SZ_WRITE_COMPLETE_NAME = "WriteComplete";
	public const string SZ_KEYPRESS_DTMF_NAME = "KeyPressDtmf";
	public const string SZ_KEYPRESS_HEX_NAME = "KeyPressHex";
	public const string SZ_KEYPRESS_DIGIT_N = "KeyPressDigit";
	public const string SZ_KEYPRESS_PRINT_N = "KeyPressPrint";
	public const string SZ_DATA_FORMAT_ERROR_N = "DataFormatError";
	public const string SZ_FIRST_CH_NOT_DELETE_NAME = "FirstChNotDelete";

	public const string SZ_FIRST_NOT_DELETE_N = "FirstNotDelete";

	public const string SZ_NAME_EXIST = "Name exists";

	public const string SZ_FILE_FORMAT_ERROR_N = "FileFormatError";

	public const string SZ_OPEN_SUCCESSFULLY_N = "OpenSuccessfully";

	public const string SZ_SAVE_SUCCESSFULLY_N = "SaveSuccessfully";

	public const string SZ_TYPE_NOT_MATCH_N = "TypeNotMatch";

	public const string SZ_EXPORT_SUCCESS_N = "ExportSuccess";

	public const string SZ_IMPORT_SUCCESS_N = "ImportSuccess";

	public const string SZ_ID_NOT_EMPTY_N = "IdNotEmpty";

	public const string SZ_ID_OUT_OF_RANGE_N = "IdOutOfRange";

	public const string SZ_ID_ALREADY_EXISTS_N = "IdAlreadyExists";

	public const string SZ_NOT_SELECT_ITEM_NOT_COPYITEM_NAME = "NotSelectItemNotCopyItem";

	public const string SZ_PROMPT_KEY1_NAME = "PromptKey1";

	public const string SZ_PROMPT_KEY2_NAME = "PromptKey2";

	public const string SZ_APP_SCAN_SELECTED_NAME = "ScanSelected";

	public const string SZ_UNABLE = "Unable to operate selected";

	public const string SZ_PROMPT_NAME = "Prompt";

	public const string SZ_ERROR_NAME = "Error";

	public const string SZ_WARNING_NAME = "Warning";

	public const int FREQ_STEP_1 = 250;

	public const int FREQ_STEP_2 = 625;

	public const string SUPER_PWD = "DT8168";

	public const int LEN_NAME_MCU = 15;

	private static UserMode curUserMode;

	public static readonly byte[] CUR_MODEL;

	public static string SZ_NONE;

	public static string SZ_SELECTED;

	public static string SZ_ADD;

	public static string SZ_OFF;

	public static string SZ_DEVICE_NOT_FOUND;

	public static string SZ_OPEN_PORT_FAIL;

	public static string SZ_COMM_ERROR;

	public static string SZ_MODEL_NOT_MATCH;

	public static string SZ_READ;

	public static string SZ_WRITE;

	public static string SZ_READ_COMPLETE;

	public static string SZ_WRITE_COMPLETE;

    public static string SZ_CODEPLUG_READ_CONFIRM;
    public static string SZ_CODEPLUG_WRITE_CONFIRM;
	public static string SZ_PLEASE_CONFIRM;
	public static string SZ_USER_AGREEMENT = "This software is supplied 'as is' with no warranties. You use it at your own risk to both your PC and to your DMR Radio. By pressing the  Yes   button you agree and understand.";

	public static string SZ_KEYPRESS_DTMF;

	public static string SZ_KEYPRESS_HEX;

	public static string SZ_KEYPRESS_DIGIT;

	public static string SZ_KEYPRESS_PRINT;

	public static string SZ_DATA_FORMAT_ERROR;

	public static string SZ_FIRST_CH_NOT_DELETE;

	public static string SZ_FIRST_NOT_DELETE;

	public static string SZ_NAME_EXIST_NAME;

	public static string SZ_FILE_FORMAT_ERROR;

	public static string SZ_OPEN_SUCCESSFULLY;

	public static string SZ_SAVE_SUCCESSFULLY;

	public static string SZ_TYPE_NOT_MATCH;

	public static string SZ_EXPORT_SUCCESS;

	public static string SZ_IMPORT_SUCCESS;

	public static string SZ_ID_NOT_EMPTY;

	public static string SZ_ID_OUT_OF_RANGE;

	public static string SZ_ID_ALREADY_EXISTS;

	public static string SZ_NOT_SELECT_ITEM_NOT_COPYITEM;

	public static string SZ_PROMPT_KEY1;

	public static string SZ_PROMPT_KEY2;

	public static string SZ_PROMPT;

	public static string SZ_ERROR;

	public static string SZ_WARNING;
	public static string SZ_DOWNLOADCONTACTS_REGION_EMPTY = "Please enter the 3 digit Region previx code. e.g. 505 for Australia.";
	public static string SZ_DOWNLOADCONTACTS_MESSAGE_ADDED = "There are {0} new ID's which are not already in your contacts";
	public static string SZ_DOWNLOADCONTACTS_DOWNLOADING = "Downloading...";

	public static string SZ_DOWNLOADCONTACTS_SELECT_CONTACTS_TO_IMPORT = "Please select the contacts you would like to import";
	public static string SZ_DOWNLOADCONTACTS_TOO_MANY = "Not all contacts could be imported because the maximum number of Digital Contacts has been reached";
	public static string SZ_UNABLEDOWNLOADFROMINTERNET = "Unable to download data. Please check your Internet connection";
	public static string SZ_IMPORT_COMPLETE = "Import complete";

	public static string SZ_CODEPLUG_UPGRADE_NOTICE = "This appears to be a V3.0.6 Codeplug. It will be converted to V3.1.x";
	public static string SZ_CODEPLUG_UPGRADE_WARNING_TO_MANY_RX_GROUPS = "Version 3.1.x can only have 76 Rx Groups. Additional Rx Groups have been ignored";

	public static string SZ_CODEPLUG_READ = "Reading codeplug from GD-77";
	public static string SZ_CODEPLUG_WRITE = "Writing codeplug to GD-77";
	public static string SZ_DMRID_READ = "Reading DMR ID database from GD-77";
	public static string SZ_DMRID_WRITE = "Writing DMR ID database to GD-77";
	public static string SZ_CALIBRATION_READ = "Reading calibration data from GD-77";
	public static string SZ_CALIBRATION_WRITE = "Writing calibration data to GD-77";
	public static string SZ_CONTACT_DUPLICATE_NAME = "Warning. Duplicate contact name.";

	public static string SZ_EnableMemoryAccessMode = "The GD-77 does not seem to be in Memory Access mode\nHold keys SK2 (Blue side key), Green Menu and * when turning on the transceiver.\nand try again";
    public static string SZ_dataRead = "Reading data from GD-77";
    public static string SZ_dataWrite  ="Writing data to GD-77";
    public static string SZ_DMRIdContcatsTotal = "Total number of IDs = {0}. Max of 10920 can be uploaded";
    public static string SZ_ErrorParsingData = "Error while parsing data";
    public static string SZ_DMRIdIntroMessage = "Data is downloaded from Ham-digital.org and appended any existing data";



	public static int CUR_MODE;

	public static uint[] MIN_FREQ;

	public static uint[] MAX_FREQ;

	public static readonly uint[] VALID_MIN_FREQ;

	public static readonly uint[] VALID_MAX_FREQ;

	public static int CUR_CH_GROUP;

	public static int CUR_ZONE_GROUP;

	public static int CUR_ZONE;

	public static string CUR_PWD;

	public static readonly uint EEROM_SPACE = 0x20000;//0x40000; // Increased to 256k (0x40000) to store DMR ID as well as codeplug   0x20000;// 0131072u;

	public static readonly int SPACE_DEVICE_INFO;

	public static readonly int ADDR_DEVICE_INFO;

	public static readonly int OFS_LAST_PRG_TIME;

	public static readonly int OFS_CPS_SW_VER;

	public static readonly int OFS_MODEL;

	public static readonly int SPACE_GENERAL_SET;

	public static readonly int ADDR_GENERAL_SET;

	public static readonly int ADDR_PWD;

	public static readonly int SPACE_BUTTON;

	public static readonly int ADDR_BUTTON;

	public static readonly int SPACE_ONE_TOUCH;

	public static readonly int ADDR_ONE_TOUCH;

	public static readonly int SPACE_TEXT_MSG;

	public static readonly int ADDR_TEXT_MSG;

	public static readonly int SPACE_ENCRYPT;

	public static readonly int ADDR_ENCRYPT;

	public static readonly int SPACE_SIGNALING_BASIC;

	public static readonly int ADDR_SIGNALING_BASIC;

	public static readonly int SPACE_DTMF_BASIC;

	public static readonly int ADDR_DTMF_BASIC;

	public static readonly int SPACE_EMG_SYSTEM;

	public static readonly int ADDR_EMG_SYSTEM;

	public static readonly int SPACE_DMR_CONTACT;

	public static readonly int ADDR_DMR_CONTACT;

	public static readonly int SPACE_DMR_CONTACT_EX;

	public static readonly int ADDR_DMR_CONTACT_EX;

	public static readonly int SPACE_DTMF_CONTACT;

	public static readonly int ADDR_DTMF_CONTACT;

	public static readonly int SPACE_RX_GRP_LIST;

	//public static readonly int ADDR_RX_GRP_LIST;

	public static readonly int ADDR_RX_GRP_LIST_EX;

	public static readonly int ADDR_ZONE_BASIC;

	public static readonly int ADDR_ZONE_LIST;

	public static readonly int ADDR_CHANNEL;

	public static readonly int SPACE_SCAN_BASIC;

	public static readonly int ADDR_SCAN;

	public static readonly int SPACE_SCAN_LIST;

	public static readonly int ADDR_SCAN_LIST;

	public static readonly int SPACE_BOOT_ITEM;

	public static readonly int ADDR_BOOT_ITEM;

	public static readonly int SPACE_DIGITAL_KEY_CONTACT;

	public static readonly int ADDR_DIGITAL_KEY_CONTACT;

	public static readonly int SPACE_MENU_CONFIG;

	public static readonly int ADDR_MENU_CONFIG;

	public static readonly int SPACE_BOOT_CONTENT;

	public static readonly int ADDR_BOOT_CONTENT;

	public static readonly int SPACE_ATTACHMENT;

	public static readonly int ADDR_ATTACHMENT;

	public static readonly int SPACE_VFO;

	public static readonly int ADDR_VFO;

	public static readonly int SPACE_EX_ZONE;

	public static readonly int ADDR_EX_ZONE;

	public static readonly int ADDR_EX_ZONE_BASIC;

	public static readonly int ADDR_EX_ZONE_LIST;

	public static readonly int SPACE_EX_SCAN;

	public static readonly int ADDR_EX_SCAN;

	public static readonly int ADDR_EX_SCAN_PRI_CH1;

	public static readonly int ADDR_EX_SCAN_PRI_CH2;

	public static readonly int ADDR_EX_SCAN_SPECIFY_CH;

	public static readonly int ADDR_EX_SCAN_CH_LIST;

	public static readonly int SPACE_EX_EMERGENCY;

	public static readonly int ADDR_EX_EMERGENCY;

	public static readonly int SPACE_EX_CH;

	public static readonly int ADDR_EX_CH;

	public static readonly int ADDR_UNUSED_START = 0x1EE60;

	public static Dictionary<string, string> dicCommon;
   // static string _003CLangXml_003Ek__BackingField;
	private static XmlDocument _languageXML=null;


	public static XmlDocument languageXML
	{
		get { return _languageXML; }
	}

	public static void setLanguageXMLFile(string xmlFile)
	{
		_languageXML = new XmlDocument();
		_languageXML.Load(xmlFile);
	}

	/*
	[CompilerGenerated]
	public static string smethod_0()
	{

		return Settings._003CLangXml_003Ek__BackingField;
	}

	
	[CompilerGenerated]
	public static void smethod_1(string string_0)
	{
		if (string_0 != null)
		{
			Settings._003CLangXml_003Ek__BackingField = string_0;
		}
		//debugging only
		//else
		//{
		//	System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
		//	MessageBox.Show(t.ToString(),"Warning. Code attempted to set language xml path to null");
		//}
	}
	 */
    static string _003CLangChm_003Ek__BackingField;
	[CompilerGenerated]
	public static string smethod_2()
	{
		return Settings._003CLangChm_003Ek__BackingField;
	}

	[CompilerGenerated]
	public static void smethod_3(string string_0)
	{
		Settings._003CLangChm_003Ek__BackingField = string_0;
	}

	public static UserMode getUserExpertSettings()
	{
		return Settings.UserMode.Expert;
//		return Settings.curUserMode;
	}

	public static void smethod_5(UserMode userMode_0)
	{
		Settings.curUserMode = userMode_0;
	}

    static SizeF _003CFactor_003Ek__BackingField;
	[CompilerGenerated]
	public static SizeF smethod_6()
	{
		return Settings._003CFactor_003Ek__BackingField;
	}

	[CompilerGenerated]
	public static void smethod_7(SizeF sizeF_0)
	{
		Settings._003CFactor_003Ek__BackingField = sizeF_0;
	}

    static string _003CCurUserPwd_003Ek__BackingField;
	[CompilerGenerated]
	public static string smethod_8()
	{
		return Settings._003CCurUserPwd_003Ek__BackingField;
	}

	[CompilerGenerated]
	public static void smethod_9(string string_0)
	{
		Settings._003CCurUserPwd_003Ek__BackingField = string_0;
	}

	public static void smethod_10()
	{
		Settings.smethod_76("None", ref Settings.SZ_NONE);
		Settings.smethod_76("Selected", ref Settings.SZ_SELECTED);
		Settings.smethod_76("Add", ref Settings.SZ_ADD);
		Settings.smethod_76("Off", ref Settings.SZ_OFF);
		Settings.smethod_76("DeviceNotFound", ref Settings.SZ_DEVICE_NOT_FOUND);
		Settings.smethod_76("CommError", ref Settings.SZ_COMM_ERROR);
		Settings.smethod_76("Model does not match", ref Settings.SZ_MODEL_NOT_MATCH);
		Settings.smethod_76("DataFormatError", ref Settings.SZ_DATA_FORMAT_ERROR);
	}

	public static void smethod_11(ref byte byte_0, byte byte_1, byte byte_2, byte byte_3)
	{
		if (!Settings.smethod_12(byte_0, byte_1, byte_2))
		{
			byte_0 = byte_3;
		}
	}

	public static bool smethod_12(byte byte_0, byte byte_1, byte byte_2)
	{
		if (byte_0 >= byte_1 && byte_0 <= byte_2)
		{
			return true;
		}
		return false;
	}

	public static bool smethod_13(int int_0, int int_1, int int_2)
	{
		if (int_0 >= int_1 && int_0 <= int_2)
		{
			return true;
		}
		return false;
	}

	public static int smethod_14(byte byte_0, int int_0, int int_1)
	{
		int num = (int)Math.Pow(2.0, (double)int_1) - 1;
		return byte_0 >> int_0 & num;
	}

	public static void smethod_15(ref byte byte_0, int int_0, int int_1)
	{
		int num = (int)Math.Pow(2.0, (double)int_1) - 1;
		byte_0 &= (byte)(~(num << int_0));
	}

	public static void smethod_16(ref byte byte_0, int int_0, int int_1, int int_2)
	{
		int num = (int)Math.Pow(2.0, (double)int_1) - 1;
		byte_0 &= (byte)(~(num << int_0));
		byte_0 |= (byte)((int_2 & num) << int_0);
	}

	public static void smethod_17(ref ushort ushort_0, int int_0, int int_1)
	{
		int num = (int)Math.Pow(2.0, (double)int_1) - 1;
		ushort_0 &= (ushort)(~(num << int_0));
	}

	public static bool smethod_18(byte[] byte_0, byte[] byte_1, int int_0)
	{
		if (byte_0.Length < int_0)
		{
			return false;
		}
		if (byte_1.Length < int_0)
		{
			return false;
		}
		int num = 0;
		while (true)
		{
			if (num < int_0)
			{
				if (byte_0[num] == byte_1[num])
				{
					num++;
					continue;
				}
				break;
			}
			return true;
		}
		return false;
	}

	public static int smethod_19(double double_0, ref uint uint_0)
	{
		int num = 0;
		uint_0 = 0u;
		num = 0;
		while (true)
		{
			if (num < Settings.MIN_FREQ.Length)
			{
				if (Settings.MIN_FREQ[num] < Settings.MAX_FREQ[num] && uint_0 == 0)
				{
					uint_0 = Settings.MIN_FREQ[num];
				}
				if (double_0 >= (double)Settings.MIN_FREQ[num] && !(double_0 > (double)Settings.MAX_FREQ[num]))
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

	public static int smethod_20(double double_0, double double_1)
	{
		int num = 0;
		int num2 = -1;
		int num3 = -1;
		num = 0;
		while (num < Settings.MIN_FREQ.Length)
		{
			if (!(double_0 >= (double)Settings.MIN_FREQ[num]) || double_0 > (double)Settings.MAX_FREQ[num])
			{
				num++;
				continue;
			}
			num2 = num;
			break;
		}
		num = 0;
		while (num < Settings.MAX_FREQ.Length)
		{
			if (!(double_1 >= (double)Settings.MIN_FREQ[num]) || double_1 > (double)Settings.MAX_FREQ[num])
			{
				num++;
				continue;
			}
			num3 = num;
			break;
		}
		if (num2 == num3 && num2 != -1)
		{
			return num2;
		}
		return -1;
	}

	public static int smethod_21(uint uint_0, ref int int_0)
	{
		int num = 0;
		int_0 = -1;
		num = 0;
		while (true)
		{
			if (num < Settings.MIN_FREQ.Length)
			{
				if (Settings.VALID_MIN_FREQ[num] < Settings.VALID_MAX_FREQ[num] && int_0 < 0)
				{
					int_0 = num;
				}
				if (uint_0 >= Settings.VALID_MIN_FREQ[num] && uint_0 <= Settings.VALID_MAX_FREQ[num])
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

	public static int smethod_22(double double_0, double double_1)
	{
		int num = 0;
		int num2 = -1;
		int num3 = -1;
		num = 0;
		while (num < Settings.VALID_MIN_FREQ.Length)
		{
			if (!(double_0 >= (double)Settings.VALID_MIN_FREQ[num]) || double_0 > (double)Settings.VALID_MAX_FREQ[num])
			{
				num++;
				continue;
			}
			num2 = num;
			break;
		}
		num = 0;
		while (num < Settings.VALID_MAX_FREQ.Length)
		{
			if (!(double_1 >= (double)Settings.VALID_MIN_FREQ[num]) || double_1 > (double)Settings.VALID_MAX_FREQ[num])
			{
				num++;
				continue;
			}
			num3 = num;
			break;
		}
		if (num2 == num3 && num2 != -1)
		{
			return num2;
		}
		return -1;
	}

	public static byte[] smethod_23(string string_0)
	{
		return Settings.smethod_24(string_0, "gb2312");
	}

	public static byte[] smethod_24(string string_0, string string_1)
	{
		Encoding encoding = Encoding.GetEncoding(string_1);
		if (encoding == null)
		{
			encoding = Encoding.Default;
		}
		return encoding.GetBytes(string_0);
	}

	public static string smethod_25(byte[] byte_0)
	{
		return Settings.smethod_26(byte_0, "gb2312");
	}

	public static string smethod_26(byte[] byte_0, string string_0)
	{
		Encoding encoding = Encoding.GetEncoding(string_0);
		if (encoding == null)
		{
			encoding = Encoding.Default;
		}
		int num = Array.IndexOf(byte_0, (byte)255);
		if (num == -1)
		{
			num = Array.IndexOf(byte_0, (byte)0);
			if (num == -1)
			{
				num = byte_0.Length;
			}
		}
		return encoding.GetString(byte_0, 0, num);
	}

	public static int smethod_27(double double_0, double double_1)
	{
		decimal d = Convert.ToDecimal(double_0);
		decimal d2 = Convert.ToDecimal(double_1);
		return Convert.ToInt32(d * d2);
	}

	public static double smethod_28(int int_0, int int_1)
	{
		decimal d = Convert.ToDecimal(int_0);
		decimal d2 = Convert.ToDecimal(int_1);
		return Convert.ToDouble(d / d2);
	}

	public static void smethod_29(ref int int_0, int int_1, int int_2)
	{
		int num = int_0 % int_1;
		int num2 = int_0 % int_2;
		if (num != 0 && num2 != 0)
		{
			int num3 = 250 - num;
			int num4 = 625 - num2;
			if (num3 < num4)
			{
				int_0 += num3;
			}
			else
			{
				int_0 += num4;
			}
		}
	}

	public static void smethod_30(ref int int_0, int int_1, int int_2)
	{
		int num = 1;
		int num2 = 0;
		int num3 = 0;
		int[] array = new int[4]
		{
			int_0 % int_1,
			0,
			0,
			0
		};
		array[1] = int_1 - array[0];
		array[2] = int_0 % int_2;
		array[3] = int_2 - array[2];
		num2 = array[0];
		for (num = 1; num < array.Length; num++)
		{
			if (array[num] < num2)
			{
				num2 = array[num];
				num3 = num;
			}
		}
		if (num3 % 2 == 0)
		{
			int_0 -= num2;
		}
		else
		{
			int_0 += num2;
		}
	}

	public static void smethod_31(ref int int_0)
	{
		int num = int_0 % 625;
		if (num != 0)
		{
			int num2 = 0;
			while (num2 < 3)
			{
				num += 25;
				if (num % 625 != 0)
				{
					num2++;
					continue;
				}
				int_0 += (num2 + 1) * 25;
				break;
			}
			if (num2 == 3)
			{
				int_0 = (int_0 + 250) / 250 * 250;
			}
		}
	}

	public static ushort smethod_32(ushort ushort_0)
	{
		int num = 0;
		int num2 = 0;
		ushort num3 = 0;
		for (num = 0; num < 4; num++)
		{
			num2 = (ushort_0 & 0xF);
			ushort_0 = (ushort)(ushort_0 >> 4);
			num3 = (ushort)((double)(int)num3 + (double)num2 * Math.Pow(10.0, (double)num));
		}
		return num3;
	}

	public static ushort smethod_33(ushort ushort_0)
	{
		int num = 0;
		int num2 = 0;
		ushort num3 = 0;
		for (num = 0; num < 4; num++)
		{
			num2 = (int)ushort_0 % 10;
			ushort_0 = (ushort)((int)ushort_0 / 10);
			num3 = (ushort)((double)(int)num3 + (double)num2 * Math.Pow(16.0, (double)num));
		}
		return num3;
	}

	public static uint smethod_34(uint uint_0)
	{
		int num = 0;
		uint num2 = 0u;
		uint num3 = 0u;
		for (num = 0; num < 8; num++)
		{
			num2 = (uint_0 & 0xF);
			uint_0 >>= 4;
			num3 += num2 * (uint)Math.Pow(10.0, (double)num);
		}
		return num3;
	}

	public static uint smethod_35(uint uint_0)
	{
		int num = 0;
		uint num2 = 0u;
		uint num3 = 0u;
		for (num = 0; num < 8; num++)
		{
			num2 = uint_0 % 10u;
			uint_0 /= 10u;
			num3 += num2 * (uint)Math.Pow(16.0, (double)num);
		}
		return num3;
	}

	public static void smethod_36(CustomNumericUpDown class12_0, Class13 class13_0)
	{
		if (class13_0.method_6() < 0m)
		{
			class12_0.Minimum = (decimal)class13_0.method_2() * class13_0.method_6();
			class12_0.Maximum = (decimal)class13_0.method_0() * class13_0.method_6();
			class12_0.Increment = Math.Abs((decimal)class13_0.method_4() * class13_0.method_6());
		}
		else
		{
			class12_0.Minimum = (decimal)class13_0.method_0() * class13_0.method_6();
			class12_0.Maximum = (decimal)class13_0.method_2() * class13_0.method_6();
			class12_0.Increment = (decimal)class13_0.method_4() * class13_0.method_6();
		}
		class12_0.method_0(class13_0.method_8());
	}

	public static void smethod_37(ComboBox comboBox_0, string[] string_0)
	{
		int num = 0;
		comboBox_0.Items.Clear();
		for (num = 0; num < string_0.Length; num++)
		{
			comboBox_0.Items.Add(string_0[num]);
		}
	}

	public static void smethod_38(ComboBox comboBox_0, string[] string_0, int int_0)
	{
		int num = 0;
		int num2 = Math.Min(string_0.Length, int_0);
		comboBox_0.Items.Clear();
		for (num = 0; num < num2; num++)
		{
			comboBox_0.Items.Add(string_0[num]);
		}
	}

	public static void smethod_39(CustomCombo class4_0, string[] string_0)
	{
		int num = 0;
		class4_0.method_0();
		foreach (string string_ in string_0)
		{
			class4_0.method_1(string_, num++);
		}
	}

	public static void smethod_40(CustomCombo class4_0, string[] string_0, int[] int_0)
	{
		class4_0.method_0();
		foreach (int num in int_0)
		{
			class4_0.method_1(string_0[num], num);
		}
	}

	public static void smethod_41(ComboBox comboBox_0, int int_0, int int_1)
	{
		int num = 0;
		comboBox_0.Items.Clear();
		for (num = int_0; num <= int_1; num++)
		{
			comboBox_0.Items.Add(num);
		}
	}

	public static void smethod_42(ComboBox comboBox_0, string string_0, int int_0, int int_1)
	{
		int num = 0;
		comboBox_0.Items.Clear();
		comboBox_0.Items.Add(string_0);
		for (num = int_0; num <= int_1; num++)
		{
			comboBox_0.Items.Add(num);
		}
	}

	public static void smethod_43(ComboBox comboBox_0, int int_0, int int_1, int int_2, string string_0)
	{
		int i = 0;
		comboBox_0.Items.Clear();
		for (i = int_0; i <= int_1; i++)
		{
			if (int_2 == i)
			{
				comboBox_0.Items.Add(string_0);
			}
			else
			{
				comboBox_0.Items.Add(i.ToString());
			}
		}
	}

	public static void smethod_44(CustomCombo class4_0, IData idata_0)
	{
		int num = 0;
		string text = "";
		class4_0.method_0();
		class4_0.method_1(Settings.SZ_NONE, 0);
		for (num = 0; num < idata_0.Count; num++)
		{
			if (idata_0.DataIsValid(num))
			{
				text = idata_0.GetName(num);
				class4_0.method_1(text, num + 1);
			}
		}
	}

	public static void smethod_45(CustomCombo class4_0, string[] string_0, IData idata_0)
	{
		int num = 0;
		string text = "";
		class4_0.method_0();
		for (num = 0; num < string_0.Length; num++)
		{
			class4_0.method_1(string_0[num], num);
		}
		for (num = 0; num < idata_0.Count; num++)
		{
			if (idata_0.DataIsValid(num))
			{
				text = string.Format("{0:d3}:{1}", num + 1, idata_0.GetName(num));
				class4_0.method_1(text, num + string_0.Length);
			}
		}
	}

	public static void smethod_46(CustomCombo class4_0, string[] string_0, ListBox listBox_0)
	{
		int num = 0;
		string text = "";
		class4_0.method_0();
		for (num = 0; num < string_0.Length; num++)
		{
			class4_0.method_1(string_0[num], num);
		}
		for (num = 0; num < listBox_0.Items.Count; num++)
		{
			text = listBox_0.Items[num].ToString();
			SelectedItemUtils @class = listBox_0.Items[num] as SelectedItemUtils;
			if (@class != null)
			{
				class4_0.method_1(text, @class.Value);
			}
		}
	}

	public static int smethod_47(ListBox listBox_0, SelectedItemUtils class14_0)
	{
		int num = 0;
		num = 0;
		while (true)
		{
			if (num < listBox_0.Items.Count)
			{
				SelectedItemUtils @class = (SelectedItemUtils)listBox_0.Items[num];
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

	public static void smethod_48(TreeNode treeNode_0, int int_0)
	{
		if (int_0 >= treeNode_0.Level)
		{
			treeNode_0.Expand();
			foreach (TreeNode node in treeNode_0.Nodes)
			{
				Settings.smethod_48(node, int_0);
			}
		}
	}

	public static void smethod_49(TreeView treeView_0, int int_0)
	{
		foreach (TreeNode node in treeView_0.Nodes)
		{
			Settings.smethod_48(node, int_0);
		}
	}

	public static bool smethod_50(TreeNode treeNode_0, string string_0)
	{
		if (string.IsNullOrEmpty(string_0))
		{
			return true;
		}
		foreach (TreeNode node in treeNode_0.Parent.Nodes)
		{
			if (node != treeNode_0 && node.Text.Trim() == string_0.Trim())
			{
				return true;
			}
		}
		return false;
	}

	public static bool smethod_51(TreeNode treeNode_0, string string_0)
	{
		if (string.IsNullOrEmpty(string_0))
		{
			return true;
		}
		foreach (TreeNode node in treeNode_0.Nodes)
		{
			if (node.Text.Trim() == string_0.Trim())
			{
				return true;
			}
		}
		return false;
	}

	public static void smethod_52(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		string helpId = dataGridView.FindForm().Name + "_" + dataGridView.Columns[e.ColumnIndex].Name;
		MainForm mainForm = Application.OpenForms[0] as MainForm;
		if (mainForm != null)
		{
			mainForm.ShowHelp(helpId);
		}
	}

	public static void smethod_53(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar >= '\0' && e.KeyChar <= '\u007f')
		{
			return;
		}
		if (e.KeyChar != '\b' && e.KeyChar != '.')
		{
			MessageBox.Show(Settings.dicCommon["KeyPressPrint"]);
			e.Handled = true;
		}
	}

	public static void smethod_54(object sender, KeyPressEventArgs e)
	{
	}

	public static void smethod_55(object sender, KeyPressEventArgs e)
	{
		if (!char.IsControl(e.KeyChar))
		{
			NumberFormatInfo numberFormat = CultureInfo.CurrentCulture.NumberFormat;
			if (e.KeyChar >= '0' && e.KeyChar <= '9')
			{
				return;
			}
			if (!numberFormat.NumberDecimalSeparator.Contains(e.KeyChar))
			{
				MessageBox.Show(string.Format(Settings.dicCommon["KeyPressDigit"], numberFormat.NumberDecimalSeparator));
				e.Handled = true;
			}
		}
	}

	public static bool smethod_56(string string_0)
	{
		char[] array = string_0.ToCharArray();
		int num = 0;
		while (true)
		{
			if (num < array.Length)
			{
				char c = array[num];
				if (c < '\0')
				{
					break;
				}
				if (c > '\u007f')
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

	public static void smethod_57(object sender, KeyPressEventArgs e)
	{
		if ("0123456789ABCD*#\b".IndexOf(char.ToUpper(e.KeyChar)) < 0 && e.KeyChar != '\b' && e.KeyChar != '.')
		{
			string str = Regex.Replace("0123456789ABCD*#\b", "[^\\dA-D\\*#]*", "");
			MessageBox.Show(Settings.dicCommon["KeyPressDtmf"] + str);
			e.Handled = true;
		}
	}

	public static void smethod_58(object sender, KeyPressEventArgs e)
	{
		if ("0123456789ABCDEF".IndexOf(char.ToUpper(e.KeyChar)) < 0 && e.KeyChar != '\b' && e.KeyChar != '.')
		{
			MessageBox.Show("KeyPressHex0123456789ABCDEF");
			e.Handled = true;
		}
	}

	public static void smethod_59(Control.ControlCollection controlCollection_0)
	{
		foreach (Control item in controlCollection_0)
		{
			if (!(item is Button) && !(item is TextBox) && !(item is ListBox) && !(item is NumericUpDown) && !(item is ComboBox) && !(item is CheckBox))
			{
				if (item is DataGridView)
				{
					DataGridView dataGridView = item as DataGridView;
					dataGridView.CellEnter += Settings.smethod_52;
				}
				else if (item.Controls.Count > 0)
				{
					Settings.smethod_59(item.Controls);
				}
			}
			else
			{
				item.Enter += Settings.smethod_60;
			}
		}
	}

	public static void smethod_60(object sender, EventArgs e)
	{
		Control control = sender as Control;
		string helpId = control.FindForm().Name + "_" + control.Name;
		MainForm mainForm = Application.OpenForms[0] as MainForm;
		if (mainForm != null)
		{
			mainForm.ShowHelp(helpId);
		}
	}

	public static byte[] objectToByteArray(object object_0, int int_0)
	{
		byte[] array = new byte[int_0];
		IntPtr intPtr = Marshal.AllocHGlobal(int_0);
		Marshal.StructureToPtr(object_0, intPtr, false);
		Marshal.Copy(intPtr, array, 0, int_0);
		Marshal.FreeHGlobal(intPtr);
		return array;
	}

	public static object smethod_62(byte[] byte_0, Type type_0)
	{
		int num = Marshal.SizeOf(type_0);
		if (num > byte_0.Length)
		{
			throw new ArgumentException();
		}
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		Marshal.Copy(byte_0, 0, intPtr, num);
		object result = Marshal.PtrToStructure(intPtr, type_0);
		Marshal.FreeHGlobal(intPtr);
		return result;
	}

	public static void smethod_63(string string_0)
	{
		FileStream fileStream = new FileStream(string_0, FileMode.Create);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Serialize(fileStream, GeneralSetForm.data);
		binaryFormatter.Serialize(fileStream, ButtonForm.data);
		binaryFormatter.Serialize(fileStream, ButtonForm.data1);
		binaryFormatter.Serialize(fileStream, TextMsgForm.data);
		binaryFormatter.Serialize(fileStream, EncryptForm.data);
		binaryFormatter.Serialize(fileStream, SignalingBasicForm.data);
		binaryFormatter.Serialize(fileStream, DtmfForm.data);
		binaryFormatter.Serialize(fileStream, EmergencyForm.data);
		binaryFormatter.Serialize(fileStream, DtmfContactForm.data);
		binaryFormatter.Serialize(fileStream, ContactForm.data);
		binaryFormatter.Serialize(fileStream, RxGroupListForm.data);
		binaryFormatter.Serialize(fileStream, ZoneForm.data);
		binaryFormatter.Serialize(fileStream, ChannelForm.data);
		binaryFormatter.Serialize(fileStream, ScanBasicForm.data);
		binaryFormatter.Serialize(fileStream, NormalScanForm.data);
		fileStream.Close();
	}

	public static void smethod_64(string string_0)
	{
		FileStream fileStream = new FileStream(string_0, FileMode.Open, FileAccess.Read, FileShare.Read);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		GeneralSetForm.data = (GeneralSetForm.GeneralSet)binaryFormatter.Deserialize(fileStream);
		ButtonForm.data = (ButtonForm.SideKey)binaryFormatter.Deserialize(fileStream);
		ButtonForm.data1 = (ButtonForm.OneTouch)binaryFormatter.Deserialize(fileStream);
		TextMsgForm.data = (TextMsgForm.TextMsg)binaryFormatter.Deserialize(fileStream);
		EncryptForm.data = (EncryptForm.Encrypt)binaryFormatter.Deserialize(fileStream);
		SignalingBasicForm.data = (SignalingBasicForm.SignalingBasic)binaryFormatter.Deserialize(fileStream);
		DtmfForm.data = (DtmfForm.Dtmf)binaryFormatter.Deserialize(fileStream);
		EmergencyForm.data = (EmergencyForm.Emergency)binaryFormatter.Deserialize(fileStream);
		DtmfContactForm.data = (DtmfContactForm.DtmfContact)binaryFormatter.Deserialize(fileStream);
		ContactForm.data = (ContactForm.Contact)binaryFormatter.Deserialize(fileStream);
		RxGroupListForm.data = (RxListData)binaryFormatter.Deserialize(fileStream);
		ZoneForm.data = (ZoneForm.Zone)binaryFormatter.Deserialize(fileStream);
		ChannelForm.data = (ChannelForm.Channel)binaryFormatter.Deserialize(fileStream);
		ScanBasicForm.data = (ScanBasicForm.ScanBasic)binaryFormatter.Deserialize(fileStream);
		NormalScanForm.data = (NormalScanForm.NormalScan)binaryFormatter.Deserialize(fileStream);
		fileStream.Close();
	}

	public static QkGVc1MQ9NxKRGCTdE smethod_65<QkGVc1MQ9NxKRGCTdE>(QkGVc1MQ9NxKRGCTdE CeqCQcoTiZ0ZwY0OmE)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		MemoryStream memoryStream = new MemoryStream();
		binaryFormatter.Serialize(memoryStream, CeqCQcoTiZ0ZwY0OmE);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		QkGVc1MQ9NxKRGCTdE result = (QkGVc1MQ9NxKRGCTdE)binaryFormatter.Deserialize(memoryStream);
		memoryStream.Close();
		return result;
	}

	public static string smethod_66(string string_0)
	{
		if (Settings.dicCommon.ContainsKey(string_0))
		{
			return Settings.dicCommon[string_0];
		}
		return "";
	}

	public static string smethod_67(string string_0)
	{
		//XmlDocument xmlDocument = new XmlDocument();
		//xmlDocument.Load(Settings._003CLangXml_003Ek__BackingField);
		new Dictionary<string, string>();
		string xpath = string.Format("/Resource/Settings/Item[@Id='{0}']", string_0);
		try
		{
			XmlNode xmlNode = _languageXML.SelectSingleNode(xpath);
			if (xmlNode != null && xmlNode.Attributes["Text"] != null)
			{
				return xmlNode.Attributes["Text"].Value;
			}
		}
		catch
		{
			return "";
		}
		return "";
	}

	public static void smethod_68(Form form_0)
	{
		new Dictionary<string, string>();
		string xpath = string.Format("/Resource/{0}", form_0.Name);
		XmlNode xmlNode = _languageXML.SelectSingleNode(xpath);
		try
		{
			form_0.Text = xmlNode.Attributes["Text"].Value;
			DockContent dockContent = form_0 as DockContent;
			if (dockContent != null)
			{
				dockContent.TabText = xmlNode.Attributes["Text"].Value;
			}
			Settings.smethod_69(form_0.smethod_12(), form_0.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void smethod_69(List<Control> Db4kySLQw7tX1WlNpo, string string_0)
	{
		//XmlDocument xmlDocument = new XmlDocument();
		//xmlDocument.Load(Settings._003CLangXml_003Ek__BackingField);
		Dictionary<string, string> dic = new Dictionary<string, string>();
		string xpath = string.Format("/Resource/{0}/Controls/Control", string_0);
		XmlNodeList xmlNodeList = _languageXML.SelectNodes(xpath);
		foreach (XmlNode item in xmlNodeList)
		{
			string value = item.Attributes["Id"].Value;
			string value2 = item.Attributes["Text"].Value;
			dic.Add(value, value2);
		}
		Db4kySLQw7tX1WlNpo.ForEach(delegate(Control x)
		{
			if (dic.ContainsKey(x.Name))
			{
				if (x is DataGridView)
				{
					DataGridView dataGridView = x as DataGridView;
					foreach (DataGridViewColumn column in dataGridView.Columns)
					{
						column.HeaderText = dic[x.Name].Split(',')[column.Index];
					}
				}
				else
				{
					x.Text = dic[x.Name];
				}
			}
		});
	}

	public static void smethod_70(List<ToolStripMenuItem> gku9yQXy4fa3WZdpnA, string string_0)
	{
		//XmlDocument xmlDocument = new XmlDocument();
		//xmlDocument.Load(Settings._003CLangXml_003Ek__BackingField);
		Dictionary<string, string> dic = new Dictionary<string, string>();
		string xpath = string.Format("/Resource/{0}/ContextMenuStrip/MenuItem", string_0);
		XmlNodeList xmlNodeList = _languageXML.SelectNodes(xpath);
		foreach (XmlNode item in xmlNodeList)
		{
			string value = item.Attributes["Id"].Value;
			string value2 = item.Attributes["Text"].Value;
			dic.Add(value, value2);
		}
		gku9yQXy4fa3WZdpnA.ForEach(delegate(ToolStripMenuItem x)
		{
			if (dic.ContainsKey(x.Name))
			{
				string text3 = x.Text = (x.ToolTipText = dic[x.Name]);
			}
		});
	}

	public static void smethod_71(List<ToolStripItem> LE9oY1wrram2m8Ao56, string string_0)
	{
		//XmlDocument xmlDocument = new XmlDocument();
		//xmlDocument.Load(Settings._003CLangXml_003Ek__BackingField);
		Dictionary<string, string> dic = new Dictionary<string, string>();
		string xpath = string.Format("/Resource/{0}/Controls/Control/ToolStripItem", string_0);
		XmlNodeList xmlNodeList = _languageXML.SelectNodes(xpath);
		foreach (XmlNode item in xmlNodeList)
		{
			string value = item.Attributes["Id"].Value;
			string value2 = item.Attributes["Text"].Value;
			dic.Add(value, value2);
		}
		LE9oY1wrram2m8Ao56.ForEach(delegate(ToolStripItem x)
		{
			if (dic.ContainsKey(x.Name))
			{
				string text3 = x.Text = (x.ToolTipText = dic[x.Name]);
			}
		});
	}

	public static void smethod_72(Dictionary<string, string> I2YgnU9gqyioPitkyF)
	{
		//XmlDocument xmlDocument = new XmlDocument();
		//xmlDocument.Load(Settings._003CLangXml_003Ek__BackingField);
		string xpath = string.Format("/Resource/Commons/Item");
		XmlNodeList xmlNodeList = _languageXML.SelectNodes(xpath);
		foreach (XmlNode item in xmlNodeList)
		{
			string value = item.Attributes["Id"].Value;
			string value2 = item.Attributes["Text"].Value;
	        if (I2YgnU9gqyioPitkyF.ContainsKey(value))
			{
				I2YgnU9gqyioPitkyF[value] = value2;
			}
		}
	}

	public static void smethod_73(Dictionary<string, string> uxSTPFh3sq4yXxEkYo, string string_0)
	{
		//XmlDocument xmlDocument = new XmlDocument();
		//xmlDocument.Load(Settings._003CLangXml_003Ek__BackingField);
		string xpath = string.Format("/Resource/{0}/Commons/Item", string_0);
		XmlNodeList xmlNodeList = _languageXML.SelectNodes(xpath);
		foreach (XmlNode item in xmlNodeList)
		{
			string value = item.Attributes["Id"].Value;
			string value2 = item.Attributes["Text"].Value;
			if (uxSTPFh3sq4yXxEkYo.ContainsKey(value))
			{
				uxSTPFh3sq4yXxEkYo[value] = value2;
			}
		}
	}

	public static void smethod_74(List<string[]> n2SR3VmEodXx385mq9, List<string> AMMonO7JcQ5lQDAuEr, string string_0)
	{
		//XmlDocument xmlDocument = new XmlDocument();
		//xmlDocument.Load(Settings._003CLangXml_003Ek__BackingField);
		for (int i = 0; i < n2SR3VmEodXx385mq9.Count; i++)
		{
			string xpath = string.Format("/Resource/{0}/Commons/Item[@Id='{1}']", string_0, AMMonO7JcQ5lQDAuEr[i]);
			XmlNode xmlNode = _languageXML.SelectSingleNode(xpath);
			if (xmlNode != null)
			{
				string value = xmlNode.Attributes["Text"].Value;
				string[] array = value.Split(',');
				for (int j = 0; j < n2SR3VmEodXx385mq9[i].Length && j < array.Length; j++)
				{
					n2SR3VmEodXx385mq9[i][j] = array[j];
				}
			}
		}
	}

	public static void smethod_75(List<string> mTBilSHhIiS5P1HoGl, List<string> QaKAVsVaOpyU5FW5pp)
	{
		//XmlDocument xmlDocument = new XmlDocument();
		//xmlDocument.Load(Settings._003CLangXml_003Ek__BackingField);
		for (int i = 0; i < mTBilSHhIiS5P1HoGl.Count; i++)
		{
			string xpath = string.Format("/Resource/Commons/Item[@Id='{0}']", QaKAVsVaOpyU5FW5pp[i]);
			XmlNode xmlNode = _languageXML.SelectSingleNode(xpath);
			if (xmlNode != null)
			{
				string text = mTBilSHhIiS5P1HoGl[i] = xmlNode.Attributes["Text"].Value;
			}
		}
	}

	public static void smethod_76(string string_0, ref string string_1)
	{
		//XmlDocument xmlDocument = new XmlDocument();
		//xmlDocument.Load(Settings._003CLangXml_003Ek__BackingField);
		string xpath = string.Format("/Resource/Commons/Item[@Id='{0}' and @Text]", string_0);
		XmlNode xmlNode = _languageXML.SelectSingleNode(xpath);
		if (xmlNode != null)
		{
			string_1 = xmlNode.Attributes["Text"].Value;
		}
	}

	public static void smethod_77(string string_0, ref string string_1, string string_2)
	{
		//XmlDocument xmlDocument = new XmlDocument();
		//xmlDocument.Load(Settings._003CLangXml_003Ek__BackingField);
		string xpath = string.Format("/Resource/{0}/Commons/Item[@Id='{1}' and @Text]", string_2, string_0);
		XmlNode xmlNode = _languageXML.SelectSingleNode(xpath);
		if (xmlNode != null)
		{
			string_1 = xmlNode.Attributes["Text"].Value;
		}
	}

	public static void smethod_78(string string_0, string[] string_1, string string_2)
	{
		//XmlDocument xmlDocument = new XmlDocument();
		//xmlDocument.Load(Settings._003CLangXml_003Ek__BackingField);
		string xpath = string.Format("/Resource/{0}/Commons/Item[@Id='{1}' and @Text]", string_2, string_0);
		XmlNode xmlNode = _languageXML.SelectSingleNode(xpath);
		if (xmlNode != null)
		{
			string value = xmlNode.Attributes["Text"].Value;
			string[] array = value.Split(',');
			for (int i = 0; i < string_1.Length && i < array.Length; i++)
			{
				string_1[i] = array[i];
			}
		}
	}

	public Settings()
	{
//		
	}

	static Settings()
	{
		
		Settings.curUserMode = UserMode.Basic;
		Settings.CUR_MODEL = new byte[8]
		{
			77,
			68,
			45,
			55,
			54,
			48,
			80,
			255
		};
		Settings.SZ_NONE = "None";
		Settings.SZ_SELECTED = "Selected";
		Settings.SZ_ADD = "Add";
		Settings.SZ_OFF = "Off";
		Settings.SZ_DEVICE_NOT_FOUND = "Device not found";
		Settings.SZ_OPEN_PORT_FAIL = "";
		Settings.SZ_COMM_ERROR = "Communication error";
		Settings.SZ_MODEL_NOT_MATCH = "";
		Settings.SZ_READ = "Read";
		Settings.SZ_WRITE = "Write";
		Settings.SZ_READ_COMPLETE = "Read Complete";
		Settings.SZ_WRITE_COMPLETE = "Write Complete";
        Settings.SZ_CODEPLUG_READ_CONFIRM = "Are you sure you want to read the codeplug from the GD-77?\nThis will overwrite the current codeplug.";
        Settings.SZ_CODEPLUG_WRITE_CONFIRM = "Are you sure you want to write this codeplug to the GD-77?\nThis will overwrite codeplug currently in the GD-77";
        Settings.SZ_PLEASE_CONFIRM = "Please confirm";


		Settings.SZ_KEYPRESS_DTMF = "";
		Settings.SZ_KEYPRESS_HEX = "Please Input: ";
		Settings.SZ_KEYPRESS_DIGIT = "Please input: Digit and {0}";
		Settings.SZ_KEYPRESS_PRINT = "Please input Alphanumeric symbol";
		Settings.SZ_DATA_FORMAT_ERROR = "";
		Settings.SZ_FIRST_CH_NOT_DELETE = "The first channel in the first zone cannot be deleted";
		Settings.SZ_FIRST_NOT_DELETE = "The first cannot be deleted";
		Settings.SZ_NAME_EXIST_NAME = "NameExist";
		Settings.SZ_FILE_FORMAT_ERROR = "File format error!";
		Settings.SZ_OPEN_SUCCESSFULLY = "Open Successfully!";
		Settings.SZ_SAVE_SUCCESSFULLY = "Save Successfully";
		Settings.SZ_TYPE_NOT_MATCH = "Type does not match";
		Settings.SZ_EXPORT_SUCCESS = "Export Success";
		Settings.SZ_IMPORT_SUCCESS = "Import Success";
		Settings.SZ_ID_NOT_EMPTY = "ID can not be empty!";
		Settings.SZ_ID_OUT_OF_RANGE = "ID out of range!";
		Settings.SZ_ID_ALREADY_EXISTS = "ID already exists！";
		Settings.SZ_NOT_SELECT_ITEM_NOT_COPYITEM = "Not select item or Not copyitem";
		Settings.SZ_PROMPT_KEY1 = "Does the software exit and save the file?";
		Settings.SZ_PROMPT_KEY2 = "Whether the new, will be restored to the initial state!";
		Settings.SZ_PROMPT = "Prompt";
		Settings.SZ_ERROR = "Error";
		Settings.SZ_WARNING = "Warning";


		Settings.CUR_MODE = 2;// Roger Clark. Changed from 0 to 2 as this seems to be the Expert settings mode.
		Settings.MIN_FREQ = new uint[2]
		{
			400u,
			130u
		};
		Settings.MAX_FREQ = new uint[2]
		{
			480u,
			174u
		};
		Settings.VALID_MIN_FREQ = new uint[2]
		{
			130u,
			130u
		};
		Settings.VALID_MAX_FREQ = new uint[2]
		{
			520u,
			470u
		};
		Settings.CUR_CH_GROUP = 0;
		Settings.CUR_ZONE_GROUP = 0;
		Settings.CUR_ZONE = 0;
		Settings.CUR_PWD = "";

		Settings.SPACE_DEVICE_INFO = Marshal.SizeOf(typeof(DeviceInfoForm.DeviceInfo));
		Settings.ADDR_DEVICE_INFO = 128;
		Settings.OFS_LAST_PRG_TIME = Marshal.OffsetOf(typeof(DeviceInfoForm.DeviceInfo), "lastPrgTime").ToInt32();
		Settings.OFS_CPS_SW_VER = Marshal.OffsetOf(typeof(DeviceInfoForm.DeviceInfo), "cpsSwVer").ToInt32();
		Settings.OFS_MODEL = Marshal.OffsetOf(typeof(DeviceInfoForm.DeviceInfo), "model").ToInt32();
		Settings.SPACE_GENERAL_SET = Marshal.SizeOf(typeof(GeneralSetForm.GeneralSet));
		Settings.ADDR_GENERAL_SET = 224;
		Settings.ADDR_PWD = Settings.ADDR_GENERAL_SET + Marshal.OffsetOf(typeof(GeneralSetForm.GeneralSet), "prgPwd").ToInt32();
		Settings.SPACE_BUTTON = Marshal.SizeOf(typeof(ButtonForm.SideKey));
		Settings.ADDR_BUTTON = 264;
		Settings.SPACE_ONE_TOUCH = Marshal.SizeOf(typeof(ButtonForm.OneTouch));
		Settings.ADDR_ONE_TOUCH = 272;
		Settings.SPACE_TEXT_MSG = Marshal.SizeOf(typeof(TextMsgForm.TextMsg));
		Settings.ADDR_TEXT_MSG = 296;
		Settings.SPACE_ENCRYPT = Marshal.SizeOf(typeof(EncryptForm.Encrypt));
		Settings.ADDR_ENCRYPT = 4976;
		Settings.SPACE_SIGNALING_BASIC = Marshal.SizeOf(typeof(SignalingBasicForm.SignalingBasic));
		Settings.ADDR_SIGNALING_BASIC = 5112;
		Settings.SPACE_DTMF_BASIC = Marshal.SizeOf(typeof(DtmfForm.Dtmf));
		Settings.ADDR_DTMF_BASIC = 5120;
		Settings.SPACE_EMG_SYSTEM = Marshal.SizeOf(typeof(EmergencyForm.Emergency));
		Settings.ADDR_EMG_SYSTEM = 5512;
		Settings.SPACE_DMR_CONTACT = Marshal.SizeOf(typeof(ContactForm.Contact));
		Settings.ADDR_DMR_CONTACT = 6024;
		Settings.SPACE_DMR_CONTACT_EX = Marshal.SizeOf(typeof(ContactForm.Contact));
		Settings.ADDR_DMR_CONTACT_EX = 95776;
		Settings.SPACE_DTMF_CONTACT = Marshal.SizeOf(typeof(DtmfContactForm.DtmfContact));
		Settings.ADDR_DTMF_CONTACT = 12168;
		Settings.SPACE_RX_GRP_LIST = Marshal.SizeOf(typeof(RxListData));
		//Settings.ADDR_RX_GRP_LIST = 13352;
		Settings.ADDR_RX_GRP_LIST_EX = 0x1D620;// 120352;
		Settings.ADDR_ZONE_BASIC = 14136;
		Settings.ADDR_ZONE_LIST = 0x3740;// 14144;
		Settings.ADDR_CHANNEL = 14208;
		Settings.SPACE_SCAN_BASIC = Marshal.SizeOf(typeof(ScanBasicForm.ScanBasic));
		Settings.ADDR_SCAN = 6024;
		Settings.SPACE_SCAN_LIST = Marshal.SizeOf(typeof(NormalScanForm.NormalScan));
		Settings.ADDR_SCAN_LIST = Settings.ADDR_SCAN + Settings.SPACE_SCAN_BASIC;
		Settings.SPACE_BOOT_ITEM = Marshal.SizeOf(typeof(BootItemForm.BootItem));
		Settings.ADDR_BOOT_ITEM = 29976;
		Settings.SPACE_DIGITAL_KEY_CONTACT = Marshal.SizeOf(typeof(DigitalKeyContactForm.NumKeyContact));
		Settings.ADDR_DIGITAL_KEY_CONTACT = 29984;
		Settings.SPACE_MENU_CONFIG = Marshal.SizeOf(typeof(MenuForm.MenuSet));
		Settings.ADDR_MENU_CONFIG = 30008;
		Settings.SPACE_BOOT_CONTENT = Marshal.SizeOf(typeof(BootItemForm.BootContent));
		Settings.ADDR_BOOT_CONTENT = 30016;
		Settings.SPACE_ATTACHMENT = Marshal.SizeOf(typeof(AttachmentForm.Attachment));
		Settings.ADDR_ATTACHMENT = 30048;
		Settings.SPACE_VFO = Marshal.SizeOf(typeof(VfoForm.Vfo));
		Settings.ADDR_VFO = 30096;
		Settings.SPACE_EX_ZONE = Marshal.SizeOf(typeof(ZoneForm.Zone));
		Settings.ADDR_EX_ZONE = 32768;
		Settings.ADDR_EX_ZONE_BASIC = Settings.ADDR_EX_ZONE;
		Settings.ADDR_EX_ZONE_LIST = Settings.ADDR_EX_ZONE + 16;
		Settings.SPACE_EX_SCAN = Marshal.SizeOf(typeof(NormalScanForm.NormalScanEx));
		Settings.ADDR_EX_SCAN = 44816;
		Settings.ADDR_EX_SCAN_PRI_CH1 = 44816;
		Settings.ADDR_EX_SCAN_PRI_CH2 = 44848;
		Settings.ADDR_EX_SCAN_SPECIFY_CH = 44880;
		Settings.ADDR_EX_SCAN_CH_LIST = 44912;
		Settings.SPACE_EX_EMERGENCY = Marshal.SizeOf(typeof(EmergencyForm.EmergencyEx));
		Settings.ADDR_EX_EMERGENCY = 45424;
		Settings.SPACE_EX_CH = ChannelForm.SPACE_CH_GROUP * 7;
		Settings.ADDR_EX_CH = 45488;
		Settings.dicCommon = new Dictionary<string, string>();
	}
}
