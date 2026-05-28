# AIData_User

This folder is the AI guidance area for third-party developers who build MVF
applications.

It is separate from the repository's own `AIData`.

`AIData` is the framework author's private continuity memory for MVF itself.
`AIData_User` is the distribution-side AI guidance area for people who develop
applications on top of MVF.

## Purpose

Use this folder when an AI assistant needs to understand:

- what MVF is
- how MVF applications should be structured
- how MVF should be used from an application project
- which responsibilities belong to MVF framework code and which belong to app code

This folder should be included in release-side template bundles.

## Update Policy

Files under `AIData_User` are intended to be split into two categories:

- files that are distributed and updated from MVF
- files that are local to the third-party developer's own application work

The distributed MVF-side files may be overwritten when a developer downloads a
newer MVF release package.

The local application-side files should be treated as project-local continuity
files and should not be overwritten by MVF updates.

## Recommended Separation

When expanding this folder for real distribution use, keep the contents
separated by role.

Recommended concept:

- MVF-side guidance files
  - overwritten when the developer updates MVF release assets
- local application files
  - maintained by the third-party developer for their own GUI application

## What MVF Is

MVF means Markup View Framework.

MVF is not an application.
MVF is a Windows GUI application framework-class library.

Current framework direction:

- WPF as the native window shell
- WebView2 as the client rendering surface
- HTML and CSS for client UI definition
- C# for runtime ownership, orchestration, and state
- JavaScript only as a DOM bridge and event-forwarding layer

MVF is currently Windows-only.
Cross-platform support is not a current design goal.

## Core Design Philosophy

MVF is designed for explicit runtime control.

Important characteristics:

- C# owns UI behavior
- updates are explicit
- runtime state is owned by C#
- DOM access is scoped and intentional
- behavior should remain predictable
- the mental model should feel closer to Unity or WinForms than to heavy WPF data binding

Avoid these assumptions unless the application explicitly chooses otherwise:

- DataBinding-centric ownership
- DependencyProperty-centric design
- DataContext-driven application structure
- framework-owned synchronization
- reactive ownership of UI state
- JS ownership of application state

## Layer Responsibilities

Use these boundaries consistently.

WPF / XAML:

- native window
- menu
- toolbar
- status bar
- WebView2 host placement
- host-side frame structure only

HTML:

- UI structure

CSS:

- layout
- theme
- visual design

C#:

- application logic
- state
- orchestration
- runtime UI control
- window-level configuration

JavaScript:

- DOM lookup
- DOM mutation
- event forwarding
- bridge utilities only

## Frame and Client Structure

Broad structure:

```text
Application Window
└─ MVFRoot.xaml
   ├─ frame-side menu / header area
   └─ MVFClientArea.xaml
      └─ MVFViewHost (WebView2)
         └─ MVFViewCanvas.html
```

`MVFViewCanvas` is the fixed root container inside the WebView2 client surface.
It is the parent placement area for top-level Widgets.

No HTML UI exists above `MVFViewCanvas`.

## Widget and Component Model

MVF uses a Node + Component style model.

- `MVFNode` represents a specific DOM node through a runtime handle
- `MVFComponent` is a C# runtime controller attached to one `MVFNode`
- one `MVFNode` may hold multiple Components
- the same concrete Component type should not be attached more than once to the same Node

Widgets are top-level UI units on the client side.

Current top-level Widget flow:

1. `MVFViewCanvas` is found as the placement root
2. a Widget-specific host node is created under it
3. the Widget HTML is inserted into that host
4. the Widget root node is found
5. the Widget Component is attached to that node

So Widget loading is:

- spawn
- find
- attach

Normal child Components usually follow:

- find
- attach

## DOM Identity Rules

MVF uses `data-ui-id` as the preferred MVF node identifier.

Important rules:

- HTML `id` is not the MVF node identity
- `data-ui-id` is the primary MVF lookup key
- `data-ui-name` may be used as a name-oriented key
- fallback to HTML `id` is allowed only as a secondary behavior
- not every HTML element needs a global MVF identifier
- smaller child elements should usually be found from the parent scope

Do not turn the full HTML tree into a global ID map unnecessarily.

## Application-Side Folder Meaning

For MVF application projects, this layout is important:

- `UI/View`
  - C# side UI controller classes
- `UI/HTML`
  - HTML, CSS, and JS assets
- `UI/HTML/Widgets`
  - Widget HTML files
- `UI/HTML/Components`
  - smaller reusable HTML fragments when needed
- `UI/HTML/CSS`
  - application-side CSS
- `UI/HTML/JS`
  - application-side JS

Everything outside that UI area should generally be treated as business-logic-side code.

## Naming Rules

For this solution family, the root identifier is `MVF`.

Use these rules:

- class names should follow `MVF + PascalCase`
- interface names should follow `IMVF + PascalCase`
- file names should normally match the main class name
- names should read naturally in English
- meaning is more important than mechanical naming

For local variables:

- use `camelCase`
- use `var` when the type is obvious from the right-hand side
- use an explicit type when it improves readability or intent

## Text Encoding

Use UTF-8 for text files.

Avoid mixing Shift-JIS / CP932 with UTF-8 across the same solution.

This is important for:

- Visual Studio
- Rider
- Git
- AI tooling

## Window-Level Control

Window-wide operations should be treated separately from client-side Widget logic.

Current direction:

- application code obtains an `MVFController` instance from `RunAsync(...)`
- the same instance can load modules and access a window-level configurator
- menu setup and other window-wide behavior belong to window-level control, not to Widget internals

## AI Usage Guidance

When AI assists MVF-based development, it should follow these behaviors:

- read the architecture and coding rules before editing
- preserve the WPF shell + WebView2 + HTML/CSS + C# ownership model
- avoid introducing automatic synchronization systems casually
- avoid moving application state ownership into JS
- keep XAML minimal and frame-oriented
- keep business logic outside UI asset folders
- prefer incremental, reviewable changes over monolithic generation

The assistant should act as:

- architectural reviewer
- implementation helper
- consistency checker

It should not behave as a framework generator that ignores the established MVF structure.
