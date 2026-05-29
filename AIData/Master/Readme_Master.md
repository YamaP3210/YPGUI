# AIData/Master

This folder contains the authoritative AI operating data for developing MVF
itself.

Read this folder when the current task is:

- designing MVF architecture
- updating MVF framework behavior
- changing MVF coding rules
- changing MVF release / maintenance policy

Do not use this folder as the main working context when the task is only to
develop an application that uses MVF.

## Primary Files

- `MVF_Architecture_FullHandover.txt`

Master-only AI work uses this folder together with:

- `AIData/CSharpCodingRules.txt`
- `AIData/MVF/MVFCodingRules.txt`

## Scope

This folder may contain:

- MVF design philosophy
- MVF architecture
- MVF internal responsibility boundaries
- MVF maintenance policy
- GitHub / release operation policy
- AI collaboration policy for MVF development

MVF-shared coding / naming rules that are also useful to MVF users should not
be duplicated here.
Place them under `AIData/MVF`.

This folder must not be simplified just because some information is not needed
by MVF users.

## Important Boundary

Master-side AI work must not drift into application design.

When operating from `Master`, the target is the MVF framework itself.
Do not think in terms of "what GUI application should be built" unless the user
explicitly returns to application-side work.

## Public Guidance Requirement

The MVF release-side guidance under `AIData/MVF` must remain self-contained for
MVF user-side operation.

That means public MVF guidance must explicitly describe the startup user-name
selection flow and the `AIData/User/<UserName>` handling model instead of
depending on `AIData/Master` knowledge.
