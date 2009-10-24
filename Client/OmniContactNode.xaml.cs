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
using System.Windows.Media.Effects;


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

            ClientController client = ClientController.GetInstance();
            Guid i = client.Me.Id;

            DropShadowEffect fx = new DropShadowEffect();
            fx.BlurRadius = 35;
            fx.ShadowDepth = 0;
            fx.Color = contact.Color;

            DropShadowEffect fxInfo = new DropShadowEffect();
            fxInfo.BlurRadius = 15;
            fxInfo.ShadowDepth = 0;
            fxInfo.Color = contact.Color;

            halo.Effect = fx;

            infoBox.Effect = fxInfo;
            infoBox.Visibility = Visibility.Collapsed;
            info.Visibility = Visibility.Collapsed;
            info.Content = "";

            if (contact.Id == i)
            {
                this.Height = 150;
                this.Width = 200;
                label.VerticalAlignment = VerticalAlignment.Stretch;
                label.Margin = new Thickness(0, 20, 0, 0);
                label.Content = "Me";
                label.FontSize = 35;
            }
            else
            {
                label.Content = contact.Name;
            }
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
