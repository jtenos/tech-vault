This doesn't do a bulk insert, but rather loops through each person and executes a command for it. Fine for small numbers of executions, but if you need real bulk, then do real bulk.

```csharp
	Person[] newPeople = [
		new Person { FirstName = "Emma", LastName = "Davis", Age = 26, Email = "emma.davis@example.com" },
		new Person { FirstName = "Frank", LastName = "Miller", Age = 31, Email = "frank.miller@example.com" },
		new Person { FirstName = "Grace", LastName = "Wilson", Age = 27, Email = "grace.wilson@example.com" }
	];

	int rowsAffected = conn.Execute(
		"INSERT INTO Person (FirstName, LastName, Age, Email) VALUES (@FirstName, @LastName, @Age, @Email)",
		newPeople
	);
```