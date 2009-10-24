using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows.Forms;

using OpenMessenger.Events;
using OpenMessenger.Client.Dialogs;

namespace OpenMessenger.Client
{
    /// <summary>
    /// Controller for the client interface. This is the class responsible for incoming/outgoing
    /// communication at the client.
    /// </summary>
    public class ClientController
    {
        /// <summary>
        /// Delegate for the OutgoingEventUnicast event
        /// </summary>
        /// <param name="contact">Requested recipient</param>
        /// <param name="e">Event sent</param>
        public delegate void OutgoingEventUnicastHandler(Contact contact, Event e);

        /// <summary>
        /// Delegate for the OutgoingEventMulticast event
        /// </summary>
        /// <param name="contacts">Requested list of recipients</param>
        /// <param name="e">Event sent</param>
        public delegate void OutgoingEventMulticastHandler(List<Contact> contacts, Event e);

        /// <summary>
        /// Delegate for the OutgoingEventBroadcast event
        /// </summary>
        /// <param name="e">Event sent</param>
        public delegate void OutgoingEventBroadcastHandler(Event e);

        /// <summary>
        /// Delegate for the Event event
        /// </summary>
        /// <param name="e">Event received</param>
        public delegate void EventHandler(Event e);

        /// <summary>
        /// Delegate for the FocusSet event
        /// </summary>
        /// <param name="contactId">Target of focus</param>
        /// <param name="level">Level of focus</param>
        public delegate void FocusSetHandler(Guid contactId, double level);

        /// <summary>
        /// Triggered on sending an event as unicast
        /// </summary>
        public event OutgoingEventUnicastHandler OutgoingEventUnicast;

        /// <summary>
        /// Triggered on sending an event as multicast
        /// </summary>
        public event OutgoingEventMulticastHandler OutgoingEventMulticast;

        /// <summary>
        /// Triggered on sending an event as broadcast
        /// </summary>
        public event OutgoingEventBroadcastHandler OutgoingEventBroadcast;

        /// <summary>
        /// Triggered when a new event is received
        /// </summary>
        public event EventHandler Event;

        /// <summary>
        /// Triggered when the client notifies the service that it wants to focus on another contact
        /// </summary>
        public event FocusSetHandler FocusSet;

        static ClientController _instance;

        IService _service;
        ServiceHost _clientHost;

        bool _connected;
        string _serviceAddress = "http://localhost:8000/OpenMessenger";

        /// <summary>
        /// The contact representation of this client
        /// </summary>
        public Contact Me { get; set; }


        ContactSet _contacts;
        Dictionary<Guid, ConversationDialog> _conversations = new Dictionary<Guid, ConversationDialog>();

        Graph.Graph _contactGraph = new Graph.Graph();

        /// <summary>
        /// True if currently connected to service
        /// </summary>
        public bool Connected
        {
            get { return _connected; }
        }

        /// <summary>
        /// Set of contacts
        /// </summary>
        public ContactSet Contacts
        {
            
            get { return _contacts; }
        }

        /// <summary>
        /// Open conversation dialogs with other contacts
        /// </summary>
        public Dictionary<Guid, ConversationDialog> Conversations
        {
            get { return _conversations; }
        }

        /// <summary>
        /// Address of service
        /// </summary>
        public string ServiceAddress
        {
            get { return _serviceAddress; }
            set { _serviceAddress = value; }
        }

        private ClientController()
        {
            Me = Contact.Create();
            _contacts = new ContactSet(this);
        }

        /// <summary>
        /// Gets the singleton instance of the ClientController
        /// </summary>
        /// <returns>The singleton instance</returns>
        public static ClientController GetInstance()
        {
            if (_instance == null)
                _instance = new ClientController();

            return _instance;
        }

        /// <summary>
        /// Connect to the service
        /// </summary>
        /// <returns>True if successfully signed in</returns>
        public bool Connect()
        {
            //ConsoleWriteLine("ClientController: Attempting connect.");
            if (Connected)
                throw new ApplicationException("Attempted to connect while already connected.");

            try
            {
                _clientHost = new ServiceHost(typeof(OpenMessenger.Client.Client), new Uri(Me.Address));

                try
                {
                    _clientHost.AddServiceEndpoint(
                        typeof(OpenMessenger.IClient),
                        new WSHttpBinding(),
                        Me.Address);

                    // enable metadata exchange.
                    ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                    smb.HttpGetEnabled = true;
                    _clientHost.Description.Behaviors.Add(smb);

                    _clientHost.Open();
                }
                catch (CommunicationException ce)
                {
                    MessageBox.Show("Communcation Error: {0}", ce.Message);
                    _clientHost.Abort();
                }

                _service = ChannelFactory<IService>.CreateChannel(
                    new WSHttpBinding(),
                    new EndpointAddress(_serviceAddress));

                _service.SignIn(Me);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error signing into service: " + e.Message);
                return false;
            }

            _connected = true;

            return true;
        }

        /// <summary>
        /// Disconnects from service
        /// </summary>
        /// <returns>True if successful</returns>
        public bool Disconnect()
        {
            if (!Connected)
                throw new ApplicationException("Not connected");

            _service.SignOut(Me.Id);

            _clientHost.Close();

            _contacts.Clear();
            
            _connected = false;

            return true;
        }

        /// <summary>
        /// Broadcasts an event to the service.
        /// </summary>
        /// <param name="e">Event to broadcast</param>
        public void BroadcastEvent(Event e)
        {
            if (e is AmplitudeEvent)
            {
                //ConsoleWriteLine("ClientController: AmplitudeEvent Recieved from Sensor!");
            }
            if (Connected)
            {
                _service.BroadcastEvent(e);
                //ConsoleWriteLine("             Sent to Service.....");
                if (OutgoingEventBroadcast != null)
                    OutgoingEventBroadcast(e);
            }
        }

        /// <summary>
        /// Unicast an event
        /// </summary>
        /// <param name="recipient">ID of recipient</param>
        /// <param name="e">Event to send</param>
        public void SendEvent(Guid recipient, Event e)
        {
            if (Connected)
            {
                _service.SendEvent(recipient, e);

                if (OutgoingEventUnicast != null)
                    OutgoingEventUnicast(Contacts[recipient], e);
            }
        }

        /// <summary>
        /// Receive an event, called by the interface
        /// </summary>
        /// <param name="e">Event received</param>
        public void DeliverEvent(Event e)
        {
            //ConsoleWriteLine("DELIVEREVENT");
            if (e is MessageEvent)
                ShowConversationDialog(e.Sender);
            if (e is AmplitudeEvent)
            {
                //ConsoleWriteLine("OTHERSIDE ClientController: AmplitudeEvent Recieved!");
            }
            if (Event != null)
                Event(e);
        }

        /// <summary>
        /// Displays the conversation dialog with a given contact
        /// </summary>
        /// <param name="contactId">ID of conversing contact</param>
        public void ShowConversationDialog(Guid contactId)
        {
            if (!Conversations.ContainsKey(contactId))
            {
                Contact contact = Contacts[contactId];

                ConversationDialog dialog = new ConversationDialog(contact);
                Conversations.Add(contactId, dialog);

                dialog.Show();
            }
            else
            {
                Conversations[contactId].Focus();
            }
        }

        /// <summary>
        /// Tell service that we want to focus on a contact
        /// </summary>
        /// <param name="contact">ID of contact to focus on</param>
        /// <param name="level">Level of focus</param>
        public void SetFocus(Guid contact, double level)
        {
            double newLev = (level > 5) ? 5 : (level < 0) ? 0 : level;
//            //ConsoleWriteLine("ClientController: FocusSet called from "+Me.Id+ " to " +contact);
            if (Connected)
            {
//                //ConsoleWriteLine("ClientController: Calling Service.SetFocus...");
                _service.SetFocus(Me.Id, contact, newLev);
                _contacts.UpdateFocus(Me.Id, contact, newLev);
            }
        }
    }
}
