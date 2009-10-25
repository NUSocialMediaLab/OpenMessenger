using System;
using System.Runtime.Serialization;

namespace OpenMessenger.Events
{
    [DataContract]
    public class KeyboardEvent : Event
    {
        int _activityLevel;

        /// <summary>
        /// Business level in typing activity
        /// </summary>
        [DataMember]
        public int activityLevel
        {
            get { return _activityLevel; }
            set { _activityLevel = value; }
        }


        public KeyboardEvent(Guid sender, int activityLevel)
            : base(sender, 2f)
        {
            _activityLevel = activityLevel;
        }

        /// <summary>
        /// Textual representation of the activity event
        /// </summary>
        /// <returns>Percentage change as text</returns>
        public override string ToString()
        {
            string level = "no activity";
            if (activityLevel > 0){
                    level = "typing...";
            }
            return level;
        }
    }
}
