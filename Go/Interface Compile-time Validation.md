```go
type Parser interface {
  Parse(content string) ([]string, error)
}

type CSVParser struct {}

// This line doesn't cause any actual work, but it will cause a compile error if CSVParser doesn't meet the interface definition. Useful if this type is part of a library that we're exposing elsewhere but not necessarily instantiating this type directly
var _ Parser = (*CSVParser)(nil)

func (c *CSVParser) Parse(content string) ([]string, error) {
  return []string{}, nil
}
```