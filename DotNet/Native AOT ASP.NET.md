Is there a way to build a .NET application into native code so it is small instead of the 100MB that you get when building a self-contained app?  
Yes, there is a way to significantly reduce the size of a self-contained .NET application from the 100MB range to a much smaller native executable. The technology to do this is called .NET  
Native Ahead-of-Time (AOT) Compilation.  
.NET Native AOT Compilation  
Native AOT compilation allows you to compile your application's Intermediate Language (IL) code directly into platform-specific native machine code at the time of publishing.  
How it Reduces Size  
• Eliminates the JIT Compiler: Traditional self-contained apps include the full .NET runtime and the Just-In-Time (JIT) compiler. Native AOT removes the JIT compiler, as the code is  
already native, which results in a smaller binary.  
• Aggressive Trimming: Native AOT inherently relies on aggressive trimming (also called tree-shaking). This process statically analyzes your code and includes only the specific  
parts of the framework and runtime libraries that your application actually uses, removing all unused code.  
This process results in a single, small, self-contained native executable that is often only a few megabytes for a simple application, and generally much smaller than a standard self-contained  
app.

How to Enable Native AOT  
You can enable Native AOT compilation by adding a property to your project file (.csproj) or by passing an argument during the publish process.  
1. In the Project File (.csproj)

Adding this to your <PropertyGroup> is the recommended approach as it enables AOT- compatibility analyzers during development:

XML  
<PropertyGroup>  
<PublishAot>true</PublishAot>  
</PropertyGroup>

2. Using the .NET CLI  
You must publish the application for a specific Runtime Identifier (RID) since the output is native code specific to an OS and architecture (e.g., Windows x64 or Linux Arm64).  
Bash  
dotnet publish -r <RID> -c Release -p:PublishAot=true  
Example for Windows 64-bit:  
Bash  
dotnet publish -r win-x64 -c Release -p:PublishAot=true  
Additional Size Optimization  
You can further reduce the size by:  
• Disabling Globalization: If your application doesn't require support for multiple cultures  
(e.g., it only uses English formatting), you can dramatically reduce size by setting:  
XML  
<InvariantGlobalization>true</InvariantGlobalization>  
• Optimization Preference: You can explicitly tell the AOT compiler to prioritize size over  
maximum speed:  
XML  
<OptimizationPreference>Size</OptimizationPreference>

Important Considerations  
While Native AOT is great for size reduction, it has some limitations:  
• Platform Specificity: The resulting executable is not portable. You must compile the  
application separately for each target platform (e.g., win-x64, linux-x64).  
• Compatibility: Native AOT relies on static analysis, which means it does not support  
dynamic code generation at runtime. Features that rely heavily on reflection, runtime  
code generation (like System.Reflection.Emit), or JIT-specific code paths may not work or  
will require source-code adjustments and annotations.

• Testing: You must thoroughly test your AOT-published application, as the aggressive trimming might accidentally remove code that the compiler didn't realize was being  
used (especially if accessed via complex reflection patterns).  
Would you like me to find the specific Runtime Identifier (RID) for your target platform?  
Does this work for ASP.NET?  
Yes, Native AOT compilation works for ASP.NET Core, particularly with the modern Minimal  
APIs.  
Starting with .NET 8, Microsoft introduced official support for Native AOT with ASP.NET Core.  
This allows you to compile your web applications into small, single-file, native executables, offering benefits like faster startup times and a smaller deployment size, which is especially  
valuable for cloud-native or containerized workloads.  
However, there are important limitations you must be aware of:

ASP.NET Core Native AOT Limitations  
Native AOT works best with application models that use minimal dynamic code generation. This means not all ASP.NET Core features are fully compatible yet.  
1. Supported/Recommended Features  
Feature Compatibility  
Status

Notes

Minimal APIs Fully  
Supported

This is the primary and recommended way to build AOT-compatible web services.

System.Text.Json Supported Requires the use of Source Generators for serialization/deserialization instead of reflection.

WebApplication.CreateSlimBuilder() Recommended Uses a stripped-down builder that only includes essential features, minimizing size.

Feature Compatibility  
Status

Notes

EF Core Partial/Limited Entity Framework Core's full functionality often relies on dynamic runtime features (like proxy generation) that are not AOT- compatible, though efforts are underway for greater support.

2. Features Not Supported or Limited  
The following features rely heavily on dynamic code features (like reflection) that conflict with the static nature of Native AOT and are generally not supported for AOT-published apps:  
• ASP.NET Core MVC (controllers and views)  
• Razor Pages  
• Blazor Server  
• SignalR  
• Entity Framework Core (full dynamic usage)  
• Unbound Reflection: Any code that uses reflection to inspect or create types at runtime (a common pattern in older libraries like Newtonsoft.Json).  
3. Compatibility Impact  
If your application uses features that are not AOT-compatible, the publish process will typically issue warnings. If you ignore these warnings, the application may compile but throw exceptions  
at runtime when it tries to execute the incompatible code.  
Benefits of Native AOT for ASP.NET Core  
When you stick to AOT-compatible features like Minimal APIs, the benefits for your web service are significant:  
4. Tiny Executable/Container Size: The single binary can be dramatically smaller than a traditional self-contained application, leading to much smaller Docker images (often under 20MB for a simple API on Alpine Linux).  
5. Faster Cold Startup: By eliminating the Just-In-Time (JIT) compilation step, Native AOT apps start in milliseconds, which is a massive performance boost for scenarios like:

• Serverless/FaaS (e.g., AWS Lambda, Azure Functions) where cold start time is critical.  
• Microservices that are scaled up rapidly or frequently restarted.  
3. Lower Memory Usage: AOT-compiled apps often have a smaller memory footprint.  
Would you like to see an example of how to set up an ASP.NET Core Minimal API to be published with Native AOT?  
You said System.Text.Json requires source generators. So does that mean Serialize and  
Deserialize don't work?  
That's a great clarifying question!  
No, it doesn't mean that Serialize and Deserialize don't work. It means you must use specific overloads of those methods that accept the source-generated metadata instead of  
relying on the default reflection-based overloads.  
In a Native AOT application, the code that uses reflection to discover a type's properties at runtime is either trimmed away or will cause a runtime exception. Source generators fix this by  
performing that discovery at compile time and generating code that knows exactly how to serialize/deserialize your objects.  
Here is the breakdown of the change you need to make.

The Change in Serialization Code

When moving to Native AOT, you transition from reflection-based calls to source-generator-  
based calls.

1. The Reflection Way (Not AOT Compatible)  
This is the standard, easy way most people use, but it requires runtime reflection:  
C#  
// NOT AOT COMPATIBLE  
// The JsonSerializer uses reflection to figure out what a 'Person' is  
var json = JsonSerializer.Serialize(person);  
var p = JsonSerializer.Deserialize<Person>(json);  
2. The Source-Generated Way (AOT Compatible)

You must pass an object that contains the pre-compiled type information for Person. This object  
is generated by the Source Generator at build time.  
C#  
// AOT COMPATIBLE  
// The generated context provides the TypeInfo for 'Person'  
var json = JsonSerializer.Serialize(person, AppJsonSerializerContext.Default.Person);  
var p = JsonSerializer.Deserialize(json, AppJsonSerializerContext.Default.Person);  
Notice that you pass AppJsonSerializerContext.Default.Person as an extra argument to  
the Serialize and Deserialize methods.

Implementation Steps  
To make this work in your project, you must define a custom JSON Serialization Context.  
Step 1: Define Your Data Model  
Assume you have a simple data model:  
C#  
public class Todo  
{  
public int Id { get; set; }  
public string? Title { get; set; }  
public bool IsComplete { get; set; }  
}  
Step 2: Create the Serialization Context  
You create a partial class that inherits from JsonSerializerContext and use  
the [JsonSerializable] attribute to tell the source generator which types it needs to generate  
code for:  
C#  
using System.Text.Json.Serialization;

// Tell the Source Generator to include serialization code for the Todo class  
// You must also include types for any collections you use as the root object, e.g., List<Todo>  
[JsonSerializable(typeof(Todo))]  
[JsonSerializable(typeof(List<Todo>))]  
internal partial class AppJsonSerializerContext : JsonSerializerContext  
{  
}  
The partial keyword is essential because the C# compiler generates the rest of the class  
definition (including the Default static property and the Todo property) behind the scenes.  
Step 3: Integrate into ASP.NET Core (Minimal APIs)  
For an ASP.NET Core Minimal API, you register this context globally in your Program.cs file. This  
ensures that when the framework handles incoming JSON requests or writes outgoing JSON  
responses, it uses your AOT-compatible source generator:  
C#  
using System.Text.Json.Serialization;  
// ... other usings

var builder = WebApplication.CreateSlimBuilder(args);

// **This is the key step in an ASP.NET Minimal API:**  
builder.Services.ConfigureHttpJsonOptions(options =>  
{  
options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);  
});

// ... rest of your code

This configuration tells the ASP.NET framework, "When you need JSON info, use  
my AppJsonSerializerContext first."  
By following these steps, you maintain the familiar JsonSerializer.Serialize/Deserialize pattern  
but use the overloads that are Native AOT-safe and enable a tiny executable size.  
Would you like me to show you a full, simple Program.cs example that is Native AOT-ready?  
Yes please

Here is a full, simple ASP.NET Core Minimal API example that is configured to be Native AOT-  
ready using Source-Generated JSON.

AOT-Ready Minimal API Example  
This example demonstrates the three key components needed for Native AOT: the project file  
setup, the serialization context, and the Program.cs configuration.  
1. The Project File (.csproj)  
Ensure your project is targeting .NET 8 or later and includes the crucial AOT properties.  
XML  
<Project Sdk="Microsoft.NET.Sdk.Web">

<PropertyGroup>  
<TargetFramework>net8.0</TargetFramework>  
<Nullable>enable</Nullable>  
<ImplicitUsings>enable</ImplicitUsings>

<PublishAot>true</PublishAot>

<EnableConfigurationBindingGenerator>true</EnableConfigurationBindingGenerator>  
</PropertyGroup>

</Project>

2. The Data Model and Context (Todo.cs / AppJsonSerializerContext.cs)  
We define the data model and use the [JsonSerializable] attribute to instruct the source  
generator which types to generate code for.  
C#  
// Define the model  
public class Todo  
{  
public int Id { get; set; }  
public string? Title { get; set; }  
public bool IsComplete { get; set; }  
}

// Define the source-generated JSON context  
using System.Text.Json.Serialization;

[JsonSerializable(typeof(Todo))]  
[JsonSerializable(typeof(List<Todo>))] // Must include collection types used as root objects  
internal partial class AppJsonSerializerContext : JsonSerializerContext  
{  
}

3. The Application Startup (Program.cs)  
The Program.cs registers the generated context with the web application's JSON handler. We  
also use the CreateSlimBuilder to reduce the application footprint further.  
C#  
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Http.Json;

// Use the stripped-down builder for a smaller footprint  
var builder = WebApplication.CreateSlimBuilder(args);

// **KEY STEP: Configure JSON to use the AOT source generator**  
builder.Services.Configure<JsonOptions>(options =>  
{  
// Insert the generated context into the resolver chain  
options.SerializerOptions.TypeInfoResolverChain.Insert(0,  
AppJsonSerializerContext.Default);  
});

// Configure other essential services only  
builder.Services.AddEndpointsApiExplorer();  
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();  
app.UseSwaggerUI();

// Example Minimal API Endpoint  
app.MapGet("/todos/{id}", (int id) =>  
{  
// For demonstration, return a hardcoded object

return Results.Ok(new Todo { Id = id, Title = $"Task {id}", IsComplete = false });  
})  
.WithName("GetTodo")  
.WithOpenApi();

app.Run();

4. Publishing the App  
To build and publish this for a small native executable (e.g., for Linux containers):  
Bash  
dotnet publish -r linux-x64 -c Release -p:PublishAot=true --self-contained true  
The resulting executable in the publish directory will be a single, small, native binary ready for deployment.