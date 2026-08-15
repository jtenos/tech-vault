```csharp
// <PackageReference Include="Microsoft.Extensions.Hosting" Version="9.0.4" />

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((HostBuilderContext context, IServiceCollection services) =>
    {
        services.AddHostedService<Program>();
    })
    .Build();

using CancellationTokenSource cts = new();

await host.StartAsync(cts.Token);
await host.StopAsync(cts.Token);

partial class Program : IHostedService
{
    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Starting...");
        return Task.CompletedTask;
    }
    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping...");
        return Task.CompletedTask;
    }
}
```