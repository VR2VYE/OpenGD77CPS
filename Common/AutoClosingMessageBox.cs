using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Common
{
	public class AutoClosingMessageBox
	{
		private const int WM_CLOSE = 16;

		private System.Threading.Timer _timeoutTimer;

		private string _caption;

		private AutoClosingMessageBox(string text, string caption, int timeout)
		{
			
			////base._002Ector();
			this._caption = caption;
			this._timeoutTimer = new System.Threading.Timer(this.method_0, null, timeout, -1);
			MessageBox.Show(text, caption);
		}

		public static void Show(string text, string caption, int timeout)
		{
			new AutoClosingMessageBox(text, caption, timeout);
		}

		private void method_0(object object_0)
		{
			IntPtr intPtr = AutoClosingMessageBox.FindWindow(null, this._caption);
			if (intPtr != IntPtr.Zero)
			{
				AutoClosingMessageBox.SendMessage(intPtr, 16u, IntPtr.Zero, IntPtr.Zero);
			}
			this._timeoutTimer.Dispose();
		}

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string string_0, string string_1);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr intptr_0, uint uint_0, IntPtr intptr_1, IntPtr intptr_2);
	}
}
