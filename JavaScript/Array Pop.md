
Removes the last element of an array and returns it. The original array is modified.
This can be used to treat an array as a stack (last in, first out).

https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/pop

```javascript
/*
arr.pop()
*/

var arr = [];
arr.push(1);
arr.push(2);
arr.push(3);
while (arr.length) {
    let x = arr.pop();
    console.log(x); // 3, then 2, then 1
}
console.log(arr); // []
```