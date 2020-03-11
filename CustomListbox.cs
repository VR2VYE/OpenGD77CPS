using System.Drawing;
using System.Windows.Forms;

[ToolboxBitmap(typeof(ListBox))]
internal class CustomListbox : ListBox
{
	private ToolTip tip;

	private void method_0(string string_0)
	{
		this.tip.SetToolTip(this, string_0);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);
		int num = base.IndexFromPoint(e.Location);
		if (num == -1)
		{
			this.method_0("");
		}
		else
		{
			string itemText = base.GetItemText(base.Items[num]);
			this.method_0(itemText);
		}
	}

	public CustomListbox()
	{
		
		this.tip = new ToolTip();
		//base._002Ector();
	}
}
