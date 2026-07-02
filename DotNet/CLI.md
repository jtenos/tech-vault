```shell
dotnet new sln
dotnet new console --name MyConsoleApp
dotnet new webapp --name MyRazorPagesApp
dotnet new mvc --name MyMvcApp
dotnet new webapi --name MyWebApi
dotnet new classlib --name MyLibrary
dotnet sln add MyConsoleApp
dotnet sln add MyRazorPagesApp
dotnet sln add MyMvcApp
dotnet sln add MyWebApi
dotnet sln add MyLibrary
dotnet sln list
cd MyConsoleApp
dotnet add reference ..\MyLibrary
dotnet add package Newtonsoft.Json
echo "using static System.Console;" > Program.cs
echo "using Newtonsoft.Json;" >> Program.cs
echo "WriteLine(JsonConvert.SerializeObject(new MyLibrary.Class1()));" >> Program.cs
dotnet run
echo "@echo off" > publish.bat
echo "rd /s/q dist" >> publish.bat
echo "dotnet publish --output dist --runtime win-x64 --configuration Release --self-contained" >> publish.bat
.\publish.bat
cd dist
.\MyConsoleApp.exe

dotnet tool install --global dotnet-search
dotnet-search --take 1 newtonsoft
```