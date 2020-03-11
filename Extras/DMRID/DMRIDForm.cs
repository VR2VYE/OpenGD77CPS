using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace DMR
{
	public partial class DMRIDForm : Form
	{
		public static byte[] DMRIDBuffer = new byte[0x40000];

		static  List<DMRDataItem> DataList = null;
		private static byte[] SIG_PATTERN_BYTES;
		private WebClient _wc;
		private bool _isDownloading = false;
		//private int MAX_RECORDS = 10920;
		private int _stringLength = 8;
		const int HEADER_LENGTH = 12;

		private SerialPort _port = null;

		private BackgroundWorker worker;

		public enum CommsDataMode { DataModeNone = 0, DataModeReadFlash = 1, DataModeReadEEPROM = 2, DataModeWriteFlash = 3, DataModeWriteEEPROM = 4 };
		public enum CommsAction { NONE, BACKUP_EEPROM, BACKUP_FLASH, RESTORE_EEPROM, RESTORE_FLASH, READ_CODEPLUG, WRITE_CODEPLUG }

		private SaveFileDialog _saveFileDialog = new SaveFileDialog();
		private OpenFileDialog _openFileDialog = new OpenFileDialog();


		public static void ClearStaticData()
		{
			DMRIDBuffer = new byte[0x40000];
		}

		public DMRIDForm()
		{
			SIG_PATTERN_BYTES = new byte[] { 0x49, 0x44, 0x2D, 0x56, 0x30, 0x30, 0x31, 0x00 };
			InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			cmbStringLen.Visible = false;
			lblEnhancedLength.Visible = false;

			txtRegionId.Text = (int.Parse(GeneralSetForm.data.RadioId) / 10000).ToString();

			DataList = new List<DMRDataItem>();

			cmbStringLen.SelectedIndex = 2;


			dataGridView1.AutoGenerateColumns = false;
			DataGridViewCell cell = new DataGridViewTextBoxCell();
			DataGridViewTextBoxColumn colFileName = new DataGridViewTextBoxColumn()
			{
				CellTemplate = cell,
				Name = "Id", // internal name
				HeaderText = "ID",// Column header text
				DataPropertyName = "DMRID" // object property
			};
			dataGridView1.Columns.Add(colFileName);

			cell = new DataGridViewTextBoxCell();
			colFileName = new DataGridViewTextBoxColumn()
			{
				CellTemplate = cell,
				Name = "Call",// internal name
				HeaderText = "Callsign",// Column header text
				DataPropertyName = "Callsign"  // object property
			};
			dataGridView1.Columns.Add(colFileName);

			cell = new DataGridViewTextBoxCell();
			colFileName = new DataGridViewTextBoxColumn()
			{
				CellTemplate = cell,
				Name = "Name",// internal name
				HeaderText = "Name",// Column header text
				DataPropertyName = "Name"  // object property
			};
			dataGridView1.Columns.Add(colFileName);


			cell = new DataGridViewTextBoxCell();
			colFileName = new DataGridViewTextBoxColumn()
			{
				CellTemplate = cell,
				Name = "Age",// internal name
				HeaderText = "Last heard (days ago)",// Column header text
				DataPropertyName = "AgeInDays",  // object property
				Width = 140,
				ValueType = typeof(int),
				SortMode = DataGridViewColumnSortMode.Automatic
			};
			dataGridView1.Columns.Add(colFileName);
			dataGridView1.UserDeletedRow += new DataGridViewRowEventHandler(dataGridRowDeleted);

			rebindData();	

#if OpenGD77
			chkEnhancedFirmware.Checked = true;
			chkEnhancedFirmware.Visible = false;
#endif
		}

		private void dataGridRowDeleted(object sender, DataGridViewRowEventArgs e)
		{
			updateTotalNumberMessage();
		}

		private void rebindData()
		{
			BindingList<DMRDataItem> bindingList = new BindingList<DMRDataItem>(DataList);
			var source = new BindingSource(bindingList, null);
			dataGridView1.DataSource = source;

			updateTotalNumberMessage();
		}

		private void btnDownload_Click(object sender, EventArgs e)
		{
			if (DataList == null || _isDownloading)
			{
				return;
			}

			_wc = new WebClient();
			try
			{
				lblMessage.Text = Settings.dicCommon["DownloadContactsDownloading"];
				Cursor.Current = Cursors.WaitCursor;
				this.Refresh();
				Application.DoEvents();
				_wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(downloadCompleteHandler);
				_wc.DownloadStringAsync(new Uri("http://ham-digital.org/user_by_lh.php?id=" + txtRegionId.Text));
	
			}
			catch (Exception )
			{
				Cursor.Current = Cursors.Default;
				MessageBox.Show(Settings.dicCommon["UnableDownloadFromInternet"]);
				return;
			}
			_isDownloading = true;

		}


		private void downloadCompleteHandler(object sender, DownloadStringCompletedEventArgs e )
		{
			string ownRadioId = GeneralSetForm.data.RadioId;
			string csv;// = e.Result;
			int maxAge = Int32.MaxValue;


			try
			{
				csv = e.Result;
			}
			catch(Exception)
			{
				MessageBox.Show(Settings.dicCommon["UnableDownloadFromInternet"]);
				return;
			}

			try
			{
				maxAge = Int32.Parse(this.txtAgeMaxDays.Text);
			}
			catch(Exception)
			{

			}

			try
			{
				bool first = true;
				foreach (var csvLine in csv.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
				{
					if (first)
					{
						first = false;
						continue;
					}
					DMRDataItem item = (new DMRDataItem()).FromHamDigital(csvLine);
					if (item.AgeAsInt <= maxAge)
					{
						DataList.Add(item);
					}
				}
				DataList = DataList.Distinct().ToList();

				rebindData();
				Cursor.Current = Cursors.Default;
	
			}
			catch (Exception ex)
			{
				MessageBox.Show(Settings.dicCommon["ErrorParsingData"]);
			}
			finally
			{
				_wc = null;
				_isDownloading = false;
				Cursor.Current = Cursors.Default;
			}
		}

		private void updateTotalNumberMessage()
		{
			string message = Settings.dicCommon["DMRIdContcatsTotal"];// "Total number of IDs = {0}. Max of MAX_RECORDS can be uploaded";
			lblMessage.Text = string.Format(message, DataList.Count, MAX_RECORDS);
		}

		private void downloadProgressHandler(object sender, DownloadProgressChangedEventArgs e)
		{
			try
			{
				BeginInvoke((Action)(() =>
				{
					lblMessage.Text = Settings.dicCommon["DownloadContactsDownloading"] + e.ProgressPercentage + "%";
				}));
			}
			catch (Exception)
			{
				// No nothing
			}
		}

		private void btnReadFromGD77_Click(object sender, EventArgs e)
		{

			MainForm.CommsBuffer = new byte[0x100000];// 128k buffer
			CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.dataRead;
			CommPrgForm commPrgForm = new CommPrgForm(true);// true =  close download form as soon as download is complete
			commPrgForm.StartPosition = FormStartPosition.CenterParent;
			CodeplugComms.startAddress = 0x50100;
			CodeplugComms.transferLength = 0x20;
			DialogResult result = commPrgForm.ShowDialog();
			Array.Copy(MainForm.CommsBuffer, 0x50100, DMRIDForm.DMRIDBuffer, 0, 0x20);
			if (!isInMemoryAccessMode(DMRIDForm.DMRIDBuffer))
			{
				MessageBox.Show(Settings.dicCommon["EnableMemoryAccessMode"]);
				return;
			}

			CodeplugComms.startAddress = 0x30000;
			CodeplugComms.transferLength = 0x20;
			result = commPrgForm.ShowDialog();
			Array.Copy(MainForm.CommsBuffer, 0x30000, DMRIDForm.DMRIDBuffer, 0, 0x20);


			int numRecords = BitConverter.ToInt32(DMRIDForm.DMRIDBuffer, 8);
			int stringLen = (int)DMRIDForm.DMRIDBuffer[3]-0x4a - 4;
			if (stringLen!=8)
			{
				chkEnhancedFirmware.Checked=true;
			}
			cmbStringLen.SelectedIndex = stringLen - 6;
			
			CodeplugComms.startAddress = 0x30000;
			CodeplugComms.transferLength = Math.Min((this.chkEnhancedFirmware.Checked == true ? 0x40000 : 0x20000), HEADER_LENGTH + (numRecords + 2) * (4 + _stringLength));

			CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.dataRead;
			result = commPrgForm.ShowDialog();
			Array.Copy(MainForm.CommsBuffer, 0x30000, DMRIDForm.DMRIDBuffer, 0, CodeplugComms.transferLength);
			radioToData();
			rebindData();
			//DataToCodeplug();
		}

		private void radioToData()
		{
			byte[] buf = new byte[(4 + _stringLength)];
			DataList = new List<DMRDataItem>();
			int numRecords = BitConverter.ToInt32(DMRIDForm.DMRIDBuffer, 8);
			for (int i = 0; i < numRecords; i++)
			{
				Array.Copy(DMRIDForm.DMRIDBuffer, HEADER_LENGTH + i * (4 + _stringLength), buf, 0, (4 + _stringLength));
				DataList.Add((new DMRDataItem()).FromRadio(buf, _stringLength));
			}
		}

		public void CodeplugToData()
		{
			byte[] buf = new byte[(4 + _stringLength)];
			DataList = new List<DMRDataItem>();
			int numRecords = BitConverter.ToInt32(DMRIDForm.DMRIDBuffer, 8);// Number of records is stored at offset 8
			for (int i = 0; i < numRecords; i++)
			{
				Array.Copy(DMRIDForm.DMRIDBuffer, HEADER_LENGTH + i * (4 + _stringLength), buf, 0, (4 + _stringLength));
				DataList.Add(new DMRDataItem(buf, _stringLength));
			}
		}

		private int MAX_RECORDS
		{
			get
			{
				return ((this.chkEnhancedFirmware.Checked==true?0x40000:0x20000) - HEADER_LENGTH) / (_stringLength + 4);
			}
		}

		private byte[] GenerateUploadData()
		{

			int numRecords = Math.Min(DataList.Count, MAX_RECORDS);
			int dataSize = numRecords * (4 + _stringLength) + HEADER_LENGTH;
			dataSize = ((dataSize / 32)+1) * 32;
			byte[] buffer = new byte[dataSize];

			Array.Copy(SIG_PATTERN_BYTES, buffer, SIG_PATTERN_BYTES.Length);
			Array.Copy(BitConverter.GetBytes(numRecords), 0, buffer, 8, 4);

			if (DataList == null)
			{
				return buffer;
			}
			List<DMRDataItem> uploadList = new List<DMRDataItem>(DataList);// Need to make a copy so we can sort it and not screw up the list in the dataGridView
			uploadList.Sort();
			for (int i = 0; i < numRecords; i++)
			{
				Array.Copy(uploadList[i].getRadioData(_stringLength), 0, buffer, HEADER_LENGTH + i * (4 + _stringLength), (4 + _stringLength));
			}
			return buffer;
		}

		private bool isInMemoryAccessMode(byte []buffer)
		{
			for (int i = 0; i < 0x20; i++)
			{
				if (buffer[i] != 00)
				{
					return true;
				}
			}
			return false;
		}

		private bool hasSig()
		{
			
			for (int i = 0; i < SIG_PATTERN_BYTES.Length; i++)
			{
				if (DMRIDForm.DMRIDBuffer[i] != SIG_PATTERN_BYTES[i])
				{
				return false;
				}
			}
			return true;
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			DataList = new List<DMRDataItem>();
			rebindData();
			//DataToCodeplug();

		}

	
		private void writeToOfficalFirmware()
		{
			MainForm.CommsBuffer = new byte[0x100000];// 128k buffer
			CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.dataRead;
			CommPrgForm commPrgForm = new CommPrgForm(true);// true =  close download form as soon as download is complete
			commPrgForm.StartPosition = FormStartPosition.CenterParent;
			CodeplugComms.startAddress = 0x50100;
			CodeplugComms.transferLength = 0x20;
			DialogResult result = commPrgForm.ShowDialog();
			Array.Copy(MainForm.CommsBuffer, 0x50100, DMRIDForm.DMRIDBuffer, 0, 0x20);
			if (!isInMemoryAccessMode(DMRIDForm.DMRIDBuffer))
			{
				MessageBox.Show(Settings.dicCommon["EnableMemoryAccessMode"]);
				return;
			}


			SIG_PATTERN_BYTES[3] = (byte)(0x4a + _stringLength + 4);

			byte[] uploadData = GenerateUploadData();
			Array.Copy(uploadData, 0, MainForm.CommsBuffer, 0x30000, (uploadData.Length / 32) * 32);
			CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.dataWrite;

			CodeplugComms.startAddress = 0x30000;
			CodeplugComms.transferLength = (uploadData.Length / 32) * 32;
			commPrgForm.StartPosition = FormStartPosition.CenterParent;
			result = commPrgForm.ShowDialog();
		}

		private void btnWriteToGD77_Click(object sender, EventArgs e)
		{
			if (SetupDiWrap.ComPortNameFromFriendlyNamePrefix("OpenGD77") != null)
			{
				writeToOpenGD77();
			}
			else
			{
				writeToOfficalFirmware();
			}
		}

		private void DMRIDFormNew_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		private void DMRIDForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);// Update texts etc from language xml file
		}

		private void chkEnhancedFirmware_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkEnhancedFirmware.Checked == false)
			{
				cmbStringLen.Visible = false;
				lblEnhancedLength.Visible = false;
				cmbStringLen.SelectedIndex = 2;
			}
			else
			{
#if OpenGD77
				cmbStringLen.SelectedIndex = 9;
				cmbStringLen.Visible = true;
				lblEnhancedLength.Visible = true;
#else
				if (DialogResult.OK == MessageBox.Show("This mode ONLY works with the OpenGD77 or other modified firmware installed in the GD-77.", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2))
				{
					cmbStringLen.SelectedIndex = 9;
					cmbStringLen.Visible = true;
					lblEnhancedLength.Visible = true;
				}
				else
				{
					this.Close();
				}
#endif
			}
			
			updateTotalNumberMessage();
		}

		private void cmbStringLen_SelectedIndexChanged(object sender, EventArgs e)
		{
			_stringLength = cmbStringLen.SelectedIndex + 6;
			updateTotalNumberMessage();
		}

		#region OpenGD77
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

		private void close_data_mode()
		{
			//data_mode = OpenGD77CommsTransferData.CommsDataMode.DataModeNone;
		}

		private void ReadFlashOrEEPROM(OpenGD77CommsTransferData dataObj)
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
					Console.WriteLine(String.Format("read stopped (error at {0:X8})", currentDataAddressInTheRadio));
					close_data_mode();

				}
				size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;
			}
			close_data_mode();
		}

		private void WriteFlash(OpenGD77CommsTransferData dataObj)
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
				if (size > 32)
				{
					size = 32;
				}

				if (dataObj.data_sector == -1)
				{
					if (!flashWritePrepareSector(currentDataAddressInTheRadio, ref sendbuffer, ref readbuffer, dataObj))
					{
						close_data_mode();
						break;
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
								close_data_mode();
								break;
							};
						}
					}
					else
					{
						close_data_mode();
						break;
					}
				}
				size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;
			}

			if (dataObj.data_sector != -1)
			{
				if (!flashWriteSector(ref sendbuffer, ref readbuffer, dataObj))
				{
					Console.WriteLine(String.Format("Error. Write stopped (write sector error at {0:X8})", currentDataAddressInTheRadio));
				};
			}

			close_data_mode();
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
		#endregion

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


		private void writeToOpenGD77()
		{
			String gd77CommPort = SetupDiWrap.ComPortNameFromFriendlyNamePrefix("OpenGD77");

			try
			{
				_port = new SerialPort(gd77CommPort, 115200, Parity.None, 8, StopBits.One);
				_port.ReadTimeout = 1000;
				_port.Open();
			}
			catch (Exception)
			{
				_port = null;
				MessageBox.Show("Failed to open comm port", "Error");
				return;
			}

			// Commands to control the screen etc in the firmware
			sendCommand(0);
			sendCommand(1);
			sendCommand(2, 0, 0, 3, 1, 0, "CPS");
			sendCommand(2, 0, 16, 3, 1, 0, "Writing");
			sendCommand(2, 0, 32, 3, 1, 0, "DMRID");
			sendCommand(2, 0, 48, 3, 1, 0, "Database");
			sendCommand(3);
			sendCommand(6, 4);// flash red LED


			OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.NONE);
			dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteFlash;

			SIG_PATTERN_BYTES[3] = (byte)(0x4a + _stringLength + 4);

			dataObj.dataBuff = GenerateUploadData();
			dataObj.localDataBufferStartPosition = 0;
			dataObj.startDataAddressInTheRadio = 0x30000;
			dataObj.transferLength = (dataObj.dataBuff.Length / 32) * 32;
			WriteFlash(dataObj);
			progressBar1.Value = 0;
			sendCommand(5);
			if (_port != null)
			{
				try
				{
					_port.Close();
				}
				catch (Exception)
				{
					MessageBox.Show("Failed to close OpenGD77 comm port", "Warning");
				}
			}
		}

		private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			if (e.Column.Index != 3)
			{
				return;
			}
			try
			{
				if (Int32.Parse(e.CellValue1.ToString()) < Int32.Parse(e.CellValue2.ToString()))
				{
					e.SortResult = -1;
				}
				else
				{
					e.SortResult = 1;
				}
				e.Handled = true;
			}
			catch (Exception)
			{
			}

		}

		private void btnImportCSV_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "firmware files|*.csv";

			if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != null)
			{
				try
				{

					bool first = true;
					String region = txtRegionId.Text;
					using (var reader = new StreamReader(openFileDialog.FileName))
					{
						while (!reader.EndOfStream)
						{
							string csvLine = reader.ReadLine();
							if (first)
							{
								first = false;
								continue;
							}
							if (csvLine.Length > 0)
							{
								DMRDataItem item = (new DMRDataItem()).FromRadioidDotNet(csvLine);
								if (item.DMRIdString.IndexOf(txtRegionId.Text) == 0)
								{
									DataList.Add(item);
								}
							}
						}
						DataList = DataList.Distinct().ToList();

						rebindData();
						Cursor.Current = Cursors.Default;

					}
				}
				catch (Exception)
				{
					MessageBox.Show("The CSV file could not be opened");
				}
			}
		}
	}
}

