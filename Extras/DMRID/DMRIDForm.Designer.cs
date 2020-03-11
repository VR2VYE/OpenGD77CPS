namespace DMR
{
	partial class DMRIDForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DMRIDForm));
			this.btnDownload = new System.Windows.Forms.Button();
			this.btnWriteToGD77 = new System.Windows.Forms.Button();
			this.txtRegionId = new System.Windows.Forms.TextBox();
			this.btnClear = new System.Windows.Forms.Button();
			this.lblMessage = new System.Windows.Forms.Label();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.txtAgeMaxDays = new System.Windows.Forms.TextBox();
			this.lblRegionId = new System.Windows.Forms.Label();
			this.lblInactivityFilter = new System.Windows.Forms.Label();
			this.chkEnhancedFirmware = new System.Windows.Forms.CheckBox();
			this.cmbStringLen = new System.Windows.Forms.ComboBox();
			this.lblEnhancedLength = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.btnImportCSV = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnDownload
			// 
			this.btnDownload.Location = new System.Drawing.Point(12, 68);
			this.btnDownload.Name = "btnDownload";
			this.btnDownload.Size = new System.Drawing.Size(305, 23);
			this.btnDownload.TabIndex = 0;
			this.btnDownload.Text = "Download from HamDigital - filtered by region and inactivity";
			this.btnDownload.UseVisualStyleBackColor = true;
			this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
			// 
			// btnWriteToGD77
			// 
			this.btnWriteToGD77.Location = new System.Drawing.Point(395, 335);
			this.btnWriteToGD77.Name = "btnWriteToGD77";
			this.btnWriteToGD77.Size = new System.Drawing.Size(123, 28);
			this.btnWriteToGD77.TabIndex = 2;
			this.btnWriteToGD77.Text = "Write to GD-77";
			this.btnWriteToGD77.UseVisualStyleBackColor = true;
			this.btnWriteToGD77.Click += new System.EventHandler(this.btnWriteToGD77_Click);
			// 
			// txtRegionId
			// 
			this.txtRegionId.Location = new System.Drawing.Point(476, 100);
			this.txtRegionId.Name = "txtRegionId";
			this.txtRegionId.Size = new System.Drawing.Size(42, 20);
			this.txtRegionId.TabIndex = 3;
			this.txtRegionId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(447, 287);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(71, 23);
			this.btnClear.TabIndex = 4;
			this.btnClear.Text = "Clear list";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// lblMessage
			// 
			this.lblMessage.Location = new System.Drawing.Point(12, 9);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(405, 23);
			this.lblMessage.TabIndex = 5;
			this.lblMessage.Text = "lblMessage";
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToResizeColumns = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(13, 129);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(505, 141);
			this.dataGridView1.TabIndex = 6;
			this.dataGridView1.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView1_SortCompare);
			// 
			// txtAgeMaxDays
			// 
			this.txtAgeMaxDays.Location = new System.Drawing.Point(476, 74);
			this.txtAgeMaxDays.Name = "txtAgeMaxDays";
			this.txtAgeMaxDays.Size = new System.Drawing.Size(42, 20);
			this.txtAgeMaxDays.TabIndex = 3;
			this.txtAgeMaxDays.Text = "365";
			this.txtAgeMaxDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblRegionId
			// 
			this.lblRegionId.Location = new System.Drawing.Point(368, 103);
			this.lblRegionId.Name = "lblRegionId";
			this.lblRegionId.Size = new System.Drawing.Size(102, 13);
			this.lblRegionId.TabIndex = 7;
			this.lblRegionId.Text = "Region";
			this.lblRegionId.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblInactivityFilter
			// 
			this.lblInactivityFilter.Location = new System.Drawing.Point(340, 77);
			this.lblInactivityFilter.Name = "lblInactivityFilter";
			this.lblInactivityFilter.Size = new System.Drawing.Size(132, 13);
			this.lblInactivityFilter.TabIndex = 7;
			this.lblInactivityFilter.Text = "Inactivity filter (days)";
			this.lblInactivityFilter.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// chkEnhancedFirmware
			// 
			this.chkEnhancedFirmware.AutoSize = true;
			this.chkEnhancedFirmware.Location = new System.Drawing.Point(15, 19);
			this.chkEnhancedFirmware.Name = "chkEnhancedFirmware";
			this.chkEnhancedFirmware.Size = new System.Drawing.Size(146, 17);
			this.chkEnhancedFirmware.TabIndex = 9;
			this.chkEnhancedFirmware.Text = "Enhanced firmware mode";
			this.chkEnhancedFirmware.UseVisualStyleBackColor = true;
			this.chkEnhancedFirmware.CheckedChanged += new System.EventHandler(this.chkEnhancedFirmware_CheckedChanged);
			// 
			// cmbStringLen
			// 
			this.cmbStringLen.FormattingEnabled = true;
			this.cmbStringLen.Items.AddRange(new object[] {
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
			this.cmbStringLen.Location = new System.Drawing.Point(15, 42);
			this.cmbStringLen.Name = "cmbStringLen";
			this.cmbStringLen.Size = new System.Drawing.Size(56, 21);
			this.cmbStringLen.TabIndex = 10;
			this.cmbStringLen.SelectedIndexChanged += new System.EventHandler(this.cmbStringLen_SelectedIndexChanged);
			// 
			// lblEnhancedLength
			// 
			this.lblEnhancedLength.AutoSize = true;
			this.lblEnhancedLength.Location = new System.Drawing.Point(77, 50);
			this.lblEnhancedLength.Name = "lblEnhancedLength";
			this.lblEnhancedLength.Size = new System.Drawing.Size(109, 13);
			this.lblEnhancedLength.TabIndex = 11;
			this.lblEnhancedLength.Text = "Number of characters";
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(15, 35);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(499, 17);
			this.progressBar1.TabIndex = 12;
			// 
			// btnImportCSV
			// 
			this.btnImportCSV.Location = new System.Drawing.Point(12, 100);
			this.btnImportCSV.Name = "btnImportCSV";
			this.btnImportCSV.Size = new System.Drawing.Size(179, 23);
			this.btnImportCSV.TabIndex = 0;
			this.btnImportCSV.Text = "Import CSV - filtered by region code";
			this.btnImportCSV.UseVisualStyleBackColor = true;
			this.btnImportCSV.Click += new System.EventHandler(this.btnImportCSV_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkEnhancedFirmware);
			this.groupBox1.Controls.Add(this.cmbStringLen);
			this.groupBox1.Controls.Add(this.lblEnhancedLength);
			this.groupBox1.Location = new System.Drawing.Point(15, 287);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 78);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Data record length";
			// 
			// DMRIDForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(526, 375);
			this.Controls.Add(this.lblRegionId);
			this.Controls.Add(this.txtAgeMaxDays);
			this.Controls.Add(this.txtRegionId);
			this.Controls.Add(this.lblInactivityFilter);
			this.Controls.Add(this.btnDownload);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.btnWriteToGD77);
			this.Controls.Add(this.btnImportCSV);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DMRIDForm";
			this.Text = "DMR ID";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DMRIDFormNew_FormClosing);
			this.Load += new System.EventHandler(this.DMRIDForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnDownload;
		private System.Windows.Forms.Button btnWriteToGD77;
		private System.Windows.Forms.TextBox txtRegionId;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.TextBox txtAgeMaxDays;
		private System.Windows.Forms.Label lblRegionId;
		private System.Windows.Forms.Label lblInactivityFilter;
		private System.Windows.Forms.CheckBox chkEnhancedFirmware;
		private System.Windows.Forms.ComboBox cmbStringLen;
		private System.Windows.Forms.Label lblEnhancedLength;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button btnImportCSV;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}