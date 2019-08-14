#tool "nsis"

var target = Argument("target", "Publish");
var configuration = Argument("configuration", "Release");
var version = Argument<string>("buildVersion", null);

var projectDir = "NetFx";
var projectPath = $"{projectDir}/WPF.Update.AutoUpdater.NetFx.csproj";
    
Task("Clean")
    .Does(() =>
{
    Information(MakeAbsolute(Directory("./")));
    CleanDirectories("./**/bin/" + configuration);
    CleanDirectories("./**/obj/" + configuration);
});

Task("Restore")
    .Does(() =>
{
    DotNetCoreRestore(projectDir);
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var settings = new MSBuildSettings
    {
        Configuration = configuration,
        ArgumentCustomization = args => args.Append("/p:Version=" + version)
    };
    MSBuild(projectPath, settings);
});

Task("Package")
    .IsDependentOn("Build")
    .Does(() =>
{
    var nuGetPackSettings = new NuGetPackSettings
    {
        Version                 = version,
        BasePath                = $"{projectDir}/bin/{configuration}/net472",
        OutputDirectory         = $"{projectDir}/bin/{configuration}"
    };

    NuGetPack($"{projectDir}/WPF.Update.AutoUpdater.NetFx.nuspec", nuGetPackSettings);
});

Task("Installer")
    .IsDependentOn("Build")
    .Does(() =>
{
    MakeNSIS("build_installer.nsi", new MakeNSISSettings
    {
        Defines = new Dictionary<string, string>
        {
            { "PRODUCT_VERSION", version }
        }
    });
});

Task("Publish")
    .IsDependentOn("Build")
    .Does(() =>
{
    Zip($"{projectDir}/bin/{configuration}/net472", "../Build/AutoUpdater/release.zip");
    
    CopyFile("release.xml", "../Build/AutoUpdater/release.xml");
    var settings = new XmlPokeSettings
    {
        Namespaces = new Dictionary<string, string> 
        {
            {"android", "http://schemas.android.com/apk/res/android"}
        }
    };

    XmlPoke("../Build/AutoUpdater/release.xml",
        "/item/version", version, settings);
});


RunTarget(target);
