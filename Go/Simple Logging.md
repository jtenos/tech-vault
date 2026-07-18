```go
// /pkg/logging/defaultwriter.go
package logging

import "fmt"

type defaultWriter struct{}

func (dw defaultWriter) Write(p []byte) (n int, err error) {
	fmt.Println(string(p))
	return len(p), nil
}

// /pkg/logging/logwriter.go
package logging

import (
	"fmt"
	"io"
	"time"
)

var lvl LogLevel = LevelNone
var writer io.Writer = defaultWriter{}

type LogLevel int

const (
	LevelDebug LogLevel = iota
	LevelInfo
	LevelWarn
	LevelError
	LevelNone
)

func write(lvl string, msg string) {
	// Modify this for your needs
	writer.Write([]byte(fmt.Sprintf("%s [%s] %s\n", time.Now().UTC().Format("2006-01-02T15:04:05.000"), lvl, msg)))
}

func SetLogLevel(l LogLevel) {
	lvl = l
}

func SetWriter(w io.Writer) {
	writer = w
}

func Debug(msg string) {
	if lvl > LevelDebug {
		return
	}
	write("DBG", msg)
}

func Debugf(format string, a ...any) {
	Debug(fmt.Sprintf(format, a...))
}

func Info(msg string) {
	if lvl > LevelInfo {
		return
	}
	write("INF", msg)
}

func Infof(format string, a ...any) {
	Info(fmt.Sprintf(format, a...))
}

func Warn(msg string) {
	if lvl > LevelWarn {
		return
	}
	write("WRN", msg)
}

func Warnf(format string, a ...any) {
	Warn(fmt.Sprintf(format, a...))
}

func Error(msg string) {
	if lvl > LevelError {
		return
	}
	write("ERR", msg)
}

func Errorf(format string, a ...any) {
	Error(fmt.Sprintf(format, a...))
}

///////////////////
// By default, this is LevelNone
logging.SetLogLevel(logging.LevelInfo)


// By default, it will write to the standard output, or you can
// set your own writer to any io.Writer
f, err := os.OpenFile("log.txt", os.O_APPEND|os.O_CREATE|os.O_WRONLY, 0644)
if err != nil {
	log.Fatal(err.Error())
}
defer f.Close()
logging.SetWriter(f)

logging.Debug("This is a debug message")
logging.Debugf("This %s a debug %s", "is", "msg")

logging.Info("This is an info message")
logging.Infof("This %s an info %s", "is", "msg")

logging.Warn("This is a warn message")
logging.Warnf("This %s a warn %s", "is", "msg")

logging.Error("This is an error message")
logging.Errorf("This %s an error %s", "is", "msg")

/*
Output

2022-10-11T14:46:30.073 [INF] This is an info message
2022-10-11T14:46:30.073 [INF] This is an info msg
2022-10-11T14:46:30.073 [WRN] This is a warn message
2022-10-11T14:46:30.073 [WRN] This is a warn msg
2022-10-11T14:46:30.073 [ERR] This is an error message
2022-10-11T14:46:30.073 [ERR] This is an error msg
*/
```