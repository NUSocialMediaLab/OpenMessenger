using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Timers;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.DirectX.DirectSound;
using NAudio;
using NAudio.Wave;

namespace OpenMessenger.Client.Sensors
{
    /// <summary>
    /// Sensor for sampling sound data from a microphone. Data is sampled at intervals specified by
    /// the _readInterval member and records the sounds in between these intervals. Every interval
    /// the SoundQuantum event is triggered with the sampled data.
    /// NOTE: The meat of this was taken out to lose the DirectX dependency and is currently incomplete.
    /// </summary>
    public class MicrophoneSensor : Sensor
    {
        /// <summary>N
        /// Delegate for the SoundQuantum event
        /// </summary>
        /// <param name="soundData">Sound data captured</param>
        public delegate void SoundQuantumEventHandler(byte[] soundData);

        /// <summary>
        /// Triggered when new sound data has been sampled from the microphone
        /// </summary>
        public event SoundQuantumEventHandler SoundQuantum;

        /*
         * These may not be needed with NAudio
         */
        //const int _quantumSizeBytes = 8000;
        //const int _readInterval = 100000;
        //Timer _readTimer;

        NAudio.Wave.WaveInStream waveIn;

        /// <summary>
        /// Name of this sensor
        /// </summary>
        public override string Text
        {
            get { return "Microphone"; }
        }

        public NAudio.Wave.WaveFormat WaveFormat
        {
            get;
            private set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MicrophoneSensor()
        {
            WaveFormat = new NAudio.Wave.WaveFormat(4000, 16, 1);
            waveIn = new WaveInStream(0, WaveFormat, null);
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(WaveIn_DataAvailable);
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs args)
        {
            if (SoundQuantum != null)
                SoundQuantum(args.Buffer);
        }

        /// <summary>
        /// Start sensor, capture sound
        /// </summary>
        public override void Start()
        {
            waveIn.StartRecording();
        }

        /// <summary>
        /// Stop sensor, capturing sound
        /// </summary>
        public override void Stop()
        {
            waveIn.StopRecording();
        }
    }
}