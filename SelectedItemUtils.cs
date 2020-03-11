using DMR;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

internal class SelectedItemUtils
{
	public int Value
	{
		get;
		set;
	}

	public string Name
	{
		get;
		set;
	}

    int _DispNum;

	[CompilerGenerated]
	public int method_0()
	{
		return this._DispNum;
	}

	[CompilerGenerated]
	public void method_1(int int_0)
	{
		this._DispNum = int_0;
	}

	public SelectedItemUtils(int int_0, int int_1, string string_0)
	{
		
		this.Value = int_1;
		this.method_1(int_0);
		this.Name = string_0;
	}

	public override string ToString()
	{
		if (this.method_0() < 0)
		{
			return this.Name;
		}
		string s = string.Format("{0:d3}:{1}", this.method_0() + 1, this.Name);
		Console.WriteLine(s);
		return s;
	}
}
