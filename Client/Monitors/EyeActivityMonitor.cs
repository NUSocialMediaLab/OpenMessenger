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
    public class EyeActivityMonitor : Monitor
    {
        private const int numReadsToStore = 10;
        /// <summary>
        /// The number of latest eye tracker readings (OmniWindow.OmniWindowPos) that are stored in history.
        /// </summary>
        public int NumReadsToStore
        {
            get { return numReadsToStore; }
        }

        private Queue<OmniWindow.OmniWindowPos> eyeReadings = new Queue<OmniWindow.OmniWindowPos>(numReadsToStore);
        /// <summary>
        /// Queue that stores the latest eye tracker readings (OmniWindow.OmniWindowPos).
        /// </summary>
        public Queue<OmniWindow.OmniWindowPos> EyeReadings
        {
            get { return eyeReadings; }
            set { eyeReadings = value; }
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
        public EyeActivityMonitor()
        {
        }

        /// <summary>
        /// Start the monitor, listening to EyeTrackerSensor
        /// </summary>
        public override void Start()
        {
            Sensor.GetInstance<EyeTrackerSensor>().EyeTrackerUpdate += 
                new EyeTrackerSensor.EyeTrackerUpdateHandler(OnEyeTrackerUpdateHandler);
        }

        /// <summary>
        /// Stop the monitor, listening to EyeTrackerSensor
        /// </summary>
        public override void Stop()
        {
            Sensor.GetInstance<EyeTrackerSensor>().EyeTrackerUpdate -= OnEyeTrackerUpdateHandler;
        }

        /// <summary>
        /// Callback method that creates a pixel representation of the eye tracker readings,
        /// checks whether an avatar was hit on the OmniWindow and stores all this information
        /// in a queue
        /// </summary>
        private void OnEyeTrackerUpdateHandler(byte sceneNum, float xIn, float yIn)
        {
            //Create an OmniWindowPos object from the raw readings (this converts inches into pixels)
            OmniWindow.OmniWindowPos owPos = new OmniWindow.OmniWindowPos(ow, sceneNum, xIn, yIn);

            if (eyeReadings.Count == numReadsToStore)
            {
                eyeReadings.Dequeue();
            }

            eyeReadings.Enqueue(owPos);

            ClientController client = ClientController.GetInstance();
                
            //client.BroadcastEvent(
            //    new EyeActivityEvent(ClientController.GetInstance().Me.Id, owPos.SceneNum, owPos.XIn, owPos.YIn, owPos.XPx, owPos.YPx, owPos.AvatarHit));
            
            client.BroadcastEvent(
               new EyeActivityEvent(client.Me.Id, owPos.SceneNum, owPos.XIn, owPos.YIn, owPos.XPx, owPos.YPx, owPos.AvatarHit==null ? "None" : owPos.AvatarHit.Name));

        }

        /// <summary>
        /// Name of this monitor
        /// </summary>
        public override string Text
        {
            get { return "Eye Activity"; }
        }

    }
}
