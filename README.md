# Pivotal .NET Core Web Api Template (PCF Enabled)

This is an opinionated .NET Core template for the dotnet new command.

## Getting Started

1. Install the template from [nuget.org](https://www.nuget.org/packages/PivotalServices.WebApiTemplate.CSharp)

    ```
    dotnet new -i PivotalServices.WebApiTemplate.CSharp
    ```

1. This should install a template with the shortname `pvtlwebapi`

    ```
    Templates                                         Short Name         Language          Tags
    ----------------------------------------------------------------------------------------------------------------------------
    PCF Ready .NET Core WebAPI                        pvtlwebapi         [C#]              PCF/WebAPI/Web
    ```

2. To generate a new project

    ```
    dotnet new pvtlwebapi -n <NAME_OF_SOLUTION>
    ```

1. Make sure you have the compatible .NET Core SDK versions installed

    Package Version 3.0.0 -> [.NET Core SDK Version 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)


1. Goto the folder and run either `build.bat` or `build.sh` for the initial build.


## Packaging this template

1. To package this template use the following command.
    ```
    dotnet pack -p:PackageVersion=[version-number]
    ```
