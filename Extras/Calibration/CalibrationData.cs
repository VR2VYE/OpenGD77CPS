using System;
using System.Runtime.InteropServices;

namespace DMR
{

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class CalibrationData
	{
		public UInt16 DigitalRxGainNarrowband_NOTCONFIRMED;// NOT CONFIRMED
		public UInt16 DigitalTxGainNarrowband_NOTCONFIRMED;// NOT CONFIRMED
		public UInt16 DigitalRxGainWideband_NOTCONFIRMED;// NOT CONFIRMED
		public UInt16 DigitalTxGainWideband_NOTCONFIRMED;// NOT CONFIRMED
		
		/* Superseded by the variables above
		// Changing any of them reduced the Tx power to virtually nothing
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] UnknownBlock1;// Uknown. Changing any of these values causes Tx power to drop to virtuially zero
		*/

		public UInt16 DACOscRefTune;// 	DAC word for frequency reference oscillator

		public byte UnknownBlock2; // Unkown byte E9 on UHF EE on VHF

		/* Power settings
		 * UHF 400 to 475 in 5Mhz stps (16 steps)
		 * VHF 136Mhz, then 140MHz -  165Mhz in steps of 5Mhz, then 172Mhz  (8 steps - upper 8 array entries contain 0xff )
		 */
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] 
		public PowerSettingData[] PowerSettings;


		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] UnknownBlock3;// Unkown

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] UknownBlock9;// Note 

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] UnknownBlock4;// Seems to contain 0x00 on both VHF and UHF. Potentially unused

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] UnknownBlock5;// Different values on VHF and UHF byt all in the range 0x12 - 0x1D


		//  Analog Squelch controls
		public byte MuteStrictWidebandClose1;
		public byte MuteStrictWidebandOpen1;
		public byte MuteStrictWidebandClose2;
		public byte MuteStrictWidebandOpen2;

		public byte MuteNormalWidebandClose1;
		public byte MuteNormalWidebandOpen1;

		public byte MuteStrictNarrowbandClose1;
		public byte MuteStrictNarrowbandOpen1;
		public byte MuteStrictNarrowbandClose2;
		public byte MuteStrictNarrowbandOpen2;

		public byte MuteNormalNarrowbandClose1;
		public byte MuteNormalNarrowbandOpen1;

		public byte RSSILowerThreshold;
		public byte RSSIUpperThreshold;

		/*
		 * VHF 136Mhz , 140Mhz - 165Mhz (in 5Mhz steps), 172Mhz 
		 * UHF 405Mhz - 475Mhz (in 10Mhz steps)
		 */
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] TXIandQ;// Don't adjust

		public byte DigitalRxAudioGainAndBeepVolume;// The Rx audio gain and the beep volume seem linked together.  0x1D on VHF and UHF

		public byte AnalogTxDeviationDTMF;
		public byte AnalogTxDeviation1750Toneburst;
		public byte AnalogTxDeviationCTCSSWideband;
		public byte AnalogTxDeviationCTCSSNarrowband;
		public byte AnalogTxDeviationDCSWideband;
		public byte AnalogTxDeviationDCSNarrowband;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] UnknownBlock8;

		public byte AnalogMicGain;// Both wide and narrow band
		public byte ReceiveAGCGainTarget; // Receiver AGC target. Higher values give more gain. Reducing this may improve receiver overload with strong signals, but would reduce sensitivity

		public UInt16 AnalogTxOverallDeviationWideband;// CTCSS, DCS, DTMF & voice, deviation .Normally a very low value like 0x0027
		public UInt16 AnalogTxOverallDeviationNarrband;// CTCSS, DCS, DTMF & voice, deviation .Normally a very low value like 0x0027
		
		// Not sure why there are 2 of these and what the difference is.
		public byte AnalogRxAudioGainWideband;// normally a 0x0F
		public byte AnalogRxAudioGainNarrowband;// normally a 0x0F

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] UnknownBlock7;

		public CalibrationData()
		{
			/* Superseded
			 * this.UnknownBlock1 = new byte[8];
			 */
			this.PowerSettings = new PowerSettingData[16];
			for (int i = 0; i < 16; i++)
			{
				PowerSettings[i] = new PowerSettingData();
			}
			this.UnknownBlock3 = new byte[8];
			this.UknownBlock9 = new byte[8];
			this.UnknownBlock4 = new byte[4];
			this.UnknownBlock5 = new byte[8];
			this.TXIandQ = new byte[8];
			this.UnknownBlock7 = new byte[2];
			this.UnknownBlock8 = new byte[2];
			
		}

	}
}
