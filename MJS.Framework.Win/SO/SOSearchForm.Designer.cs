namespace MJS.Framework.Win.SO
{
    partial class SOSearchForm
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
            this.soSearchControl1 = new MJS.Framework.Win.SO.SOSearchControl();
            this.SuspendLayout();
            // 
            // soSearchControl1
            // 
            this.soSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soSearchControl1.Location = new System.Drawing.Point(0, 0);
            this.soSearchControl1.Name = "soSearchControl1";
            this.soSearchControl1.Size = new System.Drawing.Size(544, 402);
            this.soSearchControl1.TabIndex = 0;
            // 
            // SOSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(544, 402);
            this.Controls.Add(this.soSearchControl1);
            this.Name = "SOSearchForm";
            this.ResumeLayout(false);

        }

        #endregion

        private SOSearchControl soSearchControl1;
    }
}
