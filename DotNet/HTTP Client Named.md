```csharp
await Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) =>
	{
		services.AddHttpClient(HttpClientNames.Coffee, (serviceProvider, client) =>
		{
			// Set headers or whatever else you need here
			client.BaseAddress = new("https://api.sampleapis.com/coffee/");
		});

		services.AddHostedService<Program>();
	})
	.Build()
	.RunAsync();

partial class Program(IHttpClientFactory httpClientFactory)
	: BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		// Will have the Coffee defaults
		var coffeeClient = _httpClientFactory.CreateClient(HttpClientNames.Coffee);
	}
}

static class HttpClientNames
{
	public const string Coffee = nameof(Coffee);
}
```