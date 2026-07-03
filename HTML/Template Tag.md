```html
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Template Tag</title>
  </head>
  <body>

	<table id="my-table">
		<thead>
			<tr>
				<th>ID</th>
				<th>Name</th>
			</tr>
		</thead>
		<tbody></tbody>
	</table>

	<template id="my-row-template">
		<tr>
			<td data-id></td>
			<td data-name></td>
		</tr>
	</template>

	<script>
		document.addEventListener("DOMContentLoaded", () => {
			const tbody = document.querySelector("#my-table tbody")
			const template = document.querySelector("#my-row-template")
			const data = [
				{id: 1, name: "John Doe"},
				{id: 2, name: "Jane Smith"},
				{id: 3, name: "Pat Jones"}
			]
			data.forEach(item => {
				const row = template.content.cloneNode(true)
				row.querySelector("[data-id]").textContent = item.id
				row.querySelector("[data-name]").textContent = item.name
				tbody.appendChild(row)
			})
		})
	</script>
  </body>
</html>
```