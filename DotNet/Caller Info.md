```csharp
using System.Runtime.CompilerServices;

DoSomething();

static void DoSomething() => LogCallerInfo();

static void LogCallerInfo(
    [CallerMemberName] string? memberName = default,
    [CallerFilePath] string? filePath = default,
    [CallerLineNumber] int lineNumber = default
) => Console.WriteLine($"""
    Member Name: {memberName}
    File Path: {filePath}
    Line Number: {lineNumber}
    """);

/*
dotnet run .\Program.cs
Member Name: <Main>$
File Path: C:\repos\myrepo\Program.cs
Line Number: 5
*/
```