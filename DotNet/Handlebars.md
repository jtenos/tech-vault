```csharp
// <PackageReference Include="Handlebars.Net" Version="2.1.4" />

using System;
using HandlebarsDotNet;

string templateText = """

{{title}}
Hello {{person.name}}, I see that you are {{person.age}} years old.
You have the following cars:
{{#each cars}}
    {{year}} {{make}} {{model}} {{#if color}}(Color={{color}}){{/if}}
{{/each}}

""";

var template = Handlebars.Compile(templateText);
var data = new {
    title = "My Data Object",
    person = new { name = "Billy", age = 34 },
    cars = new object[] {
        new { year = 2015, make = "Ford", model = "Mustang", color = "Red" },
        new { year = 2014, make = "Chevy", model = "Corvette" },
        new { year = 2013, make = "Dodge", model = "Aries", color = "Yellow " }
    }
};
string result = template(data);
Console.WriteLine(result);

/*******
My Data Object
Hello Billy, I see that you are 34 years old.
You have the following cars:
    2015 Ford Mustang (Color=Red)
    2014 Chevy Corvette
    2013 Dodge Aries (Color=Yellow )
*******/
```