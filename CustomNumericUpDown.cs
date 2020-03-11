using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

internal class CustomNumericUpDown : NumericUpDown
{
	public void method_0(int int_0)
	{
		TextBox textBox = base.Controls[1] as TextBox;
		if (textBox != null)
		{
			textBox.MaxLength = int_0;
		}
	}

    string _003CInputString_003Ek__BackingField;
    decimal _003CReplaceValue_003Ek__BackingField;

	[CompilerGenerated]
	public string method_1()
	{
		return this._003CInputString_003Ek__BackingField;
	}

	[CompilerGenerated]
	public void method_2(string string_0)
	{
		this._003CInputString_003Ek__BackingField = string_0;
	}

	[CompilerGenerated]
	public decimal method_3()
	{
		return this._003CReplaceValue_003Ek__BackingField;
	}

	[CompilerGenerated]
	public void method_4(decimal decimal_0)
	{
		this._003CReplaceValue_003Ek__BackingField = decimal_0;
	}
    string _003CReplaceString_003Ek__BackingField;

	[CompilerGenerated]
	public string method_5()
	{
		return this._003CReplaceString_003Ek__BackingField;
	}

	[CompilerGenerated]
	public void method_6(string string_0)
	{
		this._003CReplaceString_003Ek__BackingField = string_0;
	}

	public override void UpButton()
	{
		decimal value = base.Value;
		value += base.Increment;
		if (value > base.Maximum)
		{
			value = base.Minimum;
		}
		else if (value % base.Increment != 0m)
		{
			value -= value % base.Increment;
		}
		base.Value = value;
	}

	public override void DownButton()
	{
		decimal value = base.Value;
		value -= base.Increment;
		if (value < base.Minimum)
		{
			value = base.Maximum;
		}
		else if (value % base.Increment != 0m)
		{
			value += base.Increment - value % base.Increment;
		}
		base.Value = value;
	}

	protected override void OnLostFocus(EventArgs e)
	{
		decimal value = base.Value;
		if (value < base.Minimum)
		{
			value = (base.Value = base.Maximum);
		}
		else if (value % base.Increment != 0m)
		{
			value = (base.Value = value - value % base.Increment);
		}
		base.OnLostFocus(e);
	}

	protected virtual void vmethod_0(object sender, EventArgs e)
	{
	}

	protected override void OnTextBoxKeyPress(object source, KeyPressEventArgs e)
	{
		if (!string.IsNullOrEmpty(this.method_1()) && this.method_1().IndexOf(e.KeyChar) < 0)
		{
			e.Handled = true;
		}
		else
		{
			base.OnTextBoxKeyPress(source, e);
		}
	}

	protected override void UpdateEditText()
	{
		if (this.method_5() != null && this.method_5().Length != 0 && base.Value == this.method_3())
		{
			this.Text = this.method_5();
		}
		else
		{
			base.UpdateEditText();
		}
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		HandledMouseEventArgs handledMouseEventArgs = e as HandledMouseEventArgs;
		if (handledMouseEventArgs != null)
		{
			handledMouseEventArgs.Handled = true;
		}
		if (e.Delta < 0)
		{
			SendKeys.Send("{DOWN}");
		}
		else
		{
			SendKeys.Send("{UP}");
		}
	}

	public CustomNumericUpDown():base()
	{
	}
}
