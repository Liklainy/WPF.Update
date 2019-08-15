#tool "nsis"

var target = Argument("target", "Publish");
var configuration = Argument("configuration", "Release");
var version = Argument<string>("buildVersion", "1.0.0");

var projectDir = "NetFx";
var projectPath = $"{projectDir}/WPF.Update.AutoUpdater.NetFx.csproj";
var buildDirectory = "../Build/AutoUpdater";
    
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

Task("Installer")
    .IsDependentOn("Build")
    .Does(() =>
{
    EnsureDirectoryExists(buildDirectory);
    MakeNSIS("build_installer.nsi", new MakeNSISSettings
    {
        Defines = new Dictionary<string, string>
        {
            { "PRODUCT_VERSION", version },
            { "PRODUCT_BUILD_DIR", buildDirectory },
        }
    });
});

Task("Publish")
    .IsDependentOn("Build")
    .Does(() =>
{
    EnsureDirectoryExists(buildDirectory);
    Zip($"{projectDir}/bin/{configuration}/net472", $"{buildDirectory}/release.zip");
    
    CopyFile("index.html", $"{buildDirectory}/index.html");
    CopyFile("release.xml", $"{buildDirectory}/release.xml");

    XmlPoke($"{buildDirectory}/release.xml", "/item/version", version);
});


RunTarget(target);
