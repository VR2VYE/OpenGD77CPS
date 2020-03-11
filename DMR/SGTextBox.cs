using System;
using System.Text;
using System.Windows.Forms;

namespace DMR
{
	public class SGTextBox : TextBox
	{
		private const int WM_CHAR = 258;

		private const int WM_PASTE = 770;

		public int MaxByteLength
		{
			get;
			set;
		}

		public string InputString
		{
			get;
			set;
		}

		public SGTextBox()
		{
			
			//base._002Ector();
			this.ContextMenu = new ContextMenu();
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar))
			{
				if (!string.IsNullOrEmpty(this.InputString) && this.InputString.IndexOf(char.ToUpper(e.KeyChar)) < 0)
				{
					e.Handled = true;
				}
				else
				{
					base.OnKeyPress(e);
					if (this.MaxByteLength != 0 && !char.IsControl(e.KeyChar))
					{
						int byteCount = Encoding.GetEncoding(936).GetByteCount(this.Text + e.KeyChar.ToString());
						int byteCount2 = Encoding.GetEncoding(936).GetByteCount(this.SelectedText);
						if (byteCount - byteCount2 > this.MaxByteLength)
						{
							e.Handled = true;
						}
					}
				}
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 770)
			{
				this.method_0();
			}
			else
			{
				base.WndProc(ref m);
			}
		}

		private void method_0()
		{
			string text = Clipboard.GetText();
			foreach (char value in text)
			{
				Message message = default(Message);
				message.HWnd = base.Handle;
				message.Msg = 258;
				message.WParam = (IntPtr)(int)value;
				message.LParam = IntPtr.Zero;
				base.WndProc(ref message);
			}
		}
	}
}
