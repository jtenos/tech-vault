```csharp
services.AddControllersWithViews().AddRazorOptions(options =>
    options.ViewLocationFormats.Add("/Views/SpecialPlace/{0}.cshtml"));
```