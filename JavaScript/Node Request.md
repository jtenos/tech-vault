```javascript
"use strict";

const fs = require("fs");
const request = require("request");

const funcs = [];

(() => {
	request("http://localhost:14519/Test/GetHtml", (err, resp, body) => {
		console.log("-----");
		console.log("err: " + err);
		console.log("statusCode: " + resp.statusCode);
		console.log("body: " + body);
		console.log("-----");
	});
})();

(() => {
	request("http://localhost:14519/Test/GetJson").pipe(fs.createWriteStream("result.json"));
})();

(() => {
	request.post({
		url: "http://localhost:14519/Test/PostParams", 
		form: { name: "John Smith" }, 
	}, (err, resp, body) => {
		console.log("----");
		console.log("err: " + err);
		console.log("statusCode: " + resp.statusCode);
		console.log("body: " + body);
		console.log("-----");
	});
})();

(() => {
	const options = {
		method: "POST",
		url: "http://localhost:14519/Test/PostJson",
		json: { Name: "John Smith", Age: 34 }
	};
	const callback = (err, resp, body) => {
		console.log("-----");
		console.log(body);
		console.log("-----");
	};
	request(options, callback);
})();

(() => {
	request.post({
		url: "http://localhost:14519/Test/PostJsonGetJson",
		json: { Name: "Jack Doe", Age: 19 }
	}, (err, resp, body) => {
		console.log("-----");
		console.log("NAME: " + body.Name);
		console.log("AGE: " + body.Age);
		console.log("Content Type: " + resp.headers["content-type"]);
		console.log("-----");
	});
})();

```