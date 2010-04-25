using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using OpenMessenger.Events;
using System.Text.RegularExpressions;

namespace OpenMessenger
{
    /// <summary>
    /// Handles communication for the service. Processes incoming and outgoing events.
    /// </summary>
    public class ServiceController
    {
        #region Events

        /// <summary>
        /// Delegate for the ContactUpdate event
        /// </summary>
        /// <param name="contact">Contact updated</param>
        public delegate void ContactUpdateHandler(Contact contact);

        /// <summary>
        /// Delegate for the ContactRemoved event
        /// </summary>
        /// <param name="contact">Contact removed</param>
        public delegate void ContactRemoveHandler(Contact contact);

        /// <summary>
        /// Delegate for the IncomingEvent event
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event received</param>
        public delegate void IncomingEventHandler(Contact sender, Event e);

        /// <summary>
        /// Delegate for the OutgoingEvent event
        /// </summary>
        /// <param name="recipient">Recipient of event</param>
        /// <param name="e">Event sent</param>
        public delegate void OutgoingEventHandler(Contact recipient, Event e);

        /// <summary>
        /// Delgegate for the RequestError event
        /// </summary>
        /// <param name="message">Message supplied by the service</param>
        public delegate void RequestErrorHandler(string message);

        /// <summary>
        /// Triggered when a contact is updated
        /// </summary>
        public event ContactUpdateHandler ContactUpdate;

        /// <summary>
        /// Triggered when a contact is removed
        /// </summary>
        public event ContactRemoveHandler ContactRemove;

        /// <summary>
        /// Triggered when an event is received by the service
        /// </summary>
        public event IncomingEventHandler IncommingEvent;

        /// <summary>
        /// Triggered when an event is sent by the service
        /// </summary>
        public event OutgoingEventHandler OutgoingEvent;

        /// <summary>
        /// Triggered when the service received an erroneous request
        /// </summary>
        public event RequestErrorHandler RequestError;

        #endregion

        static ServiceController _instance;

        Guid _serviceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        ContactSet _contacts = new ContactSet();

        Dictionary<Guid, Dictionary<Type, Event>> _eventCache =
            new Dictionary<Guid, Dictionary<Type, Event>>();

        /// <summary>
        /// Set of contacts kept by the service
        /// </summary>
        public ContactSet Contacts
        {
            get { return _contacts; }
        }

        /// <summary>
        /// Reset the serviec controller
        /// </summary>
        public void Reset()
        {
            _instance = new ServiceController();
        }

        /// <summary>
        /// gets the singleton instance of the controller
        /// </summary>
        /// <returns></returns>
        public static ServiceController GetInstance()
        {
            if (_instance == null)
                _instance = new ServiceController();

            return _instance;
        }

        /// <summary>
        /// update a client in the contact set, called by service interface
        /// </summary>
        /// <param name="contact">contact to update</param>
        public void UpdateContact(Contact contact)
        {
            _contacts.Update(contact);
            Console.WriteLine(" server _contacts.update called....");
            foreach (Contact existingContact in Contacts)
            {
                
                existingContact.Client.UpdateContact(contact);
                Console.Write("   ++   ");
            }

            if (ContactUpdate != null)
                ContactUpdate(contact);
        }

        /// <summary>
        /// Remove a client in the contact set, called by service interface
        /// </summary>
        /// <param name="contactId"></param>
        public void RemoveContact(Guid contactId)
        {
            Contact contact = null;
            Console.WriteLine("{{RemoveContact called}}");
            if ((contact = _contacts.Remove(contactId)) != null)
            {
                if (ContactRemove != null)
                    ContactRemove(contact);

                foreach (Contact existingContact in Contacts)
                    existingContact.Client.RemoveContact(contactId);
            }
        }

        /// <summary>
        /// Transfer the list of contacts to a new client
        /// </summary>
        /// <param name="contact">Client to receive list</param>
        public void SendContacts(Contact contact)
        {
            Console.WriteLine("_server Sending contacts to: " + contact);
            foreach (Contact existingContact in Contacts)
            {
                if (existingContact != contact)
                    contact.Client.UpdateContact(existingContact);
            }
        }
 
        /// <summary>
        /// Process an incoming unicast request
        /// </summary>
        /// <param name="recipient">ID of requested recipient</param>
        /// <param name="e">Event to relay</param>
        public void ProcessUnicastRequest(Guid recipient, Event e)
        {
            if (_contacts.Contains(e.Sender))
            {
                Contact sender = _contacts[e.Sender];

                if (IncommingEvent != null)
                    IncommingEvent(sender, e);

                if (_contacts.Contains(recipient))
                {
                    if (OutgoingEvent != null)
                        OutgoingEvent(_contacts[recipient], e);

                    SendEvent(_contacts[recipient], e);
                }
                else
                {
                    if (RequestError != null)
                        RequestError("Unknown recipient: " + recipient);
                }
            }
            else
            {
                if (RequestError != null)
                    RequestError("Unknown sender: " + e.Sender);
            }
        }

        /// <summary>
        /// Process an incoming multicast request.
        /// </summary>
        /// <param name="recipients">List of requested recipients</param>
        /// <param name="e">Event to relay</param>
        public void ProcessMulticastRequest(List<Guid> recipients, Event e)
        {
            if (_contacts.Contains(e.Sender))
            {
                Contact sender = _contacts[e.Sender];

                if (IncommingEvent != null)
                    IncommingEvent(sender, e);

                foreach (Guid recipient in recipients)
                {
                    if (_contacts.Contains(recipient))
                    {
                        if (OutgoingEvent != null)
                            OutgoingEvent(_contacts[recipient], e);

                        SendEvent(_contacts[recipient], e);
                    }
                }
            }
        }

        /// <summary>
        /// Process an incoming broadcast request
        /// </summary>
        /// <param name="e">Event requested for broadcast</param>
        public void ProcessBroadcastRequest(Event e)
        {
            if (_contacts.Contains(e.Sender))
            {
                if (IncommingEvent != null)
                    IncommingEvent(_contacts[e.Sender], e);

                BroadcastEvent(e);
            }
            else
            {
                if (RequestError != null)
                    RequestError("Unknown sender attempted broadcast: " + e);
            }
        }

        /// <summary>
        /// A client wants to focus on a contact. Called by the service interface
        /// </summary>
        /// <param name="me">ID of contact focusing</param>
        /// <param name="contact">ID of contact to focus on</param>
        /// <param name="level">Requested focus level</param>
        public void SetFocus(Guid me, Guid contact, double level)
        {
            Console.Write(me + " is focusing on " + contact+ "...");
            if (_contacts.UpdateFocus(me, contact, level))
            {
                Console.WriteLine("and update contacts is being called");
                _contacts[contact].Client.UpdateFocus(me, contact, level);
                UpdateContact(_contacts[contact]);
            }
//TODO: Uh, make this work!!!!!
            //foreach (KeyValuePair<Type, Event> pair in _eventCache[me])
            //{
            //    if (_contacts.GetFocus(me, pair.Value.Sender) >= pair.Value.Level)
            //        SendEvent(_contacts[me], pair.Value);
            //}
        }

        void BroadcastEvent(Event e)
        {
            foreach (Contact contact in Contacts)
            {
                SendEvent(contact, e);

                if (OutgoingEvent != null)
                    OutgoingEvent(contact, e);
            }
        }

        void SendEvent(Contact recipient, Event e)
        {
            if (_contacts.GetFocus(recipient.Id, e.Sender) >= e.Level || e is MessageEvent)
                // XXX i'm delivering it anyway if it's a chat msg.
                recipient.Client.DeliverEvent(e);
            else
            {
                Dictionary<Type, Event> t;
                if (_eventCache.ContainsKey(recipient.Id))
                    t = _eventCache[recipient.Id];
                else
                    t = new Dictionary<Type, Event>();

                t.Remove(e.GetType());
                t.Add(e.GetType(), e);
                _eventCache.Remove(recipient.Id);
                _eventCache.Add(recipient.Id, t);
            }
        }
    }
}
