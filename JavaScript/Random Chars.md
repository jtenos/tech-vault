```javascript
const crypto = require('crypto');

function getRandomChars(len) {
	const CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
	const NUM_CHARS = CHARS.length
	let result = ""
	for (let i = 0; i < len; i++) {
		let idx = crypto.randomInt(NUM_CHARS)
		result += CHARS[idx]
	}
	return result
}
```