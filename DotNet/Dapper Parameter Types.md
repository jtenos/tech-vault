
## DbString
```csharp
// By default, strings are Unicode variable length (NVARCHAR(4000) in SQL Server)

// To do NVARCHAR(50), set your value to
new DbString { Value = "Hello", Length = 50 }

// To do NCHAR(50), set your value to
new DbString { Value = "Hello", Length = 50, IsFixedLength = true }

// To do VARCHAR(50), set your value to
new DbString { Value = "Hello", Length = 50, IsAnsi = true }

// To do CHAR(50), set your value to
new DbString { Value = "Hello", Length = 50, IsFixedLength = true, IsAnsi = true }
```

## Decimal
To do decimals with a particular precision and scale, use DynamicParameters instead of anonymous types

## Anonymous Type
```csharp
	IEnumerable<Person> people = conn.Query<Person>(
		"SELECT * FROM Person WHERE Age > @MinAge",
		new { MinAge = 26 }
	);
```

## DynamicParameters
```csharp
	DynamicParameters parameters = new();
	parameters.Add("@FirstName", "John", DbType.String);
	parameters.Add("@MinAge", 25, DbType.Int32);

	IEnumerable<Person> people = conn.Query<Person>(
		"SELECT * FROM Person WHERE FirstName = @FirstName AND Age >= @MinAge",
		parameters
	);
```

## Output Parameters
Some databases like SQLite don't support output parameters.
```csharp
	DynamicParameters outputParams = new();
	outputParams.Add("@Id", 1);
	outputParams.Add("@Count",
		dbType: DbType.Int32,
		direction: ParameterDirection.Output
	);
```