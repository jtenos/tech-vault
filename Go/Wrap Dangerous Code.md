When you’ve got a dangerous function, like one that grabs an element from a slice by index, you can wrap it in a safe function that returns the result or an error instead. Use the defer feature to recover from a panic if it exists, and return an error instead.

```go
package main

import (
   "fmt"
)

func main() {
   arr := []int32{
       1, 2, 3, 4, 5, 6,
   }
   i, err := getFifthElementSafe(arr)
   if err != nil {
       fmt.Printf("Fail! %v\n", err)
   } else {
       // This will print because there are at least 5 items
       fmt.Println(i)
   }
   arr = []int32{
       1, 2, 3,
   }
   i, err = getFifthElementSafe(arr)
   if err != nil {
       // This will print because there are not at least 5 items
       fmt.Printf("Fail! %v\n", err)
   } else {
       fmt.Println(i)
   }
}

func getFifthElementSafe(arr []int32) (i int32, err error) {
   defer func() {
       if e := recover(); e != nil {
           // If the call to getFifthElement failed, then
           // this recovery will set err before returning
           err = fmt.Errorf("%v", e)
       }
   }()
   i = getFifthElement(arr)
   return
}

// A dangerous function which can panic
func getFifthElement(arr []int32) int32 {
   return arr[4]
}
```