using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMR
{
	public class OpenGD77CommsTransferData
	{
		public enum CommsDataMode { DataModeNone = 0, DataModeReadFlash = 1, DataModeReadEEPROM = 2, DataModeWriteFlash = 3, DataModeWriteEEPROM = 4, DataModeReadMCUROM = 5, DataModeReadScreenGrab = 6 };
			public enum CommsAction { NONE, BACKUP_EEPROM,RESTORE_EEPROM, BACKUP_FLASH,RESTORE_FLASH, BACKUP_CALIBRATION ,RESTORE_CALIBRATION,READ_CODEPLUG, WRITE_CODEPLUG,BACKUP_MCU_ROM,DOWLOAD_SCREENGRAB	 }


			public CommsDataMode mode;
			public CommsAction action;
			public int startDataAddressInTheRadio = 0;
			public int transferLength = 0;

			public int localDataBufferStartPosition = 0;

			public int data_sector = 0;
			public byte[] dataBuff;

			public int responseCode=0;

			public OpenGD77CommsTransferData(CommsAction theAction)
			{
				action = theAction;
			}
	}
}
