using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace OpenMessenger.Client
{
    /// <summary>
    /// Abstract base class for sensors. Call the static GetInstance method to get a singleton
    /// instance for a given sensor type.
    /// </summary>
    public abstract class Sensor
    {
        static Dictionary<Type, Sensor> _instances = new Dictionary<Type, Sensor>();

        /// <summary>
        /// Gets a singleton instance for the sensor of a specified type
        /// </summary>
        /// <typeparam name="SensorType">The type of sensor to get</typeparam>
        /// <returns>Sensor singleton instance</returns>
        public static SensorType GetInstance<SensorType>() where SensorType : Sensor
        {

            if (!_instances.ContainsKey(typeof(SensorType)))
                _instances.Add(typeof(SensorType), Activator.CreateInstance<SensorType>());

            return (SensorType)_instances[typeof(SensorType)];
        }

        /// <summary>
        /// Textual representation for the sensor
        /// </summary>
        public abstract string Text { get; }

        /// <summary>
        /// Starts sensing
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stops sensing
        /// </summary>
        public abstract void Stop();
    }
}
