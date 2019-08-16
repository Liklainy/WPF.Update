/*
 * https://github.com/MihaMarkic/AutoMasshTik/blob/master/src/AutoMasshTik.Engine/Services/Implementation/WinAppUpdater.cs
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WPF.Update.Squirrel.Core
{
    public class Updater
    {
        private readonly string _url;
        private readonly string _updateExe;
        public Updater(string url)
        {
            _url = url;
            var currentDirectory = Path.GetDirectoryName(typeof(Updater).Assembly.Location);
            _updateExe = Path.Combine(currentDirectory, "..", "Update.exe");
            Debug.WriteLine($"Update.exe should be at {_updateExe}");
        }
        public Task<CheckForUpdateResult?> GetLatestVersionAsync(CancellationToken ct)
        {
            if (!File.Exists(_updateExe))
                throw new FileNotFoundException($"Couldn't find {_updateExe}");

            return Task.Run(() =>
            {
                string textResult = null;
                var p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = _updateExe,
                        Arguments = $"--checkForUpdate={_url}",
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    }
                };
                p.OutputDataReceived += (s, e) =>
                {
                    Debug.WriteLine($"Checking: {e.Data}");
                    if (e.Data?.StartsWith("{") ?? false)
                    {
                        textResult = e.Data;
                    }
                };
                p.Start();
                p.BeginOutputReadLine();
                p.WaitForExit();
                if (textResult != null)
                {
                    return JsonConvert.DeserializeObject<CheckForUpdateResult>(textResult);
                }
                else
                {
                    return (CheckForUpdateResult?)null;
                }
            });
        }
        public Task UpdateAsync(CancellationToken ct)
        {
            if (File.Exists(_updateExe))
            {
                return Task.Run(() =>
                {
                    var p = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = _updateExe,
                            Arguments = $"--update={_url}",
                            RedirectStandardOutput = true,
                            UseShellExecute = false
                        }
                    };
                    p.OutputDataReceived += (s, e) =>
                    {
                        Debug.WriteLine($"Updating: {e.Data}");
                    };
                    p.Start();
                    p.BeginOutputReadLine();
                    p.WaitForExit();
                });
            }
            return null;
        }
    }
}
