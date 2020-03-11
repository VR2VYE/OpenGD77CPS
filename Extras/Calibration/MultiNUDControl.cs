using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DMR
{
	public partial class CalibrationPowerControl : UserControl
	{
		private List<CalibrationNUDLabelControl> _controlsList = new List<CalibrationNUDLabelControl>();
		private int _rows = 1;
		private int _cols = 1;

		public string[] Names
		{
			get 
			{
				string[] d = new string[_controlsList.Count];
				for(int i=0;i<_controlsList.Count;i++)
				{
					d[i] = _controlsList[i].bandName;
				}
				return d;
			}
			set 
			{
				if (value.Length <= _rows * _cols)
				{
					for (int i = 0; i < value.Length; i++)
					{
						_controlsList[i].bandName = value[i];
					}
				}
				else
				{
					//throw(new Exception("Data length does not match rows * columns"));
				}
			}
		}

		public int[] Values
		{
			get
			{
				int [] d = new int[_controlsList.Count];
				for (int i = 0; i < _controlsList.Count; i++)
				{
					d[i] = _controlsList[i].bandValue;
				}
				return d;
			}
			set
			{
				if (value.Length <= _rows * _cols)
				{
					for (int i = 0; i < value.Length; i++)
					{
						_controlsList[i].bandValue = (byte)value[i];
					}
				}
				else
				{
					//throw(new Exception("Data length does not match rows * columns"));
				}
			}
		}

		public int Rows
		{
			get 
			{
				return _rows;
			}
			set
			{
				_rows = value;
				updateComponents();
			}
		}

		public int Cols
		{
			get
			{
				return _cols;
			}
			set
			{
				_cols = value;
				updateComponents();
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Bindable(true)]
		public override string Text
		{
			get
			{
				return this.groupBox1.Text;
			}
			set
			{
				this.groupBox1.Text = value;
			}
		}

		public string CtrlText
		{
			get
			{
				return this.groupBox1.Text;
			}
			set
			{
				this.groupBox1.Text = value;
			}
		}

		public CalibrationPowerControl()
		{

			InitializeComponent();
			updateComponents();

		}

		private void updateComponents()
		{
			if (_controlsList.Count != 0)
			{
				for (int i = 0; i < _controlsList.Count; i++)
				{
					this.groupBox1.Controls.Remove(_controlsList[i]);
				}
				_controlsList.Clear();
			}
			CalibrationNUDLabelControl ctl = null;// assign to null to stop the compiler complaining
			int xPos = groupBox1.Left + 10;
			int yPos = groupBox1.Top  + 15;
			for (int r = 0; r < _rows; r++)
			{
				xPos = 5;
				for (int c = 0; c < _cols; c++)
				{
					ctl = new CalibrationNUDLabelControl();
					ctl.bandName = "label_" + r + "_" + c;
					ctl.bandValue = ((byte)(0));
					ctl.Location = new System.Drawing.Point(xPos, yPos);
					ctl.Name = "calibrationNUDLabelControl1";
					xPos += ctl.Width;
					ctl.TabIndex = r;
					_controlsList.Add(ctl);
					this.groupBox1.Controls.Add(ctl);
				}
				yPos += ctl.Height;
			}
			groupBox1.Width = xPos+5;
			groupBox1.Height = yPos+15;
		}

	}
}
