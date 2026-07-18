```javascript

(function() {

	window.indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB;
	window.IDBTransaction = window.IDBTransaction || window.webkitIDBTransaction || window.msIDBTransaction || {READ_WRITE: "readwrite"};
	window.IDBKeyRange = window.IDBKeyRange || window.webkitIDBKeyRange || window.msIDBKeyRange;
	
	if (!window.indexedDB) {
		console.log("Your browser doesn't support a stable version of IndexedDB. Such and such feature will not be available.");
	}
	
	var db;
	var VERSION_NUMBER = 1;
	var request = indexedDB.open("databaseName", VERSION_NUMBER);
	request.onerror = function(event) {
	  // Do something with request.errorCode!
	};
	request.onupgradeneeded = function(event) { 
	  db = event.target.result;
	
	  var objectStore = db.createObjectStore("persons", { keyPath: "personID" });
	  objectStore.createIndex("firstName", "firstName", { unique: false });
	  objectStore.createIndex("lastName", "lastName", { unique: false });
	
	  var objectStore2 = db.createObjectStore("cars", { keyPath: "carID" });
	  objectStore2.createIndex("make", "make", { unique: false });
	};
	request.onsuccess = function(event) {
	  db = db || event.target.result;
	  db.onerror = function(event) {
		console.error("Database error: " + event.target.errorCode);
	  };
	
	  (function() {
		var transaction = db.transaction(["persons", "cars"], "readwrite");
		transaction.onsuccess = function(event) { console.log("Successfully added persons"); };
		transaction.onerror = function(event) { console.error("Error adding persons"); };
	 
		var personObjectStore = transaction.objectStore("persons");
		personObjectStore.add({ personID: 1, firstName: "John", lastName: "Doe", age: 34 });
		personObjectStore.add({ personID: 2, firstName: "Mary", lastName: "Smith", age: 29 });
	  })();
	
	  (function() {
		var transaction = db.transaction(["persons"], "readonly");
		var personObjectStore = transaction.objectStore("persons");
		var request = personObjectStore.get(1);
		request.onsuccess = function(event) { console.log("Person: " + JSON.stringify(request.result)); };
		request.onerror = function(event) { console.error("Error retrieving Person with personID=1"); };
	  })();
	
	  (function() {
		var transaction = db.transaction(["cars"], "readwrite");
		var carObjectStore = transaction.objectStore("cars");
		var request = carObjectStore.put({ carID: 100, make: "ford", model: "mustang" });
		request.onsuccess = function(event) { console.log("Mustang added"); };
		request.onerror = function(event) { console.error("Error adding Mustang"); };
	  })();
	
	  (function() {
		var transaction = db.transaction(["persons"], "readonly");
		var personObjectStore = transaction.objectStore("persons");
		var cursor = personObjectStore.openCursor();
		cursor.onsuccess = function(event) {
		  var cursor = event.target.result;
		  if (cursor) {
			console.log("Person: " + JSON.stringify(cursor.value));
			cursor.continue();
		  } else {
			console.log("Done retrieving persons");
		  }
		};
		cursor.onerror = function(event) { console.error("Error retrieving persons"); };
	  })();
	};
	
	
	
	})();

/*
https://developer.mozilla.org/en-US/docs/Web/API/IndexedDB_API/Using_IndexedDB
https://stackoverflow.com/a/55195418/111266
*/
```