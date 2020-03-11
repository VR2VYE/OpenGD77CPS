using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class DtmfContactForm : DockContent, IDisp
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct DtmfContactOne
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			private byte[] name;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			private byte[] code;

			public string Name
			{
				get
				{
					return Settings.smethod_25(this.name);
				}
				set
				{
					byte[] array = Settings.smethod_23(value);
					this.name.Fill((byte)255);
					Array.Copy(array, 0, this.name, 0, Math.Min(array.Length, this.name.Length));
				}
			}

			public string Code
			{
				get
				{
					int num = 0;
					StringBuilder stringBuilder = new StringBuilder(16);
					for (num = 0; num < 16 && this.code[num] < 16; num++)
					{
						stringBuilder.Append("0123456789ABCD*#\b"[this.code[num]]);
					}
					return stringBuilder.ToString();
				}
				set
				{
					int num = 0;
					int num2 = 0;
					for (num = 0; num < 16; num++)
					{
						this.code[num] = 255;
					}
					for (num = 0; num < value.Length; num++)
					{
						num2 = "0123456789ABCD*#\b".IndexOf(value[num]);
						if (num2 < 0)
						{
							break;
						}
						this.code[num] = Convert.ToByte(num2);
					}
				}
			}

			public bool Valid
			{
				get
				{
					return !string.IsNullOrEmpty(this.Name);
				}
			}

			public DtmfContactOne(int index)
			{
				
				this.name = new byte[16];
				this.code = new byte[16];
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class DtmfContact
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			private DtmfContactOne[] contact;

			public DtmfContactOne this[int index]
			{
				get
				{
					if (index >= 32)
					{
						throw new ArgumentOutOfRangeException();
					}
					return this.contact[index];
				}
				set
				{
					if (index < 32)
					{
						this.contact[index] = value;
					}
				}
			}

			public string GetName(int index)
			{
				if (index < 32)
				{
					return this[index].Name;
				}
				return "";
			}

			public void SetName(int index, string name)
			{
				if (index < 32)
				{
					this.contact[index].Name = name;
				}
			}

			public string GetCode(int index)
			{
				if (index < 32)
				{
					return this[index].Code;
				}
				return "";
			}

			public void SetCode(int index, string code)
			{
				if (index < 32)
				{
					this.contact[index].Code = code;
				}
			}

			public void RemoveAt(int index)
			{
				if (index < 32)
				{
					this.contact[index].Name = "";
					this.contact[index].Code = "";
				}
			}

			public void Insert(int index, string name, string code)
			{
				if (index < 32)
				{
					this.contact[index].Name = name;
					this.contact[index].Code = code;
				}
			}

			public void Clear()
			{
				int num = 0;
				for (num = 0; num < 32; num++)
				{
					this.RemoveAt(num);
				}
			}

			public bool Valid(int index)
			{
				if (index < 32)
				{
					return this[index].Valid;
				}
				return false;
			}

			public DtmfContact()
			{
				
				//base._002Ector();
				int num = 0;
				this.contact = new DtmfContactOne[32];
				for (num = 0; num < 32; num++)
				{
					this.contact[num] = new DtmfContactOne(num);
				}
			}
		}

		public const int CNT_DTMF_CONTACT = 32;

		private const int LEN_DTMF_NAME = 16;

		private const int LEN_DTMF_CODE = 16;

		public const string SZ_DTMF_CODE = "0123456789ABCD*#\b";

		public static DtmfContact data;

		//private IContainer components;

		private DataGridView dgvContact;

		private Button btnDel;

		private Button btnAdd;

		private MyDataGridViewTextBoxColumn txtName;

		private DataGridViewTextBoxColumn txtCode;

		private CustomPanel pnlDtmfContact;

		public TreeNode Node
		{
			get;
			set;
		}

		public void SaveData()
		{
			try
			{
				int num = 0;
				int num2 = 0;
				this.dgvContact.EndEdit();
				DtmfContactForm.data.Clear();
				for (num = 0; num < this.dgvContact.Rows.Count; num++)
				{
					num2 = Convert.ToInt32(this.dgvContact.Rows[num].Tag);
					DtmfContactForm.data.SetName(num2, this.dgvContact.Rows[num].Cells[0].Value.ToString());
					DtmfContactForm.data.SetCode(num2, this.dgvContact.Rows[num].Cells[1].Value.ToString());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void DispData()
		{
			try
			{
				int i = 0;
				int num = 0;
				this.dgvContact.Rows.Clear();
				for (; i < 32; i++)
				{
					if (DtmfContactForm.data[i].Valid)
					{
						num = this.dgvContact.Rows.Add();
						this.dgvContact.Rows[num].Tag = i;
						this.dgvContact.Rows[num].Cells[0].Value = DtmfContactForm.data.GetName(i);
						this.dgvContact.Rows[num].Cells[1].Value = DtmfContactForm.data.GetCode(i);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void RefreshName()
		{
		}

		public DtmfContactForm()
		{
			
			//base._002Ector();
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void method_0()
		{
			this.txtName.MaxByteLength = 15;
		}

		private void DtmfContactForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);
			this.method_0();
			this.DispData();
		}

		private void DtmfContactForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
		}

		private void sscOhyqVop(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			for (num2 = 0; num2 < this.dgvContact.Rows.Count && num == (int)this.dgvContact.Rows[num2].Tag; num2++)
			{
				num++;
			}
			this.dgvContact.Rows.Insert(num2, 1);
			this.dgvContact.Rows[num2].Tag = num;
			this.dgvContact.Rows[num2].Cells[0].Value = "DTMF-" + (num + 1);
			this.dgvContact.Rows[num2].Cells[1].Value = "12345678";
			this.method_1();
		}

		private void btnDel_Click(object sender, EventArgs e)
		{
			int index = this.dgvContact.CurrentRow.Index;
			int contactIndex = (int)this.dgvContact.Rows[index].Tag;
			this.dgvContact.Rows.RemoveAt(index);
			ButtonForm.data1.ClearByDtmfContact(contactIndex);
			this.method_1();
		}

		private void method_1()
		{
			int count = this.dgvContact.Rows.Count;
			this.btnAdd.Enabled = (count < 32);
			this.btnDel.Enabled = (count != 0);
		}

		private void method_2(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			if (e.RowIndex >= this.dgvContact.FirstDisplayedScrollingRowIndex)
			{
				using (SolidBrush brush = new SolidBrush(this.dgvContact.RowHeadersDefaultCellStyle.ForeColor))
				{
					string s = (Convert.ToInt32(this.dgvContact.Rows[e.RowIndex].Tag) + 1).ToString();
					e.Graphics.DrawString(s, e.InheritedRowStyle.Font, brush, (float)(e.RowBounds.Location.X + 15), (float)(e.RowBounds.Location.Y + 5));
				}
			}
		}

		private void dgvContact_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			DataGridView dataGridView = (DataGridView)sender;
			if (e.Control is DataGridViewTextBoxEditingControl)
			{
				DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = (DataGridViewTextBoxEditingControl)e.Control;
				if (dataGridView.CurrentCell.ColumnIndex == 1)
				{
					dataGridViewTextBoxEditingControl.KeyPress -= Settings.smethod_57;
					dataGridViewTextBoxEditingControl.KeyPress += Settings.smethod_57;
					dataGridViewTextBoxEditingControl.CharacterCasing = CharacterCasing.Upper;
					dataGridViewTextBoxEditingControl.MaxLength = 16;
				}
				else
				{
					int columnIndex = dataGridView.CurrentCell.ColumnIndex;
				}
			}
		}

		private void dgvContact_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			DataGridView dataGridView = (DataGridView)sender;
			if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
			{
				e.Cancel = true;
				dataGridView.CancelEdit();
			}
		}

		protected override void Dispose(bool disposing)
		{
            /*
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
             * */
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			this.dgvContact = new DataGridView();
			this.txtName = new MyDataGridViewTextBoxColumn();
			this.txtCode = new DataGridViewTextBoxColumn();
			this.btnDel = new Button();
			this.btnAdd = new Button();
			this.pnlDtmfContact = new CustomPanel();
			((ISupportInitialize)this.dgvContact).BeginInit();
			this.pnlDtmfContact.SuspendLayout();
			base.SuspendLayout();
			this.dgvContact.AllowUserToAddRows = false;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("Arial", 10f, FontStyle.Regular);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvContact.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvContact.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvContact.Columns.AddRange(this.txtName, this.txtCode);
			this.dgvContact.Location = new Point(43, 69);
			this.dgvContact.Name = "dgvContact";
			this.dgvContact.RowTemplate.Height = 23;
			this.dgvContact.Size = new Size(367, 401);
			this.dgvContact.TabIndex = 2;
			this.dgvContact.CellValidating += this.dgvContact_CellValidating;
			this.dgvContact.EditingControlShowing += this.dgvContact_EditingControlShowing;
			this.txtName.HeaderText = "Name";
			this.txtName.MaxByteLength = 2147483647;
			this.txtName.Name = "txtName";
			this.txtName.Width = 150;
			this.txtCode.HeaderText = "Number";
			this.txtCode.Name = "txtCode";
			this.txtCode.Width = 150;
			this.btnDel.Location = new Point(251, 28);
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new Size(75, 23);
			this.btnDel.TabIndex = 1;
			this.btnDel.Text = "Delete";
			this.btnDel.UseVisualStyleBackColor = true;
			this.btnDel.Click += this.btnDel_Click;
			this.btnAdd.Location = new Point(126, 28);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new Size(75, 23);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += this.sscOhyqVop;
			this.pnlDtmfContact.AutoScroll = true;
			this.pnlDtmfContact.AutoSize = true;
			this.pnlDtmfContact.Controls.Add(this.dgvContact);
			this.pnlDtmfContact.Controls.Add(this.btnAdd);
			this.pnlDtmfContact.Controls.Add(this.btnDel);
			this.pnlDtmfContact.Dock = DockStyle.Fill;
			this.pnlDtmfContact.Location = new Point(0, 0);
			this.pnlDtmfContact.Name = "pnlDtmfContact";
			this.pnlDtmfContact.Size = new Size(454, 498);
			this.pnlDtmfContact.TabIndex = 3;
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(454, 498);
			base.Controls.Add(this.pnlDtmfContact);
			this.Font = new Font("Arial", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Name = "DtmfContactForm";
			this.Text = "DTMF Contact";
			base.Load += this.DtmfContactForm_Load;
			base.FormClosing += this.DtmfContactForm_FormClosing;
			((ISupportInitialize)this.dgvContact).EndInit();
			this.pnlDtmfContact.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		static DtmfContactForm()
		{
			
			DtmfContactForm.data = new DtmfContact();
		}
	}
}
