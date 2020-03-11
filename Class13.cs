using System.Runtime.CompilerServices;

public class Class13
{
    private int _minVal;
    private int _maxVal;
	private int _incVal;
	private decimal _SdVal;
	private int _MaxLen;

	public Class13() 
	{
	}

	public Class13(int int_0, int int_1, int int_2, decimal decimal_0, int int_3)
	{
		this.method_1(int_0);
		this.method_3(int_1);
		this.method_5(int_2);
		this.method_7(decimal_0);
		this.method_9(int_3);
	}

	[CompilerGenerated]
	public int method_0()
	{
		return this._minVal;
	}

	[CompilerGenerated]
	public void method_1(int int_0)
	{
		this._minVal = int_0;
	}

	[CompilerGenerated]
	public int method_2()
	{
		return this._maxVal;
	}

	[CompilerGenerated]
	public void method_3(int int_0)
	{
		this._maxVal = int_0;
	}


	[CompilerGenerated]
	public int method_4()
	{
		return this._incVal;
	}

	[CompilerGenerated]
	public void method_5(int int_0)
	{
		this._incVal = int_0;
	}


	[CompilerGenerated]
	public decimal method_6()
	{
		return this._SdVal;
	}

	[CompilerGenerated]
	public void method_7(decimal decimal_0)
	{
		this._SdVal = decimal_0;
	}


	[CompilerGenerated]
	public int method_8()
	{
		return this._MaxLen;
	}

	[CompilerGenerated]
	public void method_9(int int_0)
	{
		this._MaxLen = int_0;
	}
}
