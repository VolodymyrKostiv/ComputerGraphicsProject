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
            NavigationService?.Navigate(new ColorSchemes(this));
        }

        private void AffineTransformations_OnClick(object sender, RoutedEventArgs e)
        {
            //NavigationService?.Navigate(new AffineTransformations());
        }

        private void AffineTransformations_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
