Node.js has introduced **native support for TypeScript** starting from version 22.7.0 so I don't think this is necessary anymore.

```bash
npm init --yes
npm install typescript -g
tsc --init
npm install is-odd --save
npm install @types/is-odd --save-dev
npm install @types/node --save-dev
mkdir src
mkdir js
```

Configure package and tsconfig, and add code to project

```bash
npm start
```

## tsconfig.json
```json
    "target": "ES2020",                          
    "module": "commonjs",                    
     "outDir": "./js",                        
    "strict": true,                           
    "esModuleInterop": true,       
    "skipLibCheck": true,
    "forceConsistentCasingInFileNames": true  
```

## src/index.ts

```typescript
import Car from "./car";
import isOdd from "is-odd";

(() => {
    const car = new Car("Mustang");
    car.drive();

    console.log(`Is 3 odd? ${isOdd(3)}`);
})();

src/car.ts

export default class Car {
    constructor(private model: string) {}
    drive() {console.log(`${this.model} goes vroom`);}
}
```

## package.json
```json
{
  "name": "code",
  "version": "1.0.0",
  "description": "",
  "main": "index.js",
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1",
    "start": "tsc && node js/index"
  },
  "keywords": [],
  "author": "",
  "license": "ISC",
  "dependencies": {
    "is-odd": "^3.0.1"
  },
  "devDependencies": {
    "@types/is-odd": "^3.0.0"
  }
}
```