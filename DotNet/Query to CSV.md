```csharp
/*
<PackageReference Include="CsvHelper" Version="33.0.1" />
<PackageReference Include="Microsoft.Data.SqlClient" Version="6.0.1" />
*/

using SqlConnection conn = new(cs);
conn.Open();

using SqlCommand comm = conn.CreateCommand();
comm.CommandText = query;

using SqlDataReader rdr = comm.ExecuteReader();

bool headerWritten = false;

while (rdr.Read())
{
    if (!headerWritten)
    {
        for (int i = 0; i < rdr.FieldCount; i++)
        {
            csv.WriteField(rdr.GetName(i));
        }
        csv.NextRecord();
        headerWritten = true;
    }
    for (int i = 0; i < rdr.FieldCount; i++)
    {
        csv.WriteField(rdr.GetValue(i));
    }
    csv.NextRecord();
}
```