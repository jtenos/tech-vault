```csharp
using Ardalis.GuardClauses;

// Guard.Against.Null - Throws ArgumentNullException if null
string? name = null;
try
{
    Guard.Against.Null(name);
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Guard.Against.Null caught: {ex.Message}");
}

// Guard.Against.NullOrEmpty - Throws ArgumentException if null or empty string
string? emptyString = "";
try
{
    Guard.Against.NullOrEmpty(emptyString);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Guard.Against.NullOrEmpty caught: {ex.Message}");
}

// Guard.Against.NullOrWhiteSpace - Throws ArgumentException if null, empty, or whitespace
string? whitespaceString = "   ";
try
{
    Guard.Against.NullOrWhiteSpace(whitespaceString);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Guard.Against.NullOrWhiteSpace caught: {ex.Message}");
}

// Guard.Against.OutOfRange - Throws ArgumentOutOfRangeException if value is outside range
int age = 150;
try
{
    Guard.Against.OutOfRange(age, nameof(age), 0, 120);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"Guard.Against.OutOfRange caught: {ex.Message}");
}

// Guard.Against.Zero - Throws ArgumentException if value is zero
int count = 0;
try
{
    Guard.Against.Zero(count, nameof(count));
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Guard.Against.Zero caught: {ex.Message}");
}

// Guard.Against.Negative - Throws ArgumentException if value is negative
int quantity = -5;
try
{
    Guard.Against.Negative(quantity, nameof(quantity));
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Guard.Against.Negative caught: {ex.Message}");
}

// Guard.Against.NegativeOrZero - Throws ArgumentException if value is negative or zero
int price = 0;
try
{
    Guard.Against.NegativeOrZero(price, nameof(price));
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Guard.Against.NegativeOrZero caught: {ex.Message}");
}

// Guard.Against.InvalidInput - Throws ArgumentException if predicate returns false (predicate should return true for valid input)
string email = "invalid-email";
try
{
    Guard.Against.InvalidInput(email, nameof(email), e => e.Contains("@"), "Email must contain @");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Guard.Against.InvalidInput caught: {ex.Message}");
}

// Valid examples that don't throw
string validName = "John";
Guard.Against.Null(validName);
Console.WriteLine($"Valid name passed: {validName}");

int validAge = 30;
Guard.Against.OutOfRange(validAge, nameof(validAge), 0, 120);
Console.WriteLine($"Valid age passed: {validAge}");

string validEmail = "user@example.com";
Guard.Against.InvalidInput(validEmail, nameof(validEmail), e => e.Contains("@"), "Email must contain @");
Console.WriteLine($"Valid email passed: {validEmail}");
```