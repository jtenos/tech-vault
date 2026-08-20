```go
package main

import (
    "flag"
    "fmt"
)

func main() {
    // Option 1: Define the variables in advance
    var id int64
    flag.Int64Var(&id, "id", 34, "The ID that you're looking for")
    // Option 2: Write to pointers to the variables
    name := flag.String("name", "default", "The name that you want")
    flag.Parse()
    fmt.Printf("id=%v\n", id)
    fmt.Printf("name=%s\n", *name)
}
```