To install moment:

```bash
npm install moment
```

To run:

```bash
deno run index.ts
```

## first.ts
```typescript
export default class Person {
    id: number;
    name: string;

    constructor(id: number, name: string) {
        this.id = id;
        this.name = name;
    }

    hello(): void {
        console.log(`Hello ${this.name}, you are person ID ${this.id}`);
    }
}
```

## second.ts
```typescript
export class Car {
    make: string;
    model: string;
    constructor(make: string, model: string) { this.make = make; this.model = model; }
    go(): void {
        console.log(`${this.make} ${this.model} goes vroom`);
    }
}

export class Shoe {
    size: number;
    style: string;
    constructor(size: number, style: string) { this.size = size; this.style = style; }
    run(): void {
        console.log(`My size ${this.size} ${this.style} shoe is running`);
    }
}

export function kickCar(shoe: Shoe, car: Car): void {
    console.log(`A size ${shoe.size} ${shoe.style} shoe just kicked the ${car.make} ${car.model}`);
}
```

## third.ts
```typescript
export class Tablet {
    operatingSystem: string;
    size: number;
    constructor(operatingSystem: string, size: number) {
        this.operatingSystem = operatingSystem;
        this.size = size;
    }
}

export function showTablet(tablet: Tablet) {
    console.log(`This is a ${tablet.size} inch tablet with ${tablet.operatingSystem}`);
}
```

## fourth.ts
```typescript
export default class Account {
  id: number;
  balance: number;
  constructor(id: number, balance: number) {
    this.id = id;
    this.balance = balance;
  }
}

export class Customer {
    name: string;
    constructor(name: string) { this.name = name; }
}
```

## index.ts
```typescript
import Person from "./first.ts";
import * as second from "./second.ts";
import { Tablet, showTablet } from "./third.ts";
import Account, { Customer } from "./fourth.ts";
import face from "https://deno.land/x/ascii_faces@v1.0.0/mod.ts";
import moment from "./node_modules/moment/dist/moment.js";

const p = new Person(34, "Mookie");
p.hello();

const c = new second.Car("Chevy", "Malibu");
c.go();

const s = new second.Shoe(15, "Running");
s.run();

second.kickCar(s, c);

const t = new Tablet("iOS", 9);
showTablet(t);

const a = new Account(123, 456);
const cust = new Customer("Joe");
console.log(a);
console.log(cust);

console.log(face());

console.log(moment().format("YYYYMMDD"));
```
