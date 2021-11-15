using System.Windows;

namespace Studying_app_kg
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
        base.OnStartup(e);
        this.ShutdownMode = System.Windows.ShutdownMode.OnLastWindowClose;
        }
    }
    
}
