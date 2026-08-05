```javascript
import { BufReader } from "https://deno.land/std/io/bufio.ts";

const file = await Deno.open("data.txt");
const bufReader = new BufReader(file);
let line: string | undefined;

const cities = new Set();
while (line = (await bufReader.readString("\n"))?.replace(/\r$/, "")) {
    const fields = line.split("\t");
    const city = fields[4];
    cities.add(city);
}
const arr = [...cities];
arr.sort();
console.log(arr.join(", "));
```

```bash
deno run --allow-read index.ts
```