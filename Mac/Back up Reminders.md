## AppleScript
```applescript
const app = Application.currentApplication();
app.includeStandardAdditions = true;

const remindersApp = Application("Reminders");
remindersApp.includeStandardAdditions = true;

//const folder = app.chooseFolder();

const listNames = remindersApp.lists.name();

const dtFormatted = new Date().toISOString().replace(/\:/g, "_");

//const fileName = `${folder}/Apple\ Reminders\ ${dtFormatted}.html`;
const fileName = `/Users/myname/Downloads/Apple\ Reminders\ ${dtFormatted}.html`;
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
		const name = names[i];
		const completed = completeds[i];
		const dueDate = dueDates[i];
		const body = bodies[i];
		
		write(`<li>${name}`);
		if (completed || dueDate || body) {
			write("<ul>");
			if (completed) { write("<li>COMPLETED</li>"); }
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

## JavaScript
```javascript
const app = Application.currentApplication();
app.includeStandardAdditions = true;

const remindersApp = Application("Reminders");
remindersApp.includeStandardAdditions = true;

const folder = app.chooseFolder();

const listNames = remindersApp.lists.name();

const dtFormatted = new Date().toISOString().replace(/\:/g, "_");

for (let i = 0; i < listNames.length; ++i) {

	const listName = listNames[i];

	const fileName = `${folder}/Reminders-${listName}-${dtFormatted}.txt`;
	const file = app.openForAccess(Path(fileName), { writePermission: true });
	app.setEof(file, { to: 0 });

	const reminders = remindersApp.lists.byName(listName).reminders;
	const names = reminders.name();
	const completeds = reminders.completed();
	const dueDates = reminders.dueDate();
	const bodies = reminders.body();

	for (let j = 0; j < names.length; ++j) {
		app.write(`${names[j]}\n`, {to: file});
		if (completeds[j]) {
			app.write("COMPLETED\n", {to: file});
		}
		if (dueDates[j]) {
			app.write(`Due: ${dueDates[j]}\n`, {to: file});
		}
		if (bodies[i]) {
			app.write(`${bodies[j]}\n`, {to: file});
		}
		// TODO: When URL becomes available, put that here
		app.write("------------------------------------\n", {to: file});
	}
	
	app.closeAccess(file);	
	
}
// TODO: Zip these files
```

## Raw Data
```bash
#!/bin/zsh

user=$(whoami)
input="/Users/$user/Library/Reminders"
now="$(date +%Y)$(date +%m)$(date +%d)$(date +%H)$(date +%M)$(date +%S)"
output="/Users/$user/icloud-drive/ZZ_temp/bak/reminders/reminders-$now.tgz"

tar -czf $output $input
```