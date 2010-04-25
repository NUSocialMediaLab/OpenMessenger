using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows.Forms;

namespace OpenMessenger.Client
{
    /// <summary>
    /// Main class for the client. Keeps track of service communication and contact list. Responsible
    /// for handling incomming events.
    /// </summary>
    public class Client : IClient
    {
        /// <summary>
        /// Controller for the client interface (singleton instance)
        /// </summary>
        public ClientController Controller
        {
            get { return ClientController.GetInstance(); }
        }

        /// <summary>
        /// Called by service (or potentially other clients) to deliver an event to the client
        /// </summary>
        /// <param name="e">Event to deliver</param>
        public void DeliverEvent(Event e)
        {
            Controller.DeliverEvent(e);
        }

        /// <summary>
        /// Called by the service to notify of an update/addition to/of a contact
        /// </summary>
        /// <param name="contact">Contact to update</param>
        public void UpdateContact(Contact contact)
        {
            Console.WriteLine("Client is calling ContactSet.Update, called from "+Controller.Me.Id+" on " +contact.Id);
            Controller.Contacts.Update(contact);
        }

        /// <summary>
        /// Called by the service to notify that a contact has signed off
        /// </summary>
        /// <param name="contactId">ID of contact to remove</param>
        public void RemoveContact(Guid contactId)
        {
            Controller.Contacts.Remove(contactId);
        }

        /// <summary>
        /// Called by the service to notify that a contact changed its focus level towards another contact
        /// </summary>
        /// <param name="contactA">ID of contact focusing</param>
        /// <param name="contactB">ID of contact being focused on</param>
        /// <param name="level">Level of focus</param>
        public void UpdateFocus(Guid contactA, Guid contactB, double level)
        {
            Controller.Contacts.UpdateFocus(contactA, contactB, level);
        }
    }
}
