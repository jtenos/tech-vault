```csharp
void ShowDisposable()
{
    using IServiceScope scope = _serviceProvider.CreateScope();

    // This gets disposed when the scope is disposed
    MyDisposable myDisposable = scope.ServiceProvider.GetRequiredService<MyDisposable>();
}

```