# Alternate Web Root Utilities

A .NET Standard library that provides razor tag helpers that allow web root relative paths ("~/") to be replaced by an alternative location rather than the local path.

[![Build Status](https://dev.azure.com/kyleherzog/AlternateWebRootUtilities/_apis/build/status/AlternateWebRootUtilities?branchName=master)](https://dev.azure.com/kyleherzog/AlternateWebRootUtilities/_build/latest?definitionId=11&branchName=master)

See the [changelog](CHANGELOG.md) for changes.

## Overview
Hosting static content from the wwwroot folder in an ASP.NET Core web application is convenient, but can have an impact on CPU usage and bandwidth used when hosted in an Azure App Service environment.  The Alternate Web Root Utilities package enables one to easily offload the static files, allowing the hosting to occur on a separate web host like Azure Storage.

The configuration settings allow for the functionality to be enabled only in certain environments.  Therefore, when developing locally, the local wwwroot folder can be used, but when hosted in a production environment an alternative location can be specified. 

### Path Replacement
If the alternative web root base URL is set to `https://contoso.com/files` the following replacement would take place. 

#### Razor Input
```
<img src="~/images/myimage.png" />
```

#### HTML Output
```
<img src="https://contoso.com/files/images/myimage.png" />
```

Only paths that are web root relative (start with "~") will have a replacement take place.  

If the configuration of replacement location is null, no replacement will take place.  This can be desirable in a local development environment.

## Getting Started
The alternate web root is initialized in the `Startup.cs`. This globally enables the alternate web root relative path replacement for all supported tags.
```C#
public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseAlternateWebRoot(new Uri("https://mystorageaccount.blob.core.windows.net/content/"));
}
```

The replacement of the web root relative paths is driven by razor tag helpers.  Therefore, the tag helpers must be registered in the `_ViewImports.cs` file.
```C#
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, AlternateWebRootUtilities
```

## Application Settings Files
Configuration via the `appsettings.json` files is available.
```
{
    "AlternateWebRoot": {
        "BaseUrl": "https://mystorageaccount.blob.core.windows.net/content/"
    }
}
```

When leveraging the `appsettings.json` files, the initialization in the `Startup.cs` is simplified.
```C#
public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseAlternateWebRoot();
}
```

## Supported HTML Tags
The following HTML tags are currently supported.
- img
- source
- link
- script

## Tag Level Support
By default, the alternate web root functionality is enabled globally.  However, the functionality can also be limited to individual HTML tags.

To limit the functionality to the tag level, the `IsGloballyEnabled` setting should be set to `false`.

```
{
    "AlternateWebRoot": {
        "BaseUrl": "https://mystorageaccount.blob.core.windows.net/content/",
        "IsGloballyEnabled" : false
    }
}
```

Then, enable individual tags with the `asp-alternate-web-root` attribute.
```
<img src="~/images/house.jpg" asp-alternate-web-root="true" />
```