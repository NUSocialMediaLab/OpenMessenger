using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenMessenger.Events;

namespace OpenMessenger.Client.Views
{
    /// <summary>
    /// A view representing the contact set as a list. Clicking a contact will open a conversation dialog.
    /// </summary>
    public partial class ContactListView : View
    {
        Guid serviceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        /// <summary>
        /// Name of this view
        /// </summary>
        public override string Text
        {
            get { return "Contact List"; }
        }
       
        /// <summary>
        /// Default constructor
        /// </summary>
        public ContactListView()
        {
            InitializeComponent();

            ClientController client = ClientController.GetInstance();

            client.Contacts.ContactUpdated += new ContactSet.ContactUpdatedHandler(UpdateContactHandler);
            client.Contacts.ContactRemoved += new ContactSet.ContactRemovedHandler(RemoveContactHandler);
            client.Event += new ClientController.EventHandler(EventHandler);
        }

        void DoubleClickHandler(object sender, System.EventArgs e)
        {
            ClientController controller = ClientController.GetInstance();
            Contact contact = (Contact)lstContacts.SelectedItem;

            controller.ShowConversationDialog(contact.Id);
        }

        void UpdateContactHandler(Contact contact)
        {
            if (!lstContacts.Items.Contains(contact))
                lstContacts.Items.Add(contact);
        }

        void RemoveContactHandler(Contact contact)
        {
            lstContacts.Items.Remove(contact);
        }

        void EventHandler(Event e)
        {
        
        }
    }
}
