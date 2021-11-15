using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Drawing;
using Microsoft.Win32;
using Studying_app_kg.Model;

namespace Studying_app_kg.Views
{
    public partial class FractalsPage : Page
    {
        private KochSnowflake fractal;
        private Page basePage;
        public FractalsPage(Page page)
        {
            InitializeComponent();
            fractal = new KochSnowflake(fractalCanvas);
            basePage = page;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                fractalCanvas.Children.Clear();
                fractal.RunGeometricKochSnowflake(Convert.ToInt32(4));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Invalid data\n "+ex.Message, "Study part", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void UserGuideButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UserGuidePage(2));
        }

        private void InstructionsButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Instructions(2));
        }

        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }

        private void ComboBoxColor_Selected(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
