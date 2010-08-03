using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Timers;

namespace OpenMessenger.Client.Sensors
{
    class ActiveWindowSensor : Sensor
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public delegate void ActiveWindowUpdateHandler(string title);

        public event ActiveWindowUpdateHandler ActiveWindowUpdate;

        private const int updateFrequency = 1000;
        private System.Timers.Timer captureTimer;

        private EventWaitHandle waitHandle;
        private Thread listeningThread;

        public ActiveWindowSensor()
        {
            captureTimer = new System.Timers.Timer(updateFrequency);
            captureTimer.Elapsed += new ElapsedEventHandler(GetActiveWindowTitle);
        }

        public override string Text
        {
            get { return "ActivityMonitor"; }
        }

        public override void Start()
        {
            captureTimer.Start();
            captureTimer.Enabled = true;
        }

        public override void Stop()
        {
            captureTimer.Stop();
            captureTimer.Enabled = false;
        }

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
