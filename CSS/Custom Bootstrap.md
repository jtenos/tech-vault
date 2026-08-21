## package.json
```json
{
  "name": "webapp",
  "version": "1.0.0",
  "description": "",
  "main": "index.js",
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1",
    "compile": "sass --no-source-map src/styles/site.scss webroot/styles/site.css",
    "start": "npm run compile && serve webroot"
  },
  "keywords": [],
  "author": "",
  "license": "ISC",
  "devDependencies": {
    "bootstrap": "^4.6.0",
    "sass": "^1.32.5",
    "serve": "^11.3.2"
  }
}
```

## \_colors.scss
```scss
$theme-colors: (
    "primary": #929
);
```

## site.scss
```scss
@import "colors";
@import "../../node_modules/bootstrap/scss/bootstrap.scss";
```

## index.html