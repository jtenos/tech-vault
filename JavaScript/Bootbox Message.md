```javascript
/*
bootbox.js is a nice simple replacement for alerts. Here’s a quick extension that shows a message by hiding any existing boxes, then showing the new one. Since it’s using Bootstrap’s event-based modals, you have to wait for one to finish loading before closing it or expecting it to be open.
*/

function showMessage(title, body, callback, noButton){
    bootbox.hideAll();
    var data = { title: title, message: body };
    if (callback) {
        data.onShown = callback;
    }
    if (noButton) {
        bootbox.dialog(data);
    } else {
        bootbox.alert(data);
    }
}
 
showMessage("Loading", "Working...", function () {
    // Do something
    showMessage("Second Message", "This is the real message");
}, true);
```