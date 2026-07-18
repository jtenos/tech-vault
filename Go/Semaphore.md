```go
package main
import (
   "fmt"
   "sync"
   "time"
)
type semaphore chan bool
func (s semaphore) acquire() {
   s <- true
}
func (s semaphore) release() {
   <-s
}
func main() {
   s := make(semaphore, 10) // Only 10 goroutines will be active at a time
   wg := new(sync.WaitGroup)
   wg.Add(100) // There will be 100 executions before exiting
   for i := 0; i < 100; i++ {
       s.acquire()
       go func(i int, s semaphore, wg *sync.WaitGroup) {
           defer wg.Done()
           defer s.release()
           doWork(i)
       }(i, s, wg)
   }
   wg.Wait()
}
func doWork(i int) {
   fmt.Printf("i=%d\n", i)
   time.Sleep(500 * time.Millisecond)
}
```