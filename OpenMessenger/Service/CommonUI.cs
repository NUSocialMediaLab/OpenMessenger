using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace OpenMessenger
{
    /// <summary>
    /// Utility functions for user-interfaces at both client and server
    /// </summary>
    public class CommonUI
    {
        /// <summary>
        /// Creates an avatar for a node in a graph. (incomplete)
        /// </summary>
        /// <param name="node">Node to represent</param>
        /// <returns>An avatar as a ContentControl</returns>
        public static ContentControl CreateAvatarUINode(Graph.Graph.Node node)
        {
            ContentControl content = new ContentControl();

            Contact contact = (Contact)node.Content;


            return content;
        }
    }
}
