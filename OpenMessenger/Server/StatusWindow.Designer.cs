namespace OpenMessenger.Server
{
    /// <summary>
    /// Main window for the server
    /// </summary>
    partial class Status
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
            this.tabViews = new System.Windows.Forms.TabControl();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuServer = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuServerExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViews = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewEventLog = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabViews
            // 
            this.tabViews.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabViews.Location = new System.Drawing.Point(3, 55);
            this.tabViews.Name = "tabViews";
            this.tabViews.SelectedIndex = 0;
            this.tabViews.Size = new System.Drawing.Size(609, 394);
            this.tabViews.TabIndex = 0;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(3, 27);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(113, 23);
            this.btnStartStop.TabIndex = 1;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Service URL:";
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Location = new System.Drawing.Point(199, 29);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(413, 20);
            this.txtUrl.TabIndex = 3;
            this.txtUrl.Text = "http://localhost:8000/OpenMessenger";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuServer,
            this.mnuViews});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(615, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "mnuStrip";
            // 
            // mnuServer
            // 
            this.mnuServer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuServerExit});
            this.mnuServer.Name = "mnuServer";
            this.mnuServer.Size = new System.Drawing.Size(51, 20);
            this.mnuServer.Text = "Server";
            // 
            // mnuServerExit
            // 
            this.mnuServerExit.Name = "mnuServerExit";
            this.mnuServerExit.Size = new System.Drawing.Size(103, 22);
            this.mnuServerExit.Text = "Exit";
            this.mnuServerExit.Click += new System.EventHandler(this.mnuServerExit_Click);
            // 
            // mnuViews
            // 
            this.mnuViews.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewEventLog});
            this.mnuViews.Name = "mnuViews";
            this.mnuViews.Size = new System.Drawing.Size(46, 20);
            this.mnuViews.Text = "Views";
            // 
            // mnuViewEventLog
            // 
            this.mnuViewEventLog.CheckOnClick = true;
            this.mnuViewEventLog.Name = "mnuViewEventLog";
            this.mnuViewEventLog.Size = new System.Drawing.Size(133, 22);
            this.mnuViewEventLog.Text = "Event Log";
            this.mnuViewEventLog.CheckedChanged += new System.EventHandler(this.mnuViewEventLog_CheckedChanged);
            // 
            // Status
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 452);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.tabViews);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Status";
            this.Text = "Open Messenger Server";
            this.Load += new System.EventHandler(this.Status_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabViews;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuViews;
        private System.Windows.Forms.ToolStripMenuItem mnuViewEventLog;
        private System.Windows.Forms.ToolStripMenuItem mnuServer;
        private System.Windows.Forms.ToolStripMenuItem mnuServerExit;
    }
}