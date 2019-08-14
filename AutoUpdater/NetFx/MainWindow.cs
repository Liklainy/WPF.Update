using System.Diagnostics;
using System.Windows;

namespace WPF.Update.Common
{
    public partial class MainWindow
    {
        private void Initialize()
        {

        }

        private void ButtonCheckForUpdate_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("AutoUpdater ButtonCheckForUpdate_Click");
        }

        private void ButtonRunAutoUpdater_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("AutoUpdater ButtonRunAutoUpdater_Click");
        }
    }
}
