Instead of sorting in memory, or merge-sorting with text files, insert all records into a temporary SQLite database and sort them from there:

```csharp
public class LineSorterSqlite
{
	private FileInfo _inputFile = default!;
	private FileInfo _outputFile = default!;
	private string _newLine = default!;

	public void SortFile(string inputFileFullName,
		string outputFileFullName,
		string? newLine = null)
	{
		if (!File.Exists(inputFileFullName))
		{
			throw new FileNotFoundException(inputFileFullName);
		}

		_inputFile = new(inputFileFullName);
		_outputFile = new(outputFileFullName);

		_newLine = newLine ?? Environment.NewLine;

		using (SqliteConnection conn = new(""))
		{
			conn.Open();
			using (SqliteCommand comm = conn.CreateCommand())
			{
				comm.CommandText = "CREATE TABLE lines (line TEXT);";
				comm.ExecuteNonQuery();
			}
			InsertInputFileToDatabase(conn);
			RetrieveDataFromDatabase(conn);
		}
	}

	private void InsertInputFileToDatabase(SqliteConnection conn)
	{
		List<string> linesToInsert = [];

		using StreamReader reader = _inputFile.OpenText();
		string? line;
		while ((line = reader.ReadLine()) is not null)
		{
			if (linesToInsert.Count == 25_000) { InsertLines(linesToInsert, conn); linesToInsert.Clear(); }
			linesToInsert.Add(line);
		}
		InsertLines(linesToInsert, conn);
	}

	private void InsertLines(List<string> lines, SqliteConnection conn)
	{
		if (lines.Count == 0) { return; }
		using SqliteTransaction tran = conn.BeginTransaction();
		foreach (string line in lines)
		{
			using SqliteCommand comm = conn.CreateCommand();
			comm.CommandText = "INSERT INTO lines (line) VALUES (@line)";
			comm.Parameters.AddWithValue("@line", line);
			comm.ExecuteNonQuery();
		}
		tran.Commit();
	}

	private void RetrieveDataFromDatabase(SqliteConnection conn)
	{
		using StreamWriter writer = _outputFile.CreateText();
		using SqliteCommand comm = conn.CreateCommand();
		comm.CommandText = "SELECT line from lines ORDER BY line;";
		using SqliteDataReader rdr = comm.ExecuteReader();
		while (rdr.Read())
		{
			writer.WriteLine(rdr.GetString(0));
		}
	}
}
```