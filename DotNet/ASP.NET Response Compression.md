```csharp
services.AddResponseCompression(options =>
    options.Providers.Add<GzipCompressionProvider>()
);
...
app.UseResponseCompression();
```