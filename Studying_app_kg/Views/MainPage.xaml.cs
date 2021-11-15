using System.Windows;
using System.Windows.Controls;

namespace Studying_app_kg.Views
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void FractalsChapter_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new FractalsPage(this));
        }

        private void ColorSchemes_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ColorSchemes());
        }

        private void AffineTransformations_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AffineTransformations());
        }

        private void InstructionsChapter_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Instructions());
        }

        private void UserGuideButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UserGuidePage());
        }
    }
}
