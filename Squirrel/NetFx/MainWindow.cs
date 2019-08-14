using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using NuGet;
using Squirrel;

namespace WPF.Update.Common
{
    public partial class MainWindow
    {
        private string _currentVersion;
        private IUpdateManager _updateManager;
        private Task _autoUpdateTask;

        private void Initialize()
        {
            var url = $"{UpdateUrl}/Squirrel";
            _currentVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            _updateManager = new UpdateManager(url, urlDownloader: new FileDownloader());
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
                    await Task.Delay(300_000);
                    await CheckForUpdate();
                }
            });
        }

        private async Task CheckForUpdate()
        {
            try
            {
                var update = await _updateManager.CheckForUpdate(false, null);
                try
                {
                    var semanticVersion = update.CurrentlyInstalledVersion?.Version ?? new SemanticVersion(0, 0, 0, 0);
                    var version = update.FutureReleaseEntry.Version;
                    if (semanticVersion < version)
                    {
                        Trace.TraceInformation($"[{_currentVersion}]: Updating to [{version}]...");
                        await _updateManager.DownloadReleases(update.ReleasesToApply, null);
                        await _updateManager.ApplyReleases(update, null);
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"[{_currentVersion}]: Fail updating - {ex}");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"[{_currentVersion}]: Can't check for update - {ex}");
            }
        }
    }
}
