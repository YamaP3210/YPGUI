# AIData/User

This folder stores user-specific local AI operating data.

Each user gets a separate folder:

- `AIData/User/<UserName>`

Only the currently selected user's folder may be read during one operating
context.
Do not read multiple user folders at the same time.

When the AI has already read MVF user-side guidance and the selected user's
folder does not exist yet, the AI may create `AIData/User/<UserName>` and
initialize a minimal `Readme_User.md`.

## Purpose

Store information that belongs to a person rather than to MVF itself or to a
specific application.

Examples:

- working preferences
- decision tendencies
- personal notes
- local operating habits

## Current Master User

`TheGrayFox` is the MVF master user name.

If the selected user name is `TheGrayFox`, the AI should treat the current
conversation as MVF master-side work unless the task explicitly switches to
application-side operation.

## Minimum Initialization

When a new user folder is created, the minimum required initialization is:

- `AIData/User/<UserName>/Readme_User.md`

That file may initially contain only:

- the user name as the title
- one sentence stating that the folder stores local operating context for that
  user
