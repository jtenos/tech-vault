### Prompts that backfire on Opus 4.8

**A few habits that quietly hurt you now:**

-   "Think deeply about everything" plus `max` effort. You'll get deliberation on every trivial step and a token bill to match. Steer thinking per turn instead.
-   **Asking for a thinking budget in prose** *("use 32k thinking tokens").* Meaningless on 4.8; budgets are gone. Use `effort` and adaptive thinking.
-   **Prompting for "more creative" by implying temperature.** There's no temperature to raise. Ask for the variety you want directly *("give me three distinct approaches, not variations on one").*
-   **Cramming twelve instructions into one prompt.** More instructions per prompt means more misreads. Split them across the system prompt and the per-turn message.

### Quick Answers

**How do I make Opus 4.8 think more or less?** Turn thinking on with `thinking: {type: "adaptive"}`, set the `effort` parameter (`low` through `max`), and nudge per turn with a line like "think carefully" or "answer directly."

**Can I set temperature on Opus 4.8?** No. `temperature`, `top_p`, and `top_k` return a 400 error. Control tone and determinism in the prompt instead.

**Is adaptive thinking on by default?** No. Thinking is off unless you set `thinking: {type: "adaptive"}`. It's the only thinking mode, and manual `budget_tokens` is rejected.

**How do I stop it claiming tests pass when they didn't?** Use the honesty prompt (Prompt 3). Opus 4.8 is much better at flagging this if you ask for it explicitly.

**Why are my thinking blocks empty?** On Opus 4.8, `thinking.display` defaults to `"omitted"`. Set `display: "summarized"` to read the reasoning. You're billed for the thinking tokens either way.

### What I'd still tweak

None of these are magic words. Steering on Opus 4.8 is sensitive to phrasing, so treat each prompt as a starting point and watch whether it actually changes behavior on your workload. If it doesn't, make it more direct. The honesty directive is the one I'd never ship without now.

The one-line *"just make it"* guardrail is the one I wish I'd written months ago. The rest I'm still wording and re-wording, which is the honest state of prompting any new model:

> *you keep a snippets file, and you keep editing it.*

If you've got a prompt that reliably beats one of these, I'd actually like to see it.