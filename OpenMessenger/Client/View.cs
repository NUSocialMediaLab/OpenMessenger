using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenMessenger.Client
{
    /// <summary>
    /// Abstract base class for views. Call the static GetInstance method to get a singleton
    /// instance for a given view type.
    /// </summary>
    public abstract class View : TabPage
    {
        static Dictionary<Type, View> _instances = new Dictionary<Type, View>();

        /// <summary>
        /// Gets a singleton instance for the view type specified
        /// </summary>
        /// <typeparam name="ViewType">Type of view to get</typeparam>
        /// <returns>Singleton instance of view type</returns>
        public static ViewType GetInstance<ViewType>() where ViewType : View
        {
            if (!_instances.ContainsKey(typeof(ViewType)))
                _instances.Add(typeof(ViewType), Activator.CreateInstance<ViewType>());

            return (ViewType)_instances[typeof(ViewType)];
        }

        /// <summary>
        /// Textual representation of the View
        /// </summary>
        public abstract override string Text { get; }
    }
}
