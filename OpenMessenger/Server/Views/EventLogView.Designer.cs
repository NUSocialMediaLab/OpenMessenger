namespace OpenMessenger.Server.Views
{
    /// <summary>
    /// Event log view for the server
    /// </summary>
    partial class EventLogView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.lstIncomingEvents = new System.Windows.Forms.ListBox();
            this.lstOutgoingEvents = new System.Windows.Forms.ListBox();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.lstIncomingEvents);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.lstOutgoingEvents);
            this.splitContainer.Size = new System.Drawing.Size(465, 396);
            this.splitContainer.SplitterDistance = 155;
            this.splitContainer.TabIndex = 0;
            // 
            // lstIncomingEvents
            // 
            this.lstIncomingEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstIncomingEvents.FormattingEnabled = true;
            this.lstIncomingEvents.Location = new System.Drawing.Point(0, 0);
            this.lstIncomingEvents.Name = "lstIncomingEvents";
            this.lstIncomingEvents.Size = new System.Drawing.Size(155, 394);
            this.lstIncomingEvents.TabIndex = 0;
            // 
            // lstOutgoingEvents
            // 
            this.lstOutgoingEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstOutgoingEvents.FormattingEnabled = true;
            this.lstOutgoingEvents.Location = new System.Drawing.Point(0, 0);
            this.lstOutgoingEvents.Name = "lstOutgoingEvents";
            this.lstOutgoingEvents.Size = new System.Drawing.Size(306, 394);
            this.lstOutgoingEvents.TabIndex = 0;
            // 
            // EventLogView
            // 
            this.Controls.Add(this.splitContainer);
            this.Name = "EventLogView";
            this.Size = new System.Drawing.Size(465, 396);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ListBox lstIncomingEvents;
        private System.Windows.Forms.ListBox lstOutgoingEvents;


    }
}
