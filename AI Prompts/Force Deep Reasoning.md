### Force deep reasoning on a hard turn

The mirror image. When one turn in a session genuinely needs the model to slow down, append this to that user message rather than cranking global effort:

```
This needs multi-step reasoning. Think carefully before you answer,and check your result against the requirements before you respond.
```

**What it does:** per-message steering raises thinking on that turn only. It's cheaper and sharper than setting `effort: "xhigh"` for the whole conversation. Steering is sensitive to wording. If *"think carefully"* doesn't visibly change behavior, make it more direct *("Work through this step by step before answering").*