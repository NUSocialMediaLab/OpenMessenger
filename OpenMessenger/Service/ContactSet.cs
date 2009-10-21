using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMessenger
{
    /// <summary>
    /// Set of contacts for the service
    /// </summary>
    public class ContactSet : IEnumerable<Contact>
    {
        Dictionary<Guid, Contact> _contacts = new Dictionary<Guid, Contact>();
        Dictionary<Pair<Guid, Guid>, double> _focusMatrix = new Dictionary<Pair<Guid, Guid>, double>();
        //Dictionary<Guid, Dictionary<Guid,double>> _focusMatrix = new Dictionary<Guid, Dictionary<Guid,double>>();
      

        /// <summary>
        /// Index the contact set by a contact ID
        /// </summary>
        /// <param name="contactId">ID to lookup</param>
        /// <returns>Contact with the ID</returns>
        public Contact this[Guid contactId]
        {
            get 
            {
                return _contacts[contactId];
            }
        }

        /// <summary>
        /// Updates/Adds a contact
        /// </summary>
        /// <param name="contact">Contact to update/add</param>
        public void Update(Contact contact)
        {
            lock (this)
            {
                if (_contacts.ContainsKey(contact.Id))
                {
                    _contacts[contact.Id] = contact;
                    Console.WriteLine("ContactSet.cs.Update(Contact) says: Contact already in");
                }
                else
                {
                    Console.WriteLine("About to add new contact");
                    foreach (Guid id in _contacts.Keys)
                    {
                        _focusMatrix.Add(new Pair<Guid,Guid>(contact.Id, id),0);
                        _focusMatrix.Add(new Pair<Guid, Guid>(id, contact.Id), 0);
                    }
                    _contacts.Add(contact.Id, contact);
                }
            }
        }

        /// <summary>
        /// Removes a contact
        /// </summary>
        /// <param name="contactId">ID of the contact to remove</param>
        /// <returns>The removed contact</returns>
        public Contact Remove(Guid contactId)
        {
            lock (this)
            {
                if (_contacts.ContainsKey(contactId))
                {
                    Contact contact = _contacts[contactId];
                    _contacts.Remove(contactId);

                    List<Pair<Guid, Guid>> removeList = new List<Pair<Guid, Guid>>();
                    foreach (Pair<Guid, Guid> pair in _focusMatrix.Keys)
                    {
                        if (pair.First == contactId || pair.Second == contactId)
                            removeList.Add(pair);
                    }

                    foreach (Pair<Guid, Guid> pair in removeList)
                    {
                        _focusMatrix.Remove(pair);
                    }

                    return contact;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Checks if a contact is present in the set
        /// </summary>
        /// <param name="contactId">ID to check</param>
        /// <returns>True if contact with the ID exists</returns>
        public bool Contains(Guid contactId)
        {
            return _contacts.ContainsKey(contactId);
        }

        bool ComparePairs (Pair<Guid,Guid> p1)
    {
        return true;
    }

        /// <summary>
        /// Updates the focus between two contacts
        /// </summary>
        /// <param name="from">ID of contact focusing</param>
        /// <param name="to">ID of focusee</param>
        /// <param name="level">Level of focus</param>
        /// <returns>True if successful (both contacts exists, and updated)</returns>
        public bool UpdateFocus(Guid from, Guid to, double level)
        {
            //Console.WriteLine("Service: UpdateFocus called from "+from+" to "+to);
            lock (this)
            {
                Pair<Guid, Guid> key = null;
                foreach (Pair<Guid, Guid> p in _focusMatrix.Keys)
                {
                    if (p.First == from && p.Second == to)
                        key = p;
                        
                }
                //Console.WriteLine("         Key is " + key.First + " to " + key.Second);
                if (_focusMatrix.ContainsKey(key))
                {
                    //Console.WriteLine("        Focus level found... changing to "+level);
                    _focusMatrix[key] = level;
                    //Console.WriteLine("        Focus changed to " + _focusMatrix[key]);
                    
                //_contacts[to].Client.UpdateContact(_contacts[from]);
                    return true;
                }
                else
                {
                    Console.WriteLine("        No Focus level Found!!!!!! NumFoci = "+_focusMatrix.Count);
                    Console.WriteLine("  0: "+_focusMatrix.Keys.ElementAt(0).First+"   "+_focusMatrix.Keys.ElementAt(0).Second);
                    Console.WriteLine("  1: "+_focusMatrix.Keys.ElementAt(1).First + "   " + _focusMatrix.Keys.ElementAt(1).Second);
                    return false;
                }
            }
        }
        
        /// <summary>
        /// Gets the focus level between two nodes
        /// </summary>
        /// <param name="from">ID of focusing contact</param>
        /// <param name="to">ID of focusee</param>
        /// <returns>Current focus level, or 1 (default)</returns>
        public double GetFocus(Guid from, Guid to)
        {
            //Pair<Guid, Guid> key = new Pair<Guid, Guid>(from, to);
            Pair<Guid, Guid> key = null;
            //Console.WriteLine("Server: GetFocus CALLED");
            foreach (Pair<Guid, Guid> p in _focusMatrix.Keys)
            {
                if (p.First == from && p.Second == to)
                {
                    //Console.WriteLine("        KEY FOUND IN GETFOCUS");
                    key = p;
                }
            }
            if (key != null && _focusMatrix.ContainsKey(key))
            {
                //Console.WriteLine("        Focus was found: " + key.Equals(null));
                return _focusMatrix[key];
            }
            else
            {
                //Console.WriteLine("        Focus not found");
                return 1f;
            }
        }

        IEnumerator<Contact> IEnumerable<Contact>.GetEnumerator()
        {
            return _contacts.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _contacts.Values.GetEnumerator();
        }
    }
}
