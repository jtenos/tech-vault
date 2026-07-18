```javascript
var api_key = 'MY_API_KEY';
var domain = 'mg.MYDOMAIN.com';
var mailgun = require('mailgun-js')({apiKey: api_key, domain: domain});
  
var data = {
  from: 'Some User <SOMEUSER@MYDOMAIN.com>',
  to: 'person1@example.com,person2@example.com',
  subject: 'Hello',
  text: 'This is a test!',
  html: 'This is a <b>test</b>!'
};
  
mailgun.messages().send(data, function (error, body) {
  console.log(body);
});
```