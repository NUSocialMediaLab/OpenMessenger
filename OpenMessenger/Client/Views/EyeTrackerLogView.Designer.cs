namespace OpenMessenger.Client.Views
{
    partial class EyeTrackerLogView
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
            this.listView2 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // listView2
            // 
            this.listView2.Location = new System.Drawing.Point(7, 8);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(490, 273);
            this.listView2.TabIndex = 1;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // EyeTrackerLogView
            // 
            this.ClientSize = new System.Drawing.Size(499, 287);
            this.Controls.Add(this.listView2);
            this.Name = "EyeTrackerLogView";
            this.Text = "EyeTrackerDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView2;

    }
}