using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenMessenger.Events;

namespace OpenMessenger.Client.Dialogs
{
    public partial class ConversationDialog : Form
    {
        Contact contact;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contact">Contact to converse with</param>
        public ConversationDialog(Contact contact)
        {
            InitializeComponent();

            this.contact = contact;

            ClientController client = ClientController.GetInstance();

            client.Event += new ClientController.EventHandler(OnEvent);
            client.Contacts.ContactRemoved += new ContactSet.ContactRemovedHandler(OnRemoveContact);
            client.Contacts.ContactUpdated += new ContactSet.ContactUpdatedHandler(OnUpdateContact);
        }

        void OnUpdateContact(Contact contact)
        {
            if (contact == this.contact)
            {
                Text = "OM - " + contact.Name;
            }
        }

        void OnRemoveContact(Contact contact)
        {
            if (contact == this.contact)
            {
                Enabled = false;
            }
        }

        void OnEvent(Event e)
        {
            if (e.Sender == this.contact.Id &&
                e is MessageEvent)
            {
                MessageEvent message = (MessageEvent)e;

                txtConversation.Text += contact.Name + ": " + message.Message + "\r\n";
            }
        }

        private void ProfileDialog_Load(object sender, EventArgs e)
        {
            Text = contact.Name;
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtConversation.Text += ClientController.GetInstance().Me.Name
                    + ": " + txtInput.Text + "\r\n";

                SendMessage();
                e.SuppressKeyPress = true;
            }
        }

        void SendMessage()
        {
            ClientController client = ClientController.GetInstance();

            MessageEvent msg = new MessageEvent(client.Me.Id, txtInput.Text);
            client.SendEvent(contact.Id, msg);

            txtInput.Text = "";
        }

        private void ConversationDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClientController.GetInstance().Conversations.Remove(contact.Id);
        }
    }
}
