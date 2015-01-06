using System.ComponentModel;

namespace CameraTest
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
			this.lstVideoDevices = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// lstVideoDevices
			// 
			this.lstVideoDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.lstVideoDevices.FormattingEnabled = true;
			this.lstVideoDevices.Location = new System.Drawing.Point(13, 13);
			this.lstVideoDevices.Name = "lstVideoDevices";
			this.lstVideoDevices.Size = new System.Drawing.Size(502, 21);
			this.lstVideoDevices.TabIndex = 0;
			this.lstVideoDevices.SelectedIndexChanged += new System.EventHandler(this.lstVideoDevices_SelectedIndexChanged);
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(13, 40);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(502, 372);
			this.panel1.TabIndex = 3;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(527, 424);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lstVideoDevices);
			this.Name = "Form1";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox lstVideoDevices;
        private System.Windows.Forms.Panel panel1;
	}
}

