```javascript
const app = Application.currentApplication();

app.includeStandardAdditions = true;

  

const remindersApp = Application("Reminders");

remindersApp.includeStandardAdditions = true;

  

//const folder = app.chooseFolder();

  

const listNames = remindersApp.lists.name();

  

const dtFormatted = new Date().toISOString().replace(/\:/g, "_");

  

//const fileName = `${folder}/Apple\ Reminders\ ${dtFormatted}.html`;

const fileName = `/Users/jtenos/Downloads/Apple\ Reminders\ ${dtFormatted}.html`;

const file = app.openForAccess(Path(fileName), { writePermission: true });

app.setEof(file, { to: 0 });

  

function write(s) { app.write(s, { to: file }); }

  

write(`

    <html>

        <head>

            <meta charset="utf-8">

            <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

            <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">

            <title>Reminders as of ${dtFormatted}</title>

        </head>

        <body>

            <div class="container">

`);

  

for (let listName of listNames) {

    console.log(`List: ${listName}`);

    const reminders = remindersApp.lists.byName(listName).reminders;

    const names = reminders.name();

    const completeds = reminders.completed();

    const dueDates = reminders.dueDate();

    const bodies = reminders.body();

    write(`<h2>${listName}</h2>`);

    write("<ul>");

    for (let i = 0; i < names.length; ++i) {

        const completed = completeds[i];

  

        if (completed) {

            continue;

        }

        const name = names[i];

        const dueDate = dueDates[i];

        const body = bodies[i];

        write(`<li>${name}`);

        if (dueDate || body) {

            write("<ul>");

            if (dueDate) { write(`<li>${dueDate}</li>`); }

            if (body) { write(`<li>${body}</li>`); }

            write("</ul>");

        }

        write("</li>");

    }

    write("</ul>");

}

  
  

write(`

            </div>

        </body>

    </html>

`);

  

app.closeAccess(file);
```