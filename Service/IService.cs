using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using OpenMessenger.Events;

namespace OpenMessenger
{
    /// <summary>
    /// Interface to the service. These are the methods clients can call.
    /// </summary>
    [ServiceContract()]
    public interface IService
    {
        /// <summary>
        /// Gets the controller for this interface
        /// </summary>
        ServiceController Controller { get; }

        /// <summary>
        /// Signs a client into the service
        /// </summary>
        /// <param name="contact">Contact information for client</param>
        [OperationContract(IsOneWay = true)]
        void SignIn(Contact contact);

        /// <summary>
        /// Signs a client out of the service
        /// </summary>
        /// <param name="contactId">ID of client signing out</param>
        [OperationContract(IsOneWay = true)]
        void SignOut(Guid contactId);

        /// <summary>
        /// Called by clients to send an event
        /// </summary>
        /// <param name="recipient">ID of recipient</param>
        /// <param name="e">Event to send</param>
        [OperationContract(IsOneWay = true)]
        void SendEvent(Guid recipient, Event e);

        /// <summary>
        /// Called by clients to send an event to a select list of contacts
        /// </summary>
        /// <param name="recipients">List of contact IDs</param>
        /// <param name="e">Event to send</param>
        [OperationContract(IsOneWay = true)]
        void MulticastEvent(List<Guid> recipients, Event e);

        /// <summary>
        /// Called by clients to send an event to everyone in the system
        /// </summary>
        /// <param name="e">Event to send</param>
        [OperationContract(IsOneWay = true)]
        void BroadcastEvent(Event e);

        /// <summary>
        /// Called by clients to update its focus level towards another contact
        /// </summary>
        /// <param name="me">ID of client</param>
        /// <param name="contact">ID of contact to focus on</param>
        /// <param name="level">Level of focus</param>
        [OperationContract(IsOneWay = true)]
        void SetFocus(Guid me, Guid contact, double level);
    }
}
