#
# Prompt Builder - A Prompt Struct Tool

A lightweight Windows desktop app that helps everyday users turn a rough idea into a clear, well-structured AI prompt — and automatically strips out sensitive personal data before you paste it into ChatGPT, Claude, or any other AI tool.

## Why this exists

Most people write prompts like "help me with marketing" and get vague, unfocused answers back. Effective prompts follow a simple structure — a clear **task**, relevant **context**, a defined **output format**, and explicit **constraints**. This app walks you through building that structure step by step, so you don't need to know anything about "prompt engineering" to get reliable results.

At the same time, everyday conversations often contain things you shouldn't paste into a third-party AI chat: names, phone numbers, emails, bank account numbers, API keys, passwords, IP addresses. This app scans your draft, flags anything sensitive, and swaps it for safe, realistic-looking test data — so you can get help with a real situation without exposing real information.

## What it does

### 1. Structure your prompt
The app asks four guided questions:
- **Instruction** — what exactly do you want the AI to do?
- **Context** — what background does it need to know?
- **Output Format** — how should the answer be structured?
- **Constraints** — what should it avoid or follow strictly?

Along the way it offers word-choice suggestions, keyword emphasis, and a "too vague / too verbose / just right" guide, so your answers land in the sweet spot between ambiguous and rambling. Your answers are assembled into a clean, Markdown-formatted draft — the format AI models parse most reliably.

### 2. Check for sensitive information
Once your draft is ready, move to the next step and the app scans it for:

| Detected | Replaced with |
|---|---|
| Names | Alice, Bob, Charlie, Dana... |
| Emails | test@test.com |
| Phone numbers | 021 123 4567 (NZ format) |
| Bank account numbers | 12-3456-7890123-00 |
| IP addresses | 123.123.123.123 |
| Passwords | ******** |
| API keys | sk-test-XXXXXXXXXXXXXXXXXXXX |

Detected items are highlighted directly in the text. You can let the app replace everything automatically, or review and edit each item yourself. The same original value always maps to the same replacement, so "John" stays "Alice" everywhere it appears in the draft.

### 3. Edit freely
A built-in Find & Replace panel (Find Next, Find All, Replace, Replace All, case-sensitive toggle) lets you fine-tune the text before copying it out.

### 4. Copy and go
One click copies the finished, sanitized prompt to your clipboard, ready to paste anywhere.

## Getting started

**Requirements:** .NET 8 SDK, Windows

```bash
git clone https://github.com/<your-username>/prompt-struct-tool.git
cd prompt-struct-tool
dotnet run
```

## Tech stack

- C# / .NET 8 WinForms
- No external dependencies beyond the .NET base class library

## Status

Personal/daily-use tool — built to make prompt writing faster and safer for non-technical, everyday AI conversations. Contributions and suggestions welcome.

## License

MIT
