using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace OpenMessenger
{
    /// <summary>
    /// Represents a contact in the system
    /// </summary>
    [DataContract]
    public class Contact : IEquatable<Contact>
    {
        /// <summary>
        /// Maximum level of focus for a contact
        /// </summary>
        public static double MaxFocusLevel { get { return 5.0; } }

        IClient _client;

        /// <summary>
        /// ID of the contact
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Adddress (protocol, host, port, path) of the client
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Name of the client
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// System path to avatar
        /// </summary>
        [DataMember]
        public string Avatar { get; set; }

        /// <summary>
        /// Color representation for the client
        /// </summary>
        [DataMember]
        public Color Color { get; set; }

        /// <summary>
        /// Client proxy used to contact this client. Creates a proxy object dynamically from the
        /// contact details.
        /// </summary>
        public IClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = ChannelFactory<IClient>.CreateChannel(
                        new WSHttpBinding(),
                        new EndpointAddress(Address));
                }

                return _client;
            }
        }

        private Contact()
        {
            Id = Guid.NewGuid(); // make each new contract globally unique
            Name = Utilities.GetReallyRandomName();
            Address = Utilities.GetLocalEndpoint(true).Uri.ToString() + Name;
            Color = Utilities.GetRandomColor();
            Avatar = Utilities.GetRandomAvatar();
        }

        /// <summary>
        /// Create a new contact statically.
        /// </summary>
        /// <returns>A new client</returns>
        public static Contact Create()
        {
            return new Contact();
        }

        /// <summary>
        /// Textual representation for the client
        /// </summary>
        /// <returns>Name of client</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Compares to client instances to see if they are equal
        /// </summary>
        /// <param name="other">Client instance to compare to</param>
        /// <returns>True if client IDs match</returns>
        public bool Equals(Contact other)
        {
            return Id.Equals(other.Id);
        }
    }
}
