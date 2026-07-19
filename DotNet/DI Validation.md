New way:
```csharp
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.ConfigureContainer(new DefaultServiceProviderFactory(new()
{
	ValidateScopes = true,
	ValidateOnBuild = true
}));
```

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostBuilder hostBuilder = new();
hostBuilder.UseDefaultServiceProvider((context, options) =>
{
    // With the validation added, the host will not build at all because the WidgetRepository
    // is required but not in DI.

    // Without this validation, the host will build and run, and the exception
    // won't happen until it tries to create the WidgetService.

    options.ValidateScopes = true;
    options.ValidateOnBuild = true;

});
hostBuilder.ConfigureServices((context, services) =>
{
    //services.AddSingleton<WidgetRepository>();
    services.AddSingleton<WidgetService>();
});

using IHost host = hostBuilder.Build(); // This is where the exception will be with validation

IHostApplicationLifetime lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

lifetime.ApplicationStarted.Register(() => Console.WriteLine("Started"));
lifetime.ApplicationStopping.Register(() => Console.WriteLine("Stopping firing"));
lifetime.ApplicationStopped.Register(() => Console.WriteLine("Stopped firing"));

host.Start();

// This is where the exception will be without validation
WidgetService widgetService = host.Services.GetRequiredService<WidgetService>(); 

Console.WriteLine($"Widget: {widgetService.GetWidget()}");

host.WaitForShutdown();

class WidgetService(WidgetRepository repo) { public Widget GetWidget() => repo.GetWidget(); }
class WidgetRepository { public Widget GetWidget() => new(Guid.NewGuid()); }
record class Widget(Guid id);
```
