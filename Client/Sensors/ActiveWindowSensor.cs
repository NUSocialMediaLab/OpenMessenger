using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Timers;

namespace OpenMessenger.Client.Sensors
{
    /// <summary>
    /// Sensor that determines the Window that currently has the user's focus at
    /// intervals specified by updateFrequency
    /// </summary>
    class ActiveWindowSensor : Sensor
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        /// <summary>
        /// Delegate for the ActiveWindowUpdate event
        /// </summary>
        /// <param name="title">The title of the currently active window</param>
        public delegate void ActiveWindowUpdateHandler(string title);

        /// <summary>
        /// Triggered when the timer goes off and the window has been determined
        /// </summary>
        public event ActiveWindowUpdateHandler ActiveWindowUpdate;

        private const int updateFrequency = 1000;
        private System.Timers.Timer captureTimer;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ActiveWindowSensor()
        {
            captureTimer = new System.Timers.Timer(updateFrequency);
            captureTimer.Elapsed += new ElapsedEventHandler(GetActiveWindowTitle);
        }

        /// <summary>
        /// Name of this sensor
        /// </summary>
        public override string Text
        {
            get { return "ActiveWindow"; }
        }

        /// <summary>
        /// Starts the sensor, capturing the active window's title
        /// </summary>
        public override void Start()
        {
            captureTimer.Start();
            captureTimer.Enabled = true;
        }
        
        /// <summary>
        /// Stops the sensor
        /// </summary>
        public override void Stop()
        {
            captureTimer.Stop();
            captureTimer.Enabled = false;
        }

        /// <summary>
        /// Method called by the timer everytime we want to find out what
        /// the active window is
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void GetActiveWindowTitle(object sender, ElapsedEventArgs e)
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();

            if (GetWindowText(handle, buff, nChars) > 0)
            {
                Console.WriteLine(buff.ToString());
                if(ActiveWindowUpdate != null)
                    ActiveWindowUpdate(buff.ToString());
            }
            else
            {
                Console.WriteLine("No active window?");
            }
        }
    }
}
