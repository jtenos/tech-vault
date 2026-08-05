```javascript
/*

Here's a quick piece of javascript to force a random query string parameter to appear on a page.
This should be on a GET, since it redirects you, and if that happened on a POST, bad things would happen.
This can be used to avoid caching, when for whatever reason the HTTP headers/cookies/etc. aren't working.
First, load up URI.js and Moment.js (not necessary, but I always use these two):

npm i urijs --save
npm install moment --save

Assuming you have a single main entry point for your javascript, add this - the return will ensure 
that nothing else runs. If you don't have a single entry point, some other scripts may still have time 
to run, so be prepared for that.
*/

var _ = URI.parseQuery(URI.parse(document.location.href).query)._;
var expectedValue = btoa(moment().format("YYYYMMDDHHmm"));
if (_ !== expectedValue) {
        document.location.href = URI(document.location.href)
            .removeQuery("_").removeQuery("__")
            .addQuery("_", expectedValue).addQuery("__", Math.random());
    return;
}

/*
There are two pieces: it checks for a query string parameter named as the underscore character 
(name it whatever you want). It expects it to be the base-64 encoded date/time, to the minute. 
If that doesn't match, then it redirects you to the same page, with the parameter changed to the
current minute, so this will be a match the next time the page loads.
The timestamp is there so that if the user bookmarks the site with a query string, the next time 
they open it, they'll still get redirected.
The random second parameter is just there to add an element of randomness, so no two people will get
the same page. Not really necessary, but it's up to you.
*/
```