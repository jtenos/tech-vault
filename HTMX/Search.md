```html
<input
  type="search"
  name="search"
  placeholder="search books by title..."
  hx-post="/books/search"
  hx-trigger="keyup changed delay:300ms"
/>
```