using System;

namespace UsbLibrary
{
	public class SpecifiedDevice : HIDDevice
	{
		public event DataRecievedEventHandler DataRecieved;

		public event DataSendEventHandler DataSend;

		public override InputReport CreateInputReport()
		{
			return new SpecifiedInputReport(this);
		}

		public static SpecifiedDevice FindSpecifiedDevice(int vendor_id, int product_id)
		{
			return (SpecifiedDevice)HIDDevice.FindDevice(vendor_id, product_id, typeof(SpecifiedDevice));
		}

		protected override void HandleDataReceived(InputReport oInRep)
		{
			if (this.DataRecieved != null)
			{
				SpecifiedInputReport specifiedInputReport = (SpecifiedInputReport)oInRep;
				this.DataRecieved(this, new DataRecievedEventArgs(specifiedInputReport.Data));
			}
		}

        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

		public bool SendData(byte[] data)
		{
#if false
			switch (data[0])
			{
				case 0x52:
					Console.WriteLine(data[1] * 256 + data[2]);
					break;
				case 0x57:
					Console.WriteLine(data[1] * 256 + data[2]);
					break;
			}

            Console.WriteLine("SendData " + SpecifiedDevice.ByteArrayToString(data));
#endif
			SpecifiedOutputReport specifiedOutputReport = new SpecifiedOutputReport(this);
			specifiedOutputReport.SendData(data);
			try
			{
				base.Write(specifiedOutputReport);
				if (this.DataSend != null)
				{
					this.DataSend(this, new DataSendEventArgs(data));
				}
			}
			catch (GException0 gException)
			{
				Console.WriteLine(gException.Message);
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			return true;
		}

		int offset = 0;

		public bool SendData(byte[] data, int index, int length)
		{

#if false
			//Console.WriteLine("SendData " + SpecifiedDevice.ByteArrayToString(data));

			if (data[0] == 0x43 && data[1] == 0x57 && data[2] == 0x42)
			{
				offset = data[5] * 65536 + data[6] * 256 + data[7];
				//Console.WriteLine("Offset " + offset);
			}

			else
			{
				switch (data[0])
				{
					case 0x52:
						//Console.WriteLine((data[1] * 256 + data[2] + offset) + "\t" + data[3]);
						break;
					case 0x57:
						//Console.WriteLine(string.Format("0x{0:X8}\t", (data[1] * 256 + data[2] + offset), data[3]));
						Console.WriteLine((data[1] * 256 + data[2] + offset) );//+ "\t" + data[3]);
						break;
				}
			}
#endif
			SpecifiedOutputReport specifiedOutputReport = new SpecifiedOutputReport(this);
			specifiedOutputReport.SendData(data, index, length);
			try
			{
				base.Write(specifiedOutputReport);
				if (this.DataSend != null)
				{
					this.DataSend(this, new DataSendEventArgs(data));
				}
			}
			catch (GException0 gException)
			{
				Console.WriteLine(gException.Message);
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			return true;
		}

		public bool ReceiveData(byte[] data)
		{
            bool retVal = base.BeginAsyncRead(data);
#if false
			switch (data[0])
			{
				case 0x52:
					Console.WriteLine(data[1] * 256 + data[2]);
					break;
				case 0x57:
					//Console.WriteLine(data[1] * 256 + data[2]);
					break;
			}

           // Console.WriteLine("ReceiveData " + SpecifiedDevice.ByteArrayToString(data));
#endif
            return retVal;
		}

		protected override void Dispose(bool bDisposing)
		{
			base.Dispose(bDisposing);
		}

		public SpecifiedDevice() : base()
		{
			
			//base._002Ector();
		}
	}
}
