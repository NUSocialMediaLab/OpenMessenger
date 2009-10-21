using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using OpenMessenger.Events;
using OpenMessenger.Client.Sensors;
 

namespace OpenMessenger.Client.Monitors
{
    /// <summary>
    /// Monitor subscribing to the screen sensor and webcam sensor. Computes the percent difference
    /// between previously sampled screenshot or frame. If this difference is above some threshold
    /// (_activeDelta), an ActivityEvent is broadcast.
    /// </summary>
    public class ActivityMonitor : Monitor
    {
        Bitmap _previousScreenshot;
        Bitmap _previousCameraFrame;
        double _activeDelta = 0.10;

        /// <summary>
        /// Name of this monitor
        /// </summary>
        public override string Text
        {
            get { return "Screen Activity"; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ActivityMonitor()
        {
        }

        /// <summary>
        /// Starts the monitor, listening to sensors
        /// </summary>
        public override void Start()
        {
            // subscribe to screenshots, if available
            Sensor.GetInstance<ScreenSensor>().ScreenUpdate += 
                new ScreenSensor.ScreenUpdateHandler(OnScreenUpdateHandler);

            // subscribe to webcam frames, if available
            Sensor.GetInstance<WebcamSensor>().CameraFrameUpdate += 
                new WebcamSensor.CameraFrameUpdateHandler(OnCameraFrameUpdate);
        }

        void OnCameraFrameUpdate(Bitmap frame)
        {
            if (_previousCameraFrame != null)
            {
                double percentChange = CalculatePercentDelta(_previousCameraFrame, frame);

                if (percentChange >= _activeDelta)
                {
                    ClientController.GetInstance().BroadcastEvent(
                        new ActivityEvent(ClientController.GetInstance().Me.Id, percentChange));
                }
            }

            _previousCameraFrame = frame;
        }

        /// <summary>
        /// Stop this monitor, listening to sensors
        /// </summary>
        public override void Stop()
        {
            Sensor.GetInstance<ScreenSensor>().ScreenUpdate -= OnScreenUpdateHandler;
        }

        private void OnScreenUpdateHandler(Bitmap screenshot)
        {
            if (_previousScreenshot != null)
            {
                double percentChange = CalculatePercentDelta(_previousScreenshot, screenshot);

                if (percentChange >= _activeDelta)
                {
                    ClientController.GetInstance().BroadcastEvent(
                        new ActivityEvent(ClientController.GetInstance().Me.Id, percentChange));
                }
            }

            _previousScreenshot = screenshot;
        }

        private double CalculatePercentDelta(Bitmap a, Bitmap b)
        {
            int width = Math.Min(a.Width, b.Width);
            int height = Math.Min(a.Height, b.Height);

            int pixelsChanged = 0;

            for (int i = 0; i < width; i += 10)
            {
                for (int j = 0; j < height; j += 10)
                {
                    if (a.GetPixel(i, j) != b.GetPixel(i, j))
                        pixelsChanged++;
                }
            }

            return Math.Pow(10, 2) * 100f * (double)pixelsChanged / (double)(width * height);
        }

    }
}
