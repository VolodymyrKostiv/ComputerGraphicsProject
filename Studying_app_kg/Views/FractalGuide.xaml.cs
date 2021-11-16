using Studying_app_kg.Views;
using System.Windows;
using System.Windows.Controls;

namespace CG.Views
{
    /// <summary>
    /// Interaction logic for FractalGuide.xaml
    /// </summary>
    public partial class FractalGuide : Page
    {
        public FractalGuide()
        {
            InitializeComponent();
        }

        private void Home_OnClick(object sender, SelectionChangedEventArgs e)
        {
            NavigationService?.Navigate(new FractalsPage(this));
        }
        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new FractalsPage(this));

        }
    }
}
