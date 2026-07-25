```csharp
using SqlConnection conn = new(...);
conn.Open();
conn.FireInfoMessageEventOnUserErrors = true;
conn.InfoMessage += (sender, e) => Console.WriteLine(e.Message);

// https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection.fireinfomessageeventonusererrors?view=dotnet-plat-ext-5.0
```