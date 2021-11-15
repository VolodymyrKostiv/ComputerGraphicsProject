using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Studying_app_kg.Views
{
    /// <summary>
    /// Interaction logic for UserGuidePage.xaml
    /// </summary>
    public partial class UserGuidePage : Page
    {
        public UserGuidePage(int senderId = 1)
        {
            InitializeComponent();
            switch (senderId)
            {
                case 1: Button_OnClick(Button1, new RoutedEventArgs()); break;
                case 2: Button_OnClick(Button2, new RoutedEventArgs()); break;
                case 3: Button_OnClick(Button3, new RoutedEventArgs()); break;
                case 4: Button_OnClick(Button4, new RoutedEventArgs()); break;
            }
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var buttonName = ((Button)sender).Name;
            SetButtonStyle(sender);
            if (buttonName == "Button1")
            {
                OutputData.Text = "";
            }
            else if (buttonName == "Button2")
            {
                
                OutputData.Text = "\tFractal - a curve or geometrical figure, each part of which has " +
                    "the same statistical character as the whole. They are useful in modelling structures " +
                    "(such as snowflakes) in which similar patterns recur at progressively smaller scales," +
                    " and in describing partly random or chaotic phenomena such as crystal growth and galaxy formation.";
            }
            else if (buttonName == "Button3")
            {
                OutputData.Text = " CMYK (short for Cyan, Magenta, Yellow, BlacK color) is a subtractive color model used in printing, especially in multicolor (full color) printing. It is used in printing presses and color printers.\n\n" +
                    "HSL (for hue, saturation, lightness)  The HSL representation models the way different  paints mix together to create colour in the real world, with the lightness dimension resembling the varying amounts of black or " +
                    "white paint in the mixture. Fully saturated colors are placed around a circle at a lightness value of ½, with a lightness value of 0 or 1 corresponding to fully black or white, respectively.";
            }
            else if (buttonName == "Button4")
            {
                OutputData.Text = "";
            }
            else if (buttonName == "Button5")
            {
                OutputData.Text = "\t\n";
            }
        }
        public void HandleLinkClick(object sender, RoutedEventArgs e)
        {
            Hyperlink hl = (Hyperlink)sender;
            string navigateUri = hl.NavigateUri.ToString();
            System.Diagnostics.Process.Start(new ProcessStartInfo(navigateUri) { UseShellExecute = true });
            e.Handled = true;
        }
        private void SetButtonStyle(object sender)
        {
            var buttonName = ((Button)sender).Name;
            var bc = new BrushConverter();
            var brush = "#DBCA9A";
            ((Button)sender).Background = (Brush)bc.ConvertFrom("#A89A85");
            if (buttonName == "Button1")
            {
                Button2.Background = (Brush)bc.ConvertFrom(brush);
                Button3.Background = (Brush)bc.ConvertFrom(brush);
                Button4.Background = (Brush)bc.ConvertFrom(brush);
                Button5.Background = (Brush)bc.ConvertFrom(brush);
            }
            else if(buttonName == "Button2")
            {
                Button1.Background = (Brush)bc.ConvertFrom(brush);
                Button3.Background = (Brush)bc.ConvertFrom(brush);
                Button4.Background = (Brush)bc.ConvertFrom(brush);
                Button5.Background = (Brush)bc.ConvertFrom(brush);
            } 
            else if (buttonName == "Button3")
            {
                Button1.Background = (Brush)bc.ConvertFrom(brush);
                Button2.Background = (Brush)bc.ConvertFrom(brush);
                Button4.Background = (Brush)bc.ConvertFrom(brush);
                Button5.Background = (Brush)bc.ConvertFrom(brush);
            }
            else if (buttonName == "Button4")
            {
                Button1.Background = (Brush)bc.ConvertFrom(brush);
                Button2.Background = (Brush)bc.ConvertFrom(brush);
                Button3.Background = (Brush)bc.ConvertFrom(brush);
                Button5.Background = (Brush)bc.ConvertFrom(brush);
            }
            else
            {
                Button1.Background = (Brush)bc.ConvertFrom(brush);
                Button2.Background = (Brush)bc.ConvertFrom(brush);
                Button3.Background = (Brush)bc.ConvertFrom(brush);
                Button4.Background = (Brush)bc.ConvertFrom(brush);
            }
        }
        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MainPage());
        }

        private void UserGuideButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Instructions());
        }
    }
}
