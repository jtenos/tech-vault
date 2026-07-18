```javascript
_relTime = new Intl.RelativeTimeFormat("en")
console.log(_relTime.format(-2, "day")) // 2 days ago
console.log(_relTime.format(2, "hour")) // in 2 hours
console.log(_relTime.format(24, "year")) // in 24 years
console.log(_relTime.format(0, "month")) // in 0 months
```