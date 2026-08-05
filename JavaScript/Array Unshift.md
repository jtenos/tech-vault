```javascript
/*
Inserts one or more items to the beginning of an array. It returns the new array count.
arr.unshift(element1[, …[, elementN]])
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/unshift
*/

var arr = [1,2,3,4];
var x = arr.unshift("a", "b"); // arr = ["a","b",1,2,3,4], x=6
```