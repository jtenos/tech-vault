```csharp
public class Option1Interface(ILogger<Option1Interface> logger
    /* Inject more here */) : IMiddleware
{
    async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
    {
        logger.LogInformation("...");
        await next(context);
    }
}

// Option 1 needs to be registered in DI
builder.Services.AddSingleton<Option1Interface>();
app.UseMiddleware<Option1Interface>();

//////////////////////////

public class Option2ByConvention(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context,
        ILogger<Option2ByConvention> logger /* Inject more here */)
    {
        logger.LogInformation("...");
        await next(context);
    }
}

// Option 2 does not need to be in DI
app.UseMiddleware<OtherMiddleware>();

// Option 3 is just a lambda, no need for a class
app.Use(async (context, next) =>
{
    // DI through context.RequestServices
    ISomeRepo repo = context.RequestServices.GetRequiredService<ISomeRepo>();
    await next(context);
});
```