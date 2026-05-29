# AIData/User

This folder stores user-specific local AI operating data.

Each user gets a separate folder:

- `AIData/User/<UserName>`

Only the currently selected user's folder may be read during one operating
context.
Do not read multiple user folders at the same time.

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
