### Keep tool-using agents from skipping or spamming tools

Opus 4.8 improved tool triggering, with fewer cases of skipping a call the task needed. This system prompt guards the opposite failure *(calling tools it doesn't need)* and uses the interleaved thinking that adaptive mode turns on automatically:

```
Before each tool call, state in one sentence why you're calling it.After you get the result, decide whether you can answer now. If youcan, stop calling tools and respond. Don't call a tool you don't need.
```

**What it does:** adaptive thinking lets the model reason between tool calls, so the *"decide after the result"* instruction actually lands instead of being ignored. Cuts the loops where an agent keeps fetching things it already has.