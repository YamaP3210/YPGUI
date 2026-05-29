# Release

This folder stores the MVF release bundle source files and the generated zip.

## Structure

- `Files/`
  - the pre-zip release contents
- `MVF-0.1.0.zip`
  - the generated release archive

## Files Contents

- `MVF.0.1.0.nupkg`
  - MVF package
- `template-sample/`
  - sample application template based on the current MVFDemo structure
- `template-blank/`
  - minimal blank application template
- `AIData/CSharpCodingRules.txt`
  - TheGrayFox common optional C# coding rules
- `AIData/MVF/`
  - MVF usage guidance for AI-assisted application development

## Regeneration Rule

When asked to generate a release, rebuild this folder's `Files/` contents from
the current solution state and regenerate the versioned zip from that folder.
