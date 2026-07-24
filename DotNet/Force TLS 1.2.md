```csharp
// In .NET Framework 4.0+, disable TLS 1.0 and force 1.1 or 1.2:
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

// In prior .NET Framework versions, you can force it without the enum value:
public const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
public const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
ServicePointManager.SecurityProtocol = Tls12;
```