```javascript
// Returns a sub-array, leaving the original array as-is. Use negative begin value as an offset from the last element. 
arr.slice([begin[, end]])

// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/slice

var arr = [1, 2, 3, 4, 5]; 
var result = arr.slice(2, 4); 
console.log(arr); // [1, 2, 3, 4, 5] 
console.log(result); // [3, 4] 
```