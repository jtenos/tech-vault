```javascript
"use strict";

function format(message, ...args) {
    for (let i = 0; i < args.length; ++i) {
        message = message.split(`{${i}}`).join(args[i]);
    }
    return message;
}

const msg = format("Hello {0} my name is {1}. I have {2} cats and {2} dogs.", "Mary", "Todd", 9);
console.log(msg);

function pickOne(...vals) {
    return vals[Math.floor(Math.random() * vals.length)];
}

const arr1 = ["a", "b", "c"];
const arr2 = ["g", "h", "i"];
const arr = [ ...arr1, "d", "e", "f", ...arr2 ];

console.log(pickOne(...arr));

function go(...vals) {
    console.log(vals); // [ 1, 2, 3 ]
    console.log(arguments); // { "0": 1, "1": 2, "2": 3 }
    console.log(Array.isArray(vals)); // true
    console.log(Array.isArray(arguments)); // false
    console.log(vals.forEach); // [Function: forEach]
    console.log(arguments.forEach); // undefined
}
go(1, 2, 3);
```