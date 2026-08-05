```javascript
// https://css-tricks.com/you-can-label-a-javascript-if-statement/

// Our outer if statement
outerIf:
if (true) {
  // Our inner if statement
  innerIf:
  if (true) {
    break outerIf; // Immediately skips to the end of the outer if statement
  }
  console.log('This never logs!');
}


let x = 0;
outerLoop:
while (x < 10) {
  x++;
  for (let y = 0; y < x; y++) {
    // This will jump back to the top of outerLoop
    if (y === 5) continue outerLoop;
    console.log(x,y);
  }
  console.log('----'); // This will only happen if x < 6
}

// This example throws a syntax error in an ES module
const myElement = document.createElement('p');
myConditionalBlock: {
  const myHash = window.location.hash;
  // escape the block if there is not a hash.
  if (!myHash) break myConditionalBlock;
  myElement.innerText = myHash;
}
console.log(myHash); // undefined
document.body.appendChild(myElement);
```