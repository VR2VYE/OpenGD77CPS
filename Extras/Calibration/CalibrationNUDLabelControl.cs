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
	public partial class CalibrationNUDLabelControl : UserControl
	{
		public string bandName
		{
			get { return label1.Text; }
			set
			{ 
				label1.Text = value; 
			}
		}
		public byte bandValue
		{
			get { return (byte)numericUpDown1.Value; }
			set { numericUpDown1.Value = value; }
		}

		public CalibrationNUDLabelControl()
		{
			InitializeComponent();
		}
	}
}
