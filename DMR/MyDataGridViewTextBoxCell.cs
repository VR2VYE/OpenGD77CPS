using System;
using System.Windows.Forms;

namespace DMR
{
	public class MyDataGridViewTextBoxCell : DataGridViewTextBoxCell
	{
		private int maxByteLength;

		public override Type EditType
		{
			get
			{
				return typeof(MyDataGridViewTextBoxEditingControl);
			}
		}

		public int MaxByteLength
		{
			get
			{
				return this.maxByteLength;
			}
			set
			{
				this.maxByteLength = value;
				if (this.method_1(base.RowIndex))
				{
					this.method_0().MaxByteLength = value;
				}
			}
		}

		public MyDataGridViewTextBoxCell()
		{
			
			//base._002Ector();
		}

		private MyDataGridViewTextBoxEditingControl method_0()
		{
			return base.DataGridView.EditingControl as MyDataGridViewTextBoxEditingControl;
		}

		private bool method_1(int int_0)
		{
			if (int_0 != -1 && base.DataGridView != null)
			{
				MyDataGridViewTextBoxEditingControl myDataGridViewTextBoxEditingControl = base.DataGridView.EditingControl as MyDataGridViewTextBoxEditingControl;
				if (myDataGridViewTextBoxEditingControl != null)
				{
					return int_0 == ((IDataGridViewEditingControl)myDataGridViewTextBoxEditingControl).EditingControlRowIndex;
				}
				return false;
			}
			return false;
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			MyDataGridViewTextBoxEditingControl myDataGridViewTextBoxEditingControl = base.DataGridView.EditingControl as MyDataGridViewTextBoxEditingControl;
			if (myDataGridViewTextBoxEditingControl != null)
			{
				myDataGridViewTextBoxEditingControl.MaxByteLength = this.MaxByteLength;
			}
		}

		public override object Clone()
		{
			MyDataGridViewTextBoxCell myDataGridViewTextBoxCell = base.Clone() as MyDataGridViewTextBoxCell;
			if (myDataGridViewTextBoxCell != null)
			{
				myDataGridViewTextBoxCell.MaxByteLength = this.MaxByteLength;
			}
			return myDataGridViewTextBoxCell;
		}
	}
}
