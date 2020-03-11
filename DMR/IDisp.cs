using System.Windows.Forms;

namespace DMR
{
	public interface IDisp
	{
		TreeNode Node
		{
			get;
			set;
		}

		void SaveData();

		void DispData();

		void RefreshName();
	}
}
