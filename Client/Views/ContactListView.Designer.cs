namespace OpenMessenger.Client.Views
{
    partial class ContactListView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox lstContacts;

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
            this.lstContacts = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstEvents
            // 
            this.lstContacts.Location = new System.Drawing.Point(50, 50);
            this.lstContacts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstContacts.Name = "lstEvents";
            this.lstContacts.Size = new System.Drawing.Size(100, 100);
            this.lstContacts.TabIndex = 0;
            this.lstContacts.DoubleClick += new System.EventHandler(DoubleClickHandler);
            // EventLogView
            // 
            this.Controls.Add(this.lstContacts);
            this.UseVisualStyleBackColor = true;
            this.ResumeLayout(false);

        }

        #endregion


    }
}
