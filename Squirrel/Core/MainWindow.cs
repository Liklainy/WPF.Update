using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WPF.Update.Squirrel.Core;

namespace WPF.Update.Common
{
    public partial class MainWindow
    {
        private Updater _updater;
        private Task _autoUpdateTask;

        private void Initialize()
        {
            _updater = new Updater($"{UpdateUrl}/Squirrel/Core");
        }

        private void ButtonCheckForUpdate_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () => await CheckForUpdate());
        }

        private void ButtonRunAutoUpdater_Click(object sender, RoutedEventArgs e)
        {
            if (_autoUpdateTask != null)
                return;

            _autoUpdateTask = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(UpdateInterval);
                    await CheckForUpdate();
                }
            });

            runAutoUpdater.IsEnabled = false;
        }

        private async Task CheckForUpdate()
        {
            try
            {
                ShowMessage("Checking new versions...");
                var result = await _updater.GetLatestVersionAsync(CancellationToken.None);
                if (result.HasValue && result.Value.CurrentVersion != result.Value.FutureVersion)
                {
                    ShowMessage($"Updater will fetch version {result.Value.FutureVersion}");
                    await _updater.UpdateAsync(CancellationToken.None);
                    ShowMessage("Update finished, restart required");
                }
                else
                {
                    ShowMessage("Got no response from updater or version is current");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Failed to update - {ex.Message}");
            }
        }
    }
}
