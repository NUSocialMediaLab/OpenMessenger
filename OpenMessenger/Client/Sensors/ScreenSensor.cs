using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;


namespace OpenMessenger.Client.Sensors
{
    /// <summary>
    /// Sensor capturing screenshots at regular intervals, specified by _updateFrequency
    /// </summary>
    public class ScreenSensor : Sensor
    {
        /// <summary>
        /// Delegate for the ScreenUpdate event
        /// </summary>
        /// <param name="screenshot"></param>
        public delegate void ScreenUpdateHandler(Bitmap screenshot);

        /// <summary>
        /// Triggered when a new screenshot has been sampled
        /// </summary>
        public event ScreenUpdateHandler ScreenUpdate;

        Size resolution = new Size(400, 300);
        const int _updateFrequency = 5000;
        System.Timers.Timer _captureTimer;

        /// <summary>
        /// Name of this sensor
        /// </summary>
        public override string Text
        {
            get { return "Screen"; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScreenSensor()
        {
            _captureTimer = new System.Timers.Timer(_updateFrequency);
            _captureTimer.Elapsed += new System.Timers.ElapsedEventHandler(CaptureScreenshot);
        }

        /// <summary>
        /// Start the sensor, capturing screenshots
        /// </summary>
        public override void Start()
        {
            _captureTimer.Start();
        }

        /// <summary>
        /// Stop the sensor, capturing screenshots
        /// </summary>
        public override void Stop()
        {
            _captureTimer.Stop();
        }

        private void CaptureScreenshot(object sender, System.Timers.ElapsedEventArgs e)
        {
            _captureTimer.Stop();

            if (ScreenUpdate != null)
            {
                Bitmap screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                    Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);

                Graphics capture = Graphics.FromImage(screenshot);

                capture.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                    Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size,
                    CopyPixelOperation.SourceCopy);

                screenshot = (Bitmap)screenshot.GetThumbnailImage(resolution.Width, resolution.Height,
                    null, System.IntPtr.Zero);

                ScreenUpdate(screenshot);
            }

            _captureTimer.Start();
        }
    }
}
