```javascript
/*
Removes the first element of an array and returns it. The original array is modified. This can be used to treat an array as a queue (first in, first out).
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/shift
*/

var arr = [1,2,3];
while (arr.length) {
	let val = arr.shift();
	console.log(val); // 1, then 2, then 3
}
console.log(arr); // []
```