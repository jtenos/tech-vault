```csharp
public class Something : IDisposable {
#region Disposable
    private bool _disposed;
 
    void IDisposable.Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
 
    protected void Dispose(bool disposing) {
        if (!_disposed) {
            if (disposing) {
                DisposeManagedResources();
            }
 
            DisposeUnmanagedResources();
            _disposed = true;
        }
    }
 
    protected virtual void DisposeManagedResources() { }
    protected virtual void DisposeUnmanagedResources() { }
 
    ~Something() {
        Dispose(false);
    }
#endregion
}
```