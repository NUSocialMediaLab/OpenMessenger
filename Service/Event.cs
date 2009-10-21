using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using OpenMessenger.Events;

namespace OpenMessenger
{
    /// <summary>
    /// Abstract class for events. All events dervice from this class
    /// </summary>
    [DataContract]
    [KnownType(typeof(MessageEvent))]
    [KnownType(typeof(ActivityEvent))]
    [KnownType(typeof(EyeActivityEvent))]
    [KnownType(typeof(AmplitudeEvent))]
    public abstract class Event
    {
        static Guid _serverGuid = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        Guid _sender;
        DateTime _timestamp;
        double _level;
        
        /// <summary>
        /// Event sender
        /// </summary>
        [DataMember]
        public Guid Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        /// <summary>
        /// Timestamp when event was created
        /// </summary>
        [DataMember]
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value;  }
        }

        /// <summary>
        /// Focus level for the event. Only contacts which are focusing at this level or above on the
        /// sender will receive the event.
        /// </summary>
        [DataMember]
        public double Level
        {
            get { return _level; }
            set { _level = value; }
        }

        /// <summary>
        /// True if this event was created by the service
        /// </summary>
        public bool IsServerEvent
        {
            get { return _sender.Equals(_serverGuid); }
        }

        /// <summary>
        /// Textual representation of the event
        /// </summary>
        /// <returns>A textual representation specified by the individual event type</returns>
        public abstract override string ToString();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender">ID of the sender</param>
        /// <param name="level">Level of focus required to receive the event</param>
        public Event(Guid sender, double level)
        {
            _sender = sender;
            _timestamp = DateTime.Now;
            _level = level;
        }

    }
}
