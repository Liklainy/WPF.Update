using System.Windows;
using System.Windows.Threading;

namespace WPF.Update.Common
{
    public partial class MainWindow
    {
        private void Initialize()
        {

        }

        private void ButtonCheckForUpdate_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void ButtonRunAutoUpdater_Click(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer { Interval = UpdateInterval };
            timer.Tick += delegate
            {
                Update();
            };
            timer.Start();

            runAutoUpdater.IsEnabled = false;
        }

        private void Update()
        {
            AutoUpdaterDotNET.AutoUpdater.Start($"{UpdateUrl}/AutoUpdater/release.xml");
        }
    }
}
