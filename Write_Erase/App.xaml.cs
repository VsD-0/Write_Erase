using ShowMeTheXAML;
using System.Windows;

namespace Write_Erase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            XamlDisplay.Init();
            base.OnStartup(e);
            ViewModelLocator.Init();
        }
    }
}
