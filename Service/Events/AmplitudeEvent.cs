using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OpenMessenger;

namespace OpenMessenger.Events
{
    /// <summary>
    /// Event representing the average amplitude sampled at the client
    /// </summary>
    [DataContract]
    public class AmplitudeEvent : Event
    {
        double _amplitude;

        /// <summary>
        /// Average amplitude of event sender
        /// </summary>
        [DataMember]
        public double Amplitude
        {
            get { return _amplitude; }
            set { _amplitude = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender">ID of sender</param>
        /// <param name="amplitude">Amplitude at sender</param>
        public AmplitudeEvent(Guid sender, double amplitude)
            : base(sender, 2f)
        {
            _amplitude = amplitude;
        }

        /// <summary>
        /// Textual representation of the amplitude event
        /// </summary>
        /// <returns>Amplitude as text</returns>
        public override string ToString()
        {
            double HIGH_THRESHOLD = 50;
            double LOW_THRESHOLD = -50;

            string level = _amplitude.ToString();
            if (_amplitude < HIGH_THRESHOLD && _amplitude > LOW_THRESHOLD)
                level = "no activity";
            else
            {
                Console.WriteLine(_amplitude + " " + HIGH_THRESHOLD + " " + LOW_THRESHOLD);
                Console.WriteLine(_amplitude < HIGH_THRESHOLD);
                Console.WriteLine(_amplitude > LOW_THRESHOLD);
            }

            return level;
        }
    }
}
