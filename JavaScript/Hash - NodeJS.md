```javascript
// https://nodejs.org/api/crypto.html

const crypto = require("crypto");
const filename = process.argv[2];
const fs = require("fs");

const hash = crypto.createHash("sha256");

const input = fs.createReadStream(filename);
input.on("readable", () => {
    const data = input.read();
    if (data) {
        hash.update(data);
    } else {
        console.log(`${hash.digest("hex")} ${filename}`);
    }
});
```