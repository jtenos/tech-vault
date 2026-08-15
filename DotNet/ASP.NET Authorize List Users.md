```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapRazorPages());
        }

public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAssertion(context => Configuration.GetSection("Users")
                        .GetChildren()
                        .Select(x => x.Value.ToLowerInvariant()).Contains(context.User.Identity?.Name?.ToLowerInvariant()));
                });
            });
        }

/*
{
"Users": [
        "mydomain\\johnsmith",
        "mydomain\\xyz"
    ]
}
*/
```