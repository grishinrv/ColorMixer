# README #
[github repository](https://github.com/grishinrv/ColorMixer)
### Used dependencies ###
* [.NET SDK 6.0.413](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [.NET Desktop development workload (WPF)](https://learn.microsoft.com/en-us/visualstudio/install/modify-visual-studio?view=vs-2022)
* [CommunityToolkit.Mvvm 8.2.1](https://www.nuget.org/packages/CommunityToolkit.Mvvm/8.2.1)
* [MahApps.Metro 2.4.10](https://www.nuget.org/packages/MahApps.Metro/2.4.10)
* [EficazFramework.Data.SqlLite 6.1.5](https://www.nuget.org/packages/EficazFramework.Data.SqlLite/6.1.5)
* [Microsoft.EntityFrameworkCore 7.0.10](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/7.0.10)

### How do I get set up? ###

Just launch ColorMixer.Application.exe (Provided build is self-contained).
On first launch application will set it's data storage to %APPDATA%/ColorMixer

### How do I compile from the source code? ###
* first, you need to install [.NET SDK 6.0.413](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and 
WPf workload for Visual Studio 2022 (or newer)
* then, clone the [repository](https://github.com/grishinrv/ColorMixer) or use provided sources from archive; navigate to the ColorMixer.Application folder:
 ```
cd .\ColorMixer.Application 
```
* run the following command to build the project:
```
dotnet build
```

### Features ###
* Persistent storage of settings
* All three mixing types are supported (Additive, Subtractive, Average)
* Localized for English, Russian and German
* Async I/O (Including view models initialzation and disposal)
* Supports UI themes 
* Supports Dark mode
* Support Sync with OS theme 
* Modern hamburger layout
* DI
* Global exception handling

### Known Issues ###
* AdonerDecoration logic should be put inside ColorNodeControl
* No tree-structure validation
* No logging
* Several Warnings (easy to fix, though)
* No unit tests
* No visualization of what mixing type was used

### Future development ideas ###
* Couple todos to implement
* Fix issues
* Extract domain logic to separate project
* Cross-platform console version
* Support multiple Color Nodes Sets (currently only one is supported)
* Support persistence for Color Nodes Sets
* Hotkeys
* Performance improvements (rendering optimization, check for memory leaks
* Unit tests
* Interactive in-build app demo
* Installer / uninstaller
* Allow user to create his own custom theme
* Allow copying of color parameters
* Support setting of initial window size
* Support Color Nodes removal
