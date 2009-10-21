using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OpenMessenger.Events
{
    /// <summary>
    /// Event representing a text message sent from a client
    /// </summary>
    [DataContract]
    public class MessageEvent : Event
    {
        string _message;

        /// <summary>
        /// Message text
        /// </summary>
        [DataMember]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender">Sender ID</param>
        /// <param name="message">Message to send</param>
        public MessageEvent(Guid sender, string message)
            :base(sender, 1f)
        {
            _message = message;
        }

        /// <summary>
        /// Textual representation of the message event
        /// </summary>
        /// <returns>Message</returns>
        public override string ToString()
        {
            return "Message: " +  _message;
        }
    }
}
