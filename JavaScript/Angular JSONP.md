```javascript
// Make sure you include it in DI:  app.factory("myFactory", function ($http, $sce) { 
factory.getSomething = function (callback) { 
   const URL = "http://something.../?param=someval&param2=otherval"; 
   var url = $sce.trustAsResourceUrl(URL); 
   $http.jsonp(url, { 
       jsonpCallbackParam: "callback" 
   }).then(function (response) { 
       console.log("Successful response: " + response); 
   }, function (response) { 
       console.log("Error response: " + response); 
   }); 
}; 
```