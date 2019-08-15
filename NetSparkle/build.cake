#tool "nsis"

var target = Argument("target", "Release");
var configuration = Argument("configuration", "Release");
var version = Argument<string>("buildVersion", "1.0.0");

var projectDir = "NetFx";
var projectPath = $"{projectDir}/WPF.Update.NetSparkle.NetFx.csproj";
var buildDirectory = "../Build/NetSparkle";
    
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

Task("Release")
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
    
    CopyFile("release-notes.md", $"{buildDirectory}/release-notes.md");
    CopyFile("appcast.xml", $"{buildDirectory}/appcast.xml");

    var settings = new XmlPokeSettings
    {
        Namespaces = new Dictionary<string, string> 
        {
            {"sparkle", "http://www.andymatuschak.org/xml-namespaces/sparkle"}
        }
    };
    XmlPoke($"{buildDirectory}/appcast.xml",
        "/rss/channel/item/enclosure/@sparkle:version",
        version, settings);
});

RunTarget(target);
