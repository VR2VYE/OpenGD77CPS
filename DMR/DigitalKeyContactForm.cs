using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class DigitalKeyContactForm : DockContent, IDisp
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class NumKeyContact
		{
			private ushort index;

			private ushort reserve;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
			private ushort[] contact;

			public ushort this[int index]
			{
				get
				{
					return this.contact[index];
				}
				set
				{
					this.method_0(index, value);
					this.contact[index] = value;
				}
			}

			private void method_0(int int_0, int int_1)
			{
				ushort num = (ushort)(~(1 << int_0));
				this.index &= num;
				if (int_1 != 0)
				{
					this.index |= (ushort)(1 << int_0);
				}
			}

			private bool method_1(int int_0)
			{
				try
				{
					BitArray bitArray = new BitArray(BitConverter.GetBytes(this.index));
					return bitArray[int_0];
				}
				catch
				{
					return false;
				}
			}

			public NumKeyContact()
			{
				
				//base._002Ector();
				this.contact = new ushort[10];
			}

			public void Verify()
			{
				int num = 0;
				int num2 = 0;
				for (num = 0; num < this.contact.Length; num++)
				{
					if (this.method_1(num))
					{
						num2 = this.contact[num] - 1;
						if (!ContactForm.data.DataIsValid(num2))
						{
							this.contact[num] = 0;
							Settings.smethod_17(ref this.index, num, 1);
						}
					}
				}
			}
		}

		private const int CNT_NUM_KEY_CONTACT = 10;

		public const string SZ_DIGIT_KEY_NAME = "DigitKey";

		private static string SZ_DIGIT_KEY_TEXT;

		private Dictionary<string, string> dicCom;

		public static NumKeyContact data;
		private DataGridViewComboBoxColumn cmbContact;

		//private IContainer components;

		private DataGridView dgvContact;

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
				this.dgvContact.EndEdit();
				for (num = 0; num < this.dgvContact.Rows.Count; num++)
				{
					if (this.dgvContact.Rows[num].Cells[0].Value != null)
					{
						DigitalKeyContactForm.data[num] = (ushort)(int)this.dgvContact.Rows[num].Cells[0].Value;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void DispData()
		{
			this.method_0();
			try
			{
				int num = 0;
				for (num = 0; num < this.dgvContact.Rows.Count; num++)
				{
					this.dgvContact.Rows[num].Cells[0].Value = (int)DigitalKeyContactForm.data[num];
				}
				this.dgvContact.EndEdit();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void RefreshName()
		{
		}

		public DigitalKeyContactForm()
		{
			
			this.dicCom = new Dictionary<string, string>();
			//base._002Ector();
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void method_0()
		{
			int i = 0;
			string text = "";
			this.dgvContact.RowCount = 10;
			this.cmbContact.Items.Clear();
			this.cmbContact.Items.Add(new NameValuePair(Settings.SZ_NONE, 0));
			for (i = 0; i < 1024; i++)
			{
				if (ContactForm.data.DataIsValid(i))
				{
					text = ContactForm.data[i].Name;
					this.cmbContact.Items.Add(new NameValuePair(text, i + 1));
				}
			}
			this.cmbContact.DisplayMember = "Text";
			this.cmbContact.ValueMember = "Value";
			for (i = 0; i < this.dgvContact.Rows.Count; i++)
			{
				this.dgvContact.Rows[i].HeaderCell.Value = DigitalKeyContactForm.SZ_DIGIT_KEY_TEXT + i.ToString();
			}
		}

		public static void RefreshCommonLang()
		{
			string name = typeof(DigitalKeyContactForm).Name;
			Settings.smethod_77("DigitKey", ref DigitalKeyContactForm.SZ_DIGIT_KEY_TEXT, name);
		}

		private void DigitalKeyContactForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);
			this.DispData();
		}

		private void DigitalKeyContactForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
		}

		private void method_1(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			try
			{
				DataGridView dataGridView = sender as DataGridView;
				if (e.RowIndex >= dataGridView.FirstDisplayedScrollingRowIndex)
				{
					using (SolidBrush brush = new SolidBrush(dataGridView.RowHeadersDefaultCellStyle.ForeColor))
					{
						string s = DigitalKeyContactForm.SZ_DIGIT_KEY_TEXT + e.RowIndex.ToString();
						e.Graphics.DrawString(s, e.InheritedRowStyle.Font, brush, (float)(e.RowBounds.Location.X + 15), (float)(e.RowBounds.Location.Y + 5));
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void dgvContact_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			DataGridView dataGridView = sender as DataGridView;
			if (e.Context == DataGridViewDataErrorContexts.Formatting && dataGridView != null)
			{
				dataGridView[e.ColumnIndex, e.RowIndex].Value = 0;
				e.Cancel = true;
			}
		}

		protected override void Dispose(bool disposing)
		{
            /*
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}*/
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dgvContact = new System.Windows.Forms.DataGridView();
			this.cmbContact = new System.Windows.Forms.DataGridViewComboBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dgvContact)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvContact
			// 
			this.dgvContact.AllowUserToAddRows = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvContact.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvContact.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvContact.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cmbContact});
			this.dgvContact.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.dgvContact.Location = new System.Drawing.Point(38, 12);
			this.dgvContact.Name = "dgvContact";
			this.dgvContact.RowHeadersWidth = 150;
			this.dgvContact.RowTemplate.Height = 23;
			this.dgvContact.Size = new System.Drawing.Size(456, 289);
			this.dgvContact.TabIndex = 16;
			// 
			// cmbContact
			// 
			this.cmbContact.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			this.cmbContact.HeaderText = "Menu";
			this.cmbContact.Name = "cmbContact";
			this.cmbContact.Width = 300;
			// 
			// DigitalKeyContactForm
			// 
			this.ClientSize = new System.Drawing.Size(570, 345);
			this.Controls.Add(this.dgvContact);
			this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "DigitalKeyContactForm";
			this.Text = "Number Key Quick Contact Access";
			((System.ComponentModel.ISupportInitialize)(this.dgvContact)).EndInit();
			this.ResumeLayout(false);

		}

		static DigitalKeyContactForm()
		{
			
			DigitalKeyContactForm.SZ_DIGIT_KEY_TEXT = "Number Key";
			DigitalKeyContactForm.data = new NumKeyContact();
		}
	}
}
