using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

using OpenMessenger.Client.Sensors;
using OpenMessenger.Client.Monitors;
using OpenMessenger.Client.Views;
using OpenMessenger.Client.Dialogs;

namespace OpenMessenger.Client
{
    /// <summary>
    /// Main window for the client
    /// </summary>
    public partial class ClientWindow : Form
    {
        OmniWindow _omni;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientWindow()
        {
            _omni = new OmniWindow(this);

            InitializeComponent();
        }

        #region Event Handlers

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        private void mnuViewEventLog_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuViewEventLog.Checked)
            {
                View view = View.GetInstance<EventLogView>();
                tabViews.Controls.Add(view);
            }
            else
                tabViews.Controls.Remove(View.GetInstance<EventLogView>());
        }

        private void mnuSensorMicrophone_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuSensorMicrophone.Checked)
                Sensor.GetInstance<MicrophoneSensor>().Start();
            else
                Sensor.GetInstance<MicrophoneSensor>().Stop();
        }

        private void mnuSensorKeyboard_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuSensorKeyboard.Checked)
                Sensor.GetInstance<KeyboardSensor>().Start();
            else
                Sensor.GetInstance<KeyboardSensor>().Stop();
        }

        private void mnuMonitorMicrophoneAmplitude_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuMonitorMicrophoneAmplitude.Checked)
                Monitor.GetInstance<MicrophoneAmplitudeMonitor>().Start();
            else
                Monitor.GetInstance<MicrophoneAmplitudeMonitor>().Stop();
        }

        private void mnuSensorScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuSensorScreen.Checked)
                Sensor.GetInstance<ScreenSensor>().Start();
            else
                Sensor.GetInstance<ScreenSensor>().Stop();
        }

        private void mnuSensorEyeTracker_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuSensorEyeTracker.Checked)
                Sensor.GetInstance<EyeTrackerSensor>().Start();
            else
                Sensor.GetInstance<EyeTrackerSensor>().Stop();
        }

        private void mnuSensorMouse_CheckChanged(object sender, EventArgs e)
        {
            if (mnuSensorMouse.Checked)
                Sensor.GetInstance<MouseSensor>().Start();
            else
                Sensor.GetInstance<MouseSensor>().Stop();
        }

        private void mnuMonitorScreenActivity_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuMonitorScreenActivity.Checked)
                Monitor.GetInstance<ActivityMonitor>().Start();
            else
                Monitor.GetInstance<ActivityMonitor>().Stop();
        }

        private void mnuMonitorKeyboardAcitivity_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuMonitorKeyboardActivity.Checked)
                Monitor.GetInstance<KeyboardMonitor>().Start();
            else
                Monitor.GetInstance<KeyboardMonitor>().Stop();
        }

        private void mnuMonitorMouseActivity_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuMonitorMouseActivity.Checked)
            {
                MouseMonitor mouseMon = Monitor.GetInstance<MouseMonitor>();
                mouseMon.Ow = _omni;
                Monitor.GetInstance<MouseMonitor>().Start();
            }
            else
                Monitor.GetInstance<MouseMonitor>().Stop();
        }

        private void mnuMonitorEyeActivity_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuMonitorEyeActivity.Checked)
            {
                EyeActivityMonitor eyeMon = Monitor.GetInstance<EyeActivityMonitor>();
                eyeMon.Ow = _omni;
                Monitor.GetInstance<EyeActivityMonitor>().Start();
            }
            else
                Monitor.GetInstance<EyeActivityMonitor>().Stop();
        }

        private void mnuClientProfile_Click(object sender, EventArgs e)
        {
            Contact me = ClientController.GetInstance().Me;
            ProfileDialog profile = new ProfileDialog();

            if (profile.ShowDialog(this) == DialogResult.OK)
            {
                me.Name = profile.Controls["txtName"].Text;
                Color color = ((Button)profile.Controls["btnColor"]).BackColor;
                me.Color = System.Windows.Media.Color.FromRgb(color.R, color.G, color.B);
            }
        }

        private void mnuViewContactList_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuViewContactList.Checked)
                tabViews.Controls.Add(View.GetInstance<ContactListView>());
            else
                tabViews.Controls.Remove(View.GetInstance<ContactListView>());
        }

        private void mnuViewEyeTrackerLog_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuViewEyeTrackerLog.Checked)
                tabViews.Controls.Add(View.GetInstance<EyeTrackerLogView>());
            else
                tabViews.Controls.Remove(View.GetInstance<EyeTrackerLogView>());
        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {
            mnuViewEventLog.Checked = true;
            mnuViewContactList.Checked = true;
        }

        private void mnuClientSettings_Click(object sender, EventArgs e)
        {
            SettingsDialog profile = new SettingsDialog();

            if (profile.ShowDialog(this) == DialogResult.OK)
            {
                ClientController.GetInstance().ServiceAddress = profile.Controls["txtServiceAddress"].Text;

            }
        }

        private void mnuConnect_Click(object sender, EventArgs e)
        {
            if (ClientController.GetInstance().Connect())
            {
                mnuClientConnect.Enabled = false;
                mnuClientDisconnect.Enabled = true;
                stsStrip.Text = "Connected";
                Text = "OpenMessenger - " + ClientController.GetInstance().Me;
            }
        }

        private void mnuOmniShow_CheckedChanged(object sender, EventArgs e)
        {
            SetOmniVisible(mnuOmniShow.Checked);              
        }

        /// <summary>
        /// Set the visibility of the Omni window
        /// </summary>
        /// <param name="visible">True if visible</param>
        public void SetOmniVisible(bool visible)
        {
            mnuOmniShow.Checked = visible;

            if (visible)
            {
                _omni.Show();
            }
            else
            {
                _omni.Hide();
            }
        }

        private void mnuSensorWebCam_CheckedChanged(object sender, EventArgs e)
        {
            if (mnuSensorWebCam.Checked)
                Sensor.GetInstance<WebcamSensor>().Start();
            else
                Sensor.GetInstance<WebcamSensor>().Stop();
        }

        private void mnuClientDisconnect_Click(object sender, EventArgs e)
        {
            if (ClientController.GetInstance().Disconnect())
            {
                mnuClientConnect.Enabled = true;
                mnuClientDisconnect.Enabled = false;
                stsStrip.Text = "Disconnected";
                Text = "OpenMessenger";
            }
        }

        private void activeWindowToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (activeWindowToolStripMenuItem.Checked)
                Sensor.GetInstance<ActiveWindowSensor>().Start();
            else
                Sensor.GetInstance<ActiveWindowSensor>().Stop();
        }
    }
}
