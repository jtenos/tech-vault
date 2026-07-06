```csharp
Person[] persons = [
    new(1, "Diana"),
    new(2, "Clark"),
    new(3, "Bruce")
];

Car[] cars = [
    new(1, 1, "Ford", "Mustang"),
    new(2, 1, "Chevrolet", "Camaro"),
    new(3, 3, "Ford", "Fusion"),
    new(4, 3, "Chevrolet", "Malibu")
];

var query = (
    from p in persons
    from c in cars.Where(c => c.PersonID == p.ID).DefaultIfEmpty()
    select new
    {
        p.ID,
        p.Name,
        CarMake = c?.Make,
        CarModel = c?.Model
    }
).ToList();

foreach (var item in query)
{
    Console.WriteLine($"{item.ID} {item.Name} {item.CarMake} {item.CarModel}");
}

record class Person(int ID, string Name);
record class Car(int ID, int PersonID, string Make, string Model);
```