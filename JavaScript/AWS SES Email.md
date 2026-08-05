```javascript
"use strict";

var ses = require('node-ses')
    , client = ses.createClient({
        key: 'my key',
        secret: 'my secret',
        amazon: "https://email.my-region.amazonaws.com"
    });

// Give SES the details and let it construct the message for you. 
client.sendEmail({
    to: 'recipient@example.com'
    , from: "sender@example.com"
    , cc: 'cc@example.com'
	, bcc: ['bcc1@example.com', 'bcc2@example.com']
    , subject: 'Test Message ' + new Date()
    , message: 'your <b>message</b> goes here'
    , altText: 'plain text'
}, function (err, data, res) {
    if (err) {
        console.log("ERROR");
        console.log(err);
    } else {
        console.log("Done");
    }
});
```