### Kick off a Dynamic Workflows migration in Claude Code

Dynamic Workflows *(Claude Code, on Team, Enterprise, and Max plans)* lets the model plan a large job and run hundreds of parallel subagents, checking its work against your test suite before it reports back. The kickoff prompt decides whether you get a clean result or a mess.

**Name the bar, the unit of work, and the ambiguity rule:**

```
Goal: migrate every call site from the `requests` library to `httpx`.The existing test suite is the bar: every test must pass when you'redone. Work module by module, keep each change behavior-identical, andopen one PR per module. If a call site is ambiguous, flag it and skipit instead of guessing.
```

**What it does:** gives the planner a verifiable success condition *(the tests)*, a parallelizable unit *(per module)*, and an explicit out for the cases it shouldn't force. This is the shape behind the public Bun port from Zig to Rust *(roughly 750,000 lines, 99.8% of tests passing, eleven days to merge)*, and it works because the goal was pinned to a test suite, not a vibe.