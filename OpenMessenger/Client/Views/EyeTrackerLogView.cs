using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace OpenMessenger.Client.Views
{
    /// <summary>
    /// A simple view that shows the readings obtained from the eye tracker and displays whether
    /// those readings match an avatar on the OmniWindow
    /// </summary>
    public partial class EyeTrackerLogView : View
    {
        /// <summary>
        /// Name of this view
        /// </summary>
        public override string Text
        {
            get { return "Eye Tracker Log"; }
        }

        /// <summary>
        /// Default constructor - receive all events
        /// </summary>
        public EyeTrackerLogView()
        {
            InitializeComponent();
            InitializeEyePosListView();

            ClientController client = ClientController.GetInstance();
            client.Event += new ClientController.EventHandler(EventHandler);
            client.OutgoingEventBroadcast += new ClientController.OutgoingEventBroadcastHandler(OnEventBroadcastHandler);
        }

        /// <summary>
        /// If an event from the eye tracker is received, process it
        /// </summary>
        private void EventHandler(Event e)
        {
            if (e is Events.EyeActivityEvent)
            {
                AddReading((Events.EyeActivityEvent) e);
            }
        }

        private void OnEventBroadcastHandler(Event e)
        {
            if (InvokeRequired) // child safety
                Invoke(new Action(delegate() { EventHandler(e); }));
            else
                EventHandler(e);
        }


        /// <summary>
        /// Add received data to the listview
        /// </summary>
        private void AddReading(Events.EyeActivityEvent e)
        {
            String[] items = new String[6] { e.SceneNum.ToString(), e.XIn.ToString(), e.YIn.ToString(), e.XPx.ToString(), e.YPx.ToString(), /*e.AvatarHit.Name*/ e.AvatarHitName };
            ListViewItem lvi = new ListViewItem(items);
            listView2.Items.Add(lvi);
            listView2.Items[listView2.Items.Count - 1].EnsureVisible();
            listView2.Refresh();
        }

        private void InitializeEyePosListView()
        {
            listView2.Columns.Add("Scene", 50);
            listView2.Columns.Add("X (in)", 50);
            listView2.Columns.Add("Y (in)", 50);
            listView2.Columns.Add("X (px)", 50);
            listView2.Columns.Add("Y (px)", 50);
            listView2.Columns.Add("Matched avatar", 120);
        }

        private void EyeTrackerDialog_OnClose(object sender, EventArgs e)
        {

        }

    }
}
