#tool nuget:?package=Squirrel.Windows
#addin nuget:?package=Cake.Squirrel

var target = Argument("target", "Publish");
var configuration = Argument("configuration", "Release");
var framework = Argument("framework", "NetFx");
var version = Argument<string>("buildVersion", "1.0.0");


var projectDir = $"{framework}";
var assemblyName = $"WPF.Update.Squirrel.{framework}";
var projectPath = $"{projectDir}/{assemblyName}.csproj";
    
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
    Func<ProcessArgumentBuilder, ProcessArgumentBuilder> argsCustomization =
        args => args.Append("/p:Version=" + version);
    switch(framework)
    {
        case "NetFx":
            MSBuild(projectPath,
                new MSBuildSettings
                {
                    Configuration = configuration,
                    ArgumentCustomization = argsCustomization
                });
            break;
        case "Core":
            DotNetCoreBuild(projectPath,
                new DotNetCoreBuildSettings
                {
                    Configuration = configuration,
                    ArgumentCustomization = argsCustomization
                });
            break;
    }
});

Task("Package")
    .IsDependentOn("Build")
    .Does(() =>
{
    var nuGetPackSettings = new NuGetPackSettings
    {
        Version                 = version,
        BasePath                = $"{projectDir}/bin/{configuration}",
        OutputDirectory         = $"{projectDir}/bin/{configuration}"
    };

    NuGetPack($"{projectDir}/{assemblyName}.nuspec", nuGetPackSettings);
});

Task("Publish")
    .IsDependentOn("Package")
    .Does(() =>
{
    var nupgk = File($"{projectDir}/bin/{configuration}/{assemblyName}.{version}.nupkg");
    Information($"Publish: {nupgk}");
    var settings = new SquirrelSettings
    {
        NoMsi = true,
        ReleaseDirectory = $"../Build/Squirrel/{framework}"
    };
    Squirrel(nupgk, settings);
});


RunTarget(target);
