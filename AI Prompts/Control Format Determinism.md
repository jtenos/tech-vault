### Control format and determinism without temperature

Since you can't reach for `temperature` anymore, determinism and terseness move into the prompt. Two I use constantly.

**For code-only, no chatter:**

```
Output only the final code block. No preamble, no explanation, noalternatives. If a caveat is essential, put it in one trailing commentinside the code.
```

**For tight, comparable options:**

```
Give exactly three options, ranked best first, one line each. Noheadings, no commentary.
```

**What it does:** these replace the "lower temperature for focused output" habit. On Opus 4.8 the model follows format constraints tightly, so spell out the shape you want instead of dialing a sampling parameter you no longer have.