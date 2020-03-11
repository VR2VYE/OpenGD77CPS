using System.Windows.Forms;

namespace DMR
{
	public interface IData
	{
		int Count
		{
			get;
		}

		string Format
		{
			get;
		}

		bool ListIsEmpty
		{
			get;
		}

		int GetMinIndex();

		string GetMinName(TreeNode node);

		bool DataIsValid(int index);

		void SetIndex(int index, int value);

		void ClearIndex(int index);

		void SetName(int index, string text);

		string GetName(int index);

		void Default(int index);

		void Paste(int from, int to);
	}
}
