# AIData

This folder stores project memory for AI-assisted development.

Before working on this solution, AI assistants must read all files in this
folder, especially the architecture handover and current coding rules.

The following files are the primary authority for this project:

- `Readme.md`
- `MVF_Architecture_FullHandover.txt`
- `CSharpCodingRules_Current.txt`

These three files must be understood before any other AIData file. If another
file in this folder conflicts with these primary files, these primary files take
precedence unless they are explicitly updated.

## Files

- `MVF_Architecture_FullHandover.txt`
  - Stores the MVF architectural background, design philosophy, responsibility boundaries, and current direction as a WPF-based Windows GUI application development framework-class library.
- `CSharpCodingRules_Current.txt`
  - Stores the current C# coding, naming, structure, and design rules for this solution.

## Maintenance Policy

- Keep these files updated when project philosophy, architecture, coding rules, or naming rules change.
- Prefer explicit updates over relying on chat history.
- Treat these files as the authoritative continuity context across AI sessions.
- Prioritize AIData over chat memory when deciding project direction, architecture, naming, and coding style.
- Treat the solution root `.editorconfig` as the Rider C# coding style baseline, but let the AIData coding rules override it when they differ.
- Reload the relevant AIData files frequently during development, especially before design decisions, naming decisions, structural changes, and code edits.
- Reflect important development decisions into the appropriate AIData file with high frequency so that AIData remains more authoritative than transient conversation context.
- Do not remove or rewrite established rules casually. Update them only when the project direction has clearly changed.
- When an important decision is made during development, update the appropriate file in this folder without waiting for a separate instruction.
