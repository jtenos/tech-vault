```javascript
"use strict";
 
// npm install handlebars

(function() {
    const handlebars = require("handlebars");
    const source = `{{title}}
Hello {{person.name}}, I see that you are {{person.age}} years old.
You have the following cars:
{{#each cars}}
    {{year}} {{make}} {{model}} {{#if color}}(Color={{color}}){{/if}}
{{/each}}`;
 
    const template = handlebars.compile(source);
    const data = {
        title: "My Data Object",
        person: { name: "Billy", age: 34 },
        cars: [
            { year: 2015, make: "Ford", model: "Mustang", color: "Red" },
            { year: 2014, make: "Chevy", model: "Corvette" },
            { year: 2013, make: "Dodge", model: "Aries", color: "Yellow" }
        ]
    };
    const result = template(data);
    console.log(result);
})();
 
/****** OUTPUT *******
My Data Object
Hello Billy, I see that you are 34 years old.
You have the following cars:
    2015 Ford Mustang (Color=Red)
    2014 Chevy Corvette
    2013 Dodge Aries (Color=Yellow)
******* OUTPUT ******/
```