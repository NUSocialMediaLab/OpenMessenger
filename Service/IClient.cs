using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using OpenMessenger.Events;

namespace OpenMessenger
{
    /// <summary>
    /// External service contract for client.
    /// </summary>
    [ServiceContract]
    public interface IClient
    {
        /// <summary>
        /// Deliver an event at the client
        /// </summary>
        /// <param name="e">The event to deliver</param>
        [OperationContract(IsOneWay = true)]
        void DeliverEvent(Event e);

        /// <summary>
        /// Service tells the client to update a contact
        /// </summary>
        /// <param name="contact">Contact to update</param>
        [OperationContract(IsOneWay = true)]
        void UpdateContact(Contact contact);

        /// <summary>
        /// Service tells the client to remove a contact
        /// </summary>
        /// <param name="contactId">ID of contact to remove</param>
        [OperationContract(IsOneWay = true)]
        void RemoveContact(Guid contactId);

        /// <summary>
        /// Service tells the client to update the focus level between two other contacts
        /// </summary>
        /// <param name="contactA">Contact focusing</param>
        /// <param name="contactB">Contact being focused on</param>
        /// <param name="level">Level of focus</param>
        [OperationContract(IsOneWay = true)]
        void UpdateFocus(Guid contactA, Guid contactB, double level);
    }
}
