using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OpenMessenger.Events
{
    /// <summary>
    /// Event representing activity/presence information from a client
    /// </summary>
    [DataContract]
    public class ActivityEvent : Event
    {
        double _percentageChange;

        /// <summary>
        /// Percentage change in activity
        /// </summary>
        [DataMember]
        public double PercentageChange
        {
            get { return _percentageChange; }
            set { _percentageChange = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender">Sender ID</param>
        /// <param name="percentageChange">Percentage change in activity</param>
        public ActivityEvent(Guid sender, double percentageChange)
            : base(sender, 2f)
        {
            _percentageChange = percentageChange;
        }

        /// <summary>
        /// Textual representation of the activity event
        /// </summary>
        /// <returns>Percentage change as text</returns>
        public override string ToString()
        {
            return "Screen Activity: " + _percentageChange + "% change";
        }
    }
}
