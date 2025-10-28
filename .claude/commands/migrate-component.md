# Migrate Single Component

**Component name**: {{args}}

**IMPORTANT**: You have zero memory of this project. Start by reading:

1. **MIGRATION_LOG.md** - Your memory from previous sessions (ALWAYS READ THIS FIRST)
2. **docs/SVELTE_TO_BLAZOR_GUIDE.md** - Find the section for {{args}} or similar component pattern
3. **docs/ARCHITECTURE_COMPARISON.md** - Component composition and state management sections
4. **CLAUDE.md** - Project overview

## Find Svelte Source

Search for the Svelte component in these locations:

1. **Settings components**: /mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/lib/{{args}}.svelte
2. **Route pages**: /mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/routes/
3. **Check types.ts**: /mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/lib/types.ts (for {{args}} type definition)

If you can't find the exact component, look for similar component patterns in SVELTE_TO_BLAZOR_GUIDE.md.

## Sub-Agent Guidance (Use Autonomously)

**Use Explore agent** (thoroughness: "medium") when:
- The Svelte component location is unclear
- You need to understand how {{args}} is used across multiple pages
- Finding all related files (props, types, usage examples)

**Do NOT use sub-agents** for:
- Direct component implementation
- Simple file reads when path is known
- Direct edits

## Analysis Steps

After finding the Svelte component, analyze:

1. **Props** (Svelte) → **Parameters** (Blazor):
   - List all props from Svelte component
   - Convert to C# properties with `[Parameter]` attribute
   - Determine appropriate C# types

2. **Snippets** (Svelte) → **RenderFragments** (Blazor):
   - Identify all `Snippet` props in Svelte
   - Convert to `RenderFragment?` properties
   - Named snippets → Named RenderFragments (e.g., `HeaderContent`, `FooterContent`)

3. **State Management**:
   - Identify Svelte state (`$state()`, `$derived()`)
   - Convert to private fields in `@code` block
   - Note: Blazor needs explicit `StateHasChanged()` calls (Svelte is automatic)

4. **Event Handlers**:
   - Identify Svelte events (on:click, custom events)
   - Convert to `EventCallback<T>` properties
   - Use `InvokeAsync()` for triggering events

5. **Tailwind Classes**:
   - Copy Tailwind classes from Svelte
   - **IMPORTANT**: Svelte uses Tailwind v4, Blazor uses v3.4.15
   - Check for any v4-specific classes that need v3 equivalents
   - Ensure `dark:` classes are present for dark mode

6. **Check MIGRATION_LOG.md**:
   - Look for similar components already migrated
   - Check Pattern Library for reusable patterns
   - Look for any learnings about similar components

## Determine Component Location

**Settings components** → `src/WebApp/Components/Settings/{{args}}.razor`
**Playground components** → `src/WebApp/Components/{{args}}.razor`
**Layout components** → `src/WebApp/Layout/{{args}}.razor`
**Pages** → `src/WebApp/Pages/{{args}}.razor`

## Implementation

Present your analysis to the user:

1. Show the Svelte component structure you found
2. Show your proposed Blazor component structure:
   - Parameters list
   - RenderFragments list
   - EventCallbacks list
   - Private fields for state
   - Methods needed
3. Explain any pattern choices or conversions
4. Ask for confirmation before creating the file

After user confirms, create the Blazor component.

## After Completion

**YOU MUST UPDATE MIGRATION_LOG.md** with:

1. Find the corresponding task in MIGRATION_LOG.md (search for {{args}})
2. Update task status to ✅ Complete
3. Add completion date
4. Document:
   - **Component**: {{args}}
   - **Location**: [file path]
   - **Svelte source**: [original file path]
   - **Parameters**: [list with types]
   - **RenderFragments**: [list]
   - **EventCallbacks**: [list]
   - **Service injections**: [if any]
   - **Patterns used**: [description]
   - **Issues encountered**: [if any]
   - **Testing notes**: [what user should verify]
5. If this pattern is reusable, add to Pattern Library section
6. Update Quick Status progress counter for appropriate phase

## Important Reminders

- Read MIGRATION_LOG.md FIRST
- Update MIGRATION_LOG.md IMMEDIATELY after completion
- SVELTE_TO_BLAZOR_GUIDE.md has complete working examples for Settings components
- Use `EditForm` + `DataAnnotationsValidator` for forms with validation
- Subscribe to service events in `OnInitialized()`, unsubscribe in `Dispose()`
- Call `StateHasChanged()` when responding to service events
- Tailwind v3.4.15 (not v4) - check class compatibility
- Ensure dark mode support with `dark:` classes
- Test responsive behavior (mobile, tablet, desktop)
