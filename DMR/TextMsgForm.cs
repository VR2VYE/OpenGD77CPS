using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class TextMsgForm : DockContent, IDisp
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class TextMsg
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			private byte[] reserve;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			private byte[] msgLen;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			private byte[] reserve1;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4608)]
			private byte[] text;

			public byte[] Reserve
			{
				get
				{
					return this.reserve;
				}
			}

			public int this[int index]
			{
				get
				{
					if (index >= 32)
					{
						throw new ArgumentOutOfRangeException();
					}
					return this.msgLen[index];
				}
				set
				{
					if (index < 32)
					{
						this.msgLen[index] = (byte)value;
					}
				}
			}

			public TextMsg()
			{
				
				//base._002Ector();
				this.msgLen = new byte[32];
				this.reserve = new byte[8];
				this.reserve1 = new byte[32];
				this.text = new byte[4608];
			}

			public void RemoveAt(int index)
			{
				if (index < 32)
				{
					int i = 0;
					this[index] = 0;
					for (; i < 144; i++)
					{
						this.text[i + index * 144] = 255;
					}
				}
			}

			public void Insert(int index)
			{
				if (index < 32)
				{
					int i = 0;
					this[index] = 1;
					for (; i < 144; i++)
					{
						this.text[i + index * 144] = 255;
					}
				}
			}

			public void Clear()
			{
				for (int i = 0; i < 32; i++)
				{
					this[i] = 0;
				}
				for (int i = 0; i < this.text.Length; i++)
				{
					this.text[i] = 255;
				}
			}

			public string GetText(int index)
			{
				if (index >= 32)
				{
					return "";
				}
				int num = 0;
				if (this[index] == 1)
				{
					return "";
				}
				int num2 = this[index] - 1;
				byte[] array = new byte[num2];
				Array.Copy(this.text, num + index * 144, array, 0, array.Length);
				return Settings.smethod_25(array);
			}

			public void SetText(int index, string msg)
			{
				if (index < 32)
				{
					this.RemoveAt(index);
					this[index] = msg.Length + 1;
					byte[] array = Settings.smethod_23(msg);
					this[index] = array.Length + 1;
					Array.Copy(array, 0, this.text, 0 + index * 144, Math.Min(array.Length, 144));
				}
			}
		}

		public const int CNT_MSG = 32;

		private const int LEN_MSG = 144;

		//private IContainer components;

		private Button btnAdd;

		private Button btnDel;

		private DataGridView dgvMsg;

		private DataGridViewTextBoxColumn txtMessage;

		private SGTextBox txtContent;

		private CustomPanel pnlTextMsg;

		private TextMsgFormCustomPasteWndProc txt;

		public static TextMsg data;

		public TreeNode Node
		{
			get;
			set;
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
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			this.btnAdd = new Button();
			this.btnDel = new Button();
			this.dgvMsg = new DataGridView();
			this.txtMessage = new DataGridViewTextBoxColumn();
			this.pnlTextMsg = new CustomPanel();
			this.txtContent = new SGTextBox();
			((ISupportInitialize)this.dgvMsg).BeginInit();
			this.pnlTextMsg.SuspendLayout();
			base.SuspendLayout();
			this.btnAdd.Location = new Point(66, 41);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new Size(75, 23);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += this.btnAdd_Click;
			this.btnDel.Location = new Point(191, 41);
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new Size(75, 23);
			this.btnDel.TabIndex = 1;
			this.btnDel.Text = "Delete";
			this.btnDel.UseVisualStyleBackColor = true;
			this.btnDel.Click += this.zesOxyCmfw;
			this.dgvMsg.AllowUserToAddRows = false;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("Arial", 10f, FontStyle.Regular);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvMsg.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvMsg.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvMsg.Columns.AddRange(this.txtMessage);
			this.dgvMsg.Location = new Point(48, 85);
			this.dgvMsg.Name = "dgvMsg";
			this.dgvMsg.RowTemplate.Height = 23;
			this.dgvMsg.Size = new Size(700, 447);
			this.dgvMsg.TabIndex = 2;
			this.dgvMsg.RowPostPaint += this.dgvMsg_RowPostPaint;
			this.dgvMsg.CellEndEdit += this.dgvMsg_CellEndEdit;
			this.dgvMsg.EditingControlShowing += this.dgvMsg_EditingControlShowing;
			this.txtMessage.HeaderText = "Text Message";
			this.txtMessage.MaxInputLength = 50;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.Width = 650;
			this.pnlTextMsg.AutoScroll = true;
			this.pnlTextMsg.AutoSize = true;
			this.pnlTextMsg.Controls.Add(this.txtContent);
			this.pnlTextMsg.Controls.Add(this.dgvMsg);
			this.pnlTextMsg.Controls.Add(this.btnAdd);
			this.pnlTextMsg.Controls.Add(this.btnDel);
			this.pnlTextMsg.Dock = DockStyle.Fill;
			this.pnlTextMsg.Location = new Point(0, 0);
			this.pnlTextMsg.Name = "pnlTextMsg";
			this.pnlTextMsg.Size = new Size(796, 573);
			this.pnlTextMsg.TabIndex = 3;
			this.txtContent.InputString = null;
			this.txtContent.Location = new Point(349, 41);
			this.txtContent.MaxByteLength = 0;
			this.txtContent.Name = "txtContent";
			this.txtContent.Size = new Size(100, 21);
			this.txtContent.TabIndex = 3;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(796, 573);
			base.Controls.Add(this.pnlTextMsg);
			this.Font = new Font("Arial", 10f, FontStyle.Regular);
			base.Name = "TextMsgForm";
			this.Text = "Text Message";
			base.Load += this.TextMsgForm_Load;
			base.FormClosing += this.TextMsgForm_FormClosing;
			((ISupportInitialize)this.dgvMsg).EndInit();
			this.pnlTextMsg.ResumeLayout(false);
			this.pnlTextMsg.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public void SaveData()
		{
			try
			{
				int num = 0;
				int num2 = 0;
				string text = null;
				this.dgvMsg.EndEdit();
				for (num = 0; num < this.dgvMsg.Rows.Count; num++)
				{
					num2 = (int)this.dgvMsg.Rows[num].Tag;
					if (this.dgvMsg.Rows[num].Cells[0].Value != null)
					{
						text = this.dgvMsg.Rows[num].Cells[0].Value.ToString();
						if (string.IsNullOrEmpty(text))
						{
							TextMsgForm.data[num2] = 1;
						}
						else
						{
							TextMsgForm.data.SetText(num2, text);
						}
					}
					else
					{
						TextMsgForm.data[num2] = 1;
					}
				}
				TextMsgForm.data.Reserve[0] = (byte)this.dgvMsg.Rows.Count;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Source);
			}
		}

		public void DispData()
		{
			try
			{
				int num = 0;
				int num2 = 0;
				this.method_1();
				this.dgvMsg.Rows.Clear();
				for (num = 0; num < 32; num++)
				{
					if (TextMsgForm.data[num] == 1)
					{
						num2 = this.dgvMsg.Rows.Add();
						this.dgvMsg.Rows[num2].Tag = num;
						this.dgvMsg.Rows[num2].Cells[0].Value = "";
					}
					else if (TextMsgForm.data[num] > 1 && TextMsgForm.data[num] <= 145)
					{
						num2 = this.dgvMsg.Rows.Add();
						this.dgvMsg.Rows[num2].Tag = num;
						this.dgvMsg.Rows[num2].Cells[0].Value = TextMsgForm.data.GetText(num);
					}
				}
				this.method_2();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Source);
			}
		}

		public void RefreshName()
		{
		}

		public TextMsgForm()
		{
			
			this.txt = new TextMsgFormCustomPasteWndProc();
			this.InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!
			base.Scale(Settings.smethod_6());
		}

		private void method_1()
		{
			this.txtMessage.MaxInputLength = 144;
			this.txtMessage.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			this.dgvMsg.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvMsg.AllowUserToDeleteRows = false;
			this.dgvMsg.AllowUserToAddRows = false;
		}

		private void TextMsgForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);
			this.txtContent.MaxByteLength = 10;
			this.txtContent.Visible = false;
			this.dgvMsg.Controls.Add(this.txtContent);
			this.DispData();
		}

		private void TextMsgForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			for (num = 0; num < this.dgvMsg.Rows.Count && num2 == (int)this.dgvMsg.Rows[num].Tag; num++)
			{
				num2++;
			}
			this.dgvMsg.Rows.Insert(num, 1);
			this.dgvMsg.Rows[num].Tag = num2;
			this.dgvMsg.Rows[num].Cells[0].Value = "";
			TextMsgForm.data[num2] = 0;
			TextMsgForm.data.SetText(num2, "");
			this.method_2();
			((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType());
		}

		private void zesOxyCmfw(object sender, EventArgs e)
		{
			int index = this.dgvMsg.CurrentRow.Index;
			int num = (int)this.dgvMsg.Rows[index].Tag;
			this.dgvMsg.Rows.RemoveAt(index);
			TextMsgForm.data[num] = 0;
			ButtonForm.data1.ClearByMessage(num);
			this.method_2();
			((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType());
		}

		private void method_2()
		{
			int count = this.dgvMsg.Rows.Count;
			this.btnAdd.Enabled = true;
			this.btnDel.Enabled = true;
			if (count == 32)
			{
				this.btnAdd.Enabled = false;
			}
			if (count == 0)
			{
				this.btnDel.Enabled = false;
			}
		}

		private void dgvMsg_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			if (e.RowIndex >= this.dgvMsg.FirstDisplayedScrollingRowIndex)
			{
				using (SolidBrush brush = new SolidBrush(this.dgvMsg.RowHeadersDefaultCellStyle.ForeColor))
				{
					StringFormat stringFormat = new StringFormat();
					stringFormat.Alignment = StringAlignment.Center;
					stringFormat.LineAlignment = StringAlignment.Center;
					string s = ((int)this.dgvMsg.Rows[e.RowIndex].Tag + 1).ToString();
					Rectangle r = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, this.dgvMsg.RowHeadersWidth, e.RowBounds.Height);
					e.Graphics.DrawString(s, e.InheritedRowStyle.Font, brush, r, stringFormat);
				}
			}
		}

		private void dgvMsg_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			if (this.dgvMsg.CurrentCell.ColumnIndex == 0)
			{
				e.Control.KeyPress += TextMsgForm.Content_KeyPress;
				this.txt.AssignHandle(e.Control.Handle);
			}
		}

		private void dgvMsg_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				this.txt.ReleaseHandle();
			}
		}

		public static void Content_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			if (textBox != null && !char.IsControl(e.KeyChar))
			{
				int byteCount = Encoding.GetEncoding(936).GetByteCount(textBox.Text + e.KeyChar.ToString());
				int byteCount2 = Encoding.GetEncoding(936).GetByteCount(textBox.SelectedText);
				if (byteCount - byteCount2 > 144)
				{
					e.Handled = true;
				}
			}
		}

		static TextMsgForm()
		{
			
			TextMsgForm.data = new TextMsg();
		}
	}
}
