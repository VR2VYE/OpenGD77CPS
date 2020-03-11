using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

internal class DoubleClickGroupBox : GroupBox
{
    bool _DoubleClickSelectCheckBox;
	[CompilerGenerated]
	public bool method_0()
	{
		return this._DoubleClickSelectCheckBox;
	}

	[CompilerGenerated]
	public void method_1(bool bool_0)
	{
		this._DoubleClickSelectCheckBox = bool_0;
	}

    bool _ClickFocus;
	[CompilerGenerated]
	public bool method_2()
	{
		return this._ClickFocus;
	}

	[CompilerGenerated]
	public void method_3(bool bool_0)
	{
		this._ClickFocus = bool_0;
	}

	public DoubleClickGroupBox()
	{
		this.method_1(false);
		this.method_3(true);
	}

	protected override void OnClick(EventArgs e)
	{
		if (this.method_2())
		{
			base.Focus();
		}
		base.OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		if (this.method_0())
		{
			MouseEventArgs mouseEventArgs = e as MouseEventArgs;
			if (mouseEventArgs != null)
			{
				foreach (object control in base.Controls)
				{
					CheckBox checkBox = control as CheckBox;
					if (checkBox != null && checkBox.Enabled)
					{
						if (mouseEventArgs.Button == MouseButtons.Left)
						{
							checkBox.Checked = true;
						}
						else if (mouseEventArgs.Button == MouseButtons.Right)
						{
							checkBox.Checked = false;
						}
						else
						{
							checkBox.Checked = !checkBox.Checked;
						}
					}
				}
			}
		}
		base.OnDoubleClick(e);
	}
}
