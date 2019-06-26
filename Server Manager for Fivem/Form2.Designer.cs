namespace Fivem_Server_Manager
{
    partial class InstallResource
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
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label25 = new System.Windows.Forms.Label();
            this.BrowserFolder = new System.Windows.Forms.Button();
            this.InstallR = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.BoxPathResource = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(139, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Installing Resource....";
            this.label1.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 54);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(362, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.AutoSize = true;
            this.label25.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label25.Location = new System.Drawing.Point(12, 9);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(122, 13);
            this.label25.TabIndex = 41;
            this.label25.Text = "New Resource Location";
            // 
            // BrowserFolder
            // 
            this.BrowserFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowserFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BrowserFolder.Location = new System.Drawing.Point(229, 23);
            this.BrowserFolder.Name = "BrowserFolder";
            this.BrowserFolder.Size = new System.Drawing.Size(75, 23);
            this.BrowserFolder.TabIndex = 40;
            this.BrowserFolder.Text = "Browse";
            this.BrowserFolder.UseVisualStyleBackColor = true;
            this.BrowserFolder.Click += new System.EventHandler(this.BrowserFolder_Click);
            // 
            // InstallR
            // 
            this.InstallR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InstallR.Location = new System.Drawing.Point(310, 23);
            this.InstallR.Name = "InstallR";
            this.InstallR.Size = new System.Drawing.Size(75, 23);
            this.InstallR.TabIndex = 42;
            this.InstallR.Text = "Install";
            this.InstallR.UseVisualStyleBackColor = true;
            this.InstallR.Click += new System.EventHandler(this.InstallR_Click);
            // 
            // BoxPathResource
            // 
            this.BoxPathResource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BoxPathResource.Location = new System.Drawing.Point(24, 25);
            this.BoxPathResource.Name = "BoxPathResource";
            this.BoxPathResource.ReadOnly = true;
            this.BoxPathResource.Size = new System.Drawing.Size(112, 20);
            this.BoxPathResource.TabIndex = 39;
            this.BoxPathResource.Text = "(Empty)";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "InFolderESX",
            "OutFolderESX"});
            this.comboBox1.Location = new System.Drawing.Point(142, 25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(81, 21);
            this.comboBox1.TabIndex = 44;
            this.comboBox1.Text = "Category";
            // 
            // InstallResource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 85);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.InstallR);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.BrowserFolder);
            this.Controls.Add(this.BoxPathResource);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallResource";
            this.ShowIcon = false;
            this.Text = "Install Resource";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InstallResource_FormClosing);
            this.Load += new System.EventHandler(this.InstallResource_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        public System.Windows.Forms.TextBox BoxPathResource;
        public System.Windows.Forms.Button BrowserFolder;
        public System.Windows.Forms.Button InstallR;
        public System.Windows.Forms.ComboBox comboBox1;
    }
}