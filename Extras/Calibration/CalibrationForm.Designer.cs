using System;
using System.Windows.Forms;

namespace DMR
{
	partial class CalibrationForm
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
			this.tabCtlBands = new System.Windows.Forms.TabControl();
			this.tabVHF = new System.Windows.Forms.TabPage();
			this.calibrationBandControlVHF = new DMR.CalibrationBandControl();
			this.tabUHF = new System.Windows.Forms.TabPage();
			this.calibrationBandControlUHF = new DMR.CalibrationBandControl();
			this.btnWrite = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnReadFile = new System.Windows.Forms.Button();
			this.btnReadFromRadio = new System.Windows.Forms.Button();
			this.lblMessage = new System.Windows.Forms.Label();
			this.tabCtlBands.SuspendLayout();
			this.tabVHF.SuspendLayout();
			this.tabUHF.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabCtlBands
			// 
			this.tabCtlBands.Controls.Add(this.tabVHF);
			this.tabCtlBands.Controls.Add(this.tabUHF);
			this.tabCtlBands.Location = new System.Drawing.Point(12, 59);
			this.tabCtlBands.Name = "tabCtlBands";
			this.tabCtlBands.SelectedIndex = 0;
			this.tabCtlBands.Size = new System.Drawing.Size(921, 524);
			this.tabCtlBands.TabIndex = 0;
			this.tabCtlBands.Visible = false;
			// 
			// tabVHF
			// 
			this.tabVHF.Controls.Add(this.calibrationBandControlVHF);
			this.tabVHF.Location = new System.Drawing.Point(4, 22);
			this.tabVHF.Name = "tabVHF";
			this.tabVHF.Padding = new System.Windows.Forms.Padding(3);
			this.tabVHF.Size = new System.Drawing.Size(913, 498);
			this.tabVHF.TabIndex = 0;
			this.tabVHF.Text = "VHF";
			this.tabVHF.UseVisualStyleBackColor = true;
			// 
			// calibrationBandControlVHF
			// 
			this.calibrationBandControlVHF.Location = new System.Drawing.Point(5, 5);
			this.calibrationBandControlVHF.Name = "calibrationBandControlVHF";
			this.calibrationBandControlVHF.Size = new System.Drawing.Size(880, 487);
			this.calibrationBandControlVHF.TabIndex = 0;
			this.calibrationBandControlVHF.Type = "VHF";
			// 
			// tabUHF
			// 
			this.tabUHF.Controls.Add(this.calibrationBandControlUHF);
			this.tabUHF.Location = new System.Drawing.Point(4, 22);
			this.tabUHF.Name = "tabUHF";
			this.tabUHF.Padding = new System.Windows.Forms.Padding(3);
			this.tabUHF.Size = new System.Drawing.Size(913, 498);
			this.tabUHF.TabIndex = 1;
			this.tabUHF.Text = "UHF";
			this.tabUHF.UseVisualStyleBackColor = true;
			// 
			// calibrationBandControlUHF
			// 
			this.calibrationBandControlUHF.Location = new System.Drawing.Point(5, 5);
			this.calibrationBandControlUHF.Name = "calibrationBandControlUHF";
			this.calibrationBandControlUHF.Size = new System.Drawing.Size(882, 487);
			this.calibrationBandControlUHF.TabIndex = 0;
			this.calibrationBandControlUHF.Type = "UHF";
			// 
			// btnWrite
			// 
			this.btnWrite.Location = new System.Drawing.Point(827, 12);
			this.btnWrite.Name = "btnWrite";
			this.btnWrite.Size = new System.Drawing.Size(102, 23);
			this.btnWrite.TabIndex = 1;
			this.btnWrite.Text = "Write to radio";
			this.btnWrite.UseVisualStyleBackColor = true;
			this.btnWrite.Visible = false;
			this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(854, 597);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnReadFile
			// 
			this.btnReadFile.Location = new System.Drawing.Point(172, 12);
			this.btnReadFile.Name = "btnReadFile";
			this.btnReadFile.Size = new System.Drawing.Size(123, 23);
			this.btnReadFile.TabIndex = 1;
			this.btnReadFile.Text = "Open Calibration file";
			this.btnReadFile.UseVisualStyleBackColor = true;
			this.btnReadFile.Click += new System.EventHandler(this.btnReadFile_Clk);
			// 
			// btnReadFromRadio
			// 
			this.btnReadFromRadio.Location = new System.Drawing.Point(12, 12);
			this.btnReadFromRadio.Name = "btnReadFromRadio";
			this.btnReadFromRadio.Size = new System.Drawing.Size(154, 23);
			this.btnReadFromRadio.TabIndex = 1;
			this.btnReadFromRadio.Text = "Read calibration from radio";
			this.btnReadFromRadio.UseVisualStyleBackColor = true;
			this.btnReadFromRadio.Click += new System.EventHandler(this.btnReadFromRadio_Clk);
			// 
			// lblMessage
			// 
			this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMessage.Location = new System.Drawing.Point(327, 12);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(436, 35);
			this.lblMessage.TabIndex = 2;
			this.lblMessage.Text = "Please read the calibration data from the radio or open a calibration file";
			this.lblMessage.Click += new System.EventHandler(this.label1_Click);
			// 
			// CalibrationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(945, 632);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnReadFromRadio);
			this.Controls.Add(this.btnReadFile);
			this.Controls.Add(this.btnWrite);
			this.Controls.Add(this.tabCtlBands);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "CalibrationForm";
			this.Text = "Calibration";
			this.Shown += new System.EventHandler(this.onFormShown);
			this.tabCtlBands.ResumeLayout(false);
			this.tabVHF.ResumeLayout(false);
			this.tabUHF.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabCtlBands;
		private System.Windows.Forms.TabPage tabVHF;
		private System.Windows.Forms.TabPage tabUHF;
		private System.Windows.Forms.Button btnWrite;
		private System.Windows.Forms.Button btnClose;
		private CalibrationBandControl calibrationBandControlUHF;
		private CalibrationBandControl calibrationBandControlVHF;
		private Button btnReadFile;
		private Button btnReadFromRadio;
		private Label lblMessage;
	}
}