#tool nuget:?package=Squirrel.Windows
#addin nuget:?package=Cake.Squirrel

var target = Argument("target", "Publish");
var configuration = Argument("configuration", "Release");
var version = Argument<string>("buildVersion", null);

var projectDir = "NetFx";
var projectPath = $"{projectDir}/WPF.Update.Squirrel.NetFx.csproj";
var assemblyName = "WPF.Update.Squirrel.NetFx";
    
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
    var settings = new MSBuildSettings {
        Configuration = configuration,
        ArgumentCustomization = args => args.Append("/p:Version=" + version)
    };
    MSBuild(projectPath, settings);
});

Task("Package")
    .IsDependentOn("Build")
    .Does(() =>
{
        var nuGetPackSettings = new NuGetPackSettings {
        Version                 = version,
        BasePath                = $"{projectDir}/bin/{configuration}/net472",
        OutputDirectory         = $"{projectDir}/bin/{configuration}"
    };

    NuGetPack($"{projectDir}/WPF.Update.Squirrel.NetFx.nuspec", nuGetPackSettings);

/*
    var buildSettings =  new DotNetCorePackSettings
    {
        Configuration = configuration,
        Verbosity = DotNetCoreVerbosity.Minimal,
        ArgumentCustomization = args => args.Append("/p:Version=" + version)
    };

    DotNetCorePack(projectPath, buildSettings);
*/
});

Task("Publish")
    .IsDependentOn("Package")
    .Does(() =>
{
    var nupgk = File($"{projectDir}/bin/{configuration}/{assemblyName}.{version}.nupkg");
    Information($"Publish: {nupgk}");
    var settings = new SquirrelSettings
    {
        ReleaseDirectory = $"../Build/Squirrel"
    };
    Squirrel(nupgk, settings);
});


RunTarget(target);
