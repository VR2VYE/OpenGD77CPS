using System;
using System.Collections.Generic;
using System.Windows.Forms;

internal static class Class16
{
	public static void Fill<templateType>(this templateType[] arrayData, templateType fillValue) where templateType : struct
	{
		int num = 0;
		for (num = 0; num < arrayData.Length; num++)
		{
			arrayData[num] = fillValue;
		}
	}

	public static void FillFromPositionWithLength<templateType>(this templateType[] arrayData, templateType item, int startPosition, int length) where templateType : struct
	{
		int num = 0;
		if (startPosition + length > arrayData.Length)
		{
			throw new IndexOutOfRangeException();
		}
		for (num = 0; num < length; num++)
		{
			arrayData[startPosition + num] = item;
		}
	}

	public static void RemoveItemFromArray<templateType>(this templateType[] arrayData, int indexNum)
	{
		int i;
		for (i = indexNum; i < arrayData.Length - 1; i++)
		{
			arrayData[i] = arrayData[i + 1];
		}
		arrayData[i] = default(templateType);
	}

	public static void smethod_3<templateType>(this templateType[] arrayData, int int_0, templateType item) where templateType : struct
	{
		int i;
		for (i = int_0; i < arrayData.Length - 1; i++)
		{
			arrayData[i] = arrayData[i + 1];
		}
		arrayData[i] = item;
	}

	public static bool smethod_4(this byte[] byte_0, byte[] byte_1)
	{
		if (byte_0.Length < byte_1.Length)
		{
			return false;
		}
		int num = 0;
		int num2 = 0;
		while (true)
		{
			if (num2 < byte_1.Length)
			{
				byte b = byte_1[num2];
				if (b == byte_0[num++])
				{
					num2++;
					continue;
				}
				break;
			}
			return true;
		}
		return false;
	}

	public static List<TreeNode> smethod_5(this TreeView treeView_0)
	{
		List<TreeNode> list = new List<TreeNode>();
		foreach (TreeNode node in treeView_0.Nodes)
		{
			list.AddRange(node.smethod_6());
		}
		return list;
	}

	public static List<TreeNode> smethod_6(this TreeNode treeNode_0)
	{
		List<TreeNode> list = new List<TreeNode>();
		list.Add(treeNode_0);
		foreach (TreeNode node in treeNode_0.Nodes)
		{
			list.AddRange(node.smethod_6());
		}
		return list;
	}

	public static List<ToolStripMenuItem> smethod_7(this MenuStrip menuStrip_0)
	{
		List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();
		foreach (ToolStripMenuItem item in menuStrip_0.Items)
		{
			list.AddRange(item.smethod_8());
		}
		return list;
	}

	public static List<ToolStripMenuItem> smethod_8(this ToolStripMenuItem toolStripMenuItem_0)
	{
		List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();
		list.Add(toolStripMenuItem_0);
		foreach (ToolStripItem dropDownItem in toolStripMenuItem_0.DropDownItems)
		{
			if (dropDownItem is ToolStripMenuItem)
			{
				ToolStripMenuItem toolStripMenuItem_ = dropDownItem as ToolStripMenuItem;
				list.AddRange(toolStripMenuItem_.smethod_8());
			}
		}
		return list;
	}

	public static List<ToolStripMenuItem> smethod_9(this ContextMenuStrip contextMenuStrip_0)
	{
		List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();
		foreach (ToolStripItem item in contextMenuStrip_0.Items)
		{
			ToolStripMenuItem toolStripMenuItem = item as ToolStripMenuItem;
			if (toolStripMenuItem != null)
			{
				list.AddRange(toolStripMenuItem.smethod_8());
			}
		}
		return list;
	}

	public static List<ToolStripItem> smethod_10(this ToolStrip toolStrip_0)
	{
		List<ToolStripItem> list = new List<ToolStripItem>();
		foreach (ToolStripItem item in toolStrip_0.Items)
		{
			list.AddRange(item.smethod_11());
		}
		return list;
	}

	public static List<ToolStripItem> smethod_11(this ToolStripItem toolStripItem_0)
	{
		List<ToolStripItem> list = new List<ToolStripItem>();
		if (toolStripItem_0 is ToolStripDropDownItem)
		{
			ToolStripDropDownItem toolStripDropDownItem = toolStripItem_0 as ToolStripDropDownItem;
			{
				foreach (ToolStripItem dropDownItem in toolStripDropDownItem.DropDownItems)
				{
					list.AddRange(dropDownItem.smethod_11());
				}
				return list;
			}
		}
		if (!(toolStripItem_0 is ToolStripSeparator))
		{
			list.Add(toolStripItem_0);
		}
		return list;
	}

	public static List<Control> smethod_12(this Form form_0)
	{
		List<Control> list = new List<Control>();
		foreach (Control control in form_0.Controls)
		{
			list.AddRange(control.smethod_13());
		}
		return list;
	}

	public static List<Control> smethod_13(this Control control_0)
	{
		List<Control> list = new List<Control>();
		if (control_0 is GroupBox)
		{
			list.Add(control_0);
			{
				foreach (Control control in control_0.Controls)
				{
					list.AddRange(control.smethod_13());
				}
				return list;
			}
		}
		if (control_0 is Panel)
		{
			{
				foreach (Control control2 in control_0.Controls)
				{
					list.AddRange(control2.smethod_13());
				}
				return list;
			}
		}
		if (control_0 is Label || control_0 is CheckBox || control_0 is DataGridView || control_0 is Button || control_0 is ToolStrip)
		{
			list.Add(control_0);
		}
		return list;
	}
}
