using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMessenger
{
    /// <summary>
    /// Generic pair (cons)
    /// </summary>
    /// <typeparam name="A">Type of first constituent</typeparam>
    /// <typeparam name="B">Type of second constituent</typeparam>
    public class Pair<A, B>
    {
        /// <summary>
        /// First constituent of pair
        /// </summary>
        public A First { get; set; }

        /// <summary>
        /// Second constituent of pair
        /// </summary>
        public B Second { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Pair()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="first">First constituent (car)</param>
        /// <param name="second">Second constituent (cdr)</param>
        public Pair(A first, B second)
        {
            this.First = first;
            this.Second = second;
        }
    }
}
