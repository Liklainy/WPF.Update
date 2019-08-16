namespace WPF.Update.Squirrel.Core
{
    public struct CheckForUpdateResult
    {
        public string CurrentVersion { get; set; }
        public string FutureVersion { get; set; }
        public ReleaseToApply[] ReleasesToApply { get; set; }
    }
}
