namespace DMR
{
	partial class FirmwareLoaderUI
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
			this.lblMessage = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnUploadFirmware = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblMessage
			// 
			this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMessage.Location = new System.Drawing.Point(15, 52);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(350, 39);
			this.lblMessage.TabIndex = 3;
			this.lblMessage.Text = "label1";
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(15, 96);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(350, 11);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar1.TabIndex = 2;
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(291, 119);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnUploadFirmware
			// 
			this.btnUploadFirmware.Location = new System.Drawing.Point(115, 12);
			this.btnUploadFirmware.Name = "btnUploadFirmware";
			this.btnUploadFirmware.Size = new System.Drawing.Size(151, 23);
			this.btnUploadFirmware.TabIndex = 5;
			this.btnUploadFirmware.Text = "Upload firmware to GD-77";
			this.btnUploadFirmware.UseVisualStyleBackColor = true;
			this.btnUploadFirmware.Click += new System.EventHandler(this.btnUploadFirmware_Click);
			// 
			// FirmwareLoaderUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(378, 156);
			this.Controls.Add(this.btnUploadFirmware);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.progressBar1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "FirmwareLoaderUI";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Firmware loader";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FirmwareLoaderUI_FormClosing);
			this.Load += new System.EventHandler(this.FirmwareLoaderUI_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnUploadFirmware;
	}
}