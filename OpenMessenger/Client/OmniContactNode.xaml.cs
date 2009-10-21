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
        Label[] infoBoxes;
        
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
            info1Box.Opacity = 1.0f;
            info1Box.Fill = new SolidColorBrush(contact.Color);
            info1Box.Visibility = Visibility.Collapsed;
            info1.Visibility = Visibility.Collapsed;
            info1.Content = "";
            label.Content = contact.Name;
            infoBoxes = new Label[3];
            infoBoxes[0] = info1; infoBoxes[1] = info2; //infoBoxes[2] = info3;   
        }

        /// <summary>
        /// The following methods are all for adding and manipulating the infoboxes .
        /// </summary>
        /// <param name="inf"></param>
        public void ShowInfo(String inf)
        {
            // In the future, this could either put the text into the first empty box,
            // or even create a new box.
            info1Box.Visibility = Visibility.Visible;
            info1.Visibility = Visibility.Visible;
            info1.Content = inf;
        }
        public void ShowInfo(String info, int box)
        {
            infoBoxes[box].Visibility = Visibility.Visible;
            infoBoxes[box].Visibility = Visibility.Visible;
            infoBoxes[box].Content = info;
        }
        public void UpdateInfo(String inf, int box)
        {
            infoBoxes[box].Content = inf;
        }
        public void HideInfo(int box)
        {
            infoBoxes[box].Visibility = Visibility.Collapsed;
            infoBoxes[box].Visibility = Visibility.Collapsed;
        }
        public void hideAllInfo()
        {
            for (int i = 0; i < infoBoxes.Length; i++)
                HideInfo(i);
        }
    }
}
