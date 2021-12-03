using Studying_app_kg.Views;
using System.Windows;
using System.Windows.Controls;

namespace CG.Views
{
    /// <summary>
    /// Interaction logic for AffineTransformationsGuide.xaml
    /// </summary>
    public partial class AffineTransformationsGuide : Page
    {
        public AffineTransformationsGuide()
        {
            InitializeComponent();
        }

        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AffineTransformations(this));
        }
    }
}
