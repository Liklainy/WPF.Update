# WPF.Update

[![Build status](https://ci.appveyor.com/api/projects/status/dvotdh27wfivm836/branch/master?svg=true)](https://ci.appveyor.com/project/Liklainy/wpf-update/branch/master)

WPF.Update is a project to show available open source ways to deploy a WPF application

## Projects

* [Squirrel.Windows](https://github.com/Squirrel/Squirrel.Windows)
* [AutoUpdater.NET](https://github.com/ravibpatel/AutoUpdater.NET)

### Squirrel
```powershell
.\build.ps1 -script .\Squirrel\build.cake --buildVersion='1.0.0'
```
### AutoUpdater.NET
AutoUpdater doesn't support installer creation out-of-the-box
#### Deploy installer
Files are packaged to Setup.exe by NSIS
```powershell
.\build.ps1 -script .\AutoUpdater\build.cake -target Installer --buildVersion='1.0.0'
```
#### Deploy update
Files are added to zip archive 
```powershell
.\build.ps1 -script .\AutoUpdater\build.cake --buildVersion='1.0.1'
```

## Serve updates
Updates must be served from `Build` folder on `http://localhost:8000`
In the root directory there is a `serve.bat` which will serve static files by `python3` module
```powershell
.\serve.bat
```

## Additionally Used
* Cake
* NSIS
