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
    /// Monitor that listens to raw data gathered by the EyeTrackingSensor and uses that information
    /// to determine if a user is gazing at another avatar on the OmniWindow in order to try to initiate
    /// conversation
    /// </summary> 
    public class MouseMonitor : Monitor
    {
        private const int numReadsToStore = 10;
        /// <summary>
        /// The number of latest mouse tracker readings (OmniWindow.OmniWindowPos) that are stored in history.
        /// </summary>
        public int NumReadsToStore
        {
            get { return numReadsToStore; }
        }

        private Queue<OmniWindow.OmniWindowPos> mouseReadings = new Queue<OmniWindow.OmniWindowPos>(numReadsToStore);
        /// <summary>
        /// Queue that stores the latest mouse tracker readings (OmniWindow.OmniWindowPos).
        /// </summary>
        public Queue<OmniWindow.OmniWindowPos> MouseReadings
        {
            get { return mouseReadings; }
            set { mouseReadings = value; }
        }

        private OmniWindow ow;
        /// <summary>
        /// A reference to the OmniWindow that is necessary in order to determine the pixel
        /// position on the screen where a user is looking at.
        /// Note: this property MUST be set immediately after the Monitor is instantiated, and before
        /// the monitor is started (before a call to Start() has been made).
        /// The reason for doing it this way is that the monitor cannot take parameters in the constructor
        /// </summary>
        public OmniWindow Ow
        {
            get { return ow; }
            set { ow = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MouseMonitor()
        {
        }

        /// <summary>
        /// Start the monitor, listening to MouseSensor
        /// </summary>
        public override void Start()
        {
            Console.WriteLine("Startcalled");
            Sensor.GetInstance<MouseSensor>().MouseUpdate +=
                new MouseSensor.MouseUpdateHandler(OnMouseUpdateHandler);        
        }



        //this.MouseDown += new MouseEventHandler(Form1_MouseDown);
        //private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        //{
        //    Console.WriteLine("MouseDown");
        //}

        /// <summary>
        /// Stop the monitor, listening to MouseSensor
        /// </summary>
        public override void Stop()
        {
            Sensor.GetInstance<MouseSensor>().MouseUpdate -= OnMouseUpdateHandler;
        }

        /// <summary>
        /// Callback method that creates a pixel representation of the mouse tracker readings,
        /// checks whether an avatar was hit on the OmniWindow and stores all this information
        /// in a queue
        /// </summary>
        private void OnMouseUpdateHandler(byte sceneNum, float xIn, float yIn)
        {
            Console.WriteLine("UpdateHandler");
            //Create an OmniWindowPos object from the raw readings (this converts inches into pixels)
            OmniWindow.OmniWindowPos owPos = new OmniWindow.OmniWindowPos(ow, xIn, yIn);

            if (mouseReadings.Count == numReadsToStore)
            {
                mouseReadings.Dequeue();
            }

            mouseReadings.Enqueue(owPos);

            ClientController client = ClientController.GetInstance();
            client.BroadcastEvent(
               new MouseEvent(client.Me.Id, owPos.XIn, owPos.YIn, owPos.XPx, owPos.YPx, owPos.AvatarHit == null ? "None" : owPos.AvatarHit.Name));

        }

        /// <summary>
        /// Name of this monitor
        /// </summary>
        public override string Text
        {
            get { return "Mouse Activity"; }
        }

    }
}
