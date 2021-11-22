using CG.Views;
using System.Windows;
using System.Windows.Controls;

namespace Studying_app_kg.Views
{
    public partial class ColorSchemes : Page
    {
        private Page basePage;
        public ColorSchemes(Page page)
        {
            InitializeComponent();
            basePage = page;
        }
        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MainPage());
        }

        private void ColorsSchemeGuide_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ColorSchemesGuide());
        }
    }
}
