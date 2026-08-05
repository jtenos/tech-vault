```javascript
javascript:navigator.clipboard.writeText(crypto.randomUUID().replace(/-/g,'').toLowerCase()) 

/*
Generates a GUID no dashes in lower case and copies to the clipboard 
The document must have focus for this to work 
*/
```