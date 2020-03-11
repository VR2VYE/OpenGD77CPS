using System;
using System.Runtime.InteropServices;

namespace DMR
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct PowerSettingData
	{
		public byte lowPower;
		public byte highPower;
	}
}
