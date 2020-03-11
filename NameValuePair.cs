internal class NameValuePair
{
	private string _text;
	private object _value;

	public string Text
	{
		get
		{
			return this._text;
		}
		set
		{
			this._text = value;
		}
	}

	public object Value
	{
		get
		{
			return this._value;
		}
		set
		{
			this._value = value;
		}
	}

	public override string ToString()
	{
		return this._text;
	}

	public NameValuePair(string string_0, object object_0)
	{
		this._text = string_0;
		this._value = object_0;
	}
}
