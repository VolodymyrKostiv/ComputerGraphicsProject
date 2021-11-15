using System.Windows;
using Studying_app_kg.Views;

namespace Studying_app_kg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Frame.Navigate(new MainPage());
        }
    }
}
