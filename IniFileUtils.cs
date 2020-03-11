using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

internal class IniFileUtils
{
	private static string iniPath;
#if OpenGD77
	static string keyName = "HKEY_CURRENT_USER\\Software\\RadioddityCommunity\\OpenGD77CPS";
#elif CP_VER_3_1_X
	static string keyName = "HKEY_CURRENT_USER\\Software\\RadioddityCommunity\\GD77CPS306";
#endif

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	private static extern long WritePrivateProfileString(string string_0, string string_1, string string_2, string string_3);

	[DllImport("kernel32.DLL ", CharSet = CharSet.Auto)]
	private static extern int GetPrivateProfileInt(string string_0, string string_1, int int_0, string string_2);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	private static extern int GetPrivateProfileString(string string_0, string string_1, string string_2, StringBuilder stringBuilder_0, int int_0, string string_3);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	public static extern int GetPrivateProfileSectionNames(IntPtr intptr_0, int int_0, string string_0);

	[DllImport("kernel32.DLL ", CharSet = CharSet.Auto)]
	private static extern int GetPrivateProfileSection(string string_0, byte[] byte_0, int int_0, string string_1);

	public static string getIniFilePath()
	{
		return IniFileUtils.iniPath;
	}

	public static void setIniFilePath(string string_0)
	{
		IniFileUtils.iniPath = string_0;
	}

	private IniFileUtils(string string_0)
	{
		IniFileUtils.setIniFilePath(string_0);
	}
	/*
	public static int smethod_2(string string_0, string string_1, int int_0)
	{
		return IniFileUtils.GetPrivateProfileInt(string_0, string_1, int_0, IniFileUtils.iniPath);
	}

	public static void smethod_3(string string_0, string string_1, int int_0)
	{
		IniFileUtils.WritePrivateProfileString(string_0, string_1, int_0.ToString(), IniFileUtils.iniPath);
	}
	*/
	public static string getProfileStringWithDefault(string string_0, string string_1, string string_2)
	{
		if (IniFileUtils.iniPath!=null)
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			IniFileUtils.GetPrivateProfileString(string_0, string_1, string_2, stringBuilder, 1024, IniFileUtils.iniPath);
			return stringBuilder.ToString();
		}
		else
		{
			var obj = Registry.GetValue(keyName, string_1, string_2);
			if (obj != null)
			{
				return (string)obj;
			}
			else
			{
				return string_2;
			}
		}
	}
	/*
	public static string smethod_5(string string_0, string string_1, string string_2, int int_0)
	{
		StringBuilder stringBuilder = new StringBuilder();
		IniFileUtils.GetPrivateProfileString(string_0, string_1, string_2, stringBuilder, int_0, IniFileUtils.iniPath);
		return stringBuilder.ToString();
	}*/

	public static void WriteProfileString(string string_0, string string_1, string string_2)
	{
		if (IniFileUtils.iniPath!=null)
		{
			IniFileUtils.WritePrivateProfileString(string_0, string_1, string_2, IniFileUtils.iniPath);
		}
		else
		{
			Registry.SetValue(keyName, string_1, string_2, RegistryValueKind.String);
		}
	}

	/* Roger Clark. Commented out code thats not used and not documented.
	 * I've done this in perparation to move to store values in the registry if possible rather than an ini file
	public static void smethod_7(string string_0, string string_1)
	{
		IniFileUtils.WritePrivateProfileString(string_0, string_1, null, IniFileUtils.iniPath);
	}

	public static void smethod_8(string string_0)
	{
		IniFileUtils.WritePrivateProfileString(string_0, null, null, IniFileUtils.iniPath);
	}

	public static int smethod_9(out string[] string_0)
	{
		IntPtr intPtr = Marshal.AllocCoTaskMem(32767);
		int privateProfileSectionNames = IniFileUtils.GetPrivateProfileSectionNames(intPtr, 32767, IniFileUtils.iniPath);
		if (privateProfileSectionNames == 0)
		{
			string_0 = null;
			return -1;
		}
		string text = Marshal.PtrToStringAnsi(intPtr, privateProfileSectionNames).ToString();
		Marshal.FreeCoTaskMem(intPtr);
		string text2 = text.Substring(0, text.Length - 1);
		char[] separator = new char[1];
		string_0 = text2.Split(separator);
		return 0;
	}

	public static int setIniFilePath0(string string_0, out string[] string_1, out string[] string_2)
	{
		byte[] array = new byte[65535];
		IniFileUtils.GetPrivateProfileSection(string_0, array, array.Length, IniFileUtils.iniPath);
		string @string = Encoding.Default.GetString(array);
		string text = @string;
		char[] separator = new char[1];
		string[] array2 = text.Split(separator);
		ArrayList arrayList = new ArrayList();
		string[] array3 = array2;
		foreach (string text2 in array3)
		{
			if (text2 != string.Empty)
			{
				arrayList.Add(text2);
			}
		}
		string_1 = new string[arrayList.Count];
		string_2 = new string[arrayList.Count];
		for (int j = 0; j < arrayList.Count; j++)
		{
			string[] array4 = arrayList[j].ToString().Split('=');
			if (array4.Length == 2)
			{
				string_1[j] = array4[0].Trim();
				string_2[j] = array4[1].Trim();
			}
			else if (array4.Length == 1)
			{
				string_1[j] = array4[0].Trim();
				string_2[j] = "";
			}
			else if (array4.Length == 0)
			{
				string_1[j] = "";
				string_2[j] = "";
			}
		}
		return 0;
	}
	*/
	static IniFileUtils()
	{
		iniPath = null;
		// for portable operation check if setup.ini is in the applications own folder
		if (File.Exists(Application.StartupPath + "\\Setup.ini"))
		{
			IniFileUtils.iniPath = Application.StartupPath + "\\Setup.ini";
			return;
		}
		/*
		else
		{
			IniFileUtils.iniPath = Application.LocalUserAppDataPath + "\\Setup.ini";// was Application.StartupPath	
		}
		*/
	}
}
