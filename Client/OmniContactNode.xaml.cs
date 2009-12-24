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
            this.Contact = contact;
            InitializeComponent();
            InitializeInfoBox();
            InitializeAvatar();
        }

        private void InitializeAvatar()
        {
            label.Content = Contact.Name;
            SetBitmapImage(avatarImg, Contact.Avatar, 70);
        }

        private void InitializeInfoBox()
        {
            SetBitmapImage(keyImg, (System.Environment.CurrentDirectory)+"..\\..\\..\\..\\images\\keyboard.png", 25);
            SetBitmapImage(micImg, (System.Environment.CurrentDirectory)+"..\\..\\..\\..\\images\\mic.png", 25);
            SetBitmapImage(eyeImg, (System.Environment.CurrentDirectory)+"..\\..\\..\\..\\images\\eye.png", 25);

            DropShadowEffect fxInfo = new DropShadowEffect();
            fxInfo.BlurRadius = 15;
            fxInfo.ShadowDepth = 0;
            fxInfo.Color = Contact.Color;
            infoBox.Effect = fxInfo;
            HideInfo();
        }

        /// <summary>
        /// Sets the image object with given bitmap image source
        /// </summary>
        /// <param name="img"></param>
        /// <param name="imgSrc"></param>
        /// <param name="size"></param>
        private void SetBitmapImage(Image img, String imgSrc, int size)
        {
            Console.WriteLine(imgSrc);
            BitmapImage bitImg = new BitmapImage();
            bitImg.BeginInit();
            bitImg.UriSource = new Uri(imgSrc, UriKind.RelativeOrAbsolute);
            bitImg.DecodePixelWidth = size;
            bitImg.EndInit();
            img.Source = bitImg;

        }

        /// <summary>
        /// Enables/disables halo effect based on boolean argument
        /// </summary>
        /// <param name="b"></param>
        public void HaloEffect(Boolean b)
        {
            if (b)
            {
                DropShadowEffect fx = new DropShadowEffect();
                fx.BlurRadius = 35;
                fx.ShadowDepth = 0;
                fx.Color = Contact.Color;
                halo.Effect = fx;
            }
            else
            {
                halo.Effect = null;
            }
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
