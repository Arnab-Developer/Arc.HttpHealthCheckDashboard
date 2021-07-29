# Http health check dashboard

[![CI CD](https://github.com/Arnab-Developer/Arc.HttpHealthCheckDashboard/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/Arnab-Developer/Arc.HttpHealthCheckDashboard/actions/workflows/ci-cd.yml)
![Nuget](https://img.shields.io/nuget/v/Arc.HttpHealthCheckDashboard)

This is a library for http health check dashboard. It has been hosted in 
[NuGet](https://www.nuget.org/packages/Arc.HttpHealthCheckDashboard/). 
Use below command to install this in your .NET application.

```
dotnet add package Arc.HttpHealthCheckDashboard
```

Create your class which inherits `BaseHealthCheck` and it should work with default naming
convention. The default naming convention is `[ClassName]HealthCheck`.

```csharp
public class [ClassName]HealthCheck : BaseHealthCheck
{
    public [ClassName]HealthCheck(IEnumerable<ApiDetail> urlDetails, ICommonHealthCheck commonHealthCheck)
        : base(urlDetails, commonHealthCheck)
    {
    }
}
```

To use a different naming convention override the `GetMatch()` method. In below example it is
using a different naming convention which is `[ClassName]HC`.

```csharp
public class GmailHC : BaseHealthCheck
{
    public GmailHC(IEnumerable<ApiDetail> urlDetails, ICommonHealthCheck commonHealthCheck)
        : base(urlDetails, commonHealthCheck)
    {
    }

    protected override Predicate<ApiDetail> GetMatch()
    {
        int indexOfHealthCheck = GetType().Name.IndexOf("HC");
        string apiNameToTest = GetType().Name.Substring(0, indexOfHealthCheck);
        return new Predicate<ApiDetail>(u => u.Name == apiNameToTest && u.IsEnable);
    }
}
```

There is a 
[dashboard app](https://github.com/Arnab-Developer/HttpHealthCheckDashboard) 
which uses the library to check health of some http endpoints. This is to show 
how you can use this library in your app.
