namespace OpenMessenger.Client.Views
{
    partial class EventLogView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox lstEvents;

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
            this.lstEvents = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstEvents
            // 
            this.lstEvents.Location = new System.Drawing.Point(50, 50);
            this.lstEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstEvents.Name = "lstEvents";
            this.lstEvents.Size = new System.Drawing.Size(100, 100);
            this.lstEvents.TabIndex = 0;
            // EventLogView
            // 
            this.Controls.Add(this.lstEvents);
            this.UseVisualStyleBackColor = true;
            this.ResumeLayout(false);

        }

        #endregion


    }
}
