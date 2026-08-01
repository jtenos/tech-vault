Add some other service to your DI and follow the options pattern:

```csharp
internal class ThirdPartyService
{
    public string ProviderType { get; set; } = "Some Default";
    public int TimeoutSeconds { get; set; } = 30;
}

internal static class ThirdPartyServiceExtensions
{
    public static IServiceCollection AddThirdPartyService(
        this IServiceCollection services, Action<ThirdPartyService>? options = null)
    {
        if (options is null) return services.AddSingleton<ThirdPartyService>();
       
        ThirdPartyService thirdPartyService = new();
        options(thirdPartyService);
        return services.AddSingleton(thirdPartyService);
    }
}

services.AddThirdPartyService(service => {
    service.ProviderType = "Some other provider type";
    service.TimeoutSeconds = 45;
});
```