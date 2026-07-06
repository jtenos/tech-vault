```csharp
using System.Runtime.CompilerServices;

partial class Program
{
    static void Main()
    {
        DoSomething();
    }

    static void DoSomething()
    {
        LogCallerInfo();
    }

    static void LogCallerInfo(
        [CallerMemberName] string? memberName = default,
        [CallerFilePath] string? filePath = default,
        [CallerLineNumber] int lineNumber = default
    ) => Console.WriteLine($"Member Name: {memberName}, File Path: {filePath}, Line Number: {lineNumber}");
}

/*
PS C:\Users\joe1\tmp\caller> dotnet run        
Member Name: DoSomething, File Path: C:\Users\joe1\tmp\caller\Program.cs, Line Number: 13
*/

```