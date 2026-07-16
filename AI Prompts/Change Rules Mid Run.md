### Change the rules mid-run with a system message

This is new in Opus 4.8 and almost nobody uses it. The Messages API now accepts a `role: "system"` entry partway through the conversation, so you can update instructions mid-task without restating the whole system prompt and without breaking the prompt cache on earlier turns. Inject something like this when conditions change:

```
{"role": "system", "content": "Update: budget is now tight. Prefer thesmallest correct fix. Stop after the failing test passes. Do notrefactor beyond what the test requires."}
```

**What it does:** changes permissions, budget, or context for the rest of the run while keeping your cached prefix intact. On a long agent loop, restating a 2,000-token system prompt every time you change one rule is pure waste; this avoids it.