```javascript
"use strict";
 
// no IE support
// no Safari support
 
function doSomething(a = 1, b = 2) {
    console.log(`a=${a}`);
    console.log(`b=${b}`);
}
 
doSomething(); // 1, 2
doSomething(10); // 10, 2
doSomething(undefined, 20); // 1, 20
doSomething(10, 20); // 10, 20
doSomething(null, null); // null, null
 
// default value can be a function call, like an argument exception:
function go(val = missingValue("val")) {
 
}
 
function missingValue(paramName) {
    throw new Error(`Parameter ${paramName} is missing`);
}
 
try { go(34); console.log("ok"); } catch (ex) { console.log(ex); } // ok
try { go(); console.log("ok"); } catch (ex) { console.log(ex); }
```