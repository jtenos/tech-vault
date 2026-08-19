```csharp
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

Person[] people = [
	new Person { Id = 1, FirstName = "John", LastName = "Doe", Age = 30, Email = "john.doe@example.com" },
	new Person { Id = 2, FirstName = "Jane", LastName = "Smith", Age = 25, Email = "jane.smith@example.com" },
	new Person { Id = 3, FirstName = "Bob", LastName = "Johnson", Age = 35, Email = "bob.johnson@example.com" }
];

Employee[] employees = [
	new Employee { EmployeeId = 101, FullName = "Alice Johnson", Department = "Engineering", Salary = 75000 },
	new Employee { EmployeeId = 102, FullName = "Bob Williams", Department = "Marketing", Salary = 65000 },
	new Employee { EmployeeId = 103, FullName = "Carol Davis", Department = "Sales", Salary = 70000 }
];

dynamic[] dynamicRecords = [
	new { Id = 1, Name = "Product A", Price = 19.99, InStock = true },
	new { Id = 2, Name = "Product B", Price = 29.99, InStock = false },
	new { Id = 3, Name = "Product C", Price = 39.99, InStock = true }
];

Product[] products = [
	new Product { ProductId = 1, ProductName = "Widget", UnitPrice = 9.99m, QuantityAvailable = 100 },
	new Product { ProductId = 2, ProductName = "Gadget", UnitPrice = 19.99m, QuantityAvailable = 50 },
	new Product { ProductId = 3, ProductName = "Doohickey", UnitPrice = 14.99m, QuantityAvailable = 75 }
];

Order[] orders = [
	new Order { OrderId = 1, OrderDate = DateTime.Now, Status = OrderStatus.Pending },
	new Order { OrderId = 2, OrderDate = DateTime.Now.AddDays(-1), Status = OrderStatus.Shipped },
	new Order { OrderId = 3, OrderDate = DateTime.Now.AddDays(-2), Status = OrderStatus.Delivered }
];

CsvConfiguration config = new(CultureInfo.InvariantCulture)
{
	Delimiter = ";",
	HasHeaderRecord = true,
	Quote = '"',
	TrimOptions = TrimOptions.Trim
};

WriteClasses();
ReadClasses();

WriteDynamics();
ReadDynamics();

WriteWithClassMaps();
ReadWithClassMaps();

WriteWithAttributes();
ReadWithAttributes();

WriteWithCustomConfig();
ReadWithCustomConfig();

WriteManually();
ReadManually();

ReadByIndex();

WriteWithTypeConversion();
ReadWithTypeConversion();

ReadWithMissingFields();

AppendToExistingFile();

WriteWithNoHeader();
ReadWithNoHeader();

ReadSpecificFields();

FieldValidation();


void WriteClasses()
{
	using StreamWriter writer = new("people.csv");
	using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);
	csv.WriteRecords(people);
}

void ReadClasses()
{
	using StreamReader reader = new("people.csv");
	using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
	Person[] records = [.. csv.GetRecords<Person>()];
	foreach (Person person in records)
	{
		Console.WriteLine($"{person.Id}: {person.FirstName} {person.LastName}, Age: {person.Age}, Email: {person.Email}");
	}
}

void WriteDynamics()
{
	using StreamWriter writer = new("products.csv");
	using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);
	csv.WriteRecords(dynamicRecords);
}

void ReadDynamics()
{
	using StreamReader reader = new("products.csv");
	using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
	dynamic[] records = [.. csv.GetRecords<dynamic>()];
	foreach (dynamic record in records)
	{
		Console.WriteLine($"{record.Id}: {record.Name}, Price: {record.Price}, In Stock: {record.InStock}");
	}
}

void WriteWithClassMaps()
{
	using StreamWriter writer = new("employees.csv");
	using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);
	csv.Context.RegisterClassMap<EmployeeMap>();
	csv.WriteRecords(employees);
}

void ReadWithClassMaps()
{
	using StreamReader reader = new("employees.csv");
	using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
	csv.Context.RegisterClassMap<EmployeeMap>();
	Employee[] records = [.. csv.GetRecords<Employee>()];
	foreach (Employee emp in records)
	{
		Console.WriteLine($"{emp.EmployeeId}: {emp.FullName}, {emp.Department}, ${emp.Salary}");
	}
}

void WriteWithAttributes()
{
	using StreamWriter writer = new("products_with_attributes.csv");
	using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);
	csv.WriteRecords(products);
}

void ReadWithAttributes()
{
	using StreamReader reader = new("products_with_attributes.csv");
	using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
	Product[] records = [.. csv.GetRecords<Product>()];
	foreach (Product product in records)
	{
		Console.WriteLine($"{product.ProductId}: {product.ProductName}, ${product.UnitPrice}, Qty: {product.QuantityAvailable}");
	}
}

void WriteWithCustomConfig()
{
	using StreamWriter writer = new("custom_config.csv");
	using CsvWriter csv = new(writer, config);
	csv.WriteRecords(people);
}

void ReadWithCustomConfig()
{
	using StreamReader reader = new("custom_config.csv");
	using CsvReader csv = new(reader, config);
	Person[] records = [.. csv.GetRecords<Person>()];
	foreach (Person person in records)
	{
		Console.WriteLine($"{person.FirstName} {person.LastName}");
	}
}

void WriteManually()
{
	using StreamWriter writer = new("manual.csv");
	using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);

	// Write headers
	csv.WriteField("ID");
	csv.WriteField("Name");
	csv.WriteField("Value");
	csv.NextRecord();

	// Write data rows
	csv.WriteField(1);
	csv.WriteField("Item 1");
	csv.WriteField(100.50);
	csv.NextRecord();

	csv.WriteField(2);
	csv.WriteField("Item 2");
	csv.WriteField(200.75);
	csv.NextRecord();
}

void ReadManually()
{
	using StreamReader reader = new("manual.csv");
	using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
	csv.Read();
	csv.ReadHeader();
	while (csv.Read())
	{
		int id = csv.GetField<int>("ID");
		string name = csv.GetField<string>("Name")!;
		double value = csv.GetField<double>("Value");
		Console.WriteLine($"{id}: {name}, Value: {value}");
	}
}

void ReadByIndex()
{
	using StreamReader reader = new("manual.csv");
	using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
	csv.Read();
	csv.ReadHeader();

	while (csv.Read())
	{
		int id = csv.GetField<int>(0);
		string? name = csv.GetField<string>(1);
		double value = csv.GetField<double>(2);
		Console.WriteLine($"{id}: {name}, Value: {value}");
	}
}

void WriteWithTypeConversion()
{
	using StreamWriter writer = new("orders.csv");
	using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);
	csv.WriteRecords(orders);
}

void ReadWithTypeConversion()
{
	using StreamReader reader = new("orders.csv");
	using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
	Order[] records = [.. csv.GetRecords<Order>()];
	foreach (var order in records)
	{
		Console.WriteLine($"Order {order.OrderId}: {order.OrderDate:yyyy-MM-dd}, Status: {order.Status}");
	}
}

void ReadWithMissingFields()
{
	CsvConfiguration missingFieldConfig = new(CultureInfo.InvariantCulture)
	{
		MissingFieldFound = null,
		HeaderValidated = null
	};
}

void AppendToExistingFile()
{
	Person[] newPeople = [
		new Person { Id = 4, FirstName = "Alice", LastName = "Brown", Age = 28, Email = "alice.brown@example.com" }
	];

	using FileStream stream = File.Open("people.csv", FileMode.Append);
	using StreamWriter writer = new(stream);
	using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);
	csv.WriteRecords(newPeople);
}

void WriteWithNoHeader()
{
	Person[] newPeople = [
		new Person { Id = 4, FirstName = "Alice", LastName = "Brown", Age = 28, Email = "alice.brown@example.com" }
	];

	CsvConfiguration noHeaderConfig = new (CultureInfo.InvariantCulture)
	{
		HasHeaderRecord = false
	};

	using StreamWriter writer = new("no_header.csv");
	using CsvWriter csv = new(writer, noHeaderConfig);
	csv.WriteRecords(people);
}

void ReadWithNoHeader()
{
	CsvConfiguration noHeaderConfig = new (CultureInfo.InvariantCulture)
	{
		HasHeaderRecord = false
	};

	using StreamReader reader = new("no_header.csv");
	using CsvReader csv = new(reader, noHeaderConfig);
	Person[] records = [.. csv.GetRecords<Person>()];
	foreach (var person in records)
	{
		Console.WriteLine($"{person.FirstName} {person.LastName}");
	}
}

void ReadSpecificFields()
{
	using StreamReader reader = new("people.csv");
	using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
	csv.Read();
	csv.ReadHeader();

	while (csv.Read())
	{
		string? firstName = csv.GetField<string>("FirstName");
		string? lastName = csv.GetField<string>("LastName");
		Console.WriteLine($"{firstName} {lastName}");
	}
}

void FieldValidation()
{
	using StreamReader reader = new("people.csv");
	using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
	List<Person> records = [];
	csv.Read();
	csv.ReadHeader();

	while (csv.Read())
	{
		try
		{
			Person person = csv.GetRecord<Person>();
			records.Add(person);
		}
		catch (CsvHelper.TypeConversion.TypeConverterException ex)
		{
			Console.WriteLine($"Error on row {csv.Context?.Parser?.Row}: {ex.Message}");
		}
	}
}
```

## Employee
```csharp
using CsvHelper.Configuration;

public class Employee
{
    public int EmployeeId { get; set; }
    public string FullName { get; set; } = default!;
    public string Department { get; set; } = default!;
    public decimal Salary { get; set; }
}

public class EmployeeMap : ClassMap<Employee>
{
    public EmployeeMap()
    {
        Map(m => m.EmployeeId).Name("Employee ID");
        Map(m => m.FullName).Name("Full Name");
        Map(m => m.Department).Name("Department");
        Map(m => m.Salary).Name("Salary").TypeConverter<SalaryConverter>();
    }
}
```

## Order
```csharp
public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
}
```

# OrderStatus
```csharp
public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
```

## Person
```csharp
public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int Age { get; set; }
    public string Email { get; set; } = default!;
}
```

## Product
```csharp
using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

public class Product
{
    [Index(0)]
    [Name("ID")]
    public int ProductId { get; set; }

    [Index(1)]
    [Name("Product Name")]
    public string ProductName { get; set; } = default!;

    [Index(2)]
    [Name("Price")]
    public decimal UnitPrice { get; set; }

    [Index(3)]
    [Name("Quantity")]
    public int QuantityAvailable { get; set; }
}
```

## SalaryConverter
```csharp
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

public class SalaryConverter : CsvHelper.TypeConversion.DefaultTypeConverter
{
	public override string ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
	{
		if (value == null)
		{
			return string.Empty;
		}
		decimal salary = (decimal)value;
		return $"${salary:N2}";
	}

	public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
	{
		text = (text ?? "").Replace("$", "").Replace(",", "");
		return decimal.Parse(text, CultureInfo.InvariantCulture);
	}
}
```