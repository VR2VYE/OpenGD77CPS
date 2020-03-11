using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DMR
{
	public class SetupDiWrap
	{
		static public string ComPortNameFromFriendlyNamePrefix(string friendlyNamePrefix)
		{
			const string className = "Ports";
			Guid[] guids = GetClassGUIDs(className);

			System.Text.RegularExpressions.Regex friendlyNameToComPort =
				new System.Text.RegularExpressions.Regex(@".? \((COM\d+)\)$");  // "..... (COMxxx)" -> COMxxxx

			foreach (Guid guid in guids)
			{
				// We start at the "root" of the device tree and look for all
				// devices that match the interface GUID of a disk
				Guid guidClone = guid;
				IntPtr h = SetupDiGetClassDevs(ref guidClone, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_PROFILE);
				if (h.ToInt32() != INVALID_HANDLE_VALUE)
				{
					int nDevice = 0;
					while (true)
					{
						SP_DEVINFO_DATA da = new SP_DEVINFO_DATA();
						da.cbSize = (uint)Marshal.SizeOf(da);

						if (0 == SetupDiEnumDeviceInfo(h, nDevice++, ref da))
							break;

						uint RegType;
						byte[] ptrBuf = new byte[BUFFER_SIZE];
						uint RequiredSize;
						if (SetupDiGetDeviceRegistryProperty(h, ref da,
							(uint)SPDRP.FRIENDLYNAME, out RegType, ptrBuf,
							BUFFER_SIZE, out RequiredSize))
						{
							const int utf16terminatorSize_bytes = 2;
							string friendlyName = System.Text.UnicodeEncoding.Unicode.GetString(ptrBuf, 0, (int)RequiredSize - utf16terminatorSize_bytes);

							if (!friendlyName.StartsWith(friendlyNamePrefix))
								continue;

							if (!friendlyNameToComPort.IsMatch(friendlyName))
								continue;

							return friendlyNameToComPort.Match(friendlyName).Groups[1].Value;
						}
					} // devices
					SetupDiDestroyDeviceInfoList(h);
				}
			} // class guids

			return null;
		}

		public static List<string> GetComPortNames()
		{
			const string className = "Ports";
			Guid[] guids = GetClassGUIDs(className);
			List<string> portNames = new List<string>();

			System.Text.RegularExpressions.Regex friendlyNameToComPort =
				new System.Text.RegularExpressions.Regex(@".? \((COM\d+)\)$");  // "..... (COMxxx)" -> COMxxxx

			foreach (Guid guid in guids)
			{
				// We start at the "root" of the device tree and look for all
				// devices that match the interface GUID of a disk
				Guid guidClone = guid;
				IntPtr h = SetupDiGetClassDevs(ref guidClone, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_PROFILE);
				if (h.ToInt32() != INVALID_HANDLE_VALUE)
				{
					int nDevice = 0;
					while (true)
					{
						SP_DEVINFO_DATA da = new SP_DEVINFO_DATA();
						da.cbSize = (uint)Marshal.SizeOf(da);

						if (0 == SetupDiEnumDeviceInfo(h, nDevice++, ref da))
							break;

						uint RegType;
						byte[] ptrBuf = new byte[BUFFER_SIZE];
						uint RequiredSize;
						if (SetupDiGetDeviceRegistryProperty(h, ref da,
							(uint)SPDRP.FRIENDLYNAME, out RegType, ptrBuf,
							BUFFER_SIZE, out RequiredSize))
						{
							const int utf16terminatorSize_bytes = 2;
							string friendlyName = System.Text.UnicodeEncoding.Unicode.GetString(ptrBuf, 0, (int)RequiredSize - utf16terminatorSize_bytes);

							portNames.Add(friendlyName);
						}
					} // devices
					SetupDiDestroyDeviceInfoList(h);
				}
			} // class guids

			return portNames;
		}

		/// <summary>
		/// The SP_DEVINFO_DATA structure defines a device instance that is a member of a device information set.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		private struct SP_DEVINFO_DATA
		{
			/// <summary>Size of the structure, in bytes.</summary>
			public uint cbSize;
			/// <summary>GUID of the device interface class.</summary>
			public Guid ClassGuid;
			/// <summary>Handle to this device instance.</summary>
			public uint DevInst;
			/// <summary>Reserved; do not use.</summary>
			public uint Reserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct SP_DEVICE_INTERFACE_DATA
		{
			public Int32 cbSize;
			public Guid interfaceClassGuid;
			public Int32 flags;
			private UIntPtr reserved;
		}

		const int BUFFER_SIZE = 1024;

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct SP_DEVICE_INTERFACE_DETAIL_DATA
		{
			public int cbSize;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
			public string DevicePath;
		}

		private enum SPDRP
		{
			DEVICEDESC = 0x00000000,
			HARDWAREID = 0x00000001,
			COMPATIBLEIDS = 0x00000002,
			NTDEVICEPATHS = 0x00000003,
			SERVICE = 0x00000004,
			CONFIGURATION = 0x00000005,
			CONFIGURATIONVECTOR = 0x00000006,
			CLASS = 0x00000007,
			CLASSGUID = 0x00000008,
			DRIVER = 0x00000009,
			CONFIGFLAGS = 0x0000000A,
			MFG = 0x0000000B,
			FRIENDLYNAME = 0x0000000C,
			LOCATION_INFORMATION = 0x0000000D,
			PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000E,
			CAPABILITIES = 0x0000000F,
			UI_NUMBER = 0x00000010,
			UPPERFILTERS = 0x00000011,
			LOWERFILTERS = 0x00000012,
			MAXIMUM_PROPERTY = 0x00000013,
		}

		[DllImport("setupapi.dll", SetLastError = true)]
		static extern bool SetupDiClassGuidsFromName(string ClassName,
			ref Guid ClassGuidArray1stItem, UInt32 ClassGuidArraySize,
			out UInt32 RequiredSize);

		[DllImport("setupapi.dll")]
		internal static extern IntPtr SetupDiGetClassDevsEx(IntPtr ClassGuid,
			[MarshalAs(UnmanagedType.LPStr)]String enumerator,
			IntPtr hwndParent, Int32 Flags, IntPtr DeviceInfoSet,
			[MarshalAs(UnmanagedType.LPStr)]String MachineName, IntPtr Reserved);

		[DllImport("setupapi.dll")]
		internal static extern Int32 SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		[DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern Boolean SetupDiEnumDeviceInterfaces(
		   IntPtr hDevInfo,
		   IntPtr optionalCrap, //ref SP_DEVINFO_DATA devInfo,
		   ref Guid interfaceClassGuid,
		   UInt32 memberIndex,
		   ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
		);

		[DllImport("setupapi.dll")]
		private static extern Int32 SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet,
			Int32 MemberIndex, ref SP_DEVINFO_DATA DeviceInterfaceData);

		[DllImport("setupapi.dll")]
		private static extern Int32 SetupDiClassNameFromGuid(ref Guid ClassGuid,
			StringBuilder className, Int32 ClassNameSize, ref Int32 RequiredSize);

		[DllImport("setupapi.dll")]
		private static extern Int32 SetupDiGetClassDescription(ref Guid ClassGuid,
			StringBuilder classDescription, Int32 ClassDescriptionSize, ref Int32 RequiredSize);

		[DllImport("setupapi.dll")]
		private static extern Int32 SetupDiGetDeviceInstanceId(IntPtr DeviceInfoSet,
			ref SP_DEVINFO_DATA DeviceInfoData,
			StringBuilder DeviceInstanceId, Int32 DeviceInstanceIdSize, ref Int32 RequiredSize);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto)]
		static extern IntPtr SetupDiGetClassDevs(           // 1st form using a ClassGUID only, with null Enumerator
		   ref Guid ClassGuid,
		   IntPtr Enumerator,
		   IntPtr hwndParent,
		   int Flags
		);

		[DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern Boolean SetupDiGetDeviceInterfaceDetail(
		   IntPtr hDevInfo,
		   ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
		   ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
		   UInt32 deviceInterfaceDetailDataSize,
		   out UInt32 requiredSize,
		   ref SP_DEVINFO_DATA deviceInfoData
		);

		/// <summary>
		/// The SetupDiGetDeviceRegistryProperty function retrieves the specified device property.
		/// This handle is typically returned by the SetupDiGetClassDevs or SetupDiGetClassDevsEx function.
		/// </summary>
		/// <param Name="DeviceInfoSet">Handle to the device information set that contains the interface and its underlying device.</param>
		/// <param Name="DeviceInfoData">Pointer to an SP_DEVINFO_DATA structure that defines the device instance.</param>
		/// <param Name="Property">Device property to be retrieved. SEE MSDN</param>
		/// <param Name="PropertyRegDataType">Pointer to a variable that receives the registry data Type. This parameter can be NULL.</param>
		/// <param Name="PropertyBuffer">Pointer to a buffer that receives the requested device property.</param>
		/// <param Name="PropertyBufferSize">Size of the buffer, in bytes.</param>
		/// <param Name="RequiredSize">Pointer to a variable that receives the required buffer size, in bytes. This parameter can be NULL.</param>
		/// <returns>If the function succeeds, the return value is nonzero.</returns>
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool SetupDiGetDeviceRegistryProperty(
			IntPtr DeviceInfoSet,
			ref SP_DEVINFO_DATA DeviceInfoData,
			uint Property,
			out UInt32 PropertyRegDataType,
			byte[] PropertyBuffer,
			uint PropertyBufferSize,
			out UInt32 RequiredSize);


		const int DIGCF_DEFAULT = 0x1;
		const int DIGCF_PRESENT = 0x2;
		const int DIGCF_ALLCLASSES = 0x4;
		const int DIGCF_PROFILE = 0x8;
		const int DIGCF_DEVICEINTERFACE = 0x10;
		const int INVALID_HANDLE_VALUE = -1;

		private static Guid[] GetClassGUIDs(string className)
		{
			UInt32 requiredSize = 0;
			Guid[] guidArray = new Guid[1];

			bool status = SetupDiClassGuidsFromName(className, ref guidArray[0], 1, out requiredSize);
			if (true == status)
			{
				if (1 < requiredSize)
				{
					guidArray = new Guid[requiredSize];
					SetupDiClassGuidsFromName(className, ref guidArray[0], requiredSize, out requiredSize);
				}
			}
			else
				throw new System.ComponentModel.Win32Exception();

			return guidArray;
		}


	}
}
