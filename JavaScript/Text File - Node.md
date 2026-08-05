```javascript
// NodeJS files

const fs = require("fs");
const readline = require("readline");

// Read full file
fs.readFile("myfile.txt", { encoding: "utf-8" } ,(err, data) => {
    console.log("Full file contents:");
    console.log(data);
    console.log("===================");
});

// Read file line-by-line
var lineReader = readline.createInterface({
    input: fs.createReadStream("myfile.txt")    
});
lineReader.on("line", line => { console.log(`Line: ${line}`); });
lineReader.on("close", () => console.log("DONE"));

// Write full file
fs.writeFile("myfile.txt", "some file contents", err => {
    if (err) console.error(err);
});

// Write file line-by-line
var outputStream = fs.createWriteStream("myfile.txt");
outputStream.write("Hello 1\n");
outputStream.write("Hello 2\n");
outputStream.end();

// Read/write binary data
var inputStream = fs.createReadStream("input.bin");
var outputStream = fs.createWriteStream("output.bin");
inputStream.on("data", data => {
    outputStream.write(data);
});
inputStream.on("end", () => {
    outputStream.end();
});
```