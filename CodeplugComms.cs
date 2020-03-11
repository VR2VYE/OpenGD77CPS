using DMR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UsbLibrary;

internal class CodeplugComms
{
	public delegate void Delegate1(object sender, FirmwareUpdateProgressEventArgs e);

	private const int HEAD_LEN = 4;
	private const int MAX_COMM_LEN = 32;
	private const byte CMD_WRITE = 87;
	private const byte CMD_READ = 82;
	private const byte CMD_CMD = 67;
	private const byte CMD_BASE = 66;
	private const int MaxReadTimeout = 5000;
	private const int MaxWriteTimeout = 1000;
	private const int MaxBuf = 160;
	private const float IndexListPercent = 5f;
	private const int HID_VID = 0x15A2;
	private const int HID_PID = 0x0073;
	private static readonly byte[] CMD_ENDR =   Encoding.ASCII.GetBytes("ENDR");
	private static readonly byte[] CMD_ENDW =   Encoding.ASCII.GetBytes("ENDW");
	private static readonly byte[] CMD_ACK=     new byte[1] {65};
    private static readonly byte[] CMD_PRG =    new byte[7] { 2, (byte)'P', (byte)'R', (byte)'O', (byte)'G', (byte)'R', (byte)'A' };// 80,82,79,71,82,65}
	private static readonly byte[] CMD_PRG2 =   new byte[2] {77,2};

	public int[] START_ADDR;
	public int[] END_ADDR;

	private Thread thread;

	private Delegate1 OnFirmwareUpdateProgress;

    bool _CancelComm;
	[CompilerGenerated]
	public bool method_0()
	{
		return this._CancelComm;
	}

	[CompilerGenerated]
	public void SetCancelComm(bool bool_0)
	{
		this._CancelComm = bool_0;
	}

	public enum CommunicationType { codeplugRead, codeplugWrite, DMRIDRead, DMRIDWrite, calibrationRead, calibrationWrite,dataRead,dataWrite };

	public static CodeplugComms.CommunicationType CommunicationMode
	{
		get;
		set;
	}

	public bool isThreadAlive()
	{
		if (this.thread != null)
		{
			return this.thread.IsAlive;
		}
		return false;
	}

	public void JoinThreadIfAlive()
	{
		if (this.isThreadAlive())
		{
			this.thread.Join();
		}
	}

	public void startCodeplugReadOrWriteInNewThread()
	{
		this.thread = null;
		switch (CommunicationMode)
		{
			case CommunicationType.codeplugRead:
				this.thread = new Thread(this.readCodeplug);
				break;
			case CommunicationType.codeplugWrite:
				this.thread = new Thread(this.writeCodeplug);
				break;
			case CommunicationType.DMRIDRead:
				startAddress = 0x30000;
				transferLength = 0x20000;
				this.thread = new Thread(this.readData);
				break;
			case CommunicationType.DMRIDWrite:
				startAddress = 0x30000;
				transferLength = 0x20000;
				this.thread = new Thread(writeData);
				break;
			case CommunicationType.calibrationRead:
				startAddress = DMR.CalibrationForm.CALIBRATION_MEMORY_LOCATION_OFFICIAL_USB_PROTOCOL;//0x80000;
				transferLength = 0x1000;
				this.thread = new Thread(readData);
				break;
			case CommunicationType.calibrationWrite:
				startAddress = DMR.CalibrationForm.CALIBRATION_MEMORY_LOCATION_OFFICIAL_USB_PROTOCOL;//0x80000;
				transferLength = 0x1000;
				this.thread = new Thread(writeData);
				break;
			case CommunicationType.dataWrite:
				this.thread = new Thread(writeData);
				break;
			case CommunicationType.dataRead:
				this.thread = new Thread(readData);
				break;

		}

		if (this.thread != null)
		{
			this.thread.Start();
		}
	}

	public static string ByteArrayToString(byte[] ba)
	{
		string hex = BitConverter.ToString(ba);
		return hex.Replace("-", "");
	}

	// Test function added by Roger Clark to read all 1Mb from the external Flash chip on the GD-77.
	// It outputs the data dummp to the file c:\\gd-77_datadump.bin
	
	// However depending on the mode in which the GD-77 is booted,  addresses 0x000000 - 0x01FFFF  (for codeplug mode)
	// or addresses 0x030000 - 0x03FFFF for DMR-ID mode are returned with anything other than 0x00 in them

	// Its strange that address 0x020000 - 0x02FFFF does not seem to be accessible
	// Note. Valid transfer lengths seem to be 8,16 or 32 bytes.  64 bytes does not work, as it just returns 0x00 in addresses after 32 bytes

	public static int transferLength;
	public static int startAddress;
	public void readData()
	{
		byte[] usbBuf = new byte[160];// buffer for individual transfers
		int blockLength = 0;
		int addr32 = 0;
		int addr16 = 0;
		int pageAddr = 0;
		SpecifiedDevice specifiedDevice = null;
		try
		{
			specifiedDevice = SpecifiedDevice.FindSpecifiedDevice(HID_VID, HID_PID);//0x152A HID_PID
			if (specifiedDevice == null)
			{
				if (this.OnFirmwareUpdateProgress != null)
				{
					this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "Device not found", true, true));
				}
			}
			else
			{
				while (true)
				{
					Array.Clear(usbBuf, 0, usbBuf.Length);
					specifiedDevice.SendData(CodeplugComms.CMD_PRG);// Send PROGRA command to initiate comms
					specifiedDevice.ReceiveData(usbBuf);// Wait for response
					if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
					{
						break;// Exit if not ack
					}
					specifiedDevice.SendData(CodeplugComms.CMD_PRG2);// Send second half of comms init sequence
					Array.Clear(usbBuf, 0, usbBuf.Length);
					specifiedDevice.ReceiveData(usbBuf);// GD77 send back device information
					byte[] array3 = new byte[8];
					Buffer.BlockCopy(usbBuf, 0, array3, 0, 8);// Extract the first 8 bytes from the response
					// REMOVED CURRENT MODEL CHECK !!!  if (array3.smethod_4(Settings.CUR_MODEL))
					{
						// its the correct model number
						specifiedDevice.SendData(CodeplugComms.CMD_ACK);// send ACK
						Array.Clear(usbBuf, 0, usbBuf.Length);
						specifiedDevice.ReceiveData(usbBuf);// Wait for response (of ACK)


						if (usbBuf[0] == CodeplugComms.CMD_ACK[0])
						{
							// --------------- removed the password checking
							blockLength = 32;// Max transfer length is 32 bytes
							int currentPage = 0;
							int bankSize = 0x10000;//64k
							int numBlocks = transferLength / blockLength;
							int startBlock = startAddress / blockLength;
							for (int block = startBlock; block < (startBlock + numBlocks); block++)
							{
								if (currentPage != (block * blockLength) / bankSize)
								{
									currentPage = (block * blockLength) / bankSize;
									addr32 = blockLength * block;
									byte[] array4 = new byte[8] { (byte)'C', (byte)'W', (byte)'B', 4, 0, 0, 0, 0 };
									pageAddr = addr32 >> 16 << 16;
									array4[4] = (byte)(pageAddr >> 24);
									array4[5] = (byte)(pageAddr >> 16);
									array4[6] = (byte)(pageAddr >> 8);
									array4[7] = (byte)pageAddr;
									Console.WriteLine(SpecifiedDevice.ByteArrayToString(array4));
									Array.Clear(usbBuf, 0, usbBuf.Length);
									specifiedDevice.SendData(array4, 0, array4.Length);
									specifiedDevice.ReceiveData(usbBuf);
									if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
									{
										goto end_IL_02a2;
									}
								}

								addr16 = (block * blockLength) & 0xffff;
								// Send request for dcata
								byte[] data2 = new byte[4] { 82, (byte)(addr16 >> 8), (byte)addr16, (byte)blockLength };

								Array.Clear(usbBuf, 0, usbBuf.Length);
								specifiedDevice.SendData(data2, 0, 4);
								if (!specifiedDevice.ReceiveData(usbBuf))
								{
									goto end_IL_02a2;
								}

								Buffer.BlockCopy(usbBuf, 4, MainForm.CommsBuffer, (block * blockLength), blockLength);// Extract the first 8 bytes from the response

								if (this.OnFirmwareUpdateProgress != null)
								{
									this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs((float)(block + 1 - startBlock) * 100 / (float)numBlocks, "", false, false));
								}
							}
							// SEND END OF READ
							specifiedDevice.SendData(CodeplugComms.CMD_ENDR);
							specifiedDevice.ReceiveData(usbBuf);
						}
						break;
					}
					return;
				end_IL_02a2:
					break;
				}

			}
		}
		catch (TimeoutException ex)
		{
			Console.WriteLine(ex.Message);
			if (this.OnFirmwareUpdateProgress != null)
			{
				this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "Comms error", false, false));
			}
		}
		finally
		{
			if (specifiedDevice != null)
			{
				specifiedDevice.Dispose();
			}
		}

		if (this.OnFirmwareUpdateProgress != null)
		{
			this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(100f, "", false, true));
		}
	}


	public void writeData()//(int startAddress, int transferLength)
	{
		//byte[] MainForm.eeprom;// no need to allocate this as its read in from file. = new byte[128 * 1024];// whole of the codeplug
		byte[] usbBuf = new byte[160];// buffer for individual usb transfers

		int blockLength = 0;
		int addr32 = 0;
		int addr16 = 0;
		int pageAddr = 0;
		SpecifiedDevice specifiedDevice = null;
		try
		{
			specifiedDevice = SpecifiedDevice.FindSpecifiedDevice(HID_VID, HID_PID);//0x152A HID_PID
			if (specifiedDevice == null)
			{
				if (this.OnFirmwareUpdateProgress != null)
				{
					this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "Device not found", true, true));
				}
			}
			else
			{
				while (true)
				{
					Array.Clear(usbBuf, 0, usbBuf.Length);
					specifiedDevice.SendData(CodeplugComms.CMD_PRG);// Send PROGRA command to initiate comms
					//Console.WriteLine("Send PRG1");
					specifiedDevice.ReceiveData(usbBuf);// Wait for response
					if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
					{
						break;// Exit if not ack
					}
					specifiedDevice.SendData(CodeplugComms.CMD_PRG2);// Send second half of comms init sequence
					//Console.WriteLine("Send PRG2");
					Array.Clear(usbBuf, 0, usbBuf.Length);
					specifiedDevice.ReceiveData(usbBuf);// GD77 send back device information
					byte[] array3 = new byte[8];
					Buffer.BlockCopy(usbBuf, 0, array3, 0, 8);// Extract the first 8 bytes from the response
					{
						// its the correct model number
						specifiedDevice.SendData(CodeplugComms.CMD_ACK);// send ACK
						Array.Clear(usbBuf, 0, usbBuf.Length);
						specifiedDevice.ReceiveData(usbBuf);// Wait for response (of ACK)

						if (usbBuf[0] == CodeplugComms.CMD_ACK[0])
						{
							//Console.WriteLine("Got ACK");
							// --------------- removed the password checking
							blockLength = 32;// Max transfer length is 32 bytes
							int currentPage = 0;
							int bankSize = 0x10000;// 64k
							int numBlocks = transferLength / blockLength;
							int startBlock = startAddress / blockLength;
							for (int block = startBlock; block < (startBlock + numBlocks); block++)
							{
								//Console.Write("Processing block " + block + " end block " + (startBlock + numBlocks));
								if (currentPage != (block * blockLength) / bankSize)
								{
									currentPage = (block * blockLength) / bankSize;
									addr32 = blockLength * block;
									byte[] array4 = new byte[8] { (byte)'C', (byte)'W', (byte)'B', 4, 0, 0, 0, 0 };
									pageAddr = addr32 >> 16 << 16;
									array4[4] = (byte)(pageAddr >> 24);
									array4[5] = (byte)(pageAddr >> 16);
									array4[6] = (byte)(pageAddr >> 8);
									array4[7] = (byte)pageAddr;
									//Console.WriteLine("Send address changed to 0x" + pageAddr.ToString("X"));
									Array.Clear(usbBuf, 0, usbBuf.Length);
									specifiedDevice.SendData(array4, 0, array4.Length);
									specifiedDevice.ReceiveData(usbBuf);
									if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
									{
										goto end_IL_02a2;
									}
								}

								addr16 = (block * blockLength) & 0xffff;
								// Send request for dcata
								byte[] data2 = new byte[4 + 32] { 87, (byte)(addr16 >> 8), (byte)addr16, (byte)blockLength,
								0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

								Array.Clear(usbBuf, 0, usbBuf.Length);
								Buffer.BlockCopy(MainForm.CommsBuffer, (block * blockLength), data2, 4, blockLength);
								specifiedDevice.SendData(data2, 0, 4 + 32);


								specifiedDevice.ReceiveData(usbBuf);
								if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
								{
									goto end_IL_02a2;
								}
								if (this.OnFirmwareUpdateProgress != null)
								{
									this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs((float)(block + 1 - startBlock) * 100 / (float)numBlocks, "", false, false));
								}
							}
							// SEND END OF WRITE
							specifiedDevice.SendData(CodeplugComms.CMD_ENDW);
							specifiedDevice.ReceiveData(usbBuf);
						}
						break;
					}
					return;
				end_IL_02a2:
					// SEND END OF WRITE
					specifiedDevice.SendData(CodeplugComms.CMD_ENDW);
					specifiedDevice.ReceiveData(usbBuf);
					break;
				}
			}
		}
		catch (TimeoutException ex)
		{
			Console.WriteLine(ex.Message);
			if (this.OnFirmwareUpdateProgress != null)
			{
				this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "Error", false, false));
			}
		}
		finally
		{
			if (specifiedDevice != null)
			{
				specifiedDevice.Dispose();
			}
		}

		if (this.OnFirmwareUpdateProgress != null)
		{
			this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(100f, "", false, true));
		}
	}





	public void readCodeplug()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		byte[] array = new byte[Settings.EEROM_SPACE];
		byte[] array2 = new byte[160];
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		bool flag = false;
		float num11 = 0f;
		int num12 = 0;

//		dumpFlash();// testing only...
//		return;

		SpecifiedDevice specifiedDevice = null;
		try
		{
			specifiedDevice = SpecifiedDevice.FindSpecifiedDevice(HID_VID, HID_PID);//0x152A HID_PID
			if (specifiedDevice == null)
			{
				if (this.OnFirmwareUpdateProgress != null)
				{
					this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.SZ_DEVICE_NOT_FOUND, true, true));
				}
			}
			else
			{
				while (true)
				{
					Array.Clear(array2, 0, array2.Length);
					specifiedDevice.SendData(CodeplugComms.CMD_PRG);
					specifiedDevice.ReceiveData(array2);
					if (array2[0] != CodeplugComms.CMD_ACK[0])
					{
						break;
					}
					specifiedDevice.SendData(CodeplugComms.CMD_PRG2);
					Array.Clear(array2, 0, array2.Length);
					specifiedDevice.ReceiveData(array2);
					byte[] array3 = new byte[8];
					Buffer.BlockCopy(array2, 0, array3, 0, 8);
					if (array3.smethod_4(Settings.CUR_MODEL))
					{
						specifiedDevice.SendData(CodeplugComms.CMD_ACK);
						Array.Clear(array2, 0, array2.Length);
						specifiedDevice.ReceiveData(array2);
						if (array2[0] == CodeplugComms.CMD_ACK[0])
						{
							if (!flag && Settings.CUR_PWD != "DT8168")
							{
								i = Settings.ADDR_PWD;
								num5 = 8;
								byte[] data = new byte[4]
								{
									82,
									(byte)(i >> 8),
									(byte)i,
									8
								};
								Array.Clear(array2, 0, array2.Length);
								specifiedDevice.SendData(data, 0, 4);
								specifiedDevice.ReceiveData(array2);
								string text = "";
								for (num = 0; num < 8; num++)
								{
									char c = Convert.ToChar(array2[num + 4]);
									if ("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz\b".IndexOf(c) < 0)
									{
										break;
									}
									text += c;
								}
								if (string.IsNullOrEmpty(text))
								{
									Array.Clear(array2, 0, array2.Length);
									specifiedDevice.SendData(CodeplugComms.CMD_ENDR);
									specifiedDevice.ReceiveData(array2);
									if (array2[0] != CodeplugComms.CMD_ACK[0])
									{
										break;
									}
									flag = true;
								}
								else
								{
									if (text != Settings.CUR_PWD)
									{
										Array.Clear(array2, 0, array2.Length);
										specifiedDevice.SendData(CodeplugComms.CMD_ENDR);
										specifiedDevice.ReceiveData(array2);
										if (array2[0] != CodeplugComms.CMD_ACK[0])
										{
											break;
										}
										Settings.CUR_PWD = "";
										PasswordForm passwordForm = new PasswordForm();
										if (passwordForm.ShowDialog() == DialogResult.OK)
										{
											num12++;
											if (num12 < 3)
											{
												continue;
											}
											this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "Password error more than three times, quit communication！", true, true));
										}
										else
										{
											this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "", true, true));
										}
										return;
									}
									Array.Clear(array2, 0, array2.Length);
									specifiedDevice.SendData(CodeplugComms.CMD_ENDR);
									specifiedDevice.ReceiveData(array2);
									if (array2[0] != CodeplugComms.CMD_ACK[0])
									{
										break;
									}
									flag = true;
								}
								continue;
							}
							List<int> list3 = new List<int>();
							List<int> list4 = new List<int>();
							list3.Add(Settings.ADDR_CHANNEL);
							list4.Add(Settings.ADDR_CHANNEL + 16);
							for (num2 = 1; num2 < 8; num2++)
							{
								num8 = Settings.ADDR_EX_CH + (num2 - 1) * ChannelForm.SPACE_CH_GROUP;
								list3.Add(num8);
								list4.Add(num8 + 16);
							}
							num8 = Settings.ADDR_EX_ZONE_LIST;
							list3.Add(num8);
							list4.Add(num8 + 32);
							num3 = 0;
							num4 = 0;
							for (num = 0; num < list3.Count; num++)
							{
								num9 = list3[num];
								num10 = list4[num];
								for (i = num9; i < num10; i += num5)
								{
									num6 = i % 32;
									num5 = ((i + 32 <= num10) ? (32 - num6) : (num10 - i));
									num4++;
								}
							}
							num8 = 0;
							num = 0;
							while (true)
							{
								if (num < list3.Count)
								{
									num9 = list3[num];
									num10 = list4[num];
									i = num9;
									while (i < num10)
									{
										if (!this.method_0())
										{
											if (num8 >> 16 != i >> 16)
											{
												byte[] array4 = new byte[8]	{(byte)'C',(byte)'W',(byte)'B',4,0,0,0,0};
												num8 = i >> 16 << 16;
												array4[4] = (byte)(num8 >> 24);
												array4[5] = (byte)(num8 >> 16);
												array4[6] = (byte)(num8 >> 8);
												array4[7] = (byte)num8;
												Array.Clear(array2, 0, array2.Length);
												specifiedDevice.SendData(array4, 0, array4.Length);
												specifiedDevice.ReceiveData(array2);
												if (array2[0] != CodeplugComms.CMD_ACK[0])
												{
													goto end_IL_02a2;
												}
											}
											num6 = i % 32;
											num5 = ((i + 32 <= num10) ? (32 - num6) : (num10 - i));
											num7 = i - num8;
											byte[] data2 = new byte[4]
											{
												82,
												(byte)(num7 >> 8),
												(byte)num7,
												(byte)num5
											};
											Array.Clear(array2, 0, array2.Length);
											specifiedDevice.SendData(data2, 0, 4);
											if (!specifiedDevice.ReceiveData(array2))
											{
												goto end_IL_02a2;
											}
											Array.Copy(array2, 4, array, i, num5);
											if (this.OnFirmwareUpdateProgress != null)
											{
												this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs((float)(++num3) * 5f / (float)num4, i.ToString(), false, false));
											}
											i += num5;
											continue;
										}
										specifiedDevice.SendData(CodeplugComms.CMD_ENDR);
										specifiedDevice.ReceiveData(array2);
										return;
									}
									num++;
									continue;
								}
								byte[] array5 = new byte[16];
								Array.Copy(array, Settings.ADDR_CHANNEL, array5, 0, array5.Length);
								BitArray bitArray = new BitArray(array5);
								list.Add(Settings.ADDR_CHANNEL);
								list2.Add(Settings.ADDR_CHANNEL + 16);
								for (num = 0; num < 128; num++)
								{
									if (bitArray[num])
									{
										num9 = Settings.ADDR_CHANNEL + 16 + num * ChannelForm.SPACE_CH;
										num10 = num9 + ChannelForm.SPACE_CH;
										list.Add(num9);
										list2.Add(num10);
									}
								}
								for (num2 = 1; num2 < 8; num2++)
								{
									num8 = Settings.ADDR_EX_CH + (num2 - 1) * ChannelForm.SPACE_CH_GROUP;
									Array.Copy(array, num8, array5, 0, array5.Length);
									bitArray = new BitArray(array5);
									list.Add(num8);
									list2.Add(num8 + 16);
									for (num = 0; num < 128; num++)
									{
										if (bitArray[num])
										{
											num9 = num8 + 16 + num * ChannelForm.SPACE_CH;
											num10 = num9 + ChannelForm.SPACE_CH;
											list.Add(num9);
											list2.Add(num10);
										}
									}
								}
								byte[] array6 = new byte[32];
								num8 = Settings.ADDR_EX_ZONE_LIST;
								Array.Copy(array, num8, array6, 0, array6.Length);
								bitArray = new BitArray(array6);
								list.Add(num8);
								list2.Add(num8 + 32);
								for (num = 0; num < 250; num++)
								{
									num8 = Settings.ADDR_EX_ZONE_LIST + 32;
									if (bitArray[num])
									{
										num9 = num8 + num * ZoneForm.SPACE_ZONE;
										num10 = num9 + ZoneForm.SPACE_ZONE;
										list.Add(num9);
										list2.Add(num10);
									}
								}
								for (num = 0; num < this.START_ADDR.Length; num++)
								{
									list.Add(this.START_ADDR[num]);
									list2.Add(this.END_ADDR[num]);
								}
								num3 = 0;
								num4 = 0;
								for (num = 0; num < list.Count; num++)
								{
									num9 = list[num];
									num10 = list2[num];
									for (i = num9; i < num10; i += num5)
									{
										num6 = i % 32;
										num5 = ((i + 32 <= num10) ? (32 - num6) : (num10 - i));
										num4++;
									}
								}
								num8 = 0;
								num = 0;
								while (true)
								{
									if (num < list.Count)
									{
										num9 = list[num];
										num10 = list2[num];
										i = num9;
										while (i < num10)
										{
											if (!this.method_0())
											{
												if (num8 >> 16 != i >> 16)
												{
													byte[] array7 = new byte[8]
												{
													(byte)'C',(byte)'W',(byte)'B',4,0,0,0,0};// Change page in flash.
													num8 = i >> 16 << 16;
													array7[4] = (byte)(num8 >> 24);
													array7[5] = (byte)(num8 >> 16);
													array7[6] = (byte)(num8 >> 8);
													array7[7] = (byte)num8;
													Array.Clear(array2, 0, array2.Length);
													specifiedDevice.SendData(array7, 0, array7.Length);
													specifiedDevice.ReceiveData(array2);
													if (array2[0] == CodeplugComms.CMD_ACK[0])
													{
														goto IL_08b4;
													}
													goto IL_0a1f;
												}
												goto IL_08b4;
											}
											specifiedDevice.SendData(CodeplugComms.CMD_ENDR);
											specifiedDevice.ReceiveData(array2);
											return;
											IL_08b4:
											num6 = i % 32;
											num5 = ((i + 32 <= num10) ? (32 - num6) : (num10 - i));
											num7 = i - num8;
											byte[] array8 = new byte[4]
											{
												82,
												(byte)(num7 >> 8),
												(byte)num7,
												(byte)num5
											};
											Array.Clear(array2, 0, array2.Length);
											specifiedDevice.SendData(array8, 0, 4);
											if (specifiedDevice.ReceiveData(array2))
											{
												if (Settings.smethod_18(array8, array2, 4))
												{
													Array.Copy(array2, 4, array, i, num5);
													if (this.OnFirmwareUpdateProgress != null)
													{
														num11 = 5f + (float)(++num3) * 95f / (float)num4;
														this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(num11, i.ToString(), false, false));
													}
													i += num5;
													continue;
												}
												goto IL_0a51;
											}
											goto IL_0a38;
										}
										num++;
										continue;
									}
									Array.Clear(array2, 0, array2.Length);
									specifiedDevice.SendData(CodeplugComms.CMD_ENDR);
									specifiedDevice.ReceiveData(array2);
									if (array2[0] != CodeplugComms.CMD_ACK[0])
									{
										break;
									}
									MainForm.ByteToData(array);
									if (this.OnFirmwareUpdateProgress != null)
									{
										this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(100f, "", false, true));
									}
									return;
									IL_0a51:
									specifiedDevice.SendData(CodeplugComms.CMD_ENDR);
									specifiedDevice.ReceiveData(array2);
									break;
									IL_0a38:
									specifiedDevice.SendData(CodeplugComms.CMD_ENDR);
									specifiedDevice.ReceiveData(array2);
									break;
									IL_0a1f:
									specifiedDevice.SendData(CodeplugComms.CMD_ENDR);
									specifiedDevice.ReceiveData(array2);
									break;
								}
								break;
							}
						}
						break;
					}
					if (this.OnFirmwareUpdateProgress != null)
					{
						this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.SZ_MODEL_NOT_MATCH, true, true));
					}
					return;
					continue;// Roger Clark. There is a known compiler warning here. This may be an artifact caused by the decompiler
					end_IL_02a2:
					break;
				}
				if (this.OnFirmwareUpdateProgress != null)
				{
					this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.SZ_COMM_ERROR, true, true));
				}
			}
		}
		catch (TimeoutException ex)
		{
			Console.WriteLine(ex.Message);
			if (this.OnFirmwareUpdateProgress != null)
			{
				this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.SZ_COMM_ERROR, false, false));
			}
		}
		finally
		{
			if (specifiedDevice != null)
			{
				specifiedDevice.Dispose();
			}
		}
	}

	public void writeCodeplug()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		byte[] array = new byte[160];
		byte[] array2 = MainForm.DataToByte();
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		bool flag = false;
		byte[] array3 = new byte[6];
		int year = DateTime.Now.Year;
		int month = DateTime.Now.Month;
		int day = DateTime.Now.Day;
		int hour = DateTime.Now.Hour;
		int minute = DateTime.Now.Minute;
		array3[0] = (byte)(year / 1000 << 4 | year / 100 % 10);
		array3[1] = (byte)(year % 100 / 10 << 4 | year % 10);
		array3[2] = (byte)(month / 10 << 4 | month % 10);
		array3[3] = (byte)(day / 10 << 4 | day % 10);
		array3[4] = (byte)(hour / 10 << 4 | hour % 10);
		array3[5] = (byte)(minute / 10 << 4 | minute % 10);
		Array.Copy(array3, 0, array2, Settings.ADDR_DEVICE_INFO + Settings.OFS_LAST_PRG_TIME, 6);
		SpecifiedDevice specifiedDevice = SpecifiedDevice.FindSpecifiedDevice(HID_VID, 0x0073);

		if (specifiedDevice == null)
		{
			if (this.OnFirmwareUpdateProgress != null)
			{
				this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.SZ_DEVICE_NOT_FOUND, true, true));
			}
		}
		else
		{
			try
			{
				while (true)
				{
					specifiedDevice.SendData(CodeplugComms.CMD_PRG);
					Array.Clear(array, 0, array.Length);
					specifiedDevice.ReceiveData(array);
					if (array[0] != CodeplugComms.CMD_ACK[0])
					{
						break;
					}
					specifiedDevice.SendData(CodeplugComms.CMD_PRG2);
					Array.Clear(array, 0, array.Length);
					specifiedDevice.ReceiveData(array);
					byte[] array4 = new byte[8];
					Buffer.BlockCopy(array, 0, array4, 0, 8);
					if (array4.smethod_4(Settings.CUR_MODEL))
					{
						specifiedDevice.SendData(CodeplugComms.CMD_ACK);
						Array.Clear(array, 0, array.Length);
						specifiedDevice.ReceiveData(array);
						if (array[0] == CodeplugComms.CMD_ACK[0])
						{
							if (!flag && Settings.CUR_PWD != "DT8168")
							{
								i = Settings.ADDR_PWD;
								num5 = 8;
								byte[] data = new byte[4]
								{
									82,
									(byte)(i >> 8),
									(byte)i,
									8
								};
								Array.Clear(array, 0, array.Length);
								specifiedDevice.SendData(data, 0, 4);
								specifiedDevice.ReceiveData(array);
								string text = "";
								for (num = 0; num < 8; num++)
								{
									char c = Convert.ToChar(array[num + 4]);
									if ("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz\b".IndexOf(c) < 0)
									{
										break;
									}
									text += c;
								}
								if (string.IsNullOrEmpty(text))
								{
									Array.Clear(array, 0, array.Length);
									specifiedDevice.SendData(CodeplugComms.CMD_ENDW);
									specifiedDevice.ReceiveData(array);
									if (array[0] != CodeplugComms.CMD_ACK[0])
									{
										break;
									}
									flag = true;
								}
								else
								{
									if (text != Settings.CUR_PWD)
									{
										Settings.CUR_PWD = "";
										PasswordForm passwordForm = new PasswordForm();
										if (passwordForm.ShowDialog() == DialogResult.OK)
										{
											Array.Clear(array, 0, array.Length);
											specifiedDevice.SendData(CodeplugComms.CMD_ENDW);
											specifiedDevice.ReceiveData(array);
											if (array[0] != CodeplugComms.CMD_ACK[0])
											{
												break;
											}
											flag = true;
											continue;
										}
										this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "", true, true));
										return;
									}
									Array.Clear(array, 0, array.Length);
									specifiedDevice.SendData(CodeplugComms.CMD_ENDW);
									specifiedDevice.ReceiveData(array);
									if (array[0] != CodeplugComms.CMD_ACK[0])
									{
										break;
									}
									flag = true;
								}
								continue;
							}
							byte[] array5 = new byte[16];
							Array.Copy(array2, Settings.ADDR_CHANNEL, array5, 0, array5.Length);
							BitArray bitArray = new BitArray(array5);
							list.Add(Settings.ADDR_CHANNEL);
							list2.Add(Settings.ADDR_CHANNEL + 16);
							for (num = 0; num < 128; num++)
							{
								if (bitArray[num])
								{
									num9 = Settings.ADDR_CHANNEL + 16 + num * ChannelForm.SPACE_CH;
									num10 = num9 + ChannelForm.SPACE_CH;
									list.Add(num9);
									list2.Add(num10);
								}
							}
							for (num2 = 1; num2 < 8; num2++)
							{
								num7 = Settings.ADDR_EX_CH + (num2 - 1) * ChannelForm.SPACE_CH_GROUP;
								Array.Copy(array2, num7, array5, 0, array5.Length);
								bitArray = new BitArray(array5);
								list.Add(num7);
								list2.Add(num7 + 16);
								for (num = 0; num < 128; num++)
								{
									if (bitArray[num])
									{
										num9 = num7 + 16 + num * ChannelForm.SPACE_CH;
										num10 = num9 + ChannelForm.SPACE_CH;
										list.Add(num9);
										list2.Add(num10);
									}
								}
							}
							byte[] array6 = new byte[32];
							num7 = Settings.ADDR_EX_ZONE_LIST;
							Array.Copy(array2, num7, array6, 0, array6.Length);
							bitArray = new BitArray(array6);
							list.Add(num7);
							list2.Add(num7 + 32);
							for (num = 0; num < 250; num++)
							{
								num7 = Settings.ADDR_EX_ZONE_LIST + 32;
								if (bitArray[num])
								{
									num9 = num7 + num * ZoneForm.SPACE_ZONE;
									num10 = num9 + ZoneForm.SPACE_ZONE;
									list.Add(num9);
									list2.Add(num10);
								}
							}
							for (num = 0; num < this.START_ADDR.Length; num++)
							{
								list.Add(this.START_ADDR[num]);
								list2.Add(this.END_ADDR[num]);
							}
							for (num = 0; num < list.Count; num++)
							{
								num9 = list[num];
								num10 = list2[num];
								for (i = num9; i < num10; i += num5)
								{
									num6 = i % 32;
									num5 = ((i + 32 <= num10) ? (32 - num6) : (num10 - i));
									num4++;
								}
							}
							num7 = 0;
							num = 0;
							while (true)
							{
								if (num < list.Count)
								{
									num9 = list[num];
									num10 = list2[num];
									i = num9;
									while (i < num10)
									{
										if (!this.method_0())
										{
											if (num7 >> 16 != i >> 16)
											{
												byte[] array7 = new byte[8]
												{
													67,
													87,
													66,
													4,
													0,
													0,
													0,
													0
												};
												num7 = i >> 16 << 16;
												array7[4] = (byte)(num7 >> 24);
												array7[5] = (byte)(num7 >> 16);
												array7[6] = (byte)(num7 >> 8);
												array7[7] = (byte)num7;
												//Console.WriteLine(string.Format("Page\t0x{0:X8}", num7));
												Array.Clear(array, 0, array.Length);
												specifiedDevice.SendData(array7, 0, array7.Length);
												specifiedDevice.ReceiveData(array);
												if (array[0] == CodeplugComms.CMD_ACK[0])
												{
													goto IL_06e2;
												}
												goto IL_083e;
											}
											goto IL_06e2;
										}
										specifiedDevice.SendData(CodeplugComms.CMD_ENDR);
										specifiedDevice.ReceiveData(array);
										return;
										IL_06e2:
										num6 = i % 32;
										num5 = ((i + 32 <= num10) ? (32 - num6) : (num10 - i));
										num8 = i - num7;
										byte[] array8 = new byte[num5 + 4];
										array8[0] = 87;
										array8[1] = (byte)(num8 >> 8);
										array8[2] = (byte)num8;
										array8[3] = (byte)num5;
										Array.Clear(array, 0, array.Length);
										Array.Copy(array2, i, array8, 4, num5);
										specifiedDevice.SendData(array8, 0, 4 + num5);
										specifiedDevice.ReceiveData(array);
										if (array[0] == CodeplugComms.CMD_ACK[0])
										{
											if (this.OnFirmwareUpdateProgress != null)
											{
												this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs((float)(++num3) * 100f / (float)num4, i.ToString(), false, false));
											}
											i += num5;
											continue;
										}
										goto IL_0857;
									}
									num++;
									continue;
								}
								Array.Clear(array, 0, array.Length);
								specifiedDevice.SendData(CodeplugComms.CMD_ENDW);
								specifiedDevice.ReceiveData(array);
								if (array[0] != CodeplugComms.CMD_ACK[0])
								{
									break;
								}
								if (this.OnFirmwareUpdateProgress != null)
								{
									this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(100f, "", false, true));
								}
								return;
								IL_0857:
								specifiedDevice.SendData(CodeplugComms.CMD_ENDW);
								specifiedDevice.ReceiveData(array);
								break;
								IL_083e:
								specifiedDevice.SendData(CodeplugComms.CMD_ENDW);
								specifiedDevice.ReceiveData(array);
								break;
							}
						}
						break;
					}
					if (this.OnFirmwareUpdateProgress != null)
					{
						this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.SZ_MODEL_NOT_MATCH, true, true));
					}
					return;
				}
				if (this.OnFirmwareUpdateProgress != null)
				{
					this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.SZ_COMM_ERROR, true, true));
				}
			}
			catch (TimeoutException ex)
			{
				Console.WriteLine(ex.Message);
				if (this.OnFirmwareUpdateProgress != null)
				{
					this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.SZ_COMM_ERROR, false, false));
				}
			}
			finally
			{
				if (specifiedDevice != null)
				{
					specifiedDevice.Dispose();
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public void SetProgressCallback(Delegate1 delegate1_0)
	{
		this.OnFirmwareUpdateProgress = (Delegate1)Delegate.Combine(this.OnFirmwareUpdateProgress, delegate1_0);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public void method_10(Delegate1 delegate1_0)
	{
		this.OnFirmwareUpdateProgress = (Delegate1)Delegate.Remove(this.OnFirmwareUpdateProgress, delegate1_0);
	}

	public CodeplugComms()
	{
		this.START_ADDR = new int[0];
		this.END_ADDR = new int[0];
	}
}
