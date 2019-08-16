using System;
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
            var url = $"{UpdateUrl}/Squirrel/NetFx";
            _currentVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            _updateManager = new UpdateManager(url, urlDownloader: new FileDownloader());

            SquirrelAwareApp.HandleEvents(
                onInitialInstall: v => _updateManager.CreateShortcutForThisExe(),
                onAppUpdate: v => _updateManager.CreateShortcutForThisExe(),
                onAppUninstall: v => _updateManager.RemoveShortcutForThisExe()
            );
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
                var update = await _updateManager.CheckForUpdate(false, x => ShowMessage($"Checking updates: {x}%"));
                try
                {
                    var semanticVersion = update.CurrentlyInstalledVersion?.Version ?? new SemanticVersion(0, 0, 0, 0);
                    var version = update.FutureReleaseEntry.Version;
                    if (semanticVersion < version)
                    {
                        ShowMessage($"Updating to [{version}]...");
                        await _updateManager.DownloadReleases(update.ReleasesToApply, x => ShowMessage($"Download update: {x}%"));
                        await _updateManager.ApplyReleases(update, x => ShowMessage($"Applying update: {x}%"));
                        ShowMessage($"Done update to [{version}]");
                    }
                    else
                        ShowMessage("The latest version is installed");
                }
                catch (Exception ex)
                {
                    ShowMessage($"[{_currentVersion}]: Fail updating - {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"[{_currentVersion}]: Can't check for update - {ex.Message}");
            }
        }
    }
}
