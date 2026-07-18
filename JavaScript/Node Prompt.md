```javascript
const readline = require("readline");
const rl = readline.createInterface(process.stdin, process.stdout);
 
rl.on("line", line => {
   console.log(`You typed ${line}`);
   rl.prompt();
});
 
rl.setPrompt("Action> ");
rl.prompt();
```