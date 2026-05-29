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
- `CSharpCodingRules.txt`
- `MVF/MVFCodingRules.txt`
- `Master/Readme_Master.md`
- `Master/MVF_Architecture_FullHandover.txt`

If another AIData file conflicts with these files, these primary files take
precedence unless they are explicitly updated.

`MVF` contains the release-side MVF usage view plus MVF-family shared rules.
Master-only maintenance information must remain under `Master`.

## User Name Handling

When AIData is loaded for actual operation, the AI must first ask the current
user name.

This is the operating-context selector for AIData.
It is not authentication.

Rules:

- if the answer is `TheGrayFox`, treat the current user as the MVF master
- otherwise, treat the current user as an MVF application developer
- user-specific local data belongs under `AIData/User/<UserName>`
- if `AIData/User/<UserName>` does not exist yet, the AI may create it and
  initialize a minimal `Readme_User.md`
- only the currently selected user's folder may be read
- never read multiple user folders at the same time

The minimum initialization file may be as small as:

- title line with the selected user name
- one sentence stating that the folder stores local operating context for that
  user

## Public MVF Package Requirement

`AIData/MVF` is the release-side AI guidance for MVF users.

That public guidance must be self-contained for MVF application development.
Therefore, even when only `AIData/MVF` is loaded, the AI must still be able to
understand that:

- it should ask the current user name at startup
- user-local information belongs under `AIData/User/<UserName>`
- it may create the user folder when missing
- it must read only the currently selected user's folder

Root `AIData` remains the full authoritative operating definition, but the
public MVF-side guidance must restate the user-side startup flow instead of
depending on the reader to also load this root file.

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
- Keep the root `CSharpCodingRules.txt` free from MVF-specific assumptions.
- Keep MVF-specific naming and structure rules in `AIData/MVF`.
- Keep the MVF release-side AI guidance self-contained enough that an AI can
  start user-side operation correctly from `AIData/MVF` alone.
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
