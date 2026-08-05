```javascript
// Old simple way

function Product(id, name, price) {
    this.id = id;
    this.name = name;
    this.price = price;
}

const product = new Product(1, "Oreos", 2.49);
console.log(product.id);
console.log(product.name);
console.log(product.price);
console.log("Applying changes...");
product.id = 2;
product.name = "Oreo Cookies";
product.price = 3.49;
console.log(product.id);
console.log(product.name);
console.log(product.price);

// Methods

const formatter = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' });

class Product {
    constructor(id, name, price) {
        this.setID(id);
        this.setName(name);
        this.setPrice(price);
    }

    getID() { return this._id; }
    setID(id) { this._id = id * 100; }
    getName() { return this._name.toUpperCase(); }
    setName(name) { this._name = name; }
    getPrice() { return formatter.format(this._price); }
    setPrice(price) { this._price = price; }
}

const product = new Product(1, "Oreos", 2.49);
console.log(product.getID());
console.log(product.getName());
console.log(product.getPrice());
console.log("Applying changes...");
product.setID(2);
product.setName("Oreo Cookies");
product.setPrice(3.49);
console.log(product.getID());
console.log(product.getName());
console.log(product.getPrice());

// Methods as functions in constructor

const formatter = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' });

function Product(id, name, price) {
    this.getID = function() { return this._id; }
    this.setID = function(id) { this._id = id * 100; }
    this.getName = function() { return this._name.toUpperCase(); }
    this.setName = function(name) { this._name = name; }
    this.getPrice = function() { return formatter.format(this._price); }
    this.setPrice = function(price) { this._price = price; }

    this.setID(id);
    this.setName(name);
    this.setPrice(price);
}

const product = new Product(1, "Oreos", 2.49);
console.log(product.getID());
console.log(product.getName());
console.log(product.getPrice());
console.log("Applying changes...");
product.setID(2);
product.setName("Oreo Cookies");
product.setPrice(3.49);
console.log(product.getID());
console.log(product.getName());
console.log(product.getPrice());

// Methods as functions in prototype

const formatter = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' });

function Product(id, name, price) {
    this.setID(id);
    this.setName(name);
    this.setPrice(price);
}

Product.prototype.getID = function() { return this._id; }
Product.prototype.setID = function(id) { this._id = id * 100; }
Product.prototype.getName = function() { return this._name.toUpperCase(); }
Product.prototype.setName = function(name) { this._name = name; }
Product.prototype.getPrice = function() { return formatter.format(this._price); }
Product.prototype.setPrice = function(price) { this._price = price; }

const product = new Product(1, "Oreos", 2.49);
console.log(product.getID());
console.log(product.getName());
console.log(product.getPrice());
console.log("Applying changes...");
product.setID(2);
product.setName("Oreo Cookies");
product.setPrice(3.49);
console.log(product.getID());
console.log(product.getName());
console.log(product.getPrice());

// Properties

const formatter = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' });

class Product {
    constructor(id, name, price) {
        this.id = id;
        this.name = name;
        this.price = price;
        this._timesNameChanged = 0;
    }

    get id() { return this._id; }
    set id(value) { this._id = value * 100; }

    get name() { return this._name.toUpperCase(); }
    set name(value) { this._name = value; ++this._timesNameChanged; }
    
    get price() { return formatter.format(this._price); }
    set price(value) { this._price = value; }

    get timesNameChanged() { return this._timesNameChanged; }
}

const product = new Product(1, "Oreos", 2.49);
console.log(`ID: ${product.id}`);
console.log(product.name);
console.log(product.price);
console.log(`Name changed ${product.timesNameChanged} times`);
console.log("Applying changes...");
product.id = 2;
product.name = "Oreo Cookies";
product.price = 3.49;
console.log(`ID: ${product.id}`);
console.log(product.name);
console.log(product.price);
console.log(`Name changed ${product.timesNameChanged} times`);

// Notes

// It’s possible to cheat and have the setter not actually set what you think it is. Don’t abuse your code by doing something like this: 

class Foo {
    constructor(id) { this._id = id || 0; }
    get ID() { return this._id; }
    set ID(value) { this._id = value * 100; } // Don't do this
}
```