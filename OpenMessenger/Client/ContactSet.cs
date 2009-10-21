using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenMessenger.Events;

namespace OpenMessenger.Client
{
    /// <summary>
    /// Contact set for clients. Keeps a list of contacts and a graph including the client's contact
    /// with edges representing focus levels.
    /// </summary>
    public class ContactSet
    {
        #region Events

        /// <summary>
        /// Delegate for the ContactUpdated event
        /// </summary>
        /// <param name="contact">Contact updated</param>
        public delegate void ContactUpdatedHandler(Contact contact);

        /// <summary>
        /// Delegate for the ContactRemoved event
        /// </summary>
        /// <param name="contact">Contact removed</param>
        public delegate void ContactRemovedHandler(Contact contact);

        /// <summary>
        /// Delegate for the FocusUpdated event
        /// </summary>
        /// <param name="contactA">ID of contact focusing</param>
        /// <param name="contactB">ID of contact being focused on</param>
        /// <param name="level">Level of focus</param>
        public delegate void FocusUpdatedHandler(Guid contactA, Guid contactB, double level);

        /// <summary>
        /// Triggered when a contact was updated by the contact set
        /// </summary>
        public event ContactUpdatedHandler ContactUpdated;

        /// <summary>
        /// Triggered when a contact was removed from the contact set
        /// </summary>
        public event ContactRemovedHandler ContactRemoved;

        /// <summary>
        /// Triggered when the focus level between two contacts was updated
        /// </summary>
        public event FocusUpdatedHandler FocusUpdated;

        #endregion

        Dictionary<Guid, Contact> _contacts = new Dictionary<Guid, Contact>();
        Graph.Graph _contactGraph = new Graph.Graph();
        Graph.Graph.Node _myNode;

        /// <summary>
        /// The node representing the client's contact in the focus graph
        /// </summary>
        public Graph.Graph.Node MyNode
        {
            get { return _myNode; }
        }

        /// <summary>
        /// Indexer retrieving the contact by its ID
        /// </summary>
        /// <param name="id">ID of contact to retrieve</param>
        /// <returns>A contact, or null</returns>
        public Contact this[Guid id]
        {
            get
            {
                if (Contains(id))
                    return _contacts[id];
                else if (ClientController.GetInstance().Me.Id == id)
                {
                    return ClientController.GetInstance().Me;
                }

                return null;
            }
        }

        /// <summary>
        /// Indexes retrieving the contact that is the sender of an event
        /// </summary>
        /// <param name="e">A received event</param>
        /// <returns>Contact that sent the event</returns>
        public Contact this[Event e]
        {
            get { return this[e.Sender]; }
        }

        /// <summary>
        /// Graph reflecting the focus levels between the contacts
        /// </summary>
        public Graph.Graph FocusGraph
        {
            get { return _contactGraph; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Controller for the client</param>
        public ContactSet(ClientController client)
        {
            _myNode = new Graph.Graph.Node(_contactGraph, client.Me.Id);
            _contactGraph.AddNode(_myNode);
        }

        /// <summary>
        /// Clears the contact set
        /// </summary>
        public void Clear()
        {
            lock (this)
            {
                _contacts.Clear();
                _contactGraph.Clear();
                _contactGraph.AddNode(MyNode);
            }
        }


        /// <summary>
        /// Updates a contact
        /// </summary>
        /// <param name="contact">Contact to update</param>
        public void Update(Contact contact)
        {
            if (contact.Equals(ClientController.GetInstance().Me))
                return;

            lock (this)
            {
                if (Contains(contact))
                {
                    // update
                    _contacts[contact.Id] = contact;
                }
                else
                {
                    // new
                    Console.WriteLine("Client.ContactSet adding new node ID: "+contact.Id+" to count: "+ _contacts.Count);
                    _contacts.Add(contact.Id, contact);
                    _contactGraph.AddNode(contact.Id);
                    Console.WriteLine("Client.ContactSet about to connect "+((Guid)MyNode.Content)+" to " +contact.Id);
                    _contactGraph.Connect(MyNode, GetNode(contact));
                    _contactGraph.Connect(GetNode(contact), MyNode);
                    Console.WriteLine("Client.ContactSet Nodes Connected");
                }

                if (ContactUpdated != null)
                    ContactUpdated(contact);
            }
        }

        /// <summary>
        /// Removes a contact
        /// </summary>
        /// <param name="contactId">ID of contact to remove</param>
        /// <returns>Contact removed</returns>
        public Contact Remove(Guid contactId)
        {
            lock (this)
            {
                if (Contains(contactId))
                {
                    Contact contact = _contacts[contactId];

                    _contacts.Remove(contactId);
                    _contactGraph.RemoveNode(GetNode(contactId));

                    if (ContactRemoved != null)
                        ContactRemoved(contact);

                    return contact;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Update the focus between two contacts
        /// </summary>
        /// <param name="contactA">ID of contact focusing</param>
        /// <param name="contactB">ID of contact being focused on</param>
        /// <param name="level">Level of focus</param>
        public void UpdateFocus(Guid contactA, Guid contactB, double level)
        {
            Console.WriteLine("Client.ContactSet: UpdateFocus called from "+contactA+" to "+contactB);
            Console.WriteLine("                   MyNode = " + (Guid)MyNode.Content);

            lock (this)
            {
                Graph.Graph.Node from = GetNode(contactA);
                Graph.Graph.Node to = GetNode(contactB);

                if (from != null && to != null)
                {
                    Graph.Graph.Edge edge = FocusGraph.GetEdge(from, to, true);
                    edge.Weight = level;
                }
            }
        }

        /// <summary>
        /// Get the focus level betwen two contacts
        /// </summary>
        /// <param name="contactA">ID of focusing contact</param>
        /// <param name="contactB">ID of focusee contact</param>
        /// <returns>Level of focus</returns>
        public double GetFocus(Guid contactA, Guid contactB)
        {
            lock (this)
            {
                Console.WriteLine("Client.ContactSet: GetFocus from " + contactA + " to " + contactB);
                Console.WriteLine("                   "+Contains(contactA) + "    " + Contains(contactB));
                Graph.Graph.Node nodeA = GetNode(contactA);
                Graph.Graph.Node nodeB = GetNode(contactB);

                Graph.Graph.Edge edge = nodeA | nodeB;

                if (edge != null)
                {
                    Console.WriteLine("                   Client.GetFocus success: "+edge.Weight);
                    return edge.Weight;
                }
                else
                {
                    Console.WriteLine("                   Client.GetFocus failure, returning 1f");
                    return 1f;
                }
            }
        }

        /// <summary>
        /// Checks if the contact set contains a contact with the given ID
        /// </summary>
        /// <param name="contactId">ID to check</param>
        /// <returns>True if contact exists</returns>
        public bool Contains(Guid contactId)
        {
            return _contacts.ContainsKey(contactId);
        }

        /// <summary>
        /// Checks if the contact set contains the contact instance
        /// </summary>
        /// <param name="contact">Contact to check</param>
        /// <returns>True if contact exists</returns>
        public bool Contains(Contact contact)
        {
            return _contacts.ContainsKey(contact.Id);
        }
        
        Graph.Graph.Node GetNode(Contact contact)
        {
            return _contactGraph.FindNode(contact.Id);
        }

        Graph.Graph.Node GetNode(Guid contactId)
        {
            return _contactGraph.FindNode(contactId);
        }
    }
}
