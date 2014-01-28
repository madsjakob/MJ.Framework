namespace MJS.Framework.Win.SO
{
    partial class SOEditForm
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
            this.tsMenu = new System.Windows.Forms.ToolStrip();
            this.tssSaveSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.tvMenu = new System.Windows.Forms.TreeView();
            this.tsMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMenu
            // 
            this.tsMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssSaveSeparator,
            this.tsbSave});
            this.tsMenu.Location = new System.Drawing.Point(0, 0);
            this.tsMenu.Name = "tsMenu";
            this.tsMenu.Size = new System.Drawing.Size(752, 31);
            this.tsMenu.TabIndex = 0;
            this.tsMenu.Text = "Menu";
            // 
            // tssSaveSeparator
            // 
            this.tssSaveSeparator.Name = "tssSaveSeparator";
            this.tssSaveSeparator.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = global::MJS.Framework.Win.Properties.Resources.save;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(28, 28);
            this.tsbSave.Text = "Save";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scMain.Location = new System.Drawing.Point(0, 31);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.tvMenu);
            this.scMain.Size = new System.Drawing.Size(752, 459);
            this.scMain.SplitterDistance = 177;
            this.scMain.TabIndex = 1;
            // 
            // tvMenu
            // 
            this.tvMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvMenu.HideSelection = false;
            this.tvMenu.Location = new System.Drawing.Point(0, 0);
            this.tvMenu.Name = "tvMenu";
            this.tvMenu.ShowLines = false;
            this.tvMenu.ShowPlusMinus = false;
            this.tvMenu.ShowRootLines = false;
            this.tvMenu.Size = new System.Drawing.Size(177, 459);
            this.tvMenu.TabIndex = 0;
            this.tvMenu.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvMenu_BeforeCollapse);
            this.tvMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SelectNode);
            // 
            // SOEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 490);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.tsMenu);
            this.Name = "SOEditForm";
            this.Text = "SOEditForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SOEditForm_FormClosing);
            this.tsMenu.ResumeLayout(false);
            this.tsMenu.PerformLayout();
            this.scMain.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMenu;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.TreeView tvMenu;
        private System.Windows.Forms.ToolStripSeparator tssSaveSeparator;
    }
}