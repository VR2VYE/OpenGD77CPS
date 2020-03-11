using System;
using System.Windows.Forms;

internal class CustomPanel : Panel
{
	protected override void OnClick(EventArgs e)
	{
		base.Focus();
		base.OnClick(e);
	}

	public CustomPanel() : base()
	{
		
		////base._002Ector();
	}
}
