using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Timers;

namespace OpenMessenger.Client.Sensors
{
    /// <summary>
    /// Sensor that finds the idle time from the last keyboard or mouse input
    /// from the operating system with intervals specifed by updateFrequency
    /// </summary>
    class IdleSensor : Sensor
    {
        /// <summary>
        /// Information on getting idle time from Windows gotten from:
        /// http://www.pinvoke.net/default.aspx/user32.GetLastInputInfo
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }

        /// <summary>
        /// Information on getting idle time from Windows gotten from:
        /// http://www.pinvoke.net/default.aspx/user32.GetLastInputInfo
        /// </summary>
        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        /// <summary>
        /// Delegate for IdleTimeUpdate event
        /// </summary>
        /// <param name="seconds">number of seconds since last activity</param>
        public delegate void IdleTimeUpdateHandler(int seconds);

        /// <summary>
        /// Triggered when the timer goes off, sending the number of seconds since
        /// the last keyboard or mouse activity
        /// </summary>
        public event IdleTimeUpdateHandler IdleTimeUpdate;

        private const int updateFrequency = 1000;
        private System.Timers.Timer captureTimer;

        /// <summary>
        /// Default constructor
        /// </summary>
        public IdleSensor()
        {
            captureTimer = new System.Timers.Timer(updateFrequency);
            captureTimer.Elapsed += new ElapsedEventHandler(GetLastInputTime);
        }

        /// <summary>
        /// Name of the sensor
        /// </summary>
        public override string Text
        {
            get { return "IdleTime"; }
        }

        /// <summary>
        /// Starts the sensor
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
        /// Queries the operating system for the amount of time since the last
        /// keyboard or mouse activity. Runs everytime the timer fires.
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void GetLastInputTime(object sender, ElapsedEventArgs e)
        {
            int idleTime = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            int envTicks = Environment.TickCount;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                //This cast may cause some subtle problems with how
                //idle calculation but hopefully nothing serious
                //enough that we actually care
                int lastInputTick = (int)lastInputInfo.dwTime;
                idleTime = envTicks - lastInputTick;
            }

            if (IdleTimeUpdate != null && idleTime > 0)
            {
                Console.WriteLine("Idletime: " + idleTime / 1000);
                IdleTimeUpdate(idleTime / 1000);
            }
        }
    }
}
