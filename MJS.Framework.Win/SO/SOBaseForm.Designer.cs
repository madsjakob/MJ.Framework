namespace MJS.Framework.Win.SO
{
    partial class SOBaseForm
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
            this.SuspendLayout();
            // 
            // SOBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "SOBaseForm";
            this.Text = "SOBaseForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SOBaseForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SOBaseForm_FormClosed);
            this.Load += new System.EventHandler(this.SOBaseForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
    }
}