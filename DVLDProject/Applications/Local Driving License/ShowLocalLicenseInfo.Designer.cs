namespace DVLDProject.Applications.Local_Driving_License
{
    partial class ShowLocalLicenseInfo
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
            this.ucDrivingLicenseInfo1 = new DVLDProject.Applications.Local_Driving_License.ucDrivingLicenseInfo();
            this.SuspendLayout();
            // 
            // ucDrivingLicenseInfo1
            // 
            this.ucDrivingLicenseInfo1.BackColor = System.Drawing.Color.White;
            this.ucDrivingLicenseInfo1.Location = new System.Drawing.Point(12, 12);
            this.ucDrivingLicenseInfo1.Name = "ucDrivingLicenseInfo1";
            this.ucDrivingLicenseInfo1.Size = new System.Drawing.Size(976, 379);
            this.ucDrivingLicenseInfo1.TabIndex = 0;
            // 
            // ShowLocalLicenseInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1000, 395);
            this.Controls.Add(this.ucDrivingLicenseInfo1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ShowLocalLicenseInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ShowLocalLicenseInfo";
            this.Load += new System.EventHandler(this.ShowLocalLicenseInfo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ucDrivingLicenseInfo ucDrivingLicenseInfo1;
    }
}