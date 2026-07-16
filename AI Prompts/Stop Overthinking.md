### Stop Opus 4.8 from overthinking simple turns

Drop this in your system prompt. It's the single biggest token-saver I use, because at the default `high` effort the model thinks on almost everything, including turns that don't need it.

```
Extended thinking costs latency and tokens. Use it only when a taskgenuinely needs multi-step reasoning. For lookups, single-file edits,renames, and formatting, answer directly without deliberating.
```

**What it does:** adaptive thinking decides per turn whether to reason first. This line pushes it to skip the deliberation on shallow work and save it for the hard stuff. Pair it with `effort: "medium"` for bimodal workloads (lots of easy turns, a few hard ones) and you stop paying for thinking you didn't need.

Caveat that bit me: this only matters if thinking is on at all. If you never set `thinking: {type: "adaptive"}`, the model isn't thinking regardless of what your prompt says.