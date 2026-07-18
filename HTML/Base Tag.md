```html
<!DOCTYPE html>
<html>
<head>
   <!-- Put this in the layout/master page, and all app URLs can be relative to the app root -->
   <base href="/temp/stuff/">
   <!-- /temp/stuff/abc.css -->
   <link href="abc.css" rel="stylesheet">
   <!-- /temp/stuff/[abc.js](http://abc.js/) -->
   <script src="[abc.js](http://abc.js/)"></script>
</head>
<body>
   <!-- Clicking takes you to /temp/stuff/hello.html -->
   <a href="hello.html">Hello</a>
   <!-- /temp/stuff/hello.png -->
   <img src="hello.png">
   <!-- Absolute - base tag is not used -->
   <a href="/1.html">One</a>
</body>
</html>
```