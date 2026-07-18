```javascript
const crypto = require("crypto");
 
class Guid {
 
    constructor(hex) {
        if (typeof hex != "string") {
            throw "Must be a string";
        }
         
        var val = hex.replace(/[^0-9a-fA-F]/g, "").toLowerCase();
        if (val.length != 32) {
            throw "Must be 32 hex characters";
        }
 
        this._hex = val;
    }
 
    static newGuid() {
 
        var bytes = crypto.randomBytes(32);
 
        for (let i = 0; i < 32; ++i) {
            bytes[i] = (bytes[i] / 16) | 0;
        }
 
        let i = 0;
        let hex = "xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx".replace(/[xy]/g, function (c) {
            var r = bytes[i++], v = c == "x" ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
        return new Guid(hex);
    }
 
    toString(format) {
        format = (format || "D");
        let sub = (start, end) => this._hex.substring(start, end);
        switch (format) {
            case "N": return this._hex.toUpperCase();
            case "n": return this._hex;
            case "D": return (`${sub(0, 8)}-${sub(8, 12)}-${sub(12, 16)}-${sub(16, 20)}-${sub(20, 32)}`).toUpperCase();
            case "d": return (`${sub(0, 8)}-${sub(8, 12)}-${sub(12, 16)}-${sub(16, 20)}-${sub(20, 32)}`);
        }
    }
}
 
module.exports = Guid;
```