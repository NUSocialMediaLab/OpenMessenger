using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenMessenger.Client.Views
{
    /// <summary>
    /// View logging events for the client
    /// </summary>
    public partial class EventLogView : View
    {
        Guid _serviceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        /// <summary>
        /// Name of this view
        /// </summary>
        public override string Text
        {
            get { return "Event Log"; }
        }
       
        /// <summary>
        /// Default constructor
        /// </summary>
        public EventLogView()
        {
            InitializeComponent();

            ClientController client = ClientController.GetInstance();

            client.Event += new ClientController.EventHandler(EventHandler);

            // outgoing events
            client.OutgoingEventBroadcast += new ClientController.OutgoingEventBroadcastHandler(OnEventBroadcastHandler);

        }

        void EventHandler(Event e)
        {
            LogEvent(e);
        }

        private void OnEventBroadcastHandler(Event e)
        {
            if (InvokeRequired) // child safety
                Invoke(new Action(delegate() { LogEvent(e); })); 
            else
                LogEvent(e);
        }

        private void LogEvent(Event e)
        {
            lstEvents.Items.Add(EventEntryAsString(e));
        }

        private string EventEntryAsString(Event e)
        {
            StringBuilder builder = new StringBuilder(e.Timestamp.TimeOfDay.ToString());

            builder.Append(" [" + GetEventSender(e) + "] ");

            if (e.Sender == ClientController.GetInstance().Me.Id)
                builder.Append(" => ");
            else
                builder.Append(" <= ");

            builder.Append(e);

            return builder.ToString();
        }

        private string GetEventSender(Event e)
        {
            if (e.Sender == _serviceId)
                return "Server";
            else
            {
                ClientController controller = ClientController.GetInstance();

                if (e.Sender == controller.Me.Id)
                    return controller.Me.Name;
                else if (controller.Contacts.Contains(e.Sender))
                    return controller.Contacts[e.Sender].Name;
                else
                    return "Unknown";
            }
        }
    }
}
