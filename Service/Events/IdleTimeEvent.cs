using System;
using System.Runtime.Serialization;

namespace OpenMessenger.Events
{
    [DataContract]
    public class IdleTimeEvent : Event
    {
        int _idleTime;

        /// <summary>
        /// Business level in typing activity
        /// </summary>
        [DataMember]
        public int IdleTime
        {
            get { return _idleTime; }
            set { _idleTime = value; }
        }


        public IdleTimeEvent(Guid sender, int time)
            : base(sender, 2f)
        {
            IdleTime = time;
        }

        /// <summary>
        /// Textual representation of the activity event
        /// </summary>
        /// <returns>Percentage change as text</returns>
        public override string ToString()
        {
            string isIdle = "Not Idle";
            if (IdleTime > 0)
            {
                isIdle = ("Idle for " + IdleTime);
            }
            return isIdle;
        }
    }
}
