namespace DMR
{
	public interface IFirmwareUpdate
	{
		event FirmwareUpdateProgressEventHandler OnFirmwareUpdateProgress;

		void UpdateFirmware();

		void MassErase();

		bool ParseDFU_File(string Filepath, out ushort VID, out ushort PID, out ushort Version);
	}
}
