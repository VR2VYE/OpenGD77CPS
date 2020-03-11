using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMR
{
	public class DMRDataItem : IEquatable<DMRDataItem>, IComparable<DMRDataItem>
	{
		public int DMRId { get; set; }
		public string DMRIdString { get; set; }
		public string Callsign { get; set; }
		public string Name { get; set; }
		public string AgeInDays { get; set; }
		public int AgeAsInt { get; set; }

		// default construrctor creates an empty item
		public DMRDataItem()
		{
			Callsign = "";
			DMRId = 0;
			AgeInDays = "n/a";
		}

		// Create from a semicolon separated string from Hamdigital
		public DMRDataItem FromHamDigital(string CSVLine)
		{
			string[] arr = CSVLine.Split(';');
			Callsign = arr[1];
			Name = arr[3];
			DMRId = Int32.Parse(arr[2]);
			try
			{
				AgeAsInt = Int32.Parse(arr[4]);// see if its a number (exception will trigger if not)
				AgeInDays = arr[4];// Only gets here is no exception triggered
			}
			catch (Exception)
			{

			}
			return this;
		}

		// Create from a semicolon separated string from Hamdigital
		public DMRDataItem FromRadioidDotNet(string CSVLine)
		{
			string[] arr = CSVLine.Split(',');
			Callsign = arr[1];
			Name = arr[2];// +" " + arr[3];
			DMRIdString = arr[0];
			DMRId = Int32.Parse(arr[0]);
			AgeAsInt = 0;
			AgeInDays = "0";
			return this;
		}

		// Create from Data stored in the Codeplug
		public DMRDataItem(byte[] data, int stringLength)
		{
			Callsign = System.Text.Encoding.Default.GetString(data, 4, stringLength);
			DMRId = BitConverter.ToInt32(data, 0);
		}

		// Create from a semicolon separated string from Hamdigital
		public DMRDataItem FromRadio(byte[] record, int stringLength)
		{
			byte[] dmrid = new byte[4];
			Callsign = System.Text.Encoding.Default.GetString(record, 4, stringLength);

			Array.Copy(record, dmrid, 4);
			DMRId = 0;
			for (int i = 0; i < 4; i++)
			{
				DMRId *= 100;
				DMRId += BCDToByte(dmrid[3 - i]);
			}
			return this;
		}

		private byte Int8ToBCD(int val)
		{
	        int hi = val / 10;
			int lo = val % 10;
			return (byte)( (hi * 16) + lo);
		}

		private byte BCDToByte(byte val)
		{
			int hi = val >> 4;
			int lo = val & 0x0F;
			return (byte)(hi * 10 + lo);
		}

		// Convert to format to send to the radio (GD-77)
		public byte[] getRadioData(int stringLength)
		{
			byte[] radioData = new byte[stringLength+4];
			if (DMRId != 0)
			{
				byte[] displayBuf;
				if (stringLength > 8)
				{
					displayBuf = Encoding.UTF8.GetBytes(Callsign + " " + Name); 
				}
				else
				{
					displayBuf = Encoding.UTF8.GetBytes(Callsign);
				}



				Array.Copy(displayBuf, 0, radioData, 4, Math.Min(stringLength, displayBuf.Length));

				int dmrid = DMRId;
				for (int i = 0; i < 4; i++)
				{
					radioData[i] = Int8ToBCD(dmrid % 100);
					dmrid /= 100;
				}
			}
			return radioData; 
		}

		public byte[] getCodeplugData(int stringLength)
		{
			byte[] codeplugData = new byte[4+stringLength];
			if (DMRId != 0)
			{
				byte[] callsignbBuf = Encoding.UTF8.GetBytes(Callsign);
				Array.Copy(callsignbBuf, 0, codeplugData, 4, callsignbBuf.Length);
				Array.Copy(BitConverter.GetBytes(DMRId), 0, codeplugData, 0, 4);
			}
			return codeplugData; 
		}

		public int CompareTo(DMRDataItem comparePart)
		{
			// A null value means that this object is greater.
			if (comparePart == null)
			{
				return 1;
			}
			else
			{
				if (comparePart.DMRId < DMRId)
				{
					return 1;
				}
				else
				{
					if (comparePart.DMRId > DMRId)
					{
						return -1;
					}
					else
					{
						return 0;
					}
				}
			}
		}

		public bool Equals(DMRDataItem other)
		{
			if (other == null)
			{ 
				return false;
			}
			if (other == this)
			{
				return true;
			}
			return (this.DMRId == other.DMRId);
		}
		public override int GetHashCode()
		{
			//Get hash code for the Name field if it is not null. 
			return DMRId;
		}

		public object GetValue()
		{
			return AgeAsInt;
		}
	}
}
