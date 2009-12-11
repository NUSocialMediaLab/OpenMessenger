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

        public Contact Contact
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contact">Contact to represent for the control</param>
        public OmniContactNode(Contact contact)
        {
            this.Content = contact.Id; // jeesung: i don't think this is usable
            this.Contact = contact;

            InitializeComponent(contact);
        }

        private void InitializeComponent(Contact contact)
        {
            InitializeComponent();

            BitmapImage keyboardBmp = new BitmapImage();
            keyboardBmp.BeginInit();
            keyboardBmp.UriSource = new Uri((System.Environment.CurrentDirectory) + "..\\..\\..\\..\\images\\keyboard.png", UriKind.RelativeOrAbsolute);
            keyboardBmp.DecodePixelWidth = 25;
            keyboardBmp.EndInit();
            keyImg.Source = keyboardBmp;

            BitmapImage micBmp = new BitmapImage();
            micBmp.BeginInit();
            micBmp.UriSource = new Uri((System.Environment.CurrentDirectory) + "..\\..\\..\\..\\images\\mic.png", UriKind.RelativeOrAbsolute);
            micBmp.DecodePixelWidth = 25;
            micBmp.EndInit();
            micImg.Source = micBmp;

            BitmapImage eyeBmp = new BitmapImage();
            eyeBmp.BeginInit();
            eyeBmp.UriSource = new Uri((System.Environment.CurrentDirectory) + "..\\..\\..\\..\\images\\eye.png", UriKind.RelativeOrAbsolute);
            eyeBmp.DecodePixelWidth = 25;
            eyeBmp.EndInit();
            eyeImg.Source = eyeBmp;

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
            HideInfo();


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
        /// Makes the info box visible;
        /// </summary>
        public void ShowInfo()
        {
            infoBox.Visibility = Visibility.Visible;
            keyInfo.Visibility = Visibility.Visible;
            micInfo.Visibility = Visibility.Visible;
            keyImg.Visibility = Visibility.Visible;
            micImg.Visibility = Visibility.Visible;
            eyeImg.Visibility = Visibility.Visible;
            eyeInfo.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Makes the info box visible coressponding to a focus level;
        /// </summary>
        public void ShowInfo(int i)
        {
            HideInfo();
            infoBox.Height = 0;
            switch (i)
            {
                case 5:
                    eyeImg.Visibility = Visibility.Visible;
                    eyeInfo.Visibility = Visibility.Visible;
                    infoBox.Height += 25;
                    goto case 4;
                case 4:
                    micInfo.Visibility = Visibility.Visible;
                    micImg.Visibility = Visibility.Visible;
                    infoBox.Height += 25;
                    goto case 3;
                case 3:
                    infoBox.Visibility = Visibility.Visible;
                    keyInfo.Visibility = Visibility.Visible;
                    keyImg.Visibility = Visibility.Visible;
                    infoBox.Height += 25;
                    break;
                default:
                    HideInfo();
                    break;
            }

        }

        /// <summary>
        /// Updates the keyboard info with the specified content
        /// </summary>
        /// <param name="inf"></param>
        public void UpdateKeyInfo(String inf)
        {
            keyInfo.Content = inf;
        }

        /// <summary>
        /// Updates the mic info with the specified content
        /// </summary>
        /// <param name="inf"></param>
        public void UpdateMicInfo(String inf)
        {
            micInfo.Content = inf;
        }

        /// <summary>
        /// Updates the eye info with the specified content
        /// </summary>
        /// <param name="inf"></param>
        public void UpdateEyeInfo(String inf)
        {
            eyeInfo.Content = inf;
        }

        /// <summary>
        /// Hides the info box
        /// </summary>
        public void HideInfo()
        {
            infoBox.Visibility = Visibility.Collapsed;
            keyInfo.Visibility = Visibility.Collapsed;
            micInfo.Visibility = Visibility.Collapsed;
            keyImg.Visibility = Visibility.Collapsed;
            micImg.Visibility = Visibility.Collapsed;
            eyeImg.Visibility = Visibility.Collapsed;
            eyeInfo.Visibility = Visibility.Collapsed;
        }

    }
}
