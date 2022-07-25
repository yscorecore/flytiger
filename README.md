# FlyTiger

> FlyTiger is a dotnet source generator class library. It provide several useful features to make coding on c# more easier.

![build](https://github.com/yscorecore/FlyTiger/workflows/build/badge.svg)
[![codecov](https://codecov.io/gh/yscorecore/FlyTiger/branch/master/graph/badge.svg)](https://codecov.io/gh/yscorecore/FlyTiger) 
[![Nuget](https://img.shields.io/nuget/v/FlyTiger)](https://nuget.org/packages/FlyTiger/) 
[![GitHub](https://img.shields.io/github/license/yscorecore/FlyTiger)](https://github.com/yscorecore/FlyTiger/blob/master/LICENSE)
[![Sonar](https://sonarcloud.io/api/project_badges/measure?project=yscorecore_changedb&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=yscorecore_flytiger)


## How it works

FlyTiger use [Source Generator](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview) to emit C# source code during compilation. It will generate both feature code and attribute class code to your output assembly, so the target assembly won't depend on the `FlyTiger` assembly in runtime anymore.  
<img src="res/architecture.png">

## Features
- [AutoConstructor](https://yscorecore.github.io/flytiger/features/AutoConstructor.html)
- [ConvertTo](https://yscorecore.github.io/flytiger/features/ConvertTo.html)
- [SingletonPattern](https://yscorecore.github.io/flytiger/features/SingletonPattern.html)
- [AutoNotify](https://yscorecore.github.io/flytiger/features/AutoNotify.html)
## How to use
1. Add `FlyTiger` package in your csharp project.
    ```bash
    dotnet add package FlyTiger 
    ```

1. Use attribute class
    ```csharp
    using FlyTiger;
    namespace ClassLibrary1
    {
        [AutoConstructor]
        public partial class User
        {
            private readonly string name;
            private readonly string age;
        }
    }
    ```
    More details usage please goto the [document](https://yscorecore.github.io/flytiger/).


## Maintainers
[@Pengbo Yang](https://github.com/obpt123)


## Contributing

Feel free to dive in! [Open an issue](https://github.com/yscorecore/flytiger/issues/new) or submit PRs.

Standard Readme follows the [Contributor Covenant](http://contributor-covenant.org/version/1/3/0/) Code of Conduct.

### Contributors

This project exists thanks to all the people who contribute. 
<a href="https://github.com/yscorecore/flytiger/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=yscorecore/flytiger" />
</a>


## License

[MIT](LICENSE) © Pengbo Yang


