```javascript
var _jq183;
 
function load183() {
    if (typeof window.jQuery === "function" && window.jQuery.fn && window.jQuery.fn.jquery === "1.8.3") {
        _jq183 = window.jQuery;
        return;
    }
 
    if (typeof window.jQuery === 'undefined' || !window.jQuery.fn || (window.jQuery.fn.jquery !== "1.8.3")) {
 
        var done = false;
 
        var body = document.getElementsByTagName("body")[0];
        var scr = document.createElement("script");
        scr.setAttribute("src", "https://code.jquery.com/jquery-1.8.3.min.js");
        scr.onload = scr.onreadystatechange = function () {
            if (!done && (!this.readyState || this.readyState === "loaded" || this.readyState === "complete")) {
                done = true;
                _jq183 = jQuery.noConflict(true);
                scr.onload = scr.onreadystatechange = null;
                body.removeChild(scr);
            };
        };
        body.appendChild(scr);
    };
}
 
function showVersion() {
    if (typeof $ === "undefined") {
        console.log("$ is undefined");
    } else {
        console.log("$: " + $.fn.jquery);
    }
    if (typeof jQuery === "undefined") {
        console.log("jQuery is undefined");
    } else {
        console.log("jQuery: " + jQuery.fn.jquery);
    }
    console.log("_jq183: " + (_jq183 ? _jq183.fn.jquery : "N/A"));
}

/*
You may get an error message, like:

Refused to load the script 'https://code.jquery.com/jquery-1.8.3.min.js'
because it violates the following Content Security Policy directive: "script-src somethingsomething.com".

From what I’ve read, if you get this, you’re out of luck because the site won’t allow you to inject a script from another domain.
*/
```