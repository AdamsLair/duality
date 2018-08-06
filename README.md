Duality - A 2D GameDev Framework
=======

Duality is a plugin based 2D game development framework based on C# and OpenTK. To get a quick overview, please visit the [project page](http://duality.adamslair.net). For documentation, take a look at our [docs page](http://dualitydocs.adamslair.net).

If you have questions or just want to say Hi, feel free to join us in the [forum](http://forum.adamslair.net) or our [chat](http://chat.adamslair.net). Also, feedback is always welcome! Bugs and feature requests that do not require further discussion, however, should be filed in the issue tracking system, directly here on github.

### Latest Binary Release

You can find the latest binary release [here](http://dualitybin.adamslair.net/). If you already have a Duality project, you can update your Duality version or any of its plugins in the [Package Manager](https://adamslair.github.io/duality-docs/pages/v2/Package-Management.html).

### Build status: 
| [Branch](https://github.com/AdamsLair/duality/wiki/Branch-Descriptions)  | Status |
|-------------|--------|
| master      | [![Build status](https://ci.appveyor.com/api/projects/status/eyxpet6jky1cqy6i/branch/master?svg=true)](https://ci.appveyor.com/project/AdamsLairBot/duality/branch/master) |
| release     | [![Build status](https://ci.appveyor.com/api/projects/status/eyxpet6jky1cqy6i/branch/release?svg=true)](https://ci.appveyor.com/project/AdamsLairBot/duality/branch/release) |
| develop-3.0 | [![Build status](https://ci.appveyor.com/api/projects/status/eyxpet6jky1cqy6i/branch/develop-3.0?svg=true)](https://ci.appveyor.com/project/AdamsLairBot/duality/branch/develop-3.0) |
| 1.x         | [![Build status](https://ci.appveyor.com/api/projects/status/eyxpet6jky1cqy6i/branch/1.x?svg=true)](https://ci.appveyor.com/project/AdamsLairBot/duality/branch/1.x)    |

### Building From Source

If you want to build Duality yourself, you can do so using Visual Studio or MonoDevelop by simply opening `Duality.sln` and selecting "Build Solution". On non-Windows systems, you will be able to build core projects only and have to unload any editor projects first. When building Duality from the command line, make sure to call `nuget restore Duality.sln` first, so the required packages can be restored.

All framework build results will be located in the shared `Build/Output` folder. Sample project build results will be separate, and located in their respective `Content/Plugins` subdirectories. To launch any of the included sample projects after building Duality, select it as a startup project in your IDE. Otherwise, you can use either `DualityEditor` or `DualityLauncher` as a startup project.

### Contributing

We're actively looking for contributors. Are you experienced with Duality and want to join the development team? Look at our [contribution guide](https://adamslair.github.io/duality-docs/pages/v2/how-to-contribute.html).

If you just want to say "Thank You", feel free to donate [on itch.io](http://adamslair.itch.io/duality).
