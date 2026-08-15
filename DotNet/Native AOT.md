# Option 1: Add to csproj

```xml
<PropertyGroup>
  <PublishAot>true</PublishAot>
</PropertyGroup>
```

# Option 2: CLI

```
dotnet publish -r <RID> -c Release -p:PublishAot=true
```

(win-x64, osx-arm64, etc.)

# Additional Optimization:

```xml
<InvariantGlobalization>true</InvariantGlobalization>
<OptimizationPreference>Size</OptimizationPreference> 
```

# Compatibility

Native AOT relies on static analysis, which means it does not support dynamic code generation at runtime. Features that rely heavily on reflection, runtime code generation (like System.Reflection.Emit), or JIT-specific code paths may not work or will require source-code adjustments and annotations.

# Testing

You must thoroughly test your AOT-published application, as the aggressive trimming might accidentally remove code that the compiler didn't realize was being used (especially if accessed via complex reflection patterns).

# ASPNET

Yes, Native AOT compilation works for ASPNET Core, particularly with the modern Minimal APIs. Starting with .NET 8, Microsoft introduced official support for Native AOT with ASP.NET Core. This allows you to compile your web applications into small, single-file, native executables, offering benefits like faster startup times and a smaller deployment size, which is especially valuable for cloud-native or containerized workloads. However, there are important limitations you must be aware of:

* Minimal APIs: Fully supported  
* WebApplication.CreateSlimBuilder(): Recommended, uses a stripped-down builder that only includes essential features, minimizing size.  
* MVC: Not supported  
* Razor Pages: Not supported  
* Blazor Server: Not supported  
* SignalR: Not supported  
* Unbound Reflection (including Newtonsoft.Json): Not supported

# EF Core

Partial support only, since full functionality relies on dynamic runtime features

# System.Text.Json

Requires the use of Source Generators So does that mean Serialize and Deserialize don't work? No, it doesn't mean that Serialize and Deserialize don't work. It means you must use specific overloads of those methods that accept the source-generated metadata instead of relying on the default reflection-based overloads. In a Native AOT application, the code that uses reflection to discover a type's properties at runtime is either trimmed away or will cause a runtime exception. Source generators fix this by performing that discovery at compile time and generating code that knows exactly how to serialize/deserialize your objects. Here is the breakdown of the change you need to make. The Change in Serialization Code When moving to Native AOT, you transition from reflection-based calls to source-generator-based calls.

```c#
// NOT AOT COMPATIBLE
var json = JsonSerializer.Serialize(person);
var p = JsonSerializer.Deserialize<Person>(json);
 
// AOT COMPATIBLE
var json = JsonSerializer.Serialize(person, AppJsonSerializerContext.Default.Person);
var p = JsonSerializer.Deserialize(json, AppJsonSerializerContext.Default.Person);
 
[JsonSerializable(typeof(Person))]
[JsonSerializable(typeof(List<Person>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext{}
builder.Services.ConfigureHttpJsonOptions(options =>
{
  options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
```

```xml
<EnableConfigurationBindingGenerator>true</EnableConfigurationBindingGenerator>
```

