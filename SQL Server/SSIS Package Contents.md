```sql
SELECT packagedata
FROM msdb.dbo.sysssispackages
WHERE name = 'MyPackageName' -- assuming your package has a unique name on the server
```

```csharp
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Data.SqlClient;

async IAsyncEnumerable<byte[]> ReadFileAsync()
{
    int startingByte = 1;
    while (true)
    {
        byte[] bytes;
        using var conn = new SqlConnection("server=XXXXX;database=msdb;integrated security=true;");
        await conn.OpenAsync().ConfigureAwait(false);
        using var comm = conn.CreateCommand();
        comm.CommandText = @"
            SELECT substring(packagedata, @StartingByte, 8000) [FileContents]
            FROM msdb.dbo.sysssispackages
            WHERE name = 'MyPackageName' -- assuming your package has a unique name on the server
        ";
        comm.Parameters.Add(new SqlParameter("@StartingByte", SqlDbType.Int) { Value = startingByte });
        using var rdr = await comm.ExecuteReaderAsync().ConfigureAwait(false);
        if (!await rdr.ReadAsync().ConfigureAwait(false))
        {
            break;
        }
        bytes = (byte[])rdr[0];
        if (bytes == null || bytes.Length == 0)
        {
            break;
        }
        startingByte += bytes.Length;
        yield return bytes;
    }
}
 
var fileWriter = File.OpenWrite("output.xml");
await foreach (var byteArray in ReadFileAsync())
{
    fileWriter.Write(byteArray, 0, byteArray.Length);
}
```