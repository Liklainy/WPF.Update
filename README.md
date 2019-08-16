# WPF.Update

[![Build status](https://ci.appveyor.com/api/projects/status/dvotdh27wfivm836/branch/master?svg=true)](https://ci.appveyor.com/project/Liklainy/wpf-update/branch/master)

WPF.Update is a project to show available open source ways to deploy a WPF application

## Projects

* [Squirrel.Windows](https://github.com/Squirrel/Squirrel.Windows)
* [AutoUpdater.NET](https://github.com/ravibpatel/AutoUpdater.NET)
* [NetSparkle](https://github.com/Deadpikle/NetSparkle)

### Squirrel.Windows

#### NetFx
Using C# API
```powershell
.\build.ps1 -script .\Squirrel\build.cake --framework='NetFx' --buildVersion='1.0.0'
```
#### Core
Running Update.exe directly (used @MihaMarkic [solution](https://github.com/MihaMarkic/AutoMasshTik/blob/eccc8ac8b95ab32050916b6d08e1ecad280f1c4d/src/AutoMasshTik.Engine/Services/Implementation/WinAppUpdater.cs))
```powershell
.\build.ps1 -script .\Squirrel\build.cake --framework='Core' --buildVersion='1.0.0'
```
### AutoUpdater.NET
AutoUpdater can update through installer or zip file
> If you provide zip file URL instead of installer then AutoUpdater.NET will extract the contents of zip file to application directory

#### Deploy installer
Files are packaged to Setup.exe by NSIS
```powershell
.\build.ps1 -script .\AutoUpdater\build.cake -target Installer --buildVersion='1.0.0'
```
#### Deploy update
Files are added to zip archive
```powershell
.\build.ps1 -script .\AutoUpdater\build.cake -target Release --buildVersion='1.0.1'
```

### NetSparkle
NetSparkle supports updates only through installers
```powershell
.\build.ps1 -script .\NetSparkle\build.cake --buildVersion='1.0.0'
```

## Serve updates
Updates must be served from `Build` folder on `http://localhost:8000`

In the root directory there is a `serve.bat` which will serve static files by `python3` module
```powershell
.\serve.bat
```

[Big list of http static server one-liners](https://gist.github.com/willurd/5720255)

## Additionally Used
* Cake
* NSIS
