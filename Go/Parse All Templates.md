## main.go
```go
package main

import (
	_ "embed"
	"flag"
	"os"
)

func main() {

	var preParse bool
	flag.BoolVar(&preParse, "pre-parse-templates", false, "Pre-parse the templates on application startup")
	flag.Parse()

	if preParse {
		PreParseTemplates()
	}

	t1 := GetTemplate(Template1)
	data := struct{ Name string }{"World"}
	t1.Execute(os.Stdout, data)

	t2 := GetTemplate(Template2)
	t2.Execute(os.Stdout, data)
	t2 = GetTemplate(Template2)
	t2.Execute(os.Stdout, data)
}
```

## templates.go

```go
package main

// Probably move this into its own package in a bigger application

import (
	"embed"
	"log"
	"text/template"
)

//go:embed templates/*.txt
var templateFs embed.FS

type TemplateName string

const (
	Template1 TemplateName = "templates/template1.txt"
	Template2 TemplateName = "templates/template2.txt"
)

var templates = map[TemplateName]*template.Template{}

// Useful for development/testing so you know right away if there's a problem parsing
// Maybe not useful in production because you know that they will parse successfully when needed
func PreParseTemplates() {
	log.Println("Preparsing templates")
	GetTemplate(Template1)
	GetTemplate(Template2)
}

func GetTemplate(tn TemplateName) *template.Template {
	t, ok := templates[tn]
	if !ok {
		log.Printf("Parsing template %s\n", tn)
		t = template.Must(template.ParseFS(templateFs, string(tn)))
		templates[tn] = t
	}
	return t
}
```