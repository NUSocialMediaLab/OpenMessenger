using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenMessenger.Server.Views
{
    public partial class EventLogView : View
    {
        /// <summary>
        /// Name of this view
        /// </summary>
        public override string Text
        {
            get { return "Server Event Log"; }
        }
       
        /// <summary>
        /// Default constructor
        /// </summary>
        public EventLogView()
        {
            InitializeComponent();

            Server server = Server.GetInstance();
            
            server.ServerStatus += new Server.ServerStatusHandler(OnServerStatusEventsHandler);

            ServiceController controller = ServiceController.GetInstance();

            controller.ContactRemove += new ServiceController.ContactRemoveHandler(ContactRemoveHandler);
            controller.ContactUpdate += new ServiceController.ContactUpdateHandler(ContactUpdateHandler);
            controller.IncommingEvent += new ServiceController.IncomingEventHandler(IncomingEventHandler);
            controller.OutgoingEvent += new ServiceController.OutgoingEventHandler(OutgoingEventHandler);
            controller.RequestError += new ServiceController.RequestErrorHandler(ErroneousRequestHandler);
        }

        void ErroneousRequestHandler(string message)
        {
            lstIncomingEvents.Items.Add("[Error] " + message); 
        }

        void OutgoingEventHandler(Contact recipient, Event e)
        {
            lstOutgoingEvents.Items.Add("[" + recipient + "] <= " + e);
        }

        void IncomingEventHandler(Contact sender, Event e)
        {
            lstIncomingEvents.Items.Add("[" + sender + "] => " + e);
        }

        void ContactUpdateHandler(Contact contact)
        {
            lstIncomingEvents.Items.Add("[Contact Update] " + contact);
        }

        void ContactRemoveHandler(Contact contact)
        {
            lstIncomingEvents.Items.Add("[Contact Remove] " + contact);
        }

        private void OnServerStatusEventsHandler(string message)
        {
            lstIncomingEvents.Items.Add("[Status] " + message);
        }
    }
}
