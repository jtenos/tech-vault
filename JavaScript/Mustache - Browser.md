```javascript
const data = {
    title: "My List (with a function)",
    items: [
        new Item(1, "milk"),
        new Item(2, "cookies"),
        new Item(3, "lettuce"),
        new Item(4, "eggs", { isImportant: true })
    ],
    showLine: false
};
    
const itemTemplate = `
    <li>
        {{#format}}
            {{id}}: {{name}}
        {{/format}}
    </li>
`;

const sectionTemplate = `
    <h3>{{title}}</h3>
    <ul>
        {{#items}}
            {{> item}}
        {{/items}}
    </ul>
    
    {{#showLine}}<hr>{{/showLine}}
    {{^showLine}}not showing line{{/showLine}}
`;
    
const html = Mustache.render(sectionTemplate, data, {
    item: itemTemplate
});

-------

const data = {
    title: "My List (using mustache files and AJAX)",
    items: [
        new Item(1, "milk"),
        new Item(2, "cookies"),
        new Item(3, "lettuce")
    ],
    showLine: false
};

let sectionTemplate;
let itemTemplate;

const populateSectionTemplate = fetch("section.mustache")
    .then(r => r.text().then(text => sectionTemplate = text));
const populateItemTemplate = fetch("item.mustache")
    .then(r => r.text().then(text => itemTemplate = text));

Promise.all([populateSectionTemplate, populateItemTemplate])
    .then(() => {
        var html = Mustache.render(sectionTemplate, data, {
            item: itemTemplate
        });
        document.getElementById("results").innerHTML = html;
    });
```