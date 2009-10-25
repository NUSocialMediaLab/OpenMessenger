using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Reflection;
using System.ServiceModel;
using System.Net;
using System.Windows.Media;
using System.Management;

namespace OpenMessenger
{
    /// <summary>
    /// Useful utility functions shared by both client and service
    /// </summary>
    public static class Utilities
    {
        static Random random = new Random(DateTime.Now.Millisecond);
        static string[] randomNames = { "Linus", "Tobias", "Xzavier", "Gregorio", "Lieber", "Surya",
                                        "Chus", "Mirek", "Yonatan", "Baz", "Julia", "Margarid",
                                        "Llinos", "Tameka", "Adhiambo", "Walentyna", "Sima",
                                        "Kinga", "Gudrun", "Lucy", "Frank", "James", "Alison" };

        /// <summary>
        /// Finds classes deriving from a given class
        /// </summary>
        /// <typeparam name="T">Abstract type</typeparam>
        /// <param name="assembly">Assembly to search</param>
        /// <returns>Collection of deriving types</returns>
        public static IEnumerable<Type> FindSubTypes<T>(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.BaseType == typeof(T))
                    yield return type;
            }

        }

        /// <summary>
        /// Creates a textual representation of an enumerable collection
        /// </summary>
        /// <typeparam name="Type">Type of collection</typeparam>
        /// <param name="collection">Collection to represent</param>
        /// <returns>List of elements as text</returns>
        public static string EnumerableToString<Type>(IEnumerable<Type> collection)
        {
            StringBuilder builder = new StringBuilder("[");

            foreach (Type item in collection)
                builder.Append(item.ToString() + ", ");

            builder.Remove(builder.Length - 3, 2);
            builder.Append("]");
            return builder.ToString();
        }

        /// <summary>
        /// Get the address of the local client
        /// Note, as currently implemented a DNS server needs to be present for determining the IP address
        /// </summary>
        /// <param name="randomizePort">Optionally randomize the port, otherwise set to 8000</param>
        /// <returns>The local endpoint address</returns>
        public static EndpointAddress GetLocalEndpoint(bool randomizePort)
        {
            string hostName = Dns.GetHostName();
            IPHostEntry local = Dns.GetHostByName(hostName);

            int port = 8000;

            if (randomizePort)
            {
                Random rand = new Random(DateTime.Now.Millisecond);
                port = rand.Next(8001, 16000);
            }

            return new EndpointAddress("http://" +
                local.AddressList[0] +
                ":" + port.ToString() + "/");
        }

        /// <summary>
        /// Retrieve a random name from the list of names
        /// </summary>
        /// <returns>A name</returns>
        public static string GetReallyRandomName()
        {
            return RandomElement<string>(randomNames);
        }

        /// <summary>
        /// Get a random avatar
        /// </summary>
        /// <returns>Filename from the avatars directory</returns>
        public static string GetRandomAvatar()
        {
            string[] avatars = Directory.GetFiles("Avatars", "*.png");
            string avatar = avatars[random.Next(avatars.Length)];

            return avatar.Substring(8, avatar.Length - 8);
        }

        /// <summary>
        /// Pick a random element from an array
        /// </summary>
        /// <typeparam name="T">Type of array</typeparam>
        /// <param name="array">Array collection</param>
        /// <returns>A random element</returns>
        public static T RandomElement<T>(T[] array)
        {
            return array[random.Next(0, array.Length - 1)];
        }

        /// <summary>
        /// Pick a random color
        /// </summary>
        /// <returns>A random color</returns>
        public static Color GetRandomColor()
        {
            int i = random.Next(10);
            Color c = new Color();
            switch (i)
            {
                case 1:
                    c = Colors.LemonChiffon;
                    break;
                case 2:
                    c = Colors.Green;
                    break;
                case 3:
                    c = Colors.Red;
                    break;
                case 4:
                    c = Colors.PowderBlue;
                    break;
                case 5:
                    c = Colors.SteelBlue;
                    break;
                case 6:
                    c = Colors.Cyan;
                    break;
                case 7:
                    c = Colors.LightPink;
                    break;
                case 8:
                    c = Colors.YellowGreen;
                    break;
                default:
                    c = Colors.MintCream;
                    break;
            }
            return c;
        }
    }
}
