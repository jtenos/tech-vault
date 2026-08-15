```csharp
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;

const string DIR = @"C:\Users\joe\OneDrive\Pictures";
DirectoryInfo dir = new(DIR);

List<FileInfo> files = (
    from file in dir.EnumerateFiles("*", SearchOption.AllDirectories)
    where file.Length > 0
    group file by file.Length into g
    where g.Count() > 1
    select g
).SelectMany(g => g).ToList();

long totalByteCount = files.Select(f => f.Length).Sum();

using SqliteConnection conn = new($"Data Source=C:\\Temp\\mydb-{Guid.NewGuid()}.sqlite3");
conn.Open();
using (SqliteCommand comm = conn.CreateCommand())
{
    comm.CommandText = @"PRAGMA journal_mode=WAL;";
    comm.ExecuteNonQuery();
}
using (SqliteCommand comm = conn.CreateCommand())
{
    comm.CommandText = """
        CREATE TABLE Files (Name TEXT, Hash TEXT);
        """;
    comm.ExecuteNonQuery();
}

long byteCount = 0;
foreach (FileInfo file in files)
{
    Console.WriteLine($"{byteCount:#,##0}/{totalByteCount:#,##0} ({100 * byteCount / totalByteCount}%) ");

    // Move cursor up one line
    Console.SetCursorPosition(0, Console.CursorTop - 1);

    byteCount += file.Length;
    string hash = GetHash(file);
    using SqliteCommand comm = conn.CreateCommand();
    comm.CommandText = "INSERT INTO Files (Name, Hash) VALUES (@Name, @Hash);";
    comm.Parameters.AddWithValue("@Name", file.FullName);
    comm.Parameters.AddWithValue("@Hash", hash);
    comm.ExecuteNonQuery();
}

using (SqliteCommand comm = conn.CreateCommand())
{
    using StreamWriter writer = new($"output-{Guid.NewGuid()}.txt");
    comm.CommandText = """
        SELECT Name, Hash FROM Files
        WHERE Hash IN (
            SELECT Hash
            FROM Files
            GROUP BY Hash
            HAVING COUNT(1) > 1
        )
        ORDER BY Hash, Name;
        """;
    using SqliteDataReader rdr = comm.ExecuteReader();
    string prevHash = "Z";
    while (rdr.Read())
    {
        string name = rdr.GetString(0);
        string newHash = rdr.GetString(1);
        if (newHash != prevHash)
        {
            writer.WriteLine("------------------------");
            writer.WriteLine(newHash);
        }
        writer.WriteLine(name);
        prevHash = newHash;
    }
}

// Randomly delete all but one from every group
// Queue<(string Hash, string FileName)> queue = [];
// using (SqliteCommand comm = conn.CreateCommand())
// {
//     comm.CommandText = """
//         SELECT Hash, MAX(Name) AS Name
//         FROM Files
//         GROUP BY Hash;
//         """;
//     using SqliteDataReader rdr = comm.ExecuteReader();
//     while (rdr.Read())
//     {
//         string hash = rdr.GetString(0);
//         string fileName = rdr.GetString(1);
//         queue.Enqueue((hash, fileName));
//     }
// }

// while (queue.TryDequeue(out (string Hash, string FileName) item))
// {
//     Console.WriteLine($"Hash: {item.Hash}");
//     using SqliteCommand comm = conn.CreateCommand();
//     comm.CommandText = """
//         SELECT Name FROM Files WHERE Hash = @Hash AND Name <> @Name;
//         """;
//     comm.Parameters.AddWithValue("@Hash", item.Hash);
//     comm.Parameters.AddWithValue("@Name", item.FileName);

//     using SqliteDataReader rdr = comm.ExecuteReader();
//     while (rdr.Read())
//     {
//         string name = rdr.GetString(0);
//         Console.WriteLine($"Deleting {name}");
//         try{File.Delete(name);}catch{}
//         //Console.ReadLine();
//     }

// }

static string GetHash(FileInfo file)
{
    //Console.WriteLine($"Hashing {file.Name}");
    using FileStream stream = file.OpenRead();
    using MD5 md5 = MD5.Create();
    byte[] hash = md5.ComputeHash(stream);
    return BitConverter.ToString(hash);
}
```