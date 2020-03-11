using System;
using System.Windows.Forms;

namespace DMR
{
	public class MyDataGridViewTextBoxColumn : DataGridViewTextBoxColumn
	{
		public int MaxByteLength
		{
			get
			{
				return this.method_0().MaxByteLength;
			}
			set
			{
				this.method_0().MaxByteLength = value;
				if (base.DataGridView != null)
				{
					DataGridViewRowCollection rows = base.DataGridView.Rows;
					int count = rows.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridViewRow dataGridViewRow = rows.SharedRow(i);
						MyDataGridViewTextBoxCell myDataGridViewTextBoxCell = dataGridViewRow.Cells[base.Index] as MyDataGridViewTextBoxCell;
						if (myDataGridViewTextBoxCell != null)
						{
							myDataGridViewTextBoxCell.MaxByteLength = value;
						}
					}
					base.DataGridView.InvalidateColumn(base.Index);
				}
			}
		}

		public MyDataGridViewTextBoxColumn()
		{
			
			//base._002Ector();
			this.CellTemplate = new MyDataGridViewTextBoxCell();
			this.MaxByteLength = 2147483647;
		}

		private MyDataGridViewTextBoxCell method_0()
		{
			MyDataGridViewTextBoxCell myDataGridViewTextBoxCell = this.CellTemplate as MyDataGridViewTextBoxCell;
			if (myDataGridViewTextBoxCell == null)
			{
				throw new InvalidOperationException("Invalid CellTemplate.");
			}
			return myDataGridViewTextBoxCell;
		}
	}
}
