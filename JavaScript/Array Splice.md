Removes elements from an array, returns the removed items as an array, and optionally replaces the items. 

https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/splice 

```javascript
array.splice(start[, deleteCount[, item1[, item2[, …]]]])

var arr = [1, 2, 3, 4, 5]; 
var result = arr.splice(1, 2, "A", "B", "C"); 
console.log(arr); // [1, "A", "B", "C", 4, 5] 
console.log(result); // [2, 3] 
```