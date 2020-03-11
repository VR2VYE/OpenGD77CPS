using System;
using System.Windows.Forms;

internal class TextMsgFormCustomPasteWndProc : NativeWindow
{
	private const int WM_CHAR = 258;
	private const int WM_PASTE = 770;

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

	public TextMsgFormCustomPasteWndProc()
	{
	}
}
