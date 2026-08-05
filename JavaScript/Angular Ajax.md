```javascript
$http({ 
   method: "POST", 
   url: "someUrl", 
   headers: { 
       "Content-Type": "application/json; charset=utf-8" 
   }, 
   data: JSON.stringify({ name: "John Doe" }) 
}).then(function(response) { 
   console.log("Successful response: " + response); 
}, function(response) { 
   console.log("Error response: " + response); 
});
```