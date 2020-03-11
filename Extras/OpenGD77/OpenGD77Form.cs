using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
//using Toub.Sound.Midi;

namespace DMR
{
	public partial class OpenGD77Form : Form
	{
		public static byte[] CustomData;
		private SerialPort _port = null;
		const string IDENTIFIER = "OpenGD77";

		private BackgroundWorker worker;

		public enum CommsDataMode { DataModeNone = 0, DataModeReadFlash = 1, DataModeReadEEPROM = 2, DataModeWriteFlash = 3, DataModeWriteEEPROM = 4 };
		public enum CommsAction { NONE, BACKUP_EEPROM, BACKUP_FLASH,RESTORE_EEPROM, RESTORE_FLASH ,READ_CODEPLUG, WRITE_CODEPLUG }
		public enum CustomDataType { UNINITIALISED_TYPE = 0xFF, IMAGE_TYPE = 1, MELODY_TYPE = 2 }

		private SaveFileDialog _saveFileDialog = new SaveFileDialog();
		private OpenFileDialog _openFileDialog = new OpenFileDialog();

		private const int MAX_TRANSFER_SIZE = 32;

		private OpenGD77Form.CommsAction _initialAction;

		public OpenGD77Form(OpenGD77Form.CommsAction initAction)
		{
			_initialAction = initAction;
			InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			int imagePosition=0;

			if (findCustomDataBlock(CustomDataType.IMAGE_TYPE, ref imagePosition))
			{
				loadImageFromCodeplug(imagePosition+8);
			}
			if (findCustomDataBlock(CustomDataType.MELODY_TYPE, ref imagePosition))
			{
				loadMelodyFromCodeplug(imagePosition + 8);
			}
		}

		int littleEndianToInt(byte[] dataBytes, int offset)
		{
			return dataBytes[offset] + dataBytes[offset+1] * 256 + dataBytes[offset+2] * 256 * 256 + dataBytes[offset+3] * 256 * 256 * 256;
		}
		private bool findCustomDataBlock(CustomDataType typeToFind, ref int returnedOffsetPos)
		{
			const int CUSTOM_DATA_BLOCK_HEADER_LENGTH = 8;
			returnedOffsetPos = 12;// Header is 12 bytes long in total
			do
			{
				if (CustomData[returnedOffsetPos] == (byte)typeToFind)
				{
					return true;
				}
				returnedOffsetPos += CUSTOM_DATA_BLOCK_HEADER_LENGTH + littleEndianToInt(CustomData, returnedOffsetPos + 4);// Move to next block

			} while (returnedOffsetPos < CustomData.Length - CUSTOM_DATA_BLOCK_HEADER_LENGTH);

			return false;
		}

		private void loadMelodyFromCodeplug(int position)
		{
			string melodyStr = "";
			for (int i = 0; i < 512; i++)
			{
				if (CustomData[position + i] == 0 && CustomData[position + i + 1] == 0)
				{
					break;
				}
				melodyStr += CustomData[position + i].ToString() + ",";
			}
			if (melodyStr == "")
			{
				melodyStr = "0,0,";
			}
			txtBootTune.Text = melodyStr.Substring(0, melodyStr.Length - 1);// remove the last comma
		}

		private void loadImageFromCodeplug(int position)
		{
			Bitmap bm_x1 = new Bitmap(128, 64);
			Graphics g = Graphics.FromImage(bm_x1);

			g.Clear(Color.White);
							
			for (int stripe = 0; stripe < 8; stripe++)
			{
				for (int column = 0; column < 128; column++)
				{
					for (int line = 0; line < 8; line++)
					{
						if (((CustomData[position + (stripe * 128) + column] >> line) & 0x01) != 0)
						{
							bm_x1.SetPixel(column, stripe * 8 + line, Color.Black);
						}
					}
				}
			}
			pictureBox1.Image = bm_x1;
		}

		public static byte[] LoadCustomData(byte [] eeprom)
		{
			CustomData=new byte[0x20000-0x1EE60];
			
			Array.Copy(eeprom, 0x1EE60, OpenGD77Form.CustomData, 0, CustomData.Length);

			bool foundTheIdentifier = true;
			for (int i = 0; i < IDENTIFIER.Length; i++)
			{
				if (CustomData[i] != IDENTIFIER[i])
				{
					foundTheIdentifier = false;
					break;
				}
			}

			if (foundTheIdentifier == false)
			{
				CustomData.Fill((byte)0xFF);

				Array.Copy(Encoding.ASCII.GetBytes(IDENTIFIER), CustomData, IDENTIFIER.Length);// Add the indentifier
				CustomData[8] = 0x01;// version
				CustomData[9] = 0x00;
				CustomData[10] = 0x00;
				CustomData[11] = 0x00;
			}

			return CustomData;
		}




		bool sendCommand(int commandNumber, int x_or_command_option_number = 0, int y = 0, int iSize = 0, int alignment = 0, int isInverted = 0, string message = "")
		{
			int retries = 100;
			byte[] buffer=new byte[64]; 
			buffer[0] = (byte)'C';
			buffer[1] = (byte)commandNumber;

			switch(commandNumber)
			{
				case 2:
					buffer[3] = (byte)y;
					buffer[4] = (byte)iSize;
					buffer[5] = (byte) alignment;
					buffer[6] = (byte) isInverted;
					Buffer.BlockCopy(Encoding.ASCII.GetBytes(message), 0, buffer, 7, Math.Min(message.Length,16));// copy the string into bytes 7 onwards
					break;
				case 6:
					// Special command
					buffer[2] = (byte)x_or_command_option_number;
					break;
				default:

					break;

			}
			commPort.Write(buffer, 0, 32);
			while (commPort.BytesToRead == 0 && retries-- > 0)
			{
				Thread.Sleep(1);
			}
			if (retries != -1)
			{
				commPort.Read(buffer, 0, 64);
			}
			return ((buffer[1] == commandNumber) && (retries!=-1));
		}


		bool flashWritePrepareSector(int address, ref byte[] sendbuffer, ref byte[] readbuffer,OpenGD77CommsTransferData dataObj)
		{
			dataObj.data_sector = address / 4096;

			sendbuffer[0] = (byte)'W';
			sendbuffer[1] = 1;
			sendbuffer[2] = (byte)((dataObj.data_sector >> 16) & 0xFF);
			sendbuffer[3] = (byte)((dataObj.data_sector >> 8) & 0xFF);
			sendbuffer[4] = (byte)((dataObj.data_sector >> 0) & 0xFF);
			commPort.Write(sendbuffer, 0, 5);
			while (commPort.BytesToRead == 0)
			{
				Thread.Sleep(0);
			}
			commPort.Read(readbuffer, 0, 64);

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
			commPort.Write(sendbuffer, 0, len + 8);
			while (commPort.BytesToRead == 0)
			{
				Thread.Sleep(0);
			}
			commPort.Read(readbuffer, 0, 64);

			return ((readbuffer[0] == sendbuffer[0]) && (readbuffer[1] == sendbuffer[1]));
		}

		bool flashWriteSector(ref byte[] sendbuffer, ref byte[] readbuffer,OpenGD77CommsTransferData dataObj)
		{
			dataObj.data_sector = -1;

			sendbuffer[0] = (byte)'W';
			sendbuffer[1] = 3;
			commPort.Write(sendbuffer, 0, 2);
			while (commPort.BytesToRead == 0)
			{
				Thread.Sleep(0);
			}
			commPort.Read(readbuffer, 0, 64);

			return ((readbuffer[0] == sendbuffer[0]) && (readbuffer[1] == sendbuffer[1]));
		}

		private void close_data_mode()
		{
			//data_mode = OpenGD77CommsTransferData.CommsDataMode.DataModeNone;
		}


		private bool ReadFlashOrEEPROMOrROMOrScreengrab(OpenGD77CommsTransferData dataObj)
		{
			int old_progress = 0;
			byte[] sendbuffer = new byte[512];
			byte[] readbuffer = new byte[512];
			byte[] com_Buf = new byte[256];

			int currentDataAddressInTheRadio = dataObj.startDataAddressInTheRadio;
			int currentDataAddressInLocalBuffer = dataObj.localDataBufferStartPosition;

			int size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;

			while (size > 0)
			{
				if (size > MAX_TRANSFER_SIZE)
				{
					size = MAX_TRANSFER_SIZE;
				}

				sendbuffer[0] = (byte)'R';
				sendbuffer[1] = (byte)dataObj.mode;
				sendbuffer[2] = (byte)((currentDataAddressInTheRadio >> 24) & 0xFF);
				sendbuffer[3] = (byte)((currentDataAddressInTheRadio >> 16) & 0xFF);
				sendbuffer[4] = (byte)((currentDataAddressInTheRadio >> 8) & 0xFF);
				sendbuffer[5] = (byte)((currentDataAddressInTheRadio >> 0) & 0xFF);
				sendbuffer[6] = (byte)((size >> 8) & 0xFF);
				sendbuffer[7] = (byte)((size >> 0) & 0xFF);
				commPort.Write(sendbuffer, 0, 8);
				while (commPort.BytesToRead == 0)
				{
					Thread.Sleep(0);
				}
				commPort.Read(readbuffer, 0, 64);

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
					Console.WriteLine(String.Format("read stopped (error at {0:X8})", currentDataAddressInTheRadio));
//					close_data_mode();
					return false;

				}
				size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;
			}
//			close_data_mode();
			return true;
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
					if (!flashWritePrepareSector(currentDataAddressInTheRadio, ref sendbuffer, ref readbuffer,dataObj))
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
							if (!flashWriteSector(ref sendbuffer, ref readbuffer,dataObj))
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
				if (!flashWriteSector(ref sendbuffer, ref readbuffer,dataObj))
				{
					Console.WriteLine(String.Format("Error. Write stopped (write sector error at {0:X8})", currentDataAddressInTheRadio));
					return false;
				};
			}

//			close_data_mode();
			return true;
		}

		private bool WriteEEPROM(OpenGD77CommsTransferData dataObj)
		{
			int old_progress = 0;
			byte[] sendbuffer = new byte[512];
			byte[] readbuffer = new byte[512];
			byte[] com_Buf = new byte[256];

			int currentDataAddressInTheRadio = dataObj.startDataAddressInTheRadio;
			int currentDataAddressInLocalBuffer = dataObj.localDataBufferStartPosition;

			int size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;
			while (size > 0)
			{
				if (size > MAX_TRANSFER_SIZE)
				{
					size = MAX_TRANSFER_SIZE;
				}

				if (dataObj.data_sector == -1)
				{
					dataObj.data_sector = currentDataAddressInTheRadio / 128;
				}

				int len = 0;
				for (int i = 0; i < size; i++)
				{
					sendbuffer[i + 8] = (byte)dataObj.dataBuff[currentDataAddressInLocalBuffer++];
					len++;

					if (dataObj.data_sector != ((currentDataAddressInTheRadio + len) / 128))
					{
						dataObj.data_sector = -1;
						break;
					}
				}

				sendbuffer[0] = (byte)'W';
				sendbuffer[1] = 4;
				sendbuffer[2] = (byte)((currentDataAddressInTheRadio >> 24) & 0xFF);
				sendbuffer[3] = (byte)((currentDataAddressInTheRadio >> 16) & 0xFF);
				sendbuffer[4] = (byte)((currentDataAddressInTheRadio >> 8) & 0xFF);
				sendbuffer[5] = (byte)((currentDataAddressInTheRadio >> 0) & 0xFF);
				sendbuffer[6] = (byte)((len >> 8) & 0xFF);
				sendbuffer[7] = (byte)((len >> 0) & 0xFF);
				commPort.Write(sendbuffer, 0, len + 8);
				while (commPort.BytesToRead == 0)
				{
					Thread.Sleep(0);
				}
				commPort.Read(readbuffer, 0, 64);

				if ((readbuffer[0] == sendbuffer[0]) && (readbuffer[1] == sendbuffer[1]))
				{
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
					Console.WriteLine(String.Format("Error. Write stopped (write sector error at {0:X8})", currentDataAddressInTheRadio));
					//close_data_mode();
					return false;
				}
				size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;
			}
			//close_data_mode();
			return true;
		}

		void updateProgess(int progressPercentage)
		{
			if (progressBar1.InvokeRequired)
				progressBar1.Invoke(new MethodInvoker(delegate()
				{
					progressBar1.Value = progressPercentage;
				}));
			else
			{
				progressBar1.Value = progressPercentage;
			}
		}

		void displayMessage(string message)
		{
			if (txtMessage.InvokeRequired)
				txtMessage.Invoke(new MethodInvoker(delegate()
				{
					txtMessage.Text = message;
				}));
			else
			{
				txtMessage.Text = message;
			}
		}


		void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			OpenGD77CommsTransferData dataObj = e.Result as OpenGD77CommsTransferData;

			if (dataObj.action != OpenGD77CommsTransferData.CommsAction.NONE)
			{
				if (dataObj.responseCode == 0)
				{
					switch (dataObj.action)
					{
						case OpenGD77CommsTransferData.CommsAction.BACKUP_EEPROM:
							_saveFileDialog.Filter = "EEPROM files (*.bin)|*.bin";
							_saveFileDialog.FilterIndex = 1;
							if (_saveFileDialog.ShowDialog() == DialogResult.OK)
							{
								File.WriteAllBytes(_saveFileDialog.FileName, dataObj.dataBuff);
							}
							enableDisableAllButtons(true);
							dataObj.action = OpenGD77CommsTransferData.CommsAction.NONE;
							break;
						case OpenGD77CommsTransferData.CommsAction.BACKUP_FLASH:
							_saveFileDialog.Filter = "Flash files (*.bin)|*.bin";
							_saveFileDialog.FilterIndex = 1;
							if (_saveFileDialog.ShowDialog() == DialogResult.OK)
							{
								File.WriteAllBytes(_saveFileDialog.FileName, dataObj.dataBuff);
							}
							enableDisableAllButtons(true);
							dataObj.action = OpenGD77CommsTransferData.CommsAction.NONE;
							break;
						case OpenGD77CommsTransferData.CommsAction.BACKUP_CALIBRATION:

							for (int p = 0; p < 8; p++)
							{
								if (dataObj.dataBuff[p] != CalibrationForm.CALIBRATION_HEADER[p])
								{
									MessageBox.Show("Calibration data could not be found. Please update your firmware");
									return;
								}
							}
							_saveFileDialog.Filter = "Flash files (*.bin)|*.bin";
							_saveFileDialog.FilterIndex = 1;
							if (_saveFileDialog.ShowDialog() == DialogResult.OK)
							{
								File.WriteAllBytes(_saveFileDialog.FileName, dataObj.dataBuff);
							}
							enableDisableAllButtons(true);
							dataObj.action = OpenGD77CommsTransferData.CommsAction.NONE;
							break;
						case OpenGD77CommsTransferData.CommsAction.RESTORE_EEPROM:
						case OpenGD77CommsTransferData.CommsAction.RESTORE_FLASH:
						case OpenGD77CommsTransferData.CommsAction.RESTORE_CALIBRATION:
							MessageBox.Show("Restore complete");
							enableDisableAllButtons(true);
							dataObj.action = OpenGD77CommsTransferData.CommsAction.NONE;
							break;
						case OpenGD77CommsTransferData.CommsAction.READ_CODEPLUG:
							MessageBox.Show("Read Codeplug complete");
#if OpenGD77
							if (!MainForm.checkZonesFor80Channels(dataObj.dataBuff))
							{
								MainForm.convertTo80ChannelZoneCodeplug(dataObj.dataBuff);
							}
#endif
							MainForm.ByteToData(dataObj.dataBuff);
							enableDisableAllButtons(true);
							{
								int imagePosition=0;
								if (findCustomDataBlock(CustomDataType.IMAGE_TYPE, ref imagePosition))
								{
									loadImageFromCodeplug(imagePosition + 8);
								}
								if (findCustomDataBlock(CustomDataType.MELODY_TYPE, ref imagePosition))
								{
									loadMelodyFromCodeplug(imagePosition + 8);
								}
							}
							if (_initialAction == CommsAction.READ_CODEPLUG)
							{
								this.Close();
							}
							break;
						case OpenGD77CommsTransferData.CommsAction.WRITE_CODEPLUG:
							enableDisableAllButtons(true);
							if (_initialAction == CommsAction.WRITE_CODEPLUG)
							{
								this.Close();
							}
							break;
						case OpenGD77CommsTransferData.CommsAction.BACKUP_MCU_ROM:
							_saveFileDialog.Filter = "MCU ROM (*.bin)|*.bin";
							_saveFileDialog.FilterIndex = 1;
							if (_saveFileDialog.ShowDialog() == DialogResult.OK)
							{
								File.WriteAllBytes(_saveFileDialog.FileName, dataObj.dataBuff);
							}
							enableDisableAllButtons(true);
							dataObj.action = OpenGD77CommsTransferData.CommsAction.NONE;
							break;
						case OpenGD77CommsTransferData.CommsAction.DOWLOAD_SCREENGRAB:

							Bitmap bm_x1 = new Bitmap(128, 64);
							Graphics g = Graphics.FromImage(bm_x1);
							Color colour = ColorTranslator.FromHtml("#99d9ea");
							g.Clear(colour);
							
							for (int stripe = 0; stripe < 8; stripe++)
							{
								for (int column = 0; column < 128; column++)
								{
									for (int line = 0; line < 8; line++)
									{
										if (((dataObj.dataBuff[(stripe * 128) + column] >> line) & 0x01) !=0 )
										{
											bm_x1.SetPixel(column, stripe * 8 + line, Color.Black);
										}
									}
								}
							}

							Bitmap obm = ResizeImage(bm_x1, bm_x1.Width * 2, bm_x1.Height * 2);
							Clipboard.SetImage(obm);

							_saveFileDialog.Filter = "Screengrab files (*.png)|*.png";
							_saveFileDialog.FilterIndex = 1;
							if (_saveFileDialog.ShowDialog() == DialogResult.OK)
							{
								obm.Save(_saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
							}
							enableDisableAllButtons(true);
							dataObj.action = OpenGD77CommsTransferData.CommsAction.NONE;
							break;
					}
				}
				else
				{
					MessageBox.Show("There has been an error. Refer to the last status message that was displayed", "Oops");
				}
			}
			progressBar1.Value = 0;
		}

		void worker_DoWork(object sender, DoWorkEventArgs e)
		{

			OpenGD77CommsTransferData dataObj = e.Argument as OpenGD77CommsTransferData;
			const int CODEPLUG_FLASH_PART_END	= 0x1EE60;
			const int CODEPLUG_FLASH_PART_START = 0xB000;
			if (commPort == null)
			{
				return;
			}
			try
			{
				switch (dataObj.action)
				{
					case OpenGD77CommsTransferData.CommsAction.BACKUP_FLASH:
						if (commPort == null)
						{
							return;
						}
						try
						{
							commPort.Open();
						}
						catch (Exception)
						{
							MessageBox.Show("Comm port not available");
							return;
						}
						// show CPS screen
						if (!sendCommand(0))
						{
							displayMessage("Error connecting to the OpenGD77");
							dataObj.responseCode = 1;
							break;
						}
						sendCommand(1);// Clear screen
						sendCommand(2, 0, 0, 3, 1, 0, "CPS");// Write a line of text to CPS screen at position x=0,y=3 with font size 3, alignment centre
						sendCommand(2, 0, 16, 3, 1, 0, "Backup");// Write a line of text to CPS screen
						sendCommand(2, 0, 32, 3, 1, 0, "Flash");// Write a line of text to CPS screen
						sendCommand(3);// render CPS
						sendCommand(6,3);// flash green LED

						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadFlash;
						dataObj.dataBuff = new Byte[1024 * 1024];
						dataObj.localDataBufferStartPosition = 0;
						dataObj.startDataAddressInTheRadio = 0;
						dataObj.transferLength = 1024 * 1024;
						displayMessage("Reading Flash");
						if (!ReadFlashOrEEPROMOrROMOrScreengrab(dataObj))
						{
							displayMessage("Error while reading flash");
							dataObj.responseCode = 1;
						}
						else
						{
							displayMessage("");
						}
						sendCommand(5);// close CPS screen
						commPort.Close();
						break;
					case OpenGD77CommsTransferData.CommsAction.BACKUP_CALIBRATION:
						if (commPort == null)
						{
							return;
						}
						try
						{
							commPort.Open();
						}
						catch (Exception)
						{
							MessageBox.Show("Comm port not available");
							return;
						}
						if (!sendCommand(0))
						{
							displayMessage("Error connecting to the OpenGD77");
							dataObj.responseCode = 1;
							break;
						}
						sendCommand(1);
						sendCommand(2, 0, 0, 3, 1, 0, "CPS");// Write a line of text to CPS screen at position x=0,y=3 with font size 3, alignment centre
						sendCommand(2, 0, 16, 3, 1, 0, "Backup");
						sendCommand(2, 0, 32, 3, 1, 0, "Calibration");
						sendCommand(3);
						sendCommand(6, 3);// flash green LED

						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadFlash;
						dataObj.dataBuff = new Byte[CalibrationForm.CALIBRATION_DATA_SIZE];
						dataObj.localDataBufferStartPosition = 0;
						dataObj.startDataAddressInTheRadio = CalibrationForm.MEMORY_LOCATION;
						dataObj.transferLength = CalibrationForm.CALIBRATION_DATA_SIZE;
						displayMessage("Reading Calibration");
						if (!ReadFlashOrEEPROMOrROMOrScreengrab(dataObj))
						{
							displayMessage("Error while reading calibration");
							dataObj.responseCode = 1;
						}
						else
						{
							displayMessage("");
						}
						sendCommand(5);// close CPS screen
						commPort.Close();
						break;
					case OpenGD77CommsTransferData.CommsAction.BACKUP_EEPROM:
						if (commPort == null)
						{
							return;
						}
						try
						{
							commPort.Open();
						}
						catch (Exception)
						{
							MessageBox.Show("Comm port not available");
							return;
						}
						if (!sendCommand(0))
						{
							displayMessage("Error connecting to the OpenGD77");
							dataObj.responseCode = 1;
							break;
						}
						sendCommand(1);
						sendCommand(2, 0, 0, 3, 1, 0, "CPS");
						sendCommand(2, 0, 16, 3, 1, 0, "Backup");
						sendCommand(2, 0, 32, 3, 1, 0, "EEPROM");
						sendCommand(3);
						sendCommand(6,3);// flash green LED

						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadEEPROM;
						dataObj.dataBuff = new Byte[64 * 1024];

						dataObj.localDataBufferStartPosition = 0;
						dataObj.startDataAddressInTheRadio = 0;
						dataObj.transferLength = 64*1024;
						displayMessage("Reading EEPROM");
						if (!ReadFlashOrEEPROMOrROMOrScreengrab(dataObj))
						{
							displayMessage("Error while reading EEPROM");
							dataObj.responseCode = 1;
						}
						else
						{
							displayMessage("");
						}
						sendCommand(5);
						commPort.Close();
						break;

					case OpenGD77CommsTransferData.CommsAction.RESTORE_FLASH:
						if (commPort == null)
						{
							return;
						}
						try
						{
							commPort.Open();
						}
						catch (Exception)
						{
							MessageBox.Show("Comm port not available");
							return;
						}
						if (!sendCommand(0))
						{
							displayMessage("Error connecting to the OpenGD77");
							dataObj.responseCode = 1;
							break;
						}
						sendCommand(1);
						sendCommand(2, 0, 0, 3, 1, 0, "CPS");
						sendCommand(2, 0, 16, 3, 1, 0, "Restoring");
						sendCommand(2, 0, 32, 3, 1, 0, "Flash");
						sendCommand(3);
						sendCommand(6,4);// flash red LED

						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteFlash;
						dataObj.localDataBufferStartPosition = 0;
						dataObj.startDataAddressInTheRadio = 0;
						dataObj.transferLength = 1024 * 1024;
						displayMessage("Restoring Flash");
						if (WriteFlash(dataObj))
						{
							displayMessage("Restore complete");
						}
						else
						{
							MessageBox.Show("Error while restoring");
							displayMessage("Error while restoring");
							dataObj.responseCode = 1;
						}
						sendCommand(6, 2);// Save settings and VFO
						sendCommand(6, 1);// Reboot
						commPort.Close();
						break;
					case OpenGD77CommsTransferData.CommsAction.RESTORE_CALIBRATION:
						if (commPort == null)
						{
							return;
						}
						try
						{
							commPort.Open();
						}
						catch (Exception)
						{
							MessageBox.Show("Comm port not available");
							return;
						}
						if (!sendCommand(0))
						{
							displayMessage("Error connecting to the OpenGD77");
							dataObj.responseCode = 1;
							break;
						}
						sendCommand(1);
						sendCommand(2, 0, 0, 3, 1, 0, "CPS");
						sendCommand(2, 0, 16, 3, 1, 0, "Restoring");
						sendCommand(2, 0, 32, 3, 1, 0, "Calibration");
						sendCommand(3);
						sendCommand(6, 4);// flash red LED


						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteFlash;
						dataObj.localDataBufferStartPosition = 0;
						dataObj.startDataAddressInTheRadio = CalibrationForm.MEMORY_LOCATION;
						dataObj.transferLength = CalibrationForm.CALIBRATION_DATA_SIZE;
						displayMessage("Restoring Calibration");
						if (WriteFlash(dataObj))
						{
							displayMessage("Restore complete");
						}
						else
						{
							MessageBox.Show("Error while restoring Calibration");
							displayMessage("Error while restoring Calibration");
							dataObj.responseCode = 1;
						}
						sendCommand(6, 2);// Save settings and VFO's to codeplug
						sendCommand(6, 1);// Reboot
						commPort.Close();
						break;
					case OpenGD77CommsTransferData.CommsAction.RESTORE_EEPROM:
						if (commPort == null)
						{
							return;
						}
						try
						{
							commPort.Open();
						}
						catch (Exception)
						{
							MessageBox.Show("Comm port not available");
							return;
						}
						if (!sendCommand(0))
						{
							displayMessage("Error connecting to the OpenGD77");
							dataObj.responseCode = 1;
							break;
						}
						sendCommand(1);
						sendCommand(2, 0, 0, 3, 1, 0, "CPS");
						sendCommand(2, 0, 16, 3, 1, 0, "Restoring");
						sendCommand(2, 0, 32, 3, 1, 0, "EEPROM");
						sendCommand(3);
						sendCommand(6,4);// flash red LED

						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteEEPROM;
						dataObj.localDataBufferStartPosition = 0;
						dataObj.startDataAddressInTheRadio = 0;
						dataObj.transferLength = 64 * 1024;
						displayMessage("Restoring EEPROM");
						if (WriteEEPROM(dataObj))
						{
							displayMessage("Restore complete");
						}
						else
						{
							MessageBox.Show("Error while restoring");
							displayMessage("Error while restoring");
							dataObj.responseCode = 1;
						}
						sendCommand(6, 0);// save settings (Not VFOs ) and reboot
						commPort.Close();
						break;
					case OpenGD77CommsTransferData.CommsAction.READ_CODEPLUG:
						if (commPort == null)
						{
							return;
						}
						try
						{
							commPort.Open();
						}
						catch (Exception)
						{
							MessageBox.Show("Comm port not available");
							return;
						}
						if (!sendCommand(0))
						{
							displayMessage("Error connecting to the OpenGD77");
							dataObj.responseCode = 1;
							break;
						}

						sendCommand(1);
						sendCommand(2, 0, 0, 3, 1, 0, "CPS");
						sendCommand(2, 0, 16, 3, 1, 0, "Reading");
						sendCommand(2, 0, 32, 3, 1, 0, "Codeplug");
						sendCommand(3);
						sendCommand(6,3);// flash green LED
						sendCommand(6, 2);// Save settuings VFO's to codeplug

						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadEEPROM;
						dataObj.localDataBufferStartPosition = 0x00E0;
						dataObj.startDataAddressInTheRadio = dataObj.localDataBufferStartPosition;
						dataObj.transferLength =  0x6000 - dataObj.localDataBufferStartPosition;
						displayMessage(String.Format("Reading EEPROM 0x{0:X6} - 0x{1:X6}", dataObj.localDataBufferStartPosition, (dataObj.localDataBufferStartPosition + dataObj.transferLength)));

						if (!ReadFlashOrEEPROMOrROMOrScreengrab(dataObj))
						{
							displayMessage("Error while reading");
							dataObj.responseCode = 1;
							break;
						}
	
						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadEEPROM;
						dataObj.localDataBufferStartPosition = 0x7500;
						dataObj.startDataAddressInTheRadio = dataObj.localDataBufferStartPosition;
						dataObj.transferLength = 0xB000 - dataObj.localDataBufferStartPosition;
						displayMessage(String.Format("Reading EEPROM 0x{0:X6} - 0x{1:X6}", dataObj.localDataBufferStartPosition, (dataObj.localDataBufferStartPosition + dataObj.transferLength)));
						if (!ReadFlashOrEEPROMOrROMOrScreengrab(dataObj))
						{
							displayMessage("Error while reading");
							dataObj.responseCode = 1;
							break;
						}

						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadFlash;
						dataObj.localDataBufferStartPosition = CODEPLUG_FLASH_PART_START;
						dataObj.startDataAddressInTheRadio = 0x7b000;
						dataObj.transferLength = CODEPLUG_FLASH_PART_END - dataObj.localDataBufferStartPosition;
						displayMessage(String.Format("Reading Flash 0x{0:X6} - 0x{1:X6}", dataObj.localDataBufferStartPosition, dataObj.localDataBufferStartPosition + dataObj.transferLength));

						if (!ReadFlashOrEEPROMOrROMOrScreengrab(dataObj))
						{
							displayMessage("Error while reading");
							dataObj.responseCode = 1;
							break;
						}


						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadFlash;
						dataObj.localDataBufferStartPosition = 0x1EE60;// OpenGD77 custom data in Flash
						dataObj.startDataAddressInTheRadio = 0;
						dataObj.transferLength = 0x20000 - 0x1EE60;
						displayMessage(String.Format("Reading Flash 0x{0:X6} - 0x{1:X6}", dataObj.localDataBufferStartPosition, dataObj.localDataBufferStartPosition + dataObj.transferLength));

						if (!ReadFlashOrEEPROMOrROMOrScreengrab(dataObj))
						{
							displayMessage("Error while reading");
							dataObj.responseCode = 1;
							break;
						}
						else
						{
							displayMessage("Codeplug read complete");
						}
						sendCommand(5);// close CPS screen on radio
						commPort.Close();
						break;
					case OpenGD77CommsTransferData.CommsAction.WRITE_CODEPLUG:
						if (commPort == null)
						{
							return;
						}
						try
						{
							commPort.Open();
						}
						catch (Exception)
						{
							MessageBox.Show("Comm port not available");
							return;
						}
					
						if (!sendCommand(0))
						{
							displayMessage("Error connecting to the OpenGD77");
							dataObj.responseCode = 1;
							break;
						}
						sendCommand(1);
						sendCommand(2, 0, 0, 3, 1, 0, "CPS");
						sendCommand(2, 0, 16, 3, 1, 0, "Writing");
						sendCommand(2, 0, 32, 3, 1, 0, "Codeplug");
						sendCommand(3);
						sendCommand(6,4);// flash red LED
						sendCommand(6, 2);// Save settings and VFOs

						dataObj.dataBuff = MainForm.DataToByte();
						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteEEPROM;
						dataObj.localDataBufferStartPosition = 0x00E0;
						dataObj.startDataAddressInTheRadio = dataObj.localDataBufferStartPosition;
						dataObj.transferLength =  0x6000 - dataObj.localDataBufferStartPosition;
						displayMessage(String.Format("Writing EEPROM 0x{0:X6} - 0x{1:X6}", dataObj.localDataBufferStartPosition, dataObj.localDataBufferStartPosition + dataObj.transferLength));

						if (!WriteEEPROM(dataObj))
						{
							displayMessage("Error while writing");
							dataObj.responseCode = 1;
							break;
						}

						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteEEPROM;
						dataObj.localDataBufferStartPosition = 0x7500;
						dataObj.startDataAddressInTheRadio = dataObj.localDataBufferStartPosition;
						dataObj.transferLength = 0xB000 - dataObj.localDataBufferStartPosition;
						displayMessage(String.Format("Writing EEPROM 0x{0:X6} - 0x{1:X6}", dataObj.localDataBufferStartPosition, dataObj.localDataBufferStartPosition + dataObj.transferLength));
						if (!WriteEEPROM(dataObj))
						{
							displayMessage("Error while writing");
							dataObj.responseCode = 1;
							break;
						}

						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteFlash;
						dataObj.localDataBufferStartPosition = CODEPLUG_FLASH_PART_START;
						dataObj.startDataAddressInTheRadio = 0x7b000;
						dataObj.transferLength = CODEPLUG_FLASH_PART_END - dataObj.localDataBufferStartPosition;
						displayMessage(String.Format("Writing Flash 0x{0:X6} - 0x{1:X6}", dataObj.localDataBufferStartPosition, dataObj.localDataBufferStartPosition + dataObj.transferLength));


						if (!WriteFlash(dataObj))
						{
							displayMessage("Error while writing");
							dataObj.responseCode = 1;
							break;
						}
						
						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteFlash;
						dataObj.localDataBufferStartPosition = 0x1EE60;
						dataObj.startDataAddressInTheRadio = 0x00000;
						dataObj.transferLength = 0x20000 - dataObj.localDataBufferStartPosition;
						displayMessage(String.Format("Writing Flash 0x{0:X6} - 0x{1:X6}", dataObj.localDataBufferStartPosition, dataObj.localDataBufferStartPosition + dataObj.transferLength));

						if (!WriteFlash(dataObj))
						{
							displayMessage("Error while writing");
							dataObj.responseCode = 1;
							break;
						}
						else
						{
							displayMessage("Codeplug write complete");
						}


						sendCommand(6, 0);// Save settings (NOT VFOs)
						commPort.Close();
						break;

					case OpenGD77CommsTransferData.CommsAction.BACKUP_MCU_ROM:
						if (commPort == null)
						{
							return;
						}
						try
						{
							commPort.Open();
						}
						catch (Exception)
						{
							MessageBox.Show("Comm port not available");
							return;
						}
						// show CPS screen
						if (!sendCommand(0))
						{
							displayMessage("Error connecting to the OpenGD77");
							dataObj.responseCode = 1;
							break;
						}
						sendCommand(1);// Clear screen
						sendCommand(2, 0, 0, 3, 1, 0, "CPS");// Write a line of text to CPS screen at position x=0,y=3 with font size 3, alignment centre
						sendCommand(2, 0, 16, 3, 1, 0, "Backup");// Write a line of text to CPS screen
						sendCommand(2, 0, 32, 3, 1, 0, "MCU ROM");// Write a line of text to CPS screen
						sendCommand(3);// render CPS
						sendCommand(6, 3);// flash green LED

						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadMCUROM;
						dataObj.dataBuff = new Byte[512 * 1024];
						dataObj.localDataBufferStartPosition = 0;
						dataObj.startDataAddressInTheRadio = 0;
						dataObj.transferLength = 512 * 1024;
						displayMessage("Reading MCU ROM");
						if (!ReadFlashOrEEPROMOrROMOrScreengrab(dataObj))
						{
							displayMessage("Error while reading MCU ROM");
							dataObj.responseCode = 1;
						}
						else
						{
							displayMessage("");
						}
						sendCommand(5);// close CPS screen
						commPort.Close();
						break;

					case OpenGD77CommsTransferData.CommsAction.DOWLOAD_SCREENGRAB:
						if (commPort == null)
						{
							return;
						}
						try
						{
							commPort.Open();
						}
						catch (Exception)
						{
							MessageBox.Show("Comm port not available");
							return;
						}
						// show CPS screen
						/*
						if (!sendCommand(0))
						{
							displayMessage("Error connecting to the OpenGD77");
							dataObj.responseCode = 1;
							break;
						}
						*/
						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadScreenGrab;
						dataObj.dataBuff = new Byte[1 * 1024];
						dataObj.localDataBufferStartPosition = 0;
						dataObj.startDataAddressInTheRadio = 0;
						dataObj.transferLength = 1 * 1024;
						displayMessage("Downloading Screengrab");
						if (!ReadFlashOrEEPROMOrROMOrScreengrab(dataObj))
						{
							displayMessage("Error while downloading Screengrab");
							dataObj.responseCode = 1;
						}
						else
						{
							displayMessage("");
						}
						//sendCommand(5);// close CPS screen
						commPort.Close();
						break;

				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			e.Result = dataObj;
		}

		void perFormCommsTask(OpenGD77CommsTransferData dataObj)
		{
			try
			{
				worker = new BackgroundWorker();
				worker.DoWork += new DoWorkEventHandler(worker_DoWork);
				worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
				worker.RunWorkerAsync(dataObj);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnBackupEEPROM_Click(object sender, EventArgs e)
		{
			if (commPort==null)
			{
				MessageBox.Show("No com port. Close and reopen the OpenGD77 window to select a com port");
				return;
			}
			OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.BACKUP_EEPROM);
			enableDisableAllButtons(false);
			perFormCommsTask(dataObj);
		}

		private void btnBackupFlash_Click(object sender, EventArgs e)
		{
			if (commPort==null)
			{
				MessageBox.Show("No com port. Close and reopen the OpenGD77 window to select a com port");
				return;
			}

			OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.BACKUP_FLASH);
			enableDisableAllButtons(false);
			perFormCommsTask(dataObj);
		}

		private void btnBackupCalibration_Click(object sender, EventArgs e)
		{
			if (commPort == null)
			{
				MessageBox.Show("No com port. Close and reopen the OpenGD77 window to select a com port");
				return;
			}

			OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.BACKUP_CALIBRATION);
			enableDisableAllButtons(false);
			perFormCommsTask(dataObj);
		}

		bool arrayCompare(byte[] buf1, byte[] buf2)
		{
			int len = Math.Min(buf1.Length, buf2.Length);

			for (int i=0; i<len; i++)
			{
				if (buf1[i]!=buf2[i])
				{
					return false;
				}
			}
			return true;
		}

		private void btnRestoreEEPROM_Click(object sender, EventArgs e)
		{
			if (commPort == null)
			{
				MessageBox.Show("No com port. Close and reopen the OpenGD77 window to select a com port");
				return;
			}
			if (DialogResult.Yes == MessageBox.Show("Are you sure you want to restore the EEPROM from a previously saved file?", "Warning", MessageBoxButtons.YesNo))
			{
				if (DialogResult.OK == _openFileDialog.ShowDialog())
				{
					OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.RESTORE_EEPROM);
					dataObj.dataBuff = File.ReadAllBytes(_openFileDialog.FileName);
					if (dataObj.dataBuff.Length == (64 * 1024))
					{
						enableDisableAllButtons(false);
						perFormCommsTask(dataObj);
					}
					else
					{
						MessageBox.Show("The file is not the correct size.", "Error");
					}
				}
			}
		}

		private void btnRestoreCalibration_Click(object sender, EventArgs e)
		{
			if (commPort == null)
			{
				MessageBox.Show("No com port. Close and reopen the OpenGD77 window to select a com port");
				return;
			}
			if (DialogResult.Yes == MessageBox.Show("Are you sure you want to restore the Calibartion from a previously saved file?", "Warning", MessageBoxButtons.YesNo))
			{
				_openFileDialog.Filter = "Calibration files (*.bin)|*.bin";
				if (DialogResult.OK == _openFileDialog.ShowDialog())
				{
					OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.RESTORE_CALIBRATION);
					dataObj.dataBuff = File.ReadAllBytes(_openFileDialog.FileName);

					if (dataObj.dataBuff.Length == CalibrationForm.CALIBRATION_DATA_SIZE)
					{
						for (int p = 0; p < 8; p++)
						{
							if (dataObj.dataBuff[p] != CalibrationForm.CALIBRATION_HEADER[p])
							{
								MessageBox.Show("Invalid Calibration data.");
								return;
							}
						}
						enableDisableAllButtons(false);
						perFormCommsTask(dataObj);
					}
					else
					{
						MessageBox.Show("The file is not the correct size.", "Error");
					}
				}
			}
		}

		private void btnRestoreFlash_Click(object sender, EventArgs e)
		{
			if (commPort == null)
			{
				MessageBox.Show("No com port. Close and reopen the OpenGD77 window to select a com port");
				return;
			}
			if (DialogResult.Yes == MessageBox.Show("Are you sure you want to restore the Flash memory from a previously saved file?", "Warning", MessageBoxButtons.YesNo))
			{
				if (DialogResult.OK == _openFileDialog.ShowDialog())
				{
					OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.RESTORE_FLASH);
					dataObj.dataBuff = File.ReadAllBytes(_openFileDialog.FileName);
					
					if (dataObj.dataBuff.Length == (1024 * 1024))
					{
						byte[] signature = { 0x54, 0x59, 0x54, 0x3A, 0x4D, 0x44, 0x2D, 0x37, 0x36, 0x30 };
						enableDisableAllButtons(false);
						perFormCommsTask(dataObj);
					}
					else
					{
						MessageBox.Show("The file is not the correct size.", "Error");
					}
				}
			}
		}

		private void enableDisableAllButtons(bool show)
		{
			btnBackupEEPROM.Enabled = show;
			btnBackupFlash.Enabled = show;
			btnRestoreEEPROM.Enabled = show;
			btnRestoreFlash.Enabled = show;
			btnReadCodeplug.Enabled = show;
			btnWriteCodeplug.Enabled = show;
		}

		private void OpenGD77Form_Load(object sender, EventArgs e)
		{
			switch (_initialAction)
			{
				case CommsAction.READ_CODEPLUG:
					readCodeplug();
					break;
				case CommsAction.WRITE_CODEPLUG:
					writeCodeplug();
					break;
				default:
					break;
			}
		}

		private SerialPort commPort
		{
			get
			{
				if (_port == null)
				{
					try
					{
						String gd77CommPort = null;

						gd77CommPort = SetupDiWrap.ComPortNameFromFriendlyNamePrefix("OpenGD77");
						if (gd77CommPort == null)
						{
							CommPortSelector cps = new CommPortSelector();
							if (DialogResult.OK == cps.ShowDialog())
							{
								gd77CommPort = SetupDiWrap.ComPortNameFromFriendlyNamePrefix(cps.SelectedPort);
								IniFileUtils.WriteProfileString("Setup", "LastCommPort", gd77CommPort);// assume they selected something useful !
							}
							else
							{
								//this.Close();
								return null;
							}
						}

						if (gd77CommPort == null)
						{
							MessageBox.Show("Please connect the GD-77 running OpenGD77 firmware, and try again.", "OpenGD77 radio not detected.");
						}
						else
						{
							_port = new SerialPort(gd77CommPort, 115200, Parity.None, 8, StopBits.One);
							commPort.ReadTimeout = 1000;
						}

					}
					catch (Exception)
					{
						_port = null;
						MessageBox.Show("Failed to open comm port", "Error");
						IniFileUtils.WriteProfileString("Setup", "LastCommPort", "");// clear any port they may have saved
						return null;
					}
					return _port;
				}
				else
				{
					return _port;
				}

			}

			set
			{
				_port = value;
			}
		}


		private void OpenGD77Form_FormClosed(object sender, FormClosedEventArgs e)
		{
			//MidiPlayer.CloseMidi();
			if (_port != null)
			{
				try
				{
					commPort = null;
				}
				catch (Exception)
				{
					// don't care if we get an error while closing the port, we can handle the error if they can't open it the next time they want to upload or download
				}
			}
		}

		private void readCodeplug()
		{			
			if (commPort == null)
			{
				MessageBox.Show("No com port. Close and reopen the OpenGD77 window to select a com port");
				return;
			}

			OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.READ_CODEPLUG);
			dataObj.dataBuff = MainForm.DataToByte();// overwrite the existing data, so that we can use the header etc, which the CPS checks for when we later call Byte2Data
			enableDisableAllButtons(false);
			perFormCommsTask(dataObj);
		}

		private void btnReadCodeplug_Click(object sender, EventArgs e)
		{
			readCodeplug();
		}

		private void writeCodeplug()
		{
			if (commPort == null)
			{
				MessageBox.Show("No com port. Close and reopen the OpenGD77 window to select a com port");
				return;
			}

			melodyToBytes(false);

			OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.WRITE_CODEPLUG);
			enableDisableAllButtons(false);
			perFormCommsTask(dataObj);
		}

		private void btnWriteCodeplug_Click(object sender, EventArgs e)
		{
			writeCodeplug();
		}

		private void btnBackupMCUROM_Click(object sender, EventArgs e)
		{
			if (commPort == null)
			{
				MessageBox.Show("No com port. Close and reopen the OpenGD77 window to select a com port");
				return;
			}
			OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.BACKUP_MCU_ROM);
			enableDisableAllButtons(false);
			perFormCommsTask(dataObj);
		}

		private void btnDownloadScreenGrab_Click(object sender, EventArgs e)
		{
			if (commPort == null)
			{
				MessageBox.Show("No com port. Close and reopen the OpenGD77 window to select a com port");
				return;
			}
			OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.DOWLOAD_SCREENGRAB);
			enableDisableAllButtons(false);
			perFormCommsTask(dataObj);
		}

		private Bitmap ResizeImage(Image image, int width, int height)
		{
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.AssumeLinear;
				graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
				graphics.SmoothingMode = SmoothingMode.None;
				graphics.PixelOffsetMode = PixelOffsetMode.Half;

				graphics.DrawImage(image, 0, 0, width, height);    
			}

			return destImage;
		}

		private void btnOpenFile_Click(object sender, EventArgs e)
		{
			byte[] bytes = new byte[1024];

			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Files (*.png)|*.png";
			if (DialogResult.OK == ofd.ShowDialog())
			{
				BootItemForm.data.BootScreenType = 0;// Image Type
				Bitmap fileImage = new Bitmap(ofd.FileName);
				Bitmap bw1 = ResizeImage(fileImage, 128, 64);
				
				Bitmap bw = bw1.Clone(new Rectangle(0, 0, bw1.Width, bw1.Height), PixelFormat.Format1bppIndexed);
				pictureBox1.Image = bw;

				int rowOffset, lineInRow;
				for (int y = 0; y < 64; y++)
				{
					rowOffset = (y / 8)*128;
					lineInRow = y % 8;
					for (int x = 0; x < 128; x++)
					{
						Color pixel = bw.GetPixel(x,y);

						if (pixel.R == 0)
						{
							bytes[rowOffset + x] += (byte)(1 << (lineInRow));
						}
					}
				}

				int address=0;
				bool foundAddress = false;

				if (findCustomDataBlock(CustomDataType.IMAGE_TYPE, ref address))
				{
					foundAddress = true;
				}
				else
				{
					if (findCustomDataBlock(CustomDataType.UNINITIALISED_TYPE, ref address))
					{
						foundAddress = true;
					}
				}

				if (foundAddress)
				{
					CustomData[address] = (byte)CustomDataType.IMAGE_TYPE;// image;
					CustomData[address + 1] = 0x00;
					CustomData[address + 2] = 0x00;
					CustomData[address + 3] = 0x00;

					// image is 1k long
					CustomData[address + 4] = 0x00;
					CustomData[address + 5] = 0x04;
					CustomData[address + 6] = 0x00;
					CustomData[address + 7] = 0x00;
					Array.Copy(bytes, 0, CustomData, address + 8, 1024);
				}
			}
		}

		private void melodyToBytes(bool playTune)
		{
			byte[] arr = new byte[512 + 8];
			arr[0] = (byte)CustomDataType.MELODY_TYPE;// image;
			arr[1] = 0x00;
			arr[2] = 0x00;
			arr[3] = 0x00;

			// Max length of melody is 256 notes and durations = 512 bytes
			arr[4] = 0x00;
			arr[5] = 0x02;
			arr[6] = 0x00;
			arr[7] = 0x00;

			string melodyStr = txtBootTune.Text;

			melodyStr = melodyStr.Replace(" ", "");
			melodyStr = melodyStr.Replace("\r\n", "");
			string[] melodyArr = melodyStr.Split(',');

			if (melodyArr.Length % 2 == 0)
			{
				int pos = 0;
				try
				{
					while (pos < (melodyArr.Length - 1))
					{
						int n = (int)Math.Round(98 * Math.Pow(2, ((float)float.Parse(melodyArr[pos]) / 12.0f)));
						if (n == 98)
						{
							n = 37;
						}
						int len = int.Parse(melodyArr[pos + 1]);
						if (playTune)
						{
							Console.Beep(n, len * 35);
						}
						arr[pos + 8] = (byte)int.Parse(melodyArr[pos]);
						arr[pos + 8 + 1] = (byte)len;
						pos += 2;
					}

					// terminate
					arr[pos + 8] = 0;
					arr[pos + 8 + 1] = 0;

					int address = 0;
					if (findCustomDataBlock(CustomDataType.MELODY_TYPE, ref address))
					{
						Array.Copy(arr, 0, CustomData, address, arr.Length);
					}
					else
					{
						if (findCustomDataBlock(CustomDataType.UNINITIALISED_TYPE, ref address))
						{
							Array.Copy(arr, 0, CustomData, address, arr.Length);
						}
					}
				}
				catch (Exception)
				{

				}
			}
		}


		private void btnPlayTune_Click(object sender, EventArgs e)
		{
			melodyToBytes(true);
		}
	}
}
