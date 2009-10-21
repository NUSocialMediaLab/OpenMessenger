using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenMessenger.Client.Dialogs
{
    public partial class ProfileDialog : Form
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ProfileDialog()
        {
            InitializeComponent();
        }

        private void ProfileDialog_Load(object sender, EventArgs e)
        {
            Contact me = ClientController.GetInstance().Me;

            txtName.Text = me.Name;
            btnColor.BackColor = Color.FromArgb(me.Color.R,
                                                me.Color.G,
                                                me.Color.B);
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = btnColor.BackColor;

            if (dialog.ShowDialog(this) == DialogResult.OK)
                btnColor.BackColor = dialog.Color;
        }

    }
}
