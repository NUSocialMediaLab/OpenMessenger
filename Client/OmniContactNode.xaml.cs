using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenMessenger.Client
{
    /// <summary>
    /// XAML control representing a node in the Omni window
    /// </summary>
    public partial class OmniContactNode : UserControl
    {
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contact">Contact to represent for the control</param>
        public OmniContactNode(Contact contact)
        {
            this.Content = contact.Id;
            InitializeComponent();
            halo.Fill = new SolidColorBrush(contact.Color);
            halo.Opacity = 1.0f;
            label.Content = contact.Name;

            infoBox.Opacity = 1.0f;
            infoBox.Fill = new SolidColorBrush(contact.Color);
            infoBox.Visibility = Visibility.Collapsed;
            info.Visibility = Visibility.Collapsed;
            info.Content = "";
        }

        /// <summary>
        /// Makes the info box visible with the specified content.
        /// </summary>
        /// <param name="inf"></param>
        public void ShowInfo(String inf)
        {
            infoBox.Visibility = Visibility.Visible;
            info.Visibility = Visibility.Visible;
            info.Content = inf;
        }

        /// <summary>
        /// Updates the info box with the specified content
        /// </summary>
        /// <param name="inf"></param>
        public void UpdateInfo(String inf)
        {
            info.Content = inf;
        }

        /// <summary>
        /// Hides the info box
        /// </summary>
        public void HideInfo()
        {
            info.Visibility = Visibility.Collapsed;
            infoBox.Visibility = Visibility.Collapsed;
        }
    }
}
