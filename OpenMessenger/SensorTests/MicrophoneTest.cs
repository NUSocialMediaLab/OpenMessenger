using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Timers;
using OpenMessenger.Client.Sensors;
using OpenMessenger.Client;
using NAudio;
using NAudio.Wave;

namespace OpenMessenger.Tests.Sensors
{
    class MicrophoneTest
    {
        MicrophoneSensor _microphone;
        String _file = "MicrophoneTest.wav";
        String _rawTest = "RawMic.wav";
        WaveFileWriter writer;

        public MicrophoneTest()
        {
            _microphone = Sensor.GetInstance
                    <OpenMessenger.Client.Sensors.MicrophoneSensor>();
            _microphone.SoundQuantum += new MicrophoneSensor.SoundQuantumEventHandler(WriteData);
            if(! File.Exists(_file))
            {
                File.Create(_file);
                System.Threading.Thread.Sleep(1000);
            }
            writer = new WaveFileWriter(_file, new WaveFormat(8000, 16, 1));
        }

        private void WriteData(byte[] data)
        {
            writer.WriteData(data, 0, data.Length);
        }

        public void StartRecording()
        {
            _microphone.Start();
        }

        public void StopRecording()
        {
            _microphone.Stop();
            writer.Close();
        }
    }
}
