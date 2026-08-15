```csharp
public class LineSorter
{
    private FileInfo _inputFile = default!;
    private FileInfo _outputFile = default!;
    private Comparison<string> _comparison = default!;
    private string _newLine = default!;
    private DirectoryInfo _tempDir = default!;
    private int _numLinesPerTempFile;

    public void SortFile(string inputFileFullName,
        string outputFileFullName,
        Comparison<string>? comparison = null,
        string? newLine = null,
        int numLinesPerTempFile = 1000)
    {
        if (!File.Exists(inputFileFullName))
        {
            throw new FileNotFoundException(inputFileFullName);
        }

        _inputFile = new(inputFileFullName);
        _outputFile = new(outputFileFullName);

        _comparison = comparison ?? ((s1, s2) => s1.CompareTo(s2));
        _newLine = newLine ?? Environment.NewLine;

        _numLinesPerTempFile = numLinesPerTempFile;

        if (_numLinesPerTempFile < 1 || _numLinesPerTempFile > 1_000_000)
        {
            _numLinesPerTempFile = 1_000;
        }

        _tempDir = new(Path.Combine(Path.GetTempPath(), $"file-line-sorter-{Guid.NewGuid():N}"));
        _tempDir.Create();
        _tempDir.Refresh();

        SplitInputFile();
        SortTemporaryFiles();
        MergeTemporaryFiles();

        _tempDir.Delete(recursive: true);
        _tempDir.Refresh();
    }

    private void MergeTemporaryFiles()
    {
        FileInfo[] inputFiles = _tempDir.GetFiles("*.input");
        while (inputFiles.Length > 1)
        {
            MergeFiles(inputFiles[0], inputFiles[1]);

            inputFiles = _tempDir.GetFiles("*.input");
        }

        inputFiles[0].MoveTo(_outputFile.FullName, overwrite: true);
        _outputFile.Refresh();
    }

    private void MergeFiles(FileInfo file1, FileInfo file2)
    {
        FileInfo outputFile = GetTempFile();
        using (StreamWriter writer = outputFile.AppendText())
        {
            using StreamReader reader1 = file1.OpenText();
            using StreamReader reader2 = file2.OpenText();
            string? line1 = reader1.ReadLine();
            string? line2 = reader2.ReadLine();
            while (true)
            {
                if (line1 is null && line2 is null) { break; }

                if (line1 is null)
                {
                    writer.WriteLine(line2);
                    line2 = reader2.ReadLine();
                    continue;
                }

                if (line2 is null)
                {
                    writer.WriteLine(line1);
                    line1 = reader1.ReadLine();
                    continue;
                }

                int compare = _comparison(line1, line2);
                switch (compare)
                {
                    case -1:
                        writer.WriteLine(line1);
                        line1 = reader1.ReadLine();
                        break;
                    case 0:
                        writer.WriteLine(line1);
                        line1 = reader1.ReadLine();
                        writer.WriteLine(line2);
                        line2 = reader2.ReadLine();
                        break;
                    case 1:
                        writer.WriteLine(line2);
                        line2 = reader2.ReadLine();
                        break;
                }
            }
        }
        file1.Delete();
        file2.Delete();
    }

    private void SortTemporaryFiles()
    {
        foreach (FileInfo file in _tempDir.GetFiles())
        {
            string[] lines = File.ReadAllLines(file.FullName);
            Array.Sort(lines, _comparison);
            File.WriteAllLines(file.FullName, lines);
        }
    }

    private void SplitInputFile()
    {
        using StreamReader reader = _inputFile.OpenText();
        string? line;
        int outputFileCount = 0;
        StreamWriter? writer = null;
        while ((line = reader.ReadLine()) is not null)
        {
            if (writer is null)
            {
                writer ??= GetTempFile().AppendText();
            }
            writer.Write(line);
            writer.Write(_newLine);
            ++outputFileCount;
            if (outputFileCount == _numLinesPerTempFile)
            {
                writer.Dispose();
                writer = null;
                outputFileCount = 0;
            }
        }
        writer?.Dispose();
    }

    private FileInfo GetTempFile() => new(Path.Combine(_tempDir.FullName, $"{DateTime.Now.Ticks}.{Guid.NewGuid():N}.input"));
}
```