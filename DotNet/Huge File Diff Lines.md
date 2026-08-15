```csharp
using System;
using System.IO;
using static System.Console;

WriteLine("Please note: Lines in the files must be sorted for this to work");
WriteLine("Usage: HugeFileLineDiff.exe first_file.txt second_file.txt");
WriteLine("This will generate {first_file}.diff_{timestamp}.txt and {second_file}.diff_{timestamp}.txt, which you can compare with a regular diff tool");

string inputFile1 = args[0];
string inputFile2 = args[1];

using StreamReader reader1 = new(inputFile1);
using StreamReader reader2 = new(inputFile2);

DateTime now = DateTime.Now;

string outputFile1 = new FileInfo($"{Path.GetFileNameWithoutExtension(inputFile1)}.diff_{now:yyyyMMddHHmmss}{new FileInfo(inputFile1).Extension}").FullName;
string outputFile2 = new FileInfo($"{Path.GetFileNameWithoutExtension(inputFile2)}.diff_{now:yyyyMMddHHmmss}{new FileInfo(inputFile2).Extension}").FullName;

using StreamWriter writer1 = new(outputFile1);
using StreamWriter writer2 = new(outputFile2);

string line1;
string line2;

line1 = reader1.ReadLine();
line2 = reader2.ReadLine();
while (true)
{
    if (line1 is null && line2 is null)
    {
        break;
    }
    if (line1 is null)
    {
        // First file is done, write remaining lines from file 2
        writer2.WriteLine(line2);
        line2 = reader2.ReadLine();
        continue;
    }
    if (line2 is null)
    {
        // Second file is done, write remaining lines from file 1
        writer1.WriteLine(line1);
        line1 = reader1.ReadLine();
        continue;
    }
    if (string.Compare(line1, line2) == 0)
    {
        // Lines match, keep moving
        line1 = reader1.ReadLine();
        line2 = reader2.ReadLine();
        continue;
    }
    if (string.Compare(line1, line2) < 0)
    {
        // Line 1 is less than line 2 - write line 1 and advance it
        writer1.WriteLine(line1);
        line1 = reader1.ReadLine();
        continue;
    }
    if (string.Compare(line1, line2) > 0)
    {
        // Line 2 is less than line 1 - write line 2 and advance it
        writer2.WriteLine(line2);
        line2 = reader2.ReadLine();
    }
}
```