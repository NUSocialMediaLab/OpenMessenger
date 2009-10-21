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
    public partial class SettingsDialog : Form
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {
            ClientController controller = ClientController.GetInstance();

            txtServiceAddress.Text = controller.ServiceAddress;
        }
    }
}
