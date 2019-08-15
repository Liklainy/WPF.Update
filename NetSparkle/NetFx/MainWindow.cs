using System.Windows;
using NetSparkle;
using NetSparkle.Enums;

namespace WPF.Update.Common
{
    public partial class MainWindow
    {
        private Sparkle _sparkle;

        private void Initialize()
        {
            // set icon in project properties!
            string manifestModuleName = System.Reflection.Assembly.GetEntryAssembly().ManifestModule.FullyQualifiedName;
            var icon = System.Drawing.Icon.ExtractAssociatedIcon(manifestModuleName);

            _sparkle = new Sparkle($"{UpdateUrl}/NetSparkle/appcast.xml", icon, SecurityMode.Unsafe);
        }

        private void ButtonCheckForUpdate_Click(object sender, RoutedEventArgs e)
        {
            _sparkle.CheckForUpdatesAtUserRequest();
        }

        private void ButtonRunAutoUpdater_Click(object sender, RoutedEventArgs e)
        {
            _sparkle.StartLoop(false, UpdateInterval);

            runAutoUpdater.IsEnabled = false;
        }
    }
}
