```go
package main


import "fmt"


// https://pkg.go.dev/fmt@go1.20.6


type concrete struct{ name string }


func main() {
var i int64 = 123
var s string = "Hello"
var sweird = "Hello\nI'm \"Joe\""
var st = struct{ name string }{"Joe"}
var c = concrete{"Joe"}
var b bool = false
var f float64 = 1234.56
var sl []byte = []byte{1, 2, 3}


fmt.Printf("%v\n", i)   // 123
fmt.Printf("%v\n", s)   // Hello
fmt.Printf("%v\n", st)  // {Joe}
fmt.Printf("%+v\n", st) // {name:Joe}
fmt.Printf("%v\n", c)   // {Joe}
fmt.Printf("%+v\n", c)  // {name:Joe}
fmt.Printf("%v\n", b)   // false
fmt.Printf("%v\n", f)   // 1234.56
fmt.Printf("%v\n", sl)  // [1 2 3]
fmt.Println()
fmt.Printf("%#v\n", i)  // 123
fmt.Printf("%#v\n", s)  // "Hello"
fmt.Printf("%#v\n", st) // struct { name string } {name:"Joe"}
fmt.Printf("%#v\n", c)  // main.concrete{name:"Joe"}
fmt.Printf("%#v\n", b)  // false
fmt.Printf("%#v\n", f)  // 1234.56
fmt.Printf("%#v\n", sl) // []byte{0x1, 0x2, 0x3}
fmt.Println()
fmt.Printf("%T\n", i)  // int64
fmt.Printf("%T\n", s)  // string
fmt.Printf("%T\n", st) // struct { name string }
fmt.Printf("%T\n", c)  // main.concrete
fmt.Printf("%T\n", b)  // bool
fmt.Printf("%T\n", f)  // float64
fmt.Printf("%T\n", sl) // []uint8
fmt.Println()
fmt.Printf("%b\n", i) // 1111011
fmt.Printf("%c\n", i) // {
fmt.Printf("%d\n", i) // 123
fmt.Printf("%o\n", i) // 173
fmt.Printf("%O\n", i) // 0o173
fmt.Printf("%q\n", i) // '{'
fmt.Printf("%x\n", i) // 7b
fmt.Printf("%X\n", i) // 7B
fmt.Printf("%U\n", i) // U+007B
fmt.Println()
fmt.Printf("%s\n", s)      // Hello
fmt.Printf("%q\n", s)      // "Hello"
fmt.Printf("%q\n", sweird) // "Hello\nI'm \"Joe\""
fmt.Printf("%x\n", s)      // 48656c6c6f
fmt.Printf("%X\n", s)      // 48656C6C6F
fmt.Println()
fmt.Printf("%p\n", sl) // 0xc00009e068
}


/*
General:


%v  the value in a default format
    when printing structs, the plus flag (%+v) adds field names
%#v a Go-syntax representation of the value
%T  a Go-syntax representation of the type of the value
%%  a literal percent sign; consumes no value


Boolean:
%t  the word true or false


Integer:
%b  base 2
%c  the character represented by the corresponding Unicode code point
%d  base 10
%o  base 8
%O  base 8 with 0o prefix
%q  a single-quoted character literal safely escaped with Go syntax.
%x  base 16, with lower-case letters for a-f
%X  base 16, with upper-case letters for A-F
%U  Unicode format: U+1234; same as "U+%04X"


Floating-point and complex constituents:
%b  decimalless scientific notation with exponent a power of two,
    in the manner of strconv.FormatFloat with the 'b' format,
    e.g. -123456p-78
%e  scientific notation, e.g. -1.234456e+78
%E  scientific notation, e.g. -1.234456E+78
%f  decimal point but no exponent, e.g. 123.456
%F  synonym for %f
%g  %e for large exponents, %f otherwise. Precision is discussed below.
%G  %E for large exponents, %F otherwise
%x  hexadecimal notation (with decimal power of two exponent), e.g. -0x1.23abcp+20
%X  upper-case hexadecimal notation, e.g. -0X1.23ABCP+20


String and slice of bytes (treated equivalently with these verbs):
%s  the uninterpreted bytes of the string or slice
%q  a double-quoted string safely escaped with Go syntax
%x  base 16, lower-case, two characters per byte
%X  base 16, upper-case, two characters per byte


Slice:
%p  address of 0th element in base 16 notation, with leading 0x


Pointer:
https://pkg.go.dev/fmt
%p  base 16 notation, with leading 0x
The %b, %d, %o, %x and %X verbs also work with pointers,
formatting the value exactly as if it were an integer.


The default format for %v is:
bool:
int, int8 etc.:
uint, uint8 etc.:
float32, complex64, etc: %g
string:                  %s
chan:                    %p
pointer:                 %p
%t
%d
%d, %#x if printed with %#v
*/
```