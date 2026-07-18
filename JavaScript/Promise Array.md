```javascript
"use strict";

const request = require("request");

go((err, data) => {
    if (err) {
        console.error(`ERROR: ${err}`);
    } else {
        console.log("-----------------------------------");
        data.forEach(x => {
            console.log(`success: ${x.success}`);
            console.log(`URL: ${x.url}`);
            console.log(`bodyLength: ${x.bodyLength}`);
            console.log(`error: ${x.err}`);
            console.log("-----------------------------------");
        });
    }
    console.log("DONE");
});

function go(callback) {
    var results = [];
    
    const urls = [
        "http://www.google.com",
        "http://www.yahoo.com",
        "http://www.bing.com",
        "asdf",
        "http://www.apple.com"
    ];

    let seq = Promise.resolve();
    urls.forEach(url => {
        seq = seq.then(() => {
            console.log(`Working on ${url}`);
            return new Promise((resolve, reject) => {
                request(url, (err, resp, body) => {
                    if (err) {
                        reject(err);
                    } else {
                        resolve({ url: url, body: body });
                    }
                });
            });
        }).then(data => {
            results.push({ url: url, bodyLength: data.body.length, success: true });
        })["catch"](err => { // ["catch"] because catch is a keyword and old IE versions blow up
            results.push({ url: url, err: err, success: false });
        });
    });
    
    seq = seq.then(() => {
        callback(undefined, results);
    });
}
```