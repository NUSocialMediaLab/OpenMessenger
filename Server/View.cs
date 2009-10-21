using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenMessenger.Server
{
    /// <summary>
    /// Abstract class for views for the server
    /// </summary>
    public abstract class View : TabPage
    {
        static Dictionary<Type, View> _instances = new Dictionary<Type, View>();

        /// <summary>
        /// Gets a singleton instance for a view by type
        /// </summary>
        /// <typeparam name="ViewType">Type of view to get</typeparam>
        /// <returns>A singleton instance</returns>
        public static ViewType GetInstance<ViewType>() where ViewType : View
        {
            if (!_instances.ContainsKey(typeof(ViewType)))
                _instances.Add(typeof(ViewType), Activator.CreateInstance<ViewType>());

            return (ViewType)_instances[typeof(ViewType)];
        }

        /// <summary>
        /// Textual representation for the server view
        /// </summary>
        public abstract override string Text { get; }

    }
}
