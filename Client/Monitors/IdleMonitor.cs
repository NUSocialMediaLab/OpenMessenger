using System;
using OpenMessenger.Client.Sensors;
using OpenMessenger.Events;

namespace OpenMessenger.Client.Monitors
{
    class IdleMonitor : Monitor
    {
        public IdleMonitor()
        {
        }

        public override void Start()
        {
            Sensor.GetInstance<IdleSensor>().IdleTimeUpdate +=
                new IdleSensor.IdleTimeUpdateHandler(OnIdleTimeUpdateHandler);
        }
        /// <summary>
        /// Stop the monitor, listening to EyeTrackerSensor
        /// </summary>
        public override void Stop()
        {
            Sensor.GetInstance<IdleSensor>().IdleTimeUpdate -= OnIdleTimeUpdateHandler;
        }

        private void OnIdleTimeUpdateHandler(int idleTime)
        {
            Console.WriteLine("IdleUpdater");
            ClientController client = ClientController.GetInstance();

            if (idleTime > 0)
            {
                Console.WriteLine("IdleTime: " + idleTime);
                client.BroadcastEvent(new IdleTimeEvent(client.Me.Id, idleTime));

            }
        }

        /// <summary>
        /// Name of this monitor
        /// </summary>
        public override string Text
        {
            get { return "Idle Time"; }
        }
    }
}
