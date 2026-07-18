```javascript
// definition
// func.bind(thisArg[, arg1[, arg2[, ...]]])

const Line = function() {
    this._spaces = [];
    for (let i = 0; i < 10; ++i) { this._spaces[i] = "."; }
 
    this.turnOn = function(idx) { this._spaces[idx] = "X"; };
    this.turnOff = function(idx) { this._spaces[idx] = "."; };
    this.toString = function() { return this._spaces.join(""); }
};
 
const line = new Line();
 
const turnOnTwo = function (idx1, idx2) {
    this.turnOn(idx1);
    this.turnOn(idx2);
};
 
console.log(line.toString()); // ..........

const boundFunction = turnOnTwo.bind(line);
boundFunction(1, 2);
console.log(line.toString()); // .XX.......
```