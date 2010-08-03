namespace OpenMessenger.Client
{
    partial class ClientWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mnuStrip = new System.Windows.Forms.MenuStrip();
            this.mnuClient = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClientConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClientDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuClientProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClientSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuClientExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSensors = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSensorMicrophone = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSensorScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSensorWebCam = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSensorEyeTracker = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSensorKeyboard = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSensorMouse = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSensorActiveWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSensorIdleTime = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViews = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewEventLog = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewContactList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewEyeTrackerLog = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMonitors = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMonitorMicrophoneAmplitude = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMonitorScreenActivity = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMonitorEyeActivity = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMonitorKeyboardActivity = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMonitorMouseActivity = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOmni = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOmniShow = new System.Windows.Forms.ToolStripMenuItem();
            this.stsStrip = new System.Windows.Forms.StatusStrip();
            this.tabViews = new System.Windows.Forms.TabControl();
            this.mnuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuStrip
            // 
            this.mnuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuClient,
            this.mnuSensors,
            this.mnuViews,
            this.mnuMonitors,
            this.mnuOmni});
            this.mnuStrip.Location = new System.Drawing.Point(0, 0);
            this.mnuStrip.Name = "mnuStrip";
            this.mnuStrip.Size = new System.Drawing.Size(562, 24);
            this.mnuStrip.TabIndex = 1;
            this.mnuStrip.Text = "menuStrip1";
            // 
            // mnuClient
            // 
            this.mnuClient.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuClientConnect,
            this.mnuClientDisconnect,
            this.toolStripSeparator1,
            this.mnuClientProfile,
            this.mnuClientSettings,
            this.toolStripSeparator2,
            this.mnuClientExit});
            this.mnuClient.Name = "mnuClient";
            this.mnuClient.Size = new System.Drawing.Size(46, 20);
            this.mnuClient.Text = "Client";
            // 
            // mnuClientConnect
            // 
            this.mnuClientConnect.Name = "mnuClientConnect";
            this.mnuClientConnect.Size = new System.Drawing.Size(137, 22);
            this.mnuClientConnect.Text = "Connect...";
            this.mnuClientConnect.Click += new System.EventHandler(this.mnuConnect_Click);
            // 
            // mnuClientDisconnect
            // 
            this.mnuClientDisconnect.Enabled = false;
            this.mnuClientDisconnect.Name = "mnuClientDisconnect";
            this.mnuClientDisconnect.Size = new System.Drawing.Size(137, 22);
            this.mnuClientDisconnect.Text = "Disconnect";
            this.mnuClientDisconnect.Click += new System.EventHandler(this.mnuClientDisconnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(134, 6);
            // 
            // mnuClientProfile
            // 
            this.mnuClientProfile.Name = "mnuClientProfile";
            this.mnuClientProfile.Size = new System.Drawing.Size(137, 22);
            this.mnuClientProfile.Text = "Profile...";
            this.mnuClientProfile.Click += new System.EventHandler(this.mnuClientProfile_Click);
            // 
            // mnuClientSettings
            // 
            this.mnuClientSettings.Name = "mnuClientSettings";
            this.mnuClientSettings.Size = new System.Drawing.Size(137, 22);
            this.mnuClientSettings.Text = "Settings...";
            this.mnuClientSettings.Click += new System.EventHandler(this.mnuClientSettings_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(134, 6);
            // 
            // mnuClientExit
            // 
            this.mnuClientExit.Name = "mnuClientExit";
            this.mnuClientExit.Size = new System.Drawing.Size(137, 22);
            this.mnuClientExit.Text = "Exit";
            this.mnuClientExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuSensors
            // 
            this.mnuSensors.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSensorMicrophone,
            this.mnuSensorScreen,
            this.mnuSensorWebCam,
            this.mnuSensorEyeTracker,
<<<<<<< .mine
            this.mnuSensorKeyboard,
            this.mnuSensorMouse,
            this.mnuSensorActiveWindow,
            this.mnuSensorIdleTime});
>>>>>>> .r26
            this.mnuSensors.Name = "mnuSensors";
            this.mnuSensors.Size = new System.Drawing.Size(57, 20);
            this.mnuSensors.Text = "Sensors";
            // 
            // mnuSensorMicrophone
            // 
            this.mnuSensorMicrophone.CheckOnClick = true;
            this.mnuSensorMicrophone.Name = "mnuSensorMicrophone";
            this.mnuSensorMicrophone.Size = new System.Drawing.Size(156, 22);
            this.mnuSensorMicrophone.Text = "Microphone";
            this.mnuSensorMicrophone.CheckedChanged += new System.EventHandler(this.mnuSensorMicrophone_CheckedChanged);
            // 
            // mnuSensorScreen
            // 
            this.mnuSensorScreen.CheckOnClick = true;
            this.mnuSensorScreen.Name = "mnuSensorScreen";
            this.mnuSensorScreen.Size = new System.Drawing.Size(156, 22);
            this.mnuSensorScreen.Text = "Screen";
            this.mnuSensorScreen.CheckedChanged += new System.EventHandler(this.mnuSensorScreen_CheckedChanged);
            // 
            // mnuSensorWebCam
            // 
            this.mnuSensorWebCam.CheckOnClick = true;
            this.mnuSensorWebCam.Name = "mnuSensorWebCam";
            this.mnuSensorWebCam.Size = new System.Drawing.Size(156, 22);
            this.mnuSensorWebCam.Text = "WebCam";
            this.mnuSensorWebCam.CheckedChanged += new System.EventHandler(this.mnuSensorWebCam_CheckedChanged);
            // 
            // mnuSensorEyeTracker
            // 
            this.mnuSensorEyeTracker.CheckOnClick = true;
            this.mnuSensorEyeTracker.Name = "mnuSensorEyeTracker";
            this.mnuSensorEyeTracker.Size = new System.Drawing.Size(156, 22);
            this.mnuSensorEyeTracker.Text = "Eye Tracker";
            this.mnuSensorEyeTracker.CheckedChanged += new System.EventHandler(this.mnuSensorEyeTracker_CheckedChanged);
            // 
            // mnuSensorKeyboard
            // 
            this.mnuSensorKeyboard.CheckOnClick = true;
            this.mnuSensorKeyboard.Name = "mnuSensorKeyboard";
            this.mnuSensorKeyboard.Size = new System.Drawing.Size(156, 22);
            this.mnuSensorKeyboard.Text = "Keyboard";
            this.mnuSensorKeyboard.CheckedChanged += new System.EventHandler(this.mnuSensorKeyboard_CheckedChanged);
            //
            // mnSensorMouse
            //
            this.mnuSensorMouse.CheckOnClick = true;
            this.mnuSensorMouse.Name = "mnuSensorMouse";
            this.mnuSensorMouse.Size = new System.Drawing.Size(152, 22);
            this.mnuSensorMouse.Text = "Mouse";
            this.mnuSensorMouse.CheckedChanged += new System.EventHandler(this.mnuSensorMouse_CheckChanged);
            // 
            // mnuSensorActiveWindow
            // 
            this.mnuSensorActiveWindow.CheckOnClick = true;
            this.mnuSensorActiveWindow.Name = "mnuSensorActiveWindow";
            this.mnuSensorActiveWindow.Size = new System.Drawing.Size(156, 22);
            this.mnuSensorActiveWindow.Text = "Active Window";
            this.mnuSensorActiveWindow.CheckedChanged += new System.EventHandler(this.mnuSensorActiveWindow_CheckedChanged);
            // 
            // mnuSensorIdleTime
            // 
            this.mnuSensorIdleTime.CheckOnClick = true;
            this.mnuSensorIdleTime.Name = "mnuSensorIdleTime";
            this.mnuSensorIdleTime.Size = new System.Drawing.Size(156, 22);
            this.mnuSensorIdleTime.Text = "Idle Time";
            this.mnuSensorIdleTime.CheckedChanged += new System.EventHandler(this.mnuSensorIdleTime_CheckedChanged);
            // 
            // mnuViews
            // 
            this.mnuViews.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewEventLog,
            this.mnuViewContactList,
            this.mnuViewEyeTrackerLog});
            this.mnuViews.Name = "mnuViews";
            this.mnuViews.Size = new System.Drawing.Size(46, 20);
            this.mnuViews.Text = "Views";
            // 
            // mnuViewEventLog
            // 
            this.mnuViewEventLog.CheckOnClick = true;
            this.mnuViewEventLog.Name = "mnuViewEventLog";
            this.mnuViewEventLog.Size = new System.Drawing.Size(162, 22);
            this.mnuViewEventLog.Text = "Event Log";
            this.mnuViewEventLog.CheckedChanged += new System.EventHandler(this.mnuViewEventLog_CheckedChanged);
            // 
            // mnuViewContactList
            // 
            this.mnuViewContactList.CheckOnClick = true;
            this.mnuViewContactList.Name = "mnuViewContactList";
            this.mnuViewContactList.Size = new System.Drawing.Size(162, 22);
            this.mnuViewContactList.Text = "Contact List";
            this.mnuViewContactList.CheckedChanged += new System.EventHandler(this.mnuViewContactList_CheckedChanged);
            // 
            // mnuViewEyeTrackerLog
            // 
            this.mnuViewEyeTrackerLog.CheckOnClick = true;
            this.mnuViewEyeTrackerLog.Name = "mnuViewEyeTrackerLog";
            this.mnuViewEyeTrackerLog.Size = new System.Drawing.Size(162, 22);
            this.mnuViewEyeTrackerLog.Text = "Eye Tracker Log";
            this.mnuViewEyeTrackerLog.CheckedChanged += new System.EventHandler(this.mnuViewEyeTrackerLog_CheckedChanged);
            // 
            // mnuMonitors
            // 
            this.mnuMonitors.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuMonitorMicrophoneAmplitude,
            this.mnuMonitorScreenActivity,
            this.mnuMonitorEyeActivity,
            this.mnuMonitorKeyboardActivity,
            this.mnuMonitorMouseActivity});
            this.mnuMonitors.Name = "mnuMonitors";
            this.mnuMonitors.Size = new System.Drawing.Size(60, 20);
            this.mnuMonitors.Text = "Monitors";
            // 
            // mnuMonitorMicrophoneAmplitude
            // 
            this.mnuMonitorMicrophoneAmplitude.CheckOnClick = true;
            this.mnuMonitorMicrophoneAmplitude.Name = "mnuMonitorMicrophoneAmplitude";
            this.mnuMonitorMicrophoneAmplitude.Size = new System.Drawing.Size(190, 22);
            this.mnuMonitorMicrophoneAmplitude.Text = "Microphone Amplitude";
            this.mnuMonitorMicrophoneAmplitude.CheckedChanged += new System.EventHandler(this.mnuMonitorMicrophoneAmplitude_CheckedChanged);
            // 
            // mnuMonitorScreenActivity
            // 
            this.mnuMonitorScreenActivity.CheckOnClick = true;
            this.mnuMonitorScreenActivity.Name = "mnuMonitorScreenActivity";
            this.mnuMonitorScreenActivity.Size = new System.Drawing.Size(190, 22);
            this.mnuMonitorScreenActivity.Text = "Activity";
            this.mnuMonitorScreenActivity.CheckedChanged += new System.EventHandler(this.mnuMonitorScreenActivity_CheckedChanged);
            // 
            // mnuMonitorEyeActivity
            // 
            this.mnuMonitorEyeActivity.CheckOnClick = true;
            this.mnuMonitorEyeActivity.Name = "mnuMonitorEyeActivity";
            this.mnuMonitorEyeActivity.Size = new System.Drawing.Size(190, 22);
            this.mnuMonitorEyeActivity.Text = "Eye Activity";
            this.mnuMonitorEyeActivity.CheckedChanged += new System.EventHandler(this.mnuMonitorEyeActivity_CheckedChanged);
            // 
            // mnuMonitorKeyboardActivity
            // 
            this.mnuMonitorKeyboardActivity.CheckOnClick = true;
            this.mnuMonitorKeyboardActivity.Name = "mnuMonitorKeyboardActivity";
            this.mnuMonitorKeyboardActivity.Size = new System.Drawing.Size(190, 22);
            this.mnuMonitorKeyboardActivity.Text = "Keyboard Activity";
            this.mnuMonitorKeyboardActivity.CheckedChanged += new System.EventHandler(this.mnuMonitorKeyboardAcitivity_CheckedChanged);
            //
            // mnuMonitorMouseActivity
            //
            this.mnuMonitorMouseActivity.CheckOnClick = true;
            this.mnuMonitorMouseActivity.Name = "mnuMonitorMouseActivity";
            this.mnuMonitorMouseActivity.Size = new System.Drawing.Size(198, 22);
            this.mnuMonitorMouseActivity.Text = "Mouse Activity";
            this.mnuMonitorMouseActivity.CheckedChanged += new System.EventHandler(this.mnuMonitorMouseActivity_CheckedChanged);
            // 
            // mnuOmni
            // 
            this.mnuOmni.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOmniShow});
            this.mnuOmni.Name = "mnuOmni";
            this.mnuOmni.Size = new System.Drawing.Size(43, 20);
            this.mnuOmni.Text = "Omni";
            // 
            // mnuOmniShow
            // 
            this.mnuOmniShow.CheckOnClick = true;
            this.mnuOmniShow.Name = "mnuOmniShow";
            this.mnuOmniShow.Size = new System.Drawing.Size(179, 22);
            this.mnuOmniShow.Text = "Show Omni Window";
            this.mnuOmniShow.CheckedChanged += new System.EventHandler(this.mnuOmniShow_CheckedChanged);
            // 
            // stsStrip
            // 
            this.stsStrip.Location = new System.Drawing.Point(0, 415);
            this.stsStrip.Name = "stsStrip";
            this.stsStrip.Size = new System.Drawing.Size(562, 22);
            this.stsStrip.TabIndex = 2;
            this.stsStrip.Text = "statusStrip1";
            // 
            // tabViews
            // 
            this.tabViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabViews.Location = new System.Drawing.Point(0, 24);
            this.tabViews.Name = "tabViews";
            this.tabViews.SelectedIndex = 0;
            this.tabViews.Size = new System.Drawing.Size(562, 391);
            this.tabViews.TabIndex = 3;
            // 
            // ClientWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 437);
            this.Controls.Add(this.tabViews);
            this.Controls.Add(this.stsStrip);
            this.Controls.Add(this.mnuStrip);
            this.MainMenuStrip = this.mnuStrip;
            this.Name = "ClientWindow";
            this.Text = "Open Messenger";
            this.Load += new System.EventHandler(this.ClientWindow_Load);
            this.mnuStrip.ResumeLayout(false);
            this.mnuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuStrip;
        private System.Windows.Forms.StatusStrip stsStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuClient;
        private System.Windows.Forms.ToolStripMenuItem mnuClientConnect;
        private System.Windows.Forms.ToolStripMenuItem mnuSensors;
        private System.Windows.Forms.ToolStripMenuItem mnuClientDisconnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuClientExit;
        private System.Windows.Forms.ToolStripMenuItem mnuViews;
        private System.Windows.Forms.ToolStripMenuItem mnuViewEventLog;
        private System.Windows.Forms.ToolStripMenuItem mnuMonitors;
        private System.Windows.Forms.ToolStripMenuItem mnuMonitorMicrophoneAmplitude;
        private System.Windows.Forms.ToolStripMenuItem mnuSensorMicrophone;
        private System.Windows.Forms.ToolStripMenuItem mnuSensorScreen;
        private System.Windows.Forms.ToolStripMenuItem mnuMonitorScreenActivity;
        private System.Windows.Forms.TabControl tabViews;
        private System.Windows.Forms.ToolStripMenuItem mnuClientProfile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuViewContactList;
        private System.Windows.Forms.ToolStripMenuItem mnuClientSettings;
        private System.Windows.Forms.ToolStripMenuItem mnuOmni;
        private System.Windows.Forms.ToolStripMenuItem mnuOmniShow;
        private System.Windows.Forms.ToolStripMenuItem mnuSensorWebCam;
        private System.Windows.Forms.ToolStripMenuItem mnuViewEyeTrackerLog;
        private System.Windows.Forms.ToolStripMenuItem mnuSensorEyeTracker;
        private System.Windows.Forms.ToolStripMenuItem mnuMonitorEyeActivity;
        private System.Windows.Forms.ToolStripMenuItem mnuSensorMouse;
        private System.Windows.Forms.ToolStripMenuItem mnuMonitorMouseActivity;
        private System.Windows.Forms.ToolStripMenuItem mnuSensorKeyboard;
        private System.Windows.Forms.ToolStripMenuItem mnuMonitorKeyboardActivity;
        private System.Windows.Forms.ToolStripMenuItem mnuSensorActiveWindow;
        private System.Windows.Forms.ToolStripMenuItem mnuSensorIdleTime;
    }
}