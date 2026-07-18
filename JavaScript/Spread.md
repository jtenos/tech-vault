```javascript
// Putting the contents of one array into another array
const arr1 = [1, 2, 3];
const arr2 = [4, 5, 6, 7];
const arr3 = [...arr1, ...arr2]; // [1, 2, 3, 4, 5, 6, 7]
const arr4 = ["a", ...arr1, "b"]; // [ "a", 1, 2, 3, "b" ]

// Clone (shallow copy)

// Clone array: primitives - new array, equal values
const origArray = [1, 2, 3];
const cloneArray = [...origArray]; // [1, 2, 3]
console.log(origArray === cloneArray); // false

// Clone array: objects - new array, same objects
const origObjArray = [{a:1},{b:2}];
const cloneObjArray = [...origObjArray];
console.log(origObjArray === cloneObjArray); // false
console.log(origObjArray[0] === cloneObjArray[0]); // true
console.log(origObjArray[1] === cloneObjArray[1]); // true

// Passing an array into function parameters
function power(base, exponent) {
    return Math.pow(base, exponent);
}
const powerArray = [2, 3];
const twoCubed = power(...powerArray); // 8

// Too many values:
function greet(first, last) {
    console.log(`Hello ${first} ${last}`);
    console.log(arguments);
}
const greetArray = ["Cal", "Ripken", "Jr"];
greet(...greetArray); // Hello Cal Ripken
                      // arguments: { "0": "Cal", "1": "Ripken", "2": "Jr" }


// Combining properties of an object
const firstObject = { a: 1, b: 2, c: 3 };
const secondObject = { ...firstObject, c: 4, d: 5 }; // { a: 1, b: 2, c: 4, d: 5 }
```