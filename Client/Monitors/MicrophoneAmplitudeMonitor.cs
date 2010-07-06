using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenMessenger.Client.Sensors;
using OpenMessenger.Events;
using NAudio.Wave;
using System.IO;

namespace OpenMessenger.Client.Monitors
{
    /// <summary>
    /// Monitor subscribing to events from the microphone sensor, computing the average ampitude. If the
    /// amplitude is outside of the specified threshold range, an AmplitudeEvent is broadcast.
    /// </summary>
    public class MicrophoneAmplitudeMonitor : Monitor
    {
        private double threshold; 

        /// <summary>
        /// Name of this monitor
        /// </summary>
        public override string Text
        {
            get { return "Microphone Amplitude"; }
        }

        /// <summary>
        /// Start the monitor
        /// </summary>
        public override void Start()
        {
            Sensor.GetInstance<MicrophoneSensor>().SoundQuantum +=
                new MicrophoneSensor.SoundQuantumEventHandler(OnSoundQuantumHandler);
        }

        private void OnSoundQuantumHandler(WaveInEventArgs args)
        {
            byte[] wavSound = args.Buffer;
            double avgAmplitude = 0;
            for (int i = 0; i < args.BytesRecorded; i += 2)
            {
                avgAmplitude += ComplementToSigned(ref wavSound, i);
            }
            avgAmplitude = avgAmplitude / (wavSound.Length / 2) * 10;

            ClientController client = ClientController.GetInstance();
            AmplitudeEvent e = new AmplitudeEvent(client.Me.Id, avgAmplitude);
            client.BroadcastEvent(e);
            Console.WriteLine("Microphone threshold exceeded: " + avgAmplitude);
        }

        private short ComplementToSigned(ref byte[] bytArr, int intPos)
        {
            short snd = BitConverter.ToInt16(bytArr, intPos);
            if (snd != 0)
                snd = Convert.ToInt16((~snd | 1));
            return snd;
        }


        private double GetAverageAmplitude(byte[] soundData)
        {
            double total = 0;

            for (int i = 0; i < soundData.Length; i++)
            {
                total += soundData[i];
            }

            return total / (double)soundData.Length;
        }

        /// <summary>
        /// Stop this monitor, listening to sensors
        /// </summary>
        public override void Stop()
        {
            Sensor.GetInstance<MicrophoneSensor>().SoundQuantum -= OnSoundQuantumHandler;
        }

    }
}
