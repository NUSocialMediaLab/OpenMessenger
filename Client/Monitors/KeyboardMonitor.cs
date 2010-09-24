using Microsoft.DirectX.DirectInput;
using OpenMessenger.Client.Sensors;
using OpenMessenger.Events;
using System;

namespace OpenMessenger.Client.Monitors
{
    /// <summary>
    /// Monitor that listens to raw data gathered by the keyboard activity
    /// </summary> 
    public class KeyboardMonitor : Monitor
    {

        private System.DateTime lastEventTime;
        private int eventInterval = 15; //Minutes inbetween firing off events
        private bool wasActive = false;
        /// <summary>
        /// Default constructor
        /// </summary>
        public KeyboardMonitor()
        {
        }

        /// <summary>
        /// Start the monitor, listening to KeyboardSensor
        /// </summary>
        public override void Start()
        {
            Sensor.GetInstance<KeyboardSensor>().KeyboardUpdate +=
                new KeyboardSensor.KeyboardUpdateHandler(OnKeyboardUpdateHandler);
        }

        /// <summary>
        /// Stop the monitor, listening to EyeTrackerSensor
        /// </summary>
        public override void Stop()
        {
            Sensor.GetInstance<KeyboardSensor>().KeyboardUpdate -= OnKeyboardUpdateHandler;
        }

        private void OnKeyboardUpdateHandler(Key[] keys)
        {
            Console.WriteLine("KeyboardUpdate");
            ClientController client = ClientController.GetInstance();
            bool isActive = (keys.Length > 0) ? true : false;
            if(wasActive != isActive)
            {
                Console.Write(System.DateTime.Now + " : " + wasActive + " | " + isActive + " ");
                for (int i = 0; i < keys.Length; i++)
                {
                    Console.Write(keys[i]);
                }
                Console.WriteLine();
                client.BroadcastEvent(new KeyboardEvent(client.Me.Id, keys.Length));
            }
            wasActive = isActive;
        }

        /// <summary>
        /// Name of this monitor
        /// </summary>
        public override string Text
        {
            get { return "Keyboard Activity"; }
        }

    }
}
