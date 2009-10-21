using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenMessenger.Client;

namespace OpenMessenger.Tests.Sensors
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientWindow());

            if (ClientController.GetInstance().Connected)
                ClientController.GetInstance().Disconnect();

            Environment.Exit(0);
        }
    }
}
