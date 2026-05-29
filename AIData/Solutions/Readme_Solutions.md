# AIData/Solutions

This folder stores solution-specific objective AI operating data.

Each solution gets a separate folder:

- `AIData/Solutions/<SolutionName>`

## Purpose

Store information that belongs to the application / solution itself rather than
to a specific user.

Examples:

- screen structure
- requirements
- naming decisions
- implementation direction
- unresolved issues
- objective project notes

## Portability Rule

A solution folder should be portable.

If a solution folder is moved to another user's environment, that other user
should be able to continue with the same application context.

Do not place user-specific preferences or personal work habits here.

## Relationship To User Folders

Solution data and user data are different layers.

- `AIData/Solutions/<SolutionName>`
  - objective application / solution information
- `AIData/User/<UserName>`
  - person-specific local operating information

In one operating context, the AI may use:

- one selected user folder
- one or more relevant solution folders

But it must not read multiple user folders at the same time.
