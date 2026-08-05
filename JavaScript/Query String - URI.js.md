```javascript
var parsedURI = URI.parse(document.location.href); // { query: "a=1&b2=", protocol: "http", hostname: "here.com", ... }
var query = parsedURI.query; // a=1&b=2
var parsedQuery = URI.parseQuery(query); // { a: "1", b: "2" }
var a = parsedQuery.a; // "1"
console.log(a);

a = URI.parseQuery(URI.parse(document.location.href).query).a;
console.log(a);
```