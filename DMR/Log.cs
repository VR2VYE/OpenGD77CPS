using System;
using System.IO;
using System.Windows.Forms;

namespace DMR
{
	public static class Log
	{
		private static string logFile;

		public static void WriteLine(string txt)
		{
			File.AppendAllText(Log.logFile, DateTime.Now.ToString() + ": " + txt + "\n");
		}

		static Log()
		{
			
			Log.logFile = Application.StartupPath + "\\Log.txt";
		}
	}
}
