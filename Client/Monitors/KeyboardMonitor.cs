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
            ClientController client = ClientController.GetInstance();
            if (keys.Length > 0)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    Console.Write(keys[i]);
                }
                Console.WriteLine();
                client.BroadcastEvent(new KeyboardEvent(client.Me.Id, keys.Length));
            }
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
