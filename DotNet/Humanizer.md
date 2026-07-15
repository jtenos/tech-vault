```csharp
using Humanizer;
using System.Globalization;

// String Humanization - Convert PascalCase to readable text
Console.WriteLine("PascalCaseInputStringExample".Humanize()); // Pascal case input string example
Console.WriteLine("Underscored_input_string_example".Humanize()); // Underscored input string example
Console.WriteLine("HTMLString".Humanize()); // HTML String

// Dehumanization - Reverse process
Console.WriteLine("Pascal case input string example".Dehumanize()); // PascalCaseInputStringExample

// Casing Transformations
Console.WriteLine("some title to transform".Transform(To.TitleCase)); // Some Title To Transform
Console.WriteLine("some sentence to transform".Transform(To.SentenceCase)); // Some sentence to transform
Console.WriteLine("LOUD TEXT".Transform(To.LowerCase)); // loud text
Console.WriteLine("quiet text".Transform(To.UpperCase)); // QUIET TEXT

// Date/Time Humanization - Relative dates
Console.WriteLine(DateTime.UtcNow.AddHours(-2).Humanize()); // 2 hours ago
Console.WriteLine(DateTime.UtcNow.AddDays(-1).Humanize()); // yesterday
Console.WriteLine(DateTime.UtcNow.AddDays(1).Humanize()); // tomorrow
Console.WriteLine(DateTime.UtcNow.AddMonths(-3).Humanize()); // 3 months ago
Console.WriteLine(DateTime.UtcNow.AddYears(-2).Humanize()); // 2 years ago

// TimeSpan Humanization
Console.WriteLine(TimeSpan.FromMilliseconds(1299).Humanize()); // 1 second
Console.WriteLine(TimeSpan.FromSeconds(125).Humanize()); // 2 minutes
Console.WriteLine(TimeSpan.FromMinutes(92).Humanize()); // an hour
Console.WriteLine(TimeSpan.FromHours(25).Humanize()); // a day
Console.WriteLine(TimeSpan.FromDays(45).Humanize()); // a month
Console.WriteLine(TimeSpan.FromDays(365).Humanize()); // a year

// TimeSpan with precision
Console.WriteLine(TimeSpan.FromMinutes(92).Humanize(precision: 2)); // 1 hour, 32 minutes
Console.WriteLine(TimeSpan.FromDays(2.5).Humanize(precision: 3)); // 2 days, 12 hours

// Number to Words
Console.WriteLine(1.ToWords()); // one
Console.WriteLine(42.ToWords()); // forty-two
Console.WriteLine(123.ToWords()); // one hundred and twenty-three
Console.WriteLine(1234567.ToWords()); // one million two hundred and thirty-four thousand five hundred and sixty-seven

// Number to Ordinal
Console.WriteLine("1".Ordinalize(CultureInfo.GetCultureInfo("en-US"))); // 1st
Console.WriteLine("2".Ordinalize(CultureInfo.GetCultureInfo("en-US"))); // 2nd
Console.WriteLine("3".Ordinalize(CultureInfo.GetCultureInfo("en-US"))); // 3rd
Console.WriteLine("4".Ordinalize(CultureInfo.GetCultureInfo("en-US"))); // 4th
Console.WriteLine("21".Ordinalize(CultureInfo.GetCultureInfo("en-US"))); // 21st
Console.WriteLine("100".Ordinalize(CultureInfo.GetCultureInfo("en-US"))); // 100th

// Ordinal Words
Console.WriteLine(1.ToOrdinalWords()); // first
Console.WriteLine(2.ToOrdinalWords()); // second
Console.WriteLine(12.ToOrdinalWords()); // twelfth
Console.WriteLine(21.ToOrdinalWords()); // twenty-first
Console.WriteLine(100.ToOrdinalWords()); // hundredth

// Roman Numerals
Console.WriteLine(1.ToRoman()); // I
Console.WriteLine(4.ToRoman()); // IV
Console.WriteLine(9.ToRoman()); // IX
Console.WriteLine(49.ToRoman()); // XLIX
Console.WriteLine(2024.ToRoman()); // MMXXIV
Console.WriteLine("MMXXIV".FromRoman()); // 2024

// Collections Humanization
var fruits = new[] { "apple", "banana", "cherry" };
Console.WriteLine(fruits.Humanize()); // apple, banana, and cherry
Console.WriteLine(fruits.Humanize("or")); // apple, banana, or cherry

var singleItem = new[] { "apple" };
Console.WriteLine(singleItem.Humanize()); // apple

var twoItems = new[] { "apple", "banana" };
Console.WriteLine(twoItems.Humanize()); // apple and banana

// Pluralization and Singularization
Console.WriteLine("car".Pluralize()); // cars
Console.WriteLine("child".Pluralize()); // children
Console.WriteLine("person".Pluralize()); // people
Console.WriteLine("mouse".Pluralize()); // mice

Console.WriteLine("cars".Singularize()); // car
Console.WriteLine("children".Singularize()); // child
Console.WriteLine("people".Singularize()); // person
Console.WriteLine("mice".Singularize()); // mouse

// Conditional Pluralization
Console.WriteLine("item".ToQuantity(0)); // 0 items
Console.WriteLine("item".ToQuantity(1)); // 1 item
Console.WriteLine("item".ToQuantity(5)); // 5 items
Console.WriteLine("person".ToQuantity(1)); // 1 person
Console.WriteLine("person".ToQuantity(3)); // 3 people

// With word format
Console.WriteLine("item".ToQuantity(0, ShowQuantityAs.Words)); // zero items
Console.WriteLine("item".ToQuantity(1, ShowQuantityAs.Words)); // one item
Console.WriteLine("item".ToQuantity(5, ShowQuantityAs.Words)); // five items

// Truncation
var longText = "This is a very long sentence that needs to be truncated at some point";
Console.WriteLine(longText.Truncate(20)); // This is a very long…
Console.WriteLine(longText.Truncate(20, Truncator.FixedLength)); // This is a very long…
Console.WriteLine(longText.Truncate(20, Truncator.FixedNumberOfCharacters)); // This is a very …
Console.WriteLine(longText.Truncate(20, Truncator.FixedNumberOfWords)); // This is a very long…

// Truncate from left
Console.WriteLine("Long text here".Truncate(10, "...", TruncateFrom.Left)); // ...xt here

// Byte Size Humanization
Console.WriteLine(1024.Bytes()); // 1 KB
Console.WriteLine(1536.Bytes()); // 1.5 KB
Console.WriteLine(1048576.Bytes()); // 1 MB
Console.WriteLine((2.5).Gigabytes()); // 2.5 GB
Console.WriteLine((10).Terabytes()); // 10 TB

// Precise byte size
Console.WriteLine(1536.Bytes().ToString("#.#")); // 1.5 KB
Console.WriteLine(1048576.Bytes().ToString("#.##")); // 1 MB

// Metric Numerals
Console.WriteLine(1000.ToMetric()); // 1k
Console.WriteLine(1000000.ToMetric()); // 1M
Console.WriteLine(1000000000.ToMetric()); // 1G
Console.WriteLine(1234567.ToMetric(decimals: 2)); // 1.23M

// Enum Humanization
Console.WriteLine(PaymentStatus.PendingPayment.Humanize()); // Pending payment
Console.WriteLine(PaymentStatus.PaymentReceived.Humanize()); // Payment received
Console.WriteLine(PaymentStatus.PaymentDeclined.Humanize()); // Payment declined

// Enum Dehumanization
var status = "Payment received".DehumanizeTo<PaymentStatus>();
Console.WriteLine(status); // PaymentReceived

public enum PaymentStatus
{
    PendingPayment,
    PaymentReceived,
    PaymentDeclined
}
```
