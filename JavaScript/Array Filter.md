```javascript
class Car {
    year;
    make;
    model;
    constructor(year, make, model) {
        this.year = year;
        this.make = make;
        this.model = model;
    }
}

// Normal use: filter the items

const cars = [
    new Car(1999, "Ford", "Bronco"),
    new Car(2020, "Chevy", "Blazer"),
    new Car(2015, "Toyota", "Prius"),
    new Car(2000, "Dodge", "Dakota"),
    new Car(2012, "Ford", "Mustang")
];

const newerCars = cars.filter(c => c.year > 2010);
console.log("Newer cars:")
console.log(newerCars);
console.log("");

// Second parameter: Index of the array
// [0, 1, 3, 6, 10, 15, 21, ...]

const iterations = [];
let runningTotal = 0;
for (let i = 0; i < 1000; ++i) {
    runningTotal += i;
    iterations.push(runningTotal);
}

let every10thItem = iterations.filter((val, idx) => idx % 10 === 0);
console.log("Every 10th item:");
console.log(every10thItem);
console.log("");

// Third parameter: The array itself
// You can even modify the array here

// Modify values - the change happens before the iterator gets to it.
// Modifying the next record, except for the last one
let arr = [1, 2, 3];
let newArr = arr.filter((item, idx, a) => {
    if (idx != a.length - 1) {
        a[idx + 1] = 10 * a[idx + 1];
    }
    if (idx != 0) {
        a[idx - 1] = -1 * a[idx - 1];
    }
    return true;
});
console.log(newArr); // 1, 20, 30
console.log(arr); // -1, -20, 30
console.log("");

// Add values - the new item is not included in the iterator:
// Each of the three items creates a "Hello" at the end, but "Hello" is not
// executed in the filter
arr = [1, 2, 3];
newArr = arr.filter((item, idx, a) => {
    a.push("Hello");
    return true;
});
console.log(newArr); // 1, 2, 3
console.log(arr); // 1, 2, 3, "Hello", "Hello", "Hello"
console.log("");

// Remove values - the removed item is not included in the iterator:
// Removing the last item in the array
arr = [1, 2, 3];
newArr = arr.filter((item, idx, a) => {
    a.pop();
    return true;
});

console.log(newArr); // 1, 2
console.log(arr); // 1
console.log("");
```