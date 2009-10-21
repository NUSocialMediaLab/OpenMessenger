using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMessenger.Tests.Sensors;
using OpenMessenger.Client;
using Microsoft.DirectX.DirectInput;
using OpenMessenger.Client.Sensors;

namespace SensorTests
{
    public partial class SensorTestForm : Form
    {
        MicrophoneTest _micTest;
        KeyboardSensor _keyboardSensor;
        public SensorTestForm()
        {
            InitializeComponent();
        }

        private void SensorTestForm_Load(object sender, EventArgs e)
        {
            StopRecordingButton.Enabled = false;
            _keyboardSensor = Sensor.GetInstance
                <OpenMessenger.Client.Sensors.KeyboardSensor>();
        }

        private void StartRecordingButton_Click(object sender, EventArgs e)
        {
            StartRecordingButton.Enabled = false;
            StopRecordingButton.Enabled = true;
            _micTest = new MicrophoneTest();
            _micTest.StartRecording();
        }

        private void StopRecordingButton_Click(object sender, EventArgs e)
        {
            StartRecordingButton.Enabled = true;
            StopRecordingButton.Enabled = false;
            _micTest.StopRecording();
        }

        private void CaptureKeyboardButton_Click(object sender, EventArgs e)
        {
            _keyboardSensor.Start();
        }
    }
}
