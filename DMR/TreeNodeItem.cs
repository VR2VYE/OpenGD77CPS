using System;
using System.Windows.Forms;

namespace DMR
{
	public class TreeNodeItem
	{
		public int Index
		{
			get;
			set;
		}

		public Type Type
		{
			get;
			set;
		}

		public Type SubType
		{
			get;
			set;
		}

		public ContextMenuStrip Cms
		{
			get;
			set;
		}

		public int Count
		{
			get;
			set;
		}

		public int ImageIndex
		{
			get;
			set;
		}

		public IData Data
		{
			get;
			set;
		}

		public TreeNodeItem()
		{
			
			//base._002Ector();
			this.Type = null;
			this.Index = -1;
			this.Count = 0;
			this.Cms = null;
			this.ImageIndex = 0;
		}

		public TreeNodeItem(ContextMenuStrip cms, Type type, Type subType, int count, int index, int imgIndex, IData data)
		{
			
			//base._002Ector();
			this.Cms = cms;
			this.Type = type;
			this.SubType = subType;
			this.Count = count;
			this.Index = index;
			this.ImageIndex = imgIndex;
			this.Data = data;
		}

		public void Paste(TreeNodeItem copyItem)
		{
			this.Data.Paste(copyItem.Index, this.Index);
		}
	}
}
