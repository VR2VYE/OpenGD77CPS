using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class EncryptForm : DockContent, IDisp
	{
		private enum EncryptType
		{
			None,
			Basic,
			Enhanced
		}

		private enum KeyLen
		{
			Length32,
			Length64,
			Length40
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class Encrypt : IVerify<Encrypt>, IData
		{
			private byte type;

			private byte keyLen;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			private byte[] keyIndex;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			private byte[] reserve;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
			private byte[] keyList;

			public int Type
			{
				get
				{
					return this.type;
				}
				set
				{
					this.type = Convert.ToByte(value);
				}
			}

			public int KeyLen
			{
				get
				{
					return this.keyLen;
				}
				set
				{
					this.keyLen = Convert.ToByte(value);
				}
			}

			public bool this[int index]
			{
				get
				{
					if (index < 16)
					{
						BitArray bitArray = new BitArray(this.keyIndex);
						return bitArray[index];
					}
					return false;
				}
				set
				{
					if (index < 16)
					{
						BitArray bitArray = new BitArray(this.keyIndex);
						bitArray[index] = value;
						bitArray.CopyTo(this.keyIndex, 0);
					}
				}
			}

			public int Count
			{
				get
				{
					return 16;
				}
			}

			public string Format
			{
				get
				{
					return "";
				}
			}

			public bool ListIsEmpty
			{
				get
				{
					int num = 0;
					while (true)
					{
						if (num < this.Count)
						{
							if (this.DataIsValid(num))
							{
								break;
							}
							num++;
							continue;
						}
						return true;
					}
					return false;
				}
			}

			public Encrypt()
			{
				
				//base._002Ector();
				this.keyIndex = new byte[2];
				this.reserve = new byte[4];
				this.keyList = new byte[128];
			}

			public void RemoveAt(int index)
			{
				int i = 0;
				this[index] = false;
				for (; i < 8; i++)
				{
					this.keyList[i + index * 8] = 255;
				}
			}

			public void Insert(int index)
			{
				int i = 0;
				this[index] = true;
				for (; i < 8; i++)
				{
					this.keyList[i + index * 8] = Convert.ToByte("53474C3953474C39".Substring(i * 2, 2));
				}
			}

			public void Clear()
			{
				for (int i = 0; i < this.keyIndex.Length; i++)
				{
					this.keyIndex[i] = 0;
				}
				for (int i = 0; i < this.keyList.Length; i++)
				{
					this.keyList[i] = 255;
				}
			}

			public string GetKey(int index)
			{
				if (index >= 16)
				{
					return "";
				}
				int i = 0;
				StringBuilder stringBuilder = new StringBuilder(16);
				for (; i < 8; i++)
				{
					byte b = this.keyList[i + index * 8];
					stringBuilder.Append(b.ToString("X2"));
				}
				if (EncryptForm.data.KeyLen == 0)
				{
					return stringBuilder.ToString().Substring(0, 8);
				}
				return stringBuilder.ToString();
			}

			public void SetKey(int index, string key)
			{
				if (index < 16)
				{
					for (int i = 0; i < key.Length / 2; i++)
					{
						this.keyList[i + index * 8] = Convert.ToByte(key.Substring(i * 2, 2), 16);
					}
					if (key.Length == 8)
					{
						Array.Copy(this.keyList, index * 8, this.keyList, index * 8 + 4, 4);
					}
				}
			}

			public int GetMinIndex()
			{
				int num = 0;
				for (num = 0; num < this.Count; num++)
				{
				}
				return -1;
			}

			public bool DataIsValid(int index)
			{
				if (index < 16)
				{
					BitArray bitArray = new BitArray(this.keyIndex);
					return bitArray[index];
				}
				return false;
			}

			public void SetIndex(int index, int value)
			{
				if (value == 0)
				{
					this.SetName(index, "");
				}
			}

			public void ClearIndex(int index)
			{
				this.SetName(index, "");
			}

			public string GetMinName(TreeNode node)
			{
				return "";
			}

			public void SetName(int index, string text)
			{
			}

			public string GetName(int index)
			{
				return this.GetKey(index);
			}

			public void Default(int index)
			{
			}

			public void Paste(int from, int to)
			{
			}

			public void Verify(Encrypt def)
			{
				if (!Enum.IsDefined(typeof(EncryptType), (int)this.type))
				{
					this.type = def.type;
				}
				if (!Enum.IsDefined(typeof(KeyLen), (int)this.keyLen))
				{
					this.keyLen = def.keyLen;
				}
			}
		}

		private const int CNT_KEY = 16;

		private const int CNT_KEY_INDEX = 2;

		private const int SPACE_PER_KEY = 8;

		private const int LEN_KEY_32BIT = 8;

		private const int LEN_KEY_64BIT = 16;

		public const string SZ_ENCRYPT_TYPE_NAME = "EncryptType";

		private const string DEF_KEY = "53474C3953474C39";

		private static readonly string[] SZ_ENCRYPT_TYPE;

		private static readonly string[] SZ_KEY_LEN;

		public static Encrypt DefaultEncrypt;

		public static Encrypt data;

		//private IContainer components;

		private Label lblType;

		private ComboBox cmbType;

		private Label lblKeyLen;

		private ComboBox cmbKeyLen;

		private DataGridView dgvKey;

		private Button btnDel;

		private Button btnAdd;

		private DataGridViewTextBoxColumn txtKey;

		private CustomPanel pnlEncrypt;

		public TreeNode Node
		{
			get;
			set;
		}

		public EncryptForm()
		{
			
			//base._002Ector();
			this.InitializeComponent();
			base.Scale(Settings.smethod_6());
		}

        int _003CPreKeyLen_003Ek__BackingField;
		[CompilerGenerated]
		private int method_0()
		{
			return this._003CPreKeyLen_003Ek__BackingField;
		}

		[CompilerGenerated]
		private void method_1(int value)
		{
			this._003CPreKeyLen_003Ek__BackingField = value;
		}

		public void SaveData()
		{
			int i = 0;
			int num = 0;
			string text = null;
			EncryptForm.data.Type = (byte)this.cmbType.SelectedIndex;
			EncryptForm.data.KeyLen = (byte)this.cmbKeyLen.SelectedIndex;
			this.dgvKey.EndEdit();
			EncryptForm.data.Clear();
			for (; i < this.dgvKey.Rows.Count; i++)
			{
				num = (int)this.dgvKey.Rows[i].Tag;
				EncryptForm.data[num] = true;
				text = this.dgvKey.Rows[i].Cells[0].Value.ToString();
				EncryptForm.data.SetKey(num, text);
			}
		}

		public void DispData()
		{
			int num = 0;
			int num2 = 0;
			this.cmbType.SelectedIndex = EncryptForm.data.Type;
			this.cmbKeyLen.SelectedIndex = EncryptForm.data.KeyLen;
			num2 = ((this.cmbKeyLen.SelectedIndex != 1) ? 8 : 16);
			this.txtKey.MaxInputLength = num2;
			this.dgvKey.Rows.Clear();
			for (num = 0; num < 16; num++)
			{
				if (EncryptForm.data[num])
				{
					int index = this.dgvKey.Rows.Add();
					this.dgvKey.Rows[index].Tag = num;
					this.dgvKey.Rows[index].Cells[0].Value = EncryptForm.data.GetKey(num).Substring(0, num2);
				}
			}
			this.method_3();
			this.RefreshByUserMode();
		}

		public void RefreshByUserMode()
		{
			bool flag = Settings.getUserExpertSettings() == Settings.UserMode.Expert;
			this.lblType.Enabled &= flag;
			this.cmbType.Enabled &= flag;
			this.lblKeyLen.Enabled &= flag;
			this.cmbKeyLen.Enabled &= flag;
			this.btnAdd.Enabled &= flag;
			this.btnDel.Enabled &= flag;
			this.dgvKey.Enabled &= flag;
		}

		public void RefreshName()
		{
		}

		private void method_2()
		{
			DataGridViewTextBoxColumn dataGridViewTextBoxColumn = this.dgvKey.Columns[0] as DataGridViewTextBoxColumn;
			dataGridViewTextBoxColumn.MaxInputLength = 8;
			dataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			Settings.smethod_37(this.cmbType, EncryptForm.SZ_ENCRYPT_TYPE);
		}

		public static void RefreshCommonLang()
		{
			string name = typeof(EncryptForm).Name;
			Settings.smethod_78("EncryptType", EncryptForm.SZ_ENCRYPT_TYPE, name);
		}

		private void EncryptForm_Load(object sender, EventArgs e)
		{
			try
			{
				Settings.smethod_59(base.Controls);
				Settings.smethod_68(this);
				this.method_2();
				this.DispData();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void EncryptForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				this.SaveData();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			string text = null;
			for (num = 0; num < this.dgvKey.Rows.Count && num2 == (int)this.dgvKey.Rows[num].Tag; num++)
			{
				num2++;
			}
			this.dgvKey.Rows.Insert(num, 1);
			this.dgvKey.Rows[num].Tag = num2;
			text = ((this.cmbKeyLen.SelectedIndex != 0) ? "53474C3953474C39" : "53474C3953474C39".Substring(8));
			this.dgvKey.Rows[num].Cells[0].Value = text;
			this.SaveData();
			this.method_3();
			((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType());
		}

		private void btnDel_Click(object sender, EventArgs e)
		{
			int index = this.dgvKey.CurrentRow.Index;
			int keyIndex = (int)this.dgvKey.Rows[index].Tag;
			this.dgvKey.Rows.RemoveAt(index);
			ChannelForm.data.ClearByEncrypt(keyIndex);
			this.method_3();
			((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType());
		}

		private void method_3()
		{
			int selectedIndex = this.cmbType.SelectedIndex;
			int count = this.dgvKey.Rows.Count;
			this.btnAdd.Enabled = true;
			this.btnDel.Enabled = true;
			if (count == 16 || selectedIndex == 0)
			{
				this.btnAdd.Enabled = false;
			}
			if (count != 0 && selectedIndex != 0)
			{
				return;
			}
			this.btnDel.Enabled = false;
		}

		private void cmbKeyLen_SelectedIndexChanged(object sender, EventArgs e)
		{
			int num = 0;
			int selectedIndex = this.cmbKeyLen.SelectedIndex;
			if (this.method_0() != selectedIndex)
			{
				this.method_1(selectedIndex);
				if (selectedIndex == 0)
				{
					this.txtKey.MaxInputLength = 8;
					for (num = 0; num < this.dgvKey.Rows.Count; num++)
					{
						string text = this.dgvKey.Rows[num].Cells[0].Value as string;
						text = text.Substring(0, 8);
						this.dgvKey.Rows[num].Cells[0].Value = text;
					}
				}
				else
				{
					this.txtKey.MaxInputLength = 16;
					for (num = 0; num < this.dgvKey.Rows.Count; num++)
					{
						string text2 = this.dgvKey.Rows[num].Cells[0].Value as string;
						text2 += text2;
						this.dgvKey.Rows[num].Cells[0].Value = text2;
					}
				}
			}
		}

		private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbType.SelectedIndex == 0)
			{
				this.cmbKeyLen.Enabled = false;
				this.dgvKey.Enabled = false;
				this.btnAdd.Enabled = false;
				this.btnDel.Enabled = false;
			}
			else
			{
				this.cmbKeyLen.Enabled = true;
				this.dgvKey.Enabled = true;
				this.btnAdd.Enabled = true;
				this.btnDel.Enabled = true;
				this.method_3();
			}
		}

		private void dgvKey_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			if (e.RowIndex >= this.dgvKey.FirstDisplayedScrollingRowIndex)
			{
				using (SolidBrush brush = new SolidBrush(this.dgvKey.RowHeadersDefaultCellStyle.ForeColor))
				{
					string s = (Convert.ToInt32(this.dgvKey.Rows[e.RowIndex].Tag) + 1).ToString();
					e.Graphics.DrawString(s, e.InheritedRowStyle.Font, brush, (float)(e.RowBounds.Location.X + 15), (float)(e.RowBounds.Location.Y + 5));
				}
			}
		}

		private void cmbKeyLen_Enter(object sender, EventArgs e)
		{
			this.method_1(this.cmbKeyLen.SelectedIndex);
		}

		private void dgvKey_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			if (e.Control is DataGridViewTextBoxEditingControl)
			{
				DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = (DataGridViewTextBoxEditingControl)e.Control;
				dataGridViewTextBoxEditingControl.KeyPress -= Settings.smethod_58;
				dataGridViewTextBoxEditingControl.KeyPress += Settings.smethod_58;
				dataGridViewTextBoxEditingControl.CharacterCasing = CharacterCasing.Upper;
			}
		}

		private void dgvKey_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			DataGridView dataGridView = (DataGridView)sender;
			if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
			{
				e.Cancel = true;
				dataGridView.CancelEdit();
				dataGridView.EndEdit();
			}
		}

		private void dgvKey_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
			DataGridView dataGridView = (DataGridView)sender;
			int maxInputLength = ((DataGridViewTextBoxColumn)this.dgvKey.Columns[e.ColumnIndex]).MaxInputLength;
			object value = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
			if (value != null)
			{
				string text = value.ToString();
				if (text.Length < maxInputLength)
				{
					text = text.PadRight(maxInputLength, 'F');
					dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = text;
				}
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
			this.lblType = new Label();
			this.cmbType = new ComboBox();
			this.lblKeyLen = new Label();
			this.cmbKeyLen = new ComboBox();
			this.dgvKey = new DataGridView();
			this.txtKey = new DataGridViewTextBoxColumn();
			this.btnDel = new Button();
			this.btnAdd = new Button();
			this.pnlEncrypt = new CustomPanel();
			((ISupportInitialize)this.dgvKey).BeginInit();
			this.pnlEncrypt.SuspendLayout();
			base.SuspendLayout();
			this.lblType.Location = new Point(45, 41);
			this.lblType.Name = "lblType";
			this.lblType.Size = new Size(109, 24);
			this.lblType.TabIndex = 0;
			this.lblType.Text = "Privacy Type";
			this.lblType.TextAlign = ContentAlignment.MiddleRight;
			this.cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbType.FormattingEnabled = true;
			this.cmbType.Items.AddRange(new object[3]
			{
				"None",
				"Basic",
				"Enhanced"

			});
			this.cmbType.Location = new Point(168, 41);
			this.cmbType.Name = "cmbType";
			this.cmbType.Size = new Size(121, 24);
			this.cmbType.TabIndex = 1;
			this.cmbType.SelectedIndexChanged += this.cmbType_SelectedIndexChanged;
			this.lblKeyLen.Location = new Point(45, 71);
			this.lblKeyLen.Name = "lblKeyLen";
			this.lblKeyLen.Size = new Size(109, 24);
			this.lblKeyLen.TabIndex = 2;
			this.lblKeyLen.Text = "Key Length";
			this.lblKeyLen.TextAlign = ContentAlignment.MiddleRight;
			this.cmbKeyLen.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbKeyLen.FormattingEnabled = true;
			this.cmbKeyLen.Items.AddRange(new object[3]
			{
				"32",
				"64",
				"40"
			});
			this.cmbKeyLen.Location = new Point(168, 71);
			this.cmbKeyLen.Name = "cmbKeyLen";
			this.cmbKeyLen.Size = new Size(121, 24);
			this.cmbKeyLen.TabIndex = 3;
			this.cmbKeyLen.SelectedIndexChanged += this.cmbKeyLen_SelectedIndexChanged;
			this.cmbKeyLen.Enter += this.cmbKeyLen_Enter;
			this.dgvKey.AllowUserToAddRows = false;
			this.dgvKey.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvKey.Columns.AddRange(this.txtKey);
			this.dgvKey.Location = new Point(75, 145);
			this.dgvKey.Name = "dgvKey";
			this.dgvKey.RowTemplate.Height = 23;
			this.dgvKey.Size = new Size(240, 353);
			this.dgvKey.TabIndex = 6;
			this.dgvKey.CellValidated += this.dgvKey_CellValidated;
			this.dgvKey.RowPostPaint += this.dgvKey_RowPostPaint;
			this.dgvKey.CellValidating += this.dgvKey_CellValidating;
			this.dgvKey.EditingControlShowing += this.dgvKey_EditingControlShowing;
			this.txtKey.HeaderText = "Key";
			this.txtKey.Name = "txtKey";
			this.btnDel.Location = new Point(220, 107);
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new Size(75, 23);
			this.btnDel.TabIndex = 5;
			this.btnDel.Text = "Delete";
			this.btnDel.UseVisualStyleBackColor = true;
			this.btnDel.Click += this.btnDel_Click;
			this.btnAdd.Location = new Point(95, 107);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new Size(75, 23);
			this.btnAdd.TabIndex = 4;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += this.btnAdd_Click;
			this.pnlEncrypt.AutoScroll = true;
			this.pnlEncrypt.AutoSize = true;
			this.pnlEncrypt.Controls.Add(this.cmbType);
			this.pnlEncrypt.Controls.Add(this.dgvKey);
			this.pnlEncrypt.Controls.Add(this.lblType);
			this.pnlEncrypt.Controls.Add(this.btnDel);
			this.pnlEncrypt.Controls.Add(this.lblKeyLen);
			this.pnlEncrypt.Controls.Add(this.btnAdd);
			this.pnlEncrypt.Controls.Add(this.cmbKeyLen);
			this.pnlEncrypt.Dock = DockStyle.Fill;
			this.pnlEncrypt.Location = new Point(0, 0);
			this.pnlEncrypt.Name = "pnlEncrypt";
			this.pnlEncrypt.Size = new Size(390, 539);
			this.pnlEncrypt.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(7f, 16f);
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(390, 539);
			base.Controls.Add(this.pnlEncrypt);
			this.Font = new Font("Arial", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Name = "EncryptForm";
			this.Text = "Privacy Setting";
			base.Load += this.EncryptForm_Load;
			base.FormClosing += this.EncryptForm_FormClosing;
			((ISupportInitialize)this.dgvKey).EndInit();
			this.pnlEncrypt.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		static EncryptForm()
		{
			
			EncryptForm.SZ_ENCRYPT_TYPE = new string[3]
			{
				"None",
				"Basic",
				"Ehnanced"
			};
			EncryptForm.SZ_KEY_LEN = new string[3]
			{
				"32",
				"64",
				"40"
			};
			EncryptForm.data = new Encrypt();
		}
	}
}
