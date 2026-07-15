```csharp
// Dapper parameter types

// By default, strings are Unicode variable length (NVARCHAR(4000) in SQL Server)

// To do NVARCHAR(50), set your value to
new DbString { Value = "Hello", Length = 50 }

// To do NCHAR(50), set your value to
new DbString { Value = "Hello", Length = 50, IsFixedLength = true }

// To do VARCHAR(50), set your value to
new DbString { Value = "Hello", Length = 50, IsAnsi = true }

// To do CHAR(50), set your value to
new DbString { Value = "Hello", Length = 50, IsFixedLength = true, IsAnsi = true }

//To do decimals with a particular precision and scale, use DynamicParameters instead of anonymous types
```
