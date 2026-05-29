# AIData

This folder stores the persistent AI operating data for this solution family.

AIData is divided by role, not by convenience.
The important distinction is what is being developed.

- `Master`
  - authoritative data for developing MVF itself
  - not for release
- `MVF`
  - release-side AI guidance for developers who use MVF to build GUI applications
  - generated / extracted from `Master`
  - not authoritative
- `User`
  - user-specific local AI operating data
  - not for release
- `Solutions`
  - solution-specific objective local AI operating data
  - not for release

## Primary Authority

When developing MVF itself, the primary authority is:

- `Readme.md`
- `Master/Readme_Master.md`
- `Master/MVF_Architecture_FullHandover.txt`
- `Master/CSharpCodingRules_Current.txt`

If another AIData file conflicts with these files, these primary files take
precedence unless they are explicitly updated.

`MVF` is a generated / release-side view of the authoritative `Master`
information.
Do not treat files under `AIData/MVF` as the source of truth while maintaining
MVF itself.

## User Name Handling

When AIData is loaded for actual operation, the AI must first ask the current
user name.

Rules:

- if the answer is `TheGrayFox`, treat the current user as the MVF master
- otherwise, treat the current user as an MVF application developer
- user-specific local data belongs under `AIData/User/<UserName>`
- only the currently selected user's folder may be read
- never read multiple user folders at the same time

This is not authentication.
It is only the conversation-mode selector for AIData usage.

## Separation Principle

The MVF master and the MVF user are not two variations of the same developer.
They are different developers building different things.

- Master side:
  - develops MVF itself
  - must not drift into application planning
- User side:
  - develops GUI applications by using MVF
  - must not casually drift into modifying MVF internals

Keep these viewpoints separate in both AI reasoning and AIData maintenance.

## Local Data Structure

Recommended local operating structure:

- `AIData/User/<UserName>`
  - user-specific preferences, work notes, judgments, habits
- `AIData/Solutions/<SolutionName>`
  - objective information for a specific application / solution
  - screen structure, requirements, naming, implementation direction,
    unresolved items

`Solutions/<SolutionName>` should remain portable so that another user can take
the same solution folder and continue with the same application context.
Do not write user-dependent personal preferences there.

## Maintenance Policy

- Keep the `Master` files updated when MVF philosophy, architecture, coding rules, or naming rules change.
- Prefer explicit updates over relying on chat history.
- Treat AIData as the authoritative continuity context across AI sessions.
- Prioritize AIData over chat memory when deciding project direction, architecture, naming, and coding style.
- Treat the solution root `.editorconfig` as the Rider C# coding style baseline, but let the AIData coding rules override it when they differ.
- Treat UTF-8 as the standard text encoding for this solution and avoid mixing Shift-JIS / CP932 text files unless there is an explicit legacy requirement.
- Reload the relevant AIData files frequently during development, especially before design decisions, naming decisions, structural changes, and code edits.
- Reflect important development decisions into the appropriate AIData file with high frequency so that AIData remains more authoritative than transient conversation context.
- Do not remove or rewrite established rules casually. Update them only when the project direction has clearly changed.
- When an important decision is made during development, update the appropriate AIData file without waiting for a separate instruction.
- If information obtained through discussion is concrete and not ambiguous, reflect it into the appropriate AIData file by AI judgment without waiting for an explicit user instruction.
- Before any commit, review whether concrete and already-resolved discussion information remains unreflected in AIData, and if so, reflect it by AI judgment before committing.
- Do not record ambiguous, tentative, or still-unresolved discussion points as settled AIData rules. Keep AIData for decisions that are sufficiently clear to preserve as project continuity context.
