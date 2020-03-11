using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DMR
{
	public partial class CalibrationBandControl : UserControl
	{

		private CalibrationData _calibrationData;
		private string _type="";

		public CalibrationBandControl()
		{
			InitializeComponent();
		}

		public string Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
				string[] freqBandNamesVHF8 = { "136MHz", "140MHz", "145MHz", "150MHz", "155MHz", "160MHz", "165MHz", "172MHz" };
				string[] freqBandNamesUHF16 = { "400MHz", "405MHz", "410MHz", "415MHz", "420MHz", "425MHz", "430MHz", "435MHz", "440MHz", "445MHz", "450MHz", "455MHz", "460MHz", "465MHz", "470MHz", "475MHz" };
				string[] freqBandNamesUHF8 = { "405MHz", "415MHz", "425MHz", "435MHz", "445MHz", "455MHz", "465MHz", "475MHz" };
				
				switch(_type)
				{
					case "VHF":
						{
							this.calibrationPowerControlHigh.Cols = 8;
							this.calibrationPowerControlLow.Cols = 8;
							this.calibrationPowerControlLow.Names = freqBandNamesVHF8;
							this.calibrationPowerControlHigh.Names = freqBandNamesVHF8;
							this.calibrationTXIandQ.Names = freqBandNamesVHF8;
						}
						break;
					case "UHF":
						{
							this.calibrationPowerControlHigh.Cols = 16;
							this.calibrationPowerControlLow.Cols = 16;
							this.calibrationPowerControlLow.Names = freqBandNamesUHF16;
							this.calibrationPowerControlHigh.Names = freqBandNamesUHF16;
							this.calibrationTXIandQ.Names = freqBandNamesUHF8;
						}
						break;
				}
			}
		}

		public CalibrationData data
		{
			get 
			{
				_calibrationData.DACOscRefTune				= (ushort)this.nudVhfOscRef.Value;
				_calibrationData.MuteNormalWidebandOpen1	= (byte)this.nudSquelchWideNormOpen.Value;
				_calibrationData.MuteNormalWidebandClose1	= (byte)this.nudSquelchWideNormClose.Value;
				_calibrationData.MuteStrictWidebandOpen1	= (byte)this.nudSquelchWideTightOpen.Value;
				_calibrationData.MuteStrictWidebandClose1	= (byte)this.nudSquelchWideTightClose.Value;
				_calibrationData.MuteNormalNarrowbandOpen1	= (byte)this.nudSquelchNarrowNormOpen.Value;
				_calibrationData.MuteNormalNarrowbandClose1 = (byte)this.nudSquelchNarrowNormClose.Value;
				_calibrationData.MuteStrictNarrowbandOpen1	= (byte)this.nudSquelchNarrowTightOpen.Value;
				_calibrationData.MuteStrictNarrowbandClose1 = (byte)this.nudSquelchNarrowTightClose.Value;
				_calibrationData.ReceiveAGCGainTarget		= (byte)this.nudReceiveAGCTarget.Value;
				_calibrationData.AnalogMicGain				= (byte)this.nudAnalogMicGain.Value;
				_calibrationData.RSSILowerThreshold			= (byte)this.nudSMeterLow.Value;
				_calibrationData.RSSIUpperThreshold			= (byte)this.nudSMeterHigh.Value;

				_calibrationData.MuteStrictWidebandOpen2 = _calibrationData.MuteStrictWidebandOpen1;
				_calibrationData.MuteStrictWidebandClose2 = _calibrationData.MuteStrictWidebandClose1;
				_calibrationData.MuteStrictNarrowbandOpen2 = _calibrationData.MuteStrictNarrowbandOpen1;
				_calibrationData.MuteStrictNarrowbandClose2 = _calibrationData.MuteStrictNarrowbandClose1;

				_calibrationData.AnalogTxDeviationDTMF					= (byte)this.nudAnalogTxDeviationDTMF.Value;
				_calibrationData.AnalogTxDeviation1750Toneburst			= (byte)this.nudAnalogTxDeviation1750Tone.Value;
				_calibrationData.AnalogTxDeviationCTCSSWideband			= (byte)this.nudAnalogTxDeviationCTCSSWideband.Value;
				_calibrationData.AnalogTxDeviationCTCSSNarrowband		= (byte)this.nudAnalogTxDeviationCTCSSNarrowband.Value;
				_calibrationData.AnalogTxDeviationDCSWideband			= (byte)this.nudlAnalogTxDeviationDCSWideband.Value;
				_calibrationData.AnalogTxDeviationDCSNarrowband			= (byte)this.nudAnalogTxDeviationDCSNarrowband.Value;
				//_calibrationData.DigitalTxGainWideband_NOTCONFIRMED		= (UInt16)this.nudDigitalTxGainWideband.Value;
				//_calibrationData.DigitalTxGainNarrowband_NOTCONFIRMED	= (UInt16)this.nudDigitalTxGainNarrowband.Value;
				_calibrationData.DigitalRxGainWideband_NOTCONFIRMED		= (UInt16)this.nudDigitalRxGainWideband.Value;
				_calibrationData.DigitalRxGainNarrowband_NOTCONFIRMED	= (UInt16)this.nudDigitalRxGainNarrowband.Value;

				_calibrationData.AnalogTxOverallDeviationWideband = (UInt16)this.nudAnalogTxGainWideband.Value;
				_calibrationData.AnalogTxOverallDeviationNarrband = (UInt16)this.nudAnalogTxGainNarrowband.Value;
				_calibrationData.AnalogRxAudioGainWideband = (byte)this.nudAnalogRxGainWideband.Value;
				_calibrationData.AnalogRxAudioGainNarrowband = (byte)this.nudAnalogRxGainNarrowband.Value;


				// Power
				int numItems = calibrationPowerControlLow.Rows * calibrationPowerControlLow.Cols;
				for (int i = 0; i < numItems; i++)
				{
					_calibrationData.PowerSettings[i].lowPower  = (byte)calibrationPowerControlLow.Values[i];
					_calibrationData.PowerSettings[i].highPower = (byte)calibrationPowerControlHigh.Values[i];
				}


				// TX I & Q
				numItems = calibrationTXIandQ.Rows * calibrationTXIandQ.Cols;
				for (int i = 0; i < numItems; i++)
				{
					_calibrationData.TXIandQ[i] = (byte)calibrationTXIandQ.Values[i];
				}

				return _calibrationData; 
			}
			set 
			{ 
				_calibrationData = value;

				this.nudVhfOscRef.Value					= _calibrationData.DACOscRefTune;
				this.nudSquelchWideNormOpen.Value		= _calibrationData.MuteNormalWidebandOpen1;
				this.nudSquelchWideNormClose.Value		= _calibrationData.MuteNormalWidebandClose1;
				this.nudSquelchWideTightOpen.Value		= _calibrationData.MuteStrictWidebandOpen1;
				this.nudSquelchWideTightClose.Value		= _calibrationData.MuteStrictWidebandClose1;
				this.nudSquelchNarrowNormOpen.Value		= _calibrationData.MuteNormalNarrowbandOpen1;
				this.nudSquelchNarrowNormClose.Value	= _calibrationData.MuteNormalNarrowbandClose1;
				this.nudSquelchNarrowTightOpen.Value	= _calibrationData.MuteStrictNarrowbandOpen1;
				this.nudSquelchNarrowTightClose.Value	= _calibrationData.MuteStrictNarrowbandClose1;
				this.nudReceiveAGCTarget.Value			= _calibrationData.ReceiveAGCGainTarget;
				this.nudAnalogMicGain.Value				= _calibrationData.AnalogMicGain;
				this.nudSMeterLow.Value					= _calibrationData.RSSILowerThreshold;
				this.nudSMeterHigh.Value				= _calibrationData.RSSIUpperThreshold;

				this.nudAnalogTxDeviationDTMF.Value				= _calibrationData.AnalogTxDeviationDTMF;
				this.nudAnalogTxDeviation1750Tone.Value			= _calibrationData.AnalogTxDeviation1750Toneburst;
				this.nudAnalogTxDeviationCTCSSWideband.Value	= _calibrationData.AnalogTxDeviationCTCSSWideband;
				this.nudAnalogTxDeviationCTCSSNarrowband.Value	= _calibrationData.AnalogTxDeviationCTCSSNarrowband;
				this.nudlAnalogTxDeviationDCSWideband.Value		= _calibrationData.AnalogTxDeviationDCSWideband;
				this.nudAnalogTxDeviationDCSNarrowband.Value	= _calibrationData.AnalogTxDeviationDCSNarrowband;
				//this.nudDigitalTxGainWideband.Value				= _calibrationData.DigitalTxGainWideband_NOTCONFIRMED;
				//this.nudDigitalTxGainNarrowband.Value			= _calibrationData.DigitalTxGainNarrowband_NOTCONFIRMED;
				this.nudDigitalRxGainWideband.Value				= _calibrationData.DigitalRxGainWideband_NOTCONFIRMED;
				this.nudDigitalRxGainNarrowband.Value			= _calibrationData.DigitalRxGainNarrowband_NOTCONFIRMED;

				this.nudAnalogTxGainWideband.Value				= _calibrationData.AnalogTxOverallDeviationWideband;
				this.nudAnalogTxGainNarrowband.Value			= _calibrationData.AnalogTxOverallDeviationNarrband;
				this.nudAnalogRxGainWideband.Value				= _calibrationData.AnalogRxAudioGainWideband;
				this.nudAnalogRxGainNarrowband.Value			= _calibrationData.AnalogRxAudioGainNarrowband;

				// Power
				int numItems = calibrationPowerControlLow.Rows * calibrationPowerControlLow.Cols;
				int[] lowPower = new int[numItems];
				int[] highPower = new int[numItems];
				for (int i = 0; i < numItems; i++)
				{
					lowPower[i] = _calibrationData.PowerSettings[i].lowPower;
					highPower[i] = _calibrationData.PowerSettings[i].highPower;
				}
				calibrationPowerControlLow.Values = lowPower;
				calibrationPowerControlHigh.Values = highPower;

				// TX I & Q
				numItems = this.calibrationTXIandQ.Rows * calibrationTXIandQ.Cols;
				int[] txIAndQ = new int[numItems];
				for (int i = 0; i < numItems; i++)
				{
					txIAndQ[i] = _calibrationData.TXIandQ[i];
				}
				calibrationTXIandQ.Values = txIAndQ;
			}
		}

	}
}
