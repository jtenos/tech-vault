```csharp
// If the validation fails, the application will error out on startup

builder.Services.AddOptions<FooSettings>()
    .BindConfiguration(FooSettings.ConfigurationSection)
    .ValidateDataAnnotations()
    .ValidateOnStart();

public class FooSettings
{
    public const string ConfigurationSection = "Foo";

    [Required]
    public required string Name { get; set; }

    [Range(1, 100)]
    public required int SomeNumber { get; set; }
}

/* appsettings.json:
{
    "Foo": {
        "Name": "hello",
        "SomeNumber": 10
    }
}
*/
```