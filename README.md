Duality - A 2D GameDev Framework
=======

Duality is a plugin based 2D game development framework based on C# and OpenTK. To get a quick overview, please visit the [project page](https://adamslair.github.io/duality). For documentation, take a look at our [docs page](https://adamslair.github.io/duality-docs).

If you have questions or just want to say Hi, feel free to check out the [discussions](https://github.com/AdamsLair/duality/discussions) area, or join us in the [chat](https://discord.gg/JZxzXrzXc6). Also, feedback is always welcome! Bugs and feature requests that do not require further discussion, however, should be filed in the issue tracking system, directly here on github.

### Latest Binary Release

You can find the latest binary release [here](https://github.com/AdamsLair/duality/releases/download/v3.0/Duality.zip). If you already have a Duality project, you can update your Duality version or any of its plugins in the [Package Manager](https://adamslair.github.io/duality-docs/pages/v3/Package-Management.html).

### Build status: 
| [Branch](https://github.com/AdamsLair/duality/wiki/Branch-Descriptions)  | Status |
|-------------|--------|
| master      | [![Build status](https://ci.appveyor.com/api/projects/status/eyxpet6jky1cqy6i/branch/master?svg=true)](https://ci.appveyor.com/project/AdamsLairBot/duality/branch/master) |
| release     | [![Build status](https://ci.appveyor.com/api/projects/status/eyxpet6jky1cqy6i/branch/release?svg=true)](https://ci.appveyor.com/project/AdamsLairBot/duality/branch/release) |
| 1.x         | [![Build status](https://ci.appveyor.com/api/projects/status/eyxpet6jky1cqy6i/branch/archive/1.x?svg=true)](https://ci.appveyor.com/project/AdamsLairBot/duality/branch/archive/1.x)    |
| 2.x         | [![Build status](https://ci.appveyor.com/api/projects/status/eyxpet6jky1cqy6i/branch/archive/2.x?svg=true)](https://ci.appveyor.com/project/AdamsLairBot/duality/branch/archive/2.x)    |
| 3.x         | [![Build status](https://ci.appveyor.com/api/projects/status/eyxpet6jky1cqy6i/branch/archive/3.x?svg=true)](https://ci.appveyor.com/project/AdamsLairBot/duality/branch/archive/3.x)    |

### Building From Source

If you want to build Duality yourself, you can do so using Visual Studio or MonoDevelop by simply opening `Duality.sln` and selecting "Build Solution". On non-Windows systems, you will be able to build core projects only and have to unload any editor projects first. When building Duality from the command line, make sure to call `nuget restore Duality.sln` first, so the required packages can be restored.

All framework build results will be located in the shared `Build/Output` folder. Sample project build results will be separate, and located in their respective `Content/Plugins` subdirectories. To launch any of the included sample projects after building Duality, select it as a startup project in your IDE. Otherwise, you can use either `DualityEditor` or `DualityLauncher` as a startup project.

### ⚠️ Project status

The project is on hold indefinitely - probably forever. The latest version is stable and usable, with some caveats:

* The package manager is not working, as it broke following a change in the NuGet APIs. If you want to check and use the available plugins you can [search for Duality on NuGet](https://www.nuget.org/packages?q=duality+plugin&includeComputedFrameworks=true&prerel=true&sortby=relevance), download the package and unzip the dlls in the plugins folder of Duality.
* A v4 was being worked on, and is the one currently available in the master branch. It should be pretty stable and you can build it as described above. That said, no further work is scheduled to happen in the near and possibly far future.

This said, I'm going to thank everyone who worked on this project. It was a blast while it lasted.

SirePi

----------

### Maintainers
- [SirePi](https://github.com/SirePi)
- [Barsonax](https://github.com/Barsonax)
- [Ilexp](https://github.com/ilexp) (creator, now inactive but still around to give us advice)
