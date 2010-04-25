using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMessenger
{
    /// <summary>
    /// Class implementing the IService interface, essentially forwards request to the ServiceController
    /// </summary>
    public class Service : IService
    {
        /// <summary>
        /// Singleton instance of the controller
        /// </summary>
        public ServiceController Controller
        {
            get { return ServiceController.GetInstance(); }
        }

        /// <summary>
        /// Handles client sign-ins. Updates service contact list and notifies other clients.
        /// </summary>
        /// <param name="contact">Contact information about the client signing in</param>
        public void SignIn(Contact contact)
        {
            Console.WriteLine("signIn called");
            Controller.SendContacts(contact);
            Controller.UpdateContact(contact);
        }

        /// <summary>
        /// Called by a client to sign out
        /// </summary>
        /// <param name="contactId">Contact ID of the client signing out</param>
        public void SignOut(Guid contactId)
        {
            Controller.RemoveContact(contactId);
        }

        /// <summary>
        /// Called by a client to send an event
        /// </summary>
        /// <param name="recipient">ID of event recipient</param>
        /// <param name="e">Event to send</param>
        public void SendEvent(Guid recipient, Event e)
        {
            Controller.ProcessUnicastRequest(recipient, e);
        }

        /// <summary>
        /// Called by a client to send an event to a selected list of contacts
        /// </summary>
        /// <param name="recipients">List of recipients</param>
        /// <param name="e">Event to send</param>
        public void MulticastEvent(List<Guid> recipients, Event e)
        {
            Controller.ProcessMulticastRequest(recipients, e);
        }

        /// <summary>
        /// Called by a client to send an event to everyone in the service
        /// </summary>
        /// <param name="e">Event to send</param>
        public void BroadcastEvent(Event e)
        {
            Controller.ProcessBroadcastRequest(e);
        }

        /// <summary>
        /// Called by a client that wants to notify the service to update its focus level to a different
        /// client
        /// </summary>
        /// <param name="me">ID of the client doing the focusing</param>
        /// <param name="contact">ID of the focusee client</param>
        /// <param name="level">New level of focus</param>
        public void SetFocus(Guid me, Guid contact, double level)
        {
            //ConsoleWriteLine("Service: SetFocus called from "+me+" to " + contact);
            //ConsoleWriteLine("           Now passing on to ServiceController");
            Controller.SetFocus(me, contact, level);
        }
    }
}
