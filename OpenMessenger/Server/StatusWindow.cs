using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenMessenger.Server.Views;

namespace OpenMessenger.Server
{
    public partial class Status : Form
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Status()
        {
            InitializeComponent();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            Server server = Server.GetInstance();

            if (server.IsRunning)
            {
                if (server.Stop())
                {
                    btnStartStop.Text = "Start";
                    Text = "Open Messenger - Stopped";
                }
            }
            else
            {
                if (server.Start(txtUrl.Text))
                {
                    btnStartStop.Text = "Stop";
                    Text = "Open Messenger - Started";
                }                
            }
        }

        private void mnuViewEventLog_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuViewEventLog.Checked)
            {
                View view = View.GetInstance<EventLogView>();
                tabViews.Controls.Add(view);
            }
            else
                tabViews.Controls.Remove(View.GetInstance<EventLogView>());
        }

        private void mnuServerExit_Click(object sender, EventArgs e)
        {
            if (Server.GetInstance().IsRunning)
                Server.GetInstance().Stop();

            Application.Exit();
        }

        private void Status_Load(object sender, EventArgs e)
        {
            mnuViewEventLog.Checked = true;
        }
    }
}
