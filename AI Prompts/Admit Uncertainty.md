### Make Opus 4.8 admit uncertainty instead of bluffing

This is the prompt I reach for most, because it turns 4.8's headline improvement into something you can actually rely on.

```
If you are not confident a change is correct, stop and tell me exactlywhat you're unsure about. Do not guess. Never say a test or buildpassed unless you ran it and saw it pass. If you didn't verifysomething, say so plainly.
```

**What it does:** 4.8 already leans toward flagging uncertainty more than 4.7 did. This makes the expectation explicit and gives it permission to stop instead of inventing a clean ending. In an agent loop, this is the difference between *"all tests green" (it didn't run them)* and *"I didn't run the suite; here's the one I'm unsure about."*