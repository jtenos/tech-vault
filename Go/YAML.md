```go
// go get gopkg.in/yaml.v2

type Book struct {
        Title  string `yaml:"title"`
        Author string `yaml:"author"`
}


type Library struct {
        ID    int    `yaml:"id"`
        Name  string `yaml:"name"`
        Books []Book `yaml:"books"`
}

func objToYaml() {
        l := Library{
                ID:   1,
                Name: "The Library",
                Books: []Book{
                        Book{Title: "First Book", Author: "First Author"},
                        Book{Title: "Second Book", Author: "Second Author"},
                },
        }


        y, err := yaml.Marshal(l)
        if err != nil {
                log.Fatal(err.Error())
        }
        fmt.Println(string(y))
}
func yamlToObj() {
        y := `id: 1
name: The Library
books:
- title: First Book
  author: First Author
- title: Second Book
  author: Second Author`


        var l Library
        err := yaml.Unmarshal([]byte(y), &l)
        if err != nil {
                log.Fatal(err.Error())
        }
        fmt.Printf("%#v\n", l)
}
```