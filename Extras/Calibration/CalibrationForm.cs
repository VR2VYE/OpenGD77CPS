using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.IO;
using System.IO.Ports;

namespace DMR
{
	public partial class CalibrationForm : Form
	{
		public static int MEMORY_LOCATION = 0xF000;//0x8F000;//0x7c00;
		public static int CALIBRATION_MEMORY_LOCATION_OFFICIAL_USB_PROTOCOL = 0x7c00;
		private const int VHF_OFFSET = 0x70;
		public static int CALIBRATION_DATA_SIZE = 224;
		public static byte[] CALIBRATION_HEADER = { 0xA0, 0x0F, 0xC0, 0x12, 0xA0, 0x0F, 0xC0, 0x12 };

		private const int MAX_TRANSFER_SIZE = 32;
		private SerialPort _port = null;

		public CalibrationForm()
		{
			InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!

		}

		bool sendCommand(int commandNumber, int x_or_command_option_number = 0, int y = 0, int iSize = 0, int alignment = 0, int isInverted = 0, string message = "")
		{
			byte[] buffer = new byte[64];
			buffer[0] = (byte)'C';
			buffer[1] = (byte)commandNumber;

			switch (commandNumber)
			{
				case 2:
					buffer[3] = (byte)y;
					buffer[4] = (byte)iSize;
					buffer[5] = (byte)alignment;
					buffer[6] = (byte)isInverted;
					Buffer.BlockCopy(Encoding.ASCII.GetBytes(message), 0, buffer, 7, Math.Min(message.Length, 16));// copy the string into bytes 7 onwards
					break;
				case 6:
					// Special command
					buffer[2] = (byte)x_or_command_option_number;
					break;
				default:

					break;

			}
			_port.Write(buffer, 0, 32);
			while (_port.BytesToRead == 0)
			{
				Thread.Sleep(0);
			}
			_port.Read(buffer, 0, 64);

			return ((buffer[1] == commandNumber));
		}

		void updateProgess(int progressPercentage)
		{
			/*
			if (progressBar1.InvokeRequired)
				progressBar1.Invoke(new MethodInvoker(delegate()
				{
					progressBar1.Value = progressPercentage;
				}));
			else
			{
				progressBar1.Value = progressPercentage;
			}*/
		}

		private bool ReadFlashOrEEPROM(OpenGD77CommsTransferData dataObj)
		{
			int old_progress = 0;
			byte[] sendbuffer = new byte[512];
			byte[] readbuffer = new byte[512];
			byte[] com_Buf = new byte[256];
			bool retVal = true;

			int currentDataAddressInTheRadio = dataObj.startDataAddressInTheRadio;
			int currentDataAddressInLocalBuffer = dataObj.localDataBufferStartPosition;

			int size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;

			while (size > 0)
			{
				if (size > 32)
				{
					size = 32;
				}

				sendbuffer[0] = (byte)'R';
				sendbuffer[1] = (byte)dataObj.mode;
				sendbuffer[2] = (byte)((currentDataAddressInTheRadio >> 24) & 0xFF);
				sendbuffer[3] = (byte)((currentDataAddressInTheRadio >> 16) & 0xFF);
				sendbuffer[4] = (byte)((currentDataAddressInTheRadio >> 8) & 0xFF);
				sendbuffer[5] = (byte)((currentDataAddressInTheRadio >> 0) & 0xFF);
				sendbuffer[6] = (byte)((size >> 8) & 0xFF);
				sendbuffer[7] = (byte)((size >> 0) & 0xFF);
				_port.Write(sendbuffer, 0, 8);
				while (_port.BytesToRead == 0)
				{
					Thread.Sleep(0);
				}
				_port.Read(readbuffer, 0, 64);

				if (readbuffer[0] == 'R')
				{
					int len = (readbuffer[1] << 8) + (readbuffer[2] << 0);
					for (int i = 0; i < len; i++)
					{
						dataObj.dataBuff[currentDataAddressInLocalBuffer++] = readbuffer[i + 3];
					}

					int progress = (currentDataAddressInTheRadio - dataObj.startDataAddressInTheRadio) * 100 / dataObj.transferLength;
					if (old_progress != progress)
					{
						updateProgess(progress);
						old_progress = progress;
					}

					currentDataAddressInTheRadio = currentDataAddressInTheRadio + len;
				}
				else
				{
					//Console.WriteLine(String.Format("read stopped (error at {0:X8})", currentDataAddressInTheRadio));
					close_data_mode();
					retVal = false;

				}
				size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;
			}
			close_data_mode();
			return retVal;
		}

		private void close_data_mode()
		{
			//data_mode = OpenGD77CommsTransferData.CommsDataMode.DataModeNone;
		}

		public bool readDataFromRadio()
		{
			bool retVal = true;
			DialogResult result = DialogResult.OK;
			lblMessage.Text = "";

			int calibrationDataSize = Marshal.SizeOf(typeof(CalibrationData));
			byte[] array = new byte[calibrationDataSize];

			String gd77CommPort = SetupDiWrap.ComPortNameFromFriendlyNamePrefix("OpenGD77");

			if (gd77CommPort != null)
			{
				_port = new SerialPort(gd77CommPort, 115200, Parity.None, 8, StopBits.One);
				_port.ReadTimeout = 1000;

				OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.NONE);

				_port.Open();
				sendCommand(0);
				sendCommand(1);
				sendCommand(2, 0, 0, 3, 1, 0, "CPS");// Write a line of text to CPS screen at position x=0,y=3 with font size 3, alignment centre
				sendCommand(2, 0, 16, 3, 1, 0, "Backup");
				sendCommand(2, 0, 32, 3, 1, 0, "Calibration");
				sendCommand(3);
				sendCommand(6, 3);// flash green LED

				dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadFlash;
				dataObj.dataBuff = new Byte[CALIBRATION_DATA_SIZE];
				dataObj.localDataBufferStartPosition = 0;
				dataObj.startDataAddressInTheRadio = MEMORY_LOCATION;
				dataObj.transferLength = CALIBRATION_DATA_SIZE;
				//displayMessage("Reading Calibration");
				if (!ReadFlashOrEEPROM(dataObj))
				{
					//displayMessage("Error while reading calibration");
					result = DialogResult.Abort;
					retVal = false;
					dataObj.responseCode = 1;
				}
				else
				{
					//displayMessage("");
				}
				sendCommand(5);// close CPS screen
				_port.Close();



				for (int p = 0; p < 8; p++)
				{
					if (dataObj.dataBuff[p] != CALIBRATION_HEADER[p])
					{
						MessageBox.Show("Calibration data could not be found. Please update your firmware");
						return false;
					}
				}

				Array.Copy(dataObj.dataBuff, 0, array, 0, calibrationDataSize);
				this.calibrationBandControlUHF.data = (CalibrationData)ByteToData(array);

				array = new byte[calibrationDataSize];
				Array.Copy(dataObj.dataBuff, 0 + VHF_OFFSET, array, 0, calibrationDataSize);
				this.calibrationBandControlVHF.data = (CalibrationData)ByteToData(array);
			}
			else
			{
				CommPrgForm commPrgForm;
				MainForm.CommsBuffer = new byte[1024 * 1024];

				// Pre-read to see if the calibration area appears to be readable.
				CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.dataRead;
				commPrgForm = new CommPrgForm(true);// true =  close download form as soon as download is complete
				commPrgForm.StartPosition = FormStartPosition.CenterParent;
				CodeplugComms.startAddress = CalibrationForm.CALIBRATION_MEMORY_LOCATION_OFFICIAL_USB_PROTOCOL;
				CodeplugComms.transferLength = 0x20;
				result = commPrgForm.ShowDialog();

				CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.calibrationRead;
				commPrgForm = new CommPrgForm(true);// true =  close download form as soon as download is complete
				commPrgForm.StartPosition = FormStartPosition.CenterParent;
				result = commPrgForm.ShowDialog();

				if (DialogResult.OK == result)
				{
					// Need to setup the VHF and UHF data storage class first, as its used when initialising the components

					Array.Copy(MainForm.CommsBuffer, CALIBRATION_MEMORY_LOCATION_OFFICIAL_USB_PROTOCOL, array, 0, calibrationDataSize);
					this.calibrationBandControlUHF.data = (CalibrationData)ByteToData(array);

					array = new byte[calibrationDataSize];
					Array.Copy(MainForm.CommsBuffer, CALIBRATION_MEMORY_LOCATION_OFFICIAL_USB_PROTOCOL + VHF_OFFSET, array, 0, calibrationDataSize);
					this.calibrationBandControlVHF.data = (CalibrationData)ByteToData(array);
				}
				else
				{
					lblMessage.Text = "Error";
					retVal = false;
				}
			}
			
			return retVal;

		}



		private void btnWrite_Click(object sender, EventArgs e)
		{
			bool retVal;

		/*	if (DialogResult.Yes != MessageBox.Show("Writing the calibration data to Radioddity GD-77 or any other compatible radio, could potentially damage your radio. By clicking 'Yes' you acknowledge that you use this feature entirely at your own risk", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2))
			{
				return;
			}
			*/
			String gd77CommPort = SetupDiWrap.ComPortNameFromFriendlyNamePrefix("OpenGD77");

			if (gd77CommPort != null)
			{
				OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.NONE);
				dataObj.dataBuff = new Byte[CALIBRATION_DATA_SIZE];

				_port = new SerialPort(gd77CommPort, 115200, Parity.None, 8, StopBits.One);
				_port.ReadTimeout = 1000;



				int calibrationDataSize = Marshal.SizeOf(typeof(CalibrationData));

				byte[] array = DataToByte(this.calibrationBandControlUHF.data);
				Array.Copy(array, 0, dataObj.dataBuff, 0, calibrationDataSize);

				array = DataToByte(this.calibrationBandControlVHF.data);
				Array.Copy(array, 0, dataObj.dataBuff, VHF_OFFSET, calibrationDataSize);



				_port.Open();
				sendCommand(0);
				sendCommand(1);
				sendCommand(2, 0, 0, 3, 1, 0, "CPS");
				sendCommand(2, 0, 16, 3, 1, 0, "Restoring");
				sendCommand(2, 0, 32, 3, 1, 0, "Calibration");
				sendCommand(3);
				sendCommand(6, 4);// flash red LED

				dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteFlash;
				 
				dataObj.localDataBufferStartPosition = 0;
				dataObj.startDataAddressInTheRadio = MEMORY_LOCATION;
				dataObj.transferLength = CALIBRATION_DATA_SIZE;
				//displayMessage("Restoring Flash");
				if (WriteFlash(dataObj))
				{
					//displayMessage("Restore complete");
				}
				else
				{
					MessageBox.Show("Error while restoring");
					//displayMessage("Error while restoring");
					dataObj.responseCode = 1;
				}
				sendCommand(6, 2);// Save settings and VFO's
				sendCommand(6, 1);// Reboot
				_port.Close();
				lblMessage.Text = "Calibration update completed";
				//MessageBox.Show("Calibration update completed");
			}
			else
			{
				// Pre-read to see if the Calibration area appears to be writable
				CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.dataRead;
				CommPrgForm commPrgForm = new CommPrgForm(true);// true =  close download form as soon as download is complete
				commPrgForm.StartPosition = FormStartPosition.CenterParent;
				CodeplugComms.startAddress = CALIBRATION_MEMORY_LOCATION_OFFICIAL_USB_PROTOCOL;
				CodeplugComms.transferLength = 0x20;
				DialogResult result = commPrgForm.ShowDialog();

				int calibrationDataSize = Marshal.SizeOf(typeof(CalibrationData));

				byte[] array = DataToByte(this.calibrationBandControlUHF.data);
				Array.Copy(array, 0, MainForm.CommsBuffer, CALIBRATION_MEMORY_LOCATION_OFFICIAL_USB_PROTOCOL, calibrationDataSize);

				array = DataToByte(this.calibrationBandControlVHF.data);
				Array.Copy(array, 0, MainForm.CommsBuffer, CALIBRATION_MEMORY_LOCATION_OFFICIAL_USB_PROTOCOL + VHF_OFFSET, calibrationDataSize);

				CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.calibrationWrite;

				commPrgForm = new CommPrgForm(true);// true =  close download form as soon as download is complete
				commPrgForm.StartPosition = FormStartPosition.CenterParent;
				commPrgForm.ShowDialog();
				MessageBox.Show("Calibration update completed");
			}
		}

		bool flashWriteSector(ref byte[] sendbuffer, ref byte[] readbuffer, OpenGD77CommsTransferData dataObj)
		{
			dataObj.data_sector = -1;

			sendbuffer[0] = (byte)'W';
			sendbuffer[1] = 3;
			_port.Write(sendbuffer, 0, 2);
			while (_port.BytesToRead == 0)
			{
				Thread.Sleep(0);
			}
			_port.Read(readbuffer, 0, 64);

			return ((readbuffer[0] == sendbuffer[0]) && (readbuffer[1] == sendbuffer[1]));
		}

		bool flashWritePrepareSector(int address, ref byte[] sendbuffer, ref byte[] readbuffer, OpenGD77CommsTransferData dataObj)
		{
			dataObj.data_sector = address / 4096;

			sendbuffer[0] = (byte)'W';
			sendbuffer[1] = 1;
			sendbuffer[2] = (byte)((dataObj.data_sector >> 16) & 0xFF);
			sendbuffer[3] = (byte)((dataObj.data_sector >> 8) & 0xFF);
			sendbuffer[4] = (byte)((dataObj.data_sector >> 0) & 0xFF);
			_port.Write(sendbuffer, 0, 5);
			while (_port.BytesToRead == 0)
			{
				Thread.Sleep(0);
			}
			_port.Read(readbuffer, 0, 64);

			return ((readbuffer[0] == sendbuffer[0]) && (readbuffer[1] == sendbuffer[1]));
		}

		bool flashSendData(int address, int len, ref byte[] sendbuffer, ref byte[] readbuffer)
		{
			sendbuffer[0] = (byte)'W';
			sendbuffer[1] = 2;
			sendbuffer[2] = (byte)((address >> 24) & 0xFF);
			sendbuffer[3] = (byte)((address >> 16) & 0xFF);
			sendbuffer[4] = (byte)((address >> 8) & 0xFF);
			sendbuffer[5] = (byte)((address >> 0) & 0xFF);
			sendbuffer[6] = (byte)((len >> 8) & 0xFF);
			sendbuffer[7] = (byte)((len >> 0) & 0xFF);
			_port.Write(sendbuffer, 0, len + 8);
			while (_port.BytesToRead == 0)
			{
				Thread.Sleep(0);
			}
			_port.Read(readbuffer, 0, 64);

			return ((readbuffer[0] == sendbuffer[0]) && (readbuffer[1] == sendbuffer[1]));
		}

		private bool WriteFlash(OpenGD77CommsTransferData dataObj)
		{
			int old_progress = 0;
			byte[] sendbuffer = new byte[512];
			byte[] readbuffer = new byte[512];
			byte[] com_Buf = new byte[256];
			int currentDataAddressInTheRadio = dataObj.startDataAddressInTheRadio;

			int currentDataAddressInLocalBuffer = dataObj.localDataBufferStartPosition;
			dataObj.data_sector = -1;// Always needs to be initialised to -1 so the first flashWritePrepareSector is called

			int size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;
			while (size > 0)
			{
				if (size > MAX_TRANSFER_SIZE)
				{
					size = MAX_TRANSFER_SIZE;
				}

				if (dataObj.data_sector == -1)
				{
					if (!flashWritePrepareSector(currentDataAddressInTheRadio, ref sendbuffer, ref readbuffer, dataObj))
					{
						//						close_data_mode();
						return false;
						//						break;
					};
				}

				if (dataObj.mode != 0)
				{
					int len = 0;
					for (int i = 0; i < size; i++)
					{
						sendbuffer[i + 8] = dataObj.dataBuff[currentDataAddressInLocalBuffer++];
						len++;

						if (dataObj.data_sector != ((currentDataAddressInTheRadio + len) / 4096))
						{
							break;
						}
					}
					if (flashSendData(currentDataAddressInTheRadio, len, ref sendbuffer, ref readbuffer))
					{
						int progress = (currentDataAddressInTheRadio - dataObj.startDataAddressInTheRadio) * 100 / dataObj.transferLength;
						if (old_progress != progress)
						{
							updateProgess(progress);
							old_progress = progress;
						}

						currentDataAddressInTheRadio = currentDataAddressInTheRadio + len;

						if (dataObj.data_sector != (currentDataAddressInTheRadio / 4096))
						{
							if (!flashWriteSector(ref sendbuffer, ref readbuffer, dataObj))
							{
								//close_data_mode();
								return false;
							};
						}
					}
					else
					{
						//close_data_mode();
						return false;
						//break;
					}
				}
				size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;
			}

			if (dataObj.data_sector != -1)
			{
				if (!flashWriteSector(ref sendbuffer, ref readbuffer, dataObj))
				{
					Console.WriteLine(String.Format("Error. Write stopped (write sector error at {0:X8})", currentDataAddressInTheRadio));
					return false;
				};
			}

			//			close_data_mode();
			return true;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			if (Control.ModifierKeys == Keys.Shift)
			{
				btnWrite.Visible = true;	
			}
			else
			{
				this.DialogResult = DialogResult.Cancel;
				Close();
			}
		}

		private CalibrationData ByteToData(byte[] byte_0)
		{
			int num = Marshal.SizeOf(typeof(CalibrationData));
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.Copy(byte_0, 0, intPtr, num);
			object result = Marshal.PtrToStructure(intPtr, typeof(CalibrationData));
			Marshal.FreeHGlobal(intPtr);
			return (CalibrationData)result;
		}

		public static byte[] DataToByte(CalibrationData object_0)
		{
			int num = Marshal.SizeOf(typeof(CalibrationData));
			byte[] array = new byte[num];
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.StructureToPtr(object_0, intPtr, false);
			Marshal.Copy(intPtr, array, 0, num);
			Marshal.FreeHGlobal(intPtr);
			return array;
		}

		private void onFormShown(object sender, EventArgs e)
		{
			MessageBox.Show("This feature is highly experental. You use it at your own risk! Make Sure you have a full backup of the Flash memory in your radio.","Warning");
		}

		private void btnReadFile_Clk(object sender, EventArgs e)
		{
			Byte[] buf=null;
			OpenFileDialog ofd = new OpenFileDialog();

			lblMessage.Text = "";

			DialogResult dialogResult = ofd.ShowDialog();
			if (dialogResult == DialogResult.OK && !string.IsNullOrEmpty(ofd.FileName))
			{
				int calibrationDataSize = Marshal.SizeOf(typeof(CalibrationData));
				byte[] array = new byte[calibrationDataSize];

				buf = File.ReadAllBytes(ofd.FileName);
				if (buf.Length == CALIBRATION_DATA_SIZE)
				{
					if (buf[0] == 0xA0 && buf[1] == 0x0F)
					{
						Array.Copy(buf, 0, array, 0, calibrationDataSize);
						this.calibrationBandControlUHF.data = (CalibrationData)ByteToData(array);

						array = new byte[calibrationDataSize];
						Array.Copy(buf, VHF_OFFSET, array, 0, calibrationDataSize);
						this.calibrationBandControlVHF.data = (CalibrationData)ByteToData(array);

						tabCtlBands.Visible = true;
						btnWrite.Visible = true;
					}
					else
					{
						MessageBox.Show("File contains invalid calibration header");
					}
				}
				else
				{
					MessageBox.Show("File is the wrong size");
				}

			}
		}

		private void btnReadFromRadio_Clk(object sender, EventArgs e)
		{
			if (readDataFromRadio())
			{
				tabCtlBands.Visible = true;
				btnWrite.Visible = true;
			}
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}
	}
}