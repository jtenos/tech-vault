```csharp
[Test]
public void EnsureAllDTOClassesHaveDefaultConstructor()
{
    foreach (var type in typeof(SomeDTO).Assembly.GetTypes())
    {
        if (!type.IsAbstract
            && type.GetConstructor(new Type[0]) == null)
        {
            Assert.Fail("Type {0} is missing the default constructor.",        
                type.FullName);
        }
    }
}
```