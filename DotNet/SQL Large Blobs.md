```csharp
using var conn = new SqlConnection(_config.GetConnectionString("MyDB"));
await conn.OpenAsync().ConfigureAwait(false);
using var tran = (SqlTransaction)await conn.BeginTransactionAsync();
int fileID;
using (var comm = conn.CreateCommand())
{
    comm.Transaction = tran;
    comm.CommandText = @"
        insert dbo.Files (FileName, FileContents)
        values (@FileName, 0x);
        select scope_identity();
";
    comm.Parameters.AddWithValue("@FileName", fileName);
    fileID = (int)(await comm.ExecuteScalarAsync().ConfigureAwait(false));
}

using var fileStream = System.IO.File.OpenRead(file);
byte[] buffer = new byte[8000];
int count;
while ((count = fileStream.Read(buffer, 0, buffer.Length)) > 0)
{
    byte[] tmp = new byte[count];
    Array.Copy(buffer, 0, tmp, 0, count);
    using var comm = conn.CreateCommand();
    comm.Transaction = tran;
    comm.CommandText = "update dbo.Files set FileContents.write(@ContentChunk, null, null) where FileID = @FileID";
    comm.Parameters.AddWithValue("@FileID", fileID);
    comm.Parameters.AddWithValue("@ContentChunk", tmp);
    await comm.ExecuteNonQueryAsync();
}
await tran.CommitAsync();

private async IAsyncEnumerable<byte[]> ReadFileAsync(int fileID)
{
    int startingByte = 1;
    while (true)
    {
        byte[] bytes;
        using var conn = new SqlConnection(_config.GetConnectionString("MyDB"));
        await conn.OpenAsync().ConfigureAwait(false);
        using var comm = conn.CreateCommand();
        comm.CommandText = @"
            select substring(FileContents, @StartingByte, 8000) [FileContents]
            from dbo.Files
            where FileID = @FileID;
        ";
        comm.Parameters.Add(new SqlParameter("@FileID", SqlDbType.Int) { Value = fileID });
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


await foreach (var byteArray in ReadImportFileAsync(importBatchID))
{
    fileWriter.Write(byteArray, 0, byteArray.Length);
}
```