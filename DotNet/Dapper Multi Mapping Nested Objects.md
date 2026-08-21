```csharp
	// Map results from JOIN query to nested objects
	string sql = @"
		SELECT 
			p.PersonId, p.FirstName, p.LastName, p.Age, p.Email,
			a.AddressId, a.PersonId, a.Street, a.City, a.State, a.ZipCode
		FROM Person p
		INNER JOIN Address a ON p.Id = a.PersonId
	";

	IEnumerable<PersonWithAddress> peopleWithAddresses = conn.Query<Person, Address, PersonWithAddress>(
		sql,
		(person, address) =>
		{
			PersonWithAddress result = new()
			{
				Id = person.Id,
				FirstName = person.FirstName,
				LastName = person.LastName,
				Age = person.Age,
				Email = person.Email,
				Address = address
			};
			return result;
		},
		splitOn: "AddressId" // This is the point where it moves to the next type
	);
```