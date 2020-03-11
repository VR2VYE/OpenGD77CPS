using DMR;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

internal class Class10
{
	public delegate void Delegate0(object sender, FirmwareUpdateProgressEventArgs e);

	private const int HEAD_LEN = 4;
	private const int MAX_COMM_LEN = 128;
	private const byte CMD_WRITE = 87;
	private const byte CMD_READ = 82;
	private const int MaxReadTimeout = 5000;
	private const int MaxWriteTimeout = 1000;
	private const int MaxBuf = 160;
	private static readonly byte[] CMD_ENDR;
	private static readonly byte[] CMD_ENDW;
	private static readonly byte[] CMD_ACK;
	private static readonly byte[] CMD_PRG;
	private static readonly byte[] CMD_PRG2;
	public int[] START_ADDR;
	public int[] END_ADDR;
	private Thread thread;

	private Delegate0 OnFirmwareUpdateProgress;

    bool _CancelComm;
    bool _IsRead;
	[CompilerGenerated]
	public bool getCancelCom()
	{
		return this._CancelComm;
	}

	[CompilerGenerated]
	public void setCancelCom(bool bool_0)
	{
		this._CancelComm = bool_0;
	}

	[CompilerGenerated]
	public bool getIsRead()
	{
		return this._IsRead;
	}

	[CompilerGenerated]
	public void setIsRead(bool bool_0)
	{
		this._IsRead = bool_0;
	}

	public bool isThreadAlive()
	{
		if (this.thread != null)
		{
			return this.thread.IsAlive;
		}
		return false;
	}

	public void method_5()
	{
		if (this.isThreadAlive())
		{
			this.thread.Join();
		}
	}

	public void method_6()
	{
		if (this.getIsRead())
		{
			this.thread = new Thread(this.method_7);
		}
		else
		{
			this.thread = new Thread(this.method_8);
		}
		this.thread.Start();
	}

	public void method_7()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		byte[] array = new byte[Settings.EEROM_SPACE];
		byte[] array2 = new byte[160];
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int i = 0;
		int num8 = 0;
		int num9 = 0;
		bool flag = false;
		Stopwatch stopwatch = Stopwatch.StartNew();
		SerialPort serialPort = new SerialPort(MainForm.CurCom, MainForm.CurCbr);
		serialPort.ReadTimeout = 5000;
		serialPort.WriteTimeout = 1000;
		try
		{
			serialPort.Open();
			if (serialPort.IsOpen)
			{
				goto end_IL_0066;
			}
			return;
			end_IL_0066:;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			if (this.OnFirmwareUpdateProgress != null)
			{
				this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.dicCommon["80000003"], false, false));
			}
			return;
		}
		try
		{
			for (num = 0; num < this.START_ADDR.Length; num++)
			{
				num8 = this.START_ADDR[num];
				num9 = this.END_ADDR[num];
				for (i = num8; i < num9; i += num5)
				{
					num6 = i % 128;
					num5 = ((i + 128 <= num9) ? (128 - num6) : (num9 - i));
					num3++;
				}
			}
			while (true)
			{
				Array.Clear(array2, 0, array2.Length);
				serialPort.DiscardInBuffer();
				serialPort.Write(Class10.CMD_PRG, 0, Class10.CMD_PRG.Length);
				stopwatch.Reset();
				while (true)
				{
					Array.Clear(array2, 0, array2.Length);
					num7 = serialPort.Read(array2, 0, 1);
					if (array2[0] != Class10.CMD_ACK[0])
					{
						if (stopwatch.ElapsedMilliseconds < 5000L)
						{
							continue;
						}
					}
					else
					{
						Array.Clear(array2, 0, array2.Length);
						serialPort.DiscardInBuffer();
						serialPort.Write(Class10.CMD_PRG2, 0, Class10.CMD_PRG2.Length);
						num4 = 16;
						for (num7 = serialPort.Read(array2, 0, 16); num7 < num4; num7 += serialPort.Read(array2, num7, num4 - num7))
						{
							Thread.Sleep(20);
						}
						Array.Clear(array2, 0, array2.Length);
						serialPort.DiscardInBuffer();
						serialPort.Write(Class10.CMD_ACK, 0, Class10.CMD_ACK.Length);
						num4 = 1;
						for (num7 = serialPort.Read(array2, 0, 1); num7 < num4; num7 += serialPort.Read(array2, num7, num4 - num7))
						{
							Thread.Sleep(20);
						}
						if (array2[0] == Class10.CMD_ACK[0])
						{
							if (!flag && Settings.CUR_PWD != "DT8168")
							{
								i = Settings.ADDR_PWD;
								num5 = 8;
								byte[] buffer = new byte[4]
								{
									82,
									(byte)(i >> 8),
									(byte)i,
									8
								};
								Array.Clear(array2, 0, array2.Length);
								serialPort.DiscardInBuffer();
								serialPort.Write(buffer, 0, 4);
								num4 = 12;
								for (num7 = serialPort.Read(array2, 0, 12); num7 < num4; num7 += serialPort.Read(array2, num7, num4 - num7))
								{
									Thread.Sleep(20);
								}
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
									serialPort.DiscardInBuffer();
									serialPort.Write(Class10.CMD_ENDR, 0, Class10.CMD_ENDR.Length);
									num4 = 1;
									for (num7 = serialPort.Read(array2, 0, 1); num7 < num4; num7 += serialPort.Read(array2, num7, num4 - num7))
									{
										Thread.Sleep(20);
									}
									if (array2[0] == Class10.CMD_ACK[0])
									{
										flag = true;
										break;
									}
								}
								else
								{
									if (text != Settings.CUR_PWD)
									{
										Settings.CUR_PWD = "";
										PasswordForm passwordForm = new PasswordForm();
										if (passwordForm.ShowDialog() == DialogResult.OK)
										{
											Array.Clear(array2, 0, array2.Length);
											serialPort.DiscardInBuffer();
											serialPort.Write(Class10.CMD_ENDR, 0, Class10.CMD_ENDR.Length);
											num4 = 1;
											for (num7 = serialPort.Read(array2, 0, 1); num7 < num4; num7 += serialPort.Read(array2, num7, num4 - num7))
											{
												Thread.Sleep(20);
											}
											if (array2[0] == Class10.CMD_ACK[0])
											{
												flag = true;
												break;
											}
											goto IL_0735;
										}
										this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "", true, true));
										serialPort.Close();
										return;
									}
									Array.Clear(array2, 0, array2.Length);
									serialPort.DiscardInBuffer();
									serialPort.Write(Class10.CMD_ENDR, 0, Class10.CMD_ENDR.Length);
									num4 = 1;
									for (num7 = serialPort.Read(array2, 0, 1); num7 < num4; num7 += serialPort.Read(array2, num7, num4 - num7))
									{
										Thread.Sleep(20);
									}
									if (array2[0] == Class10.CMD_ACK[0])
									{
										flag = true;
										break;
									}
								}
							}
							else
							{
								for (num = 0; num < this.START_ADDR.Length; num++)
								{
									num8 = this.START_ADDR[num];
									num9 = this.END_ADDR[num];
									i = num8;
									while (i < num9)
									{
										if (!this.getCancelCom())
										{
											num6 = i % 128;
											num5 = ((i + 128 <= num9) ? (128 - num6) : (num9 - i));
											byte[] buffer2 = new byte[4]
											{
												82,
												(byte)(i >> 8),
												(byte)i,
												(byte)num5
											};
											Array.Clear(array2, 0, array2.Length);
											serialPort.DiscardInBuffer();
											serialPort.Write(buffer2, 0, 4);
											num4 = num5 + 4;
											for (num7 = serialPort.Read(array2, 0, num4); num7 < num4; num7 += serialPort.Read(array2, num7, num4 - num7))
											{
												Thread.Sleep(20);
											}
											Array.Copy(array2, 4, array, i, num5);
											if (this.OnFirmwareUpdateProgress != null)
											{
												this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs((float)(++num2) * 100f / (float)num3, i.ToString(), false, false));
											}
											i += num5;
											continue;
										}
										Array.Clear(array2, 0, array2.Length);
										serialPort.DiscardInBuffer();
										serialPort.Write(Class10.CMD_ENDR, 0, Class10.CMD_ENDR.Length);
										num4 = 1;
										for (num7 = serialPort.Read(array2, 0, 1); num7 < num4; num7 += serialPort.Read(array2, num7, num4 - num7))
										{
											Thread.Sleep(20);
										}
										serialPort.Close();
										return;
									}
								}
								Array.Clear(array2, 0, array2.Length);
								serialPort.DiscardInBuffer();
								serialPort.Write(Class10.CMD_ENDR, 0, Class10.CMD_ENDR.Length);
								num4 = 1;
								for (num7 = serialPort.Read(array2, 0, 1); num7 < num4; num7 += serialPort.Read(array2, num7, num4 - num7))
								{
									Thread.Sleep(20);
								}
								if (array2[0] == Class10.CMD_ACK[0])
								{
									serialPort.Close();
									MainForm.ByteToData(array);
									if (this.OnFirmwareUpdateProgress != null)
									{
										this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(100f, Settings.dicCommon["80000101"], false, true));
									}
									return;
								}
							}
						}
					}
					goto IL_0735;
					IL_0735:
					if (this.OnFirmwareUpdateProgress != null)
					{
						this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.dicCommon["80000004"], true, true));
					}
					serialPort.Close();
					return;
				}
			}
		}
		catch (TimeoutException ex2)
		{
			Console.WriteLine(ex2.Message);
			if (this.OnFirmwareUpdateProgress != null)
			{
				this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.dicCommon["80000004"], false, false));
			}
			serialPort.Close();
		}
	}

	public void method_8()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		byte[] array = new byte[160];
		byte[] array2 = MainForm.DataToByte();
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int i = 0;
		int num8 = 0;
		int num9 = 0;
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
		Stopwatch stopwatch = Stopwatch.StartNew();
		SerialPort serialPort = new SerialPort(MainForm.CurCom, MainForm.CurCbr);
		try
		{
			for (num = 0; num < this.START_ADDR.Length; num++)
			{
				num8 = this.START_ADDR[num];
				num9 = this.END_ADDR[num];
				for (i = num8; i < num9; i += num5)
				{
					num6 = i % 128;
					num5 = ((i + 128 <= num9) ? (128 - num6) : (num9 - i));
					num3++;
				}
			}
			serialPort.ReadTimeout = 5000;
			serialPort.WriteTimeout = 1000;
			try
			{
				serialPort.Open();
				if (serialPort.IsOpen)
				{
					goto end_IL_01aa;
				}
				return;
				end_IL_01aa:;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				if (this.OnFirmwareUpdateProgress != null)
				{
					this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.dicCommon["80000003"], false, false));
				}
				return;
			}
			while (true)
			{
				Array.Clear(array, 0, array.Length);
				serialPort.DiscardInBuffer();
				serialPort.Write(Class10.CMD_PRG, 0, Class10.CMD_PRG.Length);
				stopwatch.Reset();
				while (true)
				{
					Array.Clear(array, 0, array.Length);
					num7 = serialPort.Read(array, 0, 1);
					if (array[0] != Class10.CMD_ACK[0])
					{
						if (stopwatch.ElapsedMilliseconds < 5000L)
						{
							continue;
						}
					}
					else
					{
						Array.Clear(array, 0, array.Length);
						serialPort.DiscardInBuffer();
						serialPort.Write(Class10.CMD_PRG2, 0, Class10.CMD_PRG2.Length);
						num4 = 16;
						for (num7 = serialPort.Read(array, 0, 16); num7 < num4; num7 += serialPort.Read(array, num7, num4 - num7))
						{
							Thread.Sleep(20);
						}
						Array.Clear(array, 0, array.Length);
						serialPort.DiscardInBuffer();
						serialPort.Write(Class10.CMD_ACK, 0, Class10.CMD_ACK.Length);
						num4 = 1;
						for (num7 = serialPort.Read(array, 0, 1); num7 < num4; num7 += serialPort.Read(array, num7, num4 - num7))
						{
							Thread.Sleep(20);
						}
						if (array[0] == Class10.CMD_ACK[0])
						{
							if (!flag && Settings.CUR_PWD != "DT8168")
							{
								i = Settings.ADDR_PWD;
								num5 = 8;
								byte[] buffer = new byte[4]
								{
									82,
									(byte)(i >> 8),
									(byte)i,
									8
								};
								Array.Clear(array, 0, array.Length);
								serialPort.DiscardInBuffer();
								serialPort.Write(buffer, 0, 4);
								num4 = 12;
								for (num7 = serialPort.Read(array, 0, 12); num7 < num4; num7 += serialPort.Read(array, num7, num4 - num7))
								{
									Thread.Sleep(20);
								}
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
									serialPort.DiscardInBuffer();
									serialPort.Write(Class10.CMD_ENDW, 0, Class10.CMD_ENDW.Length);
									num4 = 1;
									for (num7 = serialPort.Read(array, 0, 1); num7 < num4; num7 += serialPort.Read(array, num7, num4 - num7))
									{
										Thread.Sleep(20);
									}
									if (array[0] == Class10.CMD_ACK[0])
									{
										flag = true;
										break;
									}
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
											serialPort.DiscardInBuffer();
											serialPort.Write(Class10.CMD_ENDW, 0, Class10.CMD_ENDW.Length);
											num4 = 1;
											for (num7 = serialPort.Read(array, 0, 1); num7 < num4; num7 += serialPort.Read(array, num7, num4 - num7))
											{
												Thread.Sleep(20);
											}
											if (array[0] == Class10.CMD_ACK[0])
											{
												flag = true;
												break;
											}
											goto IL_07ef;
										}
										this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "", true, true));
										serialPort.Close();
										return;
									}
									Array.Clear(array, 0, array.Length);
									serialPort.DiscardInBuffer();
									serialPort.Write(Class10.CMD_ENDW, 0, Class10.CMD_ENDW.Length);
									num4 = 1;
									for (num7 = serialPort.Read(array, 0, 1); num7 < num4; num7 += serialPort.Read(array, num7, num4 - num7))
									{
										Thread.Sleep(20);
									}
									if (array[0] == Class10.CMD_ACK[0])
									{
										flag = true;
										flag = true;
										break;
									}
								}
							}
							else
							{
								num = 0;
								while (true)
								{
									if (num < this.START_ADDR.Length)
									{
										num8 = this.START_ADDR[num];
										num9 = this.END_ADDR[num];
										i = num8;
										while (i < num9)
										{
											if (!this.getCancelCom())
											{
												num6 = i % 128;
												num5 = ((i + 128 <= num9) ? (128 - num6) : (num9 - i));
												byte[] array4 = new byte[num5 + 4];
												array4[0] = 87;
												array4[1] = (byte)(i >> 8);
												array4[2] = (byte)i;
												array4[3] = (byte)num5;
												Array.Clear(array, 0, array.Length);
												Array.Copy(array2, i, array4, 4, num5);
												serialPort.DiscardInBuffer();
												serialPort.Write(array4, 0, 4 + num5);
												num4 = 1;
												for (num7 = serialPort.Read(array, 0, 1); num7 < num4; num7 += serialPort.Read(array, num7, num4 - num7))
												{
													Thread.Sleep(20);
												}
												if (array[0] == Class10.CMD_ACK[0])
												{
													if (this.OnFirmwareUpdateProgress != null)
													{
														this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs((float)(++num2) * 100f / (float)num3, i.ToString(), false, false));
													}
													i += num5;
													continue;
												}
												goto IL_07ef;
											}
											Array.Clear(array, 0, array.Length);
											serialPort.DiscardInBuffer();
											serialPort.Write(Class10.CMD_ENDR, 0, Class10.CMD_ENDR.Length);
											num4 = 1;
											for (num7 = serialPort.Read(array, 0, 1); num7 < num4; num7 += serialPort.Read(array, num7, num4 - num7))
											{
												Thread.Sleep(20);
											}
											serialPort.Close();
											return;
										}
										num++;
										continue;
									}
									Array.Clear(array, 0, array.Length);
									serialPort.DiscardInBuffer();
									serialPort.Write(Class10.CMD_ENDW, 0, Class10.CMD_ENDW.Length);
									num4 = 1;
									for (num7 = serialPort.Read(array, 0, 1); num7 < num4; num7 += serialPort.Read(array, num7, num4 - num7))
									{
										Thread.Sleep(20);
									}
									if (array[0] != Class10.CMD_ACK[0])
									{
										break;
									}
									serialPort.Close();
									if (this.OnFirmwareUpdateProgress != null)
									{
										this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(100f, Settings.dicCommon["80000102"], false, true));
									}
									return;
								}
							}
						}
					}
					goto IL_07ef;
					IL_07ef:
					if (this.OnFirmwareUpdateProgress != null)
					{
						this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.dicCommon["80000004"], true, true));
					}
					serialPort.Close();
					return;
				}
			}
		}
		catch (TimeoutException ex2)
		{
			Console.WriteLine(ex2.Message);
			if (this.OnFirmwareUpdateProgress != null)
			{
				this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, Settings.dicCommon["80000004"], false, false));
			}
			serialPort.Close();
		}
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public void method_9(Delegate0 delegate0_0)
	{
		this.OnFirmwareUpdateProgress = (Delegate0)Delegate.Combine(this.OnFirmwareUpdateProgress, delegate0_0);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public void setCancelCom0(Delegate0 delegate0_0)
	{
		this.OnFirmwareUpdateProgress = (Delegate0)Delegate.Remove(this.OnFirmwareUpdateProgress, delegate0_0);
	}

	public Class10() : base()
	{
		
		this.START_ADDR = new int[0];
		this.END_ADDR = new int[0];
	}

	static Class10()
	{
		
		Class10.CMD_ENDR = Encoding.ASCII.GetBytes("ENDR");
		Class10.CMD_ENDW = Encoding.ASCII.GetBytes("ENDW");
		Class10.CMD_ACK = new byte[1]
		{
			65
		};
		Class10.CMD_PRG = new byte[7]
		{
			2,
			80,
			82,
			79,
			71,
			82,
			65
		};
		Class10.CMD_PRG2 = new byte[2]
		{
			77,
			2
		};
	}
}
