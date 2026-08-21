```csharp
	using SqlMapper.GridReader multi = conn.QueryMultiple(@"
		SELECT * FROM [Person] WHERE [Age] > @MinAge;
		SELECT * FROM [Order] WHERE [TotalAmount] > @MinAmount;
	", new { MinAge = 25, MinAmount = 80 });

	IEnumerable<Person> people = multi.Read<Person>();
	IEnumerable<Order> orders = multi.Read<Order>();
```