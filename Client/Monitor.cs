using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace OpenMessenger.Client
{
    /// <summary>
    /// Abstract base class for monitors. Call the static GetInstance method to get a singleton
    /// instance for a given monitor type.
    /// </summary>
    public abstract class Monitor
    {
        static Dictionary<Type, Monitor> _instances = new Dictionary<Type, Monitor>();

        static Dictionary<string, Dictionary<string, string>> ConfigurationDictionary = LoadConstants();

        const string ConfigFileLocation = "C:\\OpenMessenger\\config.txt";
        
        const string RegexString = "^([^#]+)\\.(.+)\\s*=\\s*(.+)$";

        /// <summary>
        /// Gets a singleton instance for the monitor of a specified type
        /// </summary>
        /// <typeparam name="MonitorType">The type of monitor to get</typeparam>
        /// <returns>Monitor singleton instance</returns>
        public static MonitorType GetInstance<MonitorType>() where MonitorType : Monitor
        {
            if(ConfigurationDictionary == null)
            {
                ConfigurationDictionary = LoadConstants();
            }
            if (!_instances.ContainsKey(typeof(MonitorType)))
                _instances.Add(typeof(MonitorType), Activator.CreateInstance<MonitorType>());

            return (MonitorType)_instances[typeof(MonitorType)];
        }

        /// <summary>
        /// Textual representation for the monitor
        /// </summary>
        public abstract string Text { get; }

        /// <summary>
        /// Starts monitoring
        /// </summary>
        public abstract void Start();
        
        /// <summary>
        /// Stops monitoring
        /// </summary>
        public abstract void Stop();

        private static Dictionary<String, Dictionary<string, string>> LoadConstants()
        {
            Dictionary<String, Dictionary<string, string>> dict = new Dictionary<String, Dictionary<string, string>>();
            TextReader tr = new StreamReader(ConfigFileLocation);
            string line = "";
            while ((line = tr.ReadLine()) != null)
            {
                //ConsoleWriteLine(line);
                Match ValidConstant = Regex.Match(line, RegexString);
                if (ValidConstant.Success)
                {
                    if (dict.ContainsKey(ValidConstant.Groups[1].Value))
                    {
                        //ConsoleWriteLine(ValidConstant.Groups[1].Value + " " + ValidConstant.Groups[2].Value + " " + ValidConstant.Groups[3].Value);
                        dict[ValidConstant.Groups[1].Value].Add(ValidConstant.Groups[2].Value, ValidConstant.Groups[3].Value);
                    }
                    else
                    {
                        dict.Add(ValidConstant.Groups[1].Value, new Dictionary<string, string>());
                        dict[ValidConstant.Groups[1].Value].Add(ValidConstant.Groups[2].Value, ValidConstant.Groups[3].Value);
                    }
                }
            }
            return dict;
        }

        protected static Dictionary<string, string> GetConstants(string name)
        {
            return ConfigurationDictionary[name];
        }
    }
}
