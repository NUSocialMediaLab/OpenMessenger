namespace OpenMessenger.Client.Dialogs
{
    /// <summary>
    /// Chat window for a contact-contact message conversation
    /// </summary>
    partial class ConversationDialog
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
            this.txtConversation = new System.Windows.Forms.TextBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtConversation
            // 
            this.txtConversation.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtConversation.Location = new System.Drawing.Point(0, 0);
            this.txtConversation.Multiline = true;
            this.txtConversation.Name = "txtConversation";
            this.txtConversation.Size = new System.Drawing.Size(434, 214);
            this.txtConversation.TabIndex = 1;
            // 
            // txtInput
            // 
            this.txtInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtInput.Location = new System.Drawing.Point(0, 222);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(434, 66);
            this.txtInput.TabIndex = 0;
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyDown);
            // 
            // ConversationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 288);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.txtConversation);
            this.Name = "ConversationDialog";
            this.Text = "OM";
            this.Load += new System.EventHandler(this.ProfileDialog_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConversationDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtConversation;
        private System.Windows.Forms.TextBox txtInput;

    }
}