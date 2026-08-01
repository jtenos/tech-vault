No options pattern or built in stuff, just pulling from the config itself

```shell
dotnet add package Microsoft.Extensions.Configuration.Binder
dotnet add package Microsoft.Extensions.Configuration.Json
```

appsettings.json:
```json
```

.csproj:
```xml
<ItemGroup>
	<None Update="appsettings.json">
		<CopyToOutputDirectory>Always</CopyToOutputDirectory>
	</None>
</ItemGroup>
```

```csharp
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using static System.Console;

WriteLine();
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Simple way to get a string value:
string stringValue = config["StringValue"];
WriteLine($"stringValue={stringValue}");

// Special ConnectionStrings section:
string connectionString = config.GetConnectionString("MyApp");
WriteLine($"connectionString={connectionString}");

// Get a simple non-string value:
int intValue = config.GetValue<int>("IntegerValue");
WriteLine($"intValue={intValue}");

bool boolValue = config.GetValue<bool>("BooleanValue");
WriteLine($"boolValue={boolValue}");

// String Array - GetSection then GetChildren - the items are IConfigurationSection, so you can get .Value
string[] stringArray = config.GetSection("StringArray").GetChildren().Select(x => x.Value).ToArray();
WriteLine($"stringArray={JsonSerializer.Serialize(stringArray)}");

// String Array - Simpler way, just by getting the section, then a Get call
stringArray = config.GetSection("StringArray").Get<string[]>();
WriteLine($"stringArray={JsonSerializer.Serialize(stringArray)}");

// Object Array - GetSection then Get
CityState[] objectArray = config.GetSection("ObjectArray").Get<CityState[]>();
WriteLine($"objectArray={JsonSerializer.Serialize(objectArray)}");

// For a complex object, you can create a new empty object, then bind to it:
ComplexThing complexObject = new();
config.Bind("ComplexObject", complexObject);
WriteLine($"complexObject={JsonSerializer.Serialize(complexObject)}");

// Or the simpler GetSection and Get call:
complexObject = config.GetSection("ComplexObject").Get<ComplexThing>();
WriteLine($"complexObject={JsonSerializer.Serialize(complexObject)}");

// To retrieve a nested value, use a colon separator
string complexCity = config["ComplexObject:Location:City"];
WriteLine($"complexObjectCity={complexCity}");

// For an array, still use the colon separator and the index
int complexFirstPersonID = config.GetValue<int>("ComplexObject:PersonIDs:0");
WriteLine($"complexFirstPersonID={complexFirstPersonID}");

WriteLine();

Write($"Make your change now, then press ENTER: ");
ReadLine();

complexCity = config["ComplexObject:Location:City"];
WriteLine($"complexObjectCity={complexCity}");

record CityState(string City, string State)
{
    public CityState() : this("", "") { }
}

record ComplexThing(CityState Location, int[] PersonIDs)
{
    public ComplexThing() : this(new(), Array.Empty<int>()) { }
}
```